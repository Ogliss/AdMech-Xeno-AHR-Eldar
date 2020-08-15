using CompSlotLoadable;
using Verse;
using RimWorld;
using AdeptusMechanicus.ExtensionMethods;

namespace AdeptusMechanicus
{
    public class CompProperties_SoulStone : CompProperties_SlotLoadable
    {
        public CompProperties_SoulStone() => this.compClass = typeof(CompSoulStone);

        public bool spawnFilled = false;
        public PawnKindDef soulKind = null;
    }

    public class CompSoulStone : CompSlotLoadable.CompSlotLoadable
    {

        public bool Filled
        {
            get
            {
                return stored != null;
            }
        }

        public bool CanFill
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                if (pawn != null)
                {
                    return pawn.isEldar() && !Filled;
                }
                return false;
            }
        }

        public bool TryFillSoulStone(Pawn pawn)
        {
            if (CanFill)
            {
                stored = pawn;
            }
            return false;
        }

        public Pawn Soul
        {
            get
            {
                return stored;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look<Pawn>(ref this.stored, true, "storedSoul");
            Scribe_Deep.Look<Pawn>(ref this.attuned, true, "attunedSoul");
        }

        private Pawn stored = null;
        private Pawn attuned = null;
    }
}
