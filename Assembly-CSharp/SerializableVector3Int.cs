using System;
using UnityEngine;

// Token: 0x02000812 RID: 2066
[Serializable]
public struct SerializableVector3Int
{
	// Token: 0x06004446 RID: 17478 RVA: 0x000F1805 File Offset: 0x000EFA05
	public SerializableVector3Int(int rX, int rY, int rZ)
	{
		this.x = rX;
		this.y = rY;
		this.z = rZ;
	}

	// Token: 0x06004447 RID: 17479 RVA: 0x000F181C File Offset: 0x000EFA1C
	public override string ToString()
	{
		return string.Format("[{0}, {1}, {2}]", this.x, this.y, this.z);
	}

	// Token: 0x06004448 RID: 17480 RVA: 0x000F1849 File Offset: 0x000EFA49
	public static implicit operator Vector3Int(SerializableVector3Int rValue)
	{
		return new Vector3Int(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x06004449 RID: 17481 RVA: 0x000F1862 File Offset: 0x000EFA62
	public static implicit operator SerializableVector3Int(Vector3Int rValue)
	{
		return new SerializableVector3Int(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x04003A4E RID: 14926
	public int x;

	// Token: 0x04003A4F RID: 14927
	public int y;

	// Token: 0x04003A50 RID: 14928
	public int z;
}
