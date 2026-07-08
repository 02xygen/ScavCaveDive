using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System.Drawing.Text;
using UnityEngine;

namespace CaveDiver
{
    [HarmonyPatch(typeof(Body), "Update")]
    public static class Respirate
    {
        public static bool inhale = false;
        public static float breatheTimer = 0f;

        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            if (__instance.GetWearable("airtank") != null) __instance.hasScubaGear = true;
            if (__instance.GetWearable("rebreather") != null) __instance.hasScubaGear = true;

            if (__instance.inWater && __instance.hasScubaGear && __instance.breathing)
            {
                float rate = __instance.respiratoryRate;
                float breathInterval = (rate <= 0f ? 0f : 180f / rate);
                if (Time.time - breatheTimer > breathInterval)
                {
                    if(inhale)
                    {
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.regulator.inhale"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
                        inhale = false;
                    }

                    else if(!inhale && __instance.GetWearable("rebreather") == null)
                    {
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.regulator.exhale"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
                        Bubbles.BubbleBurst(__instance.limbs[0].transform);
                        inhale = true;
                    }

                    else if (!inhale && __instance.GetWearable("rebreather") != null)
                    {
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.rebreather.exhale"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
                        inhale = true;
                    }

                    breatheTimer = Time.time;
                }

            }
        }
    }
}
