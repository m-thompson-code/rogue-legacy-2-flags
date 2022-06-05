using System;
using System.Collections;
using System.Collections.Generic;
using Spawn;
using UnityEngine;

// Token: 0x02000AC9 RID: 2761
public class CreateTransitionPoints_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052FF RID: 21247 RVA: 0x0002D22F File Offset: 0x0002B42F
	public IEnumerator Run(BiomeController biomeController)
	{
		List<Door> transitionPoints = this.GetTransitionPoints(biomeController);
		CreateTransitionPoints_BuildStage.PopulateDataTable();
		using (List<Door>.Enumerator enumerator = transitionPoints.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Door door = enumerator.Current;
				CreateTransitionPoints_BuildStage.CreateTransitionPoint(door, door.TransitionsToBiome);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06005300 RID: 21248 RVA: 0x0013A794 File Offset: 0x00138994
	private static void CreateTransitionPoint(Door door, BiomeType doorLeadsToBiome)
	{
		Prop[] array = CreateTransitionPoints_BuildStage.m_dataTable[door.Side].DefaultProps;
		for (int i = 0; i < CreateTransitionPoints_BuildStage.m_dataTable[door.Side].PropsPerBiome.Length; i++)
		{
			if (CreateTransitionPoints_BuildStage.m_dataTable[door.Side].PropsPerBiome[i].Biome == doorLeadsToBiome)
			{
				array = CreateTransitionPoints_BuildStage.m_dataTable[door.Side].PropsPerBiome[i].Props;
				break;
			}
		}
		if (array == null || array.Length == 0 || array[0] == null)
		{
			return;
		}
		Prop prop = UnityEngine.Object.Instantiate<Prop>(array[0]);
		if (door.Side == RoomSide.Left)
		{
			prop.transform.localScale = new Vector3(-1f * prop.transform.localScale.x, prop.transform.localScale.y, prop.transform.localScale.z);
		}
		prop.transform.SetParent(door.Room.gameObject.transform);
		prop.transform.position = door.CenterPoint;
		prop.SetRoom(door.Room);
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x0002D245 File Offset: 0x0002B445
	public static IEnumerator RunFromLevelEditor(BiomeType leadsToBiome, Door door)
	{
		if (!GameUtility.IsInLevelEditor)
		{
			yield break;
		}
		CreateTransitionPoints_BuildStage.PopulateDataTable();
		if (leadsToBiome != BiomeType.None)
		{
			CreateTransitionPoints_BuildStage.CreateTransitionPoint(door, leadsToBiome);
		}
		yield break;
	}

	// Token: 0x06005302 RID: 21250 RVA: 0x0013A8C0 File Offset: 0x00138AC0
	private static void PopulateDataTable()
	{
		foreach (RoomSide key in new RoomSide[]
		{
			RoomSide.Left,
			RoomSide.Right,
			RoomSide.Top,
			RoomSide.Bottom
		})
		{
			if (!CreateTransitionPoints_BuildStage.m_dataTable.ContainsKey(key))
			{
				string path = "";
				switch (key)
				{
				case RoomSide.Top:
					path = CreateTransitionPoints_BuildStage.TOP_DOOR_DATA;
					break;
				case RoomSide.Bottom:
					path = CreateTransitionPoints_BuildStage.BOTTOM_DOOR_DATA;
					break;
				case RoomSide.Left:
				case RoomSide.Right:
					path = CreateTransitionPoints_BuildStage.SIDE_DOOR_DATA;
					break;
				}
				PropSpawnControllerData value = CDGResources.Load<PropSpawnControllerData>(path, "", true);
				CreateTransitionPoints_BuildStage.m_dataTable.Add(key, value);
			}
		}
	}

	// Token: 0x06005303 RID: 21251 RVA: 0x0013A950 File Offset: 0x00138B50
	private List<Door> GetTransitionPoints(BiomeController biomeController)
	{
		List<Door> list = new List<Door>();
		for (int i = 0; i < biomeController.Rooms.Count; i++)
		{
			if (biomeController.Rooms[i].Doors.Count != 0)
			{
				for (int j = 0; j < biomeController.Rooms[i].Doors.Count; j++)
				{
					Door door = biomeController.Rooms[i].Doors[j];
					Door connectedDoor = door.ConnectedDoor;
					if (biomeController.Rooms[i].Doors[j].IsBiomeTransitionPoint)
					{
						list.Add(biomeController.Rooms[i].Doors[j]);
					}
					else if (!connectedDoor.IsNativeNull() && ((door.Room.AppearanceBiomeType == BiomeType.Tower && connectedDoor.Room.AppearanceBiomeType == BiomeType.TowerExterior) || (door.Room.AppearanceBiomeType == BiomeType.TowerExterior && connectedDoor.Room.AppearanceBiomeType == BiomeType.Tower)))
					{
						list.Add(biomeController.Rooms[i].Doors[j]);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x04003E3E RID: 15934
	private static string TOP_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointTop_Editor_Data";

	// Token: 0x04003E3F RID: 15935
	private static string BOTTOM_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointBottom_Editor_Data";

	// Token: 0x04003E40 RID: 15936
	private static string SIDE_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointSide_Editor_Data";

	// Token: 0x04003E41 RID: 15937
	private static Dictionary<RoomSide, PropSpawnControllerData> m_dataTable = new Dictionary<RoomSide, PropSpawnControllerData>();
}
