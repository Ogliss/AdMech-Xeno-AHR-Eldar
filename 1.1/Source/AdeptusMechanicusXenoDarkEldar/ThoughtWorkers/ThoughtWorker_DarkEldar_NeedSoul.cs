using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    public class ThoughtWorker_DarkEldar_NeedSoul : ThoughtWorker
	{
		NeedDef Need;
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Need_DarkEldar_Soul need = p.needs.TryGetNeed(Need) as Need_DarkEldar_Soul;
			if (need == null)
			{
				return ThoughtState.Inactive;
			}
			switch (need.CurCategory)
			{
				case SoulCategory.Empty:
					return ThoughtState.ActiveAtStage(0);
				case SoulCategory.Withering:
					return ThoughtState.ActiveAtStage(1);
				case SoulCategory.Fading:
					return ThoughtState.ActiveAtStage(2);
				case SoulCategory.Satisfied:
					return ThoughtState.Inactive;
				case SoulCategory.Energized:
					return ThoughtState.ActiveAtStage(3);
					/*
				case SoulCategory.Extreme:
					return ThoughtState.ActiveAtStage(4);
					*/
				default:
					throw new NotImplementedException();
			}
		}
	}
}
