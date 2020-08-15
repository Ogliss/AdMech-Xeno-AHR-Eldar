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
        public static List<ResearchProjectDef> EldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Eldar_Tech_") || x.defName.Contains("OG_Aeldari_Tech_")).ToList();
        public static List<ResearchProjectDef> DarkEldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_DarkEldar_Tech_") || x.defName.Contains("OG_Aeldari_Tech_")).ToList();
        static AMEMain()
        {
            AlienRace.ThingDef_AlienRace eldar = OGEldarThingDefOf.OG_Alien_Eldar as AlienRace.ThingDef_AlienRace;
            foreach (ResearchProjectDef def in EldarResearch)
            {
                if (!AlienRace.RaceRestrictionSettings.researchRestrictionDict.ContainsKey(key: def))
                    AlienRace.RaceRestrictionSettings.researchRestrictionDict.Add(key: def, value: new List<AlienRace.ThingDef_AlienRace>());
                AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: eldar);
            }

            HarmonyPatches.TryAddRacialRestrictions(eldar, "E");

            if (!AdeptusIntergrationUtil.enabled_XenobiologisDarkEldar)
            {
                AlienRace.ThingDef_AlienRace darkeldar = DefDatabase<ThingDef>.GetNamedSilentFail("OG_Alien_DarkEldar") as AlienRace.ThingDef_AlienRace;
                if (darkeldar == null)
                {
                    foreach (ResearchProjectDef def in DarkEldarResearch)
                    {
                        if (!AlienRace.RaceRestrictionSettings.researchRestrictionDict.ContainsKey(key: def))
                            AlienRace.RaceRestrictionSettings.researchRestrictionDict.Add(key: def, value: new List<AlienRace.ThingDef_AlienRace>());
                        AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: eldar);
                    }

                    HarmonyPatches.TryAddRacialRestrictions(eldar, "DE");
                }
                else
                {
                    foreach (ResearchProjectDef def in DarkEldarResearch)
                    {
                        if (!AlienRace.RaceRestrictionSettings.researchRestrictionDict.ContainsKey(key: def))
                            AlienRace.RaceRestrictionSettings.researchRestrictionDict.Add(key: def, value: new List<AlienRace.ThingDef_AlienRace>());
                        AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: darkeldar);
                    }

                    HarmonyPatches.TryAddRacialRestrictions(darkeldar, "DE");
                }
            }
        }

    }
}