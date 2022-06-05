using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000892 RID: 2194
	public class MergeRoomStrategy_Tower : MergeRoomStrategy
	{
		// Token: 0x060047F4 RID: 18420 RVA: 0x00102B60 File Offset: 0x00100D60
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

		// Token: 0x060047F5 RID: 18421 RVA: 0x00102B7D File Offset: 0x00100D7D
		private void OriginalTowerMergeLogic(BiomeController biomeController)
		{
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.Tower);
			MergeRoomTools.MergeConnectedGridPointManagers(biomeController, BiomeType.TowerExterior);
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x00102B98 File Offset: 0x00100D98
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
