using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System;
using UnityEngine;
using AdeptusMechanicus.settings;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus.HarmonyInstance
{
    [HarmonyPatch(typeof(AMAMod), "ModLoaded")]
    public static class AMAMod_SettingsCategory_Patch
    {
        [HarmonyPostfix]
        public static void ModsLoaded(ref AMAMod __instance, ref string __result)
        {
            __result += ", " + "AME_ModName".Translate();
        }
    }

    [HarmonyPatch(typeof(AMAMod), "EldarSettings")]
    public static class AMMod_PlayableEldarSettings_Patch
    {
        [HarmonyPrefix]
        public static void EldarSettings_Prefix(ref AMAMod __instance, ref Listing_Standard listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            AMSettings settings = AMAMod.settings;
            bool showRaces = settings.ShowAllowedRaceSettings;
            bool setting = settings.ShowAllowedRaceSettings && settings.ShowEldar;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 3;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);
            Listing_Standard listing_Race;
            if (AccessTools.GetMethodNames(typeof(Listing_Standard)).Contains("BeginSection_NewTemp"))
            {
                listing_Race = ArmouryMain.BeginSection_OnePointTwo(ref listing_Main, RaceSettings);
            }
            else
            {
                listing_Race = ArmouryMain.BeginSection_OnePointOne(ref listing_Main, RaceSettings);
            }
            listing_Race.CheckboxLabeled("AMXB_ShowEldar".Translate() + " Settings" + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") ? " Menu Length: " + __instance.XenobiologisEldarMenuLength : "") + (Prefs.DevMode && SteamUtility.SteamPersonaName.Contains("Ogliss") && setting ? " options length: " + options : ""), ref settings.ShowEldar, null, false, true);

            if (setting)
            {
                Listing_Standard listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AMXB_AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()),
                    ref settings.AllowEldarCraftworld,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowEldarWraithguard".Translate(),
                    ref settings.AllowEldarWraithguard,
                    null,
                    !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) || !settings.AllowEldarWeapons,
                    DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) && settings.AllowEldarWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AMXB_AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_Faction".Translate()),
                    ref settings.AllowEldarExodite,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AMXB_AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AMXB_NotYetAvailable".Translate() : "AMXB_HiddenFaction".Translate()),
                    ref settings.AllowEldarHarlequinn,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) && settings.AllowEldarWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
            __instance.XenobiologisEldarMenuLength = listing_Race.CurHeight;
        }
    }

}