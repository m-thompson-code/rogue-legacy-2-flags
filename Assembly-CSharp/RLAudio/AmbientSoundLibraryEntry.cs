using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E45 RID: 3653
	[Serializable]
	public class AmbientSoundLibraryEntry : AudioLibraryEntry
	{
		// Token: 0x17002103 RID: 8451
		// (get) Token: 0x06006700 RID: 26368 RVA: 0x00038B42 File Offset: 0x00036D42
		public string[] SmallRoom
		{
			get
			{
				return this.m_smallRoom;
			}
		}

		// Token: 0x17002104 RID: 8452
		// (get) Token: 0x06006701 RID: 26369 RVA: 0x00038B4A File Offset: 0x00036D4A
		public string[] LargeRoom
		{
			get
			{
				return this.m_largeRoom;
			}
		}

		// Token: 0x17002105 RID: 8453
		// (get) Token: 0x06006702 RID: 26370 RVA: 0x00038B52 File Offset: 0x00036D52
		public string[] TransitionRoom
		{
			get
			{
				return this.m_transitionRoom;
			}
		}

		// Token: 0x17002106 RID: 8454
		// (get) Token: 0x06006703 RID: 26371 RVA: 0x00038B5A File Offset: 0x00036D5A
		public string SmallRoomSnapshot
		{
			get
			{
				return this.m_smallRoomSnapshot;
			}
		}

		// Token: 0x17002107 RID: 8455
		// (get) Token: 0x06006704 RID: 26372 RVA: 0x00038B62 File Offset: 0x00036D62
		public string LargeRoomSnapshot
		{
			get
			{
				return this.m_largeRoomSnapshot;
			}
		}

		// Token: 0x17002108 RID: 8456
		// (get) Token: 0x06006705 RID: 26373 RVA: 0x00038B6A File Offset: 0x00036D6A
		public string TransitionRoomSnapshot
		{
			get
			{
				return this.m_transitionRoomSnapshot;
			}
		}

		// Token: 0x17002109 RID: 8457
		// (get) Token: 0x06006706 RID: 26374 RVA: 0x00038B72 File Offset: 0x00036D72
		public AudioLibraryEventsEntry SmallRoomEntry
		{
			get
			{
				return this.m_smallRoomEntry;
			}
		}

		// Token: 0x1700210A RID: 8458
		// (get) Token: 0x06006707 RID: 26375 RVA: 0x00038B7A File Offset: 0x00036D7A
		public AudioLibraryEventsEntry LargeRoomEntry
		{
			get
			{
				return this.m_largeRoomEntry;
			}
		}

		// Token: 0x1700210B RID: 8459
		// (get) Token: 0x06006708 RID: 26376 RVA: 0x00038B82 File Offset: 0x00036D82
		public AudioLibraryEventsEntry TransitionRoomEntry
		{
			get
			{
				return this.m_transitionRoomEntry;
			}
		}

		// Token: 0x1700210C RID: 8460
		// (get) Token: 0x06006709 RID: 26377 RVA: 0x00038B8A File Offset: 0x00036D8A
		public AudioLibrarySnapshotEntry SmallRoomSnapshotEntry
		{
			get
			{
				return this.m_smallRoomSnapshotEntry;
			}
		}

		// Token: 0x1700210D RID: 8461
		// (get) Token: 0x0600670A RID: 26378 RVA: 0x00038B92 File Offset: 0x00036D92
		public AudioLibrarySnapshotEntry LargeRoomSnapshotEntry
		{
			get
			{
				return this.m_largeRoomSnapshotEntry;
			}
		}

		// Token: 0x1700210E RID: 8462
		// (get) Token: 0x0600670B RID: 26379 RVA: 0x00038B9A File Offset: 0x00036D9A
		public AudioLibrarySnapshotEntry TransitionRoomSnapshotEntry
		{
			get
			{
				return this.m_transitionRoomSnapshotEntry;
			}
		}

		// Token: 0x0400538C RID: 21388
		[SerializeField]
		[EventRef]
		private string[] m_smallRoom;

		// Token: 0x0400538D RID: 21389
		[SerializeField]
		private AudioLibraryEventsEntry m_smallRoomEntry;

		// Token: 0x0400538E RID: 21390
		[SerializeField]
		[EventRef]
		private string[] m_largeRoom;

		// Token: 0x0400538F RID: 21391
		[SerializeField]
		private AudioLibraryEventsEntry m_largeRoomEntry;

		// Token: 0x04005390 RID: 21392
		[SerializeField]
		[EventRef]
		private string[] m_transitionRoom;

		// Token: 0x04005391 RID: 21393
		[SerializeField]
		private AudioLibraryEventsEntry m_transitionRoomEntry;

		// Token: 0x04005392 RID: 21394
		[SerializeField]
		[EventRef]
		private string m_smallRoomSnapshot;

		// Token: 0x04005393 RID: 21395
		[SerializeField]
		private AudioLibrarySnapshotEntry m_smallRoomSnapshotEntry;

		// Token: 0x04005394 RID: 21396
		[SerializeField]
		[EventRef]
		private string m_largeRoomSnapshot;

		// Token: 0x04005395 RID: 21397
		[SerializeField]
		private AudioLibrarySnapshotEntry m_largeRoomSnapshotEntry;

		// Token: 0x04005396 RID: 21398
		[SerializeField]
		[EventRef]
		private string m_transitionRoomSnapshot;

		// Token: 0x04005397 RID: 21399
		[SerializeField]
		private AudioLibrarySnapshotEntry m_transitionRoomSnapshotEntry;
	}
}
