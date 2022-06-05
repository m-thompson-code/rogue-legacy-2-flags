using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000664 RID: 1636
public class CreateOneWays_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B32 RID: 15154 RVA: 0x000CB780 File Offset: 0x000C9980
	public IEnumerator Run(BiomeController biomeController)
	{
		using (List<Room>.Enumerator enumerator = biomeController.StandaloneRooms.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Room room = enumerator.Current;
				if (!BiomeCreation_EV.DO_NOT_ADD_ONE_WAYS_TO_BOTTOM_DOORS.Contains(room.AppearanceBiomeType) && !room.DisableOneWaysAtBottomDoors)
				{
					if (room != null)
					{
						List<Door> doorsOnSide = room.GetDoorsOnSide(RoomSide.Bottom);
						if (doorsOnSide == null || doorsOnSide.Count <= 0)
						{
							continue;
						}
						using (List<Door>.Enumerator enumerator2 = room.GetDoorsOnSide(RoomSide.Bottom).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Door door = enumerator2.Current;
								BiomeCreatorTools.PlaceOneWayAtDoor(door);
							}
							continue;
						}
					}
					Debug.LogFormat("<color=red>{0}: One of Biome Controller's ({1}) Standalone Rooms is null", new object[]
					{
						Time.frameCount,
						biomeController.gameObject.name
					});
				}
			}
			yield break;
		}
		yield break;
	}
}
