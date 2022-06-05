using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public class PlayerInvincibleRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170011CC RID: 4556
	// (get) Token: 0x0600300E RID: 12302 RVA: 0x000A47EC File Offset: 0x000A29EC
	// (set) Token: 0x0600300F RID: 12303 RVA: 0x000A47F4 File Offset: 0x000A29F4
	public BaseRoom Room { get; private set; }

	// Token: 0x06003010 RID: 12304 RVA: 0x000A47FD File Offset: 0x000A29FD
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x000A4824 File Offset: 0x000A2A24
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x000A4850 File Offset: 0x000A2A50
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs args)
	{
		if (!this.m_playerIsInvincible)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = true;
			this.m_playerIsInvincible = true;
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x000A486C File Offset: 0x000A2A6C
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed && this.m_playerIsInvincible)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_playerIsInvincible = false;
		}
	}

	// Token: 0x04002643 RID: 9795
	private bool m_playerIsInvincible;
}
