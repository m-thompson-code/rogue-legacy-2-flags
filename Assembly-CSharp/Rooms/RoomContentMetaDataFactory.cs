using System;
using UnityEngine;

namespace Rooms
{
	// Token: 0x020008CF RID: 2255
	public static class RoomContentMetaDataFactory
	{
		// Token: 0x060049F1 RID: 18929 RVA: 0x0010A69C File Offset: 0x0010889C
		public static string CreateContentMetaData(RoomMetaData room)
		{
			Room prefab = room.GetPrefab(true);
			string str = room.ID.ToString() + "_RoomContent";
			if (prefab != null)
			{
				SpawnLogicController[] componentsInChildren = prefab.GetComponentsInChildren<SpawnLogicController>(true);
				RoomContentMetaData roomContentMetaData = new RoomContentMetaData();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					RoomContentType roomContentType = RoomContentType.Other;
					EnemySpawnController component = componentsInChildren[i].GetComponent<EnemySpawnController>();
					if (component != null)
					{
						roomContentType = RoomContentType.Enemy;
					}
					ChestSpawnController component2 = componentsInChildren[i].GetComponent<ChestSpawnController>();
					if (component2 != null)
					{
						roomContentType = RoomContentType.Chest;
					}
					if (component != null && component2 != null)
					{
						throw new Exception(string.Format("A GameObject in Room ({0}) has both an Enemy and Chest Spawn Controller Component attached. This is not valid.", room.ID));
					}
					if (roomContentType != RoomContentType.Other)
					{
						Vector2 localPosition = componentsInChildren[i].transform.localPosition;
						roomContentMetaData.AddEntry(roomContentType, localPosition, componentsInChildren[i].SpawnConditions);
					}
				}
				GlobalTeleporterController[] componentsInChildren2 = prefab.GetComponentsInChildren<GlobalTeleporterController>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Vector2 localPosition2 = componentsInChildren2[j].transform.localPosition;
					roomContentMetaData.AddEntry(RoomContentType.Teleporter, localPosition2, new SpawnConditionsEntry[0]);
				}
			}
			return "Scriptable Objects/Room Content/" + room.ID.SceneName + "/" + str;
		}

		// Token: 0x04003E34 RID: 15924
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Room Content";

		// Token: 0x04003E35 RID: 15925
		public const string RESOURCE_PATH = "Scriptable Objects/Room Content";
	}
}
