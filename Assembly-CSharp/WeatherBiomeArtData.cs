using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
[Serializable]
public class WeatherBiomeArtData
{
	// Token: 0x17001599 RID: 5529
	// (get) Token: 0x06003F62 RID: 16226 RVA: 0x000E211B File Offset: 0x000E031B
	// (set) Token: 0x06003F63 RID: 16227 RVA: 0x000E2123 File Offset: 0x000E0323
	public Weather[] WeatherPrefabArray
	{
		get
		{
			return this.m_weatherPrefabArray;
		}
		set
		{
			this.m_weatherPrefabArray = value;
		}
	}

	// Token: 0x04002F1C RID: 12060
	[SerializeField]
	private Weather[] m_weatherPrefabArray;
}
