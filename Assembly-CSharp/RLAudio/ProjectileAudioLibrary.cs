using System;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000909 RID: 2313
	[CreateAssetMenu(menuName = "Custom/Libraries/Projectile Audio Library")]
	public class ProjectileAudioLibrary : AudioLibrary<ProjectileAudioLibraryEntry>
	{
		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x00110C05 File Offset: 0x0010EE05
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

		// Token: 0x06004BDB RID: 19419 RVA: 0x00110C2E File Offset: 0x0010EE2E
		public static ProjectileAudioLibraryEntry GetEntry(string projectileName)
		{
			return ProjectileAudioLibrary.Instance.GetAudioLibraryEntry(projectileName);
		}

		// Token: 0x06004BDC RID: 19420 RVA: 0x00110C3B File Offset: 0x0010EE3B
		public static ProjectileAudioLibraryEntry GetDefaultEntry()
		{
			return ProjectileAudioLibrary.Instance.Default;
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x00110C47 File Offset: 0x0010EE47
		public static bool GetContainsProjectileEntry(string projectileName)
		{
			return ProjectileAudioLibrary.Instance.GetContainsEntry(projectileName);
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x00110C54 File Offset: 0x0010EE54
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

		// Token: 0x06004BDF RID: 19423 RVA: 0x00110C84 File Offset: 0x0010EE84
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

		// Token: 0x04003FE7 RID: 16359
		private static ProjectileAudioLibrary m_instance;

		// Token: 0x04003FE8 RID: 16360
		public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ProjectileAudioLibrary";

		// Token: 0x04003FE9 RID: 16361
		public const string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/ProjectileAudioLibrary.asset";
	}
}
