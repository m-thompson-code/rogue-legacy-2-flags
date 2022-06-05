using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
[Serializable]
public class TerrainData
{
	// Token: 0x06003DB3 RID: 15795 RVA: 0x000F9A60 File Offset: 0x000F7C60
	public TerrainData(Ferr2DT_PathTerrain ferr2DTerrain)
	{
		this.LocalPosition = ferr2DTerrain.transform.localPosition;
		this.Edges = new List<TerrainEdge>();
		for (int i = 0; i < ferr2DTerrain.PathData.Count; i++)
		{
			Vector2 start = ferr2DTerrain.PathData.Get(i);
			Vector2 end;
			if (i < ferr2DTerrain.PathData.Count - 1)
			{
				end = ferr2DTerrain.PathData.Get(i + 1);
			}
			else
			{
				end = ferr2DTerrain.PathData.Get(0);
			}
			bool hasEdge = ferr2DTerrain.PathData.GetData(i).directionOverride != 4;
			this.Edges.Add(new TerrainEdge(start, end, hasEdge));
		}
	}

	// Token: 0x170016A3 RID: 5795
	// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00022292 File Offset: 0x00020492
	// (set) Token: 0x06003DB5 RID: 15797 RVA: 0x0002229A File Offset: 0x0002049A
	public List<TerrainEdge> Edges
	{
		get
		{
			return this.m_edges;
		}
		private set
		{
			this.m_edges = value;
		}
	}

	// Token: 0x170016A4 RID: 5796
	// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x000222A3 File Offset: 0x000204A3
	// (set) Token: 0x06003DB7 RID: 15799 RVA: 0x000222AB File Offset: 0x000204AB
	public Vector2 LocalPosition
	{
		get
		{
			return this.m_localPosition;
		}
		private set
		{
			this.m_localPosition = value;
		}
	}

	// Token: 0x04003094 RID: 12436
	[SerializeField]
	private List<TerrainEdge> m_edges;

	// Token: 0x04003095 RID: 12437
	[SerializeField]
	private Vector2 m_localPosition;
}
