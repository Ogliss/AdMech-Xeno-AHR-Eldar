using Verse;
using RimWorld;
using System.Text;
using System;

namespace AdeptusMechanicus
{

    // AdeptusMechanicus.SoulStone
    public class SoulStone : Apparel
    {
        public Soul soul;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<Soul>(ref this.soul, "soul", Array.Empty<object>());
        }

        public override string LabelNoCount
        {
            get
            {
                string result;
                if (this.soul == null)
                    result = $"{base.LabelNoCount} (Empty)";
                else
                    result = $"{base.LabelNoCount} ({this.soul.ownerName}'s Soul)";
                return result;
            }
        }

        public override string Label
        {
            get
            {
                string result;
                if (this.soul == null)
                    result = $"{base.Label} (Empty)";
                else
                    result = $"{base.Label} ({this.soul.ownerName}'s Soul)";
                return result;
            }
        }
        /*
        public override string GetInspectString()
        {
            return this.CustomDescription(base.GetInspectString());
        }
        */
        public override string DescriptionFlavor
        {
            get
            {
                return this.CustomDescription(base.DescriptionFlavor);
            }
        }

        public override string DescriptionDetailed
        {
            get
            {
                return this.CustomDescription(base.DescriptionDetailed);
            }
        }

        public string CustomDescription(string Desc)
        {
            string result;
            if (this.soul == null)
            {
                result = base.DescriptionDetailed;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder(Desc);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                if (this.soul.ownerName != null)
                {
                    stringBuilder.AppendLine($"Name: {this.soul.ownerName}");
                    if (this.soul.title != null)
                    {
                        stringBuilder.Append($" Title: {this.soul.title}");
                    }
                }
                if (this.soul?.faction != null)
                {
                    StringBuilder stringBuilder2 = stringBuilder;
                    string str = "Faction: ";
                    Faction faction = this.soul.faction;
                    stringBuilder2.AppendLine(str + ((faction != null) ? faction.ToString() : null));
                }
                if (this.soul?.childhood != null)
                {
                    stringBuilder.AppendLine($"Childhood: {this.soul.childhood.title.CapitalizeFirst()}");
                }
                if (this.soul?.adulthood != null)
                {
                    stringBuilder.AppendLine($"Adulthood: {this.soul.adulthood.title.CapitalizeFirst()}");
                }
                bool ideologyActive = ModsConfig.IdeologyActive;
                if (ideologyActive)
                {
                    stringBuilder.AppendLine($"Ideology: {this.soul?.ideo.name}");
                }
                stringBuilder.AppendLine();
                if (this.soul?.skills != null && this.soul?.skills.Count > 0)
                {
                    stringBuilder.AppendLine("Skill");
                    foreach (SkillRecord skillRecord in this.soul.skills)
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
                if (this.soul?.traits != null && this.soul.traits.Count > 0)
                {
                    stringBuilder.Append("Trait: ");
                    foreach (Trait trait in this.soul.traits)
                    {
                        stringBuilder.Append(trait.LabelCap + ",");
                    }
                }
                result = stringBuilder.ToString().TrimEndNewlines();
            }
            return result;
        }

    }
}
