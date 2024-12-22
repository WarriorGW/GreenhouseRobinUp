using GreenhouseRobinUp;
using GreenhouseRobinUp.APIs;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Buildings;
using StardewValley;
using TheWarriorGW.GreenhouseRobinUp.Data;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        private void LoadModConfigMenu()
        {
            // API GenericModConfig
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;
            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () =>
                {
                    this.Helper.WriteConfig(this.Config);
                    Helper.GameContent.InvalidateCache("Data/Buildings");
                }
            );
            // First Upgrade
            configMenu.AddSectionTitle(
                mod: this.ModManifest, text: () => I18n.Config_Upgrade_Name(1));
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_GoldCost(),
                getValue: () => this.Config.UpgradeMaterials[1].Gold,
                setValue: value => this.Config.UpgradeMaterials[1].Gold = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(1),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[1].Slot1.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[1].Slot1.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[1].Slot1.Quantity,
                setValue: value => this.Config.UpgradeMaterials[1].Slot1.Quantity = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(2),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[1].Slot2.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[1].Slot2.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[1].Slot2.Quantity,
                setValue: value => this.Config.UpgradeMaterials[1].Slot2.Quantity = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(3),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[1].Slot3.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[1].Slot3.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[1].Slot3.Quantity,
                setValue: value => this.Config.UpgradeMaterials[1].Slot3.Quantity = value
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_BuildDays_Label(),
                tooltip: () => I18n.Config_BuildDays_Description(),
                min: 1,
                getValue: () => this.Config.UpgradeMaterials[1].BuildDays,
                setValue: value => this.Config.UpgradeMaterials[1].BuildDays = value
            );

            configMenu.AddSectionTitle(
                mod: this.ModManifest, text: () => I18n.Config_Upgrade_Name(2));
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_GoldCost(),
                getValue: () => this.Config.UpgradeMaterials[2].Gold,
                setValue: value => this.Config.UpgradeMaterials[2].Gold = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(1),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[2].Slot1.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[2].Slot1.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[2].Slot1.Quantity,
                setValue: value => this.Config.UpgradeMaterials[2].Slot1.Quantity = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(2),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[2].Slot2.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[2].Slot2.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[2].Slot2.Quantity,
                setValue: value => this.Config.UpgradeMaterials[2].Slot2.Quantity = value
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Material(3),
                allowedValues: Enum.GetNames(typeof(IDMaterials)),
                getValue: () => this.Config.UpgradeMaterials[2].Slot3.MaterialID.ToString(),
                setValue: value => this.Config.UpgradeMaterials[2].Slot3.MaterialID = (IDMaterials)Enum.Parse(typeof(IDMaterials), value)
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_Slot_Quantity(),
                getValue: () => this.Config.UpgradeMaterials[2].Slot3.Quantity,
                setValue: value => this.Config.UpgradeMaterials[2].Slot3.Quantity = value
            );
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => I18n.Config_BuildDays_Label(),
                tooltip: () => I18n.Config_BuildDays_Description(),
                min: 1,
                getValue: () => this.Config.UpgradeMaterials[2].BuildDays,
                setValue: value => this.Config.UpgradeMaterials[2].BuildDays = value
            );
        }
    }
}
