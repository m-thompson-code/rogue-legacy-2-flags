using System;
using System.Collections;

// Token: 0x02000AAE RID: 2734
public class CreateRoomsBuildRule_Study : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06005273 RID: 21107 RVA: 0x0002CDE8 File Offset: 0x0002AFE8
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		yield return base.CreateRooms(biomeCreator, biomeController);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Left, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SpearKnightMinibossEntrance_Study), RoomType.Bonus);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Right, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SecretMemory_Study), RoomType.Mandatory);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Top, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.BossEntrance_Study), RoomType.BossEntrance);
		yield break;
	}
}
