using System;
using System.Collections.Generic;
using System.Globalization;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000B8D RID: 2957
[CreateAssetMenu(menuName = "Custom/Data/RNG Seed Manager")]
public class RNGSeedManager : ScriptableObject
{
	// Token: 0x17001DD1 RID: 7633
	// (get) Token: 0x06005928 RID: 22824 RVA: 0x0003081F File Offset: 0x0002EA1F
	// (set) Token: 0x06005929 RID: 22825 RVA: 0x00030826 File Offset: 0x0002EA26
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

	// Token: 0x17001DD2 RID: 7634
	// (get) Token: 0x0600592A RID: 22826 RVA: 0x00030851 File Offset: 0x0002EA51
	public static string CurrentWorldSeed
	{
		get
		{
			return RNGSeedManager.GetSeedAsHex(RNGSeedManager.GetCurrentSeed("World"));
		}
	}

	// Token: 0x17001DD3 RID: 7635
	// (get) Token: 0x0600592B RID: 22827 RVA: 0x00030862 File Offset: 0x0002EA62
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

	// Token: 0x17001DD4 RID: 7636
	// (get) Token: 0x0600592C RID: 22828 RVA: 0x00030892 File Offset: 0x0002EA92
	public static bool IsMasterSeedOverrideSet
	{
		get
		{
			return RNGSeedManager.MasterSeedOverride != -1;
		}
	}

	// Token: 0x17001DD5 RID: 7637
	// (get) Token: 0x0600592D RID: 22829 RVA: 0x0003089F File Offset: 0x0002EA9F
	public static bool IsOverrideSet
	{
		get
		{
			return (Application.isEditor && RNGSeedManager.IsTempSeedSet) || RNGSeedManager.IsProjectSeedSet || RNGSeedManager.IsMasterSeedOverrideSet;
		}
	}

	// Token: 0x17001DD6 RID: 7638
	// (get) Token: 0x0600592E RID: 22830 RVA: 0x000308BF File Offset: 0x0002EABF
	public static bool IsProjectSeedSet
	{
		get
		{
			return !string.IsNullOrEmpty(RNGSeedManager.ProjectSeed);
		}
	}

	// Token: 0x17001DD7 RID: 7639
	// (get) Token: 0x0600592F RID: 22831 RVA: 0x000308CE File Offset: 0x0002EACE
	public static bool IsTempSeedSet
	{
		get
		{
			return Application.isEditor && !string.IsNullOrEmpty(RNGSeedManager.TempSeed);
		}
	}

	// Token: 0x17001DD8 RID: 7640
	// (get) Token: 0x06005930 RID: 22832 RVA: 0x000308E6 File Offset: 0x0002EAE6
	// (set) Token: 0x06005931 RID: 22833 RVA: 0x000308F2 File Offset: 0x0002EAF2
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

	// Token: 0x17001DD9 RID: 7641
	// (get) Token: 0x06005932 RID: 22834 RVA: 0x000308FF File Offset: 0x0002EAFF
	// (set) Token: 0x06005933 RID: 22835 RVA: 0x00002FCA File Offset: 0x000011CA
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

	// Token: 0x17001DDA RID: 7642
	// (get) Token: 0x06005934 RID: 22836 RVA: 0x0003090C File Offset: 0x0002EB0C
	// (set) Token: 0x06005935 RID: 22837 RVA: 0x00030913 File Offset: 0x0002EB13
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

	// Token: 0x06005936 RID: 22838 RVA: 0x0003091B File Offset: 0x0002EB1B
	private void OnEnable()
	{
		SceneLoader_RL.SceneLoadingEndRelay.AddListener(new Action<string>(this.OnSceneLoaded), false);
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x00030935 File Offset: 0x0002EB35
	private void OnSceneLoaded(string sceneName)
	{
		if (SceneLoadingUtility.GetSceneID(sceneName) == SceneID.MainMenu || SceneLoadingUtility.GetSceneID(sceneName) == SceneID.Tutorial)
		{
			RNGSeedManager.ClearOverride();
		}
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x00152754 File Offset: 0x00150954
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

	// Token: 0x06005939 RID: 22841 RVA: 0x00152844 File Offset: 0x00150A44
	public static int GetCurrentSeed(string sceneName)
	{
		int result = -1;
		if (RNGSeedManager.m_sceneToCurrentSeedTable.ContainsKey(sceneName))
		{
			result = RNGSeedManager.m_sceneToCurrentSeedTable[sceneName];
		}
		return result;
	}

	// Token: 0x0600593A RID: 22842 RVA: 0x0003094F File Offset: 0x0002EB4F
	public static int GetPreviousSeed(string sceneName)
	{
		if (RNGSeedManager.m_sceneToPreviousSeedTable.ContainsKey(sceneName))
		{
			return RNGSeedManager.m_sceneToPreviousSeedTable[sceneName];
		}
		return -1;
	}

	// Token: 0x0600593B RID: 22843 RVA: 0x0003096B File Offset: 0x0002EB6B
	public static string GetSeedAsHex(int seed)
	{
		return string.Format("{0}", seed.ToString("X"));
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x00152870 File Offset: 0x00150A70
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

	// Token: 0x0600593D RID: 22845 RVA: 0x001528A4 File Offset: 0x00150AA4
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

	// Token: 0x0600593E RID: 22846 RVA: 0x00030983 File Offset: 0x0002EB83
	public static void SetMasterSeedOverride(int seed)
	{
		RNGSeedManager.MasterSeedOverride = seed;
		if (SaveManager.IsInitialized)
		{
			SaveManager.PlayerSaveData.PreviousMasterSeed = SaveManager.PlayerSaveData.MasterSeed;
			SaveManager.PlayerSaveData.MasterSeed = seed;
		}
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x00002FCA File Offset: 0x000011CA
	public static void SetProjectSeed(string seed)
	{
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x000309B1 File Offset: 0x0002EBB1
	public static void SetTempSeed(string seed)
	{
		RNGSeedManager.TempSeed = seed;
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x000309B9 File Offset: 0x0002EBB9
	public static void ClearOverride()
	{
		RNGSeedManager.MasterSeedOverride = -1;
		RNGSeedManager.TempSeed = string.Empty;
		RNGSeedManager.ProjectSeed = string.Empty;
	}

	// Token: 0x0400435D RID: 17245
	[SerializeField]
	private string m_projectSeed = "";

	// Token: 0x0400435E RID: 17246
	private static RNGSeedManager m_instance = null;

	// Token: 0x0400435F RID: 17247
	private static int m_masterSeedOverride = -1;

	// Token: 0x04004360 RID: 17248
	private static Dictionary<string, int> m_sceneToCurrentSeedTable = new Dictionary<string, int>();

	// Token: 0x04004361 RID: 17249
	private static Dictionary<string, int> m_sceneToPreviousSeedTable = new Dictionary<string, int>();

	// Token: 0x04004362 RID: 17250
	private static Queue<int> m_previousMasterSeedOverrides = new Queue<int>(5);

	// Token: 0x04004363 RID: 17251
	public const string RESOURCES_PATH = "Scriptable Objects/RNGSeedManager";

	// Token: 0x04004364 RID: 17252
	private const string ASSET_PATH = "Assets/Content/Scriptable Objects/RNGSeedManager.asset";

	// Token: 0x04004365 RID: 17253
	private const string EDITOR_PREFS_SEED_KEY = "Temp Seed (HEX)";

	// Token: 0x04004366 RID: 17254
	private const string WORLD_SCENE_NAME = "World";
}
