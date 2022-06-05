using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RLWorldCreation
{
	// Token: 0x02000891 RID: 2193
	public class MergeRoomStrategy_Dragon : MergeRoomStrategy
	{
		// Token: 0x060047F2 RID: 18418 RVA: 0x00102B49 File Offset: 0x00100D49
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
