using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B69 RID: 2921
[Serializable]
public class BackgroundBiomeArtData
{
	// Token: 0x17001DB2 RID: 7602
	// (get) Token: 0x060058D8 RID: 22744 RVA: 0x00030551 File Offset: 0x0002E751
	// (set) Token: 0x060058D9 RID: 22745 RVA: 0x00030559 File Offset: 0x0002E759
	public List<Background> Backgrounds
	{
		get
		{
			return this.m_backgrounds;
		}
		set
		{
			this.m_backgrounds = value;
		}
	}

	// Token: 0x17001DB3 RID: 7603
	// (get) Token: 0x060058DA RID: 22746 RVA: 0x00030562 File Offset: 0x0002E762
	public bool TileNormally
	{
		get
		{
			return this.m_tileNormally;
		}
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x0003056A File Offset: 0x0002E76A
	public void AddBackground()
	{
		if (this.Backgrounds == null)
		{
			this.Backgrounds = new List<Background>();
		}
		this.Backgrounds.Add(null);
	}

	// Token: 0x0400417F RID: 16767
	[SerializeField]
	private List<Background> m_backgrounds;

	// Token: 0x04004180 RID: 16768
	[SerializeField]
	private bool m_tileNormally = true;
}
