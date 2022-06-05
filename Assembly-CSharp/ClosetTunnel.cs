using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class ClosetTunnel : Tunnel
{
	// Token: 0x060032B4 RID: 12980 RVA: 0x000AB958 File Offset: 0x000A9B58
	protected override void OnEnable()
	{
		base.OnEnable();
		this.IsClosetUnlocked = false;
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x000AB968 File Offset: 0x000A9B68
	protected override void OnPlayerInteractedWithTunnel(GameObject otherObj)
	{
		if (this.IsClosetUnlocked)
		{
			base.OnPlayerInteractedWithTunnel(otherObj);
			return;
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddDialogue("LOC_ID_DOOR_ENDING_LOCKED_DOOR_TITLE_1", "LOC_ID_DOOR_ENDING_LOCKED_DOOR_TEXT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
	}

	// Token: 0x040027B0 RID: 10160
	[NonSerialized]
	public bool IsClosetUnlocked;
}
