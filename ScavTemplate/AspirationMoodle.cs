using BepInEx;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System;
using UnityEngine;

namespace CaveDiver
{
    [HarmonyPatch(typeof(Body), "Update")]
    public static class AspirationPatch
    {
        [HarmonyPostfix]
        private static void Postfix(Body __instance) 
        {
     
            
        }
    }
}
