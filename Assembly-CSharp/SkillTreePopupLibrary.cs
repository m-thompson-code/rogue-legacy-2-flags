using System;
using UnityEngine;

// Token: 0x02000408 RID: 1032
[CreateAssetMenu(menuName = "Custom/Libraries/Skill Tree Popup Library")]
public class SkillTreePopupLibrary : ScriptableObject
{
	// Token: 0x17000E8C RID: 3724
	// (get) Token: 0x0600211C RID: 8476 RVA: 0x000119D7 File Offset: 0x0000FBD7
	private static SkillTreePopupLibrary Instance
	{
		get
		{
			if (SkillTreePopupLibrary.m_instance == null)
			{
				SkillTreePopupLibrary.m_instance = CDGResources.Load<SkillTreePopupLibrary>("Scriptable Objects/Libraries/SkillTreePopupLibrary", "", true);
			}
			return SkillTreePopupLibrary.m_instance;
		}
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x000A68F0 File Offset: 0x000A4AF0
	public static SkillTreePopupData GetPopupData(SkillTreeType skillType)
	{
		SkillTreePopupData result = null;
		if (SkillTreePopupLibrary.Instance.m_popupLibrary != null)
		{
			SkillTreePopupLibrary.Instance.m_popupLibrary.TryGetValue(skillType, out result);
			return result;
		}
		throw new Exception("SkillTree Popup Library is null.");
	}

	// Token: 0x04001DF6 RID: 7670
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkillTreePopupLibrary";

	// Token: 0x04001DF7 RID: 7671
	[Space(10f)]
	[SerializeField]
	private SkillTreeTypeSkillTreePopupDataDictionary m_popupLibrary;

	// Token: 0x04001DF8 RID: 7672
	private static SkillTreePopupLibrary m_instance;
}
