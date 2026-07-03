using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using UnityEngine;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using CUCoreLib.Data;


namespace CaveDiver
{
    class SoundManager : MonoBehaviour
    {
        public void LoadAudio()
        {
            AudioClip respInhale = AssetLoader.LoadEmbeddedAudio("Sounds.respInhale.mp3");
            AssetLoader.CacheAudioClip("caveDiver.respirator.inhale", respInhale);

            AudioClip respExhale = AssetLoader.LoadEmbeddedAudio("Sounds.respExhale.mp3");
            AssetLoader.CacheAudioClip("caveDiver.respirator.exhale", respInhale);

            AudioClip gasp = AssetLoader.LoadEmbeddedAudio("Sounds.gasp1.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.gasp", gasp);
        }  
    }
}
