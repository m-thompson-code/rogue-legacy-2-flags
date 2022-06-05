using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLibFerr;
using Ferr;
using UnityEngine;

// Token: 0x0200067B RID: 1659
public static class MergeRoomTools
{
	// Token: 0x06003BED RID: 15341 RVA: 0x000CE318 File Offset: 0x000CC518
	public static GameObject Merge(List<Room> roomsToMerge)
	{
		if (roomsToMerge == null)
		{
			throw new ArgumentNullException("roomsToMerge");
		}
		if (roomsToMerge.Count == 0 || roomsToMerge.Count == 1)
		{
			throw new ArgumentException("roomsToMerge", "List should contain at least 2 Rooms");
		}
		if (roomsToMerge.Any((Room room) => room == null))
		{
			throw new ArgumentException("roomsToMerge", "Rooms must be of Type Room");
		}
		MergeRoom mergeRoom = MergeRoomTools.CreateMergeRoom();
		mergeRoom.AddRooms(roomsToMerge);
		return mergeRoom.gameObject;
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x000CE39C File Offset: 0x000CC59C
	private static List<Vector2Int> GetGridPointCoordinatesToFill(MergeRoom mergeRoom)
	{
		MergeRoomTools.m_gridPointsHelper.Clear();
		GridPointManager[] standaloneGridPointManagers = mergeRoom.StandaloneGridPointManagers;
		int m;
		for (m = 0; m < standaloneGridPointManagers.Length; m++)
		{
			GridPoint[,] gridPoints = standaloneGridPointManagers[m].GridPoints;
			int upperBound = gridPoints.GetUpperBound(0);
			int upperBound2 = gridPoints.GetUpperBound(1);
			for (int l = gridPoints.GetLowerBound(0); l <= upperBound; l++)
			{
				for (int k = gridPoints.GetLowerBound(1); k <= upperBound2; k++)
				{
					GridPoint item = gridPoints[l, k];
					MergeRoomTools.m_gridPointsHelper.Add(item);
				}
			}
		}
		int i2 = MergeRoomTools.m_gridPointsHelper.Min((GridPoint g) => g.GridCoordinates.x);
		int j2 = MergeRoomTools.m_gridPointsHelper.Min((GridPoint g) => g.GridCoordinates.y);
		int num = MergeRoomTools.m_gridPointsHelper.Max((GridPoint g) => g.GridCoordinates.x) + 1;
		int num2 = MergeRoomTools.m_gridPointsHelper.Max((GridPoint g) => g.GridCoordinates.y) + 1;
		MergeRoomTools.m_gridPointsToFillHelper.Clear();
		int i;
		for (i = i2; i < num; i = m + 1)
		{
			int j;
			for (j = j2; j < num2; j = m + 1)
			{
				if (!MergeRoomTools.m_gridPointsHelper.Any((GridPoint g) => g.GridCoordinates == new Vector2Int(i, j)))
				{
					MergeRoomTools.m_gridPointsToFillHelper.Add(new Vector2Int(i, j));
				}
				m = j;
			}
			m = i;
		}
		return MergeRoomTools.m_gridPointsToFillHelper;
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x000CE59C File Offset: 0x000CC79C
	public static List<Ferr2DT_PathTerrain> MergeTerrain(BaseRoom room)
	{
		BiomeType terrainBiomeType = ArtUtility.GetTerrainBiomeType(room);
		BiomeArtData biomeArtData = room.BiomeArtDataOverride;
		if (!biomeArtData)
		{
			biomeArtData = BiomeArtDataLibrary.GetArtData(terrainBiomeType);
		}
		MergeRoomTools.m_decoTerrainHelper.Clear();
		if (room is Room)
		{
			MergeRoomTools.m_decoTerrainHelper.AddRange(MergeRoomTools.MergeTerrain(room.gameObject.transform, terrainBiomeType, biomeArtData, room.Bounds, room.TerrainManager.Platforms));
		}
		else
		{
			MergeRoom mergeRoom = room as MergeRoom;
			int num = 0;
			foreach (KeyValuePair<Transform, List<Ferr2DT_PathTerrain>> keyValuePair in mergeRoom.StandaloneRoomToTerrainTable)
			{
				Bounds bounds = mergeRoom.StandaloneRoomBounds[num];
				MergeRoomTools.m_decoTerrainHelper.AddRange(MergeRoomTools.MergeTerrain(mergeRoom.gameObject.transform, terrainBiomeType, biomeArtData, bounds, keyValuePair.Value));
				num++;
			}
			List<Vector2Int> gridPointCoordinatesToFill = MergeRoomTools.GetGridPointCoordinatesToFill(mergeRoom);
			MergeRoomTools.CreateFillDecoTerrain(MergeRoomTools.m_decoTerrainHelper, mergeRoom, gridPointCoordinatesToFill);
			MergeRoomTools.m_decoTerrainHelper = MergeRoomTools.MergeDecoTerrain(mergeRoom, MergeRoomTools.m_decoTerrainHelper);
		}
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in MergeRoomTools.m_decoTerrainHelper)
		{
			BiomeCreatorTools.SetOuterEdges(ferr2DT_PathTerrain, room.Bounds);
			ferr2DT_PathTerrain.BuildMeshOnly(false);
		}
		return MergeRoomTools.m_decoTerrainHelper;
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x000CE704 File Offset: 0x000CC904
	private static void CreateFillDecoTerrain(List<Ferr2DT_PathTerrain> decoTerrain, MergeRoom mergeRoom, List<Vector2Int> gridPointCoordsToFill)
	{
		BiomeArtData artData = BiomeArtDataLibrary.GetArtData(mergeRoom.AppearanceBiomeType);
		foreach (Vector2Int gridCoordinates in gridPointCoordsToFill)
		{
			Bounds gridPointBounds = GridController.GetGridPointBounds(gridCoordinates);
			MergeRoomTools.m_pointsList.Clear();
			MergeRoomTools.m_pointsList.Add(new Vector2(gridPointBounds.min.x, gridPointBounds.min.y));
			MergeRoomTools.m_pointsList.Add(new Vector2(gridPointBounds.max.x, gridPointBounds.min.y));
			MergeRoomTools.m_pointsList.Add(new Vector2(gridPointBounds.max.x, gridPointBounds.max.y));
			MergeRoomTools.m_pointsList.Add(new Vector2(gridPointBounds.min.x, gridPointBounds.max.y));
			decoTerrain.Add(MergeRoomTools.CreateDecorativeTerrain(mergeRoom.gameObject.transform, artData, gridPointBounds, MergeRoomTools.m_pointsList, false, null));
		}
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x000CE828 File Offset: 0x000CCA28
	private static List<Ferr2DT_PathTerrain> MergeDecoTerrain(MergeRoom mergeRoom, List<Ferr2DT_PathTerrain> terrainToMerge)
	{
		BiomeArtData artData = BiomeArtDataLibrary.GetArtData(mergeRoom.AppearanceBiomeType);
		List<Ferr2DT_PathTerrain> list = new List<Ferr2DT_PathTerrain>();
		if (terrainToMerge.Count > 0)
		{
			List<List<Vector2>> union = MergeRoomTools.GetUnion(MergeRoomTools.GetTerrainPoints(terrainToMerge), true);
			for (int i = 0; i < union.Count; i++)
			{
				Ferr2DT_PathTerrain item = MergeRoomTools.CreateDecorativeTerrain(mergeRoom.gameObject.transform, artData, mergeRoom.Bounds, union[i], false, null);
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x000CE89C File Offset: 0x000CCA9C
	private static List<Ferr2DT_PathTerrain> MergeTerrain(Transform parent, BiomeType biome, BiomeArtData biomeArtData, Bounds bounds, IEnumerable<Ferr2DT_PathTerrain> terrainToMerge)
	{
		MergeRoomTools.m_mergeTerrainHelper.Clear();
		MergeRoomTools.m_normalTerrainHelper.Clear();
		MergeRoomTools.m_invertedTerrainHelper.Clear();
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in terrainToMerge)
		{
			ISpawnController component = ferr2DT_PathTerrain.GetComponent<ISpawnController>();
			if (component == null || component.ShouldSpawn)
			{
				if (ferr2DT_PathTerrain.DisableMerging)
				{
					MergeRoomTools.m_mergeTerrainHelper.Add(ferr2DT_PathTerrain);
				}
				else if (ferr2DT_PathTerrain.FillMode == Ferr2D_SectionMode.Normal)
				{
					MergeRoomTools.m_normalTerrainHelper.Add(ferr2DT_PathTerrain);
				}
				else if (ferr2DT_PathTerrain.FillMode == Ferr2D_SectionMode.Invert)
				{
					MergeRoomTools.m_invertedTerrainHelper.Add(ferr2DT_PathTerrain);
				}
			}
		}
		MergeRoomTools.SetDontMergeTerrainMaterial(biome, MergeRoomTools.m_mergeTerrainHelper);
		MergeRoomTools.m_mergeTerrainHelper.Clear();
		if (MergeRoomTools.m_normalTerrainHelper.Count > 0)
		{
			MergeRoomTools.m_mergeTerrainHelper.AddRange(MergeRoomTools.DoMerge(parent, biomeArtData, bounds, MergeRoomTools.m_normalTerrainHelper));
		}
		if (MergeRoomTools.m_invertedTerrainHelper.Count > 0)
		{
			MergeRoomTools.m_mergeTerrainHelper.AddRange(MergeRoomTools.DoMerge(parent, biomeArtData, bounds, MergeRoomTools.m_invertedTerrainHelper));
		}
		return MergeRoomTools.m_mergeTerrainHelper;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x000CE9B0 File Offset: 0x000CCBB0
	private static void SetDontMergeTerrainMaterial(BiomeType biome, List<Ferr2DT_PathTerrain> dontMergeTerrain)
	{
		Ferr2DT_Material ferr2DT_Material;
		if (biome == BiomeType.Study)
		{
			ferr2DT_Material = RoomPrefabLibrary.StudyDontMergeFerr2DMaterial;
		}
		else
		{
			ferr2DT_Material = RoomPrefabLibrary.DefaultDontMergeFerr2DMaterial;
		}
		BiomeArtData artData = BiomeArtDataLibrary.GetArtData(biome);
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in dontMergeTerrain)
		{
			Ferr2DT_Material material = ferr2DT_Material;
			if (ferr2DT_PathTerrain.UseBiomeMaterial && artData != null && artData.Ferr2DBiomeArtData != null && artData.Ferr2DBiomeArtData.TerrainMaster != null && artData.Ferr2DBiomeArtData.TerrainMaster.Ferr2DSettings.Material != null)
			{
				material = BiomeArtDataLibrary.GetArtData(biome).Ferr2DBiomeArtData.TerrainMaster.Ferr2DSettings.Material;
			}
			if (!ferr2DT_PathTerrain.OverrideDisabledMergeMaterial)
			{
				ferr2DT_PathTerrain.SetMaterial(material);
			}
			ferr2DT_PathTerrain.BuildMeshOnly(false);
		}
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x000CEA94 File Offset: 0x000CCC94
	private static List<Ferr2DT_PathTerrain> DoMerge(Transform parent, BiomeArtData biomeArtData, Bounds bounds, List<Ferr2DT_PathTerrain> terrainToMerge)
	{
		bool isInvertedTerrain = terrainToMerge[0].FillMode == Ferr2D_SectionMode.Invert;
		List<List<Vector2>> union = MergeRoomTools.GetUnion(MergeRoomTools.GetTerrainPoints(terrainToMerge), true);
		Dictionary<Line_RL, Ferr2D_PointData> directionOverrides = MergeRoomTools.GetDirectionOverrides(terrainToMerge);
		MergeRoomTools.ApplyMissingEdgesToUnionResult(union, directionOverrides);
		List<Ferr2DT_PathTerrain> list = new List<Ferr2DT_PathTerrain>();
		for (int i = 0; i < union.Count; i++)
		{
			Ferr2DT_PathTerrain item = MergeRoomTools.CreateDecorativeTerrain(parent, biomeArtData, bounds, union[i], isInvertedTerrain, directionOverrides);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x000CEB04 File Offset: 0x000CCD04
	private static void ApplyMissingEdgesToUnionResult(List<List<Vector2>> unionResult, Dictionary<Line_RL, Ferr2D_PointData> edgeOverrides)
	{
		if (edgeOverrides == null)
		{
			return;
		}
		foreach (List<Vector2> list in unionResult)
		{
			for (int i = 0; i < list.Count; i++)
			{
				int index = (i - 1 < 0) ? (list.Count - 1) : (i - 1);
				int index2 = (i + 1 < list.Count) ? (i + 1) : 0;
				Vector2 vector = list[i];
				Vector2 vector2 = list[index];
				Vector2 vector3 = list[index2];
				foreach (KeyValuePair<Line_RL, Ferr2D_PointData> keyValuePair in edgeOverrides)
				{
					Line_RL key = keyValuePair.Key;
					if (key.PointB == vector && key.PointA != vector2)
					{
						float num = CDGHelper.AngleBetweenPts(key.PointB, key.PointA);
						float num2 = CDGHelper.AngleBetweenPts(vector, vector2);
						if (num == num2)
						{
							list.Insert(i, key.PointA);
							i++;
						}
					}
					else if (key.PointA == vector && key.PointB != vector3)
					{
						float num3 = CDGHelper.AngleBetweenPts(key.PointA, key.PointB);
						float num4 = CDGHelper.AngleBetweenPts(vector, vector3);
						if (num3 == num4)
						{
							list.Insert(i + 1, key.PointB);
						}
					}
				}
			}
		}
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x000CECB8 File Offset: 0x000CCEB8
	private static Dictionary<Line_RL, Ferr2D_PointData> GetDirectionOverrides(List<Ferr2DT_PathTerrain> terrainToMerge)
	{
		Dictionary<Line_RL, Ferr2D_PointData> dictionary = new Dictionary<Line_RL, Ferr2D_PointData>();
		for (int i = 0; i < terrainToMerge.Count; i++)
		{
			Ferr2DT_PathTerrain ferr2DT_PathTerrain = terrainToMerge[i];
			List<Vector2> points = ferr2DT_PathTerrain.PathData.GetPoints(0);
			for (int j = 0; j < points.Count; j++)
			{
				Ferr2D_PointData data = ferr2DT_PathTerrain.PathData.GetData(j);
				int num = j + 1;
				if (j == points.Count - 1)
				{
					num = 0;
				}
				ferr2DT_PathTerrain.PathData.GetData(num);
				if (data.directionOverride != 100)
				{
					Vector2 pointInWorldSpaceCoordinates = MergeRoomTools.GetPointInWorldSpaceCoordinates(points[j], ferr2DT_PathTerrain.transform);
					Vector2 pointInWorldSpaceCoordinates2 = MergeRoomTools.GetPointInWorldSpaceCoordinates(points[num], ferr2DT_PathTerrain.transform);
					Line_RL key = new Line_RL(pointInWorldSpaceCoordinates, pointInWorldSpaceCoordinates2);
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, data);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x000CED9C File Offset: 0x000CCF9C
	private static List<List<Vector2>> GetTerrainPoints(IEnumerable<Ferr2DT_PathTerrain> terrainToMerge)
	{
		List<List<Vector2>> list = new List<List<Vector2>>();
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in terrainToMerge)
		{
			Vector3 localPosition = ferr2DT_PathTerrain.transform.localPosition;
			Vector3 localPosition2 = new Vector3((float)Mathf.RoundToInt(localPosition.x), (float)Mathf.RoundToInt(localPosition.y), localPosition.z);
			ferr2DT_PathTerrain.transform.localPosition = localPosition2;
			ferr2DT_PathTerrain.GetComponent<Renderer>().enabled = false;
			list.Add(MergeRoomTools.GetPoints(ferr2DT_PathTerrain));
		}
		return list;
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x000CEE3C File Offset: 0x000CD03C
	private static List<Vector2> GetPoints(Ferr2DT_PathTerrain terrain)
	{
		List<Vector2> points = terrain.PathData.GetPoints(0);
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < points.Count; i++)
		{
			list.Add(MergeRoomTools.GetPointInWorldSpaceCoordinates(points[i], terrain.transform));
		}
		return list;
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x000CEE88 File Offset: 0x000CD088
	private static Vector2 GetPointInWorldSpaceCoordinates(Vector2 localCoords, Transform transform)
	{
		int num = Mathf.RoundToInt(localCoords.x);
		int num2 = Mathf.RoundToInt(localCoords.y);
		return transform.TransformPoint(new Vector2((float)num, (float)num2));
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x000CEEC8 File Offset: 0x000CD0C8
	private static Ferr2DT_PathTerrain CreateDecorativeTerrain(Transform parent, BiomeArtData biomeArtData, Bounds bounds, List<Vector2> terrainPoints, bool isInvertedTerrain, Dictionary<Line_RL, Ferr2D_PointData> edgeOverrides = null)
	{
		Ferr2DT_PathTerrain ferr2DT_PathTerrain = UnityEngine.Object.Instantiate<Ferr2DT_PathTerrain>(RoomPrefabLibrary.DecoTerrainPrefab);
		ferr2DT_PathTerrain.ClearPoints();
		GameObject gameObject = ferr2DT_PathTerrain.gameObject;
		gameObject.transform.SetParent(parent);
		gameObject.transform.localPosition = Vector3.zero;
		BaseRoom component = parent.gameObject.GetComponent<BaseRoom>();
		ferr2DT_PathTerrain.PathData.Closed = true;
		ferr2DT_PathTerrain.pixelsPerUnit = biomeArtData.Ferr2DBiomeArtData.TerrainMaster.Ferr2DSettings.PixelsPerUnit;
		ferr2DT_PathTerrain.ColliderMode = Ferr2D_ColliderMode.None;
		Ferr2DT_Material material = biomeArtData.Ferr2DBiomeArtData.TerrainMaster.Ferr2DSettings.Material;
		ferr2DT_PathTerrain.SetMaterial(material);
		if (isInvertedTerrain)
		{
			ferr2DT_PathTerrain.FillMode = Ferr2D_SectionMode.Invert;
			ferr2DT_PathTerrain.EdgeMode = Ferr2D_SectionMode.Invert;
		}
		bool flag = component != null && !component.Equals(null);
		bool flag2 = flag && BiomeCreation_EV.BIOME_TAGS.ContainsKey(component.AppearanceBiomeType) && BiomeCreation_EV.BIOME_TAGS[component.AppearanceBiomeType].Contains(BiomeTag.Rounded_Terrain);
		float x = component.Bounds.min.x;
		float x2 = component.Bounds.max.x;
		float y = component.Bounds.max.y;
		float y2 = component.Bounds.min.y;
		for (int i = 0; i < terrainPoints.Count; i++)
		{
			Vector2 vector = terrainPoints[i];
			Vector2 aPt = gameObject.transform.InverseTransformPoint(vector);
			ferr2DT_PathTerrain.AddPoint(aPt, -1, PointType.Sharp);
			int index = i + 1;
			if (i == terrainPoints.Count - 1)
			{
				index = 0;
			}
			Vector2 pointB = terrainPoints[index];
			Line_RL key = new Line_RL(vector, pointB);
			if (edgeOverrides != null && edgeOverrides.ContainsKey(key))
			{
				ferr2DT_PathTerrain.PathData.SetData(i, edgeOverrides[key]);
			}
		}
		if (flag && flag2)
		{
			for (int j = 0; j < terrainPoints.Count; j++)
			{
				Vector2 vector2 = terrainPoints[j];
				int aRawIndex = (j + 1 < terrainPoints.Count) ? (j + 1) : 0;
				ferr2DT_PathTerrain.PathData.GetSegmentNormal(j);
				ferr2DT_PathTerrain.PathData.GetSegmentNormal(aRawIndex);
				if (false || vector2.x - x < 0.1f || x2 - vector2.x < 0.1f || y - vector2.y < 0.1f || vector2.y - y2 < 0.1f)
				{
					ferr2DT_PathTerrain.PathData.GetControls(aRawIndex).type = PointType.Sharp;
				}
				else
				{
					ferr2DT_PathTerrain.PathData.GetControls(aRawIndex).type = PointType.CircleCorner;
				}
			}
		}
		return ferr2DT_PathTerrain;
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x000CF188 File Offset: 0x000CD388
	public static List<List<Vector2>> GetUnion(List<List<Vector2>> points, bool applyOffsetCheck = true)
	{
		if (!applyOffsetCheck)
		{
			Clipper clipper = new Clipper(0);
			clipper.AddPath(points[0], PolyType.ptSubject, true);
			for (int i = 1; i < points.Count; i++)
			{
				clipper.AddPath(points[i], PolyType.ptClip, true);
			}
			List<List<Vector2>> list = new List<List<Vector2>>();
			clipper.Execute(ClipType.ctUnion, list, PolyFillType.pftNonZero);
			return list;
		}
		List<List<IntPoint>> list2 = new List<List<IntPoint>>();
		for (int j = 0; j < points.Count; j++)
		{
			list2.Add(new List<IntPoint>());
			for (int k = 0; k < points[j].Count; k++)
			{
				IntPoint item = new IntPoint((long)((int)(points[j][k].x * 1000f)), (long)((int)(points[j][k].y * 1000f)));
				list2[j].Add(item);
			}
		}
		Clipper clipper2 = new Clipper(0);
		clipper2.AddPath(list2[0], PolyType.ptSubject, true);
		for (int l = 1; l < list2.Count; l++)
		{
			clipper2.AddPath(list2[l], PolyType.ptClip, true);
		}
		List<List<IntPoint>> list3 = new List<List<IntPoint>>();
		clipper2.Execute(ClipType.ctUnion, list3, PolyFillType.pftNonZero);
		ClipperOffset clipperOffset = new ClipperOffset(2.0, 0.25);
		clipperOffset.AddPaths(list3, JoinType.jtSquare, EndType.etClosedPolygon);
		clipperOffset.Execute(ref list3, 0.00019999999494757503);
		List<List<Vector2>> list4 = new List<List<Vector2>>();
		for (int m = 0; m < list3.Count; m++)
		{
			list4.Add(new List<Vector2>());
			for (int n = 0; n < list3[m].Count; n++)
			{
				Vector2 item2 = new Vector2((float)list3[m][n].X / 1000f, (float)list3[m][n].Y / 1000f);
				list4[m].Add(item2);
			}
		}
		return list4;
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x000CF394 File Offset: 0x000CD594
	private static MergeRoom CreateMergeRoom()
	{
		MergeRoomTools.m_mergeRoomNumber++;
		GameObject gameObject = new GameObject("Merge Room " + MergeRoomTools.m_mergeRoomNumber.ToString());
		MergeRoom result = gameObject.AddComponent<MergeRoom>();
		gameObject.AddComponent<CameraZoomController>();
		return result;
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x000CF3D4 File Offset: 0x000CD5D4
	public static void ResetMergeRoomCounter()
	{
		MergeRoomTools.m_mergeRoomNumber = 0;
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x000CF3DC File Offset: 0x000CD5DC
	public static void SetMergeRoomCollider(MergeRoom mergeRoom)
	{
		mergeRoom.gameObject.AddComponent<PolygonCollider2D>();
		PolygonCollider2D component = mergeRoom.gameObject.GetComponent<PolygonCollider2D>();
		component.isTrigger = true;
		Vector2[] array = new Vector2[4];
		array[0] = mergeRoom.gameObject.transform.InverseTransformPoint(mergeRoom.Bounds.min);
		Vector2 v = new Vector2(mergeRoom.Bounds.max.x, mergeRoom.Bounds.min.y);
		array[1] = mergeRoom.gameObject.transform.InverseTransformPoint(v);
		array[2] = mergeRoom.gameObject.transform.InverseTransformPoint(mergeRoom.Bounds.max);
		Vector2 v2 = new Vector2(mergeRoom.Bounds.min.x, mergeRoom.Bounds.max.y);
		array[3] = mergeRoom.gameObject.transform.InverseTransformPoint(v2);
		component.pathCount = 1;
		component.SetPath(0, array);
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x000CF510 File Offset: 0x000CD710
	public static void ConvertCompositeColliderToPolygonCollider(CompositeCollider2D compositeCollider)
	{
		PolygonCollider2D polygonCollider2D = compositeCollider.gameObject.AddComponent<PolygonCollider2D>();
		polygonCollider2D.isTrigger = true;
		polygonCollider2D.pathCount = compositeCollider.pathCount;
		for (int i = 0; i < compositeCollider.pathCount; i++)
		{
			int pathPointCount = compositeCollider.GetPathPointCount(i);
			Vector2[] array = new Vector2[pathPointCount];
			List<Vector2> list = new List<Vector2>();
			compositeCollider.GetPath(i, array);
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			for (int j = 0; j < pathPointCount; j++)
			{
				Vector2 vector = array[j];
				if (vector.x < num)
				{
					num = vector.x;
				}
				if (vector.x > num2)
				{
					num2 = vector.x;
				}
				if (vector.y < num3)
				{
					num3 = vector.y;
				}
				if (vector.y > num4)
				{
					num4 = vector.y;
				}
			}
			list.Add(new Vector2(num, num4));
			list.Add(new Vector2(num2, num4));
			list.Add(new Vector2(num2, num3));
			list.Add(new Vector2(num, num3));
			polygonCollider2D.SetPath(i, list.ToArray());
		}
		for (int k = 0; k < compositeCollider.transform.childCount; k++)
		{
			Collider2D component = compositeCollider.transform.GetChild(k).GetComponent<Collider2D>();
			if (component != null && component.usedByComposite)
			{
				component.enabled = false;
			}
		}
		if (Application.isPlaying)
		{
			UnityEngine.Object.Destroy(compositeCollider);
		}
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x000CF69C File Offset: 0x000CD89C
	public static MergeRoom MergeRooms(BiomeController biomeController, List<Room> connectedRooms)
	{
		GameObject gameObject = MergeRoomTools.Merge(connectedRooms);
		gameObject.transform.SetParent(biomeController.StandardRoomStorageLocation);
		MergeRoom component = gameObject.GetComponent<MergeRoom>();
		MergeRoomTools.SetMergeRoomCollider(component);
		biomeController.MergeRoomCreatedByWorldBuilder(component);
		return component;
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x000CF6D4 File Offset: 0x000CD8D4
	public static void MergeConnectedGridPointManagers(BiomeController biomeController, BiomeType biome)
	{
		List<GridPointManager> list = (from room in biomeController.GridPointManager.GridPointManagers
		where room.RoomMetaData.CanMerge && room.Biome == biome
		select room).ToList<GridPointManager>();
		if (biome == BiomeType.Tower || biome == BiomeType.TowerExterior)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i].RoomType == RoomType.BossEntrance)
				{
					list.RemoveAt(i);
				}
			}
		}
		while (list.Count > 0)
		{
			List<GridPointManager> list2 = new List<GridPointManager>();
			MergeRoomTools.GetConnectedRooms_V2(list2, list.First<GridPointManager>());
			foreach (GridPointManager item in list2)
			{
				list.Remove(item);
			}
			if (list2.Count > 1)
			{
				MergeRoomTools.MergeGridPointManagers(biomeController, list2);
			}
		}
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x000CF7CC File Offset: 0x000CD9CC
	public static void MergeGridPointManagers(BiomeController biomeController, List<GridPointManager> connectedRooms)
	{
		foreach (GridPointManager gridPointManager in connectedRooms)
		{
			gridPointManager.SetMergeWithRooms(connectedRooms);
		}
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x000CF818 File Offset: 0x000CDA18
	private static void GetConnectedRooms(List<Room> connectedRooms, Room room)
	{
		connectedRooms.Add(room);
		if (room.ConnectedRooms != null && room.ConnectedRooms.Count > 0)
		{
			foreach (BaseRoom baseRoom in room.ConnectedRooms)
			{
				if ((baseRoom as Room).RoomType != RoomType.BossEntrance && (baseRoom as Room).AppearanceBiomeType == room.AppearanceBiomeType && (baseRoom as Room).CanMerge && !connectedRooms.Contains(baseRoom))
				{
					MergeRoomTools.GetConnectedRooms(connectedRooms, baseRoom as Room);
				}
			}
		}
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x000CF8C8 File Offset: 0x000CDAC8
	private static void GetConnectedRooms_V2(List<GridPointManager> connectedRooms, GridPointManager room)
	{
		connectedRooms.Add(room);
		List<GridPointManager> list = new List<GridPointManager>();
		foreach (DoorLocation doorLocation in room.DoorLocations)
		{
			GridPointManager connectedRoom = room.GetConnectedRoom(doorLocation);
			if (connectedRoom != null && !list.Contains(connectedRoom) && ((room.Biome != BiomeType.Tower && room.Biome != BiomeType.TowerExterior) || connectedRoom.RoomType != RoomType.BossEntrance))
			{
				list.Add(connectedRoom);
			}
		}
		if (list.Count > 0)
		{
			foreach (GridPointManager gridPointManager in list)
			{
				if (gridPointManager.Biome == room.Biome && gridPointManager.RoomMetaData.CanMerge && !connectedRooms.Contains(gridPointManager))
				{
					MergeRoomTools.GetConnectedRooms_V2(connectedRooms, gridPointManager);
				}
			}
		}
	}

	// Token: 0x04002D38 RID: 11576
	private static int m_mergeRoomNumber = 0;

	// Token: 0x04002D39 RID: 11577
	private const int DIRECTION_OVERRIDE_DEFAULT_VALUE = 100;

	// Token: 0x04002D3A RID: 11578
	private static List<GridPoint> m_gridPointsHelper = new List<GridPoint>();

	// Token: 0x04002D3B RID: 11579
	private static List<Vector2Int> m_gridPointsToFillHelper = new List<Vector2Int>();

	// Token: 0x04002D3C RID: 11580
	private static List<Ferr2DT_PathTerrain> m_decoTerrainHelper = new List<Ferr2DT_PathTerrain>();

	// Token: 0x04002D3D RID: 11581
	private static List<Vector2> m_pointsList = new List<Vector2>();

	// Token: 0x04002D3E RID: 11582
	private static List<Ferr2DT_PathTerrain> m_mergeTerrainHelper = new List<Ferr2DT_PathTerrain>();

	// Token: 0x04002D3F RID: 11583
	private static List<Ferr2DT_PathTerrain> m_normalTerrainHelper = new List<Ferr2DT_PathTerrain>();

	// Token: 0x04002D40 RID: 11584
	private static List<Ferr2DT_PathTerrain> m_invertedTerrainHelper = new List<Ferr2DT_PathTerrain>();
}
