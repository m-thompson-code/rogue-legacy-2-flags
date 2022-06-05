using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000EA0 RID: 3744
	[CreateAssetMenu(menuName = "Custom/Libraries/Music Library")]
	public class MusicLibrary : AudioLibrary<MusicLibraryEntry>
	{
		// Token: 0x17002192 RID: 8594
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x0003A99A File Offset: 0x00038B9A
		// (set) Token: 0x0600698E RID: 27022 RVA: 0x0003A9A1 File Offset: 0x00038BA1
		private static MusicLibrary Instance { get; set; }

		// Token: 0x17002193 RID: 8595
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x0003A9A9 File Offset: 0x00038BA9
		// (set) Token: 0x06006990 RID: 27024 RVA: 0x0003A9B0 File Offset: 0x00038BB0
		public static bool IsInitialized { get; private set; }

		// Token: 0x06006991 RID: 27025 RVA: 0x0018284C File Offset: 0x00180A4C
		public static SongID[] GetBiomeMusic(BiomeType biome)
		{
			if (MusicLibrary.m_biomeMusicLookupTable.ContainsKey(biome.ToString()))
			{
				return MusicLibrary.m_biomeMusicLookupTable[biome.ToString()].MusicTracks;
			}
			return MusicLibrary.Instance.m_default.MusicTracks;
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x0003A9B8 File Offset: 0x00038BB8
		public static string GetFMODEventPath(SongID musicID)
		{
			if (MusicLibrary.m_musicIDToFMODEventPathLookupTable.ContainsKey(musicID))
			{
				return MusicLibrary.m_musicIDToFMODEventPathLookupTable[musicID];
			}
			Debug.LogFormat("<color=red>| MusicLibrary | No entry found with key (<b>{0}</b>) in lookup table</color>", new object[]
			{
				musicID
			});
			return string.Empty;
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x0003A9F1 File Offset: 0x00038BF1
		public static void Initialize()
		{
			MusicLibrary.Instance = CDGResources.Load<MusicLibrary>("Scriptable Objects/Libraries/MusicLibrary", "", true);
			MusicLibrary.InitializeBiomeMusicLookupTable();
			MusicLibrary.InitializeMusicToFMODEventPathLookupTable();
			MusicLibrary.IsInitialized = true;
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x001828A0 File Offset: 0x00180AA0
		private static void InitializeBiomeMusicLookupTable()
		{
			MusicLibrary.m_biomeMusicLookupTable = new Dictionary<string, MusicLibraryEntry>();
			foreach (MusicLibraryEntry musicLibraryEntry in MusicLibrary.Instance.Entries)
			{
				MusicLibrary.m_biomeMusicLookupTable.Add(musicLibraryEntry.Key, musicLibraryEntry);
			}
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x0018290C File Offset: 0x00180B0C
		private static void InitializeMusicToFMODEventPathLookupTable()
		{
			MusicLibrary.m_musicIDToFMODEventPathLookupTable = new Dictionary<SongID, string>();
			foreach (MusicLookupEntry musicLookupEntry in MusicLibrary.Instance.m_lookupEntries)
			{
				MusicLibrary.m_musicIDToFMODEventPathLookupTable.Add(musicLookupEntry.ID, musicLookupEntry.EventRef);
			}
		}

		// Token: 0x040055E1 RID: 21985
		[SerializeField]
		private MusicLookupEntry[] m_lookupEntries;

		// Token: 0x040055E4 RID: 21988
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/MusicLibrary";

		// Token: 0x040055E5 RID: 21989
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/MusicLibrary.asset";

		// Token: 0x040055E6 RID: 21990
		private static Dictionary<string, MusicLibraryEntry> m_biomeMusicLookupTable;

		// Token: 0x040055E7 RID: 21991
		private static Dictionary<SongID, string> m_musicIDToFMODEventPathLookupTable;
	}
}
