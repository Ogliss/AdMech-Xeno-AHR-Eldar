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
    public static class TraitSet_GetTrait_DarkEldar_Patch
    {
        private static Trait bloodlust = new Trait(TraitDefOf.Bloodlust);
        private static Trait psychopath = new Trait(TraitDefOf.Psychopath);
        private static Trait nimble = new Trait(AdeptusTraitDefOf.Nimble);
        private static Trait masochist = new Trait(AdeptusTraitDefOf.Masochist);
        [HarmonyPostfix]
        public static void Postfix(TraitDef tDef, Pawn ___pawn, ref Trait __result)
        {
            if (___pawn != null)
            {
                if (___pawn.isDarkEldar())
                {
                    if (tDef == TraitDefOf.Bloodlust)
                    {
                        __result = bloodlust;
                    }
                    if (tDef == TraitDefOf.Psychopath)
                    {
                        __result = psychopath;
                    }
                    if (tDef == AdeptusTraitDefOf.Nimble)
                    {
                        __result = nimble;
                    }
                    if (tDef == AdeptusTraitDefOf.Masochist)
                    {
                        __result = masochist;
                    }
                }
            }
        }
    }
}
