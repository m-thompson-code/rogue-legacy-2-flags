using System;
using System.Collections.Generic;
using Ferr;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class MapRoomEntry : MonoBehaviour
{
	// Token: 0x170015A7 RID: 5543
	// (get) Token: 0x06003A95 RID: 14997 RVA: 0x000202BD File Offset: 0x0001E4BD
	// (set) Token: 0x06003A96 RID: 14998 RVA: 0x000202C5 File Offset: 0x0001E4C5
	public int BiomeControllerIndex { get; set; }

	// Token: 0x170015A8 RID: 5544
	// (get) Token: 0x06003A97 RID: 14999 RVA: 0x000202CE File Offset: 0x0001E4CE
	// (set) Token: 0x06003A98 RID: 15000 RVA: 0x000202D6 File Offset: 0x0001E4D6
	public int[] RoomCompleteBiomeControllerIndexOverrides { get; private set; }

	// Token: 0x170015A9 RID: 5545
	// (get) Token: 0x06003A99 RID: 15001 RVA: 0x000202DF File Offset: 0x0001E4DF
	// (set) Token: 0x06003A9A RID: 15002 RVA: 0x000202E7 File Offset: 0x0001E4E7
	public RoomType RoomType { get; set; }

	// Token: 0x170015AA RID: 5546
	// (get) Token: 0x06003A9B RID: 15003 RVA: 0x000202F0 File Offset: 0x0001E4F0
	// (set) Token: 0x06003A9C RID: 15004 RVA: 0x000202F8 File Offset: 0x0001E4F8
	public bool IsMergeRoomEntry { get; set; }

	// Token: 0x170015AB RID: 5547
	// (get) Token: 0x06003A9D RID: 15005 RVA: 0x00020301 File Offset: 0x0001E501
	// (set) Token: 0x06003A9E RID: 15006 RVA: 0x00020309 File Offset: 0x0001E509
	public bool HasTeleporter { get; private set; }

	// Token: 0x170015AC RID: 5548
	// (get) Token: 0x06003A9F RID: 15007 RVA: 0x00020312 File Offset: 0x0001E512
	public bool HasSpecialRoomIcon
	{
		get
		{
			return this.m_specialRoomIconsParentGO && this.m_specialRoomIconsParentGO.transform.childCount > 0;
		}
	}

	// Token: 0x170015AD RID: 5549
	// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x00020336 File Offset: 0x0001E536
	public bool HasTeleporterIcon
	{
		get
		{
			return this.m_teleporterIconParentGO && this.m_teleporterIconParentGO.transform.childCount > 0;
		}
	}

	// Token: 0x170015AE RID: 5550
	// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x0002035A File Offset: 0x0001E55A
	public Rect Bounds
	{
		get
		{
			return this.m_bounds;
		}
	}

	// Token: 0x170015AF RID: 5551
	// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x00020362 File Offset: 0x0001E562
	public Rect AbsBounds
	{
		get
		{
			return this.m_absBounds;
		}
	}

	// Token: 0x170015B0 RID: 5552
	// (get) Token: 0x06003AA3 RID: 15011 RVA: 0x0002036A File Offset: 0x0001E56A
	// (set) Token: 0x06003AA4 RID: 15012 RVA: 0x00020372 File Offset: 0x0001E572
	public GlobalTeleporterController Teleporter { get; private set; }

	// Token: 0x170015B1 RID: 5553
	// (get) Token: 0x06003AA5 RID: 15013 RVA: 0x0002037B File Offset: 0x0001E57B
	public GameObject TerrainGO
	{
		get
		{
			return this.m_terrainGO;
		}
	}

	// Token: 0x170015B2 RID: 5554
	// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x00020383 File Offset: 0x0001E583
	public Ferr2DT_PathTerrain Terrain
	{
		get
		{
			return this.m_terrain;
		}
	}

	// Token: 0x170015B3 RID: 5555
	// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x0002038B File Offset: 0x0001E58B
	// (set) Token: 0x06003AA8 RID: 15016 RVA: 0x00020393 File Offset: 0x0001E593
	public bool IsTunnelExit { get; set; }

	// Token: 0x170015B4 RID: 5556
	// (get) Token: 0x06003AA9 RID: 15017 RVA: 0x0002039C File Offset: 0x0001E59C
	// (set) Token: 0x06003AAA RID: 15018 RVA: 0x000203A4 File Offset: 0x0001E5A4
	public bool WasVisited { get; set; }

	// Token: 0x170015B5 RID: 5557
	// (get) Token: 0x06003AAB RID: 15019 RVA: 0x000203AD File Offset: 0x0001E5AD
	// (set) Token: 0x06003AAC RID: 15020 RVA: 0x000203B5 File Offset: 0x0001E5B5
	public SpecialRoomType SpecialRoomType { get; set; }

	// Token: 0x06003AAD RID: 15021 RVA: 0x000F03D4 File Offset: 0x000EE5D4
	public void ToggleIconVisibility(MapIconType iconType, int index, bool visible)
	{
		if (iconType <= MapIconType.Door)
		{
			if (iconType <= MapIconType.EnemyKilled)
			{
				if (iconType != MapIconType.Enemy)
				{
					if (iconType != MapIconType.EnemyKilled)
					{
						return;
					}
					if (index == -1)
					{
						if (this.m_enemyKilledIconList != null)
						{
							GameObject[] array = this.m_enemyKilledIconList;
							for (int i = 0; i < array.Length; i++)
							{
								array[i].SetActive(visible);
							}
							return;
						}
					}
					else if (this.m_enemyKilledIconList != null && index >= 0 && index < this.m_enemyKilledIconList.Length)
					{
						this.m_enemyKilledIconList[index].SetActive(visible);
						return;
					}
				}
				else if (index == -1)
				{
					if (this.m_enemyIconList != null)
					{
						GameObject[] array = this.m_enemyIconList;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].SetActive(visible);
						}
						return;
					}
				}
				else if (this.m_enemyIconList != null && index >= 0 && index < this.m_enemyIconList.Length)
				{
					this.m_enemyIconList[index].SetActive(visible);
					return;
				}
			}
			else if (iconType != MapIconType.Teleporter)
			{
				if (iconType != MapIconType.Door)
				{
					return;
				}
				this.m_doorsParentGO.gameObject.SetActive(visible);
				return;
			}
			else if (this.m_teleporterIconParentGO)
			{
				this.m_teleporterIconParentGO.gameObject.SetActive(visible);
				return;
			}
		}
		else if (iconType <= MapIconType.ChestOpened)
		{
			if (iconType != MapIconType.ChestClosed)
			{
				if (iconType != MapIconType.ChestOpened)
				{
					return;
				}
				if (index == -1)
				{
					if (this.m_chestOpenIconList != null)
					{
						for (int j = 0; j < this.m_chestOpenIconList.Length; j++)
						{
							if ((this.m_chestSpawnedFlag & 1 << j) != 0 || !visible)
							{
								this.m_chestOpenIconList[j].SetActive(visible);
							}
						}
						return;
					}
				}
				else
				{
					if ((this.m_chestSpawnedFlag & 1 << index) == 0 && visible)
					{
						return;
					}
					if (this.m_chestOpenIconList != null && index >= 0 && index < this.m_chestOpenIconList.Length)
					{
						this.m_chestOpenIconList[index].SetActive(visible);
						return;
					}
				}
			}
			else if (index == -1)
			{
				if (this.m_chestClosedIconList != null)
				{
					for (int k = 0; k < this.m_chestClosedIconList.Length; k++)
					{
						if ((this.m_chestSpawnedFlag & 1 << k) != 0 || !visible)
						{
							this.m_chestClosedIconList[k].SetActive(visible);
						}
					}
					return;
				}
			}
			else
			{
				if ((this.m_chestSpawnedFlag & 1 << index) == 0 && visible)
				{
					return;
				}
				if (this.m_chestClosedIconList != null && index >= 0 && index < this.m_chestClosedIconList.Length)
				{
					this.m_chestClosedIconList[index].SetActive(visible);
					return;
				}
			}
		}
		else if (iconType != MapIconType.SpecialRoom)
		{
			if (iconType != MapIconType.SpecialRoomUsed)
			{
				if (iconType != MapIconType.SpecialIndicator)
				{
					return;
				}
				if (this.m_specialIndicatorIcon)
				{
					this.m_specialIndicatorIcon.SetActive(visible);
				}
			}
			else if (this.m_specialRoomIconsParentGO)
			{
				if (index == -1)
				{
					if (this.m_specialRoomUsedIconList != null)
					{
						GameObject[] array = this.m_specialRoomUsedIconList;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].SetActive(visible);
						}
						return;
					}
				}
				else if (this.m_specialRoomUsedIconList != null && index >= 0 && index < this.m_specialRoomUsedIconList.Length)
				{
					this.m_specialRoomUsedIconList[index].SetActive(visible);
					return;
				}
			}
		}
		else if (this.m_specialRoomIconsParentGO)
		{
			if (index == -1)
			{
				if (this.m_specialRoomIconList != null)
				{
					GameObject[] array = this.m_specialRoomIconList;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].SetActive(visible);
					}
					return;
				}
			}
			else if (this.m_specialRoomIconList != null && index >= 0 && index < this.m_specialRoomIconList.Length)
			{
				this.m_specialRoomIconList[index].SetActive(visible);
				return;
			}
		}
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x000F0718 File Offset: 0x000EE918
	public void SetSpecialIconAlpha(float alpha)
	{
		if (this.m_specialRoomIconList != null)
		{
			GameObject[] specialRoomIconList = this.m_specialRoomIconList;
			for (int i = 0; i < specialRoomIconList.Length; i++)
			{
				SpriteRenderer componentInChildren = specialRoomIconList[i].GetComponentInChildren<SpriteRenderer>(true);
				if (componentInChildren)
				{
					componentInChildren.SetAlpha(alpha);
				}
			}
		}
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x000F075C File Offset: 0x000EE95C
	public void ToggleAllIconVisibility(bool visible)
	{
		foreach (MapIconType iconType in MapIconType_RL.TypeArray)
		{
			this.ToggleIconVisibility(iconType, -1, visible);
		}
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x000203BE File Offset: 0x0001E5BE
	public void ToggleTerrainVisibility(bool visible)
	{
		this.m_terrainGO.gameObject.SetActive(visible);
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x000203D1 File Offset: 0x0001E5D1
	private Vector2 GetWorldAsLocalPoint(Vector2 center, Vector2 point)
	{
		return point - center;
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x000F078C File Offset: 0x000EE98C
	public void CreateRoomTerrainForRoom(BaseRoom room)
	{
		if (room is Room)
		{
			GridPointManager gridPointManager = (room as Room).GridPointManager;
			if (GameUtility.IsInGame && gridPointManager != null)
			{
				Ferr2DT_PathTerrain ferr2DT_PathTerrain = MapTerrainLibrary.GetMapTerrain(gridPointManager.Size.x, gridPointManager.Size.y);
				bool flag = false;
				if (!ferr2DT_PathTerrain)
				{
					ferr2DT_PathTerrain = MapTerrainLibrary.GetDefaultMapTerrain();
					flag = true;
				}
				this.CreateMapRoomEntryInstance(ferr2DT_PathTerrain, gridPointManager.Bounds.center);
				if (flag)
				{
					this.m_terrain.ClearPoints();
					List<Vector2> meshPoints = this.GetMeshPoints(gridPointManager);
					List<Vector2> pointOffsets = MapRoomEntry.GetPointOffsets(meshPoints);
					for (int i = 0; i < meshPoints.Count; i++)
					{
						this.m_terrain.AddPoint(meshPoints[i] + pointOffsets[i], -1, PointType.Sharp);
					}
					this.m_terrain.BuildMeshOnly(false);
				}
				this.SetBounds();
				this.SetAppearance(room.BiomeType);
				return;
			}
			if (GameUtility.IsInLevelEditor)
			{
				Ferr2DT_PathTerrain ferr2DT_PathTerrain2 = null;
				if (gridPointManager != null)
				{
					ferr2DT_PathTerrain2 = MapTerrainLibrary.GetMapTerrain(gridPointManager.Size.x, gridPointManager.Size.y);
				}
				bool flag2 = false;
				if (!ferr2DT_PathTerrain2)
				{
					ferr2DT_PathTerrain2 = MapTerrainLibrary.GetDefaultMapTerrain();
					flag2 = true;
				}
				this.CreateMapRoomEntryInstance(ferr2DT_PathTerrain2, base.transform.position);
				if (flag2)
				{
					this.m_terrain.ClearPoints();
					List<Vector2> meshPoints2 = MapRoomEntry.GetMeshPoints(room.Collider2D as PolygonCollider2D);
					List<Vector2> pointOffsets2 = MapRoomEntry.GetPointOffsets(meshPoints2);
					for (int j = 0; j < meshPoints2.Count; j++)
					{
						this.m_terrain.AddPoint(meshPoints2[j] + pointOffsets2[j], -1, PointType.Sharp);
					}
					this.m_terrain.BuildMeshOnly(false);
				}
				this.m_terrain.transform.localPosition = Vector3.zero;
				this.SetBounds();
				this.SetAppearance(room.BiomeType);
				return;
			}
		}
		else
		{
			Vector2 center = room.Bounds.center;
			this.CreateMapRoomEntryInstance(MapTerrainLibrary.GetDefaultMapTerrain(), center);
			this.m_terrain.ClearPoints();
			List<List<Vector2>> list = new List<List<Vector2>>();
			MergeRoom mergeRoom = room as MergeRoom;
			for (int k = 0; k < mergeRoom.StandaloneGridPointManagers.Length; k++)
			{
				List<Vector2> fourCorners = this.GetFourCorners(mergeRoom.StandaloneGridPointManagers[k]);
				list.Add(fourCorners);
			}
			List<Vector2> list2 = MergeRoomTools.GetUnion(list, true)[0];
			List<Vector2> list3 = new List<Vector2>();
			for (int l = 0; l < list2.Count; l++)
			{
				Vector2 worldAsLocalPoint = this.GetWorldAsLocalPoint(center, MapController.GetScaledPoint(center, list2[l]));
				list3.Add(worldAsLocalPoint);
			}
			List<Vector2> pointOffsets3 = MapRoomEntry.GetPointOffsets(list3);
			for (int m = 0; m < list3.Count; m++)
			{
				this.m_terrain.AddPoint(list3[m] + pointOffsets3[m], -1, PointType.Sharp);
			}
			this.m_terrain.BuildMeshOnly(false);
			this.SetBounds();
			this.SetAppearance(room.BiomeType);
		}
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x000F0AAC File Offset: 0x000EECAC
	public void CreateRoomTerrainForGridPoint(GridPointManager room)
	{
		if (room.MergeWithGridPointManagers.Count == 0)
		{
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = MapTerrainLibrary.GetMapTerrain(room.Size.x, room.Size.y);
			bool flag = false;
			if (!ferr2DT_PathTerrain)
			{
				ferr2DT_PathTerrain = MapTerrainLibrary.GetDefaultMapTerrain();
				flag = true;
			}
			this.CreateMapRoomEntryInstance(ferr2DT_PathTerrain, room.Bounds.center);
			if (flag)
			{
				this.m_terrain.ClearPoints();
				List<Vector2> meshPoints = this.GetMeshPoints(room);
				List<Vector2> pointOffsets = MapRoomEntry.GetPointOffsets(meshPoints);
				for (int i = 0; i < meshPoints.Count; i++)
				{
					this.m_terrain.AddPoint(meshPoints[i] + pointOffsets[i], -1, PointType.Sharp);
				}
				this.m_terrain.BuildMeshOnly(false);
			}
			this.SetBounds();
			this.SetAppearance(room.Biome);
			return;
		}
		Vector2 center = room.Bounds.center;
		this.CreateMapRoomEntryInstance(MapTerrainLibrary.GetDefaultMapTerrain(), center);
		this.m_terrain.ClearPoints();
		List<List<Vector2>> list = new List<List<Vector2>>();
		List<GridPointManager> list2 = new List<GridPointManager>
		{
			room
		};
		for (int j = 0; j < room.MergeWithGridPointManagers.Count; j++)
		{
			list2.Add(room.MergeWithGridPointManagers[j]);
		}
		for (int k = 0; k < list2.Count; k++)
		{
			List<Vector2> fourCorners = this.GetFourCorners(list2[k]);
			list.Add(fourCorners);
		}
		List<Vector2> list3 = MergeRoomTools.GetUnion(list, true)[0];
		List<Vector2> list4 = new List<Vector2>();
		for (int l = 0; l < list3.Count; l++)
		{
			Vector2 worldAsLocalPoint = this.GetWorldAsLocalPoint(center, MapController.GetScaledPoint(center, list3[l]));
			list4.Add(worldAsLocalPoint);
		}
		List<Vector2> pointOffsets2 = MapRoomEntry.GetPointOffsets(list4);
		for (int m = 0; m < list4.Count; m++)
		{
			this.m_terrain.AddPoint(list4[m] + pointOffsets2[m], -1, PointType.Sharp);
		}
		this.m_terrain.BuildMeshOnly(false);
		this.SetBounds();
		this.SetAppearance(room.Biome);
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x000F0CEC File Offset: 0x000EEEEC
	private List<Vector2> GetFourCorners(GridPointManager gridPointManager)
	{
		Vector2 item = gridPointManager.Bounds.min;
		Vector2 item2 = new Vector2(gridPointManager.Bounds.max.x, gridPointManager.Bounds.min.y);
		Vector2 item3 = gridPointManager.Bounds.max;
		Vector2 item4 = new Vector2(gridPointManager.Bounds.min.x, gridPointManager.Bounds.max.y);
		return new List<Vector2>
		{
			item,
			item2,
			item3,
			item4
		};
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x000F0DA4 File Offset: 0x000EEFA4
	private List<Vector2> GetMeshPoints(GridPointManager gridPointManager)
	{
		float x = gridPointManager.Bounds.min.x;
		float y = gridPointManager.Bounds.min.y;
		float num = gridPointManager.Bounds.max.x - gridPointManager.Bounds.min.x;
		float num2 = gridPointManager.Bounds.max.y - gridPointManager.Bounds.min.y;
		Vector2 worldAsLocalPoint = this.GetWorldAsLocalPoint(gridPointManager.Bounds.center, MapController.GetScaledPoint(gridPointManager.Bounds.center, x, y));
		Vector2 worldAsLocalPoint2 = this.GetWorldAsLocalPoint(gridPointManager.Bounds.center, MapController.GetScaledPoint(gridPointManager.Bounds.center, x + num, y));
		Vector2 worldAsLocalPoint3 = this.GetWorldAsLocalPoint(gridPointManager.Bounds.center, MapController.GetScaledPoint(gridPointManager.Bounds.center, x + num, y + num2));
		Vector2 worldAsLocalPoint4 = this.GetWorldAsLocalPoint(gridPointManager.Bounds.center, MapController.GetScaledPoint(gridPointManager.Bounds.center, x, y + num2));
		MapRoomEntry.m_meshPointsHelper.Clear();
		MapRoomEntry.m_meshPointsHelper.Add(worldAsLocalPoint);
		MapRoomEntry.m_meshPointsHelper.Add(worldAsLocalPoint2);
		MapRoomEntry.m_meshPointsHelper.Add(worldAsLocalPoint3);
		MapRoomEntry.m_meshPointsHelper.Add(worldAsLocalPoint4);
		return MapRoomEntry.m_meshPointsHelper;
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x000F0F68 File Offset: 0x000EF168
	private void CreateMapRoomEntryInstance(Ferr2DT_PathTerrain terrainPrefab, Vector2 center)
	{
		this.m_terrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(terrainPrefab, this.m_terrainGO.transform);
		Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(center, false);
		this.m_terrain.transform.position = mapPositionFromWorld;
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x000F0FAC File Offset: 0x000EF1AC
	private void SetBounds()
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		if (this.m_terrain.MeshData == null)
		{
			using (List<Vector2>.Enumerator enumerator = this.m_terrain.PathData.GetFinalPath().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector2 vector = enumerator.Current;
					if (vector.x < num)
					{
						num = vector.x;
					}
					else if (vector.x > num2)
					{
						num2 = vector.x;
					}
					if (vector.y < num3)
					{
						num3 = vector.y;
					}
					else if (vector.y > num4)
					{
						num4 = vector.y;
					}
				}
				goto IL_1B0;
			}
		}
		num = this.m_terrain.MeshData.bounds.center.x - this.m_terrain.MeshData.bounds.extents.x;
		num2 = this.m_terrain.MeshData.bounds.center.x + this.m_terrain.MeshData.bounds.extents.x;
		num3 = this.m_terrain.MeshData.bounds.center.y - this.m_terrain.MeshData.bounds.extents.y;
		num4 = this.m_terrain.MeshData.bounds.center.y + this.m_terrain.MeshData.bounds.extents.y;
		IL_1B0:
		this.m_bounds = new Rect(num, num3, num2 - num, num4 - num3);
		this.m_absBounds = this.m_bounds;
		this.m_absBounds.x = this.m_absBounds.x + this.m_terrain.transform.position.x;
		this.m_absBounds.y = this.m_absBounds.y + this.m_terrain.transform.position.y;
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x000F11E8 File Offset: 0x000EF3E8
	private void SetAppearance(BiomeType biome)
	{
		biome = BiomeType_RL.GetGroupedBiomeType(biome);
		BiomeArtData artData = BiomeArtDataLibrary.GetArtData(biome);
		if (artData)
		{
			this.m_terrain.vertexColor = artData.Ferr2DBiomeArtData.MapColor;
			this.m_terrain.CreateVertColors(false);
		}
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x000F1230 File Offset: 0x000EF430
	private static List<Vector2> GetPointOffsets(List<Vector2> meshPoints)
	{
		float num = 0.06f;
		int count = meshPoints.Count;
		MapRoomEntry.m_pointOffsetHelper.Clear();
		for (int i = 0; i < count; i++)
		{
			MapRoomEntry.m_pointOffsetHelper.Add(default(Vector2));
		}
		for (int j = 0; j < count; j++)
		{
			Vector2 b = meshPoints[j];
			Vector2 vector = ((j < count - 1) ? meshPoints[j + 1] : meshPoints[0]) - b;
			Vector2 zero = Vector2.zero;
			if (Mathf.RoundToInt(vector.x) == 0)
			{
				int num2 = (vector.y < 0f) ? 1 : -1;
				List<Vector2> pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
				int index = j;
				pointOffsetHelper[index] += new Vector2((float)num2 * num, 0f);
				if (j < count - 1)
				{
					pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
					index = j + 1;
					pointOffsetHelper[index] += new Vector2((float)num2 * num, 0f);
				}
				else
				{
					pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
					pointOffsetHelper[0] = pointOffsetHelper[0] + new Vector2((float)num2 * num, 0f);
				}
			}
			else
			{
				int num3 = (vector.x > 0f) ? 1 : -1;
				List<Vector2> pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
				int index = j;
				pointOffsetHelper[index] += new Vector2(0f, (float)num3 * num);
				if (j < count - 1)
				{
					pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
					index = j + 1;
					pointOffsetHelper[index] += new Vector2(0f, (float)num3 * num);
				}
				else
				{
					pointOffsetHelper = MapRoomEntry.m_pointOffsetHelper;
					pointOffsetHelper[0] = pointOffsetHelper[0] + new Vector2(0f, (float)num3 * num);
				}
			}
		}
		return MapRoomEntry.m_pointOffsetHelper;
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x000F1434 File Offset: 0x000EF634
	private static List<Vector2> GetMeshPoints(PolygonCollider2D collider)
	{
		Vector2[] path = collider.GetPath(0);
		int num = path.Length;
		Vector3 position = collider.transform.position;
		MapRoomEntry.m_meshPointsHelper.Clear();
		for (int i = 0; i < num; i++)
		{
			MapRoomEntry.m_meshPointsHelper.Add(default(Vector2));
		}
		for (int j = 0; j < num; j++)
		{
			Vector2 v = path[j];
			Vector3 vector = collider.transform.TransformPoint(v);
			float x = 0.06666667f * (vector.x - position.x) + position.x;
			float y = 0.06666667f * (vector.y - position.y) + position.y;
			MapRoomEntry.m_meshPointsHelper[j] = collider.transform.InverseTransformPoint(new Vector2(x, y));
		}
		return MapRoomEntry.m_meshPointsHelper;
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x000F1520 File Offset: 0x000EF720
	public void CreateDoorIconsForRoom(GameObject horizontalDoorPrefab, GameObject verticalDoorPrefab, BaseRoom room)
	{
		foreach (Door door in room.Doors)
		{
			GameObject gameObject;
			if (door.Side == RoomSide.Left || door.Side == RoomSide.Right)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(verticalDoorPrefab, this.m_doorsParentGO.transform);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(horizontalDoorPrefab, this.m_doorsParentGO.transform);
			}
			Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(door.CenterPoint, false);
			gameObject.transform.position = mapPositionFromWorld;
		}
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x000F15C4 File Offset: 0x000EF7C4
	public void CreateDoorIconsForGridPoint(GameObject horizontalDoorPrefab, GameObject verticalDoorPrefab, GridPointManager room)
	{
		if (room.MergeWithGridPointManagers.Count > 0)
		{
			this.CreateDoorIconsForGridPoint_Internal(horizontalDoorPrefab, verticalDoorPrefab, room, true);
			for (int i = 0; i < room.MergeWithGridPointManagers.Count; i++)
			{
				this.CreateDoorIconsForGridPoint_Internal(horizontalDoorPrefab, verticalDoorPrefab, room.MergeWithGridPointManagers[i], true);
			}
			return;
		}
		this.CreateDoorIconsForGridPoint_Internal(horizontalDoorPrefab, verticalDoorPrefab, room, false);
	}

	// Token: 0x06003ABD RID: 15037 RVA: 0x000F1624 File Offset: 0x000EF824
	private void CreateDoorIconsForGridPoint_Internal(GameObject horizontalDoorPrefab, GameObject verticalDoorPrefab, GridPointManager gpRoom, bool isPartOfMergeRoom = false)
	{
		foreach (DoorLocation doorLocation in gpRoom.DoorLocations)
		{
			GridPointManager gridPointManager = null;
			if (gpRoom.IsTunnelDestination)
			{
				BiomeType groupedBiomeType = BiomeType_RL.GetGroupedBiomeType(gpRoom.Biome);
				BiomeController biomeController = GameUtility.IsInLevelEditor ? OnPlayManager.BiomeController : WorldBuilder.GetBiomeController(groupedBiomeType);
				int biomeControllerIndex = gpRoom.BiomeControllerIndex;
				BaseRoom room = biomeController.GetRoom(biomeControllerIndex);
				if (room is Room)
				{
					Door door = (room as Room).GetDoor(doorLocation.RoomSide, doorLocation.DoorNumber);
					if (door != null)
					{
						BaseRoom connectedRoom = door.ConnectedRoom;
						if (connectedRoom != null && connectedRoom is Room)
						{
							gridPointManager = (connectedRoom as Room).GridPointManager;
						}
					}
				}
			}
			else
			{
				gridPointManager = gpRoom.GetConnectedRoom(doorLocation);
			}
			if (gridPointManager != null && (!isPartOfMergeRoom || !gpRoom.MergeWithGridPointManagers.Contains(gridPointManager)))
			{
				GameObject gameObject;
				if (doorLocation.RoomSide == RoomSide.Left || doorLocation.RoomSide == RoomSide.Right)
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(verticalDoorPrefab, this.m_doorsParentGO.transform);
				}
				else
				{
					gameObject = UnityEngine.Object.Instantiate<GameObject>(horizontalDoorPrefab, this.m_doorsParentGO.transform);
				}
				Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(RoomUtility.GetDoorCenterPoint(GridController.GetRoomCoordinatesFromGridCoordinates(gpRoom.GridCoordinates), gpRoom.Size, doorLocation), false);
				gameObject.transform.position = mapPositionFromWorld;
				bool flag = (gpRoom.Biome == BiomeType.Tower && gridPointManager.Biome == BiomeType.TowerExterior) || (gpRoom.Biome == BiomeType.TowerExterior && gridPointManager.Biome == BiomeType.Tower);
				if (gpRoom.Biome != gridPointManager.Biome && !flag)
				{
					gameObject.GetComponent<SpriteRenderer>().color = Color.red;
				}
			}
		}
	}

	// Token: 0x06003ABE RID: 15038 RVA: 0x000F1808 File Offset: 0x000EFA08
	public void CreateEnemyIconsForRoom(GameObject enemyIconPrefab, GameObject enemyKilledIconPrefab, BaseRoom room)
	{
		EnemySpawnController[] enemySpawnControllers = room.SpawnControllerManager.EnemySpawnControllers;
		this.m_enemyKilledIconList = new GameObject[enemySpawnControllers.Length];
		foreach (EnemySpawnController enemySpawnController in enemySpawnControllers)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyKilledIconPrefab, this.m_enemyIconsParentGO.transform);
			gameObject.transform.position = MapController.GetMapPositionFromWorld(enemySpawnController.transform.position, true);
			this.m_enemyKilledIconList[enemySpawnController.EnemyIndex] = gameObject;
			if (!enemySpawnController.ShouldSpawn || enemySpawnController.Rank == EnemyRank.Miniboss)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06003ABF RID: 15039 RVA: 0x000F189C File Offset: 0x000EFA9C
	public void CreateChestIconsForRoom(GameObject chestOpenIconPrefab, GameObject chestClosedIconPrefab, BaseRoom room)
	{
		ChestSpawnController[] chestSpawnControllers = room.SpawnControllerManager.ChestSpawnControllers;
		this.m_chestClosedIconList = new GameObject[chestSpawnControllers.Length];
		this.m_chestOpenIconList = new GameObject[chestSpawnControllers.Length];
		int num = 0;
		foreach (ChestSpawnController chestSpawnController in chestSpawnControllers)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(chestOpenIconPrefab, this.m_chestIconsParentGO.transform);
			gameObject.transform.position = MapController.GetMapPositionFromWorld(chestSpawnController.transform.position, true);
			this.m_chestOpenIconList[num] = gameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(chestClosedIconPrefab, this.m_chestIconsParentGO.transform);
			gameObject2.transform.position = gameObject.transform.position;
			this.m_chestClosedIconList[num] = gameObject2;
			this.m_chestSpawnedFlag |= 1 << num;
			num++;
			if (chestSpawnController.ChestType == ChestType.Fairy)
			{
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
			}
		}
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x000F1990 File Offset: 0x000EFB90
	public void CreateChestIconsForGridPoint(GameObject chestOpenIconPrefab, GameObject chestClosedIconPrefab, GridPointManager room)
	{
		List<GridPointManagerContentEntry> roomContent = this.GetRoomContent(room, RoomContentType.Chest, false);
		int count = roomContent.Count;
		this.m_chestClosedIconList = new GameObject[count];
		this.m_chestOpenIconList = new GameObject[count];
		int num = 0;
		this.m_chestSpawnedFlag = 0;
		foreach (GridPointManagerContentEntry gridPointManagerContentEntry in roomContent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(chestOpenIconPrefab, this.m_chestIconsParentGO.transform);
			Vector2 worldPosition = gridPointManagerContentEntry.WorldPosition;
			gameObject.transform.position = MapController.GetMapPositionFromWorld(worldPosition, true);
			this.m_chestOpenIconList[num] = gameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(chestClosedIconPrefab, this.m_chestIconsParentGO.transform);
			gameObject2.transform.position = gameObject.transform.position;
			this.m_chestClosedIconList[num] = gameObject2;
			if (!gridPointManagerContentEntry.IsSpawned)
			{
				gameObject2.SetActive(false);
			}
			else
			{
				this.m_chestSpawnedFlag |= 1 << num;
			}
			num++;
			gameObject.SetActive(false);
		}
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x000F1AB8 File Offset: 0x000EFCB8
	public void CreateSpecialRoomIconsForRoom(GameObject specialRoomIconPrefab, GameObject specialRoomUsedIconPrefab, BaseRoom room)
	{
		MapRoomEntryIconOverrideController component = room.GetComponent<MapRoomEntryIconOverrideController>();
		if (!component || component.ShowRoomIconOnMap)
		{
			UnityEngine.Object.Instantiate<GameObject>(specialRoomIconPrefab, this.m_specialRoomIconsParentGO.transform).transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
			UnityEngine.Object.Instantiate<GameObject>(specialRoomUsedIconPrefab, this.m_specialRoomIconsParentGO.transform).transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
		}
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x000203DA File Offset: 0x0001E5DA
	public void CreateSpecialIndicatorIconForRoom(GameObject specialIndicatorIcon, BaseRoom room)
	{
		this.m_specialIndicatorIcon = UnityEngine.Object.Instantiate<GameObject>(specialIndicatorIcon, this.m_specialIndicatorIconParentGO.transform);
		this.m_specialIndicatorIcon.transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x000F1B40 File Offset: 0x000EFD40
	public void CreateEnemyIconsForGridPoint(GameObject enemyIconPrefab, GameObject enemyKilledIconPrefab, GridPointManager room)
	{
		List<GridPointManagerContentEntry> roomContent = this.GetRoomContent(room, RoomContentType.Enemy, false);
		int count = roomContent.Count;
		this.m_enemyKilledIconList = new GameObject[count];
		int num = 0;
		foreach (GridPointManagerContentEntry gridPointManagerContentEntry in roomContent)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyKilledIconPrefab, this.m_enemyIconsParentGO.transform);
			gameObject.transform.position = MapController.GetMapPositionFromWorld(gridPointManagerContentEntry.WorldPosition, true);
			this.m_enemyKilledIconList[num] = gameObject;
			gameObject.SetActive(false);
			num++;
		}
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x000F1BEC File Offset: 0x000EFDEC
	public void CreateSpecialRoomIconsForGridPoint(GridPointManager room, GameObject specialIndicatorIconPrefab)
	{
		this.RoomType = room.RoomType;
		bool flag = room.MergeWithGridPointManagers.Count > 0;
		MapRoomEntry.m_gridPointsToCheck_STATIC.Clear();
		MapRoomEntry.m_lastGridPointChecked = null;
		MapRoomEntry.m_specialIconsListHelper_STATIC.Clear();
		MapRoomEntry.m_specialUsedIconsListHelper_STATIC.Clear();
		MapRoomEntry.m_gridPointsToCheck_STATIC.Add(room);
		foreach (GridPointManager gridPointManager in room.MergeWithGridPointManagers)
		{
			if (gridPointManager.RoomType != RoomType.BossEntrance)
			{
				MapRoomEntry.m_gridPointsToCheck_STATIC.Add(gridPointManager);
			}
		}
		foreach (GridPointManager gridPointManager2 in MapRoomEntry.m_gridPointsToCheck_STATIC)
		{
			GameObject specialIconPrefab = MapController.GetSpecialIconPrefab(gridPointManager2, false, flag);
			GameObject specialIconPrefab2 = MapController.GetSpecialIconPrefab(gridPointManager2, true, flag);
			if (specialIconPrefab && specialIconPrefab2)
			{
				Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(gridPointManager2.Bounds.center, false);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(specialIconPrefab, this.m_specialRoomIconsParentGO.transform);
				gameObject.transform.position = mapPositionFromWorld;
				MapRoomEntry.m_specialIconsListHelper_STATIC.Add(gameObject);
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(specialIconPrefab2, this.m_specialRoomIconsParentGO.transform);
				gameObject2.transform.position = mapPositionFromWorld;
				MapRoomEntry.m_specialUsedIconsListHelper_STATIC.Add(gameObject2);
			}
			RoomType roomType = gridPointManager2.RoomType;
			if (roomType <= RoomType.BossEntrance)
			{
				if (roomType != RoomType.Fairy)
				{
					if (roomType == RoomType.BossEntrance)
					{
						this.SpecialRoomType = SpecialRoomType.BossEntrance;
					}
				}
				else
				{
					this.SpecialRoomType = SpecialRoomType.Fairy;
				}
			}
			else if (roomType == RoomType.Bonus || roomType == RoomType.Mandatory)
			{
				SpecialRoomType specialRoomType = gridPointManager2.RoomMetaData.SpecialRoomType;
				this.SpecialRoomType = specialRoomType;
			}
			if (room.Biome == BiomeType.Cave && (gridPointManager2.RoomType == RoomType.BossEntrance || this.SpecialRoomType == SpecialRoomType.SubbossEntrance || this.SpecialRoomType == SpecialRoomType.WhitePip || this.SpecialRoomType == SpecialRoomType.WhitePipUnique))
			{
				this.CreateSpecialIndicatorIconForGridPoint(specialIndicatorIconPrefab, gridPointManager2);
			}
		}
		int count = MapRoomEntry.m_specialIconsListHelper_STATIC.Count;
		if (count > 0)
		{
			this.m_specialRoomIconList = new GameObject[count];
			this.m_specialRoomUsedIconList = new GameObject[count];
			for (int i = 0; i < count; i++)
			{
				this.m_specialRoomIconList[i] = MapRoomEntry.m_specialIconsListHelper_STATIC[i];
				this.m_specialRoomUsedIconList[i] = MapRoomEntry.m_specialUsedIconsListHelper_STATIC[i];
			}
		}
		bool flag2 = room.Biome == BiomeType.TowerExterior && (room.RoomType == RoomType.Fairy || room.RoomType == RoomType.Bonus);
		bool flag3 = room.Biome == BiomeType.Tower && room.RoomMetaData.SpecialRoomType == SpecialRoomType.Journal && !room.IsTunnelDestination;
		if ((flag2 || flag || flag3) && count > 0 && room.RoomType != RoomType.BossEntrance)
		{
			this.RoomCompleteBiomeControllerIndexOverrides = new int[count];
			for (int j = 0; j < count; j++)
			{
				this.RoomCompleteBiomeControllerIndexOverrides[j] = -1;
			}
			MapController.AddMapEntryNeedsLinking(BiomeType_RL.GetGroupedBiomeType(room.Biome), this);
		}
		this.ToggleIconVisibility(MapIconType.SpecialIndicator, -1, false);
		this.ToggleIconVisibility(MapIconType.SpecialRoomUsed, -1, false);
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x000F1F3C File Offset: 0x000F013C
	private List<GridPointManagerContentEntry> GetRoomContent(GridPointManager room, RoomContentType contentType, bool excludeNotSpawned)
	{
		if (room != MapRoomEntry.m_lastGridPointChecked)
		{
			if (MapRoomEntry.m_gridPointsToCheck_STATIC == null)
			{
				MapRoomEntry.m_gridPointsToCheck_STATIC = new SortedSet<GridPointManager>(Comparer<GridPointManager>.Create((GridPointManager a, GridPointManager b) => a.RoomNumber.CompareTo(b.RoomNumber)));
			}
			else
			{
				MapRoomEntry.m_gridPointsToCheck_STATIC.Clear();
			}
		}
		MapRoomEntry.m_lastGridPointChecked = room;
		MapRoomEntry.m_gridPointsToCheck_STATIC.Add(room);
		foreach (GridPointManager item in room.MergeWithGridPointManagers)
		{
			MapRoomEntry.m_gridPointsToCheck_STATIC.Add(item);
		}
		MapRoomEntry.m_gridPointRoomContentList_STATIC.Clear();
		foreach (GridPointManager gridPointManager in MapRoomEntry.m_gridPointsToCheck_STATIC)
		{
			if (gridPointManager.Content != null)
			{
				foreach (GridPointManagerContentEntry item2 in gridPointManager.Content)
				{
					if (item2.ContentType == contentType && (!excludeNotSpawned || (excludeNotSpawned && item2.IsSpawned)))
					{
						MapRoomEntry.m_gridPointRoomContentList_STATIC.Add(item2);
					}
				}
			}
		}
		return MapRoomEntry.m_gridPointRoomContentList_STATIC;
	}

	// Token: 0x06003AC6 RID: 15046 RVA: 0x000F208C File Offset: 0x000F028C
	public void CreateSpecialIndicatorIconForGridPoint(GameObject specialIndicatorIcon, GridPointManager room)
	{
		this.m_specialIndicatorIcon = UnityEngine.Object.Instantiate<GameObject>(specialIndicatorIcon, this.m_specialIndicatorIconParentGO.transform);
		Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(room.Bounds.center, false);
		this.m_specialIndicatorIcon.transform.position = mapPositionFromWorld;
	}

	// Token: 0x06003AC7 RID: 15047 RVA: 0x000F20D8 File Offset: 0x000F02D8
	public bool CreateTeleporterIconForGridPoint(GameObject teleporterIconPrefab, GridPointManager room)
	{
		List<GridPointManagerContentEntry> roomContent = this.GetRoomContent(room, RoomContentType.Teleporter, false);
		foreach (GridPointManagerContentEntry gridPointManagerContentEntry in roomContent)
		{
			if (room.RoomType != RoomType.BossEntrance && room.RoomMetaData.SpecialRoomType != SpecialRoomType.SubbossEntrance && room.RoomMetaData.SpecialRoomType != SpecialRoomType.HeirloomEntrance && room.RoomMetaData.SpecialRoomType != SpecialRoomType.NPC && room.RoomMetaData.SpecialRoomType != SpecialRoomType.WhitePipUnique)
			{
				UnityEngine.Object.Instantiate<GameObject>(teleporterIconPrefab, this.m_teleporterIconParentGO.transform).transform.position = base.transform.position;
			}
		}
		this.ToggleIconVisibility(MapIconType.Teleporter, -1, false);
		bool flag = roomContent.Count > 0;
		this.HasTeleporter = flag;
		return flag;
	}

	// Token: 0x04002EC2 RID: 11970
	private const float ROOM_ABS_SHRINK_AMOUNT = 0.06f;

	// Token: 0x04002EC3 RID: 11971
	[SerializeField]
	private GameObject m_enemyIconsParentGO;

	// Token: 0x04002EC4 RID: 11972
	[SerializeField]
	private GameObject m_teleporterIconParentGO;

	// Token: 0x04002EC5 RID: 11973
	[SerializeField]
	private GameObject m_chestIconsParentGO;

	// Token: 0x04002EC6 RID: 11974
	[SerializeField]
	private GameObject m_doorsParentGO;

	// Token: 0x04002EC7 RID: 11975
	[SerializeField]
	private GameObject m_specialRoomIconsParentGO;

	// Token: 0x04002EC8 RID: 11976
	[SerializeField]
	private GameObject m_specialIndicatorIconParentGO;

	// Token: 0x04002EC9 RID: 11977
	[SerializeField]
	private GameObject m_terrainGO;

	// Token: 0x04002ECA RID: 11978
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x04002ECB RID: 11979
	private Rect m_bounds;

	// Token: 0x04002ECC RID: 11980
	private Rect m_absBounds;

	// Token: 0x04002ECD RID: 11981
	private GameObject[] m_chestOpenIconList;

	// Token: 0x04002ECE RID: 11982
	private GameObject[] m_chestClosedIconList;

	// Token: 0x04002ECF RID: 11983
	private GameObject[] m_enemyIconList;

	// Token: 0x04002ED0 RID: 11984
	private GameObject[] m_enemyKilledIconList;

	// Token: 0x04002ED1 RID: 11985
	private GameObject[] m_specialRoomIconList;

	// Token: 0x04002ED2 RID: 11986
	private GameObject[] m_specialRoomUsedIconList;

	// Token: 0x04002ED3 RID: 11987
	private GameObject m_specialIndicatorIcon;

	// Token: 0x04002ED4 RID: 11988
	private int m_chestSpawnedFlag;

	// Token: 0x04002EDE RID: 11998
	private static List<Vector2> m_meshPointsHelper = new List<Vector2>(12);

	// Token: 0x04002EDF RID: 11999
	private static List<Vector2> m_pointOffsetHelper = new List<Vector2>(12);

	// Token: 0x04002EE0 RID: 12000
	private static List<GameObject> m_specialIconsListHelper_STATIC = new List<GameObject>();

	// Token: 0x04002EE1 RID: 12001
	private static List<GameObject> m_specialUsedIconsListHelper_STATIC = new List<GameObject>();

	// Token: 0x04002EE2 RID: 12002
	private static SortedSet<GridPointManager> m_gridPointsToCheck_STATIC;

	// Token: 0x04002EE3 RID: 12003
	private static List<GridPointManagerContentEntry> m_gridPointRoomContentList_STATIC = new List<GridPointManagerContentEntry>();

	// Token: 0x04002EE4 RID: 12004
	private static GridPointManager m_lastGridPointChecked;
}
