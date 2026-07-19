using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Collections.Generic;

namespace TimeCrusadeMod.Content.Tiles
{
    public class AxitrentonCrystal : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            DustType = ModContent.DustType<AxitrentonDust>();
            VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

            AddMapEntry(new Color(192, 165, 192));
            HitSound = SoundID.Tink;
            MineResist = 1f;
            MinPick = 230;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}