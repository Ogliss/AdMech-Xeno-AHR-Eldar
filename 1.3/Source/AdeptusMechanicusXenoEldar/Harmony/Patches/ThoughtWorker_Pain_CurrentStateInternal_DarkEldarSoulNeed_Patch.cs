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
    [HarmonyPatch(typeof(ThoughtWorker_Pain), "CurrentStateInternal")]
    public class ThoughtWorker_Pain_CurrentStateInternal_DarkEldarSoulNeed_Patch
    {
        public static void Postfix(ThoughtWorker_Pain __instance, ref ThoughtState __result, Pawn p)
        {
            if (p.Map != null)
            {
                if (__result.Active)
                {
                    if (!p.Map.mapPain().painMaker.Contains(p))
                    {
                        p.Map.mapPain().painMaker.Add(p);
                    }
                //    p.Map.mapPain().totalPain += p.health.hediffSet.PainTotal;
                }
                else
                {
                    if (p.Map.mapPain().painMaker.Contains(p))
                    {
                        p.Map.mapPain().painMaker.Remove(p);
                    }
                }
            }
        }
    }
}