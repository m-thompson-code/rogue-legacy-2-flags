using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DD4 RID: 3540
	public class EnemyEventTracker : MonoBehaviour, IGameEventTracker<IEnemyEventTrackerState>
	{
		// Token: 0x17002016 RID: 8214
		// (get) Token: 0x06006373 RID: 25459 RVA: 0x00036C6C File Offset: 0x00034E6C
		// (set) Token: 0x06006374 RID: 25460 RVA: 0x00036C74 File Offset: 0x00034E74
		public List<EnemyTrackerData> EnemiesKilled
		{
			get
			{
				return this.m_enemiesKilled;
			}
			private set
			{
				this.m_enemiesKilled = value;
			}
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x00036C7D File Offset: 0x00034E7D
		private void Awake()
		{
			this.m_onEnemyKilled = new Action<MonoBehaviour, EventArgs>(this.OnEnemyKilled);
			this.m_onPlayerKilled = new Action<MonoBehaviour, EventArgs>(this.OnPlayerKilled);
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x00036CA3 File Offset: 0x00034EA3
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerKilled);
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x00036CBF File Offset: 0x00034EBF
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerKilled);
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x00036CDB File Offset: 0x00034EDB
		public IEnumerable<IGameEventData> GetGameEvents()
		{
			foreach (EnemyTrackerData enemyTrackerData in this.EnemiesKilled)
			{
				yield return enemyTrackerData;
			}
			List<EnemyTrackerData>.Enumerator enumerator = default(List<EnemyTrackerData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x00036CEB File Offset: 0x00034EEB
		public string GetPlayerKiller()
		{
			return this.m_playerKiller;
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x00036CF3 File Offset: 0x00034EF3
		public string GetSlainBy()
		{
			return this.m_slainByText;
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x001724E4 File Offset: 0x001706E4
		private void OnEnemyKilled(MonoBehaviour sender, EventArgs eventArgs)
		{
			if (eventArgs is EnemyDeathEventArgs)
			{
				EnemyController victim = (eventArgs as EnemyDeathEventArgs).Victim;
				if (!victim.IsSummoned || victim.IsBoss)
				{
					this.EnemiesKilled.Add(new EnemyTrackerData(victim.Room.BiomeType, victim.Room.BiomeControllerIndex, victim.EnemyType, victim.EnemyRank, victim.EnemyIndex));
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>| {0} | Failed to cast event args as EnemyDeathEventArgs</color>", new object[]
				{
					this
				});
			}
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x00172564 File Offset: 0x00170764
		private void OnPlayerKilled(MonoBehaviour sender, EventArgs eventArgs)
		{
			this.m_slainByLocID = null;
			this.m_playerKillerLocID = null;
			if (SaveManager.PlayerSaveData.CurrentCharacter.IsRetired)
			{
				this.m_slainByText = LocalizationManager.GetString("LOC_ID_DEATH_UI_RETIRED_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				this.m_playerKiller = "";
				return;
			}
			if (eventArgs is PlayerDeathEventArgs)
			{
				PlayerDeathEventArgs playerDeathEventArgs = eventArgs as PlayerDeathEventArgs;
				bool flag = false;
				if (playerDeathEventArgs.Killer != null)
				{
					if (playerDeathEventArgs.Killer.CompareTag("Enemy"))
					{
						EnemyController component = playerDeathEventArgs.Killer.GetComponent<EnemyController>();
						EnemyData enemyData = component.EnemyData;
						this.m_playerKillerLocID = enemyData.Title;
						if (!component.IsBoss)
						{
							this.m_slainByLocID = "LOC_ID_DEATH_UI_SLAIN_BY_ENEMY_1";
						}
						else
						{
							this.m_slainByLocID = "LOC_ID_DEATH_UI_SLAIN_BY_BOSS_1";
						}
					}
					else if (playerDeathEventArgs.Killer.CompareTag("Hazard"))
					{
						this.m_slainByLocID = "LOC_ID_DEATH_UI_SLAIN_BY_ENEMY_1";
						this.m_playerKillerLocID = "LOC_ID_ENEMY_TITLE_Hazard_Basic_1";
						this.m_slainByText = LocalizationManager.GetString("LOC_ID_DEATH_UI_SLAIN_BY_ENEMY_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
						this.m_playerKiller = LocalizationManager.GetString("LOC_ID_ENEMY_TITLE_Hazard_Basic_1", false, false);
					}
					else
					{
						this.m_slainByLocID = "LOC_ID_DEATH_UI_SLAIN_BY_ENEMY_1";
						this.m_playerKiller = LocalizationManager.GetString("LOC_ID_DEATH_UI_MYSTERIOUS_FORCE_1", false, false);
						flag = true;
					}
				}
				else
				{
					this.m_slainByLocID = "LOC_ID_DEATH_UI_SLAIN_BY_ENEMY_1";
					this.m_playerKiller = LocalizationManager.GetString("LOC_ID_DEATH_UI_MYSTERIOUS_FORCE_1", false, false);
					flag = true;
				}
				if (!flag)
				{
					this.m_playerKiller = LocalizationManager.GetString(this.m_playerKillerLocID, false, false);
				}
				this.m_slainByText = LocalizationManager.GetString(this.m_slainByLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Failed to cast event args as PlayerDeathEventArgs</color>", new object[]
			{
				this
			});
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x00036CFB File Offset: 0x00034EFB
		public void Reset()
		{
			if (this.EnemiesKilled != null)
			{
				this.EnemiesKilled.Clear();
			}
			this.m_playerKiller = "UNDEFINED";
			this.m_slainByText = "UNDEFINED";
		}

		// Token: 0x0600637E RID: 25470 RVA: 0x00036D26 File Offset: 0x00034F26
		public void RestoreState(IEnemyEventTrackerState state)
		{
			this.EnemiesKilled = state.EnemiesKilled;
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x00036D34 File Offset: 0x00034F34
		public IEnemyEventTrackerState SaveState()
		{
			if (this.m_state == null)
			{
				this.m_state = new EnemyEventTrackerState(this.EnemiesKilled);
			}
			else
			{
				this.m_state.Initialise(this.EnemiesKilled);
			}
			return this.m_state;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x00172720 File Offset: 0x00170920
		public void ForceRefreshText()
		{
			if (!string.IsNullOrEmpty(this.m_playerKillerLocID))
			{
				this.m_playerKiller = LocalizationManager.GetString(this.m_playerKillerLocID, false, false);
			}
			if (!string.IsNullOrEmpty(this.m_slainByLocID))
			{
				this.m_slainByText = LocalizationManager.GetString(this.m_slainByLocID, SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false);
			}
		}

		// Token: 0x04005137 RID: 20791
		private IEnemyEventTrackerState m_state;

		// Token: 0x04005138 RID: 20792
		private List<EnemyTrackerData> m_enemiesKilled = new List<EnemyTrackerData>();

		// Token: 0x04005139 RID: 20793
		private string m_playerKiller = "UNDEFINED";

		// Token: 0x0400513A RID: 20794
		private string m_slainByText = "UNDEFINED";

		// Token: 0x0400513B RID: 20795
		private string m_playerKillerLocID;

		// Token: 0x0400513C RID: 20796
		private string m_slainByLocID;

		// Token: 0x0400513D RID: 20797
		private Action<MonoBehaviour, EventArgs> m_onEnemyKilled;

		// Token: 0x0400513E RID: 20798
		private Action<MonoBehaviour, EventArgs> m_onPlayerKilled;
	}
}
