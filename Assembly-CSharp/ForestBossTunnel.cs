using System;
using System.Collections;
using RL_Windows;

// Token: 0x0200056A RID: 1386
public class ForestBossTunnel : BossTunnel
{
	// Token: 0x060032BE RID: 12990 RVA: 0x000ABBB4 File Offset: 0x000A9DB4
	protected override void Awake()
	{
		base.Awake();
		this.m_healthChangeArgs = new MaxHealthChangeEventArgs(0f, 0f);
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x000ABBD1 File Offset: 0x000A9DD1
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

	// Token: 0x060032C0 RID: 12992 RVA: 0x000ABBE0 File Offset: 0x000A9DE0
	private void RemoveLilyRelics()
	{
		int num = 0 + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).Level + SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).Level;
		int level = SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).Level;
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily1).SetLevel(0, false, true);
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily2).SetLevel(0, false, true);
		SaveManager.PlayerSaveData.GetRelic(RelicType.Lily3).SetLevel(0, false, true);
		PlayerManager.GetPlayerController().InitializeHealthMods();
	}

	// Token: 0x040027B1 RID: 10161
	private MaxHealthChangeEventArgs m_healthChangeArgs;
}
