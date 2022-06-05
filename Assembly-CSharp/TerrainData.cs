using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
[Serializable]
public class TerrainData
{
	// Token: 0x06002D07 RID: 11527 RVA: 0x00098BA0 File Offset: 0x00096DA0
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

	// Token: 0x1700113C RID: 4412
	// (get) Token: 0x06002D08 RID: 11528 RVA: 0x00098C50 File Offset: 0x00096E50
	// (set) Token: 0x06002D09 RID: 11529 RVA: 0x00098C58 File Offset: 0x00096E58
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

	// Token: 0x1700113D RID: 4413
	// (get) Token: 0x06002D0A RID: 11530 RVA: 0x00098C61 File Offset: 0x00096E61
	// (set) Token: 0x06002D0B RID: 11531 RVA: 0x00098C69 File Offset: 0x00096E69
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

	// Token: 0x04002431 RID: 9265
	[SerializeField]
	private List<TerrainEdge> m_edges;

	// Token: 0x04002432 RID: 9266
	[SerializeField]
	private Vector2 m_localPosition;
}
