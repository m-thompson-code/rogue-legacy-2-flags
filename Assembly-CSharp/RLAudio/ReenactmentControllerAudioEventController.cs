using System;
using FMODUnity;
using GameEventTracking;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200090F RID: 2319
	public class ReenactmentControllerAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x00111552 File Offset: 0x0010F752
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

		// Token: 0x06004C16 RID: 19478 RVA: 0x00111578 File Offset: 0x0010F778
		private void Awake()
		{
			ReenactmentController component = base.GetComponent<ReenactmentController>();
			component.OnRoomTrackerTriggeredRelay.AddListener(new Action<RoomTrackerData>(this.OnRoomEntered), false);
			component.OnEnemyTrackerTriggeredRelay.AddListener(new Action<EnemyTrackerData>(this.OnEnemyKilled), false);
			component.OnChestTrackerTriggeredRelay.AddListener(new Action<ChestTrackerData>(this.OnChestOpened), false);
			component.OnItemTrackerTriggeredRelay.AddListener(new Action<ItemTrackerData>(this.OnGoldGain), false);
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x001115F0 File Offset: 0x0010F7F0
		private void OnDestroy()
		{
			ReenactmentController component = base.GetComponent<ReenactmentController>();
			component.OnRoomTrackerTriggeredRelay.RemoveListener(new Action<RoomTrackerData>(this.OnRoomEntered));
			component.OnEnemyTrackerTriggeredRelay.RemoveListener(new Action<EnemyTrackerData>(this.OnEnemyKilled));
			component.OnChestTrackerTriggeredRelay.RemoveListener(new Action<ChestTrackerData>(this.OnChestOpened));
			component.OnItemTrackerTriggeredRelay.RemoveListener(new Action<ItemTrackerData>(this.OnGoldGain));
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x00111664 File Offset: 0x0010F864
		private void OnGoldGain(ItemTrackerData goldData)
		{
			AudioManager.Play(this, this.m_goldGainEventPath, default(Vector3));
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x00111688 File Offset: 0x0010F888
		private void OnChestOpened(ChestTrackerData chestData)
		{
			AudioManager.Play(this, this.m_chestOpenedEventPath, default(Vector3));
			if (!chestData.ContainsGold)
			{
				AudioManager.Play(this, this.m_discoveryEventPath, default(Vector3));
			}
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x001116C8 File Offset: 0x0010F8C8
		private void OnEnemyKilled(EnemyTrackerData enemyData)
		{
			AudioManager.Play(this, this.m_enemyKilledEventPath, default(Vector3));
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x001116EC File Offset: 0x0010F8EC
		private void OnRoomEntered(RoomTrackerData roomData)
		{
			foreach (GridPointManager gridPointManager in WorldBuilder.GetBiomeController(roomData.Biome).GridPointManager.GridPointManagers)
			{
				if (gridPointManager.BiomeControllerIndex == roomData.BiomeControllerIndex)
				{
					if (RoomUtility.GetIsRoomLarge(gridPointManager.Size))
					{
						AudioManager.Play(this, this.m_largeRoomEnteredEventPath, default(Vector3));
						break;
					}
					AudioManager.Play(this, this.m_smallRoomEnteredEventPath, default(Vector3));
					break;
				}
			}
		}

		// Token: 0x0400400C RID: 16396
		[SerializeField]
		[EventRef]
		private string m_smallRoomEnteredEventPath;

		// Token: 0x0400400D RID: 16397
		[SerializeField]
		[EventRef]
		private string m_largeRoomEnteredEventPath;

		// Token: 0x0400400E RID: 16398
		[SerializeField]
		[EventRef]
		private string m_enemyKilledEventPath;

		// Token: 0x0400400F RID: 16399
		[SerializeField]
		[EventRef]
		private string m_chestOpenedEventPath;

		// Token: 0x04004010 RID: 16400
		[SerializeField]
		[EventRef]
		private string m_goldGainEventPath;

		// Token: 0x04004011 RID: 16401
		[SerializeField]
		[EventRef]
		private string m_discoveryEventPath;

		// Token: 0x04004012 RID: 16402
		private string m_description = string.Empty;
	}
}
