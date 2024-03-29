﻿using System;
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
        public static List<ResearchProjectDef> EldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Aeldari_Tech_") || x.defName.Contains("OG_Eldar_Tech_")).ToList();
        public static List<ResearchProjectDef> DarkEldarResearch => DefDatabase<ResearchProjectDef>.AllDefs.Where(x => x.defName.Contains("OG_Aeldari_Tech_") || x.defName.Contains("OG_DarkEldar_Tech_")).ToList();
        static AMEMain()
        {
            List<string> blackTags = ArmouryMain.humansTags;
            List<ResearchProjectDef> blackProjects = new List<ResearchProjectDef>();
            blackProjects.AddRange(ArmouryMain.ReseachImperial);
            blackProjects.AddRange(ArmouryMain.ReseachChaos);

            List<ResearchProjectDef> whiteProjects = EldarResearch;
            List<string> whiteTags = new List<string>() { "E" };
            List<ThingDef> whiteApparel = DefDatabase<ThingDef>.AllDefsListForReading.FindAll(x => x.defName.Contains("OGE_Apparel_") || x.defName.Contains("OGDE_Apparel_"));
            whiteProjects.AddRange(AeldariResearch);
            if (AdeptusThingDefOf.OG_Alien_DarkEldar == null)
            {
                whiteProjects.AddRange(DarkEldarResearch);
                whiteTags.Add("DE");
            }

            AlienRaceUtility.DoRacialRestrictionsFor(AdeptusThingDefOf.OG_Alien_Eldar, whiteTags, blackTags, whiteProjects, blackProjects, whiteApparel, Logging: AMAMod.Dev);
        }

    }
}