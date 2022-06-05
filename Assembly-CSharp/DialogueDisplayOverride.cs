using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class DialogueDisplayOverride : MonoBehaviour
{
	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06001459 RID: 5209 RVA: 0x0003DDDE File Offset: 0x0003BFDE
	public bool UseLocIDOverride
	{
		get
		{
			return this.m_useLocIDOverride;
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x0600145A RID: 5210 RVA: 0x0003DDE6 File Offset: 0x0003BFE6
	public string DialogueOverride
	{
		get
		{
			return this.m_dialogueOverride;
		}
	}

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x0600145B RID: 5211 RVA: 0x0003DDEE File Offset: 0x0003BFEE
	public string RepeatedDialogueOverride
	{
		get
		{
			return this.m_repeatedDialogueOverride;
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x0600145C RID: 5212 RVA: 0x0003DDF6 File Offset: 0x0003BFF6
	public string SpeakerOverride
	{
		get
		{
			return this.m_speakerOverride;
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x0600145D RID: 5213 RVA: 0x0003DDFE File Offset: 0x0003BFFE
	public PlayerSaveFlag SpokenFlagOverride
	{
		get
		{
			return this.m_spokenFlagOverride;
		}
	}

	// Token: 0x04001421 RID: 5153
	[SerializeField]
	private bool m_useLocIDOverride = true;

	// Token: 0x04001422 RID: 5154
	[SerializeField]
	private string m_dialogueOverride;

	// Token: 0x04001423 RID: 5155
	[SerializeField]
	private string m_repeatedDialogueOverride;

	// Token: 0x04001424 RID: 5156
	[SerializeField]
	private string m_speakerOverride;

	// Token: 0x04001425 RID: 5157
	[SerializeField]
	private PlayerSaveFlag m_spokenFlagOverride;
}
