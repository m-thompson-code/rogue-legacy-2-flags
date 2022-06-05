using System;
using UnityEngine;

// Token: 0x020006BB RID: 1723
[Serializable]
public class SkyBiomeArtData
{
	// Token: 0x170015AC RID: 5548
	// (get) Token: 0x06003F83 RID: 16259 RVA: 0x000E2316 File Offset: 0x000E0516
	// (set) Token: 0x06003F84 RID: 16260 RVA: 0x000E231E File Offset: 0x000E051E
	public GameObject SkyPrefab
	{
		get
		{
			return this.m_skyPrefab;
		}
		set
		{
			this.m_skyPrefab = value;
		}
	}

	// Token: 0x170015AD RID: 5549
	// (get) Token: 0x06003F85 RID: 16261 RVA: 0x000E2327 File Offset: 0x000E0527
	// (set) Token: 0x06003F86 RID: 16262 RVA: 0x000E232F File Offset: 0x000E052F
	public Vector2Int Dimensions
	{
		get
		{
			return this.m_dimensions;
		}
		set
		{
			this.m_dimensions = value;
		}
	}

	// Token: 0x04002F24 RID: 12068
	[SerializeField]
	private GameObject m_skyPrefab;

	// Token: 0x04002F25 RID: 12069
	[SerializeField]
	private Vector2Int m_dimensions;
}
