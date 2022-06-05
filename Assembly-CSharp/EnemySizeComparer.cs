using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CBB RID: 3259
public class EnemySizeComparer : IComparer<EnemyController>
{
	// Token: 0x06005D40 RID: 23872 RVA: 0x0015AB20 File Offset: 0x00158D20
	public int Compare(EnemyController a, EnemyController b)
	{
		Bounds bounds = EnemyUtility.GetBounds(a.gameObject);
		Bounds bounds2 = EnemyUtility.GetBounds(b.gameObject);
		float num = bounds.size.x * bounds.size.y;
		float value = bounds2.size.x * bounds2.size.y;
		return num.CompareTo(value);
	}
}
