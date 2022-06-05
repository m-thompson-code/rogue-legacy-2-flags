using System;
using System.Collections.Generic;

// Token: 0x02000684 RID: 1668
public class RNGWeightTableEntryKeyEqualityComparer : IEqualityComparer<RNGWeightTableEntryKey>
{
	// Token: 0x06003C27 RID: 15399 RVA: 0x000D0304 File Offset: 0x000CE504
	public bool Equals(RNGWeightTableEntryKey x, RNGWeightTableEntryKey y)
	{
		bool flag = x.ID == y.ID;
		bool flag2 = x.IsMirrored == y.IsMirrored;
		return flag && flag2;
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x000D0334 File Offset: 0x000CE534
	public int GetHashCode(RNGWeightTableEntryKey obj)
	{
		int num = 1;
		if (obj.IsMirrored)
		{
			num = 2;
		}
		return obj.ID * num;
	}
}
