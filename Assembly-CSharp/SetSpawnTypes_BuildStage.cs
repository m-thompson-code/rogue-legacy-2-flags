using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000671 RID: 1649
public class SetSpawnTypes_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B68 RID: 15208 RVA: 0x000CC48B File Offset: 0x000CA68B
	public IEnumerator Run(BiomeController biomeController)
	{
		Stopwatch timer = new Stopwatch();
		timer.Start();
		if (biomeController.Rooms != null && biomeController.Rooms.Count > 0)
		{
			foreach (BaseRoom baseRoom in biomeController.Rooms)
			{
				Room room = baseRoom as Room;
				if (room != null)
				{
					this.SetSpawnTypes(room);
				}
				else
				{
					MergeRoom spawnTypes = baseRoom as MergeRoom;
					this.SetSpawnTypes(spawnTypes);
				}
				if (timer.Elapsed.TotalMilliseconds >= 30.0)
				{
					yield return null;
					timer.Restart();
				}
			}
			List<BaseRoom>.Enumerator enumerator = default(List<BaseRoom>.Enumerator);
		}
		else
		{
			UnityEngine.Debug.LogFormat("<color=red>[{0}] BiomeController ({1})'s Rooms Property is null or empty.</color>", new object[]
			{
				this,
				biomeController.Biome
			});
		}
		yield break;
		yield break;
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x000CC4A4 File Offset: 0x000CA6A4
	private void SetSpawnTypes(Room room)
	{
		if (room.GridPointManager == null)
		{
			return;
		}
		RNGManager.SetSeed(RngID.Prop_RoomSeed, room.GridPointManager.PropSeed);
		RNGManager.SetSeed(RngID.Deco_RoomSeed, room.GridPointManager.DecoSeed);
		RNGManager.SetSeed(RngID.Enemy_RoomSeed, room.GridPointManager.EnemySeed);
		RNGManager.SetSeed(RngID.Chest_RoomSeed, room.GridPointManager.ChestSeed);
		RNGManager.SetSeed(RngID.Hazards_RoomSeed, room.GridPointManager.HazardSeed);
		ISpawnController[] spawnControllers = room.SpawnControllerManager.SpawnControllers;
		for (int i = 0; i < spawnControllers.Length; i++)
		{
			ISetSpawnType setSpawnType = spawnControllers[i] as ISetSpawnType;
			if (setSpawnType != null)
			{
				setSpawnType.SetSpawnType();
			}
		}
		room.RoomEnemyManager.SetSpawnType();
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x000CC550 File Offset: 0x000CA750
	private void SetSpawnTypes(MergeRoom room)
	{
		for (int i = 0; i < room.StandaloneGridPointManagers.Length; i++)
		{
			if (room.StandaloneGridPointManagers[i] != null)
			{
				GridPointManager gridPointManager = room.StandaloneGridPointManagers[i];
				RNGManager.SetSeed(RngID.Prop_RoomSeed, gridPointManager.PropSeed);
				RNGManager.SetSeed(RngID.Deco_RoomSeed, gridPointManager.DecoSeed);
				RNGManager.SetSeed(RngID.Enemy_RoomSeed, gridPointManager.EnemySeed);
				RNGManager.SetSeed(RngID.Chest_RoomSeed, gridPointManager.ChestSeed);
				RNGManager.SetSeed(RngID.Hazards_RoomSeed, gridPointManager.HazardSeed);
				ISpawnController[] spawnControllers = room.SpawnControllerManager.SpawnControllers;
				for (int j = 0; j < spawnControllers.Length; j++)
				{
					ISetSpawnType setSpawnType = spawnControllers[j] as ISetSpawnType;
					if (setSpawnType != null)
					{
						setSpawnType.SetSpawnType();
					}
				}
				RoomEnemyManager[] standaloneRoomEnemyManagers = room.StandaloneRoomEnemyManagers;
				for (int j = 0; j < standaloneRoomEnemyManagers.Length; j++)
				{
					standaloneRoomEnemyManagers[j].SetSpawnType();
				}
			}
		}
	}
}
