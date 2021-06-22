using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using Verse;

namespace AdeptusMechanicus
{
    // Token: 0x0200081F RID: 2079
    public class ThoughtWorker_Eldar_NeedSoulstone : ThoughtWorker
	{
		// Token: 0x0600353E RID: 13630 RVA: 0x00124F04 File Offset: 0x00123104
		public override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.isEldar())
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.Inactive;
		}
	}
}
