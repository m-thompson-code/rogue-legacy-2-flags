using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D2 RID: 2002
public class RoomTerrainData : ScriptableObject
{
	// Token: 0x170016A5 RID: 5797
	// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x000222B4 File Offset: 0x000204B4
	// (set) Token: 0x06003DB9 RID: 15801 RVA: 0x000222BC File Offset: 0x000204BC
	public List<TerrainData> Terrain
	{
		get
		{
			return this.m_terrain;
		}
		private set
		{
			this.m_terrain = value;
		}
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x000F9B10 File Offset: 0x000F7D10
	public void Initialise(BaseRoom room)
	{
		this.Terrain = new List<TerrainData>();
		foreach (Ferr2DT_PathTerrain ferr2DTerrain in room.TerrainManager.Platforms)
		{
			this.Terrain.Add(new TerrainData(ferr2DTerrain));
		}
	}

	// Token: 0x04003096 RID: 12438
	[SerializeField]
	private List<TerrainData> m_terrain;
}
