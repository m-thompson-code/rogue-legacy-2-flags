using System;
using UnityEngine;

// Token: 0x02000388 RID: 904
public class DialogueDisplayOverride : MonoBehaviour
{
	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0000F362 File Offset: 0x0000D562
	public bool UseLocIDOverride
	{
		get
		{
			return this.m_useLocIDOverride;
		}
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x06001D7B RID: 7547 RVA: 0x0000F36A File Offset: 0x0000D56A
	public string DialogueOverride
	{
		get
		{
			return this.m_dialogueOverride;
		}
	}

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x06001D7C RID: 7548 RVA: 0x0000F372 File Offset: 0x0000D572
	public string RepeatedDialogueOverride
	{
		get
		{
			return this.m_repeatedDialogueOverride;
		}
	}

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x06001D7D RID: 7549 RVA: 0x0000F37A File Offset: 0x0000D57A
	public string SpeakerOverride
	{
		get
		{
			return this.m_speakerOverride;
		}
	}

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x06001D7E RID: 7550 RVA: 0x0000F382 File Offset: 0x0000D582
	public PlayerSaveFlag SpokenFlagOverride
	{
		get
		{
			return this.m_spokenFlagOverride;
		}
	}

	// Token: 0x04001AC8 RID: 6856
	[SerializeField]
	private bool m_useLocIDOverride = true;

	// Token: 0x04001AC9 RID: 6857
	[SerializeField]
	private string m_dialogueOverride;

	// Token: 0x04001ACA RID: 6858
	[SerializeField]
	private string m_repeatedDialogueOverride;

	// Token: 0x04001ACB RID: 6859
	[SerializeField]
	private string m_speakerOverride;

	// Token: 0x04001ACC RID: 6860
	[SerializeField]
	private PlayerSaveFlag m_spokenFlagOverride;
}
