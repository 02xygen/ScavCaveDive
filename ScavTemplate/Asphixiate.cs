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
    [HarmonyPatch(typeof(Body), "Update")]
    public static class Asphixiate
    {
        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            if(__instance.bloodOxygen <= 50f)
            {
               
            }
        }

    }
}
