using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.CompProperties_TargetableDef
    public class CompProperties_TargetableDef : CompProperties_Targetable
    {
        public CompProperties_TargetableDef()
        {
            this.compClass = typeof(CompTargetable_TargetableDef);
        }
        public List<ThingDef> installedOnDefs = new List<ThingDef>();
    }
    // AdeptusMechanicus.CompTargetable_TargetableDef
    public class CompTargetable_TargetableDef: CompTargetable_SingleCorpse
    {
        public new CompProperties_TargetableDef Props => base.Props as CompProperties_TargetableDef;
        // need to add hediff
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            bool wraithguard = target.Thing is Corpse c && Props.installedOnDefs.Contains(c.InnerPawn.def) && !c.InnerPawn.health.hediffSet.GetNotMissingParts(depth: BodyPartDepth.Inside).Any(x => x.def.defName == "ArtificialBrain");
            bool infinityCircuit = target.Thing is Building b && Props.installedOnDefs.Contains(b.def);
            return  base.ValidateTarget(target.Thing, showMessages);
        }

        public SoulStone SoulStone => this.parent as SoulStone;

        public override AcceptanceReport CanBeUsedBy(Pawn p)
        {
            return base.CanBeUsedBy(p) && SoulStone?.soul != null;
        }
    }
    public class CompBiocodableOverTime : CompBiocodable
    {

    }
}
