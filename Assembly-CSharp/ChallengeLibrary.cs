using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
[CreateAssetMenu(menuName = "Custom/Libraries/Challenge Library")]
public class ChallengeLibrary : ScriptableObject
{
	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06001666 RID: 5734 RVA: 0x00045E6C File Offset: 0x0004406C
	private static ChallengeLibrary Instance
	{
		get
		{
			if (ChallengeLibrary.m_instance == null)
			{
				ChallengeLibrary.m_instance = CDGResources.Load<ChallengeLibrary>("Scriptable Objects/Libraries/ChallengeLibrary", "", true);
			}
			return ChallengeLibrary.m_instance;
		}
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x00045E98 File Offset: 0x00044098
	public static ChallengeData GetChallengeData(ChallengeType challengeType)
	{
		ChallengeData result = null;
		if (ChallengeLibrary.Instance.m_challengeLibrary != null)
		{
			ChallengeLibrary.Instance.m_challengeLibrary.TryGetValue(challengeType, out result);
			return result;
		}
		throw new Exception("Challenge Library is null.");
	}

	// Token: 0x040015A3 RID: 5539
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ChallengeLibrary";

	// Token: 0x040015A4 RID: 5540
	[Space(10f)]
	[SerializeField]
	private ChallengeTypeChallengeDataDictionary m_challengeLibrary;

	// Token: 0x040015A5 RID: 5541
	private static ChallengeLibrary m_instance;

	// Token: 0x02000B2C RID: 2860
	[Serializable]
	public class ChallengeIconEntry
	{
		// Token: 0x04004B98 RID: 19352
		public Sprite ChallengeIcon;

		// Token: 0x04004B99 RID: 19353
		public Sprite BronzeTrophyIcon;

		// Token: 0x04004B9A RID: 19354
		public Sprite SilverTrophyIcon;

		// Token: 0x04004B9B RID: 19355
		public Sprite GoldTrophyIcon;
	}

	// Token: 0x02000B2D RID: 2861
	public enum ChallengeIconEntryType
	{
		// Token: 0x04004B9D RID: 19357
		Challenge,
		// Token: 0x04004B9E RID: 19358
		Bronze,
		// Token: 0x04004B9F RID: 19359
		Silver,
		// Token: 0x04004BA0 RID: 19360
		Gold
	}
}
