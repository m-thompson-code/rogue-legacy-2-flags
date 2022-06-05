using System;
using RL_Windows;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000903 RID: 2307
public class DriftHouseExteriorController : BaseSpecialPropController, IDisplaySpeechBubble
{
	// Token: 0x170018D4 RID: 6356
	// (get) Token: 0x06004618 RID: 17944 RVA: 0x00112A08 File Offset: 0x00110C08
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

	// Token: 0x170018D5 RID: 6357
	// (get) Token: 0x06004619 RID: 17945 RVA: 0x00026843 File Offset: 0x00024A43
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

	// Token: 0x0600461A RID: 17946 RVA: 0x00112A70 File Offset: 0x00110C70
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

	// Token: 0x0600461B RID: 17947 RVA: 0x00026859 File Offset: 0x00024A59
	private void EnterDriftHouse(GameObject gameObj)
	{
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.EnteredDriftHouseOnce))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.EnteredDriftHouseOnce, true);
		}
		this.m_updateSkillTreeButtons = true;
	}

	// Token: 0x0400361D RID: 13853
	[SerializeField]
	private GameObject m_closedShack;

	// Token: 0x0400361E RID: 13854
	[SerializeField]
	private GameObject m_openShack;

	// Token: 0x0400361F RID: 13855
	[SerializeField]
	private SpeechBubbleController m_speechBubble;

	// Token: 0x04003620 RID: 13856
	private Tunnel m_driftHouseTunnel;

	// Token: 0x04003621 RID: 13857
	private bool m_updateSkillTreeButtons;
}
