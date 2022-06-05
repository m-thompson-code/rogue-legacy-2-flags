using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000665 RID: 1637
public class CreateRoomBackgrounds_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B34 RID: 15156 RVA: 0x000CB797 File Offset: 0x000C9997
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<Room>.Enumerator enumerator = biomeController.StandaloneRooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BaseRoom baseRoom = enumerator.Current;
				Room room = baseRoom as Room;
				if (room)
				{
					List<GridPointManager> mergeWithGridPointManagers = room.GridPointManager.MergeWithGridPointManagers;
					if (mergeWithGridPointManagers != null && mergeWithGridPointManagers.Count > 0)
					{
						bool flag = true;
						using (List<GridPointManager>.Enumerator enumerator2 = mergeWithGridPointManagers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.RoomNumber < room.GridPointManager.RoomNumber)
								{
									flag = false;
									break;
								}
							}
						}
						if (!flag)
						{
							continue;
						}
					}
				}
				BiomeCreatorTools.CreateBackground(baseRoom);
			}
			yield break;
		}
		yield break;
	}
}
