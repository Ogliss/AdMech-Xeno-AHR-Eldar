using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class DarkEldarExtensions
    {
        public static bool isDarkEldar(this Pawn pawn)
        {
            return pawn.def == AdeptusThingDefOf.OG_Alien_DarkEldar;
        }

        public static DarkEldar_Pain_Tracker mapPain(this Map map)
        {
            return map.GetComponent<DarkEldar_Pain_Tracker>();
        }

    }
    
}
