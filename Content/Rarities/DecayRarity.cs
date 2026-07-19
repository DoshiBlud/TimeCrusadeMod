using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Rarities
{
    public class DecayRarity : ModRarity
    {
        public override Color RarityColor => new Color(0.5f, 0, 0.5f);
        /*RGB is VERY important for the colors in this code.
         * R = red
         * G = green
         * B = blue
         * */
    }
}