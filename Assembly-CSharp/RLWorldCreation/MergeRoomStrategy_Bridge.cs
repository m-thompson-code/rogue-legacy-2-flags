using System;
using System.Collections;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x0200088F RID: 2191
	public class MergeRoomStrategy_Bridge : MergeRoomStrategy
	{
		// Token: 0x060047EC RID: 18412 RVA: 0x00102ACB File Offset: 0x00100CCB
		public override IEnumerator Run(CreateMergeRooms_BuildStage buildStage, BiomeController biomeController)
		{
			this.m_buildStage = buildStage;
			List<GridPointManager> potentialMergeTargets_V = this.GetPotentialMergeTargets_V2(biomeController, BiomeType.Stone, new RoomType[]
			{
				RoomType.Transition
			});
			if (potentialMergeTargets_V.Count > 0)
			{
				yield return base.MergeRooms_V2(biomeController, BiomeType.Stone, potentialMergeTargets_V, false);
			}
			yield break;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x00102AE8 File Offset: 0x00100CE8
		protected override bool AttemptMerge(BiomeController biomeController, RoomSide side, GridPointManager connectedRoom)
		{
			bool result = base.AttemptMerge(biomeController, side, connectedRoom);
			if (connectedRoom.RoomMetaData == RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SplitRoomRight_Bridge))
			{
				result = false;
			}
			return result;
		}
	}
}
