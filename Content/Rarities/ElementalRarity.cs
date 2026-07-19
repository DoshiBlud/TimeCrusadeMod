using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Rarities
{
    public class ElementalRarity : ModRarity
    {
        public override Color RarityColor => new Color(Main.DiscoR / 0.55f, (byte)(Main.DiscoG / 0.45f), (byte)(Main.DiscoB / 0.55f));
        /*RGB is VERY important for the colors in this code.
         * R = red
         * G = green
         * B = blue
         * */
    }
}