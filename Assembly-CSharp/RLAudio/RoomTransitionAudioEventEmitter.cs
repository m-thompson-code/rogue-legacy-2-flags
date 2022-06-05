using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E8E RID: 3726
	public class RoomTransitionAudioEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700217B RID: 8571
		// (get) Token: 0x0600691B RID: 26907 RVA: 0x0003A3C7 File Offset: 0x000385C7
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

		// Token: 0x0600691C RID: 26908 RVA: 0x0003A3E8 File Offset: 0x000385E8
		private void Awake()
		{
			this.m_onPlayerEnterRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterRoom);
		}

		// Token: 0x0600691D RID: 26909 RVA: 0x0003A3FC File Offset: 0x000385FC
		private void Start()
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x0003A40A File Offset: 0x0003860A
		private void OnDestroy()
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
		}

		// Token: 0x0600691F RID: 26911 RVA: 0x00181758 File Offset: 0x0017F958
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

		// Token: 0x06006920 RID: 26912 RVA: 0x00181838 File Offset: 0x0017FA38
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

		// Token: 0x04005574 RID: 21876
		[SerializeField]
		[EventRef]
		private string m_enterLeftTransitionPath;

		// Token: 0x04005575 RID: 21877
		[SerializeField]
		[EventRef]
		private string m_enterRightTransitionPath;

		// Token: 0x04005576 RID: 21878
		[SerializeField]
		[EventRef]
		private string m_enterCenterTransitionPath;

		// Token: 0x04005577 RID: 21879
		[SerializeField]
		[EventRef]
		private string m_bossEntranceRoomDiscoveredPath;

		// Token: 0x04005578 RID: 21880
		[SerializeField]
		[EventRef]
		private string m_enterTransitionRoomPath;

		// Token: 0x04005579 RID: 21881
		private string m_description;

		// Token: 0x0400557A RID: 21882
		private List<BiomeType> m_hasBossRoomBeenDiscovered = new List<BiomeType>();

		// Token: 0x0400557B RID: 21883
		private List<BiomeType> m_hasTransitionRoomBeenDiscovered = new List<BiomeType>();

		// Token: 0x0400557C RID: 21884
		private Action<MonoBehaviour, EventArgs> m_onPlayerEnterRoom;
	}
}
