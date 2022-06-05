using System;
using System.Collections.Generic;
using System.Globalization;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x020006E2 RID: 1762
[CreateAssetMenu(menuName = "Custom/Data/RNG Seed Manager")]
public class RNGSeedManager : ScriptableObject
{
	// Token: 0x170015D9 RID: 5593
	// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x000E2BD8 File Offset: 0x000E0DD8
	// (set) Token: 0x06003FF2 RID: 16370 RVA: 0x000E2BDF File Offset: 0x000E0DDF
	public static int MasterSeedOverride
	{
		get
		{
			return RNGSeedManager.m_masterSeedOverride;
		}
		private set
		{
			if (RNGSeedManager.PreviousMasterSeedOverrides.Count == 5)
			{
				RNGSeedManager.PreviousMasterSeedOverrides.Dequeue();
			}
			RNGSeedManager.PreviousMasterSeedOverrides.Enqueue(value);
			RNGSeedManager.m_masterSeedOverride = value;
		}
	}

	// Token: 0x170015DA RID: 5594
	// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x000E2C0A File Offset: 0x000E0E0A
	public static string CurrentWorldSeed
	{
		get
		{
			return RNGSeedManager.GetSeedAsHex(RNGSeedManager.GetCurrentSeed("World"));
		}
	}

	// Token: 0x170015DB RID: 5595
	// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x000E2C1B File Offset: 0x000E0E1B
	private static RNGSeedManager Instance
	{
		get
		{
			if (RNGSeedManager.m_instance == null && Application.isPlaying)
			{
				RNGSeedManager.m_instance = CDGResources.Load<RNGSeedManager>("Scriptable Objects/RNGSeedManager", "", true);
			}
			return RNGSeedManager.m_instance;
		}
	}

	// Token: 0x170015DC RID: 5596
	// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x000E2C4B File Offset: 0x000E0E4B
	public static bool IsMasterSeedOverrideSet
	{
		get
		{
			return RNGSeedManager.MasterSeedOverride != -1;
		}
	}

	// Token: 0x170015DD RID: 5597
	// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x000E2C58 File Offset: 0x000E0E58
	public static bool IsOverrideSet
	{
		get
		{
			return (Application.isEditor && RNGSeedManager.IsTempSeedSet) || RNGSeedManager.IsProjectSeedSet || RNGSeedManager.IsMasterSeedOverrideSet;
		}
	}

	// Token: 0x170015DE RID: 5598
	// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x000E2C78 File Offset: 0x000E0E78
	public static bool IsProjectSeedSet
	{
		get
		{
			return !string.IsNullOrEmpty(RNGSeedManager.ProjectSeed);
		}
	}

	// Token: 0x170015DF RID: 5599
	// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x000E2C87 File Offset: 0x000E0E87
	public static bool IsTempSeedSet
	{
		get
		{
			return Application.isEditor && !string.IsNullOrEmpty(RNGSeedManager.TempSeed);
		}
	}

	// Token: 0x170015E0 RID: 5600
	// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x000E2C9F File Offset: 0x000E0E9F
	// (set) Token: 0x06003FFA RID: 16378 RVA: 0x000E2CAB File Offset: 0x000E0EAB
	public static string ProjectSeed
	{
		get
		{
			return RNGSeedManager.Instance.m_projectSeed;
		}
		private set
		{
			RNGSeedManager.Instance.m_projectSeed = value;
		}
	}

	// Token: 0x170015E1 RID: 5601
	// (get) Token: 0x06003FFB RID: 16379 RVA: 0x000E2CB8 File Offset: 0x000E0EB8
	// (set) Token: 0x06003FFC RID: 16380 RVA: 0x000E2CC5 File Offset: 0x000E0EC5
	public static string TempSeed
	{
		get
		{
			string empty = string.Empty;
			bool isEditor = Application.isEditor;
			return empty;
		}
		private set
		{
		}
	}

	// Token: 0x170015E2 RID: 5602
	// (get) Token: 0x06003FFD RID: 16381 RVA: 0x000E2CC7 File Offset: 0x000E0EC7
	// (set) Token: 0x06003FFE RID: 16382 RVA: 0x000E2CCE File Offset: 0x000E0ECE
	public static Queue<int> PreviousMasterSeedOverrides
	{
		get
		{
			return RNGSeedManager.m_previousMasterSeedOverrides;
		}
		private set
		{
			RNGSeedManager.m_previousMasterSeedOverrides = value;
		}
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x000E2CD6 File Offset: 0x000E0ED6
	private void OnEnable()
	{
		SceneLoader_RL.SceneLoadingEndRelay.AddListener(new Action<string>(this.OnSceneLoaded), false);
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x000E2CF0 File Offset: 0x000E0EF0
	private void OnSceneLoaded(string sceneName)
	{
		if (SceneLoadingUtility.GetSceneID(sceneName) == SceneID.MainMenu || SceneLoadingUtility.GetSceneID(sceneName) == SceneID.Tutorial)
		{
			RNGSeedManager.ClearOverride();
		}
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x000E2D0C File Offset: 0x000E0F0C
	public static void GenerateNewSeed(string sceneName)
	{
		if (RNGSeedManager.m_sceneToPreviousSeedTable.ContainsKey(sceneName))
		{
			RNGSeedManager.m_sceneToPreviousSeedTable[sceneName] = RNGSeedManager.m_sceneToCurrentSeedTable[sceneName];
		}
		else
		{
			int value = -1;
			if (RNGSeedManager.m_sceneToCurrentSeedTable.ContainsKey(sceneName))
			{
				value = RNGSeedManager.m_sceneToCurrentSeedTable[sceneName];
			}
			RNGSeedManager.m_sceneToPreviousSeedTable.Add(sceneName, value);
		}
		if (SceneLoadingUtility.ActiveScene.name == "World" && RNGSeedManager.IsOverrideSet)
		{
			string empty = string.Empty;
			if (RNGSeedManager.IsTempSeedSet)
			{
				RNGSeedManager.m_sceneToCurrentSeedTable[sceneName] = RNGSeedManager.GetSeedAsInt(RNGSeedManager.TempSeed);
				return;
			}
			if (RNGSeedManager.IsProjectSeedSet)
			{
				RNGSeedManager.m_sceneToCurrentSeedTable[sceneName] = RNGSeedManager.GetSeedAsInt(RNGSeedManager.ProjectSeed);
				return;
			}
			if (RNGSeedManager.IsMasterSeedOverrideSet && sceneName.Equals("World"))
			{
				RNGSeedManager.m_sceneToCurrentSeedTable[sceneName] = RNGSeedManager.MasterSeedOverride;
				return;
			}
		}
		else
		{
			RNGSeedManager.m_sceneToCurrentSeedTable[sceneName] = Environment.TickCount;
		}
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x000E2DFC File Offset: 0x000E0FFC
	public static int GetCurrentSeed(string sceneName)
	{
		int result = -1;
		if (RNGSeedManager.m_sceneToCurrentSeedTable.ContainsKey(sceneName))
		{
			result = RNGSeedManager.m_sceneToCurrentSeedTable[sceneName];
		}
		return result;
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x000E2E25 File Offset: 0x000E1025
	public static int GetPreviousSeed(string sceneName)
	{
		if (RNGSeedManager.m_sceneToPreviousSeedTable.ContainsKey(sceneName))
		{
			return RNGSeedManager.m_sceneToPreviousSeedTable[sceneName];
		}
		return -1;
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x000E2E41 File Offset: 0x000E1041
	public static string GetSeedAsHex(int seed)
	{
		return string.Format("{0}", seed.ToString("X"));
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x000E2E5C File Offset: 0x000E105C
	private static int GetSeedAsInt(string seed)
	{
		int result;
		try
		{
			result = int.Parse(seed, NumberStyles.HexNumber);
		}
		catch (Exception)
		{
			result = -1;
		}
		return result;
	}

	// Token: 0x06004006 RID: 16390 RVA: 0x000E2E90 File Offset: 0x000E1090
	public static bool IsSeedValid(string seed)
	{
		if (!string.IsNullOrEmpty(seed))
		{
			try
			{
				int.Parse(seed, NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		return true;
	}

	// Token: 0x06004007 RID: 16391 RVA: 0x000E2ECC File Offset: 0x000E10CC
	public static void SetMasterSeedOverride(int seed)
	{
		RNGSeedManager.MasterSeedOverride = seed;
		if (SaveManager.IsInitialized)
		{
			SaveManager.PlayerSaveData.PreviousMasterSeed = SaveManager.PlayerSaveData.MasterSeed;
			SaveManager.PlayerSaveData.MasterSeed = seed;
		}
	}

	// Token: 0x06004008 RID: 16392 RVA: 0x000E2EFA File Offset: 0x000E10FA
	public static void SetProjectSeed(string seed)
	{
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x000E2EFC File Offset: 0x000E10FC
	public static void SetTempSeed(string seed)
	{
		RNGSeedManager.TempSeed = seed;
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x000E2F04 File Offset: 0x000E1104
	public static void ClearOverride()
	{
		RNGSeedManager.MasterSeedOverride = -1;
		RNGSeedManager.TempSeed = string.Empty;
		RNGSeedManager.ProjectSeed = string.Empty;
	}

	// Token: 0x0400310E RID: 12558
	[SerializeField]
	private string m_projectSeed = "";

	// Token: 0x0400310F RID: 12559
	private static RNGSeedManager m_instance = null;

	// Token: 0x04003110 RID: 12560
	private static int m_masterSeedOverride = -1;

	// Token: 0x04003111 RID: 12561
	private static Dictionary<string, int> m_sceneToCurrentSeedTable = new Dictionary<string, int>();

	// Token: 0x04003112 RID: 12562
	private static Dictionary<string, int> m_sceneToPreviousSeedTable = new Dictionary<string, int>();

	// Token: 0x04003113 RID: 12563
	private static Queue<int> m_previousMasterSeedOverrides = new Queue<int>(5);

	// Token: 0x04003114 RID: 12564
	public const string RESOURCES_PATH = "Scriptable Objects/RNGSeedManager";

	// Token: 0x04003115 RID: 12565
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/RNGSeedManager.asset";

	// Token: 0x04003116 RID: 12566
	private const string EDITOR_PREFS_SEED_KEY = "Temp Seed (HEX)";

	// Token: 0x04003117 RID: 12567
	private const string WORLD_SCENE_NAME = "World";
}
