using System;
using UnityEngine;

// Token: 0x02000850 RID: 2128
public class CaveMinibossEntranceRoomController : BaseSpecialRoomController
{
	// Token: 0x1700179A RID: 6042
	// (get) Token: 0x060041AA RID: 16810 RVA: 0x000245F5 File Offset: 0x000227F5
	public bool IsWhiteDoor
	{
		get
		{
			return this.m_bossBeatenFlag == PlayerSaveFlag.CaveMiniboss_White_Defeated;
		}
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x001083C8 File Offset: 0x001065C8
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

	// Token: 0x060041AC RID: 16812 RVA: 0x00024604 File Offset: 0x00022804
	private void OnDisable()
	{
		BossEntranceRoomController.RunDoorCrumbleAnimation = false;
	}

	// Token: 0x0400336A RID: 13162
	[SerializeField]
	protected PlayerSaveFlag m_bossBeatenFlag;

	// Token: 0x0400336B RID: 13163
	[SerializeField]
	private TunnelSpawnController m_bossTunnel;
}
