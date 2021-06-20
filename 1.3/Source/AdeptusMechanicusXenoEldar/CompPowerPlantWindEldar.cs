using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace AdeptusMechanicus
{
	// AdeptusMechanicus.CompPowerPlantWindEldar
	[StaticConstructorOnStartup]
	public class CompPowerPlantWindEldar : CompPowerPlantWind
	{
		public override void PostDraw()
		{
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.PowerOff);
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn)
				{
					this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.NeedsPower);
				}
			}
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantWind.BarSize,
				fillPercent = this.PowerPercent,
				filledMat = CompPowerPlantWind.WindTurbineBarFilledMat,
				unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = this.parent.TrueCenter();
			vector += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWind.VerticalBladeOffset;
			vector += this.parent.Rotation.RighthandCell.ToVector3() * CompPowerPlantWind.HorizontalBladeOffset;
			vector.y += 0.042857144f;
			float num = CompPowerPlantWind.BladeWidth * Mathf.Sin(this.spinPosition);
			if (num < 0f)
			{
				num *= -1f;
			}
			bool flag = this.spinPosition % 3.1415927f * 2f < 3.1415927f;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWindEldar.WindTurbineBladesMat, 0);
			vector.y -= 0.08571429f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWindEldar.WindTurbineBladesMat, 0);
		}

		protected static new readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);
		protected static new readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
		protected static new readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Eldar/Power/WindTurbine/WindTurbineBlades",false);
	}
}
