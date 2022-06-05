using System;
using TMPro;
using UnityEngine;

// Token: 0x02000680 RID: 1664
public class SeedPanelController : MonoBehaviour
{
	// Token: 0x060032CB RID: 13003 RVA: 0x000D9FA0 File Offset: 0x000D81A0
	private void OnEnable()
	{
		if (SceneLoadingUtility.ActiveScene.name == SceneLoadingUtility.GetSceneName(SceneID.World))
		{
			int currentSeed = RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name);
			this.m_seedText.text = "Seed: " + RNGSeedManager.GetSeedAsHex(currentSeed) + "-" + BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString();
			return;
		}
		this.m_seedText.text = string.Empty;
	}

	// Token: 0x0400298E RID: 10638
	[SerializeField]
	private TextMeshProUGUI m_seedText;
}
