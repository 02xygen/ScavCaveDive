using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using UnityEngine;

namespace CaveDiver
{
    [StatusOptions(Key = "com.O2xymoron.aspiration", SaveEnabled = true)]
    public sealed class AspirationStatus : BodyStatus
    {
        public float amount;
        public bool startedAspirating = false;
        public bool startedCoughing = false;
    }


    [HarmonyPatch(typeof(Body), "Update")]
    public static class Drowning
    {
       

        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            AspirationStatus status = __instance.GetStatus<AspirationStatus>();
            if(__instance.alive)
            {
                if (__instance.bloodOxygen < 30f && __instance.inWater)
                {
                    if (status.amount <= 0)
                    {
                        //Object.Destroy(Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitBloodParticle"), __instance.limbs[0].transform), 10f);
                    }

                    status.amount += 200 * Time.deltaTime;
                    status.amount = Mathf.Clamp(status.amount, 0f, 100f);
                }

                else if (status.amount > 0) // Cough up water
                {
                    status.amount -= 100 * Time.deltaTime;
                    status.amount = Mathf.Clamp(status.amount, 0f, 100f);
                }

                if (status.amount > 75f)
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

                else if (status.amount > 50f)
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

                else if (status.amount > 25f)
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

                else if (status.amount > 0f)
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
}
