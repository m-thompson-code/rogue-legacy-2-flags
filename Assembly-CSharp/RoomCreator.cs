using System;
using System.Collections.Generic;
using Cinemachine;
using ClipperLibFerr;
using Ferr;
using UnityEngine;

// Token: 0x02000A5D RID: 2653
public class RoomCreator : MonoBehaviour
{
	// Token: 0x17001BA3 RID: 7075
	// (get) Token: 0x0600502F RID: 20527 RVA: 0x0002BCCD File Offset: 0x00029ECD
	public GameObject RoomTemplatePrefab
	{
		get
		{
			return this.m_roomPrefab;
		}
	}

	// Token: 0x17001BA4 RID: 7076
	// (get) Token: 0x06005030 RID: 20528 RVA: 0x0002BCD5 File Offset: 0x00029ED5
	public GameObject SimpleRoomPrefab
	{
		get
		{
			return this.m_simpleRoomPrefab;
		}
	}

	// Token: 0x17001BA5 RID: 7077
	// (get) Token: 0x06005031 RID: 20529 RVA: 0x0002BCDD File Offset: 0x00029EDD
	public GameObject Bridge1x3RoomPrefab
	{
		get
		{
			return this.m_bridge1x3RoomPrefab;
		}
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x00131958 File Offset: 0x0012FB58
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

	// Token: 0x06005033 RID: 20531 RVA: 0x001319CC File Offset: 0x0012FBCC
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

	// Token: 0x06005034 RID: 20532 RVA: 0x00131A5C File Offset: 0x0012FC5C
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

	// Token: 0x06005035 RID: 20533 RVA: 0x00131B40 File Offset: 0x0012FD40
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

	// Token: 0x06005036 RID: 20534 RVA: 0x00131BC8 File Offset: 0x0012FDC8
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

	// Token: 0x06005037 RID: 20535 RVA: 0x0000F49B File Offset: 0x0000D69B
	public GameObject CreateBuildingInterior(Room currentlySelectedRoom)
	{
		return null;
	}

	// Token: 0x06005038 RID: 20536 RVA: 0x0000F49B File Offset: 0x0000D69B
	public GameObject CreateHiddenRoomWall(Room currentlySelectedRoom)
	{
		return null;
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x00132170 File Offset: 0x00130370
	public GameObject CreateCloud(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_cloudSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x00132194 File Offset: 0x00130394
	public GameObject CreateInvisibleCeiling(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_invisibleCeilingSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001321B8 File Offset: 0x001303B8
	public GameObject CreateTunnel(Room room)
	{
		return this.InstantiatePrefabInRoomAtLocalPosition(this.m_tunnelSpawnControllerPrefab, room, default(Vector3));
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateSpikes(Room room)
	{
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateLocalTeleporter(Room room)
	{
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateTeleporterLeyLines(Room room)
	{
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateGlobalTeleporter(Room room)
	{
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateOrbiter(Room room)
	{
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateTurret(Room room, bool diagonal)
	{
	}

	// Token: 0x06005042 RID: 20546 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateHazardSpawnController(HazardCategory hazardCategory, Room room)
	{
	}

	// Token: 0x06005043 RID: 20547 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateChest(Room room)
	{
	}

	// Token: 0x06005044 RID: 20548 RVA: 0x00002FCA File Offset: 0x000011CA
	public void CreateBouncePlatform(Room room)
	{
	}

	// Token: 0x06005045 RID: 20549 RVA: 0x001321DC File Offset: 0x001303DC
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

	// Token: 0x06005046 RID: 20550 RVA: 0x00132238 File Offset: 0x00130438
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

	// Token: 0x06005047 RID: 20551 RVA: 0x00132630 File Offset: 0x00130830
	public void CreatePlayerSpawn(Room room)
	{
		this.InstantiatePrefabInRoomAtLocalPosition(this.m_playerSpawnPrefab, room, default(Vector3));
	}

	// Token: 0x06005048 RID: 20552 RVA: 0x0002BCE5 File Offset: 0x00029EE5
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

	// Token: 0x06005049 RID: 20553 RVA: 0x0000F49B File Offset: 0x0000D69B
	private GameObject InstantiatePrefabInRoomAtLocalPosition(GameObject prefab, Room room, Vector3 localPosition = default(Vector3))
	{
		return null;
	}

	// Token: 0x04003CB3 RID: 15539
	[SerializeField]
	private GameObject m_playerSpawnPrefab;

	// Token: 0x04003CB4 RID: 15540
	[SerializeField]
	private GameObject m_roomPrefab;

	// Token: 0x04003CB5 RID: 15541
	[SerializeField]
	private GameObject m_chestPrefab;

	// Token: 0x04003CB6 RID: 15542
	[SerializeField]
	private GameObject m_specialPlatformSpawnerPrefab;

	// Token: 0x04003CB7 RID: 15543
	[SerializeField]
	private GameObject m_spikePrefab;

	// Token: 0x04003CB8 RID: 15544
	[SerializeField]
	private GameObject m_localTeleporterPrefab;

	// Token: 0x04003CB9 RID: 15545
	[SerializeField]
	private GameObject m_teleporterLeyLinesPrefab;

	// Token: 0x04003CBA RID: 15546
	[SerializeField]
	private GameObject m_globalTeleporterPrefab;

	// Token: 0x04003CBB RID: 15547
	[SerializeField]
	private GameObject m_lineHazardSpawnControllerPrefab;

	// Token: 0x04003CBC RID: 15548
	[SerializeField]
	private GameObject m_pointHazardSpawnControllerPrefab;

	// Token: 0x04003CBD RID: 15549
	[SerializeField]
	private GameObject m_turretHazardSpawnControllerPrefab;

	// Token: 0x04003CBE RID: 15550
	[SerializeField]
	private GameObject m_turretDiagonalPrefab;

	// Token: 0x04003CBF RID: 15551
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultTerrainPrefab;

	// Token: 0x04003CC0 RID: 15552
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultDoorsPrefab;

	// Token: 0x04003CC1 RID: 15553
	[SerializeField]
	private Ferr2DT_PathTerrain m_defaultOneWayTerrainPrefab;

	// Token: 0x04003CC2 RID: 15554
	[SerializeField]
	private CinemachineVirtualCamera m_cinemachineVirtualCameraPrefab;

	// Token: 0x04003CC3 RID: 15555
	[SerializeField]
	private GameObject m_enemyPrefab;

	// Token: 0x04003CC4 RID: 15556
	[SerializeField]
	private GameObject m_cutoutPrefab;

	// Token: 0x04003CC5 RID: 15557
	[SerializeField]
	private GameObject m_tunnelSpawnControllerPrefab;

	// Token: 0x04003CC6 RID: 15558
	[SerializeField]
	private GameObject m_cloudSpawnControllerPrefab;

	// Token: 0x04003CC7 RID: 15559
	[SerializeField]
	private GameObject m_invisibleCeilingSpawnControllerPrefab;

	// Token: 0x04003CC8 RID: 15560
	[SerializeField]
	private GameObject m_simpleRoomPrefab;

	// Token: 0x04003CC9 RID: 15561
	[SerializeField]
	private GameObject m_bridge1x3RoomPrefab;

	// Token: 0x04003CCA RID: 15562
	[SerializeField]
	private GameObject m_roomForTunnelTestingPrefab;

	// Token: 0x04003CCB RID: 15563
	[SerializeField]
	private GameObject m_hiddenWallPrefab;

	// Token: 0x04003CCC RID: 15564
	[SerializeField]
	private GameObject m_buildingInteriorPrefab;

	// Token: 0x04003CCD RID: 15565
	public const int ROOM_UNIT_WIDTH = 32;

	// Token: 0x04003CCE RID: 15566
	public const int ROOM_UNIT_HEIGHT = 18;

	// Token: 0x04003CCF RID: 15567
	public const int SIDE_DOOR_UNIT_SIZE = 5;

	// Token: 0x04003CD0 RID: 15568
	public const int UPPER_LOWER_DOOR_UNIT_SIZE = 6;

	// Token: 0x04003CD1 RID: 15569
	public const float WALL_WIDTH = 1f;
}
