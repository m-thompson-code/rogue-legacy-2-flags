using System;
using System.Collections.Generic;

// Token: 0x02000B04 RID: 2820
public class RNGWeightTableEntryKeyEqualityComparer : IEqualityComparer<RNGWeightTableEntryKey>
{
	// Token: 0x06005486 RID: 21638 RVA: 0x001400D8 File Offset: 0x0013E2D8
	public bool Equals(RNGWeightTableEntryKey x, RNGWeightTableEntryKey y)
	{
		bool flag = x.ID == y.ID;
		bool flag2 = x.IsMirrored == y.IsMirrored;
		return flag && flag2;
	}

	// Token: 0x06005487 RID: 21639 RVA: 0x00140108 File Offset: 0x0013E308
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
