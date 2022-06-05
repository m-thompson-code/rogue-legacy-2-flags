using System;
using UnityEngine;

// Token: 0x020006AE RID: 1710
public class SkillTreeManager : MonoBehaviour
{
	// Token: 0x06003EF9 RID: 16121 RVA: 0x000E0231 File Offset: 0x000DE431
	private void Awake()
	{
		this.Initialize();
		Debug.Log("<color=green>Creating SkillTreeManager...</color>");
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x000E0243 File Offset: 0x000DE443
	private void Initialize()
	{
		if (Application.isPlaying)
		{
			this.m_skillLevelChangedEventArgs = new SkillLevelChangedEventArgs(SkillTreeType.None, 0, 0);
			if (!SaveManager.EquipmentSaveData.IsInitialized)
			{
				SaveManager.EquipmentSaveData.Initialize();
			}
			SkillTreeManager.IsInitialized = true;
		}
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x000E0276 File Offset: 0x000DE476
	private void OnDestroy()
	{
		SkillTreeManager.m_skillTreeManager = null;
		SkillTreeManager.IsInitialized = false;
	}

	// Token: 0x17001589 RID: 5513
	// (get) Token: 0x06003EFC RID: 16124 RVA: 0x000E0284 File Offset: 0x000DE484
	public static SkillTreeManager Instance
	{
		get
		{
			if (!SkillTreeManager.m_skillTreeManager)
			{
				SkillTreeManager.m_skillTreeManager = CDGHelper.FindStaticInstance<SkillTreeManager>(false);
			}
			return SkillTreeManager.m_skillTreeManager;
		}
	}

	// Token: 0x1700158A RID: 5514
	// (get) Token: 0x06003EFD RID: 16125 RVA: 0x000E02A2 File Offset: 0x000DE4A2
	// (set) Token: 0x06003EFE RID: 16126 RVA: 0x000E02A9 File Offset: 0x000DE4A9
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003EFF RID: 16127 RVA: 0x000E02B1 File Offset: 0x000DE4B1
	public static void ResetCachedTotalSkills()
	{
		SkillTreeManager.m_useCachedTotalSkillLevel = false;
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x000E02BC File Offset: 0x000DE4BC
	public static int GetTotalSkillObjLevel()
	{
		if (!SkillTreeManager.m_useCachedTotalSkillLevel)
		{
			int num = 0;
			foreach (SkillTreeType skillTreeType in SkillTreeType_RL.TypeArray)
			{
				if (skillTreeType != SkillTreeType.None)
				{
					num += SkillTreeManager.GetSkillObjLevel(skillTreeType);
				}
			}
			SkillTreeManager.m_useCachedTotalSkillLevel = true;
			SkillTreeManager.m_cachedTotalSkillLevel = num;
		}
		return SkillTreeManager.m_cachedTotalSkillLevel;
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x000E0308 File Offset: 0x000DE508
	public static int GetSkillObjLevel(SkillTreeType skillTreeType)
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(skillTreeType);
		if (skillTreeObj != null)
		{
			return skillTreeObj.ClampedLevel;
		}
		return 0;
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x000E0328 File Offset: 0x000DE528
	public static bool SetSkillObjLevel(SkillTreeType skillTreeType, int level, bool additive, bool runEvents = true, bool ignoreMaxLevelClamping = false)
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(skillTreeType);
		if (skillTreeObj != null)
		{
			int level2 = skillTreeObj.Level;
			if (additive)
			{
				skillTreeObj.Level += level;
			}
			else
			{
				skillTreeObj.Level = level;
			}
			if (!ignoreMaxLevelClamping)
			{
				skillTreeObj.Level = Mathf.Clamp(skillTreeObj.Level, 0, skillTreeObj.MaxLevel);
			}
			if (level2 != skillTreeObj.Level)
			{
				SkillTreeManager.m_useCachedTotalSkillLevel = false;
				if (runEvents)
				{
					SkillTreeManager.Instance.m_skillLevelChangedEventArgs.Initialize(skillTreeType, level2, skillTreeObj.Level);
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.SkillLevelChanged, SkillTreeManager.Instance, SkillTreeManager.Instance.m_skillLevelChangedEventArgs);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x000E03C0 File Offset: 0x000DE5C0
	public static SkillTreeObj GetSkillTreeObj(SkillTreeType skillTreeType)
	{
		SkillTreeObj result = null;
		SaveManager.EquipmentSaveData.SkillTreeDict.TryGetValue(skillTreeType, out result);
		return result;
	}

	// Token: 0x04002ED2 RID: 11986
	private const string SKILLTREE_MANAGER = "SkillTreeManager";

	// Token: 0x04002ED3 RID: 11987
	private SkillLevelChangedEventArgs m_skillLevelChangedEventArgs;

	// Token: 0x04002ED4 RID: 11988
	private static SkillTreeManager m_skillTreeManager;

	// Token: 0x04002ED5 RID: 11989
	private static int m_cachedTotalSkillLevel;

	// Token: 0x04002ED6 RID: 11990
	private static bool m_useCachedTotalSkillLevel;
}
