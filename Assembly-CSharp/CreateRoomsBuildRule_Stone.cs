using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rooms;
using UnityEngine;

// Token: 0x02000AAB RID: 2731
public class CreateRoomsBuildRule_Stone : CreateRoomsBuildRule
{
	// Token: 0x06005263 RID: 21091 RVA: 0x0002CDA0 File Offset: 0x0002AFA0
	public override IEnumerator CreateRooms(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		if (biomeCreator.BuildQueue.Peek().RoomType == RoomType.Transition)
		{
			biomeCreator.BuildQueue.Dequeue();
		}
		this.CreateStartOfBridge(biomeCreator, biomeController);
		yield return base.CreateRooms(biomeCreator, biomeController);
		this.CreateEndOfBridge(biomeCreator, biomeController);
		yield break;
	}

	// Token: 0x06005264 RID: 21092 RVA: 0x001392BC File Offset: 0x001374BC
	private void CreateStartOfBridge(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		GridPointManager gridPointManager = biomeController.GridPointManager.GridPointManagers.First<GridPointManager>();
		int x = gridPointManager.GridCoordinates.x + gridPointManager.Size.x;
		int y = gridPointManager.GridCoordinates.y;
		base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.FirstNonTransitionRoom_Bridge), new Vector2Int(x, y), new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, false);
	}

	// Token: 0x06005265 RID: 21093 RVA: 0x00139328 File Offset: 0x00137528
	private void CreateEndOfBridge(BiomeCreator biomeCreator, BiomeController biomeController)
	{
		GridPointManager gridPointManager = biomeController.GridPointManager.GridPointManagers.First<GridPointManager>();
		int x = biomeController.GridPointManager.Extents[RoomSide.Right];
		int y = gridPointManager.GridCoordinates.y;
		base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SecondLastRoom_Bridge), new Vector2Int(x, y), new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, true);
		x = biomeController.GridPointManager.Extents[RoomSide.Right];
		base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.LastRoom_Bridge), new Vector2Int(x, y), new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.BossEntrance, true);
	}

	// Token: 0x06005266 RID: 21094 RVA: 0x001393BC File Offset: 0x001375BC
	protected override void BuildRoomsAtRandomDoorLocations(BiomeCreator biomeCreator, BiomeController biomeController, int finalRoomCountInBiome, GridPointManager originRoom, List<RoomSetEntry> standardRoomPoolOverride)
	{
		int num = biomeController.TargetRoomCountsByRoomType.Sum((KeyValuePair<RoomType, int> entry) => entry.Value) + 2 + 3 + 2;
		int num2 = (int)((float)num * 0.25f);
		int num3 = (int)((float)num * 0.5f);
		int num4 = (int)((float)num * 0.75f);
		if (!this.m_isToTowerBuilt && biomeCreator.RoomNumber > num3)
		{
			int x = biomeController.GridPointManager.Extents[RoomSide.Right];
			int y = originRoom.GridCoordinates.y;
			Vector2Int coords = new Vector2Int(x, y);
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.PreTowerRoom_Bridge), coords, new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, false);
			coords.x++;
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.ToTowerBiome_Bridge), coords, new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.None, false);
			coords.x += 2;
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.PostTowerRoom_Bridge), coords, new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, false);
			this.m_isToTowerBuilt = true;
			return;
		}
		if ((!this.m_isFirstSplitBuilt && biomeCreator.RoomNumber > num2) || (!this.m_isSecondSplitBuilt && biomeCreator.RoomNumber > num4))
		{
			int x2 = biomeController.GridPointManager.Extents[RoomSide.Right];
			int y2 = originRoom.GridCoordinates.y;
			Vector2Int coords2 = new Vector2Int(x2, y2);
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SplitRoomLeft_Bridge), coords2, new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, false);
			coords2.x++;
			base.CreateSpecificRoom(biomeCreator, biomeController, RoomReferenceLibrary.GetRoomMetaData(RoomReferenceType.SplitRoomRight_Bridge), coords2, new DoorLocation(RoomSide.Left, 2), BiomeType.None, RoomType.Standard, false);
			if (!this.m_isFirstSplitBuilt)
			{
				this.m_isFirstSplitBuilt = true;
				return;
			}
			if (!this.m_isSecondSplitBuilt)
			{
				this.m_isSecondSplitBuilt = true;
				return;
			}
		}
		else
		{
			base.BuildRoomsAtRandomDoorLocations(biomeCreator, biomeController, finalRoomCountInBiome, originRoom, standardRoomPoolOverride);
		}
	}

	// Token: 0x04003DEE RID: 15854
	private bool m_isToTowerBuilt;

	// Token: 0x04003DEF RID: 15855
	private bool m_isFirstSplitBuilt;

	// Token: 0x04003DF0 RID: 15856
	private bool m_isSecondSplitBuilt;

	// Token: 0x04003DF1 RID: 15857
	private const bool BUILD_TO_TOWER = true;

	// Token: 0x04003DF2 RID: 15858
	public static int BRIDGE_HEIGHT = 3;
}
