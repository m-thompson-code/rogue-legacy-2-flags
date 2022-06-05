using System;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x020008D3 RID: 2259
	[CreateAssetMenu(menuName = "Custom/Libraries/Ambient Sound Library")]
	public class AmbientSoundLibrary : AudioLibrary<AmbientSoundLibraryEntry>
	{
		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06004A43 RID: 19011 RVA: 0x0010B91B File Offset: 0x00109B1B
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

		// Token: 0x06004A44 RID: 19012 RVA: 0x0010B944 File Offset: 0x00109B44
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

		// Token: 0x06004A45 RID: 19013 RVA: 0x0010B982 File Offset: 0x00109B82
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

		// Token: 0x06004A46 RID: 19014 RVA: 0x0010B9C0 File Offset: 0x00109BC0
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

		// Token: 0x06004A47 RID: 19015 RVA: 0x0010BAA8 File Offset: 0x00109CA8
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

		// Token: 0x04003E68 RID: 15976
		[SerializeField]
		[EventRef]
		private string[] m_fairyRoomSmall;

		// Token: 0x04003E69 RID: 15977
		[SerializeField]
		[EventRef]
		private string m_fairyRoomSmallSnapshot;

		// Token: 0x04003E6A RID: 15978
		[SerializeField]
		[EventRef]
		private string[] m_fairyRoomLarge;

		// Token: 0x04003E6B RID: 15979
		[SerializeField]
		[EventRef]
		private string m_fairyRoomLargeSnapshot;

		// Token: 0x04003E6C RID: 15980
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AmbientSoundLibrary";

		// Token: 0x04003E6D RID: 15981
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/AmbientSoundLibrary.asset";

		// Token: 0x04003E6E RID: 15982
		private static AmbientSoundLibrary m_instance;
	}
}
