using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class EldarExtensions
    {
        public static bool isEldar(this Pawn pawn)
        {
            return pawn.def == EldarThingDefOf.OG_Alien_Eldar;
        }

        public static bool isWraithConstruct(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == EldarThingDefOf.OG_Flesh_Construct_Eldar;
        }

        public static CompSoulStone SlotLoadable(this Pawn pawn)
        {
            return pawn.TryGetCompFast<CompSoulStone>();
        }
    }
    
}
