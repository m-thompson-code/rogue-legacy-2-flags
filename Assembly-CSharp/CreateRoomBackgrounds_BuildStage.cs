using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000AC5 RID: 2757
public class CreateRoomBackgrounds_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052EF RID: 21231 RVA: 0x0002D1E3 File Offset: 0x0002B3E3
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
