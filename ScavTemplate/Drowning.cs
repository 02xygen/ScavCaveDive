using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System.Drawing.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Runtime.Remoting.Messaging;


namespace CaveDiver
{
    [StatusOptions(Key = "com.O2xymoron.aspiration", SaveEnabled = true)]
    public sealed class AspirationStatus : BodyStatus
    {
        public float amount = 0;
        public bool aspirating = false;
        public float minBreathHoldTimer = 0f;
        public float minSurfaceTimer = 0f;
        public Color liquidColor = Color.white;
    }


    [HarmonyPatch(typeof(Body), "Update")]
    public static class Drowning
    {
        static bool wasSubmerged = false;
        static bool canAspirate = false;
        static bool canCough = false;
        static float timeWhenSubmerged;
        static float timeWhenSurfaced;

        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            AspirationStatus status = __instance.GetStatus<AspirationStatus>();
            

            if (__instance.alive)
            {
                HandleBreathHoldTime(__instance);

                if (__instance.bloodOxygen < 45f && __instance.inWater)
                {
                    if(__instance.GetWearable("airtank") != null || __instance.GetWearable("rebreather") != null
                        || __instance.GetWearable("scubadivinggear") != null && status.amount >= 0
                        && __instance.conscious && __instance.limbs[1].muscleHealth > 5f) // MIGHT HAVE ISSUES
                    {
                        status.amount = CoughUpLiquid(status, __instance);
                    }

                    else if(canAspirate)
                    {
                        status.amount = Aspirate(status, __instance);
                    }
                    
                }

                else if (status.amount > 0 && __instance.conscious && __instance.limbs[1].muscleHealth > 5f && canCough) // Cough up water
                {
                    status.amount = CoughUpLiquid(status, __instance);
                }




                if (status.aspirating)
                {
                    __instance.limbs[1].pain = Mathf.Clamp(__instance.limbs[1].pain, 40f, 100f);
                    if(__instance.limbs[1].muscleHealth > 6) __instance.limbs[1].muscleHealth -= 3f * Time.deltaTime;
                    else __instance.limbs[1].muscleHealth -= 0.5f * Time.deltaTime;

                    __instance.breathing = false;
                }

                AspirationPatch.manageMoodle(status.amount);

                if(__instance.eyePanicTime > 0 || __instance.eyePanicTime > 0)
                {
                    __instance.eyeCloseTime = 0;
                }

            }
        }

        private static readonly Dictionary<Talker, int> lastVisibleCounts = new Dictionary<Talker, int>();

        [HarmonyPostfix]
        [HarmonyPatch(typeof (Talker), "Update")]

        private static void talkerPostFix(Talker __instance)
        {
            string text = __instance.text.text;
            if (__instance?.body == null) return;
            AspirationStatus status = __instance.body.GetStatus<AspirationStatus>();

            if (status.amount > 0)
            {
                __instance.Skip();
                __instance.text.text = "";
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                lastVisibleCounts[__instance] = 0;
                return;
            }

            int visableCount = text.Length;

            if (!lastVisibleCounts.TryGetValue(__instance, out int lastVisibleCount))
            {
                lastVisibleCount = 0;
            }

            if (visableCount < lastVisibleCount) lastVisibleCount = 0;

            for (int i = lastVisibleCount; i < visableCount; i++)
            {
                if(__instance.body.inWater && __instance.body.GetWearable("rebreather") == null)
                {
                    Bubbles.BubbleSingle(__instance.body.limbs[0].transform);
                }
                
            }

            lastVisibleCounts[__instance] = visableCount;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FacialExpression), "Update")]
        private static void EyeExpressionPostfix(FacialExpression __instance)
        {

            Body body = __instance.body;
            if (!body.inWater)
            {
                return;
            }

            if (__instance.body.eyeGone)
            {
                __instance.eyeTimeHealed += Time.deltaTime;
                if (__instance.body.isRight || __instance.body.bothEyesGone)
                {
                    __instance.eyes.sprite = ((__instance.eyeTimeHealed > 350f) ? __instance.eyesGoneHealed : __instance.eyesGone);
                }
                return;
            }

            if (body.eyePanicTime > 0f && body.conscious)
            {
                Eye eye = __instance.eyeList[4];
                __instance.eyes.sprite = eye.front;
                return;
            }

            if (body.eyeScareTime > 0f && body.conscious)
            {
                Eye eye = __instance.eyeList[3];
                __instance.eyes.sprite = eye.front;
            }
        }

       
        private static float Aspirate(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == false)
            {
                
                status.aspirating = true;
                Bubbles.BubbleBurst(__instance.limbs[0].transform);
                __instance.eyePanicTime = 1f;
                __instance.eyeScareTime = 2f;
                Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.drown" + Random.Range(1, 3).ToString()), __instance.transform.position, true, true, null, 6.0f, 1f, false, false);
                FluidManager.main.DrinkLiquid(WorldGeneration.world.WorldToBlockPos(__instance.transform.position), __instance);

                var cell = WorldGeneration.world.WorldToBlockPos(__instance.limbs[0].transform.position);
                byte liquid = FluidManager.main.GetLiquid(cell.x, cell.y);

                if (liquid == 1) status.liquidColor = new Color(0.28f, 0.4f, 0.76f, 1f); // Water has no assigned color, so we manually set it
                else status.liquidColor = FluidManager.main.LiquidColor(cell).WithAlpha(1.0f);
                Debug.Log(status.liquidColor);
                
            }

            status.amount += 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);


            return status.amount;

        }


        
        private static float CoughUpLiquid(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == true)
            {
                status.aspirating = false;
                GameObject particles = Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitParticle"), __instance.limbs[0].transform);
                BleedParticle bleedParticles = particles.GetComponent<BleedParticle>();
                ParticleSystem.MainModule main = particles.GetComponent<ParticleSystem>().main;
                main.startColor = status.liquidColor;
                bleedParticles.enabled = false;
                Object.Destroy(particles, 10f);
                Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.cough1"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
            }

            status.amount -= 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }

        private static void HandleBreathHoldTime(Body __instance)
        {
            float minBreathHoldTime = 5.0f;
            float minCoughTime = 0.25f;

            if (__instance.inWater && !Drowning.wasSubmerged)
            {
                Drowning.wasSubmerged = true;
                Drowning.timeWhenSubmerged = Time.time;
            }

            else if(!__instance.inWater && Drowning.wasSubmerged)
            {
                Drowning.wasSubmerged = false;
                Drowning.timeWhenSurfaced = Time.time;
                if (__instance.bloodOxygen <= 90 && __instance.GetStatus<AspirationStatus>().amount == 0) // Aspiration check not working?
                {
                    Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.gasp" + Random.Range(2, 4).ToString()), __instance.transform.position, true, false, null, 0.5f, 1f, false, false);
                }
            }

            if (Time.time - Drowning.timeWhenSubmerged > minBreathHoldTime) Drowning.canAspirate = true;
            else Drowning.canAspirate = false;
            
            if (Time.time - Drowning.timeWhenSurfaced > minCoughTime) Drowning.canCough = true;
            else Drowning.canCough = false;
            
        }
            
    }
}
