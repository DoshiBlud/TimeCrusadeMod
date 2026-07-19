using Microsoft.Xna.Framework;
using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TimeCrusadeMod.Common.Systems
{
	public class DownedBossSystem : ModSystem
	{
        public static bool downedAsphalia = false;
        public static bool downedTerrifier = false;
        public static bool downedAxy = false;
        public override void OnWorldLoad()
        {
            // Reset to false when loading a new world
            downedAsphalia = false;
            downedTerrifier = false;
            downedAxy = false;
        }

        public override void OnWorldUnload()
        {
            // Reset to false when leaving a world
            downedAsphalia = false;
            downedTerrifier = false;
            downedAxy = false;
        }

        // Saving the downed state to the world file
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedAsphalia) tag["downedAsphalia"] = true;
            if (downedTerrifier) tag["downedTerrifier"] = true;
            if (downedAxy) tag["downedAxy"] = true;
        }

        // Loading the downed state from the world file
        public override void LoadWorldData(TagCompound tag)
        {
            downedAsphalia = tag.ContainsKey("downedAsphalia");
            downedTerrifier = tag.ContainsKey("downedTerrifier");
            downedAxy = tag.ContainsKey("downedAxy");
        }

        // Syncing data between server and clients in multiplayer
        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = downedAsphalia;
            flags[1] = downedTerrifier;
            flags[2] = downedAxy;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedAsphalia = flags[0];
            downedTerrifier = flags[1];
            downedAxy = flags[2];
        }
	}
}