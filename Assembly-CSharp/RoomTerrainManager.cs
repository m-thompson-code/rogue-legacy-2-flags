using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class RoomTerrainManager
{
	// Token: 0x1700113F RID: 4415
	// (get) Token: 0x06002D10 RID: 11536 RVA: 0x00098CFC File Offset: 0x00096EFC
	// (set) Token: 0x06002D11 RID: 11537 RVA: 0x00098D04 File Offset: 0x00096F04
	public Ferr2DT_PathTerrain[] Hazards { get; private set; }

	// Token: 0x17001140 RID: 4416
	// (get) Token: 0x06002D12 RID: 11538 RVA: 0x00098D0D File Offset: 0x00096F0D
	// (set) Token: 0x06002D13 RID: 11539 RVA: 0x00098D15 File Offset: 0x00096F15
	public List<Ferr2DT_PathTerrain> OneWays { get; private set; }

	// Token: 0x17001141 RID: 4417
	// (get) Token: 0x06002D14 RID: 11540 RVA: 0x00098D1E File Offset: 0x00096F1E
	// (set) Token: 0x06002D15 RID: 11541 RVA: 0x00098D26 File Offset: 0x00096F26
	public List<Ferr2DT_PathTerrain> Platforms { get; private set; }

	// Token: 0x17001142 RID: 4418
	// (get) Token: 0x06002D16 RID: 11542 RVA: 0x00098D2F File Offset: 0x00096F2F
	public BaseRoom Room { get; }

	// Token: 0x06002D17 RID: 11543 RVA: 0x00098D37 File Offset: 0x00096F37
	public RoomTerrainManager(BaseRoom room)
	{
		this.Room = room;
		this.Initialize();
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x00098D4C File Offset: 0x00096F4C
	public void Initialize()
	{
		this.Platforms = new List<Ferr2DT_PathTerrain>();
		this.OneWays = new List<Ferr2DT_PathTerrain>();
		RoomTerrainManager.m_hazardsHelper_STATIC.Clear();
		RoomTerrainManager.m_pathTerrainChildrenHelper_STATIC.Clear();
		this.Room.gameObject.GetComponentsInChildren<Ferr2DT_PathTerrain>(RoomTerrainManager.m_pathTerrainChildrenHelper_STATIC);
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in RoomTerrainManager.m_pathTerrainChildrenHelper_STATIC)
		{
			if (ferr2DT_PathTerrain.CompareTag("Platform"))
			{
				this.Platforms.Add(ferr2DT_PathTerrain);
			}
			else if (ferr2DT_PathTerrain.CompareTag("OneWay"))
			{
				this.OneWays.Add(ferr2DT_PathTerrain);
			}
			else if (ferr2DT_PathTerrain.CompareTag("Hazard") || ferr2DT_PathTerrain.CompareTag("TriggerHazard"))
			{
				RoomTerrainManager.m_hazardsHelper_STATIC.Add(ferr2DT_PathTerrain);
			}
		}
		this.Hazards = RoomTerrainManager.m_hazardsHelper_STATIC.ToArray();
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain2 in RoomTerrainManager.m_pathTerrainChildrenHelper_STATIC)
		{
			ferr2DT_PathTerrain2.DisableOnStartMeshBuild = true;
		}
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x00098E80 File Offset: 0x00097080
	public void AddOneWay(Ferr2DT_PathTerrain oneWay)
	{
		this.OneWays.Add(oneWay);
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x00098E90 File Offset: 0x00097090
	public void RemoveOneWay(GameObject oneWayGameObject)
	{
		for (int i = this.OneWays.Count - 1; i >= 0; i--)
		{
			if (this.OneWays[i].gameObject == oneWayGameObject)
			{
				this.OneWays.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x00098EDB File Offset: 0x000970DB
	public void AddPlatform(Ferr2DT_PathTerrain doorTerrain)
	{
		this.Platforms.Add(doorTerrain);
	}

	// Token: 0x04002438 RID: 9272
	private static List<Ferr2DT_PathTerrain> m_pathTerrainChildrenHelper_STATIC = new List<Ferr2DT_PathTerrain>();

	// Token: 0x04002439 RID: 9273
	private static List<Ferr2DT_PathTerrain> m_hazardsHelper_STATIC = new List<Ferr2DT_PathTerrain>();
}
