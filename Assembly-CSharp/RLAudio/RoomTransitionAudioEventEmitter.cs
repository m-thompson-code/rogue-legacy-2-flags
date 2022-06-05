using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000911 RID: 2321
	public class RoomTransitionAudioEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x001117C0 File Offset: 0x0010F9C0
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_description))
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x001117E1 File Offset: 0x0010F9E1
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x001117F5 File Offset: 0x0010F9F5
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x00111803 File Offset: 0x0010FA03
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x00111814 File Offset: 0x0010FA14
		private void OnPlayerEnterRoom(MonoBehaviour sender, EventArgs eventArgs)
		{
			RoomViaDoorEventArgs roomViaDoorEventArgs = eventArgs as RoomViaDoorEventArgs;
			if (roomViaDoorEventArgs != null)
			{
				if (roomViaDoorEventArgs.ViaDoor != null)
				{
					this.PlayTransition(roomViaDoorEventArgs.ViaDoor.Side);
				}
				if (roomViaDoorEventArgs.Room.RoomType == RoomType.BossEntrance)
				{
					if (!this.m_hasBossRoomBeenDiscovered.Contains(roomViaDoorEventArgs.Room.AppearanceBiomeType))
					{
						this.m_hasBossRoomBeenDiscovered.Add(roomViaDoorEventArgs.Room.AppearanceBiomeType);
						AudioManager.PlayOneShot(this, this.m_bossEntranceRoomDiscoveredPath, default(Vector3));
						return;
					}
				}
				else if (roomViaDoorEventArgs.Room.RoomType == RoomType.Transition && !this.m_hasTransitionRoomBeenDiscovered.Contains(roomViaDoorEventArgs.Room.AppearanceBiomeType))
				{
					this.m_hasTransitionRoomBeenDiscovered.Add(roomViaDoorEventArgs.Room.AppearanceBiomeType);
					AudioManager.PlayOneShot(this, this.m_enterTransitionRoomPath, default(Vector3));
				}
			}
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x001118F4 File Offset: 0x0010FAF4
		private void PlayTransition(RoomSide side)
		{
			switch (side)
			{
			case RoomSide.Top:
			case RoomSide.Bottom:
				AudioManager.PlayOneShot(this, this.m_enterCenterTransitionPath, default(Vector3));
				return;
			case RoomSide.Left:
				AudioManager.PlayOneShot(this, this.m_enterLeftTransitionPath, default(Vector3));
				return;
			case RoomSide.Right:
				AudioManager.PlayOneShot(this, this.m_enterRightTransitionPath, default(Vector3));
				return;
			default:
				return;
			}
		}

		// Token: 0x04004014 RID: 16404
		[SerializeField]
		[EventRef]
		private string m_enterLeftTransitionPath;

		// Token: 0x04004015 RID: 16405
		[SerializeField]
		[EventRef]
		private string m_enterRightTransitionPath;

		// Token: 0x04004016 RID: 16406
		[SerializeField]
		[EventRef]
		private string m_enterCenterTransitionPath;

		// Token: 0x04004017 RID: 16407
		[SerializeField]
		[EventRef]
		private string m_bossEntranceRoomDiscoveredPath;

		// Token: 0x04004018 RID: 16408
		[SerializeField]
		[EventRef]
		private string m_enterTransitionRoomPath;

		// Token: 0x04004019 RID: 16409
		private string m_description;

		// Token: 0x0400401A RID: 16410
		private List<BiomeType> m_hasBossRoomBeenDiscovered = new List<BiomeType>();

		// Token: 0x0400401B RID: 16411
		private List<BiomeType> m_hasTransitionRoomBeenDiscovered = new List<BiomeType>();

		// Token: 0x0400401C RID: 16412
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
	}
}
