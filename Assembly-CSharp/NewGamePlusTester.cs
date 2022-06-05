using System;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class NewGamePlusTester : MonoBehaviour
{
	// Token: 0x060018AD RID: 6317 RVA: 0x0004D530 File Offset: 0x0004B730
	private void Start()
	{
		SaveManager.PlayerSaveData.NewGamePlusLevel = this.m_NGPlusLevel;
		foreach (NewGamePlusTester.BurdenTestEntry burdenTestEntry in this.m_burdenTestArray)
		{
			BurdenManager.SetBurdenLevel(burdenTestEntry.BurdenType, burdenTestEntry.BurdenLevel, false, true);
		}
	}

	// Token: 0x040017F6 RID: 6134
	[SerializeField]
	private NewGamePlusTester.BurdenTestEntry[] m_burdenTestArray;

	// Token: 0x040017F7 RID: 6135
	[SerializeField]
	private int m_NGPlusLevel;

	// Token: 0x02000B41 RID: 2881
	[Serializable]
	private struct BurdenTestEntry
	{
		// Token: 0x04004BD8 RID: 19416
		public BurdenType BurdenType;

		// Token: 0x04004BD9 RID: 19417
		public int BurdenLevel;
	}
}
