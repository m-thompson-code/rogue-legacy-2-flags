using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B68 RID: 2920
[Serializable]
public class AmbientLightAndFogBiomeArtData
{
	// Token: 0x17001DA8 RID: 7592
	// (get) Token: 0x060058C6 RID: 22726 RVA: 0x000304A1 File Offset: 0x0002E6A1
	// (set) Token: 0x060058C7 RID: 22727 RVA: 0x000304A9 File Offset: 0x0002E6A9
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

	// Token: 0x17001DA9 RID: 7593
	// (get) Token: 0x060058C8 RID: 22728 RVA: 0x00152350 File Offset: 0x00150550
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

	// Token: 0x17001DAA RID: 7594
	// (get) Token: 0x060058C9 RID: 22729 RVA: 0x000304B9 File Offset: 0x0002E6B9
	// (set) Token: 0x060058CA RID: 22730 RVA: 0x000304C1 File Offset: 0x0002E6C1
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

	// Token: 0x17001DAB RID: 7595
	// (get) Token: 0x060058CB RID: 22731 RVA: 0x000304CA File Offset: 0x0002E6CA
	// (set) Token: 0x060058CC RID: 22732 RVA: 0x000304D2 File Offset: 0x0002E6D2
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

	// Token: 0x17001DAC RID: 7596
	// (get) Token: 0x060058CD RID: 22733 RVA: 0x000304E2 File Offset: 0x0002E6E2
	// (set) Token: 0x060058CE RID: 22734 RVA: 0x000304EA File Offset: 0x0002E6EA
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

	// Token: 0x17001DAD RID: 7597
	// (get) Token: 0x060058CF RID: 22735 RVA: 0x000304FA File Offset: 0x0002E6FA
	// (set) Token: 0x060058D0 RID: 22736 RVA: 0x00030502 File Offset: 0x0002E702
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

	// Token: 0x17001DAE RID: 7598
	// (get) Token: 0x060058D1 RID: 22737 RVA: 0x00030512 File Offset: 0x0002E712
	// (set) Token: 0x060058D2 RID: 22738 RVA: 0x0003051A File Offset: 0x0002E71A
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

	// Token: 0x17001DAF RID: 7599
	// (get) Token: 0x060058D3 RID: 22739 RVA: 0x0003052A File Offset: 0x0002E72A
	public Color AmbientLightColor
	{
		get
		{
			return Color.black;
		}
	}

	// Token: 0x17001DB0 RID: 7600
	// (get) Token: 0x060058D4 RID: 22740 RVA: 0x000047A4 File Offset: 0x000029A4
	public AmbientMode AmbientMode
	{
		get
		{
			return AmbientMode.Flat;
		}
	}

	// Token: 0x17001DB1 RID: 7601
	// (get) Token: 0x060058D5 RID: 22741 RVA: 0x00030531 File Offset: 0x0002E731
	// (set) Token: 0x060058D6 RID: 22742 RVA: 0x00030539 File Offset: 0x0002E739
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

	// Token: 0x04004177 RID: 16759
	[SerializeField]
	private bool m_fog;

	// Token: 0x04004178 RID: 16760
	[SerializeField]
	private int m_fogModeIndex;

	// Token: 0x04004179 RID: 16761
	[SerializeField]
	private FogMode m_fogMode = FogMode.Linear;

	// Token: 0x0400417A RID: 16762
	[SerializeField]
	private Color m_fogColor;

	// Token: 0x0400417B RID: 16763
	[SerializeField]
	private float m_fogDensity;

	// Token: 0x0400417C RID: 16764
	[SerializeField]
	private float m_fogStartDistance;

	// Token: 0x0400417D RID: 16765
	[SerializeField]
	private float m_fogEndDistance;

	// Token: 0x0400417E RID: 16766
	[SerializeField]
	private Light m_lightMasterPrefab;
}
