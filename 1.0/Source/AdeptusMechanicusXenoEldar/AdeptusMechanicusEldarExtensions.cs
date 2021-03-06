﻿using System;
using System.Collections.Generic;
using System.Linq;
using AbilityUser;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class AdeptusMechanicusEldarExtensions
    {
        public static bool isEldar(this Pawn pawn)
        {
            return pawn.def == OGEldarThingDefOf.OG_Alien_Eldar;
        }

        public static bool isWraithConstruct(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == OGEldarThingDefOf.OG_Flesh_Construct_Eldar;
        }

        public static CompSoulStone SlotLoadable(this Pawn pawn)
        {
            return pawn.TryGetComp<CompSoulStone>();
        }
    }
    
}
