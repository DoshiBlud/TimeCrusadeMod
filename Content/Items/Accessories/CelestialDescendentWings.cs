using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Common.Configs;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Rarities;
using TimeCrusadeMod.Content.Tiles.Furniture;
using static Terraria.GameContent.Bestiary.IL_BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions;

namespace TimeCrusadeMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class CelestialDescendentWings : ModItem
    {
        // To see how this config option was added, see ExampleModConfig.cs
        // This code allows users to toggle loading this content via a config. Another common usage of IsLoadingEnabled would be to use ModLoader.HasMod to check if another mod is enabled or not.
        // Feel free to remove this method in your own Wings if using this as a template, it is superfluous.
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<TimeCrusadeModConfig>().CelestialDescendentWingsToggle;
        }

        public override void SetStaticDefaults()
        {
            // These wings use the same values as the solar wings
            // Fly time: 180 ticks = 3 seconds
            // Fly speed: 9
            // Acceleration multiplier: 2.5
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(300, 9f, 2.5f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<IllusionaryRarity>();
            Item.accessory = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 2.3f; // Falling glide speed
            ascentWhenRising = 0.10f; // Rising speed
            maxCanAscendMultiplier = 4f;
            maxAscentMultiplier = 2f;
            constantAscend = 0.5f;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IllusionaryDescendentBar>(), 3)
                .AddIngredient(ItemID.AngelWings)
                .AddIngredient(ItemID.LunarOre, 20)
                .AddTile(ModContent.TileType<Tiles.Furniture.AxitrentonObserver>())
                .SortBefore(Main.recipe.First(recipe => recipe.createItem.wingSlot != -1)) // Places this recipe before any wing so every wing stays together in the crafting menu.
                .Register();
        }
    }
}