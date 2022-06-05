using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEventTracking
{
	// Token: 0x020008AE RID: 2222
	public class ItemEventTracker : MonoBehaviour, IGameEventTracker<IItemEventTrackerState>
	{
		// Token: 0x170017C6 RID: 6086
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x001041D7 File Offset: 0x001023D7
		// (set) Token: 0x06004874 RID: 18548 RVA: 0x001041DF File Offset: 0x001023DF
		public List<ChestTrackerData> ChestsOpened { get; private set; } = new List<ChestTrackerData>();

		// Token: 0x170017C7 RID: 6087
		// (get) Token: 0x06004875 RID: 18549 RVA: 0x001041E8 File Offset: 0x001023E8
		// (set) Token: 0x06004876 RID: 18550 RVA: 0x001041F0 File Offset: 0x001023F0
		public List<ItemTrackerData> ItemsCollected { get; private set; } = new List<ItemTrackerData>();

		// Token: 0x06004877 RID: 18551 RVA: 0x001041F9 File Offset: 0x001023F9
		private void Awake()
		{
			this.m_onItemCollected = new Action<MonoBehaviour, EventArgs>(this.OnItemCollected);
			this.m_onChestOpened = new Action<MonoBehaviour, EventArgs>(this.OnChestOpened);
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x0010421F File Offset: 0x0010241F
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ItemCollected, this.m_onItemCollected);
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.ChestOpened, this.m_onChestOpened);
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x0010423B File Offset: 0x0010243B
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ItemCollected, this.m_onItemCollected);
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.ChestOpened, this.m_onChestOpened);
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x00104257 File Offset: 0x00102457
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

		// Token: 0x0600487B RID: 18555 RVA: 0x00104268 File Offset: 0x00102468
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

		// Token: 0x0600487C RID: 18556 RVA: 0x001042E0 File Offset: 0x001024E0
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

		// Token: 0x0600487D RID: 18557 RVA: 0x00104348 File Offset: 0x00102548
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

		// Token: 0x0600487E RID: 18558 RVA: 0x00104370 File Offset: 0x00102570
		public void RestoreState(IItemEventTrackerState state)
		{
			this.ChestsOpened = state.ChestsOpened;
			this.ItemsCollected = state.ItemsCollected;
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x0010438A File Offset: 0x0010258A
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

		// Token: 0x04003D2A RID: 15658
		private IItemEventTrackerState m_state;

		// Token: 0x04003D2B RID: 15659
		private Action<MonoBehaviour, EventArgs> m_onItemCollected;

		// Token: 0x04003D2C RID: 15660
		private Action<MonoBehaviour, EventArgs> m_onChestOpened;
	}
}
