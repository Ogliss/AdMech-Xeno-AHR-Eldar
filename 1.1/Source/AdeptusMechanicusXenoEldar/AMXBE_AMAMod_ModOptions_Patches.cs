using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;

namespace AdeptusMechanicus
{
    [HarmonyPatch(typeof(AMAMod), "SettingsCategory")]
    public static class AME_AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void SettingsCategory_Postfix(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AME_ModName".Translate();
        }
    }
	/*
    [HarmonyPatch(typeof(AMAMod), "get_MenuLength")]
    public static class AMO_AMAMod_MenuLength_Patch
    {
        [HarmonyPostfix]
        public static void MenuLength_Postfix(ref float __result)
        {
            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}",  num2));
            __result += (AMSettings.Instance.ShowOrk ? (AdeptusIntergrationUtil.enabled_MagosXenobiologis ? 60f : 120f) : 0);

            //    Log.Message(string.Format("PreModOptions_Prefix num2: {0}", num2));
        }

    }
	*/
    [HarmonyPatch(typeof(AMMod), "EldarSettings")]
    public static class AME_AMMod_PlayableEldarSettings_Patch
    {
        [HarmonyPrefix, HarmonyPriority(401)]
        public static bool EldarSettings_Prefix(ref Listing_Standard listing_Standard, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings AMAsettings = SettingsHelper.latest;
            listing_Standard.CheckboxLabeled("AME_ModName".Translate() + " Settings", ref AMSettings.Instance.ShowEldar);
            if (AMAsettings.ShowEldar)
            {
                Rect rect2 = new Rect(rect.x, rect.y + 10, num, 120f);
                Show(ref listing_Standard, rect2, AMAsettings);
            }
            return false;
        }
        static void Show(ref Listing_Standard listing_Standard, Rect rect2, AMSettings settings)
        {
            listing_Standard.BeginSection(60f);
            Widgets.CheckboxLabeled(rect2.TopHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarCraftworld, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")));
            Widgets.CheckboxLabeled(rect2.TopHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarWraithguard".Translate(), ref settings.AllowEldarWraithguard, !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().LeftHalf().ContractedBy(4), "AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()), ref settings.AllowEldarExodite, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")));
            Widgets.CheckboxLabeled(rect2.BottomHalf().RightHalf().ContractedBy(4), "AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()), ref settings.AllowEldarHarlequinn, !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")));
            listing_Standard.EndSection(listing_Standard);
        }
    }

    /*
    [HarmonyPatch(typeof(AMAMod), "PostModOptions")]
    public static class AMO_AMAMod_PostModOptions_Patch
    {
        [HarmonyPostfix, HarmonyPriority(399)]
        public static void PostModOptions_Prefix(ref Listing_Standard listing_Standard, Rect inRect, ref float num, ref float num2)
        {
            AMSettings settings = AMSettings.Instance;
            settings.Write();
        }

    }
    */
}