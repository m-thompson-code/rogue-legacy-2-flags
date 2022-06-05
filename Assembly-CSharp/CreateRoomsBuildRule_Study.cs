using System;
using System.Collections;

// Token: 0x0200065B RID: 1627
public class CreateRoomsBuildRule_Study : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06003AFB RID: 15099 RVA: 0x000CAC2A File Offset: 0x000C8E2A
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		yield return base.CreateRooms(biomeCreator, biomeController);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Left, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SpearKnightMinibossEntrance_Study), RoomType.Bonus);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Right, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SecretMemory_Study), RoomType.Mandatory);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Top, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntrance_Study), RoomType.BossEntrance);
		yield break;
	}
}
