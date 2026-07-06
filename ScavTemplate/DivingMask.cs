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

            // Add screen blur logic

            // Add waterlog logic

            // Add clear action logic
        }
    }
}
