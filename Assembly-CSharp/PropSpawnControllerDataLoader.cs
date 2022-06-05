using System;
using Spawn;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class PropSpawnControllerDataLoader : MonoBehaviour
{
	// Token: 0x17001439 RID: 5177
	// (get) Token: 0x06003949 RID: 14665 RVA: 0x000C3225 File Offset: 0x000C1425
	// (set) Token: 0x0600394A RID: 14666 RVA: 0x000C322D File Offset: 0x000C142D
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

	// Token: 0x04002C1E RID: 11294
	[SerializeField]
	private PropSpawnControllerData[] m_propSpawnControllerDataArray;
}
