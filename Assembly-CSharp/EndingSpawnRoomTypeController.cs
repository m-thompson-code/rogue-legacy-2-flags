using System;
using UnityEngine;

// Token: 0x0200085E RID: 2142
public class EndingSpawnRoomTypeController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170017B1 RID: 6065
	// (get) Token: 0x06004208 RID: 16904 RVA: 0x0002493C File Offset: 0x00022B3C
	// (set) Token: 0x06004209 RID: 16905 RVA: 0x00024944 File Offset: 0x00022B44
	public BaseRoom Room { get; private set; }

	// Token: 0x170017B2 RID: 6066
	// (get) Token: 0x0600420A RID: 16906 RVA: 0x0002494D File Offset: 0x00022B4D
	public EndingSpawnRoomType EndingSpawnRoomType
	{
		get
		{
			return this.m_endingSpawnRoomType;
		}
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x00024955 File Offset: 0x00022B55
	public virtual void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x0002497C File Offset: 0x00022B7C
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x00109178 File Offset: 0x00107378
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

	// Token: 0x040033AF RID: 13231
	[SerializeField]
	private EndingSpawnRoomType m_endingSpawnRoomType;
}
