using TimeCrusadeMod.Common.Systems;
using TimeCrusadeMod.Content.Biomes;
using Terraria;

namespace TimeCrusadeMod.Common
{
	// This class contains conditions that will be reused in multiple places in TimeCrusadeMod.
	// There is nothing wrong with making a new Condition where is it used, such as is shown in TimeCrusadeModNPCShop and TimeCrusadeModPerson,
	// but it is a good idea to place Conditions used multiple times in a central location to avoid typos and other bugs.
	// Storing the Condition in a field also exposes these conditions for easier cross-mod compatibility.
	// For more information on using the Condition class, see https://github.com/tModLoader/tModLoader/wiki/Conditions
	public static class NeutralBiomeCondition
	{
		public static Condition InNeutralBiome = new Condition("Mods.TimeCrusadeMod.Conditions.InNeutralBiome", () => Main.LocalPlayer.InModBiome<NeutralUndergroundBiome>());
	}
}