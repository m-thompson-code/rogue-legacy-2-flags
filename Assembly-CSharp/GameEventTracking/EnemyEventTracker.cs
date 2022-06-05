using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008A4 RID: 2212
	public class EnemyEventTracker : MonoBehaviour, IGameEventTracker<IEnemyEventTrackerState>
	{
		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x0600482B RID: 18475 RVA: 0x00103963 File Offset: 0x00101B63
		// (set) Token: 0x0600482C RID: 18476 RVA: 0x0010396B File Offset: 0x00101B6B
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

		// Token: 0x0600482D RID: 18477 RVA: 0x00103974 File Offset: 0x00101B74
		private void Awake()
		{
			this.m_onEnemyKilled = new Action<MonoBehaviour, EventArgs>(this.OnEnemyKilled);
			this.m_onPlayerKilled = new Action<MonoBehaviour, EventArgs>(this.OnPlayerKilled);
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0010399A File Offset: 0x00101B9A
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerKilled);
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x001039B6 File Offset: 0x00101BB6
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyKilled);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerKilled);
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x001039D2 File Offset: 0x00101BD2
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

		// Token: 0x06004831 RID: 18481 RVA: 0x001039E2 File Offset: 0x00101BE2
		public string GetPlayerKiller()
		{
			return this.m_playerKiller;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x001039EA File Offset: 0x00101BEA
		public string GetSlainBy()
		{
			return this.m_slainByText;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x001039F4 File Offset: 0x00101BF4
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

		// Token: 0x06004834 RID: 18484 RVA: 0x00103A74 File Offset: 0x00101C74
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

		// Token: 0x06004835 RID: 18485 RVA: 0x00103C30 File Offset: 0x00101E30
		public void Reset()
		{
			if (this.EnemiesKilled != null)
			{
				this.EnemiesKilled.Clear();
			}
			this.m_playerKiller = "UNDEFINED";
			this.m_slainByText = "UNDEFINED";
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x00103C5B File Offset: 0x00101E5B
		public void RestoreState(IEnemyEventTrackerState state)
		{
			this.EnemiesKilled = state.EnemiesKilled;
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00103C69 File Offset: 0x00101E69
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

		// Token: 0x06004838 RID: 18488 RVA: 0x00103CA0 File Offset: 0x00101EA0
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

		// Token: 0x04003CFC RID: 15612
		private IEnemyEventTrackerState m_state;

		// Token: 0x04003CFD RID: 15613
		private List<EnemyTrackerData> m_enemiesKilled = new List<EnemyTrackerData>();

		// Token: 0x04003CFE RID: 15614
		private string m_playerKiller = "UNDEFINED";

		// Token: 0x04003CFF RID: 15615
		private string m_slainByText = "UNDEFINED";

		// Token: 0x04003D00 RID: 15616
		private string m_playerKillerLocID;

		// Token: 0x04003D01 RID: 15617
		private string m_slainByLocID;

		// Token: 0x04003D02 RID: 15618
		private Action<MonoBehaviour, EventArgs> m_onEnemyKilled;

		// Token: 0x04003D03 RID: 15619
		private Action<MonoBehaviour, EventArgs> m_onPlayerKilled;
	}
}
