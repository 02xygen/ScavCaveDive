using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace CaveDiver
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "com.O2xymoron.CaveDiver";
        public const string ModName = "Cave Diver";
        public const string ModVersion = "0.0.2";

        internal static new ManualLogSource Logger;
        private readonly Harmony _harmony = new(ModGUID);
        public static Plugin Instance { get; private set; } = null!;

        void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            _harmony.PatchAll();
            SoundManager sm = gameObject.AddComponent<SoundManager>();
            sm.LoadAudio();
            RegisterItems ri = gameObject.AddComponent<RegisterItems>();
       
            Logger.LogInfo($"Plugin {ModName} is loaded!");
        }

           
        
        void Update()
        {

        }

        void OnDestroy()
        {
            _harmony?.UnpatchSelf();
            Instance = null!;
        }
    }
}
