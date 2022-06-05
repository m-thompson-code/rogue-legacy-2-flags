using System;
using Spawn;
using UnityEngine;

// Token: 0x02000A5A RID: 2650
public class PropSpawnControllerDataLoader : MonoBehaviour
{
	// Token: 0x17001BA0 RID: 7072
	// (get) Token: 0x06005028 RID: 20520 RVA: 0x0002BCAC File Offset: 0x00029EAC
	// (set) Token: 0x06005029 RID: 20521 RVA: 0x0002BCB4 File Offset: 0x00029EB4
	public PropSpawnControllerData[] PropSpawnControllerDataArray
	{
		get
		{
			return this.m_propSpawnControllerDataArray;
		}
		set
		{
			this.m_propSpawnControllerDataArray = value;
		}
	}

	// Token: 0x04003CB0 RID: 15536
	[SerializeField]
	private PropSpawnControllerData[] m_propSpawnControllerDataArray;
}
