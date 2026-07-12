using CUCoreLib.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CaveDiver
{
    public sealed class RefillAirTank : MonoBehaviour
    {
        private UsableObject usable;
        bool used = false;

        private void Start()
        {
            usable = gameObject.AddComponent<UsableObject>();
            usable.didLangString = true;
            usable.toggleString = "Refill air tank. (Will break the pump)";
        }

        public void OnUse()
        {
            Item heldItem = PlayerCamera.main.body.GetItem(PlayerCamera.main.body.handSlot);
            if (heldItem == null || !heldItem.TryGetComponent(out AirTank tank))
            {
                return;
            }

            if(!used)
            {
                tank.gameObject.GetComponent<Item>().condition = 1f;
                Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.airtank.filltank"), transform.position, true, true, null, 3f, 0.85f);
                gameObject.GetComponent<LifepodPump>().StopAllCoroutines();
                gameObject.GetComponent<BuildingEntity>().description = "Broken, does nothing.";
                usable.toggleString = "Pump broken and unusable.";
                used = true;
            }


        }
    }
}