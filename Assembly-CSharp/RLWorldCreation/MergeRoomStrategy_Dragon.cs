using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RLWorldCreation
{
	// Token: 0x02000DBA RID: 3514
	public class MergeRoomStrategy_Dragon : MergeRoomStrategy
	{
		// Token: 0x0600631B RID: 25371 RVA: 0x0003699A File Offset: 0x00034B9A
		public IEnumerator Run(BiomeController biomeController)
		{
			List<GridPointManager> connectedRooms = (from room in biomeController.GridPointManager.GridPointManagers
			where room.RoomMetaData.CanMerge
			select room).ToList<GridPointManager>();
			MergeRoomTools.MergeGridPointManagers(biomeController, connectedRooms);
			yield break;
		}
	}
}
