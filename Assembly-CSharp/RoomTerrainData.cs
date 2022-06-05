using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class RoomTerrainData : ScriptableObject
{
	// Token: 0x1700113E RID: 4414
	// (get) Token: 0x06002D0C RID: 11532 RVA: 0x00098C72 File Offset: 0x00096E72
	// (set) Token: 0x06002D0D RID: 11533 RVA: 0x00098C7A File Offset: 0x00096E7A
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

	// Token: 0x06002D0E RID: 11534 RVA: 0x00098C84 File Offset: 0x00096E84
	public void Initialise(BaseRoom room)
	{
		this.Terrain = new List<TerrainData>();
		foreach (Ferr2DT_PathTerrain ferr2DTerrain in room.TerrainManager.Platforms)
		{
			this.Terrain.Add(new TerrainData(ferr2DTerrain));
		}
	}

	// Token: 0x04002433 RID: 9267
	[SerializeField]
	private List<TerrainData> m_terrain;
}
