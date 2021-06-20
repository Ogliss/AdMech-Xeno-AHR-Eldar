using AdeptusMechanicus.ExtensionMethods;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{

    public class Need_DarkEldar_Soul : Need
	{
		public bool Soulblight
		{
			get
			{
				return this.CurCategory <= SoulCategory.Withering;
			}
		}
		public bool Soulenergize
		{
			get
			{
				return this.CurCategory >= SoulCategory.Energized;
			}
		}

		public float PercentageThreshFading
		{
			get
			{
				return 1f * 0.4f;
			}
		}

		public float PercentageThreshDiminished
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		public float SoulBetweenDiminishedAndEnergized
		{
			get
			{
				return (1f - this.PercentageThreshDiminished) * this.MaxLevel;
			}
		}

		public SoulCategory CurCategory
		{
			get
			{
				if (base.CurLevelPercentage <= 0f)
				{
					return SoulCategory.Empty;
				}
				if (base.CurLevelPercentage < 0.15f)
				{
					return SoulCategory.Withering;
				}
				if (base.CurLevelPercentage < this.PercentageThreshFading)
				{
					return SoulCategory.Fading;
				}
				if (base.CurLevelPercentage < this.PercentageThreshDiminished)
				{
					return SoulCategory.Diminished;
				}
				if (base.CurLevelPercentage < 0.9f)
				{
					return SoulCategory.Satisfied;
				}
				return SoulCategory.Energized;
			}
		}

		public float SoulFallPerTick
		{
			get
			{
				return this.SoulFallPerTickAssumingCategory(this.CurCategory, false);
			}
		}

		public int TicksUntilSoulbareWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.SoulBetweenDiminishedAndEnergized / this.SoulFallPerTickAssumingCategory(SoulCategory.Energized, false));
			}
		}

		public int TicksUntilHungryWhenFedIgnoringMalnutrition
		{
			get
			{
				return Mathf.CeilToInt(this.SoulBetweenDiminishedAndEnergized / this.SoulFallPerTickAssumingCategory(SoulCategory.Energized, true));
			}
		}

		public override int GUIChangeArrow
		{
			get
			{
                if (this.IsFrozen)
                {
					return 0;
                }
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		public override float MaxLevel
		{
			get
			{
				return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
			}
		}

		public float EnergyWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		private float DecayRate
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.HungerRateFactor * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		private float DecayRateIgnoringSoulbare
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.GetHungerRateFactor(HediffDefOf.Malnutrition) * ((this.pawn.story == null || this.pawn.story.traits == null) ? 1f : this.pawn.story.traits.HungerRateFactor) * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		public int TicksSoulbare
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonSoulbareTick);
			}
		}

		private float SoulbareSeverityPerInterval
		{
			get
			{
				Rand.PushState();
				float rand = Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674);
				Rand.PopState();
				return 0.00113333331f * Mathf.Lerp(0.8f, 1.2f, rand);
			}
		}

		public Need_DarkEldar_Soul(Pawn pawn) : base(pawn)
		{
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonSoulbareTick, "lastNonSoulbareTick", -99999, false);
		}

		public float SoulFallPerTickAssumingCategory(SoulCategory cat, bool ignoreSoulbare = false)
		{
			float num = ignoreSoulbare ? this.DecayRateIgnoringSoulbare : this.DecayRate;
			switch (cat)
			{
				case SoulCategory.Energized:
					return 2.66666666E-05f * num;
				case SoulCategory.Satisfied:
					return 2.66666666E-05f * num * 0.5f;
				case SoulCategory.Diminished:
					return 2.66666666E-05f * num * 0.25f;
				case SoulCategory.Fading:
					return 2.66666666E-05f * num * 0.125f;
				case SoulCategory.Withering:
					return 2.66666666E-05f * num * 0.075f;
				default:
					return 999f;
			}
		}

		public override void NeedInterval()
		{
			if (!this.IsFrozen)
            {
				float soulfall = -this.SoulFallPerTick * 150f;
				float curLevel = this.CurLevel;
				if (this.pawn.Map != null)
                {
					DarkEldar_Pain_Tracker mapPain = this.pawn.Map.mapPain();
                    if (mapPain != null)
					{
                        if (!mapPain.darkEldar.Contains(this.pawn))
                        {
							mapPain.darkEldar.Add(this.pawn);

						}
						if (mapPain.totalPain > 0)
						{
							mapPain.totalPain += soulfall;
							curLevel += soulfall * 2;
						}
					}
				}
				if (soulfall < 0f)
				{
					this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + soulfall, 0));
				}
				else
				{
					this.CurLevel = Mathf.Min(this.CurLevel + soulfall, 1f);
				}
				this.lastEffectiveDelta = this.CurLevel - curLevel;
				this.CurLevel -= soulfall;
			}
			if (!this.Soulblight)
			{
				this.lastNonSoulbareTick = Find.TickManager.TicksGame;
				if (this.Soulenergize)
				{
					this.lastEnergizedTick = Find.TickManager.TicksGame;
				}
			}
			if (!this.IsFrozen)
			{
                if (hediffSoulblight != null)
				{
					if (this.Soulblight)
					{
						HealthUtility.AdjustSeverity(this.pawn, hediffSoulblight, this.SoulbareSeverityPerInterval);
						return;
					}
					HealthUtility.AdjustSeverity(this.pawn, hediffSoulblight, -this.SoulbareSeverityPerInterval);
				}
				if (hediffPowerFromPain != null)
                {
					if (this.Soulenergize)
					{
						HealthUtility.AdjustSeverity(this.pawn, hediffPowerFromPain, this.SoulbareSeverityPerInterval);
						return;
					}
					HealthUtility.AdjustSeverity(this.pawn, hediffPowerFromPain, -hediffPowerFromPain.maxSeverity);
				//	HealthUtility.AdjustSeverity(this.pawn, hediffPowerFromPain, -this.SoulbareSeverityPerInterval);
				}
			}
		}

		public override void SetInitialLevel()
		{
			if (this.pawn.RaceProps.Humanlike)
			{
				base.CurLevelPercentage = 0.8f;
			}
			else
			{
				Rand.PushState();
				base.CurLevelPercentage = Rand.Range(0.5f, 0.9f);
				Rand.PopState();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonSoulbareTick = Find.TickManager.TicksGame;
			}
		}

		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				base.LabelCap,
				": ",
				base.CurLevelPercentage.ToStringPercent(),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description,
				"\n",
				"Map Pain Total: "+this.pawn.Map.mapPain().totalPain
			});
		}
        public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = int.MaxValue, float customMargin = -1, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshDiminished);
			this.threshPercents.Add(this.PercentageThreshFading);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip);
        }

		private HediffDef hediffSoulblight = DefDatabase<HediffDef>.GetNamedSilentFail("OGDE_Soulblight");
		private HediffDef hediffPowerFromPain = DefDatabase<HediffDef>.GetNamedSilentFail("OGDE_PowerFromPain"); 

		private int lastNonSoulbareTick = -99999;
		private int lastEnergizedTick = -99999;

		public const float BaseSoulFallPerTick = 2.66666666E-05f;

		private const float BaseSoulbareSeverityPerDay = 0.17f;

		private const float BaseSoulbareSeverityPerInterval = 0.00113333331f;

		private float lastEffectiveDelta;
	}
}
