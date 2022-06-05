using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008E6 RID: 2278
	public class CheerOnKillsAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x06004AD1 RID: 19153 RVA: 0x0010CD33 File Offset: 0x0010AF33
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

		// Token: 0x06004AD2 RID: 19154 RVA: 0x0010CD5C File Offset: 0x0010AF5C
		private void Awake()
		{
			CheerOnKills_Trait component = base.GetComponent<CheerOnKills_Trait>();
			component.CheerAfterKillRelay.AddListener(new Action<EnemyTypeAndRank>(this.OnCheerAfterKill), false);
			component.RosesThrownRelay.AddListener(new Action(this.OnRosesThrown), false);
			component.PlayerDeathRelay.AddListener(new Action(this.OnPlayerDeath), false);
		}

		// Token: 0x06004AD3 RID: 19155 RVA: 0x0010CDBC File Offset: 0x0010AFBC
		private void OnPlayerDeath()
		{
			AudioManager.PlayOneShot(this, this.m_playerDeathAudioPath, default(Vector3));
		}

		// Token: 0x06004AD4 RID: 19156 RVA: 0x0010CDE0 File Offset: 0x0010AFE0
		private void OnRosesThrown()
		{
			AudioManager.PlayOneShot(this, this.m_rosesThrownAudioPath, default(Vector3));
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x0010CE04 File Offset: 0x0010B004
		private void OnCheerAfterKill(EnemyTypeAndRank enemyTypeAndRank)
		{
			string path = this.m_cheerShortAudioPath;
			if (this.GetUseLongCheer(enemyTypeAndRank))
			{
				path = this.m_cheerLongAudioPath;
			}
			AudioManager.PlayOneShot(this, path, default(Vector3));
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x0010CE38 File Offset: 0x0010B038
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

		// Token: 0x04003EC1 RID: 16065
		[SerializeField]
		[EventRef]
		private string m_cheerShortAudioPath;

		// Token: 0x04003EC2 RID: 16066
		[SerializeField]
		[EventRef]
		private string m_cheerLongAudioPath;

		// Token: 0x04003EC3 RID: 16067
		[SerializeField]
		[EventRef]
		private string m_rosesThrownAudioPath;

		// Token: 0x04003EC4 RID: 16068
		[SerializeField]
		[EventRef]
		private string m_playerDeathAudioPath;

		// Token: 0x04003EC5 RID: 16069
		private string m_description = string.Empty;
	}
}
