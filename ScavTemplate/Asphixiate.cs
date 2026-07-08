using CaveDiver;
using CUCoreLib.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{

    public static class Asphixiate
    {
        static GameObject blush = new GameObject();

        [HarmonyPatch(typeof(Body), "Start")]
        public static class BlushMaker
        {
            [HarmonyPostfix]
            private static void Postfix(Body __instance)
            {

                    GameObject.Instantiate(blush);
                    Sprite cyanosis = AssetLoader.LoadEmbeddedSprite("cyanosis.png");
                    blush.AddComponent<SpriteRenderer>().sprite = cyanosis;
                    blush.GetComponent<SpriteRenderer>().sortingOrder = 52;
                    blush.transform.SetParent(__instance.limbs[0].gameObject.transform, false);
                    Color color = blush.GetComponent<SpriteRenderer>().color;
                    color.a = 0.0f;
                    blush.GetComponent<SpriteRenderer>().color = color;

            }
        }



        [HarmonyPatch(typeof(Body), "Update")]
        public static class BlushPatch
        {
            [HarmonyPostfix]
            private static void Postfix(Body __instance)
            {
               if(__instance.bloodOxygen < 90f)
               {
                    Color color = blush.GetComponent<SpriteRenderer>().color;
                    color.a = Mathf.Lerp(0.0f, 1.0f, Mathf.InverseLerp(90f, 60f, __instance.bloodOxygen));
                    blush.GetComponent<SpriteRenderer>().color = color;  
               }
               else
               {
                    Color color = blush.GetComponent<SpriteRenderer>().color;
                    color.a = 0.0f;
                    blush.GetComponent<SpriteRenderer>().color = color;
               }
            }
        }

    }
}
