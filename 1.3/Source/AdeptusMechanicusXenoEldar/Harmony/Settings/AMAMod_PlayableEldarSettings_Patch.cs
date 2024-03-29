﻿using RimWorld;
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
        public static void ModsLoaded(ref string __result)
        {
            __result += ", " + "AdeptusMechanicus.Eldar.ModName".Translate();
        }
    }
    [HarmonyPatch(typeof(AMAMod), "EldarSettings")]
    public static class AMAMod_PlayableEldarSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static AMAMod mod = AMAMod.Instance;
        private static float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool Xenobiologis => AdeptusIntergrationUtility.enabled_MagosXenobiologis;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => (Xenobiologis && settings.ShowAllowedRaceSettings && ShowXB) || (!Xenobiologis && settings.ShowEldar);
        private static bool Setting => ShowRaces && settings.ShowEldar;

        private static int Options = 3;
        private static float RaceSettings => mod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void EldarSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            string label = "AdeptusMechanicus.Xenobiologis.ShowEldar".Translate() + " Settings";
            string tooltip = string.Empty;
            if (Dev)
            {
                label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
            }

            if (!Xenobiologis)
            {
                if (!listing_Main.ButtonText(label, ref settings.ShowEldar))
                {
                    return;
                }
            }
            if (ShowRaces)
            {
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings) + inc, false, 3, 4, 0);
                if (Xenobiologis)
                {
                    listing_Race.CheckboxLabeled(label, ref settings.ShowEldar, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex);
                }
                if (settings.ShowEldar)
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowEldarCraftworld,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) && settings.AllowEldarWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarWraithguard".Translate(),
                        ref settings.AllowEldarWraithguard,
                        null,
                        !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) || !settings.AllowEldarWeapons,
                        DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) && settings.AllowEldarWeapons);
                    listing_General.NewColumn();
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                        ref settings.AllowEldarExodite,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) && settings.AllowEldarWeapons);
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowEldarHarlequinn,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) || !settings.AllowEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) && settings.AllowEldarWeapons);
                    listing_Race.EndSection(listing_General);
                    MenuLength = listing_General.CurHeight != 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength - inc;
            }
        }
    }

    /*
    [HarmonyPatch(typeof(AMAMod), "EldarSettings")]
    public static class AMAMod_PlayableEldarSettings_Patch
    {
        private static bool Xenobiologis = AdeptusIntergrationUtility.enabled_MagosXenobiologis;
        private static AMSettings settings = AMAMod.settings;
        [HarmonyPrefix, HarmonyPriority(401)]
        public static void EldarSettings_Prefix(ref AMAMod __instance, ref Listing_StandardExpanding listing_Main, Rect rect, Rect inRect, float num, float num2)
        {
            bool showRaces = settings.ShowAllowedRaceSettings || !Xenobiologis;
            bool setting = showRaces && settings.ShowEldar;
            float lineheight = (Text.LineHeight + listing_Main.verticalSpacing);
            float w = rect.width * 0.480f;
            int Options = 3;
            float RaceSettings = __instance.Length(setting, Options, lineheight, 8, showRaces ? 1 : 0); //(settings.ShowImperium ? (lineheight * 2) : (lineheight * 1)) + (settings.ShowImperium ? 10 : 0);
            float options = __instance.Length(setting, Options - 1, lineheight, 0, 0);
            if (!Xenobiologis)
            {
                if (!listing_Main.ButtonText("AdeptusMechanicus.Eldar.ModName".Translate() + " Options", ref settings.ShowEldar))
                {
                //    __instance.XenobiologisEldarMenuLength = 0;
                    return;
                }
            }
            Listing_StandardExpanding listing_Race = listing_Main.BeginSection(RaceSettings, false, 3, 4, 0);
            listing_Race.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.ShowEldar".Translate() + " Settings", ref settings.ShowEldar, null, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex);

            if (setting)
            {
                Listing_StandardExpanding listing_General = listing_Race.BeginSection(options, true);
                listing_General.ColumnWidth *= 0.488f;
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarCraftworld".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                    ref settings.AllowEldarCraftworld,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Craftworld")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarWraithguard".Translate(),
                    ref settings.AllowEldarWraithguard,
                    null,
                    !DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) || !settings.AllowEldarWeapons,
                    DefDatabase<ThingDef>.AllDefs.Any(x => x.defName.Contains("Wraithguard")) && settings.AllowEldarWeapons);
                listing_General.NewColumn();
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarExodite".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.Faction".Translate()),
                    ref settings.AllowEldarExodite,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Exodite")) && settings.AllowEldarWeapons);
                listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowEldarHarlequinn".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                    ref settings.AllowEldarHarlequinn,
                    null,
                    !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) || !settings.AllowEldarWeapons,
                    DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_Eldar_Harlequin")) && settings.AllowEldarWeapons);
                listing_Race.EndSection(listing_General);
            }
            listing_Main.EndSection(listing_Race);
        }
    }
    */
}