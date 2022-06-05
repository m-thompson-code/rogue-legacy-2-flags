using System;
using UnityEngine;

// Token: 0x02000406 RID: 1030
[CreateAssetMenu(menuName = "Custom/Libraries/Skill Tree Library")]
public class SkillTreeLibrary : ScriptableObject
{
	// Token: 0x17000E8B RID: 3723
	// (get) Token: 0x06002117 RID: 8471 RVA: 0x000119AE File Offset: 0x0000FBAE
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

	// Token: 0x06002118 RID: 8472 RVA: 0x000A68B4 File Offset: 0x000A4AB4
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

	// Token: 0x04001DEF RID: 7663
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkillTreeLibrary";

	// Token: 0x04001DF0 RID: 7664
	[Space(10f)]
	[SerializeField]
	private SkillTreeTypeSkillTreeDataDictionary m_skillTreeLibrary;

	// Token: 0x04001DF1 RID: 7665
	private static SkillTreeLibrary m_instance;
}
