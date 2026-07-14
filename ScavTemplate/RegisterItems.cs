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
			Sprite wetsuitFootSprite = AssetLoader.LoadEmbeddedSprite("wetsuitFoot.png");
			Sprite diveMaskWornSprite = AssetLoader.LoadEmbeddedSprite("diveMaskHead.png");
			Sprite regulatorSprite = AssetLoader.LoadEmbeddedSprite("regulatorHead.png");
			Sprite swimGogglesWornSprite = AssetLoader.LoadEmbeddedSprite("swimGogglesHead.png");
			Sprite makeshiftFinsWornSprite = AssetLoader.LoadEmbeddedSprite("makeshiftFinFoot.png");
			Sprite finsWornSprite = AssetLoader.LoadEmbeddedSprite("finsFoot.png");
			Sprite airTankSprite = AssetLoader.LoadEmbeddedSprite("airTank.png");
			Sprite airTankTorsoSprite = AssetLoader.LoadEmbeddedSprite("scubatorso.png");
			Sprite airTankHeadSprite = AssetLoader.LoadEmbeddedSprite("regulatorHead.png");
			Sprite lifeVestSprite = AssetLoader.LoadEmbeddedSprite("lifeVest.png");
			Sprite lifeVestWornSprite = AssetLoader.LoadEmbeddedSprite("lifeVestWorn.png");
			Sprite BCDWornSprite = AssetLoader.LoadEmbeddedSprite("BCDWorn.png");
			Sprite rebreatherTorsoSprite = AssetLoader.LoadEmbeddedSprite("rebreatherTorso.png");
			Sprite rebreatherHeadSprite = AssetLoader.LoadEmbeddedSprite("rebreatherHead.png");
			Sprite weightBeltSprite = AssetLoader.LoadEmbeddedSprite("weightBelt.png");

            
			Sprite closedCellFoamSprite = AssetLoader.LoadEmbeddedSprite("closedCellFoam.png");
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
                decayMinutes = 60f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
              ItemInfo.DecayType.NoDecayWhenNotWorn |
              ItemInfo.DecayType.NoDecayWhenStill
          ),
                wearable = true,
                wearableCanBeHeld = true,
                weight = 0.2f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "harness",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableVisualOffset = 5,
                value = 8,
                WornSprite = lifeVestWornSprite,
                SpawnFrequency = 1
            }.AddSpawnComponent<LifeVest>(), lifeVestSprite);

            ItemRegistry.Register("bcd", new CustomItemInfo
            {
                fullName = "Buoyancy Control Device",
                description = "A wearable device that automatically maintains neutral bouyancy.",
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
                wearableCanBeHeld = true,
                weight = 0.5f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "harness",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableVisualOffset = 5,
                value = 30,
                WornSprite = BCDWornSprite,
                SpawnFrequency = 1
            }, BCDWornSprite);

            ItemRegistry.Register("wetsuit", new CustomItemInfo
			{
				fullName = "Wetsuit",
				description = "A skin-tight bodysuit that keeps you warm in the water.",
				category = "utility",
				slotRotation = 0f,
				usable = false,
				usableOnLimb = false,
				decayMinutes = 180f,
				tags = "cangetwet",
				destroyAtZeroCondition = true,
				decayInfo = (byte)(
				ItemInfo.DecayType.NoDecayWhenNotWorn |
				ItemInfo.DecayType.NoDecayWhenStill
			),
				wearable = true,
                wearableCanBeHeld = true,
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
                MultiWornSprites = new Dictionary<string, Sprite>
                {
                    ["UpArmF"] = wetsuitUpArmSprite,
                    ["UpArmB"] = wetsuitUpArmSprite,
                    ["DownArmF"] = wetsuitDownArmSprite,
                    ["DownArmB"] = wetsuitDownArmSprite,
                    ["DownTorso"] = wetsuitDownTorsoSprite,
                    ["ThighF"] = wetsuitThighSprite,
                    ["ThighB"] = wetsuitThighBackSprite,
                    ["CrusF"] = wetsuitCrusSprite,
                    ["CrusB"] = wetsuitCrusSprite,
                    ["FootF"] = wetsuitFootSprite,
                    ["FootB"] = wetsuitFootSprite
                },
                SpawnFrequency = 1,
                WornSpriteOffset = new Vector2(0f, -0.04f)
            }.AddSpawnComponent<Wetsuit>(), wetsuitSprite);

            ItemRegistry.Register("divingmask", new CustomItemInfo
            {
                fullName = "Diving Mask",
                description = "An air-tight mask that covers your eyes and nose. Can be cleared with a sharp exhale through the nose (Use Item).",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 180f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
                ItemInfo.DecayType.NoDecayWhenNotWorn |
                ItemInfo.DecayType.NoDecayWhenStill
            ),
                wearable = true,
                wearableCanBeHeld = true,
                useAction = delegate (Body body, Item item)
                {
                    if (!CUCoreUtils.hasEquipped("divingmask")) // Has issues, runs after the item is already equipped, so will always run.
                    {
                        if (body.inWater) ItemRegistry.SetCustomData(item, "waterlogAmount", 1f);
                        else ItemRegistry.SetCustomData(item, "waterlogAmount", 0f);
                    }

                    ItemRegistry.TryGetCustomData<float>(item, "waterlogAmount", out float amount);
                    if (amount > 0 && body.inWater && body.GetWearable("divingmask") != null && !body.GetStatus<AspirationStatus>().aspirating)
                    {
                        ItemRegistry.SetCustomData(item, "waterlogAmount", 0f);
                        Sound.Play(AssetLoader.GetCachedAudioClip("caveDiver.regulator.exhale"), body.transform.position, true, true, null, 0.75f, 0.85f);
                        Bubbles.BubbleBurst(body.limbs[0].transform);
                        body.stamina -= 5f;
                    }
                },
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
                SpawnFrequency = 1
            }.AddSpawnComponent<DivingMask>(), divingMaskSprite);

            ItemRegistry.Register("swimgoggles", new CustomItemInfo
            {
                fullName = "Swim Goggles",
                description = "Air-tight goggles that cover your eyes. Cannot be cleared since it doesn't cover your nose.",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 80f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
                ItemInfo.DecayType.NoDecayWhenNotWorn |
                ItemInfo.DecayType.NoDecayWhenStill
            ),
                useAction = delegate (Body body, Item item)
                {
                    if (!CUCoreUtils.hasEquipped("swimgoggles")) // Has issues, runs after the item is already equipped
                    {
                        if (body.inWater) ItemRegistry.SetCustomData(item, "waterlogAmount", 1f);
                        else ItemRegistry.SetCustomData(item, "waterlogAmount", 0f);
                    }
                },
                wearable = true,
                wearableCanBeHeld = true,
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
                SpawnFrequency = 1
            }.AddSpawnComponent<SwimGoggles>(), swimGogglesSprite);

            ItemRegistry.Register("makeshiftfins", new CustomItemInfo
            {
                fullName = "Makeshift Fins",
                description = "A pair of crude fins. Slightly improves movement in liquids.",
                category = "utility",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 80f,
                destroyAtZeroCondition = true,
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn |
               ItemInfo.DecayType.NoDecayWhenStill
           ),
                wearable = true,
                wearableCanBeHeld = true,
                weight = 0.4f,
                desiredWearLimb = "FootF",
                wearSlotId = "feet",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                WornSprite = makeshiftFinsWornSprite,
                MultiWornSprites = new Dictionary<string, Sprite>
                {
                    ["FootB"] = makeshiftFinsWornSprite
                },
                SpawnFrequency = 1
            },  makeshiftFinsWornSprite);

            ItemRegistry.Register("fins", new CustomItemInfo
            {
                fullName = "Fins",
                description = "A pair of swim fins. Greatly improves movement in liquids.",
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
                wearableCanBeHeld = true,
                weight = 0.4f,
                desiredWearLimb = "FootF",
                wearSlotId = "feet",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 12,
                WornSprite = finsWornSprite,
                MultiWornSprites = new Dictionary<string, Sprite>
                {
                    ["FootB"] = finsWornSprite
                },
                SpawnFrequency = 1
            }, finsWornSprite);

            ItemRegistry.Register("airtank", new CustomItemInfo
            {
                fullName = "Air Tank",
                description = "A canister full of pressurized air, with a regulator attached. When worn, indefintely allows breathing and stamina regeneration when submerged. Don't forget to put the regulator in your mouth! (Use Item)",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 120f,
                destroyAtZeroCondition = false,
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn
           ),
                wearable = true,
                wearableCanBeHeld = true,
                weight = 0.9f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "back",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                CustomData =
                {
                    ["RegInMouth"] = false
                },

                useAction = delegate (Body body, Item item)
                {
                    ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn); //Add sprite logic for this
                    if (regIn)
                    {
                        ItemRegistry.SetCustomData(item, "RegInMouth", false);
                        Debug.Log("Removed Regulator");
                    }
                    else if (!regIn && !body.disfigured)
                    {
                        ItemRegistry.SetCustomData(item, "RegInMouth", true);
                        Debug.Log("Inserted Regulator");
                    }

                },
                WornSprite = airTankTorsoSprite,
                MultiWornSprites = new Dictionary<string, Sprite>
                {
                    ["Head"] = regulatorSprite
                },
                SpawnFrequency = 1
            }.AddSpawnComponent<AirTank>(), airTankSprite);

            ItemRegistry.Register("rebreather", new CustomItemInfo
            {
                fullName = "Rebreather",
                description = "A device which re-oxyginates the air you exhale, allowing it to be breathed again. When worn, indefintely allows breathing and stamina regeneration when submerged. Don't forget to put the regulator in your mouth! (Use Item)",
                category = "utility",
                slotRotation = 0f,
                usable = true,
                usableOnLimb = false,
                decayMinutes = 180f,
                Battery = new BatteryProperties
                {
                    Preset = BatteryItem.BatteryPreset.Large
                },
                decayInfo = (byte)(
               ItemInfo.DecayType.NoDecayWhenNotWorn |
               ItemInfo.DecayType.BatteryDecay
           ),
                wearable = true,
                wearableCanBeHeld = true,
                weight = 0.9f,
                desiredWearLimb = "UpTorso",
                wearSlotId = "back",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableArmor = 0.15f,
                wearableVisualOffset = 5,
                value = 8,
                CustomData =
                {
                    ["RegInMouth"] = false
                },

                useAction = delegate (Body body, Item item) //Add sprite logic for this
                {
                    ItemRegistry.TryGetCustomData<bool>(item, "RegInMouth", out bool regIn);
                    if (regIn)
                    {
                        ItemRegistry.SetCustomData(item, "RegInMouth", false);
                        Debug.Log("Removed Regulator");
                    }
                    else if (!regIn && !body.disfigured)
                    {
                        ItemRegistry.SetCustomData(item, "RegInMouth", true);
                        Debug.Log("Inserted Regulator");
                    }

                },

                WornSprite = rebreatherTorsoSprite,
                MultiWornSprites = new Dictionary<string, Sprite>
                {
                    ["Head"] = rebreatherHeadSprite
                },
                SpawnFrequency = 1
            }.AddSpawnComponent<Rebreather>(), rebreatherTorsoSprite);

            ItemRegistry.Register("closedcellfoam", new CustomItemInfo
            {
                fullName = "Closed-cell foam",
                description = "A dense block of foam filled with sealed gas pockets. Very buoyant.",
                category = "material",
                slotRotation = 0f,
                usable = false,
                usableOnLimb = false,
                decayMinutes = 80f,
                destroyAtZeroCondition = true,
                weight = 0.2f,
                value = 10,
                SpawnFrequency = 0
            }.AddSpawnComponent<ClosedCellFoam>(), closedCellFoamSprite);

            ItemRegistry.Register("weightbelt", new CustomItemInfo
            {
                fullName = "Weight Belt",
                description = "A belt fitted with heavy weights. Decreases bouyancy when worn, making you sink quickly in liquids. Is much more effective if you relax your body (ragdoll).",
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
                wearableCanBeHeld = true,
                weight = 1.5f,
                desiredWearLimb = "DownTorso",
                wearSlotId = "belt",
                wearableHitDurabilityLossMultiplier = 0.25f,
                wearableVisualOffset = 2,
                value = 12,
                WornSprite = weightBeltSprite,
                SpawnFrequency = 1
            }.AddSpawnComponent<WeightBelt>(), weightBeltSprite);

        }
    }

}
