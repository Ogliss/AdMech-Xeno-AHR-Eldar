using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	[DefOf]
	public static class OGEldarThingDefOf
    {
		// Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
		static OGEldarThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OGEldarThingDefOf));
		}
        // Token: 0x04001EE3 RID: 7907
        // Plant Defs
        /*
        // Building Defs
        public static ThingDef OG_Eldar_FermentingBarrel;

        // Item Defs
        public static ThingDef OG_Eldar_Waart;
        public static ThingDef OG_Eldar_Grog;
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

        public static FleshTypeDef OG_Flesh_Construct_Eldar;
        /*
        public static ThingDef OG_Eldar_Snotling;
        public static ThingDef Squig;
        public static ThingDef OG_Squig_Eldar;
        */
    }

    public static class OGEldarPawnKindDefOf
    {
        // Token: 0x0600377C RID: 14204 RVA: 0x001A83CC File Offset: 0x001A67CC
        static OGEldarPawnKindDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGEldarPawnKindDefOf));
        }
        /*
        public static PawnKindDef Squig;

        public static PawnKindDef Snotling;

        public static PawnKindDef WildGrot;

        public static PawnKindDef WildEldar;
        */
    }

    public static class OGEldarFactionDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGEldarFactionDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGEldarFactionDefOf));
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

    public static class OGEldarJobDefOf
    {
        // Token: 0x06003770 RID: 14192 RVA: 0x001A8272 File Offset: 0x001A6672
        static OGEldarJobDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(OGEldarFactionDefOf));
        }

        //    public static JobDef TakeGrogOutOfEldarFermentingBarrel;
    }
}
