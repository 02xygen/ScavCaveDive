using System.Collections;
using System.Collections.Generic;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using UnityEngine;

namespace CaveDiver
{
    class BubbleRemover : MonoBehaviour
    {
        private ParticleSystem ps;
        private ParticleSystem.Particle[] particles;

        void Start()
        {
            ps = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
        }

        void Update()
        {
            int amount = ps.GetParticles(particles);

            for( int i = 0; i < amount; i++)
            {
                var cell = WorldGeneration.world.WorldToBlockPos(particles[i].position);
                byte liquid = FluidManager.main.GetLiquid(cell.x, cell.y);

                List<Color> colors = new List<Color> { Color.white, Color.white, Color.green, Color.black, new Color(0.59f, 0.3f, 0.0f), Color.grey, Color.red };
                
                if (liquid == 0)
                {
                    particles[i].remainingLifetime = -1f;
                }

                else
                {
                    particles[i].startColor = colors[liquid];
                }


            }
            ps.SetParticles(particles, amount);
        }
    }
}
