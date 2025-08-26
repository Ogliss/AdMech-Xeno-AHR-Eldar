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
    //    private static bool showRaces => (settings.ShowAllowedRaceSettings && showXB) || !Xenobiologis;
        [HarmonyPostfix]
        public static void ModsLoaded(ref string __result)
        {
            __result += ", " + "AdeptusMechanicus.DarkEldar.ModName".Translate();
        }
    }

    [HarmonyPatch(typeof(AMAMod), "DarkEldarSettings")]
    public static class AMAMod_PlayableDarkEldarSettings_Patch
    {
        private static AMSettings settings = AMAMod.settings;
        private static readonly AMAMod mod = AMAMod.Instance;
        private static readonly float lineheight = AMAMod.lineheight;

        private static bool Dev => AMAMod.Dev;
        private static bool Xenobiologis => AdeptusIntergrationUtility.enabled_MagosXenobiologis;
        private static bool ShowXB => settings.ShowXenobiologisSettings;
        private static bool ShowRaces => (Xenobiologis && settings.ShowAllowedRaceSettings && ShowXB) || (!Xenobiologis && settings.ShowAeldari);
        private static bool Setting => ShowRaces && settings.ShowAeldari;

        private static int Options = 2;
        private static float RaceSettings => AMAMod.Length(Setting, Options, lineheight, 8, ShowRaces ? 1 : 0);

        public static float MainMenuLength = 0;
        public static float MenuLength = 0;
        private static float inc = 0;
        [HarmonyPrefix]
        public static void DarkEldarSettings_Prefix(ref Listing_StandardExpanding listing_Main, ref float num2)
        {
            string label = "AdeptusMechanicus.Xenobiologis.ShowDarkEldar".Translate() + " Settings";
            string tooltip = string.Empty;
            if (Dev)
            {
                label += " Main Length: " + MainMenuLength + " SubLength: " + MenuLength + " Passed: " + num2 + " Inc: " + inc;
            }
            if (!Xenobiologis)
            {
                if (!listing_Main.ButtonText(label, ref settings.ShowAeldari))
                {
                    return;
                }
            }
            if (ShowRaces)
            {
                Listing_StandardExpanding listing_Race = listing_Main.BeginSection((num2 != 0 ? num2 : RaceSettings), false, 3, 4, 0);
                if (Xenobiologis)
                {
                    listing_Race.CheckboxLabeled(label, ref settings.ShowAeldari, Dev, ref inc, tooltip, false, true, ArmouryMain.collapseTex, ArmouryMain.expandTex);
                }
                if (settings.ShowAeldari)
                {
                    Listing_StandardExpanding listing_General = listing_Race.BeginSection(MenuLength, true);
                    listing_General.ColumnWidth *= 0.488f;
                    listing_General.CheckboxLabeled("AdeptusMechanicus.Xenobiologis.AllowDarkEldar".Translate() + (!DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_DarkEldar")) ? "AdeptusMechanicus.Xenobiologis.NotYetAvailable".Translate() : "AdeptusMechanicus.Xenobiologis.HiddenFaction".Translate()),
                        ref settings.AllowDarkEldar,
                        null,
                        !DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_DarkEldar")) || !settings.AllowDarkEldarWeapons,
                        DefDatabase<FactionDef>.AllDefs.Any(x => x.defName.Contains("OG_DarkEldar")) && settings.AllowDarkEldarWeapons);
                    listing_General.NewColumn();
                    listing_Race.EndSection(listing_General);
                    MenuLength = listing_General.CurHeight != 0 ? listing_General.CurHeight : listing_General.MaxColumnHeightSeen;
                }
                listing_Main.EndSection(listing_Race);
                MainMenuLength = listing_Race.CurHeight;
                num2 = MainMenuLength;
            }
        }
    }

}