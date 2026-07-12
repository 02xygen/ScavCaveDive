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
using CUCoreLib.ContentReload;



namespace CaveDiver
{
    class RegisterRecipes : MonoBehaviour
    {
        void Awake()
        {
            try { ContentReloadManager.EnableHotReload("com.O2xymoron.CaveDiver"); RegisterItemRecipies(); } catch (Exception e) { Debug.Log(e); }
        }
        void RegisterItemRecipies()
        {
            RecipeRegistry.Register(new Recipe
            {
                INT = 10,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "swimgoggles",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "rope" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "flexiglass" },
                new RecipeItem(25f) { specific = true, specificId = "biochem", isLiquid = true }
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 12,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "divingmask",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "rope" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "flexiglass" },
                new RecipeItem(0f) { specific = true, specificId = "flexiglass" },
                new RecipeItem(40f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 12,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "wetsuit",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(40f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 10,
                category = Recipes.RecipeCategory.Materials,
                result = new RecipeResult
                {
                    id = "closedcellfoam",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "scraptube" ,destroyItem = false},
                new RecipeItem(0f) { specific = true, specificId = "scrappanel" ,destroyItem = false},
                new RecipeItem(0f) { specific = true, specificId = "scrappanel" ,destroyItem = false},
                new RecipeItem(30f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f) { quality = CUCoreUtils.CreateCraftingQuality("heatsource"), destroyItem = false },
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 10,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "lifevest",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "closedcellfoam" },
                new RecipeItem(0f) { specific = true, specificId = "closedcellfoam" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(0f) { specific = true, specificId = "canvas" },
                new RecipeItem(0f) { specific = true, specificId = "belt" },
                new RecipeItem(30f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 10,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "weightbelt",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "scrapcube" },
                new RecipeItem(0f) { specific = true, specificId = "scrapcube" },
                new RecipeItem(0f) { specific = true, specificId = "belt" },
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 12,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "airtank",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "oxygencrystalshard" },
                new RecipeItem(0f) { specific = true, specificId = "scrappanel" },
                new RecipeItem(0f) { specific = true, specificId = "scrappanel" },
                new RecipeItem(0f) { specific = true, specificId = "scraptube" },
                new RecipeItem(0f) { specific = true, specificId = "scraptube" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "bundleofwires" },
                new RecipeItem(10f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "hammering", destroyItem = false },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });

            RecipeRegistry.Register(new Recipe
            {
                INT = 15,
                category = Recipes.RecipeCategory.Utilities,
                result = new RecipeResult
                {
                    id = "rebreather",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem(0f) { specific = true, specificId = "oxygencrystalshard" },
                new RecipeItem(0f) { specific = true, specificId = "titaniumsheet" },
                new RecipeItem(0f) { specific = true, specificId = "titaniumsheet" },
                new RecipeItem(0f) { specific = true, specificId = "plasticchunk" },
                new RecipeItem(0f) { specific = true, specificId = "bundleofwires" },
                new RecipeItem(0f) { specific = true, specificId = "bundleofwires" },
                new RecipeItem(0f) { specific = true, specificId = "circuitboard" },
                new RecipeItem(10f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "hammering", destroyItem = false },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });
        }
    }
}
