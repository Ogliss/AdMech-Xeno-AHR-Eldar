﻿using System;
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
using UnityEngine;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    public static class PawnGenerator_GeneratePawn_Eldar_Patch
    {
        [HarmonyPrefix, HarmonyPriority(Priority.Last)]
        public static void Pre_GeneratePawn(ref PawnGenerationRequest request)
        {
            if (request.KindDef.race == AdeptusThingDefOf.OG_Alien_Eldar)
            {
                /*
                //    Log.Message(string.Format("GeneratePawn request is {0}, {1}, {2}", request.KindDef.LabelCap, request.FixedGender, request.MustBeCapableOfViolence));
                request.AllowGay = false;
                request.FixedGender = Gender.None;
                request.CanGeneratePawnRelations = false;
                request.ColonistRelationChanceFactor = 0f;
                request.RelationWithExtraPawnChanceFactor = 0f;
                request.ExtraPawnForExtraRelationChance = null;

                request.MustBeCapableOfViolence = request.KindDef.isOrk();
                if (request.KindDef == OGOrkPawnKindDefOf.OG_Ork_Wild || request.KindDef == OGOrkPawnKindDefOf.OG_Grot_Wild)
                {
                    request.Newborn = true;
                    request.ForbidAnyTitle = true;
                    request.ForceGenerateNewPawn = true;
                    request.ForceBodyType = request.KindDef.isOrk() ? BodyTypeDefOf.Male : BodyTypeDefOf.Thin;
                    request.FixedBiologicalAge = 0f;
                    request.FixedChronologicalAge = 0f;
                    request.AllowAddictions = false;
                    request.ForceAddFreeWarmLayerIfNeeded = false;
                }
                */
            //    request = new PawnGenerationRequest(request.KindDef, request.Faction, request.Context, request.Tile, request.ForceGenerateNewPawn, request.Newborn, request.AllowDead, request.AllowDowned, request.CanGeneratePawnRelations, request.MustBeCapableOfViolence, request.RelationWithExtraPawnChanceFactor, request.ForceAddFreeWarmLayerIfNeeded, request.AllowGay, request.AllowFood, request.AllowAddictions, request.Inhabitant, request.CertainlyBeenInCryptosleep, request.ForceRedressWorldPawnIfFormerColonist, request.WorldPawnFactionDoesntMatter, request.BiocodeWeaponChance, request.ExtraPawnForExtraRelationChance, request.RelationWithExtraPawnChanceFactor, request.ValidatorPreGear, request.ValidatorPostGear, request.ForcedTraits, request.ProhibitedTraits, request.MinChanceToRedressWorldPawn, request.FixedBiologicalAge, request.FixedChronologicalAge, request.FixedGender, request.FixedMelanin, request.FixedLastName);
            //      Log.Message(string.Format("GeneratePawn End request is {0}\n{1}", request.KindDef.LabelCap, request.ToString()));
            }
        }
        
        [HarmonyPostfix, HarmonyPriority(Priority.Last)]
        public static void Post_GeneratePawn(PawnGenerationRequest request, ref Pawn __result)
        {
            if (__result != null && __result.def == AdeptusThingDefOf.OG_Alien_Eldar)
            {
            //    Log.Message(__result.Name + " " + __result.gender.ToString());
                if (__result.story == null)
                {
                    return;
                }
                Pawn_StoryTracker storyTracker = __result.story;
                Backstory adulthood = storyTracker.adulthood;
                bool adult = adulthood != null;
                switch (__result.gender)
                {
                    case Gender.Male:
                        storyTracker.bodyType = BodyTypeDefOf.Male;
                        break;
                    case Gender.Female:
                        storyTracker.bodyType = BodyTypeDefOf.Female;
                        break;
                    default:
                        break;
                }
                if (storyTracker.childhood.spawnCategories.Contains("Eldar_Craftworld_Psyker"))
                {
                    if (!storyTracker.traits.HasTrait(TraitDefOf.PsychicSensitivity))
                    {
                        Trait trait = new Trait(TraitDefOf.PsychicSensitivity, 1);
                        if (storyTracker.adulthood != null)
                        {
                            if (storyTracker.adulthood.identifier.Contains("_Farseer"))
                            {
                                trait = new Trait(TraitDefOf.PsychicSensitivity, 2);
                            }
                            else if (storyTracker.adulthood.identifier.Contains("_Warlock"))
                            {
                                Rand.PushState();
                                trait = new Trait(TraitDefOf.PsychicSensitivity, Rand.RangeInclusive(1, 2));
                                Rand.PopState();
                            }
                        }
                        __result.story.traits.GainTrait(trait);
                    }
                    if (AdeptusIntergrationUtility.enabled_Royalty)
                    {
                        if (!__result.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier))
                        {
                            Hediff_Psylink _Psylink = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, __result, __result.RaceProps.body.AllParts.FirstOrDefault(x => x.def == BodyPartDefOf.Brain)) as Hediff_Psylink;
                            _Psylink.suppressPostAddLetter = true;
                            __result.health.AddHediff(_Psylink);
                        }
                        if (storyTracker.adulthood.identifier.Contains("_Farseer"))
                        {
                            Rand.PushState();
                            __result.ChangePsylinkLevel(Math.Min(Rand.RangeInclusive(3, 5), __result.GetMaxPsylinkLevel()), false);
                            Rand.PopState();
                        }
                        else if (storyTracker.adulthood.identifier.Contains("_Warlock"))
                        {
                            Rand.PushState();
                            __result.ChangePsylinkLevel(Math.Min(Rand.RangeInclusive(1, 3), __result.GetMaxPsylinkLevel()), false);
                            Rand.PopState();
                        }
                        else if (adult)
                        {
                            Rand.PushState();
                            __result.ChangePsylinkLevel(Math.Min(Rand.RangeInclusive(0, 2), __result.GetMaxPsylinkLevel()), false);
                            Rand.PopState();
                        }
                    }
                }
            }
        }
        
    }
}
