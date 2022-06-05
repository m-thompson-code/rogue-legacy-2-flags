using System;
using System.Collections.Generic;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000921 RID: 2337
	[CreateAssetMenu(menuName = "Custom/Libraries/Music Library")]
	public class MusicLibrary : AudioLibrary<MusicLibraryEntry>
	{
		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06004C86 RID: 19590 RVA: 0x00112CBA File Offset: 0x00110EBA
		// (set) Token: 0x06004C87 RID: 19591 RVA: 0x00112CC1 File Offset: 0x00110EC1
		private static MusicLibrary Instance { get; set; }

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06004C88 RID: 19592 RVA: 0x00112CC9 File Offset: 0x00110EC9
		// (set) Token: 0x06004C89 RID: 19593 RVA: 0x00112CD0 File Offset: 0x00110ED0
		public static bool IsInitialized { get; private set; }

		// Token: 0x06004C8A RID: 19594 RVA: 0x00112CD8 File Offset: 0x00110ED8
		public static SongID[] GetBiomeMusic(BiomeType biome)
		{
			if (MusicLibrary.m_biomeMusicLookupTable.ContainsKey(biome.ToString()))
			{
				return MusicLibrary.m_biomeMusicLookupTable[biome.ToString()].MusicTracks;
			}
			return MusicLibrary.Instance.m_default.MusicTracks;
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x00112D2A File Offset: 0x00110F2A
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

		// Token: 0x06004C8C RID: 19596 RVA: 0x00112D63 File Offset: 0x00110F63
		public static void Initialize()
		{
			MusicLibrary.Instance = CDGResources.Load<MusicLibrary>("Scriptable Objects/Libraries/MusicLibrary", "", true);
			MusicLibrary.InitializeBiomeMusicLookupTable();
			MusicLibrary.InitializeMusicToFMODEventPathLookupTable();
			MusicLibrary.IsInitialized = true;
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x00112D8C File Offset: 0x00110F8C
		private static void InitializeBiomeMusicLookupTable()
		{
			MusicLibrary.m_biomeMusicLookupTable = new Dictionary<string, MusicLibraryEntry>();
			foreach (MusicLibraryEntry musicLibraryEntry in MusicLibrary.Instance.Entries)
			{
				MusicLibrary.m_biomeMusicLookupTable.Add(musicLibraryEntry.Key, musicLibraryEntry);
			}
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x00112DF8 File Offset: 0x00110FF8
		private static void InitializeMusicToFMODEventPathLookupTable()
		{
			MusicLibrary.m_musicIDToFMODEventPathLookupTable = new Dictionary<SongID, string>();
			foreach (MusicLookupEntry musicLookupEntry in MusicLibrary.Instance.m_lookupEntries)
			{
				MusicLibrary.m_musicIDToFMODEventPathLookupTable.Add(musicLookupEntry.ID, musicLookupEntry.EventRef);
			}
		}

		// Token: 0x04004075 RID: 16501
		[SerializeField]
		private MusicLookupEntry[] m_lookupEntries;

		// Token: 0x04004078 RID: 16504
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/MusicLibrary";

		// Token: 0x04004079 RID: 16505
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/MusicLibrary.asset";

		// Token: 0x0400407A RID: 16506
		private static Dictionary<string, MusicLibraryEntry> m_biomeMusicLookupTable;

		// Token: 0x0400407B RID: 16507
		private static Dictionary<SongID, string> m_musicIDToFMODEventPathLookupTable;
	}
}
