using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria.Localization;

namespace TimeCrusadeMod.Content.Tiles
{
    internal class SciFiBar : ModTile
    {
        public override void SetStaticDefaults()
        {
                Main.tileSolid[Type] = true;
                Main.tileMergeDirt[Type] = true;
                Main.tileBlockLight[Type] = true;
                Main.tileLighted[Type] = false;
                Main.tileOreFinderPriority[Type] = 500;
                Main.tileFrameImportant[Type] = true;
                Main.tileSolidTop[Type] = true;
    
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
                TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
                TileObjectData.newTile.StyleHorizontal = true;
                TileObjectData.newTile.LavaDeath = false;
                TileObjectData.addTile(Type);
    
                AddMapEntry(new Color(20, 20, 200), Language.GetText("SciFi Bar"));
        }
        public bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            int style = t.TileFrameX / 18;

            switch (style)
            {
                case 0:
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.TileType<SciFiBar>());
                    break;
                    case 1:
                    // ADD THE ITEM
                    break;
                    case 2:
                    // ADD THE ITEM
                    break;
            }
            return true;
        }
    }
}