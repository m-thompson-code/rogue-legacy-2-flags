using System;
using System.Collections;
using System.Collections.Generic;
using Spawn;
using UnityEngine;

// Token: 0x02000667 RID: 1639
public class CreateTransitionPoints_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B38 RID: 15160 RVA: 0x000CB7C5 File Offset: 0x000C99C5
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

	// Token: 0x06003B39 RID: 15161 RVA: 0x000CB7DC File Offset: 0x000C99DC
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

	// Token: 0x06003B3A RID: 15162 RVA: 0x000CB907 File Offset: 0x000C9B07
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

	// Token: 0x06003B3B RID: 15163 RVA: 0x000CB920 File Offset: 0x000C9B20
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

	// Token: 0x06003B3C RID: 15164 RVA: 0x000CB9B0 File Offset: 0x000C9BB0
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

	// Token: 0x04002D06 RID: 11526
	private static string TOP_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointTop_Editor_Data";

	// Token: 0x04002D07 RID: 11527
	private static string BOTTOM_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointBottom_Editor_Data";

	// Token: 0x04002D08 RID: 11528
	private static string SIDE_DOOR_DATA = "Scriptable Objects/Spawn Controller Data/Props/TransitionPointSide_Editor_Data";

	// Token: 0x04002D09 RID: 11529
	private static Dictionary<RoomSide, PropSpawnControllerData> m_dataTable = new Dictionary<RoomSide, PropSpawnControllerData>();
}
