using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E86 RID: 3718
	[CreateAssetMenu(menuName = "Custom/Libraries/Projectile Audio Library")]
	public class ProjectileAudioLibrary : AudioLibrary<ProjectileAudioLibraryEntry>
	{
		// Token: 0x17002164 RID: 8548
		// (get) Token: 0x060068D5 RID: 26837 RVA: 0x0003A11C File Offset: 0x0003831C
		private static ProjectileAudioLibrary Instance
		{
			get
			{
				if (ProjectileAudioLibrary.m_instance == null)
				{
					ProjectileAudioLibrary.m_instance = CDGResources.Load<ProjectileAudioLibrary>("Scriptable Objects/Libraries/ProjectileAudioLibrary", "", true);
				}
				return ProjectileAudioLibrary.m_instance;
			}
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x0003A145 File Offset: 0x00038345
		public static ProjectileAudioLibraryEntry GetEntry(string projectileName)
		{
			return ProjectileAudioLibrary.Instance.GetAudioLibraryEntry(projectileName);
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x0003A152 File Offset: 0x00038352
		public static ProjectileAudioLibraryEntry GetDefaultEntry()
		{
			return ProjectileAudioLibrary.Instance.Default;
		}

		// Token: 0x060068D8 RID: 26840 RVA: 0x0003A15E File Offset: 0x0003835E
		public static bool GetContainsProjectileEntry(string projectileName)
		{
			return ProjectileAudioLibrary.Instance.GetContainsEntry(projectileName);
		}

		// Token: 0x060068D9 RID: 26841 RVA: 0x00180E64 File Offset: 0x0017F064
		public static string GetSingleShotAudioPath(string projectileName)
		{
			ProjectileAudioLibraryEntry entry = ProjectileAudioLibrary.GetEntry(projectileName);
			ProjectileAudioLibraryEntry defaultEntry = ProjectileAudioLibrary.GetDefaultEntry();
			string spawnSingleEventPath = entry.SpawnSingleEventPath;
			if (string.IsNullOrEmpty(spawnSingleEventPath))
			{
				spawnSingleEventPath = defaultEntry.SpawnSingleEventPath;
			}
			return spawnSingleEventPath;
		}

		// Token: 0x060068DA RID: 26842 RVA: 0x00180E94 File Offset: 0x0017F094
		public static string GetMultiShotAudioPath(string projectileName)
		{
			ProjectileAudioLibraryEntry entry = ProjectileAudioLibrary.GetEntry(projectileName);
			ProjectileAudioLibraryEntry defaultEntry = ProjectileAudioLibrary.GetDefaultEntry();
			string spawnManyEventPath = entry.SpawnManyEventPath;
			if (string.IsNullOrEmpty(spawnManyEventPath))
			{
				spawnManyEventPath = defaultEntry.SpawnManyEventPath;
			}
			return spawnManyEventPath;
		}

		// Token: 0x04005547 RID: 21831
		private static ProjectileAudioLibrary m_instance;

		// Token: 0x04005548 RID: 21832
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ProjectileAudioLibrary";

		// Token: 0x04005549 RID: 21833
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ProjectileAudioLibrary.asset";
	}
}
