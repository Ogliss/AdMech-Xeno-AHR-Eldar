using Verse;
using HarmonyLib;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AlienRaceUtility), "RaceAgeSkillFactor")]
    public static class AlienRaceUtility_RaceAgeSkillFactor_Aeldari_Patch
    {
        [HarmonyPostfix]
        public static SimpleCurve Postfix(SimpleCurve __result, Pawn pawn)
        {
            if (pawn != null && pawn.RaceProps.Humanlike && pawn.isAeldari())
            {
            //    if (AMAMod.Dev) Log.Message($"{pawn.Name} Using Aeldari AgeSkillMaxFactorCurve");
                return AgeSkillFactor;
            }
            return __result;
        }

        private static readonly SimpleCurve AgeSkillFactor = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0f),
                true
            },
            {
                new CurvePoint(20f, 0.75f),
                true
            },
            {
                new CurvePoint(70f, 1f),
                true
            }
        };
    }
}
