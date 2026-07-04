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
        public static bool inhale = true;
        public static float breatheTimer = 0f;

        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            if(__instance.inWater && __instance.hasScubaGear)
            {
                float rate = __instance.respiratoryRate;
                float breathInterval = (rate <= 0f ? 0f : 180f / rate);
                if (Time.time - breatheTimer > breathInterval)
                {
                    if(inhale)
                    {
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.respirator.inhale"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
                        Plugin.Logger.LogError($"Inhale");
                        inhale = false;
                    }

                    else if(!inhale)
                    {
                        Plugin.Logger.LogError($"Exhale");
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.respirator.exhale"), __instance.transform.position, true, true, null, 0.75f, 0.85f);
                        Bubbles.BubbleBurst(__instance.limbs[0].transform);
                        inhale = true;
                    }
                    
                    breatheTimer = Time.time;
                }

            }
        }
    }
}
