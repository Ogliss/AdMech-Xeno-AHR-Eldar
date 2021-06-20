using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using HarmonyLib;
using Verse.Sound;
using AdeptusMechanicus;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(TraitSet), "HasTrait")]
    public static class TraitSet_HasTrait_DarkEldar_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref bool __result)
        {
            if (___pawn != null)
            {
                if (tDef == TraitDefOf.Bloodlust ||tDef == TraitDefOf.Psychopath  || tDef == AdeptusTraitDefOf.Masochist || tDef == AdeptusTraitDefOf.Nimble)
                {
                    if (___pawn.isDarkEldar())
                    {
                        __result = true;
                    }
                }
            }
        }
    }
}
