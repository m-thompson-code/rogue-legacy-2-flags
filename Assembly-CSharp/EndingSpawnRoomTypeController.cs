using System;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class EndingSpawnRoomTypeController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170011C2 RID: 4546
	// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000A353F File Offset: 0x000A173F
	// (set) Token: 0x06002FBF RID: 12223 RVA: 0x000A3547 File Offset: 0x000A1747
	public BaseRoom Room { get; private set; }

	// Token: 0x170011C3 RID: 4547
	// (get) Token: 0x06002FC0 RID: 12224 RVA: 0x000A3550 File Offset: 0x000A1750
	public EndingSpawnRoomType EndingSpawnRoomType
	{
		get
		{
			return this.m_endingSpawnRoomType;
		}
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x000A3558 File Offset: 0x000A1758
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x000A357F File Offset: 0x000A177F
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x000A35AC File Offset: 0x000A17AC
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.m_endingSpawnRoomType == EndingSpawnRoomType.Docks)
		{
			SaveManager.PlayerSaveData.EndingSpawnRoom = EndingSpawnRoomType.None;
			return;
		}
		if (this.m_endingSpawnRoomType >= EndingSpawnRoomType.Hallway && this.m_endingSpawnRoomType < EndingSpawnRoomType.AboveGround)
		{
			SaveManager.PlayerSaveData.EndingSpawnRoom = EndingSpawnRoomType.Hallway;
			return;
		}
		SaveManager.PlayerSaveData.EndingSpawnRoom = this.m_endingSpawnRoomType;
	}

	// Token: 0x04002610 RID: 9744
	[SerializeField]
	private EndingSpawnRoomType m_endingSpawnRoomType;
}
