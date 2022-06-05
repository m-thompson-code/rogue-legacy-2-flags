using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E56 RID: 3670
	public class CheerOnKillsAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700212E RID: 8494
		// (get) Token: 0x0600678A RID: 26506 RVA: 0x000391E8 File Offset: 0x000373E8
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x0600678B RID: 26507 RVA: 0x0017D100 File Offset: 0x0017B300
		private void Awake()
		{
			CheerOnKills_Trait component = base.GetComponent<CheerOnKills_Trait>();
			component.CheerAfterKillRelay.AddListener(new Action<EnemyTypeAndRank>(this.OnCheerAfterKill), false);
			component.RosesThrownRelay.AddListener(new Action(this.OnRosesThrown), false);
			component.PlayerDeathRelay.AddListener(new Action(this.OnPlayerDeath), false);
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x0017D160 File Offset: 0x0017B360
		private void OnPlayerDeath()
		{
			AudioManager.PlayOneShot(this, this.m_playerDeathAudioPath, default(Vector3));
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x0017D184 File Offset: 0x0017B384
		private void OnRosesThrown()
		{
			AudioManager.PlayOneShot(this, this.m_rosesThrownAudioPath, default(Vector3));
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x0017D1A8 File Offset: 0x0017B3A8
		private void OnCheerAfterKill(EnemyTypeAndRank enemyTypeAndRank)
		{
			string path = this.m_cheerShortAudioPath;
			if (this.GetUseLongCheer(enemyTypeAndRank))
			{
				path = this.m_cheerLongAudioPath;
			}
			AudioManager.PlayOneShot(this, path, default(Vector3));
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x0017D1DC File Offset: 0x0017B3DC
		private bool GetUseLongCheer(EnemyTypeAndRank enemyTypeAndRank)
		{
			bool flag = false;
			EnemyType type = enemyTypeAndRank.Type;
			if (type <= EnemyType.StudyBoss)
			{
				if (type <= EnemyType.DancingBoss)
				{
					if (type != EnemyType.SpellswordBoss && type != EnemyType.DancingBoss)
					{
						goto IL_6E;
					}
				}
				else if (type - EnemyType.SkeletonBossA > 1 && type != EnemyType.StudyBoss)
				{
					goto IL_6E;
				}
			}
			else if (type <= EnemyType.CaveBoss)
			{
				if (type - EnemyType.EyeballBoss_Left > 3 && type != EnemyType.CaveBoss)
				{
					goto IL_6E;
				}
			}
			else if (type != EnemyType.TraitorBoss && type != EnemyType.FinalBoss)
			{
				goto IL_6E;
			}
			flag = true;
			IL_6E:
			if (!flag)
			{
				EnemyRank rank = enemyTypeAndRank.Rank;
				if (rank - EnemyRank.Expert <= 1)
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x040053DF RID: 21471
		[SerializeField]
		[EventRef]
		private string m_cheerShortAudioPath;

		// Token: 0x040053E0 RID: 21472
		[SerializeField]
		[EventRef]
		private string m_cheerLongAudioPath;

		// Token: 0x040053E1 RID: 21473
		[SerializeField]
		[EventRef]
		private string m_rosesThrownAudioPath;

		// Token: 0x040053E2 RID: 21474
		[SerializeField]
		[EventRef]
		private string m_playerDeathAudioPath;

		// Token: 0x040053E3 RID: 21475
		private string m_description = string.Empty;
	}
}
