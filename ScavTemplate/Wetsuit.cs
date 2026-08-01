using CUCoreLib.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{
    public sealed class Wetsuit : MonoBehaviour
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
            if(body == null) return;


            if (item.isWet)
            {
                item.Stats.wearableIsolation = 0.5f;
            }
            else
            {
                item.Stats.wearableIsolation = 0.1f;
            }

            float num =  WorldGeneration.globalDecayRate;
            item.condition -= item.Stats.rotSpeed * num * Time.deltaTime * 0.01f;
            item.condition = Mathf.Clamp(item.condition, 0f, 1f);
        }
    }
}
