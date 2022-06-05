using System;
using TMPro;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class SeedPanelController : MonoBehaviour
{
	// Token: 0x0600248F RID: 9359 RVA: 0x00079C9C File Offset: 0x00077E9C
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

	// Token: 0x04001F16 RID: 7958
	[SerializeField]
	private TextMeshProUGUI m_seedText;
}
