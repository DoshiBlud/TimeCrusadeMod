using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Threading;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

namespace TimeCrusadeMod.Content.Tiles
{
    internal class ZilvicOreTile : ModTile
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

            AddMapEntry(new Color(99, 65, 28), CreateMapEntryName());
            
            DustType = DustID.Silver;
           VanillaFallbackOnModDeletion = TileID.Silver;
            HitSound = SoundID.Tink;
                MineResist = 2f;
                MinPick = 190;
                
        }
    }
    public class ZilvicOreSystem : ModSystem
    {
        public static bool ZilvicOreSpawned = true;
        public override void PostWorldGen()
        {
            ZilvicOreSpawned = true;
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Zilvic Ore", (Progress, gameStructure) =>
                {
                    OreGeneration(Progress);
                }));
            }
        }

        private void OreGeneration(GenerationProgress progress)
        {
            progress.Message = "Generating Zilvic Ore";

            for (int i = 0; i < (int)((Main.maxTilesX * Main.maxTilesY) * 0.0008); i++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 500);
                if (Main.tile[x, y].TileType == TileID.Sandstone && Main.tile[x, y].HasTile)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<ZilvicOreTile>());
                }
            }
        }
    }
}