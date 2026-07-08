using System;
using System.Collections.Generic;
using CaveDiver;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using CUCoreLib.Data;
using CUCoreLib;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{
    public static class ScubaManager
    {
        public static bool regInMouth;
        public static bool hasAirTank;
        public static bool hasRebreather;
        public static bool hasMakshiftFins;
        public static bool hasFins;

        [HarmonyPatch(typeof(Limb), "ImpactDamage")]
        public static class ImpactPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Limb __instance, ref float force)
            {
                if (__instance.isHead && force > __instance.body.jumpSpeed * 1.66f)
                {
                    if (__instance.body.GetWearable("divingmask") != null) __instance.body.DropWearable(__instance.body.GetWearable("divingmask"));
                    if (__instance.body.GetWearable("swimgoggles") != null) __instance.body.DropWearable(__instance.body.GetWearable("swimgoggles"));
                } 
            }
        }

        [HarmonyPatch(typeof(Body), "HandleCirculation")]
        public static class RespirationPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Body __instance)
            {
                    if (__instance.GetWearable("airtank") != null) __instance.hasScubaGear = true;
                    if (__instance.GetWearable("rebreather") != null) __instance.hasScubaGear = true;
                    if (__instance.GetWearable("scubadivinggear") != null) __instance.hasScubaGear = true;
            }

            [HarmonyPostfix]
            private static void Postfix(Body __instance)
            {
                __instance.hasScubaGear = false;
            }
        }


        //[HarmonyPatch(typeof(Talker), "Update")]
       // public static class TalkerPatch // Causes null reference error
       // {
       //     [HarmonyPrefix]
       //     private static void Prefix(Talker __instance)
        //    {
        //        if (__instance.body.GetWearable("airtank") != null) __instance.body.hasScubaGear = true;
       //         if (__instance.body.GetWearable("rebreather") != null) __instance.body.hasScubaGear = true;
       //         if (__instance.body.GetWearable("scubadivinggear") != null) __instance.body.hasScubaGear = true;
       //     }

        //    [HarmonyPostfix]
        //    private static void Postfix(Talker __instance)
        //    {
        //        __instance.body.hasScubaGear = false;
       //     }
      // }


        [HarmonyPatch(typeof(PlayerCamera), "HandleScreenShaders")]
        public static class BlurPatch
        {
            [HarmonyPostfix]
            private static void Postfix(PlayerCamera __instance)
            {
                if (__instance.body.GetWearable("divingmask") != null) __instance.waterSmooth = 0.05f;
                if (__instance.body.GetWearable("swimgoggles") != null) __instance.waterSmooth = 0.05f;

                __instance.blurPass.SetFloat("_BlurIntensity", Mathf.Clamp01(1f - __instance.body.consciousness * 0.01f) * 1.5f + (__instance.body.bothEyesGone ? 2f : 0f) + __instance.waterSmooth * 2f);
            }
        }

        [HarmonyPatch(typeof(WorldGeneration), "ApplyLayerModifiers")]
        public static class WgFloodPatch
        {
            [HarmonyPrefix]
            private static bool Prefix(WorldGeneration __instance) // MAYBE WORKING???
                                                                   // ALSO NEED TO DISABLE LUMIAGLAE IT CAN CREATE UNWINNABLE SCENARIOS.
            {
                var flooded = LayerModifier.availableModifiers[5];
                var flooded2 = LayerModifier.availableModifiers[5];
                flooded.Initialize(__instance);
                flooded2.Initialize(__instance);
                flooded.active = true;
                flooded2.active = true;
                __instance.layerPrefix = Locale.GetOther("layermodifier5");
                __instance.layerDescription = Locale.GetOther("layermodifier5dsc");
                return false;
            }
        }
    }

   
}
