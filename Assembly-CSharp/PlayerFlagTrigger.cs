using System;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public class PlayerFlagTrigger : MonoBehaviour, IDisplaySpeechBubble
{
	// Token: 0x17001257 RID: 4695
	// (get) Token: 0x06003255 RID: 12885 RVA: 0x000AA8BC File Offset: 0x000A8ABC
	public SpeechBubbleType BubbleType
	{
		get
		{
			return SpeechBubbleType.Dialogue;
		}
	}

	// Token: 0x17001258 RID: 4696
	// (get) Token: 0x06003256 RID: 12886 RVA: 0x000AA8BF File Offset: 0x000A8ABF
	public bool ShouldDisplaySpeechBubble
	{
		get
		{
			return !SaveManager.PlayerSaveData.GetFlag(this.m_flagToTrigger);
		}
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x000AA8D4 File Offset: 0x000A8AD4
	public void Trigger()
	{
		SaveManager.PlayerSaveData.SetFlag(this.m_flagToTrigger, true);
	}

	// Token: 0x04002788 RID: 10120
	[SerializeField]
	private GameObject m_speechBubble;

	// Token: 0x04002789 RID: 10121
	[SerializeField]
	private PlayerSaveFlag m_flagToTrigger;
}
