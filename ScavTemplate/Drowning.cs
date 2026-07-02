using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using HarmonyLib;
using System.Drawing.Text;
using UnityEngine;

namespace CaveDiver
{
    [StatusOptions(Key = "com.O2xymoron.aspiration", SaveEnabled = true)]
    public sealed class AspirationStatus : BodyStatus
    {
        public float amount;
        public bool isOil;
        public bool aspirating = false;
        public float minBreathHoldTimer = 0f;
        public float minSurfaceTimer = 0f;
    }


    [HarmonyPatch(typeof(Body), "Update")]
    public static class Drowning
    {
        private static bool submergedLastFrame = false;
        [HarmonyPostfix]
        private static void Postfix(Body __instance)
        {
            AspirationStatus status = __instance.GetStatus<AspirationStatus>();
            
            if (__instance.alive)
            {
                if (__instance.bloodOxygen < 35f && __instance.inWater)
                {
                    status.amount = Aspirate(status, __instance);

                    if (FluidManager.main.WaterInfo(WorldGeneration.world.WorldToBlockPos(__instance.limbs[0].transform.position)).Item3 == 3)
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


                if(status.aspirating)
                {
                    __instance.limbs[1].pain = Mathf.Clamp(__instance.limbs[1].pain, 50f, 100f);
                    __instance.limbs[1].muscleHealth -= 2f * Time.deltaTime;
                    __instance.breathing = false;
                }

                AspirationPatch.manageMoodle(status.amount);

            }
        }


        // TODO: Add minimum time underwater before aspirating, maybe like 3 seconds or so.
        private static float Aspirate(AspirationStatus status, Body __instance)
        {
            if (status.aspirating == false)
            {
                status.aspirating = true;
                __instance.eyePanicTime += 1f;
            }

            status.amount += 150 * Time.deltaTime;
            status.amount = Mathf.Clamp(status.amount, 0f, 100f);

            return status.amount;

        }


        // TODO: Add minimum time above water before clearing lungs, maybe half a second or so.
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
