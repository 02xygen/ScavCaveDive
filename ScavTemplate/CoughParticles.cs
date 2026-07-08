using System.Collections;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using UnityEngine;

namespace CaveDiver
{

    public static class CoughParticles
    {
        private static GameObject temp;
        public static GameObject coughParticles
        {
            get
            {
                if (temp == null)
                {
                    temp = new GameObject("coughParticles");
                    temp.AddComponent<ParticleSystem>();
                    Object.DontDestroyOnLoad(temp);
                    temp.SetActive(false);
                }
                return temp;
            }
        }

        public static void Burst(Transform spawnTransform, Color color)
        {
            SpawnParticles(coughParticles, spawnTransform, 0.3f, 120f, color);
        }


        static void SpawnParticles(GameObject prefab, Transform spawnTransform, float time, float amount, Color color)
        {
            GameObject vfx = GameObject.Instantiate(prefab, spawnTransform.position, Quaternion.identity);
            vfx.transform.SetParent(spawnTransform);
            vfx.SetActive(true);
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            ParticleSystem.Burst psb = new ParticleSystem.Burst(time, amount);
            var main = ps.main;
            var emission = ps.emission;
            var shape = ps.shape;

            main.gravityModifierMultiplier = 1f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.duration = 0.2f;

            main.startSize = 1f;
            main.startColor = color;
            main.loop = false;

            shape.angle = 10f;
            emission.rateOverTime = amount;

            ParticleSystemRenderer psRenderer = ps.GetComponent<ParticleSystemRenderer>();
            var mat = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            mat.SetFloat("_Surface", 0.5f);
            mat.SetInt("_Blend", 0);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            psRenderer.material = mat;
            var textureSheet = ps.textureSheetAnimation;
            textureSheet.enabled = true;
            textureSheet.mode = ParticleSystemAnimationMode.Sprites;

            if (textureSheet.spriteCount > 0)
            {
                textureSheet.RemoveSprite(0);
            }
            textureSheet.AddSprite(AssetLoader.LoadEmbeddedSprite("vomit.png"));

            ps.Play();
        }

    }
}
