using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000521 RID: 1313
[Serializable]
public class PlayMusic_SummonRule : BaseSummonRule
{
	// Token: 0x170011E8 RID: 4584
	// (get) Token: 0x06003089 RID: 12425 RVA: 0x000A5D48 File Offset: 0x000A3F48
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.PlayMusic;
		}
	}

	// Token: 0x170011E9 RID: 4585
	// (get) Token: 0x0600308A RID: 12426 RVA: 0x000A5D4F File Offset: 0x000A3F4F
	public override string RuleLabel
	{
		get
		{
			return "Play Music";
		}
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x000A5D56 File Offset: 0x000A3F56
	public override IEnumerator RunSummonRule()
	{
		if (this.m_songID != MusicManager.CurrentSong)
		{
			MusicManager.StopMusic();
			if (this.m_songID != SongID.None)
			{
				MusicManager.PlayMusic(this.m_songID, true, false);
			}
		}
		base.IsRuleComplete = true;
		yield break;
	}

	// Token: 0x04002689 RID: 9865
	[SerializeField]
	private SongID m_songID;
}
