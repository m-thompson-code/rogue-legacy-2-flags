using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E41 RID: 3649
	[CreateAssetMenu(menuName = "Custom/Libraries/Ambient Sound Library")]
	public class AmbientSoundLibrary : AudioLibrary<AmbientSoundLibraryEntry>
	{
		// Token: 0x170020FE RID: 8446
		// (get) Token: 0x060066F3 RID: 26355 RVA: 0x00038A75 File Offset: 0x00036C75
		private static AmbientSoundLibrary Instance
		{
			get
			{
				if (AmbientSoundLibrary.m_instance == null)
				{
					AmbientSoundLibrary.m_instance = CDGResources.Load<AmbientSoundLibrary>("Scriptable Objects/Libraries/AmbientSoundLibrary", "", true);
				}
				return AmbientSoundLibrary.m_instance;
			}
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x00038A9E File Offset: 0x00036C9E
		public static string[] GetAmbientAudioEventPaths(BiomeType biome, RoomType roomType, bool isRoomLarge)
		{
			if (roomType != RoomType.Fairy)
			{
				return AmbientSoundLibrary.GetEventPaths(AmbientSoundLibrary.Instance.GetAudioLibraryEntry(biome.ToString()), roomType, isRoomLarge);
			}
			if (isRoomLarge)
			{
				return AmbientSoundLibrary.Instance.m_fairyRoomLarge;
			}
			return AmbientSoundLibrary.Instance.m_fairyRoomSmall;
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x00038ADC File Offset: 0x00036CDC
		public static string GetAmbientAudioSnapshotPath(BiomeType biome, RoomType roomType, bool isRoomLarge)
		{
			if (roomType != RoomType.Fairy)
			{
				return AmbientSoundLibrary.GetSnapshotPath(AmbientSoundLibrary.Instance.GetAudioLibraryEntry(biome.ToString()), roomType, isRoomLarge);
			}
			if (isRoomLarge)
			{
				return AmbientSoundLibrary.Instance.m_fairyRoomLargeSnapshot;
			}
			return AmbientSoundLibrary.Instance.m_fairyRoomSmallSnapshot;
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x0017C3B8 File Offset: 0x0017A5B8
		private static string[] GetEventPaths(AmbientSoundLibraryEntry entry, RoomType roomType, bool isRoomLarge)
		{
			string[] result = new string[0];
			if (roomType == RoomType.Transition)
			{
				if (entry.TransitionRoomEntry.Source == AudioSource.UserSpecified)
				{
					result = entry.TransitionRoomEntry.EventPaths;
				}
				else if (entry.TransitionRoomEntry.Source == AudioSource.Default)
				{
					result = AmbientSoundLibrary.Instance.Default.TransitionRoomEntry.EventPaths;
				}
			}
			else if (isRoomLarge)
			{
				if (entry.LargeRoomEntry.Source == AudioSource.UserSpecified)
				{
					result = entry.LargeRoomEntry.EventPaths;
				}
				else if (entry.LargeRoomEntry.Source == AudioSource.Default)
				{
					result = AmbientSoundLibrary.Instance.Default.LargeRoomEntry.EventPaths;
				}
			}
			else if (entry.SmallRoomEntry.Source == AudioSource.UserSpecified)
			{
				result = entry.SmallRoomEntry.EventPaths;
			}
			else if (entry.SmallRoomEntry.Source == AudioSource.Default)
			{
				result = AmbientSoundLibrary.Instance.Default.SmallRoomEntry.EventPaths;
			}
			return result;
		}

		// Token: 0x060066F7 RID: 26359 RVA: 0x0017C4A0 File Offset: 0x0017A6A0
		private static string GetSnapshotPath(AmbientSoundLibraryEntry entry, RoomType roomType, bool isRoomLarge)
		{
			string result = string.Empty;
			if (roomType == RoomType.Transition)
			{
				if (entry.TransitionRoomSnapshotEntry.Source == AudioSource.UserSpecified)
				{
					result = entry.TransitionRoomSnapshotEntry.SnapshotPath;
				}
				else if (entry.TransitionRoomEntry.Source == AudioSource.Default)
				{
					result = AmbientSoundLibrary.Instance.Default.TransitionRoomSnapshot;
				}
			}
			else if (isRoomLarge)
			{
				if (entry.LargeRoomSnapshotEntry.Source == AudioSource.UserSpecified)
				{
					result = entry.LargeRoomSnapshotEntry.SnapshotPath;
				}
				else if (entry.LargeRoomSnapshotEntry.Source == AudioSource.Default)
				{
					result = AmbientSoundLibrary.Instance.Default.LargeRoomSnapshot;
				}
			}
			else if (entry.SmallRoomSnapshotEntry.Source == AudioSource.UserSpecified)
			{
				result = entry.SmallRoomSnapshotEntry.SnapshotPath;
			}
			else if (entry.SmallRoomSnapshotEntry.Source == AudioSource.Default)
			{
				result = AmbientSoundLibrary.Instance.Default.SmallRoomSnapshot;
			}
			return result;
		}

		// Token: 0x0400537D RID: 21373
		[SerializeField]
		[EventRef]
		private string[] m_fairyRoomSmall;

		// Token: 0x0400537E RID: 21374
		[SerializeField]
		[EventRef]
		private string m_fairyRoomSmallSnapshot;

		// Token: 0x0400537F RID: 21375
		[SerializeField]
		[EventRef]
		private string[] m_fairyRoomLarge;

		// Token: 0x04005380 RID: 21376
		[SerializeField]
		[EventRef]
		private string m_fairyRoomLargeSnapshot;

		// Token: 0x04005381 RID: 21377
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AmbientSoundLibrary";

		// Token: 0x04005382 RID: 21378
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AmbientSoundLibrary.asset";

		// Token: 0x04005383 RID: 21379
		private static AmbientSoundLibrary m_instance;
	}
}
