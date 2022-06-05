using System;
using RLAudio;
using RL_Windows;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class RebelTunnel : Tunnel
{
	// Token: 0x060032C3 RID: 12995 RVA: 0x000ABC8D File Offset: 0x000A9E8D
	protected override void Awake()
	{
		this.m_enterTunnelAfterDialogue = new Action(this.EnterTunnelAfterDialogue);
		base.Awake();
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x000ABCA8 File Offset: 0x000A9EA8
	protected override void OnEnable()
	{
		bool value = SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.RebelKey) > 0 && SaveManager.PlayerSaveData.GetFlag(PlayerSaveFlag.Rebel_Door_Opened);
		base.Animator.SetBool("Open", value);
		base.OnEnable();
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x000ABCF0 File Offset: 0x000A9EF0
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

	// Token: 0x060032C6 RID: 12998 RVA: 0x000ABDCE File Offset: 0x000A9FCE
	private void EnterTunnelAfterDialogue()
	{
		base.OnPlayerInteractedWithTunnel(null);
	}

	// Token: 0x040027B2 RID: 10162
	private Action m_enterTunnelAfterDialogue;
}
