using System;
using UnityEngine.SceneManagement;

// Token: 0x02000201 RID: 513
public static class GameUtility
{
	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x060015AE RID: 5550 RVA: 0x00043856 File Offset: 0x00041A56
	public static bool IsApplicationQuitting
	{
		get
		{
			return GameUtility.m_applicationQuitting;
		}
	}

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x060015AF RID: 5551 RVA: 0x00043860 File Offset: 0x00041A60
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

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x060015B0 RID: 5552 RVA: 0x000438E7 File Offset: 0x00041AE7
	public static bool IsInGame
	{
		get
		{
			return !GameUtility.IsInLevelEditor;
		}
	}

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x060015B1 RID: 5553 RVA: 0x000438F4 File Offset: 0x00041AF4
	public static bool IsInLevelEditor
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00043902 File Offset: 0x00041B02
	public static int MaxDifficulty
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x060015B3 RID: 5555 RVA: 0x00043905 File Offset: 0x00041B05
	// (set) Token: 0x060015B4 RID: 5556 RVA: 0x0004390C File Offset: 0x00041B0C
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

	// Token: 0x040014DF RID: 5343
	public const string LEVEL_EDITOR_NAME = "LevelEditor";

	// Token: 0x040014E0 RID: 5344
	public const string GAME_SCENE_NAME = "World";

	// Token: 0x040014E1 RID: 5345
	private static bool m_isSceneOpen;

	// Token: 0x040014E2 RID: 5346
	private static bool m_isInLevelEditor;

	// Token: 0x040014E3 RID: 5347
	private static bool m_isInGame;

	// Token: 0x040014E4 RID: 5348
	private static bool m_hasChecked;

	// Token: 0x040014E5 RID: 5349
	private static int m_difficulty;

	// Token: 0x040014E6 RID: 5350
	private static bool m_applicationQuitting;
}
