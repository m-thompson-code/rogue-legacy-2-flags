using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class JohanPropController : BaseSpecialPropController, IDisplaySpeechBubble, IAudioEventEmitter
{
	// Token: 0x1700171B RID: 5915
	// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x1700171C RID: 5916
	// (get) Token: 0x06003FB8 RID: 16312 RVA: 0x000FF068 File Offset: 0x000FD268
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

	// Token: 0x1700171D RID: 5917
	// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x0002339A File Offset: 0x0002159A
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

	// Token: 0x06003FBA RID: 16314 RVA: 0x000FF12C File Offset: 0x000FD32C
	protected override void Awake()
	{
		base.Awake();
		this.m_prop = base.GetComponent<Prop>();
		this.m_npcController = base.GetComponent<NPCController>();
		this.m_endInteraction = new Action(this.EndInteraction);
		this.m_teleportToTraitorFight = new Action(this.TeleportToTraitorFight);
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x000FF17C File Offset: 0x000FD37C
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

	// Token: 0x06003FBC RID: 16316 RVA: 0x000FF1D0 File Offset: 0x000FD3D0
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

	// Token: 0x06003FBD RID: 16317 RVA: 0x000233A7 File Offset: 0x000215A7
	public void TalkToJohan()
	{
		this.m_interactable.SetIsInteractableActive(false);
		base.StartCoroutine(this.TalkToJohanCoroutine());
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x000233C2 File Offset: 0x000215C2
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

	// Token: 0x06003FBF RID: 16319 RVA: 0x000233D1 File Offset: 0x000215D1
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

	// Token: 0x06003FC0 RID: 16320 RVA: 0x000FF344 File Offset: 0x000FD544
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

	// Token: 0x06003FC1 RID: 16321 RVA: 0x000FF454 File Offset: 0x000FD654
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

	// Token: 0x06003FC2 RID: 16322 RVA: 0x000FF594 File Offset: 0x000FD794
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

	// Token: 0x06003FC3 RID: 16323 RVA: 0x000233E0 File Offset: 0x000215E0
	private void EndInteraction()
	{
		this.m_interactable.SetIsInteractableActive(true);
		AudioManager.PlayOneShotAttached(this, this.m_farewellAudioEvent, base.gameObject);
	}

	// Token: 0x06003FC4 RID: 16324 RVA: 0x000FF600 File Offset: 0x000FD800
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

	// Token: 0x06003FC5 RID: 16325 RVA: 0x00023400 File Offset: 0x00021600
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

	// Token: 0x06003FC6 RID: 16326 RVA: 0x000FF680 File Offset: 0x000FD880
	private void RunLanternInsightResolved()
	{
		if (SaveManager.PlayerSaveData.GetInsightState(InsightType.HeirloomLantern) < InsightState.ResolvedButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.HeirloomLantern, InsightState.ResolvedButNotViewed, false);
			InsightObjectiveCompleteHUDEventArgs eventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.HeirloomLantern, false, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, null, eventArgs);
		}
	}

	// Token: 0x06003FC7 RID: 16327 RVA: 0x00002FCA File Offset: 0x000011CA
	protected override void DisableProp(bool firstTimeDisabled)
	{
	}

	// Token: 0x06003FC8 RID: 16328 RVA: 0x000FF6D0 File Offset: 0x000FD8D0
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

	// Token: 0x040031C8 RID: 12744
	[SerializeField]
	private GameObject m_playerPositionObj;

	// Token: 0x040031C9 RID: 12745
	[SerializeField]
	[EventRef]
	private string m_greetingAudioEvent;

	// Token: 0x040031CA RID: 12746
	[SerializeField]
	[EventRef]
	private string m_farewellAudioEvent;

	// Token: 0x040031CB RID: 12747
	private Prop m_prop;

	// Token: 0x040031CC RID: 12748
	private NPCController m_npcController;

	// Token: 0x040031CD RID: 12749
	private bool m_canGiveLantern;

	// Token: 0x040031CE RID: 12750
	private bool m_endingSpeechBubblePlayed;

	// Token: 0x040031CF RID: 12751
	private bool m_speechBubbleDisabled;

	// Token: 0x040031D0 RID: 12752
	private Action m_endInteraction;

	// Token: 0x040031D1 RID: 12753
	private Action m_teleportToTraitorFight;

	// Token: 0x02000814 RID: 2068
	public enum Johan_SpawnCondition
	{
		// Token: 0x040031D3 RID: 12755
		None,
		// Token: 0x040031D4 RID: 12756
		DiedAtLeastOnce = 10,
		// Token: 0x040031D5 RID: 12757
		TowerBossBeatenAndNotCollectedLantern = 20,
		// Token: 0x040031D6 RID: 12758
		MemoryHeirloomObtained = 30,
		// Token: 0x040031D7 RID: 12759
		TowerBossNotBeatenAndLanternInsightNotObtained = 40,
		// Token: 0x040031D8 RID: 12760
		CastleBossDefeated = 1000,
		// Token: 0x040031D9 RID: 12761
		BridgeBossDefeated = 1010,
		// Token: 0x040031DA RID: 12762
		ForestBossDefeated = 1020,
		// Token: 0x040031DB RID: 12763
		StudyBossDefeated = 1030,
		// Token: 0x040031DC RID: 12764
		TowerBossDefeated = 1040,
		// Token: 0x040031DD RID: 12765
		CaveBossDefeated = 1050,
		// Token: 0x040031DE RID: 12766
		GardenBossDefeated = 1055,
		// Token: 0x040031DF RID: 12767
		FinalBossDefeated = 1060
	}
}
