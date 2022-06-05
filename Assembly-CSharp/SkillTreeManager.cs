using System;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public class SkillTreeManager : MonoBehaviour
{
	// Token: 0x06005801 RID: 22529 RVA: 0x0002FD73 File Offset: 0x0002DF73
	private void Awake()
	{
		this.Initialize();
		Debug.Log("<color=green>Creating SkillTreeManager...</color>");
	}

	// Token: 0x06005802 RID: 22530 RVA: 0x0002FD85 File Offset: 0x0002DF85
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

	// Token: 0x06005803 RID: 22531 RVA: 0x0002FDB8 File Offset: 0x0002DFB8
	private void OnDestroy()
	{
		SkillTreeManager.m_skillTreeManager = null;
		SkillTreeManager.IsInitialized = false;
	}

	// Token: 0x17001D75 RID: 7541
	// (get) Token: 0x06005804 RID: 22532 RVA: 0x0002FDC6 File Offset: 0x0002DFC6
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

	// Token: 0x17001D76 RID: 7542
	// (get) Token: 0x06005805 RID: 22533 RVA: 0x0002FDE4 File Offset: 0x0002DFE4
	// (set) Token: 0x06005806 RID: 22534 RVA: 0x0002FDEB File Offset: 0x0002DFEB
	public static bool IsInitialized { get; private set; }

	// Token: 0x06005807 RID: 22535 RVA: 0x0002FDF3 File Offset: 0x0002DFF3
	public static void ResetCachedTotalSkills()
	{
		SkillTreeManager.m_useCachedTotalSkillLevel = false;
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x00150604 File Offset: 0x0014E804
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

	// Token: 0x06005809 RID: 22537 RVA: 0x00150650 File Offset: 0x0014E850
	public static int GetSkillObjLevel(SkillTreeType skillTreeType)
	{
		SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(skillTreeType);
		if (skillTreeObj != null)
		{
			return skillTreeObj.ClampedLevel;
		}
		return 0;
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x00150670 File Offset: 0x0014E870
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

	// Token: 0x0600580B RID: 22539 RVA: 0x00150708 File Offset: 0x0014E908
	public static SkillTreeObj GetSkillTreeObj(SkillTreeType skillTreeType)
	{
		SkillTreeObj result = null;
		SaveManager.EquipmentSaveData.SkillTreeDict.TryGetValue(skillTreeType, out result);
		return result;
	}

	// Token: 0x0400410B RID: 16651
	private const string SKILLTREE_MANAGER = "SkillTreeManager";

	// Token: 0x0400410C RID: 16652
	private SkillLevelChangedEventArgs m_skillLevelChangedEventArgs;

	// Token: 0x0400410D RID: 16653
	private static SkillTreeManager m_skillTreeManager;

	// Token: 0x0400410E RID: 16654
	private static int m_cachedTotalSkillLevel;

	// Token: 0x0400410F RID: 16655
	private static bool m_useCachedTotalSkillLevel;
}
