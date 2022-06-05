using System;
using UnityEngine;

// Token: 0x02000B62 RID: 2914
[Serializable]
public class WeatherBiomeArtData
{
	// Token: 0x17001D91 RID: 7569
	// (get) Token: 0x06005899 RID: 22681 RVA: 0x00030265 File Offset: 0x0002E465
	// (set) Token: 0x0600589A RID: 22682 RVA: 0x0003026D File Offset: 0x0002E46D
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

	// Token: 0x0400416B RID: 16747
	[SerializeField]
	private Weather[] m_weatherPrefabArray;
}
