using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x02000DE1 RID: 3553
	public class ItemEventTracker : MonoBehaviour, IGameEventTracker<IItemEventTrackerState>
	{
		// Token: 0x17002040 RID: 8256
		// (get) Token: 0x060063D3 RID: 25555 RVA: 0x000370BF File Offset: 0x000352BF
		// (set) Token: 0x060063D4 RID: 25556 RVA: 0x000370C7 File Offset: 0x000352C7
		public List<ChestTrackerData> ChestsOpened { get; private set; } = new List<ChestTrackerData>();

		// Token: 0x17002041 RID: 8257
		// (get) Token: 0x060063D5 RID: 25557 RVA: 0x000370D0 File Offset: 0x000352D0
		// (set) Token: 0x060063D6 RID: 25558 RVA: 0x000370D8 File Offset: 0x000352D8
		public List<ItemTrackerData> ItemsCollected { get; private set; } = new List<ItemTrackerData>();

		// Token: 0x060063D7 RID: 25559 RVA: 0x000370E1 File Offset: 0x000352E1
		private void Awake()
		{
			this.m_onItemCollected = new Action<MonoBehaviour, EventArgs>(this.OnItemCollected);
			this.m_onChestOpened = new Action<MonoBehaviour, EventArgs>(this.OnChestOpened);
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x00037107 File Offset: 0x00035307
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ItemCollected, this.m_onItemCollected);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChestOpened, this.m_onChestOpened);
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x00037123 File Offset: 0x00035323
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ItemCollected, this.m_onItemCollected);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChestOpened, this.m_onChestOpened);
		}

		// Token: 0x060063DA RID: 25562 RVA: 0x0003713F File Offset: 0x0003533F
		public IEnumerable<IGameEventData> GetGameEvents()
		{
			foreach (ChestTrackerData chestTrackerData in this.ChestsOpened)
			{
				yield return chestTrackerData;
			}
			List<ChestTrackerData>.Enumerator enumerator = default(List<ChestTrackerData>.Enumerator);
			foreach (ItemTrackerData itemTrackerData in this.ItemsCollected)
			{
				yield return itemTrackerData;
			}
			List<ItemTrackerData>.Enumerator enumerator2 = default(List<ItemTrackerData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060063DB RID: 25563 RVA: 0x00172D00 File Offset: 0x00170F00
		private void OnChestOpened(MonoBehaviour sender, EventArgs eventArgs)
		{
			if (eventArgs is ChestOpenedEventArgs)
			{
				ChestOpenedEventArgs chestOpenedEventArgs = eventArgs as ChestOpenedEventArgs;
				bool containsGold = chestOpenedEventArgs.GoldValue > 0;
				ChestObj chest = chestOpenedEventArgs.Chest;
				ChestTrackerData item = new ChestTrackerData(chest.ChestType, containsGold, chest.Room.BiomeType, chest.Room.BiomeControllerIndex, chest.ChestIndex);
				this.ChestsOpened.Add(item);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Failed to cast event args as ChestOpenedEventArgs</color>", new object[]
			{
				this
			});
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x00172D78 File Offset: 0x00170F78
		private void OnItemCollected(MonoBehaviour sender, EventArgs eventArgs)
		{
			ItemCollectedEventArgs itemCollectedEventArgs = eventArgs as ItemCollectedEventArgs;
			if (itemCollectedEventArgs != null)
			{
				BaseItemDrop item = itemCollectedEventArgs.Item;
				int num = Economy_EV.GetItemDropValue(item.ItemDropType, false);
				if (item.ValueOverride != -1)
				{
					num = item.ValueOverride;
				}
				this.ItemsCollected.Add(new ItemTrackerData((float)num, item.ItemDropType));
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Failed to cast event args as ItemCollectedEventArgs</color>", new object[]
			{
				this
			});
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x0003714F File Offset: 0x0003534F
		public void Reset()
		{
			if (this.ChestsOpened != null)
			{
				this.ChestsOpened.Clear();
			}
			if (this.ItemsCollected != null)
			{
				this.ItemsCollected.Clear();
			}
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x00037177 File Offset: 0x00035377
		public void RestoreState(IItemEventTrackerState state)
		{
			this.ChestsOpened = state.ChestsOpened;
			this.ItemsCollected = state.ItemsCollected;
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x00037191 File Offset: 0x00035391
		public IItemEventTrackerState SaveState()
		{
			if (this.m_state == null)
			{
				this.m_state = new ItemEventTrackerState(this.ChestsOpened, this.ItemsCollected);
			}
			else
			{
				this.m_state.Initialise(this.ChestsOpened, this.ItemsCollected);
			}
			return this.m_state;
		}

		// Token: 0x04005171 RID: 20849
		private IItemEventTrackerState m_state;

		// Token: 0x04005172 RID: 20850
		private Action<MonoBehaviour, EventArgs> m_onItemCollected;

		// Token: 0x04005173 RID: 20851
		private Action<MonoBehaviour, EventArgs> m_onChestOpened;
	}
}
