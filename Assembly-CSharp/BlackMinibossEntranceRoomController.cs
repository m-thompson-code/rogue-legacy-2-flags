using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public class BlackMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x06004133 RID: 16691 RVA: 0x00105EB8 File Offset: 0x001040B8
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

	// Token: 0x04003301 RID: 13057
	[SerializeField]
	protected PlayerSaveFlag m_chestOpenedFlag;

	// Token: 0x04003302 RID: 13058
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;
}
