using CUCoreLib.Data;
using CUCoreLib.Helpers;
using CUCoreLib.Registries;
using UnityEngine;

namespace CaveDiver
{
    internal class RegisterSettings : MonoBehaviour
    {
        private void Awake()
        {
            ModOptionsRegistry.Register(ModOptionDefinition.Bool(
            "caveDiver.forceFlood.enabled",
            "Force Flooded modifier",
            "Causes every layer to generate with a Flooded modifier.",
            Setting.SettingCategory.Game,
            PlayerPrefs.GetInt("ForceFlood_Enabled", 1) == 1,
            value =>
            {
                PlayerPrefs.SetInt("ForceFlood_Enabled", value ? 1 : 0);
                PlayerPrefs.Save();
            }
            ));

            ModOptionsRegistry.Register(ModOptionDefinition.Int(
            "caveDiver.forceFlood.amount",
            "Force Flooded Strength",
            "Controls how many times the flooded modifer is applied during layer generation. Higher numbers mean more flooding. Ranges from 1 to 10 for performance.",
            Setting.SettingCategory.Game,
            PlayerPrefs.GetInt("ForceFlood_Strength", 2),
            1,
            10,
            value =>
            {
            PlayerPrefs.SetInt("ForceFlood_Strength", value);
            PlayerPrefs.Save();
            }
            ));


            ModOptionsRegistry.Register(ModOptionDefinition.Bool(
            "caveDiver.floodRamp.enabled",
            "Increase Flooding amount every layer (up to x10)",
            "Causes every sebsequent layer to generate with a stronger Flooded modifier.",
            Setting.SettingCategory.Game,
            PlayerPrefs.GetInt("FloodRamp_Enabled", 1) == 1,
            value =>
            {
                PlayerPrefs.SetInt("ForceRamp_Enabled", value ? 1 : 0);
                PlayerPrefs.Save();
            }
            ));
        }
    }
}
