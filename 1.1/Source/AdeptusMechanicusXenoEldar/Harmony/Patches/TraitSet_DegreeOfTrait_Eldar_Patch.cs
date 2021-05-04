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
    [HarmonyPatch(typeof(TraitSet), "DegreeOfTrait")]
    public static class TraitSet_DegreeOfTrait_Eldar_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref int __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isEldar())
                {
                    if (tDef == TraitDefOf.Beauty)
                    {
                        __result = ___pawn.gender == Gender.Male ? 1 : 2;
                    }
                    else
                    if (tDef == AdeptusTraitDefOf.Nimble)
                    {
                        __result = 0;
                    }
                }
            }
        }
    }
}
