using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
[CreateAssetMenu(menuName = "Custom/Libraries/Achievement Library")]
public class AchievementLibrary : ScriptableObject
{
	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x06001652 RID: 5714 RVA: 0x00045BBF File Offset: 0x00043DBF
	private static AchievementLibrary Instance
	{
		get
		{
			if (!AchievementLibrary.m_instance)
			{
				AchievementLibrary.m_instance = CDGResources.Load<AchievementLibrary>("Scriptable Objects/Libraries/AchievementLibrary", "", true);
			}
			return AchievementLibrary.m_instance;
		}
	}

	// Token: 0x06001653 RID: 5715 RVA: 0x00045BE8 File Offset: 0x00043DE8
	public static AchievementData GetAchievementData(AchievementType achievementType)
	{
		AchievementData result = null;
		if (AchievementLibrary.Instance.m_achievementLibrary != null)
		{
			AchievementLibrary.Instance.m_achievementLibrary.TryGetValue(achievementType, out result);
			return result;
		}
		throw new Exception("Achievement Library is null.");
	}

	// Token: 0x04001591 RID: 5521
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AchievementLibrary";

	// Token: 0x04001592 RID: 5522
	[SerializeField]
	private AchievementTypeAchievementDataDictionary m_achievementLibrary;

	// Token: 0x04001593 RID: 5523
	private static AchievementLibrary m_instance;
}
