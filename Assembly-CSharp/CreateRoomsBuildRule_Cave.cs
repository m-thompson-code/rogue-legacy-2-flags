using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
public class CreateRoomsBuildRule_Cave : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06005247 RID: 21063 RVA: 0x0002CD1E File Offset: 0x0002AF1E
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		Vector2Int doorLeadsToGridCoordinates = biomeController.GridPointManager.GridPointManagers[0].GetDoorLeadsToGridCoordinates(new DoorLocation(RoomSide.Bottom, 1));
		base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.FirstNonTransitionRoom_Cave), doorLeadsToGridCoordinates, new DoorLocation(RoomSide.Top, 0), BiomeType.None, RoomType.None, false);
		yield return base.CreateRooms(biomeCreator, biomeController);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Right, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntrance_Cave), RoomType.BossEntrance);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Bottom, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.MinesEntrance_Cave), RoomType.Mandatory);
		yield break;
	}
}
