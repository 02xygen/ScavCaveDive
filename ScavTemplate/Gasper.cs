using CaveDiver;
using CUCoreLib.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CaveDiver
{
    [HarmonyPatch(typeof(Body), "Update")]
    public static class Gasper
    {
        private static bool wasSumberged = false;

        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            if(__instance.inWater && !wasSumberged) wasSumberged = true;

            else if(!__instance.inWater && wasSumberged)
            {
                if(__instance.bloodOxygen <= 70 && __instance.GetStatus<AspirationStatus>().amount == 0)
                {
                    Plugin.Logger.LogError($"GASP");
                    Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.player.gasp"), __instance.transform.position, false, true, null, 1f, 1f, false, false);
                }
                wasSumberged = false;
            }
        }

    }
}
