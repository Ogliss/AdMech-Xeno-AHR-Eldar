using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.CompProperties_SoulStoneSlot
    public class CompProperties_SoulStoneSlot: CompProperties
    {
        public CompProperties_SoulStoneSlot()
        {
            this.compClass = typeof(CompSoulStoneSlot);
        }
    }

    // 
    public class CompSoulStoneSlot : ThingComp, IThingHolder
    {
        public CompSoulStoneSlot()
        {
            this.innerContainer = new ThingOwner<Thing>(this);
        }
        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.innerContainer;
        }

        public SoulStone SoulStone
        {
            get
            {
                if (this.innerContainer.InnerListForReading.Count <= 0)
                {
                    return null;
                }
                SoulStone stone = this.innerContainer.InnerListForReading[0] as SoulStone;
                if (stone != null)
                {
                    return stone;
                }
                return null;
            }
            set
            {
                if (SoulStone != null)
                {
                    return;
                }
                this.innerContainer.TryAddOrTransfer(value);

                SoulStone stone = value;
                if (this.parent is Pawn p && p.drafter == null)
                {
                    p.drafter = new Pawn_DraftController(p);
                }
                
            }
        }
        public Soul Soul
        {
            get
            {
                if (SoulStone == null)
                {
                    return null;
                }
                if (SoulStone.soul == null)
                {
                    return null;
                }
                return this.SoulStone.soul;
            }
        }

        public override void PostPostMake()
        {
        //    Log.Message($"CompSoulStoneSlot PostPostMake");
            
            if (this.parent is Pawn pawn && pawn.relations == null)
            {
                pawn.relations = new Pawn_RelationsTracker(pawn);
            }
            
            base.PostPostMake();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
        //    Log.Message($"CompSoulStoneSlot PostSpawnSetup");
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public override string TransformLabel(string label)
        {
            return base.TransformLabel(label);
        }

        public override void PostExposeData()
        {
            Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
            {
                this
            });
            base.PostExposeData();
        }
        private ThingOwner<Thing> innerContainer;
    }

}
