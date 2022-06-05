using System;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x0200093C RID: 2364
public class RebelTunnel : Tunnel
{
	// Token: 0x0600479E RID: 18334 RVA: 0x000273DF File Offset: 0x000255DF
	protected override void Awake()
	{
		this.m_enterTunnelAfterDialogue = new Action(this.EnterTunnelAfterDialogue);
		base.Awake();
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x0011659C File Offset: 0x0011479C
	protected override void OnEnable()
	{
		bool value = SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.RebelKey) > 0 && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Rebel_Door_Opened);
		base.Animator.SetBool("Open", value);
		base.OnEnable();
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x001165E4 File Offset: 0x001147E4
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.RebelKey) <= 0)
		{
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_REBEL_HIDEOUT_DOOR_LOCKED_TITLE_1", "LOC_ID_REBEL_HIDEOUT_DOOR_LOCKED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			return;
		}
		if (!SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Rebel_Door_Opened))
		{
			SaveManager.PlayerSaveData.SetFlag(PlayerSaveFlag.Rebel_Door_Opened, true);
			base.Animator.SetBool("Open", true);
			AudioManager.PlayOneShotAttached(null, "event:/SFX/Interactables/sfx_env_prop_trapDoor_open", base.gameObject);
			DialogueManager.StartNewDialogue(null, NPCState.Idle);
			DialogueManager.AddDialogue("LOC_ID_REBEL_HIDEOUT_DOOR_LOCKED_TITLE_1", "LOC_ID_REBEL_HIDEOUT_DOOR_UNLOCKED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			DialogueManager.AddDialogueCompleteEndHandler(this.m_enterTunnelAfterDialogue);
			WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
			return;
		}
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x000273F9 File Offset: 0x000255F9
	private void EnterTunnelAfterDialogue()
	{
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x040036E3 RID: 14051
	private Action m_enterTunnelAfterDialogue;
}
