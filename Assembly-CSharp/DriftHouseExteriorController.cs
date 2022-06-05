using System;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200054F RID: 1359
public class DriftHouseExteriorController : BaseSpecialPropController, IDisplaySpeechBubble
{
	// Token: 0x17001245 RID: 4677
	// (get) Token: 0x060031E6 RID: 12774 RVA: 0x000A8B50 File Offset: 0x000A6D50
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
			{
				return false;
			}
			if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DriftHouseUnlocked))
			{
				if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnteredDriftHouseOnce))
				{
					return true;
				}
				if (NPCDialogueManager.CanSpeak(NPCType.NewGamePlusHood) || NPCDialogueManager.CanSpeak(NPCType.ChallengeHood))
				{
					return true;
				}
				if (ChallengeShop.HasEventDialogue() || NewGamePlusShop.HasEventDialogue())
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17001246 RID: 4678
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x000A8BB5 File Offset: 0x000A6DB5
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnteredDriftHouseOnce))
			{
				return SpeechBubbleType.PointOfInterest;
			}
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x000A8BCC File Offset: 0x000A6DCC
	protected override void InitializePooledPropOnEnter()
	{
		TunnelSpawnController tunnelSpawnController = base.Room.gameObject.FindObjectReference("DriftHouseTunnel", false, false);
		if (tunnelSpawnController)
		{
			this.m_driftHouseTunnel = tunnelSpawnController.GetComponent<TunnelSpawnController>().Tunnel;
			if (!this.m_driftHouseTunnel.IsNativeNull())
			{
				this.m_driftHouseTunnel.Interactable.SpeechBubble = this.m_speechBubble;
				this.m_driftHouseTunnel.Interactable.TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.EnterDriftHouse));
				this.m_driftHouseTunnel.Interactable.TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.EnterDriftHouse));
			}
		}
		if (SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.DriftHouseUnlocked))
		{
			this.m_closedShack.SetActive(false);
			this.m_openShack.SetActive(true);
			if (!this.m_driftHouseTunnel.IsNativeNull())
			{
				this.m_driftHouseTunnel.SetIsLocked(false);
			}
			this.m_speechBubble.SetSpeechBubbleEnabled(true);
		}
		else
		{
			this.m_closedShack.SetActive(false);
			this.m_openShack.SetActive(false);
			if (!this.m_driftHouseTunnel.IsNativeNull())
			{
				this.m_driftHouseTunnel.SetIsLocked(true);
			}
			this.m_speechBubble.SetSpeechBubbleEnabled(false);
		}
		if (this.m_updateSkillTreeButtons)
		{
			SkillTreeWindowController skillTreeWindowController = WindowManager.GetWindowController(WindowID.SkillTree) as SkillTreeWindowController;
			EffectManager.AddAnimatorToDisableList(skillTreeWindowController.CastleAnimator);
			skillTreeWindowController.ForceUpdateSkillTreeAnimatorParams();
			skillTreeWindowController.CastleAnimator.Update(1f);
			EffectManager.RemoveAnimatorFromDisableList(skillTreeWindowController.CastleAnimator);
			this.m_updateSkillTreeButtons = false;
		}
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x000A8D41 File Offset: 0x000A6F41
	private void EnterDriftHouse(GameObject gameObj)
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnteredDriftHouseOnce))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.EnteredDriftHouseOnce, true);
		}
		this.m_updateSkillTreeButtons = true;
	}

	// Token: 0x04002748 RID: 10056
	[SerializeField]
	private GameObject m_closedShack;

	// Token: 0x04002749 RID: 10057
	[SerializeField]
	private GameObject m_openShack;

	// Token: 0x0400274A RID: 10058
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x0400274B RID: 10059
	private Tunnel m_driftHouseTunnel;

	// Token: 0x0400274C RID: 10060
	private bool m_updateSkillTreeButtons;
}
