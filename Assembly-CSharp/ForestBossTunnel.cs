using System;
using System.Collections;
using RL_Windows;

// Token: 0x0200093A RID: 2362
public class ForestBossTunnel : BossTunnel
{
	// Token: 0x06004793 RID: 18323 RVA: 0x00027394 File Offset: 0x00025594
	protected override void Awake()
	{
		base.Awake();
		this.m_healthChangeArgs = new MaxHealthChangeEventArgs(0f, 0f);
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x000273B1 File Offset: 0x000255B1
	protected override IEnumerator EnterTunnelCoroutine()
	{
		if (0 + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).Level + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).Level + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).Level > 0)
		{
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_SPECIAL_TITLE_FOREST_ACCEPT_OFFERING_1", "LOC_ID_SPECIAL_DESCRIPTION_FOREST_ACCEPT_OFFERING_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			while (WindowManager.GetIsWindowOpen(WindowID.Dialogue))
			{
				yield return null;
			}
			this.RemoveLilyRelics();
		}
		yield return base.EnterTunnelCoroutine();
		yield break;
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00116408 File Offset: 0x00114608
	private void RemoveLilyRelics()
	{
		int num = 0 + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).Level + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).Level;
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).Level;
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).SetLevel(0, false, true);
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).SetLevel(0, false, true);
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).SetLevel(0, false, true);
		PlayerManager.GetPlayerController().InitializeHealthMods();
	}

	// Token: 0x040036DF RID: 14047
	private MaxHealthChangeEventArgs m_healthChangeArgs;
}
