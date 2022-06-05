using System;
using FMODUnity;
using GameEventTracking;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E8C RID: 3724
	public class ReenactmentControllerAudioEventController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17002179 RID: 8569
		// (get) Token: 0x06006910 RID: 26896 RVA: 0x0003A37D File Offset: 0x0003857D
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

		// Token: 0x06006911 RID: 26897 RVA: 0x0018153C File Offset: 0x0017F73C
		private void Awake()
		{
			ReenactmentController component = base.GetComponent<ReenactmentController>();
			component.OnRoomTrackerTriggeredRelay.AddListener(new Action<RoomTrackerData>(this.OnRoomEntered), false);
			component.OnEnemyTrackerTriggeredRelay.AddListener(new Action<EnemyTrackerData>(this.OnEnemyKilled), false);
			component.OnChestTrackerTriggeredRelay.AddListener(new Action<ChestTrackerData>(this.OnChestOpened), false);
			component.OnItemTrackerTriggeredRelay.AddListener(new Action<ItemTrackerData>(this.OnGoldGain), false);
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x001815B4 File Offset: 0x0017F7B4
		private void OnDestroy()
		{
			ReenactmentController component = base.GetComponent<ReenactmentController>();
			component.OnRoomTrackerTriggeredRelay.RemoveListener(new Action<RoomTrackerData>(this.OnRoomEntered));
			component.OnEnemyTrackerTriggeredRelay.RemoveListener(new Action<EnemyTrackerData>(this.OnEnemyKilled));
			component.OnChestTrackerTriggeredRelay.RemoveListener(new Action<ChestTrackerData>(this.OnChestOpened));
			component.OnItemTrackerTriggeredRelay.RemoveListener(new Action<ItemTrackerData>(this.OnGoldGain));
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x00181628 File Offset: 0x0017F828
		private void OnGoldGain(ItemTrackerData goldData)
		{
			AudioManager.Play(this, this.m_goldGainEventPath, default(Vector3));
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x0018164C File Offset: 0x0017F84C
		private void OnChestOpened(ChestTrackerData chestData)
		{
			AudioManager.Play(this, this.m_chestOpenedEventPath, default(Vector3));
			if (!chestData.ContainsGold)
			{
				AudioManager.Play(this, this.m_discoveryEventPath, default(Vector3));
			}
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x0018168C File Offset: 0x0017F88C
		private void OnEnemyKilled(EnemyTrackerData enemyData)
		{
			AudioManager.Play(this, this.m_enemyKilledEventPath, default(Vector3));
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x001816B0 File Offset: 0x0017F8B0
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

		// Token: 0x0400556C RID: 21868
		[SerializeField]
		[EventRef]
		private string m_smallRoomEnteredEventPath;

		// Token: 0x0400556D RID: 21869
		[SerializeField]
		[EventRef]
		private string m_largeRoomEnteredEventPath;

		// Token: 0x0400556E RID: 21870
		[SerializeField]
		[EventRef]
		private string m_enemyKilledEventPath;

		// Token: 0x0400556F RID: 21871
		[SerializeField]
		[EventRef]
		private string m_chestOpenedEventPath;

		// Token: 0x04005570 RID: 21872
		[SerializeField]
		[EventRef]
		private string m_goldGainEventPath;

		// Token: 0x04005571 RID: 21873
		[SerializeField]
		[EventRef]
		private string m_discoveryEventPath;

		// Token: 0x04005572 RID: 21874
		private string m_description = string.Empty;
	}
}
