using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class MagicBoost_Trait : BaseTrait
{
	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06002046 RID: 8262 RVA: 0x0006667A File Offset: 0x0006487A
	public override TraitType TraitType
	{
		get
		{
			return TraitType.MagicBoost;
		}
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x00066681 File Offset: 0x00064881
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.InitializeStrengthMods();
		playerController.InitializeManaMods();
		playerController.InitializeMagicMods();
		playerController.ResetMana();
		yield break;
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x00066689 File Offset: 0x00064889
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.InitializeStrengthMods();
			playerController.InitializeManaMods();
			playerController.InitializeMagicMods();
			playerController.ResetMana();
		}
	}
}
