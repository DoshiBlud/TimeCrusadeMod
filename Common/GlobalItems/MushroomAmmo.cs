using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Projectiles.Rockets;

public class MushroomAmmo : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        if (entity.type == ItemID.Mushroom)
        {
            entity.ammo = ItemID.Mushroom;
        }
        if (entity.type == ItemID.GlowingMushroom)
        {
            entity.ammo = ItemID.GlowingMushroom;
        }
    }

    public override void PickAmmo(Item item, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
    {
        if (ammo.type == ItemID.Mushroom)
        {
            type = ModContent.ProjectileType<MushroomRocket>();
        }
        if (ammo.type == ItemID.GlowingMushroom)
        {
            type = ModContent.ProjectileType<GlowingMushroomRocket>();
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        // Add ammo tooltip since it doesn't have it because the Placeable ("Can be placed") tooltip replaces it
        // Only needs to be done for placeable items (walls/blocks)
        if (item.type == ItemID.Mushroom || item.type == ItemID.GlowingMushroom)
        {
            int index = tooltips.FindLastIndex(tt => tt.Mod.Equals("Terraria") && tt.Name.Equals("Placeable"));
            if (index != -1)
            {
                tooltips.Insert(index + 1, new TooltipLine(Mod, "Ammo", Language.GetTextValue("LegacyTooltip.34")));
            }
        }
    }
}