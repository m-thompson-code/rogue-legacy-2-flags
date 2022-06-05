using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008A7 RID: 2215
	public class GameEventTrackerManager : MonoBehaviour
	{
		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x06004841 RID: 18497 RVA: 0x00103D7D File Offset: 0x00101F7D
		public static RoomEventTracker RoomEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_roomEventTracker;
			}
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x06004842 RID: 18498 RVA: 0x00103D89 File Offset: 0x00101F89
		public static EnemyEventTracker EnemyEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_enemyEventTracker;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x00103D95 File Offset: 0x00101F95
		public static ItemEventTracker ItemEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_itemEventTracker;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x06004844 RID: 18500 RVA: 0x00103DA1 File Offset: 0x00101FA1
		public static bool IsInstantiated
		{
			get
			{
				return GameEventTrackerManager.m_instance != null;
			}
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00103DAE File Offset: 0x00101FAE
		private void Awake()
		{
			if (GameEventTrackerManager.m_instance == null)
			{
				GameEventTrackerManager.m_instance = this;
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x00103DCF File Offset: 0x00101FCF
		private void OnDestroy()
		{
			GameEventTrackerManager.m_instance = null;
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x00103DD8 File Offset: 0x00101FD8
		public static List<IGameEventData> GetGameEvents()
		{
			return (from gameEvent in GameEventTrackerManager.GetGameEventsFromTrackers()
			orderby gameEvent.TimesLoaded, gameEvent.TimeStamp
			select gameEvent).ToList<IGameEventData>();
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00103E37 File Offset: 0x00102037
		private static IEnumerable<IGameEventData> GetGameEventsFromTrackers()
		{
			foreach (IGameEventData gameEventData in GameEventTrackerManager.RoomEventTracker.GetGameEvents())
			{
				yield return gameEventData;
			}
			IEnumerator<IGameEventData> enumerator = null;
			foreach (IGameEventData gameEventData2 in GameEventTrackerManager.EnemyEventTracker.GetGameEvents())
			{
				yield return gameEventData2;
			}
			enumerator = null;
			foreach (IGameEventData gameEventData3 in GameEventTrackerManager.ItemEventTracker.GetGameEvents())
			{
				yield return gameEventData3;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00103E40 File Offset: 0x00102040
		public static void Reset()
		{
			GameEventTrackerManager.RoomEventTracker.Reset();
			GameEventTrackerManager.EnemyEventTracker.Reset();
			GameEventTrackerManager.ItemEventTracker.Reset();
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00103E60 File Offset: 0x00102060
		public static void RestoreStates(List<IGameEventTrackerState> states)
		{
			foreach (IGameEventTrackerState gameEventTrackerState in states)
			{
				if (gameEventTrackerState is IRoomEventTrackerState)
				{
					GameEventTrackerManager.RoomEventTracker.RestoreState(gameEventTrackerState as IRoomEventTrackerState);
				}
				else if (gameEventTrackerState is IEnemyEventTrackerState)
				{
					GameEventTrackerManager.EnemyEventTracker.RestoreState(gameEventTrackerState as IEnemyEventTrackerState);
				}
				else if (gameEventTrackerState is IItemEventTrackerState)
				{
					GameEventTrackerManager.ItemEventTracker.RestoreState(gameEventTrackerState as IItemEventTrackerState);
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | No condition exists for Game Tracker State ({1})", new object[]
					{
						GameEventTrackerManager.m_instance,
						gameEventTrackerState.GetType()
					});
				}
			}
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x00103F18 File Offset: 0x00102118
		public static void SaveStates(ref List<IGameEventTrackerState> trackerStateList)
		{
			trackerStateList.Clear();
			trackerStateList.Add(GameEventTrackerManager.RoomEventTracker.SaveState());
			trackerStateList.Add(GameEventTrackerManager.EnemyEventTracker.SaveState());
			trackerStateList.Add(GameEventTrackerManager.ItemEventTracker.SaveState());
		}

		// Token: 0x04003D06 RID: 15622
		[SerializeField]
		private RoomEventTracker m_roomEventTracker;

		// Token: 0x04003D07 RID: 15623
		[SerializeField]
		private EnemyEventTracker m_enemyEventTracker;

		// Token: 0x04003D08 RID: 15624
		[SerializeField]
		private ItemEventTracker m_itemEventTracker;

		// Token: 0x04003D09 RID: 15625
		private static GameEventTrackerManager m_instance;
	}
}
