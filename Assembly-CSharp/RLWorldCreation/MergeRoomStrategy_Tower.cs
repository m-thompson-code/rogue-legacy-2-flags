using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000DBD RID: 3517
	public class MergeRoomStrategy_Tower : MergeRoomStrategy
	{
		// Token: 0x06006326 RID: 25382 RVA: 0x000369D9 File Offset: 0x00034BD9
		public override IEnumerator Run(CreateMergeRooms_BuildStage buildStage, BiomeController biomeController)
		{
			this.m_buildStage = buildStage;
			this.MergeTopOfTower(biomeController);
			if (BiomeCreation_EV.USE_DEFAULT_MERGE_LOGIC_FOR_TOWER_INTERIOR)
			{
				List<GridPointManager> potentialMergeTargets_V = this.GetPotentialMergeTargets_V2(biomeController, BiomeType.Tower, new RoomType[]
				{
					RoomType.BossEntrance,
					RoomType.Fairy,
					RoomType.Transition,
					RoomType.Bonus
				});
				yield return base.MergeRooms_V2(biomeController, biomeController.Biome, potentialMergeTargets_V, true);
				MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.TowerExterior);
			}
			else
			{
				this.OriginalTowerMergeLogic(biomeController);
			}
			MergeRoomStrategy.UpdateBiomeControllerIndices(biomeController);
			yield break;
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x000369F6 File Offset: 0x00034BF6
		private void OriginalTowerMergeLogic(BiomeController biomeController)
		{
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.Tower);
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.TowerExterior);
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x00171718 File Offset: 0x0016F918
		private void MergeTopOfTower(BiomeController biomeController)
		{
			List<GridPointManager> list = (from room in biomeController.GridPointManager.GridPointManagers
			where room.RoomType == RoomType.BossEntrance
			select room).ToList<GridPointManager>();
			if (list != null && list.Count > 1)
			{
				MergeRoomTools.MergeGridPointManagers(biomeController, list);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Failed to merge top of Tower biome because we were unable to find the top of the Tower biome</color>", new object[]
			{
				this
			});
		}
	}
}
