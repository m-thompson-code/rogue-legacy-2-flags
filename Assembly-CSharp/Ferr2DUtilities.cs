using System;
using System.Collections.Generic;
using ClipperLibFerr;
using Ferr;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public static class Ferr2DUtilities
{
	// Token: 0x060043D2 RID: 17362 RVA: 0x000EE84F File Offset: 0x000ECA4F
	public static void ChangeMaterial(Ferr2DT_PathTerrain obj1, Ferr2DT_PathTerrain obj2)
	{
		obj1.SetMaterial(obj2.TerrainMaterial);
		obj1.Build(true);
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x000EE864 File Offset: 0x000ECA64
	public static List<GameObject> Boolean(ClipType clipType, Ferr2DT_PathTerrain subject, List<Ferr2DT_PathTerrain> clipList, bool roundToInt = true)
	{
		Ferr2DUtilities.m_subjIPList.Clear();
		Ferr2DUtilities.m_clipper.Clear();
		Ferr2DUtilities.m_edgeDataList.Clear();
		Ferr2DUtilities.m_newObjList.Clear();
		foreach (List<IntPoint> list in Ferr2DUtilities.m_ipResultList)
		{
			list.Clear();
		}
		Ferr2DUtilities.m_ipResultList.Clear();
		List<Vector2> finalPath = subject.PathData.GetFinalPath();
		Vector2 vector = subject.gameObject.transform.position;
		for (int i = 0; i < finalPath.Count; i++)
		{
			Ferr2DUtilities.m_subjIPList.Add(new IntPoint((double)((finalPath[i].x + vector.x) * 1000f), (double)((finalPath[i].y + vector.y) * 1000f)));
		}
		Ferr2DUtilities.m_clipper.AddPath(Ferr2DUtilities.m_subjIPList, PolyType.ptSubject, true);
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in clipList)
		{
			if (!ferr2DT_PathTerrain.DisableMerging)
			{
				Ferr2DUtilities.m_clipIPList.Clear();
				List<Vector2> finalPath2 = ferr2DT_PathTerrain.PathData.GetFinalPath();
				Vector2 vector2 = ferr2DT_PathTerrain.gameObject.transform.position;
				for (int j = 0; j < finalPath2.Count; j++)
				{
					Ferr2DUtilities.m_clipIPList.Add(new IntPoint((double)((finalPath2[j].x + vector2.x) * 1000f), (double)((finalPath2[j].y + vector2.y) * 1000f)));
				}
				Ferr2DUtilities.m_clipper.AddPath(Ferr2DUtilities.m_clipIPList, PolyType.ptClip, true);
			}
		}
		Ferr2DUtilities.CDGFerr2D_EdgeData item = default(Ferr2DUtilities.CDGFerr2D_EdgeData);
		if (Ferr2DUtilities.m_clipper.Execute(clipType, Ferr2DUtilities.m_ipResultList, PolyFillType.pftNonZero))
		{
			Ferr2DUtilities.m_clipperOffset.Clear();
			Ferr2DUtilities.m_clipperOffset.AddPaths(Ferr2DUtilities.m_ipResultList, JoinType.jtSquare, EndType.etClosedPolygon);
			Ferr2DUtilities.m_clipperOffset.Execute(ref Ferr2DUtilities.m_ipResultList, 0.00019999999494757503);
			List<Ferr2D_PointData> data = subject.PathData.GetData();
			List<Vector2> finalPath3 = subject.PathData.GetFinalPath();
			for (int k = 0; k < data.Count; k++)
			{
				Ferr2D_PointData ferr2D_PointData = data[k];
				if (ferr2D_PointData.directionOverride != 100)
				{
					item.pointData = ferr2D_PointData;
					item.rightPoint = new IntPoint((double)((finalPath3[k].x + vector.x) * 1000f), (double)((finalPath3[k].y + vector.y) * 1000f));
					if (k < data.Count - 1)
					{
						item.leftPoint = new IntPoint((double)((finalPath3[k + 1].x + vector.x) * 1000f), (double)((finalPath3[k + 1].y + vector.y) * 1000f));
					}
					else
					{
						item.leftPoint = new IntPoint((double)((finalPath3[0].x + vector.x) * 1000f), (double)((finalPath3[0].y + vector.y) * 1000f));
					}
					Ferr2DUtilities.m_edgeDataList.Add(item);
				}
			}
			for (int l = 0; l < Ferr2DUtilities.m_ipResultList.Count; l++)
			{
				List<IntPoint> list2 = Ferr2DUtilities.m_ipResultList[l];
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(subject.gameObject);
				if (gameObject.transform.parent != subject.transform.parent)
				{
					gameObject.transform.parent = subject.transform.parent;
					gameObject.transform.position += subject.transform.parent.position;
				}
				Ferr2DUtilities.m_newObjList.Add(gameObject);
				Ferr2DT_PathTerrain component = gameObject.GetComponent<Ferr2DT_PathTerrain>();
				component.ClearPoints();
				for (int m = 0; m < list2.Count; m++)
				{
					IntPoint intPoint = list2[m];
					Vector2 vector3 = new Vector2((float)intPoint.X / 1000f - vector.x, (float)intPoint.Y / 1000f - vector.y);
					if (roundToInt)
					{
						vector3.x = (float)Mathf.RoundToInt(vector3.x);
						vector3.y = (float)Mathf.RoundToInt(vector3.y);
					}
					component.AddPoint(vector3, -1, PointType.Sharp);
				}
			}
			int num = 0;
			using (List<GameObject>.Enumerator enumerator3 = Ferr2DUtilities.m_newObjList.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					GameObject gameObject2 = enumerator3.Current;
					Ferr2DT_PathTerrain component2 = gameObject2.GetComponent<Ferr2DT_PathTerrain>();
					List<Vector2> finalPath4 = component2.PathData.GetFinalPath();
					for (int n = 0; n < finalPath4.Count; n++)
					{
						Vector2 lhs = finalPath4[n];
						foreach (Ferr2DUtilities.CDGFerr2D_EdgeData cdgferr2D_EdgeData in Ferr2DUtilities.m_edgeDataList)
						{
							Vector2 vector4 = new Vector2((float)cdgferr2D_EdgeData.leftPoint.X / 1000f - vector.x, (float)cdgferr2D_EdgeData.leftPoint.Y / 1000f - vector.y);
							if (roundToInt)
							{
								vector4.x = (float)Mathf.RoundToInt(vector4.x);
								vector4.y = (float)Mathf.RoundToInt(vector4.y);
							}
							if (lhs == vector4)
							{
								if (n == 0)
								{
									component2.PathData.SetData(component2.PathData.Count - 1, cdgferr2D_EdgeData.pointData);
								}
								else
								{
									component2.PathData.SetData(n - 1, cdgferr2D_EdgeData.pointData);
								}
							}
							else
							{
								vector4 = new Vector2((float)cdgferr2D_EdgeData.rightPoint.X / 1000f - vector.x, (float)cdgferr2D_EdgeData.rightPoint.Y / 1000f - vector.y);
								if (roundToInt)
								{
									vector4.x = (float)Mathf.RoundToInt(vector4.x);
									vector4.y = (float)Mathf.RoundToInt(vector4.y);
								}
								if (lhs == vector4)
								{
									component2.PathData.SetData(n, cdgferr2D_EdgeData.pointData);
								}
							}
						}
					}
					if (num > 0)
					{
						gameObject2.name = subject.name + " (" + num.ToString() + ")";
					}
					else
					{
						gameObject2.name = subject.name;
					}
					num++;
					bool rebuild = !Application.isPlaying;
					Ferr2DUtilities.RecentreFerr2DTerrain(component2, rebuild, true);
				}
				goto IL_700;
			}
		}
		Debug.Log("Collision between Ferr2D objects was not found. CDGFerr2D.Boolean: " + clipType.ToString() + " failed.");
		IL_700:
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain2 in clipList)
		{
			if (ferr2DT_PathTerrain2.DisableMerging)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(ferr2DT_PathTerrain2.gameObject);
				if (Application.isPlaying)
				{
					gameObject3.transform.parent = subject.transform.parent;
					gameObject3.transform.position = ferr2DT_PathTerrain2.transform.position;
				}
				else if (gameObject3.transform.parent != subject.transform.parent)
				{
					gameObject3.transform.parent = subject.transform.parent;
					gameObject3.transform.position += subject.transform.parent.position;
				}
				Ferr2DUtilities.m_newObjList.Add(gameObject3);
			}
		}
		return Ferr2DUtilities.m_newObjList;
	}

	// Token: 0x060043D4 RID: 17364 RVA: 0x000EF0E0 File Offset: 0x000ED2E0
	public static List<GameObject> EasyBoolean(ClipType clipType, List<Ferr2DT_PathTerrain> objsToCollide, Ferr2DUtilities.Ferr2DUndoType undoType, bool roundToInt = true)
	{
		Ferr2DUtilities.m_boolNewObjList.Clear();
		int layer = LayerMask.NameToLayer("HELPER_NOTOUCH");
		int layer2 = objsToCollide[0].gameObject.layer;
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain in objsToCollide)
		{
			if (ferr2DT_PathTerrain != null)
			{
				ferr2DT_PathTerrain.gameObject.layer = layer;
			}
		}
		Ferr2DUtilities.m_colliderFilter.SetLayerMask(LayerMask.GetMask(new string[]
		{
			"HELPER_NOTOUCH"
		}));
		Ferr2DUtilities.m_colliderFilter.useTriggers = true;
		for (int i = 0; i < objsToCollide.Count; i++)
		{
			Ferr2DT_PathTerrain ferr2DT_PathTerrain2 = objsToCollide[i];
			if (ferr2DT_PathTerrain2 != null)
			{
				Ferr2DUtilities.m_daisyChainedColliderList.Clear();
				Ferr2DUtilities.m_terrainCollList.Clear();
				Collider2D component = ferr2DT_PathTerrain2.GetComponent<Collider2D>();
				List<GameObject> list = null;
				if (component != null)
				{
					Ferr2DUtilities.DaisyChainOverlapColliderRecursionLoop(component, Ferr2DUtilities.m_daisyChainedColliderList, Ferr2DUtilities.m_colliderFilter);
					foreach (Collider2D collider2D in Ferr2DUtilities.m_daisyChainedColliderList)
					{
						if (collider2D != null && collider2D != component)
						{
							Ferr2DT_PathTerrain component2 = collider2D.GetComponent<Ferr2DT_PathTerrain>();
							if (component2 != null)
							{
								Ferr2DUtilities.m_terrainCollList.Add(component2);
							}
						}
					}
					if (Ferr2DUtilities.m_terrainCollList.Count > 0)
					{
						list = Ferr2DUtilities.Boolean(clipType, ferr2DT_PathTerrain2, Ferr2DUtilities.m_terrainCollList, roundToInt);
					}
				}
				else
				{
					Debug.Log("Cannot perform Boolean. Subject: " + ferr2DT_PathTerrain2.name + " has no collider.");
				}
				if (list != null && list.Count > 0)
				{
					switch (undoType)
					{
					case Ferr2DUtilities.Ferr2DUndoType.None:
						goto IL_25D;
					case Ferr2DUtilities.Ferr2DUndoType.Destroy:
						UnityEngine.Object.Destroy(ferr2DT_PathTerrain2.gameObject);
						using (List<Collider2D>.Enumerator enumerator2 = Ferr2DUtilities.m_daisyChainedColliderList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Collider2D collider2D2 = enumerator2.Current;
								if (collider2D2 != null)
								{
									UnityEngine.Object.Destroy(collider2D2.gameObject);
								}
							}
							goto IL_25D;
						}
						break;
					case Ferr2DUtilities.Ferr2DUndoType.DestroyImmediate:
						break;
					default:
						goto IL_25D;
					}
					UnityEngine.Object.DestroyImmediate(ferr2DT_PathTerrain2.gameObject);
					foreach (Collider2D collider2D3 in Ferr2DUtilities.m_daisyChainedColliderList)
					{
						if (collider2D3 != null)
						{
							UnityEngine.Object.DestroyImmediate(collider2D3.gameObject);
						}
					}
				}
				IL_25D:
				if (list != null)
				{
					Ferr2DUtilities.m_boolNewObjList.AddRange(list);
				}
			}
		}
		foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain3 in objsToCollide)
		{
			if (ferr2DT_PathTerrain3 != null)
			{
				ferr2DT_PathTerrain3.gameObject.layer = layer2;
			}
		}
		foreach (GameObject gameObject in Ferr2DUtilities.m_boolNewObjList)
		{
			if (gameObject != null)
			{
				gameObject.layer = layer2;
			}
		}
		return Ferr2DUtilities.m_boolNewObjList;
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x000EF444 File Offset: 0x000ED644
	public static List<GameObject> FastEasyBoolean(ClipType clipType, List<Ferr2DT_PathTerrain> clipList, Ferr2DUtilities.Ferr2DUndoType undoType, bool roundToInt = true)
	{
		if (clipList != null && clipList.Count > 0)
		{
			Ferr2DT_PathTerrain subjectTerrain = clipList[0];
			return Ferr2DUtilities.FastEasyBoolean(clipType, subjectTerrain, clipList, undoType, roundToInt);
		}
		throw new ArgumentNullException("<color=red>[Ferr2DUtilities] FastEasyBoolean was passed an emtpy clipList</color>");
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x000EF47C File Offset: 0x000ED67C
	public static List<GameObject> FastEasyBoolean(ClipType clipType, Ferr2DT_PathTerrain subjectTerrain, List<Ferr2DT_PathTerrain> clipList, Ferr2DUtilities.Ferr2DUndoType undoType, bool roundToInt = true)
	{
		if (clipList == null)
		{
			throw new ArgumentNullException("objsToCollide");
		}
		if (clipList.Count == 0)
		{
			return null;
		}
		if (clipList.Contains(subjectTerrain))
		{
			clipList.Remove(subjectTerrain);
		}
		Ferr2DUtilities.m_boolNewObjList.Clear();
		Ferr2DT_PathTerrain ferr2DT_PathTerrain = subjectTerrain;
		if (ferr2DT_PathTerrain != null)
		{
			UnityEngine.Object component = ferr2DT_PathTerrain.GetComponent<Collider2D>();
			List<GameObject> list = null;
			Ferr2DUtilities.m_terrainCollList.Clear();
			if (component != null)
			{
				Ferr2DUtilities.m_terrainCollList.AddRange(clipList);
				if (clipType != ClipType.ctDifference)
				{
					list = Ferr2DUtilities.Boolean(clipType, ferr2DT_PathTerrain, Ferr2DUtilities.m_terrainCollList, roundToInt);
				}
				else
				{
					List<Ferr2DT_PathTerrain> list2 = new List<Ferr2DT_PathTerrain>();
					list = new List<GameObject>();
					for (int i = 0; i < Ferr2DUtilities.m_terrainCollList.Count; i++)
					{
						ferr2DT_PathTerrain = Ferr2DUtilities.m_terrainCollList[i];
						list2.Clear();
						list2.Add(subjectTerrain);
						List<GameObject> collection = Ferr2DUtilities.Boolean(clipType, ferr2DT_PathTerrain, list2, roundToInt);
						list.AddRange(collection);
					}
				}
				Ferr2DUtilities.m_terrainCollList.Add(subjectTerrain);
			}
			else
			{
				Debug.Log("Cannot perform Boolean. Subject: " + ferr2DT_PathTerrain.name + " has no collider.");
			}
			if (list != null && list.Count > 0)
			{
				switch (undoType)
				{
				case Ferr2DUtilities.Ferr2DUndoType.None:
					goto IL_1BF;
				case Ferr2DUtilities.Ferr2DUndoType.Destroy:
					UnityEngine.Object.Destroy(ferr2DT_PathTerrain.gameObject);
					using (List<Ferr2DT_PathTerrain>.Enumerator enumerator = Ferr2DUtilities.m_terrainCollList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Ferr2DT_PathTerrain ferr2DT_PathTerrain2 = enumerator.Current;
							if (ferr2DT_PathTerrain2 != null)
							{
								UnityEngine.Object.Destroy(ferr2DT_PathTerrain2.gameObject);
							}
						}
						goto IL_1BF;
					}
					break;
				case Ferr2DUtilities.Ferr2DUndoType.DestroyImmediate:
					break;
				default:
					goto IL_1BF;
				}
				UnityEngine.Object.DestroyImmediate(ferr2DT_PathTerrain.gameObject);
				foreach (Ferr2DT_PathTerrain ferr2DT_PathTerrain3 in Ferr2DUtilities.m_terrainCollList)
				{
					if (ferr2DT_PathTerrain3 != null)
					{
						UnityEngine.Object.DestroyImmediate(ferr2DT_PathTerrain3.gameObject);
					}
				}
			}
			IL_1BF:
			if (list != null)
			{
				Ferr2DUtilities.m_boolNewObjList.AddRange(list);
			}
		}
		return Ferr2DUtilities.m_boolNewObjList;
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x000EF678 File Offset: 0x000ED878
	private static void DaisyChainOverlapColliderRecursionLoop(Collider2D subject, List<Collider2D> listToModify, ContactFilter2D contactFilter)
	{
		Collider2D[] array = new Collider2D[10];
		subject.OverlapCollider(contactFilter, array);
		foreach (Collider2D collider2D in array)
		{
			if (collider2D != null && !listToModify.Contains(collider2D))
			{
				listToModify.Add(collider2D);
				Ferr2DUtilities.DaisyChainOverlapColliderRecursionLoop(collider2D, listToModify, contactFilter);
			}
		}
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x000EF6CC File Offset: 0x000ED8CC
	public static bool RecentreFerr2DTerrain(Ferr2DT_PathTerrain terrain, bool rebuild, bool roundToInt = true)
	{
		if (terrain == null)
		{
			return false;
		}
		Vector2 vector = PathUtil.Average(terrain.PathData.GetPathRaw());
		Vector2 vector2 = vector;
		if (roundToInt)
		{
			vector2.x = (float)Mathf.RoundToInt(vector2.x);
			vector2.y = (float)Mathf.RoundToInt(vector2.y);
		}
		bool flag = vector2 != Vector2.zero;
		if (flag)
		{
			Vector3 vector3 = terrain.transform.position + vector;
			Vector3 vector4 = new Vector3((float)((int)Math.Round((double)vector3.x)), (float)((int)Math.Round((double)vector3.y)), vector3.z);
			vector -= vector3 - vector4;
			terrain.transform.position = vector4;
			List<Vector2> pathRaw = terrain.PathData.GetPathRaw();
			for (int i = 0; i < pathRaw.Count; i++)
			{
				Vector2 vector5 = pathRaw[i] - vector;
				if (roundToInt)
				{
					vector5.x = (float)Mathf.RoundToInt(vector5.x);
					vector5.y = (float)Mathf.RoundToInt(vector5.y);
				}
				terrain.PathData.Set(i, vector5);
			}
		}
		if (rebuild)
		{
			Bounds rhs = default(Bounds);
			if (!Application.isPlaying && !flag)
			{
				Collider2D component = terrain.GetComponent<Collider2D>();
				if (component != null)
				{
					rhs = component.bounds;
				}
			}
			terrain.Build(true);
			if (!Application.isPlaying && !flag)
			{
				Collider2D component2 = terrain.GetComponent<Collider2D>();
				if (component2 != null && component2.bounds != rhs)
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x000EF868 File Offset: 0x000EDA68
	public static void Flip(Ferr2DT_PathTerrain terrain, bool flipHorizontally, bool flipVertically, bool recentre = true)
	{
		if (!flipHorizontally && !flipVertically)
		{
			return;
		}
		Ferr2DUtilities.m_invertedPointList.Clear();
		Ferr2DUtilities.m_pointDataList.Clear();
		Ferr2DUtilities.m_pointList.Clear();
		if (recentre)
		{
			Ferr2DUtilities.RecentreFerr2DTerrain(terrain, false, true);
		}
		Ferr2DUtilities.m_pointList.AddRange(terrain.PathData.GetFinalPath());
		Ferr2DUtilities.m_pointDataList.AddRange(terrain.PathData.GetData());
		if (flipHorizontally)
		{
			for (int i = Ferr2DUtilities.m_pointList.Count - 1; i >= 0; i--)
			{
				Ferr2DUtilities.m_invertedPointList.Add(new Vector2(Ferr2DUtilities.m_pointList[i].x * -1f, Ferr2DUtilities.m_pointList[i].y));
			}
			if (flipVertically)
			{
				Ferr2DUtilities.m_pointList.Clear();
				Ferr2DUtilities.m_pointList.AddRange(Ferr2DUtilities.m_invertedPointList);
				Ferr2DUtilities.m_invertedPointList.Clear();
				for (int j = Ferr2DUtilities.m_pointList.Count - 1; j >= 0; j--)
				{
					Ferr2DUtilities.m_invertedPointList.Add(new Vector2(Ferr2DUtilities.m_pointList[j].x, Ferr2DUtilities.m_pointList[j].y * -1f));
				}
			}
		}
		else if (flipVertically)
		{
			for (int k = Ferr2DUtilities.m_pointList.Count - 1; k >= 0; k--)
			{
				Ferr2DUtilities.m_invertedPointList.Add(new Vector2(Ferr2DUtilities.m_pointList[k].x, Ferr2DUtilities.m_pointList[k].y * -1f));
			}
		}
		for (int l = 0; l < Ferr2DUtilities.m_invertedPointList.Count; l++)
		{
			terrain.PathData.Set(l, Ferr2DUtilities.m_invertedPointList[l]);
			int num = Ferr2DUtilities.m_invertedPointList.Count - 2 - l;
			if (num < 0)
			{
				num += Ferr2DUtilities.m_invertedPointList.Count;
			}
			terrain.PathData.SetData(l, Ferr2DUtilities.m_pointDataList[num]);
		}
		terrain.RecreateCollider();
	}

	// Token: 0x040039FF RID: 14847
	private static List<IntPoint> m_subjIPList = new List<IntPoint>();

	// Token: 0x04003A00 RID: 14848
	private static List<IntPoint> m_clipIPList = new List<IntPoint>();

	// Token: 0x04003A01 RID: 14849
	private static List<Ferr2DUtilities.CDGFerr2D_EdgeData> m_edgeDataList = new List<Ferr2DUtilities.CDGFerr2D_EdgeData>();

	// Token: 0x04003A02 RID: 14850
	private static Clipper m_clipper = new Clipper(0);

	// Token: 0x04003A03 RID: 14851
	private static ClipperOffset m_clipperOffset = new ClipperOffset(2.0, 0.25);

	// Token: 0x04003A04 RID: 14852
	private static List<List<IntPoint>> m_ipResultList = new List<List<IntPoint>>();

	// Token: 0x04003A05 RID: 14853
	private static List<Collider2D> m_daisyChainedColliderList = new List<Collider2D>();

	// Token: 0x04003A06 RID: 14854
	private static List<Ferr2DT_PathTerrain> m_terrainCollList = new List<Ferr2DT_PathTerrain>();

	// Token: 0x04003A07 RID: 14855
	private static ContactFilter2D m_colliderFilter = default(ContactFilter2D);

	// Token: 0x04003A08 RID: 14856
	private static List<GameObject> m_boolNewObjList = new List<GameObject>();

	// Token: 0x04003A09 RID: 14857
	private static List<GameObject> m_newObjList = new List<GameObject>();

	// Token: 0x04003A0A RID: 14858
	private static List<Vector2> m_pointList = new List<Vector2>(10);

	// Token: 0x04003A0B RID: 14859
	private static List<Vector2> m_invertedPointList = new List<Vector2>(10);

	// Token: 0x04003A0C RID: 14860
	private static List<Ferr2D_PointData> m_pointDataList = new List<Ferr2D_PointData>(10);

	// Token: 0x02000E38 RID: 3640
	public struct CDGFerr2D_EdgeData
	{
		// Token: 0x0400573B RID: 22331
		public Ferr2D_PointData pointData;

		// Token: 0x0400573C RID: 22332
		public IntPoint leftPoint;

		// Token: 0x0400573D RID: 22333
		public IntPoint rightPoint;
	}

	// Token: 0x02000E39 RID: 3641
	public enum Ferr2DUndoType
	{
		// Token: 0x0400573F RID: 22335
		None,
		// Token: 0x04005740 RID: 22336
		Destroy,
		// Token: 0x04005741 RID: 22337
		DestroyImmediate
	}
}
