using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020006BD RID: 1725
[Serializable]
public class AmbientLightAndFogBiomeArtData
{
	// Token: 0x170015B0 RID: 5552
	// (get) Token: 0x06003F8F RID: 16271 RVA: 0x000E238C File Offset: 0x000E058C
	// (set) Token: 0x06003F90 RID: 16272 RVA: 0x000E2394 File Offset: 0x000E0594
	public bool Fog
	{
		get
		{
			return this.m_fog;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_fog = value;
			}
		}
	}

	// Token: 0x170015B1 RID: 5553
	// (get) Token: 0x06003F91 RID: 16273 RVA: 0x000E23A4 File Offset: 0x000E05A4
	public FogMode FogMode
	{
		get
		{
			if (this.FogModeIndex == 0)
			{
				this.m_fogMode = FogMode.Linear;
			}
			else if (this.FogModeIndex == 1)
			{
				this.m_fogMode = FogMode.Exponential;
			}
			else
			{
				if (this.FogModeIndex != 2)
				{
					throw new ArgumentOutOfRangeException("FogModeIndex");
				}
				this.m_fogMode = FogMode.ExponentialSquared;
			}
			return this.m_fogMode;
		}
	}

	// Token: 0x170015B2 RID: 5554
	// (get) Token: 0x06003F92 RID: 16274 RVA: 0x000E23F7 File Offset: 0x000E05F7
	// (set) Token: 0x06003F93 RID: 16275 RVA: 0x000E23FF File Offset: 0x000E05FF
	public int FogModeIndex
	{
		get
		{
			return this.m_fogModeIndex;
		}
		set
		{
			this.m_fogModeIndex = value;
		}
	}

	// Token: 0x170015B3 RID: 5555
	// (get) Token: 0x06003F94 RID: 16276 RVA: 0x000E2408 File Offset: 0x000E0608
	// (set) Token: 0x06003F95 RID: 16277 RVA: 0x000E2410 File Offset: 0x000E0610
	public Color FogColor
	{
		get
		{
			return this.m_fogColor;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_fogColor = value;
			}
		}
	}

	// Token: 0x170015B4 RID: 5556
	// (get) Token: 0x06003F96 RID: 16278 RVA: 0x000E2420 File Offset: 0x000E0620
	// (set) Token: 0x06003F97 RID: 16279 RVA: 0x000E2428 File Offset: 0x000E0628
	public float FogDensity
	{
		get
		{
			return this.m_fogDensity;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_fogDensity = value;
			}
		}
	}

	// Token: 0x170015B5 RID: 5557
	// (get) Token: 0x06003F98 RID: 16280 RVA: 0x000E2438 File Offset: 0x000E0638
	// (set) Token: 0x06003F99 RID: 16281 RVA: 0x000E2440 File Offset: 0x000E0640
	public float FogStartDistance
	{
		get
		{
			return this.m_fogStartDistance;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_fogStartDistance = value;
			}
		}
	}

	// Token: 0x170015B6 RID: 5558
	// (get) Token: 0x06003F9A RID: 16282 RVA: 0x000E2450 File Offset: 0x000E0650
	// (set) Token: 0x06003F9B RID: 16283 RVA: 0x000E2458 File Offset: 0x000E0658
	public float FogEndDistance
	{
		get
		{
			return this.m_fogEndDistance;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_fogEndDistance = value;
			}
		}
	}

	// Token: 0x170015B7 RID: 5559
	// (get) Token: 0x06003F9C RID: 16284 RVA: 0x000E2468 File Offset: 0x000E0668
	public Color AmbientLightColor
	{
		get
		{
			return Color.black;
		}
	}

	// Token: 0x170015B8 RID: 5560
	// (get) Token: 0x06003F9D RID: 16285 RVA: 0x000E246F File Offset: 0x000E066F
	public AmbientMode AmbientMode
	{
		get
		{
			return AmbientMode.Flat;
		}
	}

	// Token: 0x170015B9 RID: 5561
	// (get) Token: 0x06003F9E RID: 16286 RVA: 0x000E2472 File Offset: 0x000E0672
	// (set) Token: 0x06003F9F RID: 16287 RVA: 0x000E247A File Offset: 0x000E067A
	public Light LightMasterPrefab
	{
		get
		{
			return this.m_lightMasterPrefab;
		}
		set
		{
			this.m_lightMasterPrefab = value;
		}
	}

	// Token: 0x04002F28 RID: 12072
	[SerializeField]
	private bool m_fog;

	// Token: 0x04002F29 RID: 12073
	[SerializeField]
	private int m_fogModeIndex;

	// Token: 0x04002F2A RID: 12074
	[SerializeField]
	private FogMode m_fogMode = FogMode.Linear;

	// Token: 0x04002F2B RID: 12075
	[SerializeField]
	private Color m_fogColor;

	// Token: 0x04002F2C RID: 12076
	[SerializeField]
	private float m_fogDensity;

	// Token: 0x04002F2D RID: 12077
	[SerializeField]
	private float m_fogStartDistance;

	// Token: 0x04002F2E RID: 12078
	[SerializeField]
	private float m_fogEndDistance;

	// Token: 0x04002F2F RID: 12079
	[SerializeField]
	private Light m_lightMasterPrefab;
}
