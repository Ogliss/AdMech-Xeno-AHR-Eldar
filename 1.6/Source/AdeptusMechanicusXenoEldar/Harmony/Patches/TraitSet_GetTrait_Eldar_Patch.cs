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
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef) })]
    [HarmonyPatch(typeof(TraitSet), "GetTrait", new Type[] { typeof(TraitDef), typeof(int) })]
    public static class TraitSet_GetTrait_Eldar_Patch
    {
        private static Trait beautyEldarMale = new Trait(AdeptusTraitDefOf.Beauty, 1);
        private static Trait beautyEldarFemale = new Trait(AdeptusTraitDefOf.Beauty, 2);
        private static Trait nimble = new Trait(AdeptusTraitDefOf.Nimble);
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isEldar())
                {
                    if (tDef == AdeptusTraitDefOf.Beauty)
                    {
                        __result = ___pawn.gender == Gender.Male ? beautyEldarMale : beautyEldarFemale;
                    }
                    if (tDef == AdeptusTraitDefOf.Nimble)
                    {
                        __result = nimble;
                    }
                }
            }
        }
    }
}
