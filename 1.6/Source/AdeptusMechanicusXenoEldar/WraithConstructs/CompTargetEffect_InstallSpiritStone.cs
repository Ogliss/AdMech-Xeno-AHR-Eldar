using Verse;
using RimWorld;
using Verse.AI;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.CompProperties_InstallSpiritStone
    public class CompProperties_InstallSpiritStone : CompProperties
    {
        public CompProperties_InstallSpiritStone()
        {
            this.compClass = typeof(CompTargetEffect_InstallSpiritStone);
        }

        public ThingDef moteDef;

        public bool withSideEffects = true;

        public HediffDef addsHediff;
    }

    // AdeptusMechanicus.CompTargetEffect_InstallSpiritStone
    public class CompTargetEffect_InstallSpiritStone : CompTargetEffect
    {
        public CompProperties_InstallSpiritStone Props
        {
            get
            {
                return (CompProperties_InstallSpiritStone)this.props;
            }
        }

        public override void DoEffectOn(Pawn user, Thing target)
        {
            if (!user.IsColonistPlayerControlled)
            {
                return;
            }
            Job job = JobMaker.MakeJob(AdeptusJobDefOf.OGE_InstallSpiritStone, target, this.parent);
            job.count = 1;
            job.playerForced = true;
            user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
        }
    }
}
