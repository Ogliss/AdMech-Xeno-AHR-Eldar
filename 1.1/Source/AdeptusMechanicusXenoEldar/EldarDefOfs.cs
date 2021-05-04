using System;
using Verse;
using RimWorld;

namespace AdeptusMechanicus
{
	[DefOf]
	public static class EldarThingDefOf
    {
		static EldarThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(EldarThingDefOf));
		}
        // Plant Defs
        /*
        // Building Defs
        public static ThingDef OG_Eldar_FermentingBarrel;

        // Item Defs
        public static ThingDef OGE_Wraithbone;
        public static ThingDef OGE_SpiritStones;
        */
        // Backstory Defs
        /*
        public static AlienRace.BackstoryDef Eldar_Exodite_Child;
        public static AlienRace.BackstoryDef Eldar_Exodite_Psyker_Child;
        public static AlienRace.BackstoryDef Eldar_Craftworld_Child;
        public static AlienRace.BackstoryDef Eldar_Craftworld_Psyker_Child;
        public static AlienRace.BackstoryDef Eldar_Harlequinn_Child;
        public static AlienRace.BackstoryDef Eldar_Harlequinn_Psyker_Child;
        */

        // Humanlike Race Defs
        public static AlienRace.ThingDef_AlienRace OG_Alien_Eldar;
        [MayRequireDarkEldar]
        public static AlienRace.ThingDef_AlienRace OG_Alien_DarkEldar;

        public static FleshTypeDef OG_Flesh_Construct_Eldar;
        /*
        public static ThingDef OG_Eldar_Snotling;
        public static ThingDef Squig;
        public static ThingDef OG_Squig_Eldar;
        */
    }

    [DefOf]
    public static class EldarPawnKindDefOf
    {
        static EldarPawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EldarPawnKindDefOf));
        }
        /*
        public static PawnKindDef Squig;

        public static PawnKindDef Snotling;

        public static PawnKindDef WildGrot;

        public static PawnKindDef WildEldar;
        */
    }

    [DefOf]
    public static class EldarFactionDefOf
    {
        static EldarFactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EldarFactionDefOf));
        }

        /*
        public static FactionDef EldarPlayerColony;
        public static FactionDef EldarPlayerColonyTribal;

        public static FactionDef RokEldarz;
        public static FactionDef HulkEldarz;

        public static FactionDef EldarFaction;
        public static FactionDef FeralEldarFaction;
        */
    }

    [DefOf]
    public static class EldarJobDefOf
    {
        static EldarJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(EldarFactionDefOf));
        }

        //    public static JobDef TakeGrogOutOfEldarFermentingBarrel;
    }
}
