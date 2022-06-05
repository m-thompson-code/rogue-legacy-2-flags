using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CCF RID: 3279
public class ProjectileSizeComparer : IComparer<Projectile_RL>
{
	// Token: 0x06005D8D RID: 23949 RVA: 0x0015AB20 File Offset: 0x00158D20
	public int Compare(Projectile_RL a, Projectile_RL b)
	{
		Bounds bounds = EnemyUtility.GetBounds(a.gameObject);
		Bounds bounds2 = EnemyUtility.GetBounds(b.gameObject);
		float num = bounds.size.x * bounds.size.y;
		float value = bounds2.size.x * bounds2.size.y;
		return num.CompareTo(value);
	}
}
