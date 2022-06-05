using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class JohanPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x1700116E RID: 4462
	// (get) Token: 0x06002E39 RID: 11833 RVA: 0x0009C126 File Offset: 0x0009A326
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x1700116F RID: 4463
	// (get) Token: 0x06002E3A RID: 11834 RVA: 0x0009C130 File Offset: 0x0009A330
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			if (this.m_speechBubbleDisabled)
			{
				return false;
			}
			if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
			{
				return !SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.SeenParade) && !this.m_endingSpeechBubblePlayed;
			}
			if (base.Room.BiomeType == BiomeType.Garden)
			{
				return !this.m_endingSpeechBubblePlayed;
			}
			if (this.m_canGiveLantern)
			{
				return true;
			}
			DialogueDisplayOverride_Johan component = this.m_prop.PropSpawnController.GetComponent<DialogueDisplayOverride_Johan>();
			if (component)
			{
				bool flag = this.IsJohanSpawnConditionTrue(component.SpawnCondition);
				PlayerSaveFlag spokenFlagOverride = component.SpokenFlagOverride;
				return (spokenFlagOverride == PlayerSaveFlag.None || !SaveManager.PlayerSaveData.GetFlag(spokenFlagOverride)) && ((flag && !component.SpawnIfFalse) || (!flag && component.SpawnIfFalse));
			}
			return false;
		}
	}

	// Token: 0x17001170 RID: 4464
	// (get) Token: 0x06002E3B RID: 11835 RVA: 0x0009C1F1 File Offset: 0x0009A3F1
	public SpeechBubbleType BubbleType
	{
		get
		{
			if (this.m_canGiveLantern)
			{
				return SpeechBubbleType.PointOfInterest;
			}
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x0009C200 File Offset: 0x0009A400
	protected override void Awake()
	{
		base.Awake();
		this.m_prop = base.GetComponent<Prop>();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_endInteraction = new Action(this.EndInteraction);
		this.m_teleportToTraitorFight = new Action(this.TeleportToTraitorFight);
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x0009C250 File Offset: 0x0009A450
	public void SetEndingCutsceneStateEnabled(bool enabled)
	{
		this.m_interactable.SetIsInteractableActive(!enabled);
		this.m_speechBubbleDisabled = enabled;
		this.m_interactable.SpeechBubble.SetSpeechBubbleEnabled(!enabled);
		if (enabled)
		{
			this.m_npcController.HideHeart();
			return;
		}
		this.m_npcController.ShowHeart();
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x0009C2A4 File Offset: 0x0009A4A4
	protected override void InitializePooledPropOnEnter()
	{
		this.m_endingSpeechBubblePlayed = false;
		this.m_canGiveLantern = false;
		if (base.Room.BiomeType == BiomeType.Cave && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) > 0)
		{
			this.RunLanternInsightResolved();
		}
		if (base.Room.BiomeType != BiomeType.Garden && BossID_RL.IsBossBeaten(BossID.Castle_Boss) && BossID_RL.IsBossBeaten(BossID.Bridge_Boss) && BossID_RL.IsBossBeaten(BossID.Forest_Boss) && BossID_RL.IsBossBeaten(BossID.Study_Boss) && BossID_RL.IsBossBeaten(BossID.Tower_Boss) && BossID_RL.IsBossBeaten(BossID.Cave_Boss) && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) > 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		DialogueDisplayOverride_Johan component = this.m_prop.PropSpawnController.GetComponent<DialogueDisplayOverride_Johan>();
		if (component)
		{
			bool flag = this.IsJohanSpawnConditionTrue(component.SpawnCondition);
			PlayerSaveFlag spokenFlagOverride = component.SpokenFlagOverride;
			bool flag2 = (spokenFlagOverride == PlayerSaveFlag.None || !SaveManager.PlayerSaveData.GetFlag(spokenFlagOverride)) && ((flag && !component.SpawnIfFalse) || (!flag && component.SpawnIfFalse));
			if (component.SpawnCondition == JohanPropController.Johan_SpawnCondition.TowerBossBeatenAndNotCollectedLantern && flag2)
			{
				this.m_canGiveLantern = true;
			}
			if (!flag2)
			{
				base.gameObject.SetActive(false);
				return;
			}
			PropSpawnController propSpawnController = base.Room.gameObject.FindObjectReference("PizzaGirlSpawner", false, false);
			if (propSpawnController && propSpawnController.PropInstance)
			{
				propSpawnController.PropInstance.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x0009C418 File Offset: 0x0009A618
	public void TalkToJohan()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToJohanCoroutine());
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x0009C433 File Offset: 0x0009A633
	private IEnumerator TalkToJohanCoroutine()
	{
		this.m_npcController.SetNPCState(NPCState.AtAttention, false);
		AudioManager.PlayOneShotAttached(this, this.m_greetingAudioEvent, base.gameObject);
		RewiredMapController.SetCurrentMapEnabled(false);
		yield return this.MovePlayerToJohan();
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (SaveManager.PlayerSaveData.EndingSpawnRoom == EndingSpawnRoomType.AboveGround)
		{
			this.RunEndingDialogue();
		}
		else
		{
			bool flag = base.Room.BiomeType == BiomeType.Garden;
			if (this.m_canGiveLantern)
			{
				yield return this.GiveHeirloomCoroutine();
			}
			else if (flag)
			{
				this.RunGardenDialogue();
			}
			else
			{
				this.RunDialogue();
			}
		}
		yield break;
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x0009C442 File Offset: 0x0009A642
	private IEnumerator GiveHeirloomCoroutine()
	{
		this.m_canGiveLantern = false;
		RewiredMapController.SetIsInCutscene(true);
		this.RunDialogue();
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.CaveLantern, 1, false, true);
		float delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Animator.SetBool("Victory", true);
		delay = Time.time + 0.75f;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		HeirloomDrop heirloomDrop = new HeirloomDrop(HeirloomType.CaveLantern);
		(WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController).AddSpecialItemDrop(heirloomDrop);
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		playerController.Animator.SetBool("Victory", false);
		delay = Time.time + 0.25f;
		while (Time.time < delay)
		{
			yield return null;
		}
		RewiredMapController.SetIsInCutscene(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		this.RunLanternInsightResolved();
		yield break;
	}

	// Token: 0x06002E42 RID: 11842 RVA: 0x0009C454 File Offset: 0x0009A654
	private void RunDialogue()
	{
		DialogueDisplayOverride component = this.m_prop.PropSpawnController.gameObject.GetComponent<DialogueDisplayOverride>();
		if (component)
		{
			string text = string.IsNullOrEmpty(component.SpeakerOverride) ? NPCDialogue_EV.GetNPCTitleLocID(NPCType.Johan) : component.SpeakerOverride;
			PlayerSaveFlag spokenFlagOverride = component.SpokenFlagOverride;
			string text2;
			if ((spokenFlagOverride == PlayerSaveFlag.None || !SaveManager.PlayerSaveData.GetFlag(component.SpokenFlagOverride)) && !string.IsNullOrEmpty(component.DialogueOverride))
			{
				if (spokenFlagOverride != PlayerSaveFlag.None)
				{
					SaveManager.PlayerSaveData.SetFlag(component.SpokenFlagOverride, true);
				}
				text2 = component.DialogueOverride;
			}
			else
			{
				text2 = component.RepeatedDialogueOverride;
			}
			DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
			if (component.UseLocIDOverride)
			{
				DialogueManager.AddDialogue(text, text2, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			}
			else
			{
				DialogueManager.AddNonLocDialogue(LocalizationManager.GetString(text, false, false), text2, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			}
			DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			return;
		}
		this.EndInteraction();
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x0009C564 File Offset: 0x0009A764
	private void RunGardenDialogue()
	{
		bool flag = false;
		string textLocID;
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.GardenBoss_Defeated))
		{
			if (!SaveManager.PlayerSaveData.SpokenToTraitor)
			{
				SaveManager.PlayerSaveData.SpokenToTraitor = true;
				int num = Mathf.Clamp(SaveManager.PlayerSaveData.TimesBeatenTraitor, 0, Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 1);
				textLocID = Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS[num];
			}
			else
			{
				textLocID = Ending_EV.GARDEN_PREFIGHT_REPEAT_DIALOGUE_LOCIDS[(int)SaveManager.PlayerSaveData.TraitorPreFightRepeatDialogueIndex];
				PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
				playerSaveData.TraitorPreFightRepeatDialogueIndex += 1;
				if ((int)SaveManager.PlayerSaveData.TraitorPreFightRepeatDialogueIndex >= Ending_EV.GARDEN_PREFIGHT_REPEAT_DIALOGUE_LOCIDS.Length)
				{
					SaveManager.PlayerSaveData.TraitorPreFightRepeatDialogueIndex = 0;
				}
			}
			flag = true;
		}
		else
		{
			this.m_endingSpeechBubblePlayed = true;
			textLocID = Ending_EV.GARDEN_POSTFIGHT_REPEAT_DIALOGUE_LOCIDS[(int)SaveManager.PlayerSaveData.TraitorPostFightRepeatDialogueIndex];
			PlayerSaveData playerSaveData2 = SaveManager.PlayerSaveData;
			playerSaveData2.TraitorPostFightRepeatDialogueIndex += 1;
			if ((int)SaveManager.PlayerSaveData.TraitorPostFightRepeatDialogueIndex >= Ending_EV.GARDEN_POSTFIGHT_REPEAT_DIALOGUE_LOCIDS.Length)
			{
				SaveManager.PlayerSaveData.TraitorPostFightRepeatDialogueIndex = 0;
			}
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		if (flag)
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_teleportToTraitorFight);
		}
		else
		{
			DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x0009C6A4 File Offset: 0x0009A8A4
	private void RunEndingDialogue()
	{
		string textLocID;
		if (!this.m_endingSpeechBubblePlayed)
		{
			this.m_endingSpeechBubblePlayed = true;
			textLocID = "LOC_ID_J_OUTRO_FRIENDS_INTRO_1";
		}
		else
		{
			textLocID = "LOC_ID_J_OUTRO_FRIENDS_REPEAT_1";
		}
		DialogueManager.StartNewDialogue(this.m_npcController, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		DialogueManager.AddDialogueCompleteEndHandler(this.m_endInteraction);
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x0009C70E File Offset: 0x0009A90E
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x0009C730 File Offset: 0x0009A930
	private void TeleportToTraitorFight()
	{
		TunnelSpawnController tunnelSpawnController;
		if (BurdenManager.IsBurdenActive(BurdenType.FinalBossUp))
		{
			tunnelSpawnController = base.Room.gameObject.FindObjectReference("FightUpTunnel", false, false);
		}
		else
		{
			tunnelSpawnController = base.Room.gameObject.FindObjectReference("FightTunnel", false, false);
		}
		if (tunnelSpawnController && tunnelSpawnController.Tunnel)
		{
			tunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
			return;
		}
		Debug.Log("Could not teleport to traitor fight. Tunnel Spawner (FightTunnel) not found.");
		this.EndInteraction();
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x0009C7AE File Offset: 0x0009A9AE
	private IEnumerator MovePlayerToJohan()
	{
		PlayerManager.GetPlayerController().SetVelocity(0f, 0f, false);
		Vector3 position = this.m_playerPositionObj.transform.position;
		PlayerMovementHelper.StopAllMovementInput();
		yield return PlayerMovementHelper.MoveTo(position, true);
		if (this.m_prop.transform.localScale.x > 0f)
		{
			PlayerManager.GetPlayerController().SetFacing(false);
		}
		else
		{
			PlayerManager.GetPlayerController().SetFacing(true);
		}
		PlayerMovementHelper.ResumeAllMovementInput();
		yield break;
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x0009C7C0 File Offset: 0x0009A9C0
	private void RunLanternInsightResolved()
	{
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.HeirloomLantern) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.HeirloomLantern, InsightState.ResolvedButNotViewed, false);
			InsightObjectiveCompleteHUDEventArgs eventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.HeirloomLantern, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, null, eventArgs);
		}
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x0009C80D File Offset: 0x0009AA0D
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x0009C810 File Offset: 0x0009AA10
	public bool IsJohanSpawnConditionTrue(JohanPropController.Johan_SpawnCondition spawnCondition)
	{
		if (spawnCondition <= JohanPropController.Johan_SpawnCondition.BridgeBossDefeated)
		{
			if (spawnCondition <= JohanPropController.Johan_SpawnCondition.MemoryHeirloomObtained)
			{
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.DiedAtLeastOnce)
				{
					return SaveManager.PlayerSaveData.TimesDied > 0;
				}
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.TowerBossBeatenAndNotCollectedLantern)
				{
					return BossID_RL.IsBossBeaten(BossID.Tower_Boss) && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) < 1;
				}
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.MemoryHeirloomObtained)
				{
					return SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) > 0;
				}
			}
			else
			{
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.TowerBossNotBeatenAndLanternInsightNotObtained)
				{
					return !BossID_RL.IsBossBeaten(BossID.Tower_Boss) && SaveManager.PlayerSaveData.GetInsightState(InsightType.HeirloomLantern) <= InsightState.Undiscovered && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.CaveLantern) < 1;
				}
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.CastleBossDefeated)
				{
					return BossID_RL.IsBossBeaten(BossID.Castle_Boss);
				}
				if (spawnCondition == JohanPropController.Johan_SpawnCondition.BridgeBossDefeated)
				{
					return BossID_RL.IsBossBeaten(BossID.Bridge_Boss);
				}
			}
		}
		else if (spawnCondition <= JohanPropController.Johan_SpawnCondition.TowerBossDefeated)
		{
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.ForestBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Forest_Boss);
			}
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.StudyBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Study_Boss);
			}
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.TowerBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Tower_Boss);
			}
		}
		else
		{
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.CaveBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Cave_Boss);
			}
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.GardenBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Garden_Boss);
			}
			if (spawnCondition == JohanPropController.Johan_SpawnCondition.FinalBossDefeated)
			{
				return BossID_RL.IsBossBeaten(BossID.Final_Boss);
			}
		}
		return true;
	}

	// Token: 0x040024E2 RID: 9442
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040024E3 RID: 9443
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040024E4 RID: 9444
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x040024E5 RID: 9445
	private Prop m_prop;

	// Token: 0x040024E6 RID: 9446
	private NPCController m_npcController;

	// Token: 0x040024E7 RID: 9447
	private bool m_canGiveLantern;

	// Token: 0x040024E8 RID: 9448
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040024E9 RID: 9449
	private bool m_speechBubbleDisabled;

	// Token: 0x040024EA RID: 9450
	private Action m_endInteraction;

	// Token: 0x040024EB RID: 9451
	private Action m_teleportToTraitorFight;

	// Token: 0x02000CB4 RID: 3252
	public enum Johan_SpawnCondition
	{
		// Token: 0x0400517B RID: 20859
		None,
		// Token: 0x0400517C RID: 20860
		DiedAtLeastOnce = 10,
		// Token: 0x0400517D RID: 20861
		TowerBossBeatenAndNotCollectedLantern = 20,
		// Token: 0x0400517E RID: 20862
		MemoryHeirloomObtained = 30,
		// Token: 0x0400517F RID: 20863
		TowerBossNotBeatenAndLanternInsightNotObtained = 40,
		// Token: 0x04005180 RID: 20864
		CastleBossDefeated = 1000,
		// Token: 0x04005181 RID: 20865
		BridgeBossDefeated = 1010,
		// Token: 0x04005182 RID: 20866
		ForestBossDefeated = 1020,
		// Token: 0x04005183 RID: 20867
		StudyBossDefeated = 1030,
		// Token: 0x04005184 RID: 20868
		TowerBossDefeated = 1040,
		// Token: 0x04005185 RID: 20869
		CaveBossDefeated = 1050,
		// Token: 0x04005186 RID: 20870
		GardenBossDefeated = 1055,
		// Token: 0x04005187 RID: 20871
		FinalBossDefeated = 1060
	}
}
