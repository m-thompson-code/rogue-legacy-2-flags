using System;
using UnityEngine;

// Token: 0x02000B66 RID: 2918
[Serializable]
public class SkyBiomeArtData
{
	// Token: 0x17001DA4 RID: 7588
	// (get) Token: 0x060058BA RID: 22714 RVA: 0x0003043B File Offset: 0x0002E63B
	// (set) Token: 0x060058BB RID: 22715 RVA: 0x00030443 File Offset: 0x0002E643
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

	// Token: 0x17001DA5 RID: 7589
	// (get) Token: 0x060058BC RID: 22716 RVA: 0x0003044C File Offset: 0x0002E64C
	// (set) Token: 0x060058BD RID: 22717 RVA: 0x00030454 File Offset: 0x0002E654
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

	// Token: 0x04004173 RID: 16755
	[SerializeField]
	private GameObject m_skyPrefab;

	// Token: 0x04004174 RID: 16756
	[SerializeField]
	private Vector2Int m_dimensions;
}
