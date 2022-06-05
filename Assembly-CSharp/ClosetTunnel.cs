using System;
using RL_Windows;
using UnityEngine;

// Token: 0x02000936 RID: 2358
public class ClosetTunnel : Tunnel
{
	// Token: 0x06004789 RID: 18313 RVA: 0x0002737D File Offset: 0x0002557D
	protected override void OnEnable()
	{
		base.OnEnable();
		this.IsClosetUnlocked = false;
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x001161E4 File Offset: 0x001143E4
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

	// Token: 0x040036DE RID: 14046
	[NonSerialized]
	public bool IsClosetUnlocked;
}
