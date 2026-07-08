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
                new RecipeItem(0f) { specific = true, specificId = "string" },
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
                new RecipeItem() { specific = true, specificId = "rope" },
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "flexiglass" },
                new RecipeItem() { specific = true, specificId = "flexiglass" },
                new RecipeItem(0f) { specific = true, specificId = "string" },
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
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem(40f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
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
                new RecipeItem() { specific = true, specificId = "airbladder" },
                new RecipeItem() { specific = true, specificId = "airbladder" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem() { specific = true, specificId = "canvas" },
                new RecipeItem(0.25f) { specific = true, specificId = "belt" },
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
                    id = "airbladder",
                    amount = 1,
                    isLiquid = false,
                    resultCondition = 1f
                },
                items = new List<RecipeItem>
            {
                new RecipeItem() { specific = true, specificId = "plasticchunk" },
                new RecipeItem() { specific = true, specificId = "flexiglass" },
                new RecipeItem() { specific = true, specificId = "flexiglass" },
                new RecipeItem() { specific = true, specificId = "scraptube" },
                new RecipeItem(10f) { specific = true, specificId = "biochem", isLiquid = true },
                new RecipeItem(0f){ quality = "cutting", destroyItem = false }
            }
            });
        }
    }
}
