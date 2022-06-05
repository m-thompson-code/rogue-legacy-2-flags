using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public class CreateOneWays_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052E7 RID: 21223 RVA: 0x0002D1BD File Offset: 0x0002B3BD
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
