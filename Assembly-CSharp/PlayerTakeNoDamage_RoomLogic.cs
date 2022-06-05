using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class PlayerTakeNoDamage_RoomLogic : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x06002B01 RID: 11009 RVA: 0x00091CA8 File Offset: 0x0008FEA8
	// (set) Token: 0x06002B02 RID: 11010 RVA: 0x00091CB0 File Offset: 0x0008FEB0
	public BaseRoom Room { get; private set; }

	// Token: 0x06002B03 RID: 11011 RVA: 0x00091CBC File Offset: 0x0008FEBC
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit), false);
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x00091D0C File Offset: 0x0008FF0C
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (!playerController.TakesNoDamage)
		{
			playerController.TakesNoDamage = true;
			this.m_invincibilityAdded = true;
		}
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x00091D35 File Offset: 0x0008FF35
	private void OnPlayerExit(object sender, EventArgs args)
	{
		if (this.m_invincibilityAdded)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_invincibilityAdded = false;
		}
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x00091D54 File Offset: 0x0008FF54
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit));
		}
	}

	// Token: 0x0400230F RID: 8975
	private bool m_invincibilityAdded;
}
