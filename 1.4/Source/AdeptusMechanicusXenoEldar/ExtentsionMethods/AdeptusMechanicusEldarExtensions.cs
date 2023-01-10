using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus.ExtensionMethods
{

    public static class AeldariExtensions
    {
        public static bool isAeldari(this Thing pawn)
        {
            return pawn.isEldar() || pawn.isDarkEldar();
        }
        
        public static bool isAeldari(this Pawn pawn)
        {
            return pawn.isEldar() || pawn.isDarkEldar();
        }

        public static bool isEldar(this Thing pawn)
        {
            return pawn.def.isEldar();
        }
        
        public static bool isEldar(this Pawn pawn)
        {
            return pawn.def.isEldar();
        }
        
        public static bool isEldar(this ThingDef def)
        {
            return def == AdeptusThingDefOf.OG_Alien_Eldar;
        }

        public static bool isDarkEldar(this Thing pawn)
        {
            return pawn.def.isEldar();
        }
        
        public static bool isDarkEldar(this Pawn pawn)
        {
            return pawn.def.isEldar();
        }
        
        public static bool isDarkEldar(this ThingDef def)
        {
            return def == AdeptusThingDefOf.OG_Alien_DarkEldar;
        }

        public static DarkEldar_Pain_Tracker mapPain(this Map map)
        {
            return map.GetComponent<DarkEldar_Pain_Tracker>();
        }
        

        public static bool isWraithConstruct(this Pawn pawn)
        {
            return pawn.RaceProps.FleshType == AdeptusFleshTypeDefOf.OG_Flesh_Construct_Eldar;
        }

        public static CompSoulStone SlotLoadable(this Pawn pawn)
        {
            return pawn.TryGetCompFast<CompSoulStone>();
        }
    }
    
}
