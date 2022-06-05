using System;
using UnityEngine;

// Token: 0x020003D3 RID: 979
[CreateAssetMenu(menuName = "Custom/Libraries/Challenge Library")]
public class ChallengeLibrary : ScriptableObject
{
	// Token: 0x17000E45 RID: 3653
	// (get) Token: 0x06002002 RID: 8194 RVA: 0x00010F29 File Offset: 0x0000F129
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

	// Token: 0x06002003 RID: 8195 RVA: 0x000A42D0 File Offset: 0x000A24D0
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

	// Token: 0x04001CA6 RID: 7334
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ChallengeLibrary";

	// Token: 0x04001CA7 RID: 7335
	[Space(10f)]
	[SerializeField]
	private ChallengeTypeChallengeDataDictionary m_challengeLibrary;

	// Token: 0x04001CA8 RID: 7336
	private static ChallengeLibrary m_instance;

	// Token: 0x020003D4 RID: 980
	[Serializable]
	public class ChallengeIconEntry
	{
		// Token: 0x04001CA9 RID: 7337
		public Sprite ChallengeIcon;

		// Token: 0x04001CAA RID: 7338
		public Sprite BronzeTrophyIcon;

		// Token: 0x04001CAB RID: 7339
		public Sprite SilverTrophyIcon;

		// Token: 0x04001CAC RID: 7340
		public Sprite GoldTrophyIcon;
	}

	// Token: 0x020003D5 RID: 981
	public enum ChallengeIconEntryType
	{
		// Token: 0x04001CAE RID: 7342
		Challenge,
		// Token: 0x04001CAF RID: 7343
		Bronze,
		// Token: 0x04001CB0 RID: 7344
		Silver,
		// Token: 0x04001CB1 RID: 7345
		Gold
	}
}
