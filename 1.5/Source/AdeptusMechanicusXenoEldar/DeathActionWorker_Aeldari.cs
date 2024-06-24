using Verse;
using Verse.AI.Group;

namespace AdeptusMechanicus
{
    // AdeptusMechanicus.DeathActionWorker_Aeldari
    public class DeathActionWorker_Aeldari : DeathActionWorker_Simple
    {
        public override void PawnDied(Corpse corpse, Lord prevLord)
        {
            if (corpse.InnerPawn != null)
            {
                Pawn p = corpse.InnerPawn;
                if (!p.EquippedWornOrInventoryThings.EnumerableNullOrEmpty())
                {
                    SoulStone stone = (SoulStone)p.EquippedWornOrInventoryThings.FirstOrFallback(x => x.GetType() == typeof(SoulStone));
                    if (stone != null)
                    {
                        Log.Message($"Aeldari pawn: {p.Name} died with a soulstone");
                        stone.soul = SoulUtility.StorePawnSoulIn(p);
                    }
                }
            }
            base.PawnDied(corpse, prevLord);
        }
    }
}