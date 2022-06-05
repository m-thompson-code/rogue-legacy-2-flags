using System;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
[Serializable]
public class TerrainEdge
{
	// Token: 0x06003DAC RID: 15788 RVA: 0x00022242 File Offset: 0x00020442
	public TerrainEdge(Vector2 start, Vector2 end, bool hasEdge)
	{
		this.Start = start;
		this.End = end;
		this.HasEdge = hasEdge;
	}

	// Token: 0x170016A0 RID: 5792
	// (get) Token: 0x06003DAD RID: 15789 RVA: 0x0002225F File Offset: 0x0002045F
	// (set) Token: 0x06003DAE RID: 15790 RVA: 0x00022267 File Offset: 0x00020467
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

	// Token: 0x170016A1 RID: 5793
	// (get) Token: 0x06003DAF RID: 15791 RVA: 0x00022270 File Offset: 0x00020470
	// (set) Token: 0x06003DB0 RID: 15792 RVA: 0x00022278 File Offset: 0x00020478
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

	// Token: 0x170016A2 RID: 5794
	// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x00022281 File Offset: 0x00020481
	// (set) Token: 0x06003DB2 RID: 15794 RVA: 0x00022289 File Offset: 0x00020489
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

	// Token: 0x04003091 RID: 12433
	[SerializeField]
	private Vector2 m_start;

	// Token: 0x04003092 RID: 12434
	[SerializeField]
	private Vector2 m_end;

	// Token: 0x04003093 RID: 12435
	[SerializeField]
	private bool m_hasEdge;
}
