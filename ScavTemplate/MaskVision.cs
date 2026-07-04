using CaveDiver;
using CUCoreLib.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{
    class MaskVision
    {
        [HarmonyPatch(typeof(PlayerCamera), "HandleScreenShaders")] // Not working

        [HarmonyPostfix]
        private static void Postfix(PlayerCamera __instance)
        {
            __instance.waterSmooth = Mathf.Lerp(__instance.waterSmooth, __instance.body.inWater && !__instance.body.hasScubaGear ? 0.75f : 0f, Time.deltaTime * 4f);
        }
       
    }
}
