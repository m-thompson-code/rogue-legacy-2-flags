using System;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class CaveMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x170011B5 RID: 4533
	// (get) Token: 0x06002F83 RID: 12163 RVA: 0x000A29B8 File Offset: 0x000A0BB8
	public bool IsWhiteDoor
	{
		get
		{
			return this.m_bossBeatenFlag == PlayerSaveFlag.CaveMiniboss_White_Defeated;
		}
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x000A29C8 File Offset: 0x000A0BC8
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CachedHealthOverride = 0f;
		playerController.CachedManaOverride = 0f;
		(this.m_bossTunnel.Tunnel as CaveMinibossTunnel).IsWhiteTunnel = this.IsWhiteDoor;
		bool flag = (this.IsWhiteDoor && SaveManager.PlayerSaveData.GetRelic(RelicType.DragonKeyWhite).Level > 0) || (!this.IsWhiteDoor && SaveManager.PlayerSaveData.GetRelic(RelicType.DragonKeyBlack).Level > 0);
		bool flag2 = (this.IsWhiteDoor && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_WhiteDoor_Opened)) || (!this.IsWhiteDoor && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_BlackDoor_Opened));
		if (SaveManager.PlayerSaveData.GetFlag(this.m_bossBeatenFlag) && !BossEntranceRoomController.RunDoorCrumbleAnimation)
		{
			base.ForceRoomCompleted();
		}
		if (base.IsRoomComplete)
		{
			this.m_bossTunnel.Tunnel.Animator.SetTrigger("DisableInstant");
			this.m_bossTunnel.Tunnel.SetIsLocked(true);
			return;
		}
		if (SaveManager.PlayerSaveData.GetFlag(this.m_bossBeatenFlag))
		{
			this.m_bossTunnel.Tunnel.Animator.SetTrigger("Disable");
			this.m_bossTunnel.Tunnel.SetIsLocked(true);
			this.RoomCompleted();
			return;
		}
		if (flag2)
		{
			this.m_bossTunnel.Tunnel.Animator.SetTrigger("GateOpenInstant");
		}
		else
		{
			this.m_bossTunnel.Tunnel.Animator.SetTrigger("Enable");
		}
		if (!flag && !flag2)
		{
			this.m_bossTunnel.Tunnel.SetIsLocked(true);
			return;
		}
		this.m_bossTunnel.Tunnel.SetIsLocked(false);
	}

	// Token: 0x06002F85 RID: 12165 RVA: 0x000A2B8A File Offset: 0x000A0D8A
	private void OnDisable()
	{
		BossEntranceRoomController.RunDoorCrumbleAnimation = false;
	}

	// Token: 0x040025E3 RID: 9699
	[SerializeField]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x040025E4 RID: 9700
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;
}
