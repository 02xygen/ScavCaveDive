using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System.Drawing.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace CaveDiver
{
    [StatusOptions(Key = "com.O2xymoron.aspiration", SaveEnabled = true)]
    public sealed class AspirationStatus : BodyStatus
    {
        public float amount = 0;
        public bool isOil;
        public bool aspirating = false;
        public float minBreathHoldTimer = 0f;
        public float minSurfaceTimer = 0f;
    }


    [HarmonyPatch(typeof(Body), "Update")]
    public static class Drowning
    {
        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            AspirationStatus status = __instance.GetStatus<AspirationStatus>();

            if (__instance.alive)
            {
                if (__instance.bloodOxygen < 40f && __instance.inWater)
                {
                    if(__instance.hasScubaGear && status.amount >= 0) // MIGHT HAVE ISSUES
                    {
                        status.amount = CoughUpLiquid(status, __instance);
                    }

                    else
                    {
                        status.amount = Aspirate(status, __instance);

                        if (FluidManager.main.WaterInfo(WorldGeneration.world.WorldToBlockPos(__instance.limbs[0].transform.position)).Item3 == 3)
                        {
                            status.isOil = true;
                            Plugin.Logger.LogError($"In Oil");
                        }
                        else status.isOil = false;
                    }
                    
                }

                else if (status.amount > 0 && __instance.conscious && __instance.limbs[1].muscleHealth > 5f) // Cough up water
                {
                    status.amount = CoughUpLiquid(status, __instance);
                }




                if (status.aspirating)
                {
                    __instance.limbs[1].pain = Mathf.Clamp(__instance.limbs[1].pain, 50f, 100f);
                    __instance.limbs[1].muscleHealth -= 2f * Time.deltaTime;
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
                if(__instance.body.inWater)
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

        // TODO: Add minimum time underwater before aspirating, maybe like 3 seconds or so.
        private static float Aspirate(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == false)
            {
                status.aspirating = true;
                Bubbles.BubbleBurst(__instance.limbs[0].transform);
                __instance.eyePanicTime = 1f;
                __instance.eyeScareTime = 2f;
                Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.drown" + Random.Range(1, 3).ToString()), __instance.transform.position, true, true, null, 1.5f, 1f, false, false);
            }

            status.amount += 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }


        // TODO: Add minimum time above water before clearing lungs, maybe half a second or so.
        private static float CoughUpLiquid(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == true)
            {
                status.aspirating = false;
                if(status.isOil)
                {
                    Object.Destroy(Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitParticle"), __instance.limbs[0].transform), 10f);
                }
                else Object.Destroy(Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitBloodParticle"), __instance.limbs[0].transform), 10f);
                Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.cough1"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
            }

            status.amount -= 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }
            
    }
}
