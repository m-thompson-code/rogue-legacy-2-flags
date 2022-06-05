using System;
using UnityEngine;

// Token: 0x02000790 RID: 1936
public class PlayerTakeNoDamage_RoomLogic : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170015D6 RID: 5590
	// (get) Token: 0x06003B3E RID: 15166 RVA: 0x0002081A File Offset: 0x0001EA1A
	// (set) Token: 0x06003B3F RID: 15167 RVA: 0x00020822 File Offset: 0x0001EA22
	public BaseRoom Room { get; private set; }

	// Token: 0x06003B40 RID: 15168 RVA: 0x000F374C File Offset: 0x000F194C
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit), false);
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x000F379C File Offset: 0x000F199C
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (!playerController.TakesNoDamage)
		{
			playerController.TakesNoDamage = true;
			this.m_invincibilityAdded = true;
		}
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x0002082B File Offset: 0x0001EA2B
	private void OnPlayerExit(object sender, EventArgs args)
	{
		if (this.m_invincibilityAdded)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_invincibilityAdded = false;
		}
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x000F37C8 File Offset: 0x000F19C8
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit));
		}
	}

	// Token: 0x04002F1A RID: 12058
	private bool m_invincibilityAdded;
}
