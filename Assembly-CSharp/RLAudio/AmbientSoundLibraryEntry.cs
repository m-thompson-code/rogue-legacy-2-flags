using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D7 RID: 2263
	[Serializable]
	public class AmbientSoundLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0010BBB6 File Offset: 0x00109DB6
		public string[] SmallRoom
		{
			get
			{
				return this.m_smallRoom;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x0010BBBE File Offset: 0x00109DBE
		public string[] LargeRoom
		{
			get
			{
				return this.m_largeRoom;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06004A52 RID: 19026 RVA: 0x0010BBC6 File Offset: 0x00109DC6
		public string[] TransitionRoom
		{
			get
			{
				return this.m_transitionRoom;
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x06004A53 RID: 19027 RVA: 0x0010BBCE File Offset: 0x00109DCE
		public string SmallRoomSnapshot
		{
			get
			{
				return this.m_smallRoomSnapshot;
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06004A54 RID: 19028 RVA: 0x0010BBD6 File Offset: 0x00109DD6
		public string LargeRoomSnapshot
		{
			get
			{
				return this.m_largeRoomSnapshot;
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x0010BBDE File Offset: 0x00109DDE
		public string TransitionRoomSnapshot
		{
			get
			{
				return this.m_transitionRoomSnapshot;
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06004A56 RID: 19030 RVA: 0x0010BBE6 File Offset: 0x00109DE6
		public AudioLibraryEventsEntry SmallRoomEntry
		{
			get
			{
				return this.m_smallRoomEntry;
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x0010BBEE File Offset: 0x00109DEE
		public AudioLibraryEventsEntry LargeRoomEntry
		{
			get
			{
				return this.m_largeRoomEntry;
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06004A58 RID: 19032 RVA: 0x0010BBF6 File Offset: 0x00109DF6
		public AudioLibraryEventsEntry TransitionRoomEntry
		{
			get
			{
				return this.m_transitionRoomEntry;
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x0010BBFE File Offset: 0x00109DFE
		public AudioLibrarySnapshotEntry SmallRoomSnapshotEntry
		{
			get
			{
				return this.m_smallRoomSnapshotEntry;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06004A5A RID: 19034 RVA: 0x0010BC06 File Offset: 0x00109E06
		public AudioLibrarySnapshotEntry LargeRoomSnapshotEntry
		{
			get
			{
				return this.m_largeRoomSnapshotEntry;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06004A5B RID: 19035 RVA: 0x0010BC0E File Offset: 0x00109E0E
		public AudioLibrarySnapshotEntry TransitionRoomSnapshotEntry
		{
			get
			{
				return this.m_transitionRoomSnapshotEntry;
			}
		}

		// Token: 0x04003E77 RID: 15991
		[SerializeField]
		[EventRef]
		private string[] m_smallRoom;

		// Token: 0x04003E78 RID: 15992
		[SerializeField]
		private AudioLibraryEventsEntry m_smallRoomEntry;

		// Token: 0x04003E79 RID: 15993
		[SerializeField]
		[EventRef]
		private string[] m_largeRoom;

		// Token: 0x04003E7A RID: 15994
		[SerializeField]
		private AudioLibraryEventsEntry m_largeRoomEntry;

		// Token: 0x04003E7B RID: 15995
		[SerializeField]
		[EventRef]
		private string[] m_transitionRoom;

		// Token: 0x04003E7C RID: 15996
		[SerializeField]
		private AudioLibraryEventsEntry m_transitionRoomEntry;

		// Token: 0x04003E7D RID: 15997
		[SerializeField]
		[EventRef]
		private string m_smallRoomSnapshot;

		// Token: 0x04003E7E RID: 15998
		[SerializeField]
		private AudioLibrarySnapshotEntry m_smallRoomSnapshotEntry;

		// Token: 0x04003E7F RID: 15999
		[SerializeField]
		[EventRef]
		private string m_largeRoomSnapshot;

		// Token: 0x04003E80 RID: 16000
		[SerializeField]
		private AudioLibrarySnapshotEntry m_largeRoomSnapshotEntry;

		// Token: 0x04003E81 RID: 16001
		[SerializeField]
		[EventRef]
		private string m_transitionRoomSnapshot;

		// Token: 0x04003E82 RID: 16002
		[SerializeField]
		private AudioLibrarySnapshotEntry m_transitionRoomSnapshotEntry;
	}
}
