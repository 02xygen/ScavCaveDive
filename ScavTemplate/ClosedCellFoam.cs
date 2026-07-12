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
    public sealed class ClosedCellFoam : MonoBehaviour
    {
        private void FixedUpdate()
        {
            var cell = WorldGeneration.world.WorldToBlockPos(new Vector2(this.transform.position.x, this.transform.position.y));
            byte liquid = FluidManager.main.GetLiquid(cell.x, cell.y);

            if (liquid != 0)
            {
                GetComponent<Rigidbody2D>().velocity += Vector2.up * 2.0f;
            }
        }
    }
}
