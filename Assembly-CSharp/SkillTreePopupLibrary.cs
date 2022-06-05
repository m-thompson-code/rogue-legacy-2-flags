using System;
using UnityEngine;

// Token: 0x0200024B RID: 587
[CreateAssetMenu(menuName = "Custom/Libraries/Skill Tree Popup Library")]
public class SkillTreePopupLibrary : ScriptableObject
{
	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x06001769 RID: 5993 RVA: 0x00048F56 File Offset: 0x00047156
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

	// Token: 0x0600176A RID: 5994 RVA: 0x00048F80 File Offset: 0x00047180
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

	// Token: 0x040016DE RID: 5854
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/SkillTreePopupLibrary";

	// Token: 0x040016DF RID: 5855
	[Space(10f)]
	[SerializeField]
	private SkillTreeTypeSkillTreePopupDataDictionary m_popupLibrary;

	// Token: 0x040016E0 RID: 5856
	private static SkillTreePopupLibrary m_instance;
}
