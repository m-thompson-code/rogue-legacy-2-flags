using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200080F RID: 2063
public static class SceneLoadingUtility
{
	// Token: 0x170016F7 RID: 5879
	// (get) Token: 0x0600443A RID: 17466 RVA: 0x000F15E2 File Offset: 0x000EF7E2
	public static Scene ActiveScene
	{
		get
		{
			return SceneManager.GetActiveScene();
		}
	}

	// Token: 0x0600443B RID: 17467 RVA: 0x000F15E9 File Offset: 0x000EF7E9
	public static string GetSceneName(SceneID sceneID)
	{
		return SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE[sceneID];
	}

	// Token: 0x0600443C RID: 17468 RVA: 0x000F15F8 File Offset: 0x000EF7F8
	public static SceneID GetSceneID(string sceneName)
	{
		SceneID result = SceneID.None;
		if (SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE.ContainsValue(sceneName))
		{
			result = SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE.Single((KeyValuePair<SceneID, string> entry) => entry.Value == sceneName).Key;
		}
		return result;
	}

	// Token: 0x0600443D RID: 17469 RVA: 0x000F1648 File Offset: 0x000EF848
	public static void LoadScene(SceneID sceneID)
	{
		if (SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE.ContainsKey(sceneID))
		{
			RLTimeScale.Reset();
			SceneManager.LoadScene(SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE[sceneID]);
			return;
		}
		Debug.LogFormat("<color=red>[{0}] Table does not contain an entry with Key ({1})</color>", new object[]
		{
			"SceneLoadingUtility",
			sceneID
		});
	}

	// Token: 0x04003A49 RID: 14921
	private static Dictionary<SceneID, string> SCENE_ID_TO_SCENE_NAME_TABLE = new Dictionary<SceneID, string>
	{
		{
			SceneID.Lineage,
			"Lineage"
		},
		{
			SceneID.MainMenu,
			"MainMenu"
		},
		{
			SceneID.Splash,
			"Splash"
		},
		{
			SceneID.Town,
			"Town"
		},
		{
			SceneID.World,
			"World"
		},
		{
			SceneID.SkillTree,
			"SkillTree"
		},
		{
			SceneID.Disclaimer,
			"Disclaimer"
		},
		{
			SceneID.Pause,
			"Pause"
		},
		{
			SceneID.Tutorial,
			"Tutorial"
		},
		{
			SceneID.Parade,
			"Parade"
		},
		{
			SceneID.Credits,
			"Credits"
		},
		{
			SceneID.Testing,
			"UserReport"
		}
	};
}
