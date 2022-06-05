using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007F5 RID: 2037
public class EnemySizeComparer : IComparer<EnemyController>
{
	// Token: 0x060043B7 RID: 17335 RVA: 0x000ECBC4 File Offset: 0x000EADC4
	public int Compare(EnemyController a, EnemyController b)
	{
		Bounds bounds = EnemyUtility.GetBounds(a.gameObject);
		Bounds bounds2 = EnemyUtility.GetBounds(b.gameObject);
		float num = bounds.size.x * bounds.size.y;
		float value = bounds2.size.x * bounds2.size.y;
		return num.CompareTo(value);
	}
}
