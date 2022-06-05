using System;
using System.Collections;
using RLAudio;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class BridgeBossRoomController : BossRoomController
{
	// Token: 0x170011B2 RID: 4530
	// (get) Token: 0x06002F68 RID: 12136 RVA: 0x000A222F File Offset: 0x000A042F
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

	// Token: 0x170011B3 RID: 4531
	// (get) Token: 0x06002F69 RID: 12137 RVA: 0x000A2254 File Offset: 0x000A0454
	// (set) Token: 0x06002F6A RID: 12138 RVA: 0x000A225C File Offset: 0x000A045C
	public EnemyController Boss2 { get; private set; }

	// Token: 0x06002F6B RID: 12139 RVA: 0x000A2268 File Offset: 0x000A0468
	protected override void InitializeObjectiveCompleteArgs(float bossDefeatedDisplayDuration)
	{
		string @string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STYGIAN_BOSS_DEFEATED_TITLE_1", false, false);
		if (this.m_isPrimeVariant)
		{
			@string = LocalizationManager.GetString("LOC_ID_BIG_TEXT_UI_STONE_PRIME_BOSS_DEFEATED_TITLE_1", false, false);
		}
		this.m_bossDefeatedArgs.Initialize(base.Boss.EnemyType, EnemyRank.Basic, bossDefeatedDisplayDuration, @string, null, null);
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x000A22B2 File Offset: 0x000A04B2
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

	// Token: 0x06002F6D RID: 12141 RVA: 0x000A22C1 File Offset: 0x000A04C1
	protected override void OnModeShift(object sender, EventArgs args)
	{
		base.OnModeShift(sender, args);
		if (!this.m_secondBossSpawned)
		{
			this.m_secondBossSpawned = true;
			base.StartCoroutine(this.SpawnSecondBossCoroutine());
		}
	}

	// Token: 0x06002F6E RID: 12142 RVA: 0x000A22E7 File Offset: 0x000A04E7
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

	// Token: 0x040025CD RID: 9677
	private const string ON_ENTER_AUDIO = "event:/SFX/Enemies/vo_skellyBoss_agro";

	// Token: 0x040025CE RID: 9678
	private const string SECOND_BOSS_SUMMON_AUDIO = "event:/SFX/Enemies/vo_skellyBoss_laugh";

	// Token: 0x040025CF RID: 9679
	[SerializeField]
	private GameObject m_secondBossSpawnPosition;

	// Token: 0x040025D0 RID: 9680
	[SerializeField]
	private bool m_isPrimeVariant;

	// Token: 0x040025D1 RID: 9681
	private int m_bossesSpawned;

	// Token: 0x040025D2 RID: 9682
	private bool m_secondBossSpawned;

	// Token: 0x040025D3 RID: 9683
	private bool m_outroPlaying;
}
