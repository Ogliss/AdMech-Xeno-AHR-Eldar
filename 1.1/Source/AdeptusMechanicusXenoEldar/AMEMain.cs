using System;
using System.Collections.Generic;
using System.Linq;
using AdeptusMechanicus.HarmonyInstance;
using AdeptusMechanicus.settings;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdeptusMechanicus
{
    [StaticConstructorOnStartup]
    public class AMEMain
    {
        public static List<ResearchProjectDef> AeldariResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Aeldari_Tech_")).ToList();
        public static List<ResearchProjectDef> EldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Eldar_Tech_")).ToList();
        public static List<ResearchProjectDef> DarkEldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_DarkEldar_Tech_")).ToList();
        static AMEMain()
        {
            AlienRace.ThingDef_AlienRace eldar = OGEldarThingDefOf.OG_Alien_Eldar as AlienRace.ThingDef_AlienRace;
            AlienRace.ThingDef_AlienRace darkeldar = DefDatabase<ThingDef>.GetNamedSilentFail("OG_Alien_DarkEldar") as AlienRace.ThingDef_AlienRace;
            List<ResearchProjectDef> research = EldarResearch;
            List<string> Tags = new List<string>() { "E" };
            research.AddRange(AeldariResearch);
            if (darkeldar == null)
            {
                research.AddRange(DarkEldarResearch);
                Tags.Add("DE");
            }
            ArmouryMain.DoRacialRestrictionsFor(eldar, Tags, research);
            if (!AdeptusIntergrationUtil.enabled_XenobiologisDarkEldar)
            {
                if (darkeldar != null)
                {
                    research = DarkEldarResearch;
                    research.AddRange(AeldariResearch);
                    ArmouryMain.DoRacialRestrictionsFor(darkeldar, "DE", research);
                }
            }
        }

    }
}