using System;
using System.Collections.Generic;
using Ferr;
using UnityEngine;

// Token: 0x02000483 RID: 1155
public class MapRoomEntry : MonoBehaviour
{
	// Token: 0x17001068 RID: 4200
	// (get) Token: 0x06002A6D RID: 10861 RVA: 0x0008E740 File Offset: 0x0008C940
	// (set) Token: 0x06002A6E RID: 10862 RVA: 0x0008E748 File Offset: 0x0008C948
	public int BiomeControllerIndex { get; set; }

	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06002A6F RID: 10863 RVA: 0x0008E751 File Offset: 0x0008C951
	// (set) Token: 0x06002A70 RID: 10864 RVA: 0x0008E759 File Offset: 0x0008C959
	public int[] RoomCompleteBiomeControllerIndexOverrides { get; private set; }

	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06002A71 RID: 10865 RVA: 0x0008E762 File Offset: 0x0008C962
	// (set) Token: 0x06002A72 RID: 10866 RVA: 0x0008E76A File Offset: 0x0008C96A
	public RoomType RoomType { get; set; }

	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06002A73 RID: 10867 RVA: 0x0008E773 File Offset: 0x0008C973
	// (set) Token: 0x06002A74 RID: 10868 RVA: 0x0008E77B File Offset: 0x0008C97B
	public bool IsMergeRoomEntry { get; set; }

	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x06002A75 RID: 10869 RVA: 0x0008E784 File Offset: 0x0008C984
	// (set) Token: 0x06002A76 RID: 10870 RVA: 0x0008E78C File Offset: 0x0008C98C
	public bool HasTeleporter { get; private set; }

	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x06002A77 RID: 10871 RVA: 0x0008E795 File Offset: 0x0008C995
	public bool HasSpecialRoomIcon
	{
		get
		{
			return this.m_specialRoomIconsParentGO && this.m_specialRoomIconsParentGO.transform.childCount > 0;
		}
	}

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x06002A78 RID: 10872 RVA: 0x0008E7B9 File Offset: 0x0008C9B9
	public bool HasTeleporterIcon
	{
		get
		{
			return this.m_teleporterIconParentGO && this.m_teleporterIconParentGO.transform.childCount > 0;
		}
	}

	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06002A79 RID: 10873 RVA: 0x0008E7DD File Offset: 0x0008C9DD
	public Rect Bounds
	{
		get
		{
			return this.m_bounds;
		}
	}

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06002A7A RID: 10874 RVA: 0x0008E7E5 File Offset: 0x0008C9E5
	public Rect AbsBounds
	{
		get
		{
			return this.m_absBounds;
		}
	}

	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06002A7B RID: 10875 RVA: 0x0008E7ED File Offset: 0x0008C9ED
	// (set) Token: 0x06002A7C RID: 10876 RVA: 0x0008E7F5 File Offset: 0x0008C9F5
	public GlobalTeleporterController Teleporter { get; private set; }

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06002A7D RID: 10877 RVA: 0x0008E7FE File Offset: 0x0008C9FE
	public GameObject TerrainGO
	{
		get
		{
			return this.m_terrainGO;
		}
	}

	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x06002A7E RID: 10878 RVA: 0x0008E806 File Offset: 0x0008CA06
	public Ferr2DT_PathTerrain Terrain
	{
		get
		{
			return this.m_terrain;
		}
	}

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06002A7F RID: 10879 RVA: 0x0008E80E File Offset: 0x0008CA0E
	// (set) Token: 0x06002A80 RID: 10880 RVA: 0x0008E816 File Offset: 0x0008CA16
	public bool IsTunnelExit { get; set; }

	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x06002A81 RID: 10881 RVA: 0x0008E81F File Offset: 0x0008CA1F
	// (set) Token: 0x06002A82 RID: 10882 RVA: 0x0008E827 File Offset: 0x0008CA27
	public bool WasVisited { get; set; }

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x06002A83 RID: 10883 RVA: 0x0008E830 File Offset: 0x0008CA30
	// (set) Token: 0x06002A84 RID: 10884 RVA: 0x0008E838 File Offset: 0x0008CA38
	public SpecialRoomType SpecialRoomType { get; set; }

	// Token: 0x06002A85 RID: 10885 RVA: 0x0008E844 File Offset: 0x0008CA44
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

	// Token: 0x06002A86 RID: 10886 RVA: 0x0008EB88 File Offset: 0x0008CD88
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

	// Token: 0x06002A87 RID: 10887 RVA: 0x0008EBCC File Offset: 0x0008CDCC
	public void ToggleAllIconVisibility(bool visible)
	{
		foreach (MapIconType iconType in MapIconType_RL.TypeArray)
		{
			this.ToggleIconVisibility(iconType, -1, visible);
		}
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x0008EBFA File Offset: 0x0008CDFA
	public void ToggleTerrainVisibility(bool visible)
	{
		this.m_terrainGO.gameObject.SetActive(visible);
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x0008EC0D File Offset: 0x0008CE0D
	private Vector2 GetWorldAsLocalPoint(Vector2 center, Vector2 point)
	{
		return point - center;
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x0008EC18 File Offset: 0x0008CE18
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

	// Token: 0x06002A8B RID: 10891 RVA: 0x0008EF38 File Offset: 0x0008D138
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

	// Token: 0x06002A8C RID: 10892 RVA: 0x0008F178 File Offset: 0x0008D378
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

	// Token: 0x06002A8D RID: 10893 RVA: 0x0008F230 File Offset: 0x0008D430
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

	// Token: 0x06002A8E RID: 10894 RVA: 0x0008F3F4 File Offset: 0x0008D5F4
	private void CreateMapRoomEntryInstance(Ferr2DT_PathTerrain terrainPrefab, Vector2 center)
	{
		this.m_terrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(terrainPrefab, this.m_terrainGO.transform);
		Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(center, false);
		this.m_terrain.transform.position = mapPositionFromWorld;
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x0008F438 File Offset: 0x0008D638
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

	// Token: 0x06002A90 RID: 10896 RVA: 0x0008F674 File Offset: 0x0008D874
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

	// Token: 0x06002A91 RID: 10897 RVA: 0x0008F6BC File Offset: 0x0008D8BC
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

	// Token: 0x06002A92 RID: 10898 RVA: 0x0008F8C0 File Offset: 0x0008DAC0
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

	// Token: 0x06002A93 RID: 10899 RVA: 0x0008F9AC File Offset: 0x0008DBAC
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

	// Token: 0x06002A94 RID: 10900 RVA: 0x0008FA50 File Offset: 0x0008DC50
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

	// Token: 0x06002A95 RID: 10901 RVA: 0x0008FAB0 File Offset: 0x0008DCB0
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

	// Token: 0x06002A96 RID: 10902 RVA: 0x0008FC94 File Offset: 0x0008DE94
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

	// Token: 0x06002A97 RID: 10903 RVA: 0x0008FD28 File Offset: 0x0008DF28
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

	// Token: 0x06002A98 RID: 10904 RVA: 0x0008FE1C File Offset: 0x0008E01C
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

	// Token: 0x06002A99 RID: 10905 RVA: 0x0008FF44 File Offset: 0x0008E144
	public void CreateSpecialRoomIconsForRoom(GameObject specialRoomIconPrefab, GameObject specialRoomUsedIconPrefab, BaseRoom room)
	{
		MapRoomEntryIconOverrideController component = room.GetComponent<MapRoomEntryIconOverrideController>();
		if (!component || component.ShowRoomIconOnMap)
		{
			UnityEngine.Object.Instantiate<GameObject>(specialRoomIconPrefab, this.m_specialRoomIconsParentGO.transform).transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
			UnityEngine.Object.Instantiate<GameObject>(specialRoomUsedIconPrefab, this.m_specialRoomIconsParentGO.transform).transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
		}
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x0008FFCA File Offset: 0x0008E1CA
	public void CreateSpecialIndicatorIconForRoom(GameObject specialIndicatorIcon, BaseRoom room)
	{
		this.m_specialIndicatorIcon = UnityEngine.Object.Instantiate<GameObject>(specialIndicatorIcon, this.m_specialIndicatorIconParentGO.transform);
		this.m_specialIndicatorIcon.transform.position = MapController.GetMapPositionFromWorld(room.gameObject.transform.position, false);
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x0009000C File Offset: 0x0008E20C
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

	// Token: 0x06002A9C RID: 10908 RVA: 0x000900B8 File Offset: 0x0008E2B8
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

	// Token: 0x06002A9D RID: 10909 RVA: 0x00090408 File Offset: 0x0008E608
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

	// Token: 0x06002A9E RID: 10910 RVA: 0x00090558 File Offset: 0x0008E758
	public void CreateSpecialIndicatorIconForGridPoint(GameObject specialIndicatorIcon, GridPointManager room)
	{
		this.m_specialIndicatorIcon = UnityEngine.Object.Instantiate<GameObject>(specialIndicatorIcon, this.m_specialIndicatorIconParentGO.transform);
		Vector3 mapPositionFromWorld = MapController.GetMapPositionFromWorld(room.Bounds.center, false);
		this.m_specialIndicatorIcon.transform.position = mapPositionFromWorld;
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000905A4 File Offset: 0x0008E7A4
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

	// Token: 0x040022C4 RID: 8900
	private const float ROOM_ABS_SHRINK_AMOUNT = 0.06f;

	// Token: 0x040022C5 RID: 8901
	[SerializeField]
	private GameObject m_enemyIconsParentGO;

	// Token: 0x040022C6 RID: 8902
	[SerializeField]
	private GameObject m_teleporterIconParentGO;

	// Token: 0x040022C7 RID: 8903
	[SerializeField]
	private GameObject m_chestIconsParentGO;

	// Token: 0x040022C8 RID: 8904
	[SerializeField]
	private GameObject m_doorsParentGO;

	// Token: 0x040022C9 RID: 8905
	[SerializeField]
	private GameObject m_specialRoomIconsParentGO;

	// Token: 0x040022CA RID: 8906
	[SerializeField]
	private GameObject m_specialIndicatorIconParentGO;

	// Token: 0x040022CB RID: 8907
	[SerializeField]
	private GameObject m_terrainGO;

	// Token: 0x040022CC RID: 8908
	private Ferr2DT_PathTerrain m_terrain;

	// Token: 0x040022CD RID: 8909
	private Rect m_bounds;

	// Token: 0x040022CE RID: 8910
	private Rect m_absBounds;

	// Token: 0x040022CF RID: 8911
	private GameObject[] m_chestOpenIconList;

	// Token: 0x040022D0 RID: 8912
	private GameObject[] m_chestClosedIconList;

	// Token: 0x040022D1 RID: 8913
	private GameObject[] m_enemyIconList;

	// Token: 0x040022D2 RID: 8914
	private GameObject[] m_enemyKilledIconList;

	// Token: 0x040022D3 RID: 8915
	private GameObject[] m_specialRoomIconList;

	// Token: 0x040022D4 RID: 8916
	private GameObject[] m_specialRoomUsedIconList;

	// Token: 0x040022D5 RID: 8917
	private GameObject m_specialIndicatorIcon;

	// Token: 0x040022D6 RID: 8918
	private int m_chestSpawnedFlag;

	// Token: 0x040022E0 RID: 8928
	private static List<Vector2> m_meshPointsHelper = new List<Vector2>(12);

	// Token: 0x040022E1 RID: 8929
	private static List<Vector2> m_pointOffsetHelper = new List<Vector2>(12);

	// Token: 0x040022E2 RID: 8930
	private static List<GameObject> m_specialIconsListHelper_STATIC = new List<GameObject>();

	// Token: 0x040022E3 RID: 8931
	private static List<GameObject> m_specialUsedIconsListHelper_STATIC = new List<GameObject>();

	// Token: 0x040022E4 RID: 8932
	private static SortedSet<GridPointManager> m_gridPointsToCheck_STATIC;

	// Token: 0x040022E5 RID: 8933
	private static List<GridPointManagerContentEntry> m_gridPointRoomContentList_STATIC = new List<GridPointManagerContentEntry>();

	// Token: 0x040022E6 RID: 8934
	private static GridPointManager m_lastGridPointChecked;
}
