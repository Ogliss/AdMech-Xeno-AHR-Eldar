using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public class Pawn_NeedsTracker_ShouldHaveNeed_DarkEldarSoulNeed_Patch
    {
        public static void Postfix(NeedDef nd, ref bool __result, Pawn ___pawn)
        {
            if (nd == AdeptusNeedDefOf.OGDE_Soul)
            {
                __result = ___pawn.isDarkEldar();
            }
        }
    }
}