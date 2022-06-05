using System;
using System.Collections.Generic;
using Cinemachine;
using ClipperLibFerr;
using Ferr;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class RoomCreator : MonoBehaviour
{
	// Token: 0x1700143C RID: 5180
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x000C325E File Offset: 0x000C145E
	public GameObject RoomTemplatePrefab
	{
		get
		{
			return this.m_roomPrefab;
		}
	}

	// Token: 0x1700143D RID: 5181
	// (get) Token: 0x06003951 RID: 14673 RVA: 0x000C3266 File Offset: 0x000C1466
	public GameObject SimpleRoomPrefab
	{
		get
		{
			return this.m_simpleRoomPrefab;
		}
	}

	// Token: 0x1700143E RID: 5182
	// (get) Token: 0x06003952 RID: 14674 RVA: 0x000C326E File Offset: 0x000C146E
	public GameObject Bridge1x3RoomPrefab
	{
		get
		{
			return this.m_bridge1x3RoomPrefab;
		}
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x000C3278 File Offset: 0x000C1478
	public GameObject CreateRoomInstance(int roomWidth, int roomHeight)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_roomPrefab);
		gameObject.tag = "Room";
		Room component = gameObject.GetComponent<Room>();
		component.SetUnitDimensions(roomWidth * 32, roomHeight * 18);
		GameObject wallsLocation = new GameObject("Walls");
		component.WallsLocation = wallsLocation;
		this.CreateRoomSides(component);
		GameObject doorsLocation = new GameObject("Doors");
		component.DoorsLocation = doorsLocation;
		this.CreateRoomDoors(component);
		this.CreateRoomBounds(roomWidth, roomHeight, component);
		return gameObject;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x000C32EC File Offset: 0x000C14EC
	private GameObject CreateRoomBounds(int roomWidth, int roomHeight, Room room)
	{
		Ferr2DT_PathTerrain component = room.gameObject.GetComponent<Ferr2DT_PathTerrain>();
		int num = roomWidth * 16;
		int num2 = roomHeight * 9;
		component.ClearPoints();
		component.AddPoint(new Vector2((float)(-(float)num), (float)num2), -1, PointType.Sharp);
		component.AddPoint(new Vector2((float)(-(float)num), (float)(-(float)num2)), -1, PointType.Sharp);
		component.AddPoint(new Vector2((float)num, (float)(-(float)num2)), -1, PointType.Sharp);
		component.AddPoint(new Vector2((float)num, (float)num2), -1, PointType.Sharp);
		component.FillMode = Ferr2D_SectionMode.Normal;
		component.EdgeMode = Ferr2D_SectionMode.None;
		component.Build(true);
		return component.gameObject;
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x000C337C File Offset: 0x000C157C
	public Ferr2DT_PathTerrain CreateTerrain(Room room)
	{
		Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultTerrainPrefab);
		ferr2DT_PathTerrain.transform.SetParent(room.WallsLocation.transform);
		ferr2DT_PathTerrain.transform.localPosition = Vector3.zero;
		ferr2DT_PathTerrain.transform.localRotation = Quaternion.identity;
		ferr2DT_PathTerrain.transform.localScale = Vector3.one;
		ferr2DT_PathTerrain.name = "Terrain";
		ferr2DT_PathTerrain.tag = "Platform";
		ferr2DT_PathTerrain.ClearPoints();
		ferr2DT_PathTerrain.AddPoint(new Vector2(-1f, 1f), -1, PointType.Sharp);
		ferr2DT_PathTerrain.AddPoint(new Vector2(-1f, -1f), -1, PointType.Sharp);
		ferr2DT_PathTerrain.AddPoint(new Vector2(1f, -1f), -1, PointType.Sharp);
		ferr2DT_PathTerrain.AddPoint(new Vector2(1f, 1f), -1, PointType.Sharp);
		ferr2DT_PathTerrain.Build(true);
		return ferr2DT_PathTerrain;
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x000C3460 File Offset: 0x000C1660
	public Ferr2DT_PathTerrain CreateOneWay(Room room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultOneWayTerrainPrefab);
		if (ferr2DT_PathTerrain != null)
		{
			ferr2DT_PathTerrain.transform.SetParent(room.OneWayLocation);
			ferr2DT_PathTerrain.transform.localPosition = Vector3.zero;
			ferr2DT_PathTerrain.name = "One Way";
			ferr2DT_PathTerrain.tag = "OneWay";
		}
		else
		{
			Debug.LogFormat("Failed to instantiate One Way Platform", Array.Empty<object>());
		}
		ferr2DT_PathTerrain.Build(true);
		return ferr2DT_PathTerrain;
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x000C34E8 File Offset: 0x000C16E8
	private void CreateRoomSides(Room room)
	{
		int x = room.UnitDimensions.x;
		int y = room.UnitDimensions.y;
		float num = 13f;
		int num2 = (int)num;
		int num3 = (int)Math.Round((double)num, MidpointRounding.AwayFromZero);
		num3 -= num2;
		float num4 = 6.5f;
		int num5 = (int)num4;
		int num6 = (int)Math.Round((double)num4, MidpointRounding.AwayFromZero);
		num6 -= num5;
		for (int i = 0; i < x / 32; i++)
		{
			int num7 = -(int)((float)x / 2f) + i * 32;
			int num8 = (int)((float)y / 2f);
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = this.CreateTerrain(room);
			ferr2DT_PathTerrain.PathData.Set(0, new Vector2((float)num7, (float)num8));
			ferr2DT_PathTerrain.PathData.Set(1, new Vector2((float)num7, (float)(num8 - 1)));
			ferr2DT_PathTerrain.PathData.Set(2, new Vector2((float)(num7 + num2), (float)(num8 - 1)));
			ferr2DT_PathTerrain.PathData.Set(3, new Vector2((float)(num7 + num2), (float)num8));
			ferr2DT_PathTerrain.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain, true, true);
			num7 += num2 + 6;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain2 = this.CreateTerrain(room);
			ferr2DT_PathTerrain2.PathData.Set(0, new Vector2((float)num7, (float)num8));
			ferr2DT_PathTerrain2.PathData.Set(1, new Vector2((float)num7, (float)(num8 - 1)));
			ferr2DT_PathTerrain2.PathData.Set(2, new Vector2((float)(num7 + num2 + num3), (float)(num8 - 1)));
			ferr2DT_PathTerrain2.PathData.Set(3, new Vector2((float)(num7 + num2 + num3), (float)num8));
			ferr2DT_PathTerrain2.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain2, true, true);
			num7 = -(int)((float)x / 2f) + i * 32;
			num8 = -num8 + 1;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain3 = this.CreateTerrain(room);
			ferr2DT_PathTerrain3.PathData.Set(0, new Vector2((float)num7, (float)num8));
			ferr2DT_PathTerrain3.PathData.Set(1, new Vector2((float)num7, (float)(num8 - 1)));
			ferr2DT_PathTerrain3.PathData.Set(2, new Vector2((float)(num7 + num2), (float)(num8 - 1)));
			ferr2DT_PathTerrain3.PathData.Set(3, new Vector2((float)(num7 + num2), (float)num8));
			ferr2DT_PathTerrain3.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain3, true, true);
			num7 += num2 + 6;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain4 = this.CreateTerrain(room);
			ferr2DT_PathTerrain4.PathData.Set(0, new Vector2((float)num7, (float)num8));
			ferr2DT_PathTerrain4.PathData.Set(1, new Vector2((float)num7, (float)(num8 - 1)));
			ferr2DT_PathTerrain4.PathData.Set(2, new Vector2((float)(num7 + num2 + num3), (float)(num8 - 1)));
			ferr2DT_PathTerrain4.PathData.Set(3, new Vector2((float)(num7 + num2 + num3), (float)num8));
			ferr2DT_PathTerrain4.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain4, true, true);
		}
		for (int j = 0; j < y / 18; j++)
		{
			int num9 = -(int)((float)x / 2f);
			int num10 = (int)((float)y / 2f) - j * 18;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain5 = this.CreateTerrain(room);
			ferr2DT_PathTerrain5.PathData.Set(0, new Vector2((float)num9, (float)num10));
			ferr2DT_PathTerrain5.PathData.Set(1, new Vector2((float)num9, (float)(num10 - num5)));
			ferr2DT_PathTerrain5.PathData.Set(2, new Vector2((float)(num9 + 1), (float)(num10 - num5)));
			ferr2DT_PathTerrain5.PathData.Set(3, new Vector2((float)(num9 + 1), (float)num10));
			ferr2DT_PathTerrain5.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain5, true, true);
			num10 -= num5 + 5;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain6 = this.CreateTerrain(room);
			ferr2DT_PathTerrain6.PathData.Set(0, new Vector2((float)num9, (float)num10));
			ferr2DT_PathTerrain6.PathData.Set(1, new Vector2((float)num9, (float)(num10 - num5 - num6)));
			ferr2DT_PathTerrain6.PathData.Set(2, new Vector2((float)(num9 + 1), (float)(num10 - num5 - num6)));
			ferr2DT_PathTerrain6.PathData.Set(3, new Vector2((float)(num9 + 1), (float)num10));
			ferr2DT_PathTerrain6.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain6, true, true);
			num10 = (int)((float)y / 2f) - j * 18;
			num9 = -num9 - 1;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain7 = this.CreateTerrain(room);
			ferr2DT_PathTerrain7.PathData.Set(0, new Vector2((float)num9, (float)num10));
			ferr2DT_PathTerrain7.PathData.Set(1, new Vector2((float)num9, (float)(num10 - num5)));
			ferr2DT_PathTerrain7.PathData.Set(2, new Vector2((float)(num9 + 1), (float)(num10 - num5)));
			ferr2DT_PathTerrain7.PathData.Set(3, new Vector2((float)(num9 + 1), (float)num10));
			ferr2DT_PathTerrain7.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain7, true, true);
			num10 -= num5 + 5;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain8 = this.CreateTerrain(room);
			ferr2DT_PathTerrain8.PathData.Set(0, new Vector2((float)num9, (float)num10));
			ferr2DT_PathTerrain8.PathData.Set(1, new Vector2((float)num9, (float)(num10 - num5 - num6)));
			ferr2DT_PathTerrain8.PathData.Set(2, new Vector2((float)(num9 + 1), (float)(num10 - num5 - num6)));
			ferr2DT_PathTerrain8.PathData.Set(3, new Vector2((float)(num9 + 1), (float)num10));
			ferr2DT_PathTerrain8.name = "Sides";
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain8, true, true);
		}
		List<Ferr2DT_PathTerrain> list = new List<Ferr2DT_PathTerrain>();
		foreach (object obj in room.WallsLocation.transform)
		{
			Ferr2DT_PathTerrain component = ((Transform)obj).gameObject.GetComponent<Ferr2DT_PathTerrain>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		Ferr2DUtilities.EasyBoolean(ClipType.ctUnion, list, Ferr2DUtilities.Ferr2DUndoType.DestroyImmediate, true);
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x000C3A90 File Offset: 0x000C1C90
	public GameObject CreateBuildingInterior(Room currentlySelectedRoom)
	{
		return null;
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x000C3A93 File Offset: 0x000C1C93
	public GameObject CreateHiddenRoomWall(Room currentlySelectedRoom)
	{
		return null;
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x000C3A98 File Offset: 0x000C1C98
	public GameObject CreateCloud(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_cloudSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x000C3ABC File Offset: 0x000C1CBC
	public GameObject CreateInvisibleCeiling(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_invisibleCeilingSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x000C3AE0 File Offset: 0x000C1CE0
	public GameObject CreateTunnel(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_tunnelSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x000C3B03 File Offset: 0x000C1D03
	public void CreateSpikes(Room room)
	{
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x000C3B05 File Offset: 0x000C1D05
	public void CreateLocalTeleporter(Room room)
	{
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x000C3B07 File Offset: 0x000C1D07
	public void CreateTeleporterLeyLines(Room room)
	{
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x000C3B09 File Offset: 0x000C1D09
	public void CreateGlobalTeleporter(Room room)
	{
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x000C3B0B File Offset: 0x000C1D0B
	public void CreateOrbiter(Room room)
	{
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x000C3B0D File Offset: 0x000C1D0D
	public void CreateTurret(Room room, bool diagonal)
	{
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x000C3B0F File Offset: 0x000C1D0F
	public void CreateHazardSpawnController(HazardCategory hazardCategory, Room room)
	{
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x000C3B11 File Offset: 0x000C1D11
	public void CreateChest(Room room)
	{
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x000C3B13 File Offset: 0x000C1D13
	public void CreateBouncePlatform(Room room)
	{
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x000C3B18 File Offset: 0x000C1D18
	public GameObject CreateEnemy(Room room)
	{
		if (room == null)
		{
			throw new ArgumentNullException("room");
		}
		if (this.m_enemyPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_enemyPrefab);
			gameObject.transform.SetParent(room.EnemyLocation);
			return gameObject;
		}
		Debug.LogFormat("<color=yellow>Enemy Prefab Field in RoomCreator is null</color>", Array.Empty<object>());
		return null;
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x000C3B74 File Offset: 0x000C1D74
	private void CreateRoomDoors(Room room)
	{
		int x = room.UnitDimensions.x;
		int y = room.UnitDimensions.y;
		int num = 13;
		int num2 = 6;
		int num3 = x / 32;
		for (int i = 0; i < num3; i++)
		{
			int num4 = -(int)((float)x / 2f) + 32 * i + num;
			int num5 = (int)((float)y / 2f);
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultDoorsPrefab);
			ferr2DT_PathTerrain.GetComponent<Door>().InitialiseInEditor(RoomSide.Top, i);
			ferr2DT_PathTerrain.transform.SetParent(room.DoorsLocation.transform);
			ferr2DT_PathTerrain.transform.localPosition = Vector3.zero;
			ferr2DT_PathTerrain.transform.localRotation = Quaternion.identity;
			ferr2DT_PathTerrain.transform.localScale = Vector3.one;
			ferr2DT_PathTerrain.name = "Top Door";
			ferr2DT_PathTerrain.ClearPoints();
			ferr2DT_PathTerrain.AddPoint(new Vector2((float)num4, (float)num5), -1, PointType.Sharp);
			ferr2DT_PathTerrain.AddPoint(new Vector2((float)num4, (float)(num5 - 1)), -1, PointType.Sharp);
			ferr2DT_PathTerrain.AddPoint(new Vector2((float)(num4 + 6), (float)(num5 - 1)), -1, PointType.Sharp);
			ferr2DT_PathTerrain.AddPoint(new Vector2((float)(num4 + 6), (float)num5), -1, PointType.Sharp);
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain, true, true);
			Ferr2DT_PathTerrain ferr2DT_PathTerrain2 = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultDoorsPrefab);
			ferr2DT_PathTerrain2.GetComponent<Door>().InitialiseInEditor(RoomSide.Bottom, i);
			ferr2DT_PathTerrain2.transform.SetParent(room.DoorsLocation.transform);
			ferr2DT_PathTerrain2.transform.localPosition = Vector3.zero;
			ferr2DT_PathTerrain2.transform.localRotation = Quaternion.identity;
			ferr2DT_PathTerrain2.transform.localScale = Vector3.one;
			ferr2DT_PathTerrain2.name = "Bottom Door";
			num5 = -num5 + 1;
			ferr2DT_PathTerrain2.ClearPoints();
			ferr2DT_PathTerrain2.AddPoint(new Vector2((float)num4, (float)num5), -1, PointType.Sharp);
			ferr2DT_PathTerrain2.AddPoint(new Vector2((float)num4, (float)(num5 - 1)), -1, PointType.Sharp);
			ferr2DT_PathTerrain2.AddPoint(new Vector2((float)(num4 + 6), (float)(num5 - 1)), -1, PointType.Sharp);
			ferr2DT_PathTerrain2.AddPoint(new Vector2((float)(num4 + 6), (float)num5), -1, PointType.Sharp);
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain2, true, true);
		}
		num3 = y / 18;
		for (int j = 0; j < num3; j++)
		{
			int num6 = -(int)((float)x / 2f);
			int num7 = (int)((float)y / 2f) - j * 18 - num2;
			Ferr2DT_PathTerrain ferr2DT_PathTerrain3 = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultDoorsPrefab);
			ferr2DT_PathTerrain3.GetComponent<Door>().InitialiseInEditor(RoomSide.Left, j);
			ferr2DT_PathTerrain3.transform.SetParent(room.DoorsLocation.transform);
			ferr2DT_PathTerrain3.transform.localPosition = Vector3.zero;
			ferr2DT_PathTerrain3.transform.localRotation = Quaternion.identity;
			ferr2DT_PathTerrain3.transform.localScale = Vector3.one;
			ferr2DT_PathTerrain3.name = "Left Door";
			ferr2DT_PathTerrain3.ClearPoints();
			ferr2DT_PathTerrain3.AddPoint(new Vector2((float)num6, (float)num7), -1, PointType.Sharp);
			ferr2DT_PathTerrain3.AddPoint(new Vector2((float)num6, (float)(num7 - 5)), -1, PointType.Sharp);
			ferr2DT_PathTerrain3.AddPoint(new Vector2((float)(num6 + 1), (float)(num7 - 5)), -1, PointType.Sharp);
			ferr2DT_PathTerrain3.AddPoint(new Vector2((float)(num6 + 1), (float)num7), -1, PointType.Sharp);
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain3, true, true);
			Ferr2DT_PathTerrain ferr2DT_PathTerrain4 = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(this.m_defaultDoorsPrefab);
			ferr2DT_PathTerrain4.GetComponent<Door>().InitialiseInEditor(RoomSide.Right, j);
			ferr2DT_PathTerrain4.transform.SetParent(room.DoorsLocation.transform);
			ferr2DT_PathTerrain4.transform.localPosition = Vector3.zero;
			ferr2DT_PathTerrain4.transform.localRotation = Quaternion.identity;
			ferr2DT_PathTerrain4.transform.localScale = Vector3.one;
			ferr2DT_PathTerrain4.name = "Right Door";
			num6 = -num6 - 1;
			ferr2DT_PathTerrain4.ClearPoints();
			ferr2DT_PathTerrain4.AddPoint(new Vector2((float)num6, (float)num7), -1, PointType.Sharp);
			ferr2DT_PathTerrain4.AddPoint(new Vector2((float)num6, (float)(num7 - 5)), -1, PointType.Sharp);
			ferr2DT_PathTerrain4.AddPoint(new Vector2((float)(num6 + 1), (float)(num7 - 5)), -1, PointType.Sharp);
			ferr2DT_PathTerrain4.AddPoint(new Vector2((float)(num6 + 1), (float)num7), -1, PointType.Sharp);
			Ferr2DUtilities.RecentreFerr2DTerrain(ferr2DT_PathTerrain4, true, true);
		}
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x000C3F6C File Offset: 0x000C216C
	public void CreatePlayerSpawn(Room room)
	{
		this.InstantiatePrefabInRoomAtLocalPosition(this.m_playerSpawnPrefab, room, default(Vector3));
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x000C3F90 File Offset: 0x000C2190
	public GameObject CreateRoomForTunnelTesting(Vector3 position)
	{
		if (this.m_roomForTunnelTestingPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_roomForTunnelTestingPrefab);
			gameObject.transform.position = position;
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x000C3FB9 File Offset: 0x000C21B9
	private GameObject InstantiatePrefabInRoomAtLocalPosition(GameObject prefab, Room room, Vector3 localPosition = default(Vector3))
	{
		return null;
	}

	// Token: 0x04002C21 RID: 11297
	[SerializeField]
	private GameObject m_playerSpawnPrefab;

	// Token: 0x04002C22 RID: 11298
	[SerializeField]
	private GameObject m_roomPrefab;

	// Token: 0x04002C23 RID: 11299
	[SerializeField]
	private GameObject m_chestPrefab;

	// Token: 0x04002C24 RID: 11300
	[SerializeField]
	private GameObject m_specialPlatformSpawnerPrefab;

	// Token: 0x04002C25 RID: 11301
	[SerializeField]
	private GameObject m_spikePrefab;

	// Token: 0x04002C26 RID: 11302
	[SerializeField]
	private GameObject m_localTeleporterPrefab;

	// Token: 0x04002C27 RID: 11303
	[SerializeField]
	private GameObject m_teleporterLeyLinesPrefab;

	// Token: 0x04002C28 RID: 11304
	[SerializeField]
	private GameObject m_globalTeleporterPrefab;

	// Token: 0x04002C29 RID: 11305
	[SerializeField]
	private GameObject m_lineHazardSpawnControllerPrefab;

	// Token: 0x04002C2A RID: 11306
	[SerializeField]
	private GameObject m_pointHazardSpawnControllerPrefab;

	// Token: 0x04002C2B RID: 11307
	[SerializeField]
	private GameObject m_turretHazardSpawnControllerPrefab;

	// Token: 0x04002C2C RID: 11308
	[SerializeField]
	private GameObject m_turretDiagonalPrefab;

	// Token: 0x04002C2D RID: 11309
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultTerrainPrefab;

	// Token: 0x04002C2E RID: 11310
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultDoorsPrefab;

	// Token: 0x04002C2F RID: 11311
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultOneWayTerrainPrefab;

	// Token: 0x04002C30 RID: 11312
	[SerializeField]
	private CinemachineVirtualCamera m_cinemachineVirtualCameraPrefab;

	// Token: 0x04002C31 RID: 11313
	[SerializeField]
	private GameObject m_enemyPrefab;

	// Token: 0x04002C32 RID: 11314
	[SerializeField]
	private GameObject m_cutoutPrefab;

	// Token: 0x04002C33 RID: 11315
	[SerializeField]
	private GameObject m_tunnelSpawnControllerPrefab;

	// Token: 0x04002C34 RID: 11316
	[SerializeField]
	private GameObject m_cloudSpawnControllerPrefab;

	// Token: 0x04002C35 RID: 11317
	[SerializeField]
	private GameObject m_invisibleCeilingSpawnControllerPrefab;

	// Token: 0x04002C36 RID: 11318
	[SerializeField]
	private GameObject m_simpleRoomPrefab;

	// Token: 0x04002C37 RID: 11319
	[SerializeField]
	private GameObject m_bridge1x3RoomPrefab;

	// Token: 0x04002C38 RID: 11320
	[SerializeField]
	private GameObject m_roomForTunnelTestingPrefab;

	// Token: 0x04002C39 RID: 11321
	[SerializeField]
	private GameObject m_hiddenWallPrefab;

	// Token: 0x04002C3A RID: 11322
	[SerializeField]
	private GameObject m_buildingInteriorPrefab;

	// Token: 0x04002C3B RID: 11323
	public const int ROOM_UNIT_WIDTH = 32;

	// Token: 0x04002C3C RID: 11324
	public const int ROOM_UNIT_HEIGHT = 18;

	// Token: 0x04002C3D RID: 11325
	public const int SIDE_DOOR_UNIT_SIZE = 5;

	// Token: 0x04002C3E RID: 11326
	public const int UPPER_LOWER_DOOR_UNIT_SIZE = 6;

	// Token: 0x04002C3F RID: 11327
	public const float WALL_WIDTH = 1f;
}
