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
    [HarmonyPatch(typeof(TraitSet), "GetTrait")]
    public static class TraitSet_GetTrait_Eldar_Patch
    {
        private static Trait beautyEldarMale = new Trait(TraitDefOf.Beauty, 1);
        private static Trait beautyEldarFemale = new Trait(TraitDefOf.Beauty, 2);
        private static Trait nimble = new Trait(OGTraitDefOf.Nimble);
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isEldar())
                {
                    if (tDef == TraitDefOf.Beauty)
                    {
                        __result = ___pawn.gender == Gender.Male ? beautyEldarMale : beautyEldarFemale;
                    }
                    if (tDef == OGTraitDefOf.Nimble)
                    {
                        __result = nimble;
                    }
                }
            }
        }
    }
}
