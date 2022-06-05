using System;
using UnityEngine;

// Token: 0x0200091E RID: 2334
public class PlayerFlagTrigger : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x17001904 RID: 6404
	// (get) Token: 0x060046E2 RID: 18146 RVA: 0x000047A7 File Offset: 0x000029A7
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001905 RID: 6405
	// (get) Token: 0x060046E3 RID: 18147 RVA: 0x00026E3C File Offset: 0x0002503C
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return !SaveManager.PlayerSaveData.GetFlag(this.m_flagToTrigger);
		}
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x00026E51 File Offset: 0x00025051
	public void Trigger()
	{
		SaveManager.PlayerSaveData.SetFlag(this.m_flagToTrigger, true);
	}

	// Token: 0x0400368F RID: 13967
	[SerializeField]
	private GameObject m_speechBubble;

	// Token: 0x04003690 RID: 13968
	[SerializeField]
	private PlayerSaveFlag m_flagToTrigger;
}
