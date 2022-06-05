using System;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public class OneWay : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x170015D0 RID: 5584
	// (get) Token: 0x06003B34 RID: 15156 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170015D1 RID: 5585
	// (get) Token: 0x06003B35 RID: 15157 RVA: 0x00003DA1 File Offset: 0x00001FA1
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170015D2 RID: 5586
	// (get) Token: 0x06003B36 RID: 15158 RVA: 0x0000F49B File Offset: 0x0000D69B
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x000F35FC File Offset: 0x000F17FC
	public void SetVisuals(BiomeType biomeType, BaseRoom room)
	{
		BiomeArtData biomeArtData = room.BiomeArtDataOverride;
		if (!biomeArtData)
		{
			biomeArtData = BiomeArtDataLibrary.GetArtData(biomeType);
		}
		if (!biomeArtData)
		{
			Debug.LogFormat("<color=red>[{0}] Failed to find Biome Art Data for Biome ({1})</color>", new object[]
			{
				this,
				biomeType
			});
			return;
		}
		if (biomeArtData.Ferr2DBiomeArtData == null || biomeArtData.Ferr2DBiomeArtData.OneWayMaster == null || biomeArtData.Ferr2DBiomeArtData.OneWayMaster.Ferr2DSettings == null)
		{
			Debug.LogFormat("<color=red>[{0}] Biome Art Data ({1})'s Ferr2DBiomeArtData, Ferr2DBiomeArtData.OneWayMaster or Ferr2DBiomeArtData.OneWayMaster.Ferr2DSettings is null</color>", new object[]
			{
				this,
				biomeArtData
			});
			return;
		}
		Ferr2DSettings ferr2DSettings = biomeArtData.Ferr2DBiomeArtData.OneWayMaster.Ferr2DSettings;
		if (ferr2DSettings == null)
		{
			Debug.LogFormat("Unable to find Ferr2DSettings for One Way in Biome ({0})", new object[]
			{
				biomeType
			});
			return;
		}
		this.UpdateFerr2DSettings(ferr2DSettings);
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x000F36BC File Offset: 0x000F18BC
	private void UpdateFerr2DSettings(Ferr2DSettings settings)
	{
		Ferr2DT_PathTerrain component = base.GetComponent<Ferr2DT_PathTerrain>();
		component.SetMaterial(settings.Material);
		component.fillSplit = false;
		component.fillSplitDistance = settings.GridSpacing;
		component.vertexColorType = settings.ColorType;
		component.vertexGradientDistance = settings.VertexGradientDistance;
		component.vertexGradient = settings.VertexGradient;
		component.pixelsPerUnit = settings.PixelsPerUnit;
		component.createTangents = settings.CreateTangents;
		Renderer component2 = base.gameObject.GetComponent<Renderer>();
		component2.sortingLayerID = settings.SortingLayer;
		component2.sortingOrder = settings.OrderInLayer;
	}
}
