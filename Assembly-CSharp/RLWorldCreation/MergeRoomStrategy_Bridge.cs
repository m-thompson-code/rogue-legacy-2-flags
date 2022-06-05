using System;
using System.Collections;
using System.Collections.Generic;

namespace RLWorldCreation
{
	// Token: 0x02000DB6 RID: 3510
	public class MergeRoomStrategy_Bridge : MergeRoomStrategy
	{
		// Token: 0x06006309 RID: 25353 RVA: 0x00036927 File Offset: 0x00034B27
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

		// Token: 0x0600630A RID: 25354 RVA: 0x001715C4 File Offset: 0x0016F7C4
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
