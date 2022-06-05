using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x0200089B RID: 2203
[Serializable]
public class PlayMusic_SummonRule : BaseSummonRule
{
	// Token: 0x17001809 RID: 6153
	// (get) Token: 0x06004369 RID: 17257 RVA: 0x00025408 File Offset: 0x00023608
	public override SummonRuleType RuleType
	{
		get
		{
			return SummonRuleType.PlayMusic;
		}
	}

	// Token: 0x1700180A RID: 6154
	// (get) Token: 0x0600436A RID: 17258 RVA: 0x0002540F File Offset: 0x0002360F
	public override string RuleLabel
	{
		get
		{
			return "Play Music";
		}
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x00025416 File Offset: 0x00023616
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

	// Token: 0x04003484 RID: 13444
	[SerializeField]
	private SongID m_songID;
}
