using System;
using System.Collections.Generic;
using CaveDiver;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using CUCoreLib.Data;
using CUCoreLib;
using HarmonyLib;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CaveDiver
{
    public static class ScubaManager
    {
        public static bool hasAirTank;
        public static bool hasRebreather;
        public static bool hasMakshiftFins;
        public static bool hasFins;

        [HarmonyPatch(typeof(Limb), "ImpactDamage")]
        public static class ImpactPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Limb __instance, ref float force)
            {
                if (__instance.isHead && force > __instance.body.jumpSpeed * 1.66f)
                {
                    if (__instance.body.GetWearable("divingmask") != null) __instance.body.DropWearable(__instance.body.GetWearable("divingmask"));
                    if (__instance.body.GetWearable("swimgoggles") != null) __instance.body.DropWearable(__instance.body.GetWearable("swimgoggles"));

                    if (__instance.body.GetWearable("airtank") != null)
                    {
                        ItemRegistry.SetCustomData(__instance.body.GetWearable("airtank"), "RegInMouth", false);
                        CUCoreUtils.setMultiWornSprite(__instance.body.GetWearable("airtank"), "Head", AssetLoader.LoadEmbeddedSprite("dummy.png"));
                    }

                    if (__instance.body.GetWearable("rebreather") != null)
                    {
                        ItemRegistry.SetCustomData(__instance.body.GetWearable("rebreather"), "RegInMouth", false);
                        CUCoreUtils.setMultiWornSprite(__instance.body.GetWearable("rebreather"), "Head", AssetLoader.LoadEmbeddedSprite("dummy.png"));
                    }
                    
                } 
            }
        }

        [HarmonyPatch(typeof(Body), "HandleCirculation")] // For any scuba reliant functions we need a prefix that checks current
                                                          // equipped items and sets hasScubaGear to either true or false;
        public static class RespirationPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Body __instance)
            {
                if (__instance.GetWearable("airtank") != null && __instance.GetWearable("airtank").condition > 0f)
                {
                    Item item = __instance.GetWearable("airtank");
                    float num = item.decayMultiplier * WorldGeneration.globalDecayRate;

                    ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        if (!__instance.disfigured)
                        {
                            __instance.hasScubaGear = true;
                            
                            if(__instance.breathing && __instance.inWater)
                            {
                                item.condition -= item.Stats.rotSpeed * num * Time.deltaTime * 0.01f;
                            }
                        } 

                        else
                        {
                            ItemRegistry.SetCustomData(item, "RegInMouth", false);
                            CUCoreUtils.setMultiWornSprite(item, "Head", AssetLoader.LoadEmbeddedSprite("dummy.png"));
                        }

                    }
                    item.condition = Mathf.Clamp(item.condition, 0f, 1f);
                   
                }

                else if (__instance.GetWearable("rebreather") != null && __instance.GetWearable("rebreather").condition > 0f)
                {

                    Item item = __instance.GetWearable("rebreather");
                    float num = item.decayMultiplier * WorldGeneration.globalDecayRate;

                    ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        if (!__instance.disfigured)
                        {
                            __instance.hasScubaGear = true;

                            if (__instance.breathing && __instance.inWater)
                            {
                                item.condition -= item.Stats.rotSpeed * num * Time.deltaTime * 0.01f;
                            }
                        } 

                        else
                        {
                            ItemRegistry.SetCustomData(item, "RegInMouth", false);
                            CUCoreUtils.setMultiWornSprite(item, "Head", AssetLoader.LoadEmbeddedSprite("dummy.png"));
                        }
                    }
                    item.condition = Mathf.Clamp(item.condition, 0f, 1f);
                }

                else __instance.hasScubaGear = false;
            }
        }

        [HarmonyPatch(typeof(PlayerCamera), "HandleWorldUI")]
        public static class DrinkUIPatch
        {
            [HarmonyPrefix]
            private static void Prefix(PlayerCamera __instance)
            {
                if (__instance.body.GetWearable("bcd") != null)
                {
                    __instance.body.liquidDrinkTime = 0f;
                }
            }
        }

        [HarmonyPatch(typeof(InvButton), "UpdateGraphic")]
        public static class RegulatorUIPatch
        {
            [HarmonyPostfix]
            private static void Postfix(InvButton __instance)
            {
                if(__instance.slot == 2) // Checks if it is the head slot
                {
                    if(__instance?.body.GetWearable("airtank") != null)
                    {
                        Item item = __instance.body.GetWearable("airtank");
                        ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn);
                        if (regIn)
                        {
                            float alpha = __instance.image.color.a;
                            __instance.image.color = Color.red.WithAlpha(alpha);
                        }
                    }

                    if(__instance?.body.GetWearable("rebreather") != null)
                    {
                        Item item = __instance.body.GetWearable("rebreather");
                        ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn);
                        if (regIn)
                        {
                            float alpha = __instance.image.color.a;
                            __instance.image.color = Color.red.WithAlpha(alpha);
                        }
                    }
                }
            }
        }


        [HarmonyPatch(typeof(Body), "FixedUpdate")]
        public static class WaterMovementPatch
        {
            [HarmonyPostfix]
            private static void Postfix(Body __instance)
            {
                var cell = WorldGeneration.world.WorldToBlockPos(new Vector2(PlayerCamera.main.body.limbs[2].transform.position.x, PlayerCamera.main.body.limbs[2].transform.position.y));
                byte liquid = FluidManager.main.GetLiquid(cell.x, cell.y);


                if (liquid != 0)
                {
                    if (__instance.GetWearable("makeshiftfins") != null) __instance.rb.AddForce(new Vector2(__instance.moveDir.x, 0f) * 1000f);

                    if (__instance.GetWearable("bcd") != null)
                    {
                        ValueTuple<float, float, int> valueTuple = FluidManager.main.WaterInfo(cell);
                        if(__instance.standing) __instance.rb.gravityScale = Mathf.Lerp(0.54f, 2.7f, Mathf.InverseLerp(0.6f, 3f, valueTuple.Item1));
                        if (!__instance.standing)
                        {
                            foreach(Limb limb in __instance.limbs)
                            {
                                limb.rb.gravityScale = Mathf.Lerp(0.54f, 2.7f, Mathf.InverseLerp(0.6f, 3f, valueTuple.Item1));
                            }
                        }
                       


                        if (__instance.rb.velocity.y < __instance.actualMaxSpeed && __instance.moveDir.y > 0f)
                        {
                            __instance.rb.AddForce(Vector2.up * __instance.actualMoveForce);
                        }
                        if (__instance.rb.velocity.y > -__instance.actualMaxSpeed && __instance.moveDir.y < 0f)
                        {
                            __instance.rb.AddForce(Vector2.down * __instance.actualMoveForce);
                            __instance.liquidDrinkTime = 0f;
                        }
                    } 
                    else __instance.rb.gravityScale = 1f;
                }
            }
        }

        [HarmonyPatch(typeof(Talker), "Update")]
        public static class TalkerPatch
        {
            [HarmonyPrefix]
            private static void Prefix(Talker __instance)
            {
                if (__instance?.trader) return;
                if (__instance?.body?.GetWearable("airtank") != null && __instance?.body?.GetWearable("airtank").condition > 0f)
                {
                    ItemRegistry.TryGetCustomData<bool>(__instance?.body?.GetWearable("airtank"), "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        __instance.body.hasScubaGear = true;
                    }
                }

                else if (__instance?.body?.GetWearable("rebreather") != null && __instance?.body?.GetWearable("rebreather").condition > 0f)
                {
                    ItemRegistry.TryGetCustomData<bool>(__instance.body.GetWearable("rebreather"), "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        __instance.body.hasScubaGear = true;
                    }
                }

                else if(__instance?.body) __instance.body.hasScubaGear = false;
            }
        }

        [HarmonyPatch(typeof(Talker), "impairedSpeech", MethodType.Getter)]
        public static class RegulatorSpeechPatch
        {
            static void Postfix(Talker __instance, ref bool __result)
            {
                if (__instance?.body?.GetWearable("airtank") != null)
                {
                    ItemRegistry.TryGetCustomData<bool>(__instance?.body?.GetWearable("airtank"), "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        __result = true;
                    }
                }

                if (__instance?.body?.GetWearable("rebreather") != null)
                {
                    ItemRegistry.TryGetCustomData<bool>(__instance.body.GetWearable("rebreather"), "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        __result = true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerCamera), "HandleScreenShaders")]
        public static class BlurPatch
        {
            [HarmonyPostfix]
            private static void Postfix(PlayerCamera __instance)
            {
                if (__instance.body.GetWearable("divingmask") != null)
                {
                    Item item = __instance.body.GetWearable("divingmask");
                    ItemRegistry.TryGetCustomData<float>(item, "waterlogAmount", out float amount);
                    __instance.waterSmooth = Mathf.Clamp(amount, 0.05f, 1f);
                }
                
                if (__instance.body.GetWearable("swimgoggles") != null)
                {
                    Item item = __instance.body.GetWearable("swimgoggles");
                    ItemRegistry.TryGetCustomData<float>(item, "waterlogAmount", out float amount);
                    __instance.waterSmooth = Mathf.Clamp(amount, 0.05f, 1f);
                }

                __instance.blurPass.SetFloat("_BlurIntensity", Mathf.Clamp01(1f - __instance.body.consciousness * 0.01f) * 1.5f + (__instance.body.bothEyesGone ? 2f : 0f) + __instance.waterSmooth * 2f);
            }
        }

        [HarmonyPatch(typeof(WorldGeneration), "ApplyLayerModifiers")]
        public static class WgFloodPatch
        {
            [HarmonyPrefix]
            private static bool Prefix(WorldGeneration __instance) // Maybe make a "super flooded modifier, though lumalgae spawns will have to be removed as they can cause impossible scenarios."
            {
                Recipes.recipes[70].INT = 40; // Sets ASG recipe int requirement to 40

                if (PlayerPrefs.GetInt("ForceFlood_Enabled", 1) == 0) return true;
               
                for (int i = 0; i < __instance.width; i++)
                {
                    Vector2Int pos = new Vector2Int(i, -461);
                    __instance.SetBlockNoUpdate(pos, 12);
                }

                for (int i = 1; i < Mathf.Clamp(PlayerPrefs.GetInt("ForceFlood_Strength", 2) + (WorldGeneration.world.biomeDepth) * PlayerPrefs.GetInt("FloodRamp_Enabled", 0), 1, 10); i++)
                {
                    var flooded = LayerModifier.availableModifiers[5];
                    flooded.Initialize(__instance);
                    flooded.active = true;
                }

                __instance.layerPrefix = Locale.GetOther("layermodifier5");
                __instance.layerDescription = Locale.GetOther("layermodifier5dsc");
                return false;
            }
        }

        [HarmonyPatch(typeof(WoundView), "Awake")]
        public static class AspirationFillBuilderPatch
        {
            [HarmonyPostfix]
            private static void Postfix(WoundView __instance)
            {
                FilledImagePP aspirationFill = UnityEngine.Object.Instantiate(__instance.hemothoraxFill);
                aspirationFill.name = "AspirationFill";
                aspirationFill.transform.SetParent(GameObject.Find("Main Camera/Canvas/WoundView/StatMenu").transform, false);
                aspirationFill.GetComponent<Image>().sprite = AssetLoader.LoadEmbeddedSprite("aspirationFill.png");
                aspirationFill.fillAmount = 1.0f;
            }
        }

        [HarmonyPatch(typeof(WoundView), "UpdateView")]
        public static class AspirationFillPatch
        {
            [HarmonyPostfix]
            private static void Postfix(WoundView __instance)
            {
                AspirationStatus status = __instance.body.GetStatus<AspirationStatus>();
                GameObject.Find("Main Camera/Canvas/WoundView/StatMenu/AspirationFill").GetComponent<FilledImagePP>().fillAmount = status.amount / 100f;
                GameObject.Find("Main Camera/Canvas/WoundView/StatMenu/AspirationFill").GetComponent<FilledImagePP>().GetComponent<Image>().color = status.liquidColor;
            }
        }

        [HarmonyPatch(typeof(LifepodPump), "Awake")]
        public static class LifepodPumpPatch
        {
            [HarmonyPostfix]
            private static void Postfix(LifepodPump __instance)
            {
                __instance.gameObject.AddComponent<RefillAirTank>();
            }
        }

        [HarmonyPatch(typeof(LiquidAffect), "FixedUpdate")]
        public static class LiquidDragPatch
        {
            [HarmonyPrefix]
            private static void Prefix(LiquidAffect __instance)
            {
                if (__instance?.body?.GetWearable("fins")) __instance.body.hasScubaGear = true;
                else if (__instance.body) __instance.body.hasScubaGear = false;
            }
        }


        [HarmonyPatch(typeof(Item), "SetupItems")]
        public static class ASGSpawnPatch
        {
            [HarmonyPostfix]
            private static void Postfix()
            {
                Item.GlobalItems.TryGetValue("scubadivinggear", out ItemInfo asg);
                //Debug.Log(Item.GlobalItems.Count);
                //if (asg != null) Debug.Log("Found ASG");
                asg.decayMinutes = 0;
                asg.category = "unused";
            }
        }
    }
}
