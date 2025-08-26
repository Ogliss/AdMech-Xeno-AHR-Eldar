using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;
using System;
using Verse.Sound;
using UnityEngine;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.JobDriver_InstallSpiritStone
    public class JobDriver_InstallSpiritStone : JobDriver
    {
        private Corpse Corpse
        {
            get
            {
                return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
            }
        }

        private Thing Item
        {
            get
            {
                return this.job.GetTarget(TargetIndex.B).Thing;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(600, TargetIndex.None);
            toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            toil.tickAction = delegate ()
            {
                CompUsable compUsable = this.Item.TryGetComp<CompUsable>();
                if (compUsable != null && this.warmupMote == null && compUsable.Props.warmupMote != null)
                {
                    this.warmupMote = MoteMaker.MakeAttachedOverlay(this.Corpse, compUsable.Props.warmupMote, Vector3.zero, 1f, -1f);
                }
                Mote mote = this.warmupMote;
                if (mote == null)
                {
                    return;
                }
                mote.Maintain();
            };
            yield return toil;
            yield return Toils_General.Do(new Action(this.InstallSpiritStone));
            yield break;
        }

        private void InstallSpiritStone()
        {
            Pawn innerPawn = this.Corpse.InnerPawn;
            CompTargetEffect_InstallSpiritStone compTargetEffect_InstallSpiritStone = this.Item.TryGetComp<CompTargetEffect_InstallSpiritStone>();
            CompSoulStoneSlot slot = innerPawn.TryGetComp<CompSoulStoneSlot>();
            bool flag = true;
            slot.SoulStone = Item as SoulStone;
            if (slot.SoulStone == null)
            {
            }
            else if (!ResurrectionUtility.TryResurrect(innerPawn, null))
            {
                flag = false;
            }
            if (flag)
            {
                SoundDefOf.MechSerumUsed.PlayOneShot(SoundInfo.InMap(innerPawn, MaintenanceType.None));
                Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
                if (compTargetEffect_InstallSpiritStone.Props.moteDef != null)
                {
                    MoteMaker.MakeAttachedOverlay(innerPawn, compTargetEffect_InstallSpiritStone.Props.moteDef, Vector3.zero, 1f, -1f);
                }
                if (compTargetEffect_InstallSpiritStone.Props.addsHediff != null)
                {
                    innerPawn.health.AddHediff(compTargetEffect_InstallSpiritStone.Props.addsHediff, null, null, null);
                }
            }
        //    this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
        }

        private const TargetIndex CorpseInd = TargetIndex.A;
        private const TargetIndex ItemInd = TargetIndex.B;
        private const int DurationTicks = 600;
        private Mote warmupMote;
    }
}
