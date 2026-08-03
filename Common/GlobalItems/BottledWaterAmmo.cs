using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Projectiles;

public class BottledWaterAmmo : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        if (entity.type == ItemID.BottledWater)
        {
            entity.ammo = ItemID.BottledWater;
        }
    }

    public override void PickAmmo(Item item, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
    {
        if (ammo.type == ItemID.BottledWater)
        {
            type = ModContent.ProjectileType<WaterSpike>();
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        // Add ammo tooltip since it doesn't have it because the Placeable ("Can be placed") tooltip replaces it
        // Only needs to be done for placeable items (walls/blocks)
        if (item.type == ItemID.BottledWater)
        {
            int index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Placeable"));
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "Ammo", Language.GetTextValue("LegacyTooltip.34")));
            }
        }
    }
}