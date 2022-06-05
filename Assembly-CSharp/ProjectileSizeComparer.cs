using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000807 RID: 2055
public class ProjectileSizeComparer : IComparer<Projectile_RL>
{
	// Token: 0x06004404 RID: 17412 RVA: 0x000F0B3C File Offset: 0x000EED3C
	public int Compare(Projectile_RL a, Projectile_RL b)
	{
		Bounds bounds = EnemyUtility.GetBounds(a.gameObject);
		Bounds bounds2 = EnemyUtility.GetBounds(b.gameObject);
		float num = bounds.size.x * bounds.size.y;
		float value = bounds2.size.x * bounds2.size.y;
		return num.CompareTo(value);
	}
}
