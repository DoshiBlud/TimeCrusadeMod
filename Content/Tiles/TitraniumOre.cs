using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Tiles
{
    internal class TitraniumOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileShine2[Type] = true;
            Main.tileOreFinderPriority[Type] = 410;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(156, 154, 156), CreateMapEntryName());
            
            DustType = DustID.Silver;
           VanillaFallbackOnModDeletion = TileID.Silver;
            HitSound = SoundID.Tink;
                MineResist = 1f;
                MinPick = 150;
                
        }
    }
}