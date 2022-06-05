using System;
using UnityEngine;

// Token: 0x02000811 RID: 2065
[Serializable]
public struct SerializableVector2Int
{
	// Token: 0x06004442 RID: 17474 RVA: 0x000F17AB File Offset: 0x000EF9AB
	public SerializableVector2Int(int rX, int rY)
	{
		this.x = rX;
		this.y = rY;
	}

	// Token: 0x06004443 RID: 17475 RVA: 0x000F17BB File Offset: 0x000EF9BB
	public override string ToString()
	{
		return string.Format("[{0}, {1}]", this.x, this.y);
	}

	// Token: 0x06004444 RID: 17476 RVA: 0x000F17DD File Offset: 0x000EF9DD
	public static implicit operator Vector2Int(SerializableVector2Int rValue)
	{
		return new Vector2Int(rValue.x, rValue.y);
	}

	// Token: 0x06004445 RID: 17477 RVA: 0x000F17F0 File Offset: 0x000EF9F0
	public static implicit operator SerializableVector2Int(Vector2Int rValue)
	{
		return new SerializableVector2Int(rValue.x, rValue.y);
	}

	// Token: 0x04003A4C RID: 14924
	public int x;

	// Token: 0x04003A4D RID: 14925
	public int y;
}
