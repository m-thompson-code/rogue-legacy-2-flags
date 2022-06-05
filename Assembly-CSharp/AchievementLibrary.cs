using System;
using UnityEngine;

// Token: 0x020003CD RID: 973
[CreateAssetMenu(menuName = "Custom/Libraries/Achievement Library")]
public class AchievementLibrary : ScriptableObject
{
	// Token: 0x17000E40 RID: 3648
	// (get) Token: 0x06001FEE RID: 8174 RVA: 0x00010E36 File Offset: 0x0000F036
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

	// Token: 0x06001FEF RID: 8175 RVA: 0x000A4150 File Offset: 0x000A2350
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

	// Token: 0x04001C94 RID: 7316
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/AchievementLibrary";

	// Token: 0x04001C95 RID: 7317
	[SerializeField]
	private AchievementTypeAchievementDataDictionary m_achievementLibrary;

	// Token: 0x04001C96 RID: 7318
	private static AchievementLibrary m_instance;
}
