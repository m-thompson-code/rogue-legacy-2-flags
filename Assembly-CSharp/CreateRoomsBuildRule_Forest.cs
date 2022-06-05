using System;
using System.Collections;

// Token: 0x02000658 RID: 1624
public class CreateRoomsBuildRule_Forest : CreateRoomsBuildRule_PlaceRoomOnBiomeSide
{
	// Token: 0x06003AE8 RID: 15080 RVA: 0x000CA4B1 File Offset: 0x000C86B1
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		yield return base.CreateRooms(biomeCreator, biomeController);
		base.CreateRoomOnBiomeSide(biomeCreator, biomeController, RoomSide.Right, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.HeirloomDoubleJump_Forest), RoomType.None);
		yield break;
	}
}
