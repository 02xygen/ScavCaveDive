using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;

namespace CaveDiver
{
    public sealed class LifeVest : MonoBehaviour
    {
        private void FixedUpdate()
        {
            var cell = WorldGeneration.world.WorldToBlockPos(new Vector2(PlayerCamera.main.body.limbs[1].transform.position.x, PlayerCamera.main.body.limbs[1].transform.position.y));
            byte liquid = FluidManager.main.GetLiquid(cell.x, cell.y);

            if (liquid != 0 && PlayerCamera.main.body.HasWearable("lifevest"))
            {

                if (!PlayerCamera.main.body.standing)
                {
                    PlayerCamera.main.body.rb.AddForce(Vector2.up * 3000);
                    PlayerCamera.main.body.limbs[0].rb.AddForce(Vector2.up * 1000);
                }
                else PlayerCamera.main.body.rb.velocity += Vector2.up * 2.0f;

                return;
            }

            cell = WorldGeneration.world.WorldToBlockPos(new Vector2(this.transform.position.x, this.transform.position.y));
            liquid = FluidManager.main.GetLiquid(cell.x, cell.y);

            if (liquid != 0)
            {
                GetComponent<Rigidbody2D>().velocity += Vector2.up * 2.0f;
            }
        }
    }
}
