using System;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class PlayerInvincibleRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170017D7 RID: 6103
	// (get) Token: 0x060042AC RID: 17068 RVA: 0x00024DD9 File Offset: 0x00022FD9
	// (set) Token: 0x060042AD RID: 17069 RVA: 0x00024DE1 File Offset: 0x00022FE1
	public BaseRoom Room { get; private set; }

	// Token: 0x060042AE RID: 17070 RVA: 0x00024DEA File Offset: 0x00022FEA
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x060042AF RID: 17071 RVA: 0x00024E11 File Offset: 0x00023011
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x060042B0 RID: 17072 RVA: 0x00024E3D File Offset: 0x0002303D
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs args)
	{
		if (!this.m_playerIsInvincible)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = true;
			this.m_playerIsInvincible = true;
		}
	}

	// Token: 0x060042B1 RID: 17073 RVA: 0x00024E59 File Offset: 0x00023059
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed && this.m_playerIsInvincible)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_playerIsInvincible = false;
		}
	}

	// Token: 0x04003415 RID: 13333
	private bool m_playerIsInvincible;
}
