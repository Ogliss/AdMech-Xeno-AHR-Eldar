using System;
using System.Linq;
using AdeptusMechanicus.ExtensionMethods;
using HarmonyLib;
using RimWorld;
using Verse;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.WraithPawn
    public class WraithPawn : ArtificalPawn
    {
        public override void ExposeData()
        {
            base.ExposeData();

        }
        public CompSoulStoneSlot soulStoneSlot;

        public CompSoulStoneSlot SoulStoneSlot
        {
            get
            {
                soulStoneSlot ??= this.TryGetCompFast<CompSoulStoneSlot>();
                return soulStoneSlot;
            }
        }
        
        public SoulStone SoulStone
        {
            get
            {
                if (SoulStoneSlot.SoulStone == null)
                {
                    return null;
                }
                return this.SoulStoneSlot.SoulStone;
            }
        }
        
        public Soul Soul
        {
            get
            {
                if (SoulStoneSlot?.SoulStone == null)
                {
                    return null;
                }
                if (SoulStoneSlot?.Soul == null)
                {
                    return null;
                }
                return this.SoulStoneSlot?.Soul;
            }
        }

        public override Pawn InnerPawn
        {
            get
            {
                Thing stone = this.SoulStone;
                if (stone == null)
                {
                    return null;
                }
                SoulStone soulstone;
                if ((soulstone = (stone as SoulStone)) != null)
                {
                    if (soulstone.soul != null)
                    {
                        return soulstone.soul.originalPawn;
                    }
                }
                return base.InnerPawn;
            }
        }

        public bool Active
        {
            get
            {
                return this.Soul != null;
            }
        }

        public override Faction Faction
        {
            get
            {
                if (this.Active)
                {
                    return Soul.faction;
                }
                return base.Faction;
            }
        }

        public override bool Suspended => base.Suspended || !Active;
    }

}
