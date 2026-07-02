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
        public bool isOil;
        public bool aspirating = false;
    }


    [HarmonyPatch(typeof(Body), "Update")]
    public static class Drowning
    {
        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            AspirationStatus status = __instance.GetStatus<AspirationStatus>();
            if (__instance.alive)
            {
                if (__instance.bloodOxygen < 35f && __instance.inWater)
                {
                    status.amount = Aspirate(status, __instance);

                    if (FluidManager.main.WaterInfo(WorldGeneration.world.WorldToBlockPos(__instance.limbs[0].transform.position)).Item3 > 0)
                    {
                        status.isOil = true;
                        Plugin.Logger.LogError($"In Oil");
                    }
                       
                    else status.isOil = false;
                }

                else if (status.amount > 0) // Cough up water
                {
                    status.amount = CoughUpLiquid(status, __instance);
                }

                AspirationPatch.manageMoodle(status.amount);

            }
        }

        private static float Aspirate(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == false)
            {
                status.aspirating = true;
            }

            status.amount += 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }

        private static float CoughUpLiquid(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == true)
            {
                status.aspirating = false;
                if(status.isOil)
                {
                    Object.Destroy(Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitParticle"), __instance.limbs[0].transform), 10f);
                }
                else Object.Destroy(Object.Instantiate<GameObject>(Resources.Load<GameObject>("vomitBloodParticle"), __instance.limbs[0].transform), 10f);
            }

            status.amount -= 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }
            
    }
}
