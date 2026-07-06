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
    }

   
}
