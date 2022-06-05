using System;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class BlackMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x06002F31 RID: 12081 RVA: 0x000A109C File Offset: 0x0009F29C
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CachedHealthOverride = 0f;
		playerController.CachedManaOverride = 0f;
		if (!base.IsRoomComplete)
		{
			if (SaveManager.PlayerSaveData.GetFlag(this.m_chestOpenedFlag))
			{
				this.m_bossTunnel.Tunnel.Animator.SetTrigger("Disable");
				this.m_bossTunnel.Tunnel.SetIsLocked(true);
				this.RoomCompleted();
				return;
			}
		}
		else
		{
			this.m_bossTunnel.Tunnel.Animator.Play("DisabledIdle");
			this.m_bossTunnel.Tunnel.SetIsLocked(true);
		}
	}

	// Token: 0x04002597 RID: 9623
	[SerializeField]
	protected PlayerSaveFlag m_chestOpenedFlag;

	// Token: 0x04002598 RID: 9624
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;
}
