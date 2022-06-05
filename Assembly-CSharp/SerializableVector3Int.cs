using System;
using UnityEngine;

// Token: 0x02000CE2 RID: 3298
[Serializable]
public struct SerializableVector3Int
{
	// Token: 0x06005DFA RID: 24058 RVA: 0x00033BDA File Offset: 0x00031DDA
	public SerializableVector3Int(int rX, int rY, int rZ)
	{
		this.x = rX;
		this.y = rY;
		this.z = rZ;
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x00033BF1 File Offset: 0x00031DF1
	public override string ToString()
	{
		return string.Format("[{0}, {1}, {2}]", this.x, this.y, this.z);
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x00033C1E File Offset: 0x00031E1E
	public static implicit operator Vector3Int(SerializableVector3Int rValue)
	{
		return new Vector3Int(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x00033C37 File Offset: 0x00031E37
	public static implicit operator SerializableVector3Int(Vector3Int rValue)
	{
		return new SerializableVector3Int(rValue.x, rValue.y, rValue.z);
	}

	// Token: 0x04004D34 RID: 19764
	public int x;

	// Token: 0x04004D35 RID: 19765
	public int y;

	// Token: 0x04004D36 RID: 19766
	public int z;
}
