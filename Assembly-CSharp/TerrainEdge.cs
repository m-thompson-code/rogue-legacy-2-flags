using System;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
[Serializable]
public class TerrainEdge
{
	// Token: 0x06002D00 RID: 11520 RVA: 0x00098B4D File Offset: 0x00096D4D
	public TerrainEdge(Vector2 start, Vector2 end, bool hasEdge)
	{
		this.Start = start;
		this.End = end;
		this.HasEdge = hasEdge;
	}

	// Token: 0x17001139 RID: 4409
	// (get) Token: 0x06002D01 RID: 11521 RVA: 0x00098B6A File Offset: 0x00096D6A
	// (set) Token: 0x06002D02 RID: 11522 RVA: 0x00098B72 File Offset: 0x00096D72
	public Vector2 Start
	{
		get
		{
			return this.m_start;
		}
		private set
		{
			this.m_start = value;
		}
	}

	// Token: 0x1700113A RID: 4410
	// (get) Token: 0x06002D03 RID: 11523 RVA: 0x00098B7B File Offset: 0x00096D7B
	// (set) Token: 0x06002D04 RID: 11524 RVA: 0x00098B83 File Offset: 0x00096D83
	public Vector2 End
	{
		get
		{
			return this.m_end;
		}
		private set
		{
			this.m_end = value;
		}
	}

	// Token: 0x1700113B RID: 4411
	// (get) Token: 0x06002D05 RID: 11525 RVA: 0x00098B8C File Offset: 0x00096D8C
	// (set) Token: 0x06002D06 RID: 11526 RVA: 0x00098B94 File Offset: 0x00096D94
	public bool HasEdge
	{
		get
		{
			return this.m_hasEdge;
		}
		private set
		{
			this.m_hasEdge = value;
		}
	}

	// Token: 0x0400242E RID: 9262
	[SerializeField]
	private Vector2 m_start;

	// Token: 0x0400242F RID: 9263
	[SerializeField]
	private Vector2 m_end;

	// Token: 0x04002430 RID: 9264
	[SerializeField]
	private bool m_hasEdge;
}
