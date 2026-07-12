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
using UnityEngine.UI;

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

        [HarmonyPatch(typeof(Body), "HandleCirculation")] // For any scuba reliant functions we need a prefix that checks current
                                                          // equipped items and sets hasScubaGear to either true or false;
        public static class RespirationPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Body __instance)
            {
                    if (__instance.GetWearable("airtank") != null) __instance.hasScubaGear = true;
                    if (__instance.GetWearable("rebreather") != null) __instance.hasScubaGear = true;
                    if (__instance.GetWearable("scubadivinggear") != null) __instance.hasScubaGear = true;
            }
        }

        [HarmonyPatch(typeof(Body), "FixedUpdate")]
        public static class FinPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Body __instance)
            {

            }
        }


        [HarmonyPatch(typeof(Talker), "Update")]
        public static class TalkerPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Talker __instance)
            {
               if (__instance?.trader) return;
               if (__instance?.body?.GetWearable("airtank") != null) __instance.body.hasScubaGear = true;
               if (__instance?.body?.GetWearable("rebreather") != null) __instance.body.hasScubaGear = true;
               if (__instance?.body?.GetWearable("scubadivinggear") != null) __instance.body.hasScubaGear = true;
            }
        }


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
            private static bool Prefix(WorldGeneration __instance) // Maybe make a "super flooded modifier, though lumalgae spawns will have to be removed as they can cause impossible scenarios."
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

        [HarmonyPatch(typeof(WoundView), "Awake")]
        public static class AspirationFillBuilderPatch
        {
            [HarmonyPostfix]
            private static void Postfix(WoundView __instance)
            {
                FilledImagePP aspirationFill = UnityEngine.Object.Instantiate(__instance.hemothoraxFill);
                aspirationFill.name = "AspirationFill";
                aspirationFill.transform.SetParent(GameObject.Find("Main Camera/Canvas/WoundView/StatMenu").transform, false);
                aspirationFill.GetComponent<Image>().sprite = AssetLoader.LoadEmbeddedSprite("aspirationFill.png");
                aspirationFill.fillAmount = 1.0f;
            }
        }

        [HarmonyPatch(typeof(WoundView), "UpdateView")]
        public static class AspirationFillPatch
        {
            [HarmonyPostfix]
            private static void Postfix(WoundView __instance)
            {
                AspirationStatus status = __instance.body.GetStatus<AspirationStatus>();
                GameObject.Find("Main Camera/Canvas/WoundView/StatMenu/AspirationFill").GetComponent<FilledImagePP>().fillAmount = status.amount / 100f;
                GameObject.Find("Main Camera/Canvas/WoundView/StatMenu/AspirationFill").GetComponent<FilledImagePP>().GetComponent<Image>().color = status.liquidColor;
            }
        }

        [HarmonyPatch(typeof(LifepodPump), "Awake")]
        public static class LifepodPumpPatch
        {
            [HarmonyPostfix]
            private static void Postfix(LifepodPump __instance)
            {
                __instance.gameObject.AddComponent<RefillAirTank>();
            }
        }
    }
}
