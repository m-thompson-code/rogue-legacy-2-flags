using System;
using UnityEngine;

// Token: 0x0200048D RID: 1165
public class OneWay : MonoBehaviour, IPlayHitEffect
{
	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x00091B36 File Offset: 0x0008FD36
	public bool PlayDirectionalHitEffect
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x00091B39 File Offset: 0x0008FD39
	public bool PlayHitEffect
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x00091B3C File Offset: 0x0008FD3C
	public string EffectNameOverride
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x00091B40 File Offset: 0x0008FD40
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

	// Token: 0x06002AFB RID: 11003 RVA: 0x00091C00 File Offset: 0x0008FE00
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
