using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x02000847 RID: 2119
public class BridgeBossRoomController : BossRoomController
{
	// Token: 0x17001791 RID: 6033
	// (get) Token: 0x0600417D RID: 16765 RVA: 0x00024445 File Offset: 0x00022645
	public override float BossHealthAsPercentage
	{
		get
		{
			if (this.m_bossesSpawned == 1)
			{
				return 1f;
			}
			if (this.m_bossesSpawned == 2)
			{
				return 0.5f;
			}
			return 0f;
		}
	}

	// Token: 0x17001792 RID: 6034
	// (get) Token: 0x0600417E RID: 16766 RVA: 0x0002446A File Offset: 0x0002266A
	// (set) Token: 0x0600417F RID: 16767 RVA: 0x00024472 File Offset: 0x00022672
	public EnemyController Boss2 { get; private set; }

	// Token: 0x06004180 RID: 16768 RVA: 0x00107A14 File Offset: 0x00105C14
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		if (this.m_isPrimeVariant)
		{
			@string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STONE_PRIME_BOSS_DEFEATED_TITLE_1", false, false);
		}
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x0002447B File Offset: 0x0002267B
	protected override IEnumerator StartIntro()
	{
		if (PlayerManager.GetPlayerController().IsFacingRight)
		{
			PlayerManager.GetPlayerController().CharacterCorgi.Flip(false, false);
		}
		AudioManager.Play(null, "event:/SFX/Enemies/vo_skellyBoss_agro", default(Vector3));
		MusicManager.PlayMusic(SongID.BridgeBossBGM_Tettix_Miniboss_BGM_137, false, false);
		base.Boss.Visuals.SetActive(false);
		yield return base.StartIntro();
		this.m_bossesSpawned++;
		yield break;
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x0002448A File Offset: 0x0002268A
	protected override void OnModeShift(object sender, EventArgs args)
	{
		base.OnModeShift(sender, args);
		if (!this.m_secondBossSpawned)
		{
			this.m_secondBossSpawned = true;
			base.StartCoroutine(this.SpawnSecondBossCoroutine());
		}
	}

	// Token: 0x06004183 RID: 16771 RVA: 0x000244B0 File Offset: 0x000226B0
	private IEnumerator SpawnSecondBossCoroutine()
	{
		SkeletonBoss_Basic_AIScript aiScript = base.Boss.LogicController.LogicScript as SkeletonBoss_Basic_AIScript;
		while (!aiScript.ModeShiftComplete_SpawnSecondBoss)
		{
			yield return null;
		}
		if (base.Boss.EnemyRank == EnemyRank.Basic)
		{
			this.Boss2 = EnemyManager.SummonEnemy(base.Boss, EnemyType.SkeletonBossB, EnemyRank.Basic, this.m_secondBossSpawnPosition.transform.position, true, true, 1f, 1f);
		}
		else
		{
			this.Boss2 = EnemyManager.SummonEnemy(base.Boss, EnemyType.SkeletonBossB, EnemyRank.Advanced, this.m_secondBossSpawnPosition.transform.position, true, true, 1f, 1f);
		}
		this.m_bossesSpawned++;
		AudioManager.Play(null, "event:/SFX/Enemies/vo_skellyBoss_laugh", this.Boss2.transform.localPosition);
		MusicManager.SetBossEncounterParam(0.51f);
		yield break;
	}

	// Token: 0x04003348 RID: 13128
	private const string ON_ENTER_AUDIO = "event:/SFX/Enemies/vo_skellyBoss_agro";

	// Token: 0x04003349 RID: 13129
	private const string SECOND_BOSS_SUMMON_AUDIO = "event:/SFX/Enemies/vo_skellyBoss_laugh";

	// Token: 0x0400334A RID: 13130
	[SerializeField]
	private GameObject m_secondBossSpawnPosition;

	// Token: 0x0400334B RID: 13131
	[SerializeField]
	private bool m_isPrimeVariant;

	// Token: 0x0400334C RID: 13132
	private int m_bossesSpawned;

	// Token: 0x0400334D RID: 13133
	private bool m_secondBossSpawned;

	// Token: 0x0400334E RID: 13134
	private bool m_outroPlaying;
}
