using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
public class NewGamePlusTester : MonoBehaviour
{
	// Token: 0x0600229C RID: 8860 RVA: 0x000AB26C File Offset: 0x000A946C
	private void Start()
	{
		SaveManager.PlayerSaveData.NewGamePlusLevel = this.m_NGPlusLevel;
		foreach (NewGamePlusTester.BurdenTestEntry burdenTestEntry in this.m_burdenTestArray)
		{
			BurdenManager.SetBurdenLevel(burdenTestEntry.BurdenType, burdenTestEntry.BurdenLevel, false, true);
		}
	}

	// Token: 0x04001F39 RID: 7993
	[SerializeField]
	private NewGamePlusTester.BurdenTestEntry[] m_burdenTestArray;

	// Token: 0x04001F3A RID: 7994
	[SerializeField]
	private int m_NGPlusLevel;

	// Token: 0x02000434 RID: 1076
	[Serializable]
	private struct BurdenTestEntry
	{
		// Token: 0x04001F3B RID: 7995
		public BurdenType BurdenType;

		// Token: 0x04001F3C RID: 7996
		public int BurdenLevel;
	}
}
