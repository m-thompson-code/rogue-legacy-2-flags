using System;
using UnityEngine;

// Token: 0x02000681 RID: 1665
public class StaticBatch : MonoBehaviour
{
	// Token: 0x060032CD RID: 13005 RVA: 0x0001BC5A File Offset: 0x00019E5A
	private void Awake()
	{
		StaticBatchingUtility.Combine(base.gameObject);
	}
}
