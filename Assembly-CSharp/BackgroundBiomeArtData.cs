using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006BE RID: 1726
[Serializable]
public class BackgroundBiomeArtData
{
	// Token: 0x170015BA RID: 5562
	// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x000E2492 File Offset: 0x000E0692
	// (set) Token: 0x06003FA2 RID: 16290 RVA: 0x000E249A File Offset: 0x000E069A
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

	// Token: 0x170015BB RID: 5563
	// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x000E24A3 File Offset: 0x000E06A3
	public bool TileNormally
	{
		get
		{
			return this.m_tileNormally;
		}
	}

	// Token: 0x06003FA4 RID: 16292 RVA: 0x000E24AB File Offset: 0x000E06AB
	public void AddBackground()
	{
		if (this.Backgrounds == null)
		{
			this.Backgrounds = new List<Background>();
		}
		this.Backgrounds.Add(null);
	}

	// Token: 0x04002F30 RID: 12080
	[SerializeField]
	private List<Background> m_backgrounds;

	// Token: 0x04002F31 RID: 12081
	[SerializeField]
	private bool m_tileNormally = true;
}
