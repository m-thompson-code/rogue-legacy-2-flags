using System;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public class CaveBossEntranceRoomController : BossEntranceRoomController
{
	// Token: 0x06002F7A RID: 12154 RVA: 0x000A266C File Offset: 0x000A086C
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		if (!CutsceneManager.IsCutsceneActive)
		{
			Animator component = this.m_dragonPropSpawner.PropInstance.GetComponent<Animator>();
			bool flag = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_White_Defeated);
			bool flag2 = SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.CaveMiniboss_Black_Defeated);
			if (flag)
			{
				component.SetTrigger("LeftChainBreakInstant");
			}
			if (flag2)
			{
				component.SetTrigger("RightChainBreakInstant");
			}
			if (flag && flag2)
			{
				component.SetTrigger("CollarBreakInstant");
			}
			if (!base.IsRoomComplete)
			{
				if (flag && flag2 && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_BossDoorOpen))
				{
					this.SetBossTunnelState(BossTunnelState.PartlyOpen, true);
				}
				else
				{
					this.SetBossTunnelState(BossTunnelState.Closed, true);
				}
			}
		}
		else
		{
			this.SetBossTunnelState(BossTunnelState.Closed, true);
		}
		this.m_bossTunnel.Tunnel.SetIsVisible(false);
		this.m_bossUpTunnel.Tunnel.SetIsVisible(false);
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x000A2748 File Offset: 0x000A0948
	public override void SetBossTunnelState(BossTunnelState state, bool skipToIdleState)
	{
		Animator component = this.m_dragonPropSpawner.PropInstance.GetComponent<Animator>();
		if (state == BossTunnelState.Destroyed && (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DragonDialogue_AfterDefeatingTubal) || NPCDialogueManager.CanSpeak(NPCType.Dragon)))
		{
			state = BossTunnelState.Closed;
		}
		(base.BossTunnelSpawner.Tunnel as BossTunnel).TunnelState = state;
		component.SetBool("CloseMouth", false);
		component.SetBool("OpenMouth", false);
		component.SetBool("OpenMouthInstant", false);
		component.SetBool("Sleep", false);
		component.SetBool("SleepInstant", false);
		string text = null;
		switch (state)
		{
		case BossTunnelState.Closed:
			if (!skipToIdleState)
			{
				component.Update(0f);
				component.Update(0f);
				component.SetBool("OpenMouthInstant", true);
			}
			text = "CloseMouth";
			base.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			base.BossTunnelSpawner.Tunnel.Interactable.SetIsInteractableActive(false);
			break;
		case BossTunnelState.PartlyOpen:
			this.m_doorPartlyOpenedRelay.Dispatch(skipToIdleState);
			text = "OpenMouth";
			base.BossTunnelSpawner.Tunnel.SetIsLocked(false);
			base.BossTunnelSpawner.Tunnel.Interactable.SetIsInteractableActive(true);
			break;
		case BossTunnelState.Destroyed:
			this.m_doorDestroyedRelay.Dispatch(skipToIdleState);
			text = "Sleep";
			base.BossTunnelSpawner.Tunnel.SetIsLocked(true);
			base.BossTunnelSpawner.Tunnel.Interactable.SetIsInteractableActive(false);
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (skipToIdleState)
			{
				if (state != BossTunnelState.Closed)
				{
					text += "Instant";
				}
				component.Update(0f);
				component.Update(0f);
				component.SetBool(text, true);
				return;
			}
			component.SetBool(text, true);
		}
	}

	// Token: 0x040025E0 RID: 9696
	[SerializeField]
	private PropSpawnController m_dragonPropSpawner;

	// Token: 0x040025E1 RID: 9697
	private InsightObjectiveCompleteHUDEventArgs m_insightArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.None, false, 5f, null, null, null);
}
