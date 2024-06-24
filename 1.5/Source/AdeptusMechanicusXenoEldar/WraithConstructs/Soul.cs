using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

namespace AdeptusMechanicus
{
    public class Soul : IExposable
    {
        public Soul()
        {

        }

        public void ExposeData()
        {
            Scribe_Values.Look<string>(ref this.ownerName, "ownerName");
            Scribe_Values.Look<float>(ref this.pawnMarketValue, "pawnMarketValue", 0f);
            Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
            Scribe_Values.Look<string>(ref this.title, "title");
            Scribe_References.Look<Pawn>(ref this.originalPawn, "originalPawn", true);
            Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Undefined, Array.Empty<object>());
            Scribe_Collections.Look<Trait>(ref this.traits, "traits", LookMode.Undefined, Array.Empty<object>());
            Scribe_Defs.Look<RimWorld.BackstoryDef>(ref this.childhood, "childhood");
            Scribe_Defs.Look<RimWorld.BackstoryDef>(ref this.adulthood, "adulthood");
            Scribe_Collections.Look<DirectPawnRelation>(ref this.relations, "relation", LookMode.Undefined, Array.Empty<object>());
            Scribe_Collections.Look<Pawn>(ref this.relatedPawns, "relatedPawns", LookMode.Reference);
            Scribe_Collections.Look<WorkTypeDef, int>(ref this.priorities, "priorities", LookMode.Undefined, LookMode.Undefined);
            Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
            Scribe_References.Look<Faction>(ref this.faction, "faction", true);
            Scribe_Values.Look<bool>(ref this.isPrisoner, "isPrisoner", false);
            if (ModsConfig.IdeologyActive)
            {
                Scribe_References.Look<Ideo>(ref this.ideo, "ideo", true);
                Scribe_Values.Look<Color?>(ref this.favColor, "favoriteColor", null, false);
                Scribe_Values.Look<float>(ref this.certainty, "certainty", 0f, false);
                Scribe_References.Look<Precept_RoleSingle>(ref this.precept_RoleSingle, "precept_RoleSingle", false);
                Scribe_References.Look<Precept_RoleMulti>(ref this.precept_RoleMulti, "precept_RoleMulti", false);
                Scribe_Values.Look<bool>(ref this.isSlave, "isSlave", false, false);
            }
        }

        public string Label
        {
            get
            {
                string result;
                if (this.ownerName == null)
                    result = "";
                else
                    result = this.ownerName + "'s Soul";
                return result;
            }
        }

        public string DescriptionDetailed
        {
            get
            {
                return this.CustomDescription();
            }
        }

        public string CustomDescription()
        {
            string result;
            if (this.ownerName == null)
            {
                result = "No Soul";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                if (this.ownerName != null)
                {
                    stringBuilder.AppendLine("Name: " + this.ownerName);
                    if (this.title != null)
                    {
                        stringBuilder.Append(" Title: " + this.title);
                    }
                }
                if (this.faction != null)
                {
                    StringBuilder stringBuilder2 = stringBuilder;
                    string str = "Faction: ";
                    Faction faction = this.faction;
                    stringBuilder2.AppendLine(str + ((faction != null) ? faction.ToString() : null));
                }
                if (this.childhood != null)
                {
                    stringBuilder.AppendLine("Childhood: " + this.childhood.title.CapitalizeFirst());
                }
                if (this.adulthood != null)
                {
                    stringBuilder.AppendLine("Adulthood: " + this.adulthood.title.CapitalizeFirst());
                }
                bool ideologyActive = ModsConfig.IdeologyActive;
                if (ideologyActive)
                {
                    stringBuilder.AppendLine("Ideology: " + this.ideo.name);
                }
                stringBuilder.AppendLine();
                if (this.skills != null && this.skills.Count > 0)
                {
                    stringBuilder.AppendLine("Skill");
                    foreach (SkillRecord skillRecord in this.skills)
                    {
                        if (skillRecord.passion >= Passion.Major)
                        {
                            stringBuilder.AppendLine(string.Concat(new string[]
                            {
                                "<color=#FF8000>",
                                skillRecord.def.label.CapitalizeFirst(),
                                "</color>**: ",
                                skillRecord.levelInt.ToString(),
                                " (",
                                skillRecord.xpSinceLastLevel.ToString(),
                                "xp)"
                            }));
                        }
                        if (skillRecord.passion == Passion.Minor)
                        {
                            stringBuilder.AppendLine(string.Concat(new string[]
                            {
                                "<color=#EDEA72>",
                                skillRecord.def.label.CapitalizeFirst(),
                                "</color>*: ",
                                skillRecord.levelInt.ToString(),
                                " (",
                                skillRecord.xpSinceLastLevel.ToString(),
                                "xp)"
                            }));
                        }
                        if (skillRecord.passion == Passion.None)
                        {
                            stringBuilder.AppendLine(string.Concat(new string[]
                            {
                                "<color=#FFFFFF>",
                                skillRecord.def.label.CapitalizeFirst(),
                                "</color>: ",
                                skillRecord.levelInt.ToString(),
                                " (",
                                skillRecord.xpSinceLastLevel.ToString(),
                                "xp)"
                            }));
                        }
                    }
                }
                stringBuilder.AppendLine();
                if (this.traits != null && this.traits.Count > 0)
                {
                    stringBuilder.Append("Trait: ");
                    foreach (Trait trait in this.traits)
                    {
                        stringBuilder.Append(trait.LabelCap + ",");
                    }
                }
                result = stringBuilder.ToString().TrimEndNewlines();
            }
            return result;
        }

        public string ownerName;
        public Name name;
        public string title;
        public Pawn originalPawn;
        public List<SkillRecord> skills = new List<SkillRecord>();
        public List<Trait> traits;
        public RimWorld.BackstoryDef childhood;
        public RimWorld.BackstoryDef adulthood;
        public List<DirectPawnRelation> relations;
        public HashSet<Pawn> relatedPawns;
        public Dictionary<WorkTypeDef, int> priorities;
        public DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();
        public Faction faction;
        public Ideo ideo;
        public Color? favColor;
        public float certainty;
        public Precept_RoleMulti precept_RoleMulti;
        public Precept_RoleSingle precept_RoleSingle;
        public float pawnMarketValue;
        public bool isPrisoner;
        public bool isSlave;
    }
}
