using BepInEx;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System;
using UnityEngine;

namespace CaveDiver
{
    public static class AspirationPatch
    {
        public static void manageMoodle(float val)
        {
            if (val > 75f)
            {
                MoodleRegistry.AddMoodle(
              3,
              AssetLoader.LoadEmbeddedSprite("Sprites.aspiration.png", 33.33f),
              "Aspiration",
              $"Liquid has been inhaled into the lungs, filling them completely. Breathing is impossible, you need fresh air NOW.",
              critical: true,
              chippedOnly: false,
              important: true,
              key: "aspiration");
            }

            else if (val > 50f)
            {
                MoodleRegistry.AddMoodle(
              3,
              AssetLoader.LoadEmbeddedSprite("Sprites.aspiration.png", 33.33f),
              "Aspiration",
              $"A large amount of liquid has been inhaled into the lungs. Breathing is nearly impossible.",
              critical: false,
              chippedOnly: false,
              important: true,
              key: "aspiration");
            }

            else if (val > 25f)
            {
                MoodleRegistry.AddMoodle(
              2,
              AssetLoader.LoadEmbeddedSprite("Sprites.aspiration.png", 33.33f),
              "Aspiration",
              $"A moderate amount of liquid has been inhaled into the lungs. Breathing is very difficult.",
              critical: false,
              chippedOnly: false,
              important: true,
              key: "aspiration");
            }

            else if (val > 0f)
            {
                MoodleRegistry.AddMoodle(
              1,
              AssetLoader.LoadEmbeddedSprite("Sprites.aspiration.png", 33.33f),
              "Aspiration",
              $"A small amount of liquid has been inhaled into the lungs. Breathing is difficult.",
              critical: false,
              chippedOnly: false,
              important: true,
              key: "aspiration");
            }

        }
    }
}
