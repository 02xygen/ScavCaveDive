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
    class RegisterItems : MonoBehaviour
    {
		
        void Awake()
        {
            try { ContentReloadManager.EnableHotReload("com.O2xymoron.CaveDiver"); CreateItems(); } catch (Exception e) { Debug.Log(e); }
        }

		void CreateItems() // TODO: MAKE DIVING BELT ITEM THAT DECREASES BOUYANCY.
        {
			Sprite wetsuitTorsoSprite = AssetLoader.LoadEmbeddedSprite("wetsuitUpTorso.png");
			Sprite wetsuitDownTorsoSprite = AssetLoader.LoadEmbeddedSprite("wetsuitDownTorso.png");
			Sprite wetsuitUpArmSprite = AssetLoader.LoadEmbeddedSprite("wetsuitUpArm.png");
			Sprite wetsuitDownArmSprite = AssetLoader.LoadEmbeddedSprite("wetsuitDownArm.png");
			Sprite wetsuitThighSprite = AssetLoader.LoadEmbeddedSprite("wetsuitThigh.png");
			Sprite wetsuitThighBackSprite = AssetLoader.LoadEmbeddedSprite("wetsuitThighBack.png");
			Sprite wetsuitCrusSprite = AssetLoader.LoadEmbeddedSprite("wetsuitCrus.png");
			Sprite diveMaskWornSprite = AssetLoader.LoadEmbeddedSprite("diveMaskHead.png");
			Sprite respiratorSprite = AssetLoader.LoadEmbeddedSprite("respiratorHead.png");
			Sprite swimGogglesWornSprite = AssetLoader.LoadEmbeddedSprite("swimGogglesHead.png");
			Sprite makeshiftFinsWornSprite = AssetLoader.LoadEmbeddedSprite("makeshiftFinFoot.png");
			Sprite airTankSprite = AssetLoader.LoadEmbeddedSprite("airTank.png");
			Sprite airTankTorsoSprite = AssetLoader.LoadEmbeddedSprite("scubatorso.png");
			Sprite airTankHeadSprite = AssetLoader.LoadEmbeddedSprite("regulatorHead.png");
			Sprite lifeVestSprite = AssetLoader.LoadEmbeddedSprite("lifeVest.png");
			Sprite lifeVestWornSprite = AssetLoader.LoadEmbeddedSprite("lifeVestWorn.png");
			Sprite rebreatherTorsoSprite = AssetLoader.LoadEmbeddedSprite("rebreatherTorso.png");
			Sprite rebreatherHeadSprite = AssetLoader.LoadEmbeddedSprite("rebreatherHead.png");

			Sprite airBladderFullSprite = AssetLoader.LoadEmbeddedSprite("airBladder.png");
			Sprite wetsuitSprite = AssetLoader.LoadEmbeddedSprite("wetsuitIcon.png");
			Sprite swimGogglesSprite = AssetLoader.LoadEmbeddedSprite("swimGogglesIcon.png");
			Sprite divingMaskSprite = AssetLoader.LoadEmbeddedSprite("divingMaskIcon.png");


            ItemRegistry.Register("lifevest", new CustomItemInfo
            {
                fullName = "Life Vest",
                description = "A wearable device that greatly increases bouyancy. Keeps you afloat in liquids. Is much more effective if you relax your body (ragdoll).",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 180f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
              ItemInfo.DecayType.NoDecayWhenNotWorn |
              ItemInfo.DecayType.NoDecayWhenStill
          ),
                wearable = true,
                weight = 0.2f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "harness",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableVisualOffset = 5,
                value = 8,
                WornSprite = lifeVestWornSprite,
            }.AddSpawnComponent<LifeVest>(), lifeVestSprite);

            ItemRegistry.Register("wetsuit", new CustomItemInfo
			{
				fullName = "Wetsuit",
				description = "A skin-tight bodysuit that keeps you warm in the water.",
				category = "utility",
				slotRotation = 0f,
				usable = false,
				usableOnLimb = false,
				decayMinutes = 240f,
				tags = "cangetwet",
				destroyAtZeroCondition = true,
				decayInfo = (byte)(
				ItemInfo.DecayType.NoDecayWhenNotWorn |
				ItemInfo.DecayType.NoDecayWhenStill
			),
				wearable = true,
				weight = 0.5f,
				desiredWearLimb = "UpTorso",
				wearSlotId = "outertorso",
				wearableIsolation = 0.1f,
				wearableHitDurabilityLossMultiplier = 0.25f,
				wearableArmor = 0.1f,
				wearableVisualOffset = 8,
				qualities = new List<CraftingQuality>
			{
				new CraftingQuality("rippable", 4f)
			},
				value = 16,
				WornSprite = wetsuitTorsoSprite,
                WornSpriteOffset = new Vector2(0f, -0.04f),
            }.AddSpawnComponent<Wetsuit>(), wetsuitSprite);

            ItemRegistry.Register("divingmask", new CustomItemInfo
            {
                fullName = "Diving Mask",
                description = "An air-tight mask that covers your eyes and nose. Can be cleared with a sharp exhale through the nose (Use Item).",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 240f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
                ItemInfo.DecayType.NoDecayWhenNotWorn |
                ItemInfo.DecayType.NoDecayWhenStill
            ),
                wearable = true,
                weight = 0.2f,
                desiredWearLimb = "Head",
                wearSlotId = "eyes",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.2f,
                wearableVisualOffset = 5,
                value = 12,
                CustomData =
                {
                    ["waterlogAmount"] = 0f
                },
                WornSprite = diveMaskWornSprite,
            }.AddSpawnComponent<DivingMask>(), divingMaskSprite);

            ItemRegistry.Register("swimgoggles", new CustomItemInfo
            {
                fullName = "Swim Goggles",
                description = "Air-tight goggles that cover your eyes. Cannot be cleared since it doesn't cover your nose.",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 180f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
                ItemInfo.DecayType.NoDecayWhenNotWorn |
                ItemInfo.DecayType.NoDecayWhenStill
            ),
                wearable = true,
                weight = 0.1f,
                desiredWearLimb = "Head",
                wearSlotId = "eyes",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                CustomData =
                {
                    ["waterlogAmount"] = 0f
                },
                WornSprite = swimGogglesWornSprite,
            }.AddSpawnComponent<SwimGoggles>(), swimGogglesSprite);

            ItemRegistry.Register("makeshiftfins", new CustomItemInfo
            {
                fullName = "Makeshift Fins",
                description = "A pair of crude fins. Slightly improves movement in liquids.",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 180f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn |
               ItemInfo.DecayType.NoDecayWhenStill
           ),
                wearable = true,
                weight = 0.9f,
                desiredWearLimb = "FootF",
                wearSlotId = "feet",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                WornSprite = makeshiftFinsWornSprite,
            },  makeshiftFinsWornSprite);

            ItemRegistry.Register("airtank", new CustomItemInfo
            {
                fullName = "Air Tank",
                description = "A canister full of pressurized air, with a regulator attached. When worn, indefintely allows breathing and stamina regeneration when submerged. Don't forget to put the regulator in your mouth! (Use Item)",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 180f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn
           ),
                wearable = true,
                weight = 0.9f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "back",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                CustomData =
                {
                    ["RegInMouth"] = true
                },
                WornSprite = airTankTorsoSprite,
            }.AddSpawnComponent<AirTank>(), airTankSprite);

            ItemRegistry.Register("rebreather", new CustomItemInfo
            {
                fullName = "Rebreather",
                description = "A device which re-oxyginates the air you exhale, allowing it to be breathed again. When worn, indefintely allows breathing and stamina regeneration when submerged. Don't forget to put the regulator in your mouth! (Use Item)",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 240f,
                Battery = new BatteryProperties
                {
                    Preset = BatteryItem.BatteryPreset.Large
                },
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn |
               ItemInfo.DecayType.BatteryDecay
           ),
                wearable = true,
                weight = 0.9f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "back",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                CustomData =
                {
                    ["RegInMouth"] = true
                },

                useAction = (body, item) =>
                {

                },

               WornSprite = rebreatherTorsoSprite,
            }.AddSpawnComponent<Rebreather>(), rebreatherTorsoSprite);

            ItemRegistry.Register("airbladder", new CustomItemInfo // PROBLEM CAN BE USED TO RAPIDLY REGAN STAMINA / SPo2 out of the water since it can be used multiple times.
                                                                   // Mabye make it a one use thing and reduce the material cost?
            {
                fullName = "Air Bladder",
                description = "A simple bladder featuring a hand pump and quick release valve. Holds a lungful of air that can be breathed in emergency situations. One time use.",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 120f,
                destroyAtZeroCondition = true,
                weight = 0.2f,
                value = 10,
                useAction = (body, item) =>
                {
                     body.bloodOxygen += 3f;
                     body.stamina += 5f;
                     Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.regulator.inhale"), body.transform.position, true, true, null, 0.75f, 0.85f);
                     item.SetCondition(0f);
                    
                },
            }, airBladderFullSprite);

        }
    }

}
