using System;
using UnityEngine.SceneManagement;

// Token: 0x020003AD RID: 941
public static class GameUtility
{
	// Token: 0x17000E0F RID: 3599
	// (get) Token: 0x06001F22 RID: 7970 RVA: 0x00010595 File Offset: 0x0000E795
	public static bool IsApplicationQuitting
	{
		get
		{
			return GameUtility.m_applicationQuitting;
		}
	}

	// Token: 0x17000E10 RID: 3600
	// (get) Token: 0x06001F23 RID: 7971 RVA: 0x000A1E58 File Offset: 0x000A0058
	public static bool SceneHasRooms
	{
		get
		{
			if (GameUtility.IsInLevelEditor)
			{
				return true;
			}
			Scene activeScene = SceneLoadingUtility.ActiveScene;
			return activeScene.name == SceneLoadingUtility.GetSceneName(SceneID.World) || activeScene.name == SceneLoadingUtility.GetSceneName(SceneID.Town) || activeScene.name == SceneLoadingUtility.GetSceneName(SceneID.Tutorial) || activeScene.name == SceneLoadingUtility.GetSceneName(SceneID.Lineage) || activeScene.name == SceneLoadingUtility.GetSceneName(SceneID.Parade);
		}
	}

	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0001059C File Offset: 0x0000E79C
	public static bool IsInGame
	{
		get
		{
			return !GameUtility.IsInLevelEditor;
		}
	}

	// Token: 0x17000E12 RID: 3602
	// (get) Token: 0x06001F25 RID: 7973 RVA: 0x000A1EE0 File Offset: 0x000A00E0
	public static bool IsInLevelEditor
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000E13 RID: 3603
	// (get) Token: 0x06001F26 RID: 7974 RVA: 0x00004762 File Offset: 0x00002962
	public static int MaxDifficulty
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x06001F27 RID: 7975 RVA: 0x000105A6 File Offset: 0x0000E7A6
	// (set) Token: 0x06001F28 RID: 7976 RVA: 0x000105AD File Offset: 0x0000E7AD
	public static int Difficulty
	{
		get
		{
			return GameUtility.m_difficulty;
		}
		set
		{
			GameUtility.m_difficulty = value;
			if (GameUtility.m_difficulty > GameUtility.MaxDifficulty)
			{
				GameUtility.m_difficulty = 0;
			}
		}
	}

	// Token: 0x04001BC6 RID: 7110
	public const string LEVEL_EDITOR_NAME = "LevelEditor";

	// Token: 0x04001BC7 RID: 7111
	public const string GAME_SCENE_NAME = "World";

	// Token: 0x04001BC8 RID: 7112
	private static bool m_isSceneOpen;

	// Token: 0x04001BC9 RID: 7113
	private static bool m_isInLevelEditor;

	// Token: 0x04001BCA RID: 7114
	private static bool m_isInGame;

	// Token: 0x04001BCB RID: 7115
	private static bool m_hasChecked;

	// Token: 0x04001BCC RID: 7116
	private static int m_difficulty;

	// Token: 0x04001BCD RID: 7117
	private static bool m_applicationQuitting;
}
