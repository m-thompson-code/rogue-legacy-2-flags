using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using RL_Windows;

// Token: 0x02000887 RID: 2183
public class TraitorBossRoomController : BossRoomController
{
	// Token: 0x060042EB RID: 17131 RVA: 0x0010C2E4 File Offset: 0x0010A4E4
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_TRAITOR_BOSS_DEFEATED_TITLE_1", false, false);
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x00025034 File Offset: 0x00023234
	protected override IEnumerator StartIntro()
	{
		if (MusicManager.CurrentMusicInstance.isValid())
		{
			RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress_cain", 0.5f, false);
		}
		yield return base.StartIntro();
		yield break;
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x00025043 File Offset: 0x00023243
	protected override IEnumerator StartOutro()
	{
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		bool awardRebelKey = false;
		int num = SaveManager.PlayerSaveData.TimesBeatenTraitor - 1;
		if (num >= Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2)
		{
			awardRebelKey = true;
		}
		if (num == Ending_EV.GARDEN_PREFIGHT_DIALOGUE_LOCIDS.Length - 2)
		{
			num = Ending_EV.GARDEN_POSTFIGHT_DIALOGUE_LOCIDS.Length - 1;
		}
		else if (num > Ending_EV.GARDEN_POSTFIGHT_DIALOGUE_LOCIDS.Length - 2)
		{
			num = Ending_EV.GARDEN_POSTFIGHT_DIALOGUE_LOCIDS.Length - 2;
		}
		string textLocID = Ending_EV.GARDEN_POSTFIGHT_DIALOGUE_LOCIDS[num];
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_NAME_J_REVEALED_1", textLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		yield return null;
		while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
		{
			yield return null;
		}
		RewiredMapController.SetIsInCutscene(true);
		RewiredMapController.SetCurrentMapEnabled(false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.Animator.SetBool("Victory", true);
		this.m_waitYield.CreateNew(0.75f, false);
		yield return this.m_waitYield;
		if (!WindowManager.GetIsWindowLoaded(WindowID.SpecialItemDrop))
		{
			WindowManager.LoadWindow(WindowID.SpecialItemDrop);
		}
		SpecialItemDropWindowController specialItemDropWindowController = WindowManager.GetWindowController(WindowID.SpecialItemDrop) as SpecialItemDropWindowController;
		HeirloomDrop heirloomDrop = new HeirloomDrop(HeirloomType.Fruit);
		specialItemDropWindowController.AddSpecialItemDrop(heirloomDrop);
		SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.Fruit, 1, false, true);
		if (awardRebelKey && SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.RebelKey) < 1)
		{
			HeirloomDrop heirloomDrop2 = new HeirloomDrop(HeirloomType.RebelKey);
			specialItemDropWindowController.AddSpecialItemDrop(heirloomDrop2);
			SaveManager.PlayerSaveData.SetHeirloomLevel(HeirloomType.RebelKey, 1, false, true);
		}
		WindowManager.SetWindowIsOpen(WindowID.SpecialItemDrop, true);
		while (WindowManager.GetIsWindowOpen(WindowID.SpecialItemDrop))
		{
			yield return null;
		}
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		playerController.Animator.SetBool("Victory", false);
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		RewiredMapController.SetIsInCutscene(false);
		RewiredMapController.SetCurrentMapEnabled(true);
		if (awardRebelKey && SaveManager.PlayerSaveData.GetInsightState(InsightType.Ending_RebelsHidout) < InsightState.DiscoveredButNotViewed)
		{
			SaveManager.PlayerSaveData.SetInsightState(InsightType.Ending_RebelsHidout, InsightState.DiscoveredButNotViewed, false);
			InsightObjectiveCompleteHUDEventArgs eventArgs = new InsightObjectiveCompleteHUDEventArgs(InsightType.Ending_RebelsHidout, true, 5f, null, null, null);
			Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, null, eventArgs);
			this.m_waitYield.CreateNew(4f, false);
			yield return this.m_waitYield;
		}
		yield return base.StartOutro();
		yield break;
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x00025052 File Offset: 0x00023252
	protected override void TeleportOut()
	{
		if (base.TunnelSpawnController && base.TunnelSpawnController.Tunnel)
		{
			base.TunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
		}
	}
}
