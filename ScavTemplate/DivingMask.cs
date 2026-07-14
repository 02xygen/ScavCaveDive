using CUCoreLib.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{
    public sealed class DivingMask() : MonoBehaviour
    {
        private Item item;

        private void Awake()
        {
            item = GetComponent<Item>();
        }

        private void Update()
        {
            Limb wornLimb = item != null && item.transform.parent != null
                ? item.transform.parent.GetComponent<Limb>() : null;

            if (wornLimb == null) return;

            Body body = wornLimb.body;
            if (body == null) return;

            if (item.condition < 0.5 && body.inWater)
            {
                ItemRegistry.TryGetCustomData<float>(item, "waterlogAmount", out float amount);
                amount += (1f - item.condition) * Time.deltaTime / 120f;
                amount = Mathf.Clamp(amount, 0.05f, 1.0f);
                ItemRegistry.SetCustomData(item, "waterlogAmount", amount);
            }

        }
    }
}
