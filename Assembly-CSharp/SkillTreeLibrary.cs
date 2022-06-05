using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
[CreateAssetMenu(menuName = "Custom/Libraries/Skill Tree Library")]
public class SkillTreeLibrary : ScriptableObject
{
	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x06001764 RID: 5988 RVA: 0x00048EDE File Offset: 0x000470DE
	private static SkillTreeLibrary Instance
	{
		get
		{
			if (SkillTreeLibrary.m_instance == null)
			{
				SkillTreeLibrary.m_instance = CDGResources.Load<SkillTreeLibrary>("Scriptable Objects/Libraries/SkillTreeLibrary", "", true);
			}
			return SkillTreeLibrary.m_instance;
		}
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x00048F08 File Offset: 0x00047108
	public static SkillTreeData GetSkillTreeData(SkillTreeType skillType)
	{
		SkillTreeData result = null;
		if (SkillTreeLibrary.Instance.m_skillTreeLibrary != null)
		{
			SkillTreeLibrary.Instance.m_skillTreeLibrary.TryGetValue(skillType, out result);
			return result;
		}
		throw new Exception("SkillTree Library is null.");
	}

	// Token: 0x040016D7 RID: 5847
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkillTreeLibrary";

	// Token: 0x040016D8 RID: 5848
	[Space(10f)]
	[SerializeField]
	private SkillTreeTypeSkillTreeDataDictionary m_skillTreeLibrary;

	// Token: 0x040016D9 RID: 5849
	private static SkillTreeLibrary m_instance;
}
