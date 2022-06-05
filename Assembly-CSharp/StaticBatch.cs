using System;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class StaticBatch : MonoBehaviour
{
	// Token: 0x06002491 RID: 9361 RVA: 0x00079D1E File Offset: 0x00077F1E
	private void Awake()
	{
		StaticBatchingUtility.Combine(base.gameObject);
	}
}
