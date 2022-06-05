using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000CDE RID: 3294
public static class SceneLoadingUtility
{
	// Token: 0x17001EFD RID: 7933
	// (get) Token: 0x06005DEC RID: 24044 RVA: 0x00033B4C File Offset: 0x00031D4C
	public static Scene ActiveScene
	{
		get
		{
			return SceneManager.GetActiveScene();
		}
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x00033B53 File Offset: 0x00031D53
	public static string GetSceneName(SceneID sceneID)
	{
		return SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE[sceneID];
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x0015F178 File Offset: 0x0015D378
	public static SceneID GetSceneID(string sceneName)
	{
		SceneID result = SceneID.None;
		if (SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE.ContainsValue(sceneName))
		{
			result = SceneLoadingUtility.SCENE_ID_TO_SCENE_NAME_TABLE.Single((KeyValuePair<SceneID, string> entry) => entry.Value == sceneName).Key;
		}
		return result;
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x0015F1C8 File Offset: 0x0015D3C8
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

	// Token: 0x04004D2E RID: 19758
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
