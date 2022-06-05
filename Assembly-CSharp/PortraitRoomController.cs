using System;
using System.Collections.Generic;
using RL_Windows;
using UnityEngine;

// Token: 0x0200087B RID: 2171
public class PortraitRoomController : BaseSpecialRoomController
{
	// Token: 0x060042B5 RID: 17077 RVA: 0x0010B508 File Offset: 0x00109708
	public void TriggerPortrait()
	{
		if (this.m_portraitType == PortraitType.Random)
		{
			List<PortraitType> list = new List<PortraitType>();
			foreach (PortraitType portraitType in PortraitType_RL.TypeArray)
			{
				if (portraitType != PortraitType.None && portraitType != PortraitType.Random)
				{
					list.Add(portraitType);
				}
			}
			this.m_portraitType = list[UnityEngine.Random.Range(0, list.Count)];
		}
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		PortraitType portraitType2 = this.m_portraitType;
		if (portraitType2 != PortraitType.Story1)
		{
			if (portraitType2 == PortraitType.Story2)
			{
				DialogueManager.AddNonLocDialogue("Narrator", "This is the second story.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			}
		}
		else
		{
			DialogueManager.AddNonLocDialogue("Narrator", "This is the first story.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		this.RoomCompleted();
	}

	// Token: 0x0400341D RID: 13341
	[SerializeField]
	private PortraitType m_portraitType;
}
