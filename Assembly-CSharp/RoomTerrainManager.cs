using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007D3 RID: 2003
public class RoomTerrainManager
{
	// Token: 0x170016A6 RID: 5798
	// (get) Token: 0x06003DBC RID: 15804 RVA: 0x000222C5 File Offset: 0x000204C5
	// (set) Token: 0x06003DBD RID: 15805 RVA: 0x000222CD File Offset: 0x000204CD
	public Ferr2DT_PathTerrain[] Hazards { get; private set; }

	// Token: 0x170016A7 RID: 5799
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x000222D6 File Offset: 0x000204D6
	// (set) Token: 0x06003DBF RID: 15807 RVA: 0x000222DE File Offset: 0x000204DE
	public List<Ferr2DT_PathTerrain> OneWays { get; private set; }

	// Token: 0x170016A8 RID: 5800
	// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x000222E7 File Offset: 0x000204E7
	// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x000222EF File Offset: 0x000204EF
	public List<Ferr2DT_PathTerrain> Platforms { get; private set; }

	// Token: 0x170016A9 RID: 5801
	// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x000222F8 File Offset: 0x000204F8
	public BaseRoom Room { get; }

	// Token: 0x06003DC3 RID: 15811 RVA: 0x00022300 File Offset: 0x00020500
	public RoomTerrainManager(BaseRoom room)
	{
		this.Room = room;
		this.Initialize();
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x000F9B80 File Offset: 0x000F7D80
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

	// Token: 0x06003DC5 RID: 15813 RVA: 0x00022315 File Offset: 0x00020515
	public void AddOneWay(Ferr2DT_PathTerrain oneWay)
	{
		this.OneWays.Add(oneWay);
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x000F9CB4 File Offset: 0x000F7EB4
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

	// Token: 0x06003DC7 RID: 15815 RVA: 0x00022323 File Offset: 0x00020523
	public void AddPlatform(Ferr2DT_PathTerrain doorTerrain)
	{
		this.Platforms.Add(doorTerrain);
	}

	// Token: 0x0400309B RID: 12443
	private static List<Ferr2DT_PathTerrain> m_pathTerrainChildrenHelper_STATIC = new List<Ferr2DT_PathTerrain>();

	// Token: 0x0400309C RID: 12444
	private static List<Ferr2DT_PathTerrain> m_hazardsHelper_STATIC = new List<Ferr2DT_PathTerrain>();
}
