using System;
using System.Collections;

// Token: 0x02000AA8 RID: 2728
public class CreateRoomsBuildRule_Forest : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06005251 RID: 21073 RVA: 0x0002CD6C File Offset: 0x0002AF6C
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		yield return base.CreateRooms(biomeCreator, biomeController);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Right, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.HeirloomDoubleJump_Forest), RoomType.None);
		yield break;
	}
}
