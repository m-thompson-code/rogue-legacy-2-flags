using System;
using UnityEngine;

// Token: 0x02000CE1 RID: 3297
[Serializable]
public struct SerializableVector2Int
{
	// Token: 0x06005DF6 RID: 24054 RVA: 0x00033B80 File Offset: 0x00031D80
	public SerializableVector2Int(int rX, int rY)
	{
		this.x = rX;
		this.y = rY;
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x00033B90 File Offset: 0x00031D90
	public override string ToString()
	{
		return string.Format("[{0}, {1}]", this.x, this.y);
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x00033BB2 File Offset: 0x00031DB2
	public static implicit operator Vector2Int(SerializableVector2Int rValue)
	{
		return new Vector2Int(rValue.x, rValue.y);
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x00033BC5 File Offset: 0x00031DC5
	public static implicit operator SerializableVector2Int(Vector2Int rValue)
	{
		return new SerializableVector2Int(rValue.x, rValue.y);
	}

	// Token: 0x04004D32 RID: 19762
	public int x;

	// Token: 0x04004D33 RID: 19763
	public int y;
}
