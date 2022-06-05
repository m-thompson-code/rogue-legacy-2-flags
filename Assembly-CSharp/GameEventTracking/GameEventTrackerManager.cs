using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DD8 RID: 3544
	public class GameEventTrackerManager : MonoBehaviour
	{
		// Token: 0x1700201B RID: 8219
		// (get) Token: 0x06006392 RID: 25490 RVA: 0x00036E1E File Offset: 0x0003501E
		public static RoomEventTracker RoomEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_roomEventTracker;
			}
		}

		// Token: 0x1700201C RID: 8220
		// (get) Token: 0x06006393 RID: 25491 RVA: 0x00036E2A File Offset: 0x0003502A
		public static EnemyEventTracker EnemyEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_enemyEventTracker;
			}
		}

		// Token: 0x1700201D RID: 8221
		// (get) Token: 0x06006394 RID: 25492 RVA: 0x00036E36 File Offset: 0x00035036
		public static ItemEventTracker ItemEventTracker
		{
			get
			{
				return GameEventTrackerManager.m_instance.m_itemEventTracker;
			}
		}

		// Token: 0x1700201E RID: 8222
		// (get) Token: 0x06006395 RID: 25493 RVA: 0x00036E42 File Offset: 0x00035042
		public static bool IsInstantiated
		{
			get
			{
				return GameEventTrackerManager.m_instance != null;
			}
		}

		// Token: 0x06006396 RID: 25494 RVA: 0x00036E4F File Offset: 0x0003504F
		private void Awake()
		{
			if (GameEventTrackerManager.m_instance == null)
			{
				GameEventTrackerManager.m_instance = this;
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06006397 RID: 25495 RVA: 0x00036E70 File Offset: 0x00035070
		private void OnDestroy()
		{
			GameEventTrackerManager.m_instance = null;
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x001728A8 File Offset: 0x00170AA8
		public static List<IGameEventData> GetGameEvents()
		{
			return (from gameEvent in GameEventTrackerManager.GetGameEventsFromTrackers()
			orderby gameEvent.TimesLoaded, gameEvent.TimeStamp
			select gameEvent).ToList<IGameEventData>();
		}

		// Token: 0x06006399 RID: 25497 RVA: 0x00036E78 File Offset: 0x00035078
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

		// Token: 0x0600639A RID: 25498 RVA: 0x00036E81 File Offset: 0x00035081
		public static void Reset()
		{
			GameEventTrackerManager.RoomEventTracker.Reset();
			GameEventTrackerManager.EnemyEventTracker.Reset();
			GameEventTrackerManager.ItemEventTracker.Reset();
		}

		// Token: 0x0600639B RID: 25499 RVA: 0x00172908 File Offset: 0x00170B08
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

		// Token: 0x0600639C RID: 25500 RVA: 0x00036EA1 File Offset: 0x000350A1
		public static void SaveStates(ref List<IGameEventTrackerState> trackerStateList)
		{
			trackerStateList.Clear();
			trackerStateList.Add(GameEventTrackerManager.RoomEventTracker.SaveState());
			trackerStateList.Add(GameEventTrackerManager.EnemyEventTracker.SaveState());
			trackerStateList.Add(GameEventTrackerManager.ItemEventTracker.SaveState());
		}

		// Token: 0x04005146 RID: 20806
		[SerializeField]
		private RoomEventTracker m_roomEventTracker;

		// Token: 0x04005147 RID: 20807
		[SerializeField]
		private EnemyEventTracker m_enemyEventTracker;

		// Token: 0x04005148 RID: 20808
		[SerializeField]
		private ItemEventTracker m_itemEventTracker;

		// Token: 0x04005149 RID: 20809
		private static GameEventTrackerManager m_instance;
	}
}
