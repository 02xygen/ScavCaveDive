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
            AssetLoader.CacheAudioClip("caveDiver.respirator.exhale", respExhale);

            AudioClip gasp = AssetLoader.LoadEmbeddedAudio("Sounds.gasp1.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.gasp1", gasp);

            AudioClip drown = AssetLoader.LoadEmbeddedAudio("Sounds.drown1.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.drown1", drown);

            AudioClip drown2 = AssetLoader.LoadEmbeddedAudio("Sounds.drown2.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.drown2", drown2);

            AudioClip drown3 = AssetLoader.LoadEmbeddedAudio("Sounds.drown3.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.drown3", drown3);

            AudioClip cough1 = AssetLoader.LoadEmbeddedAudio("Sounds.cough1.mp3");
            AssetLoader.CacheAudioClip("caveDiver.player.cough1", cough1);
        }  
    }
}
