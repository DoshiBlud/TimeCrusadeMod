using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Rarities;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace TimeCrusadeMod.Content.Items.Weapons.Summoner
{
    public class WhipofTorture : ModItem
    {

        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            // Mouse over to see its parameters.
            Item.autoReuse = true;
            Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.WhipofTorture>(), 1, 20, 20);
            Item.rare = ModContent.RarityType<TerrifierRarity>();
            Item.damage = 184;
            Item.width = 76;
            Item.height = 68;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}