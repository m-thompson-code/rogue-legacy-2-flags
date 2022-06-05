using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class CreateRoomsBuildRule_Cave : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06003AE4 RID: 15076 RVA: 0x000CA47A File Offset: 0x000C867A
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
