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
    public class AMKMain
    {
        public static List<ResearchProjectDef> EldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Eldar_Tech_") || x.defName.Contains("OG_Aeldari_Tech_")).ToList();
        static AMKMain()
        {
            AlienRace.ThingDef_AlienRace eldar = OGEldarThingDefOf.OG_Alien_Eldar as AlienRace.ThingDef_AlienRace;
            foreach (ResearchProjectDef def in EldarResearch)
            {
                if (!AlienRace.RaceRestrictionSettings.researchRestrictionDict.ContainsKey(key: def))
                    AlienRace.RaceRestrictionSettings.researchRestrictionDict.Add(key: def, value: new List<AlienRace.ThingDef_AlienRace>());
                AlienRace.RaceRestrictionSettings.researchRestrictionDict[key: def].Add(item: eldar);
            }

            HarmonyPatches.TryAddRacialRestrictions(eldar, "E");
        }

    }
}