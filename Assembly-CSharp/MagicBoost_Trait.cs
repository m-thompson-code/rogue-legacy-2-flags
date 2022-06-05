using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class MagicBoost_Trait : BaseTrait
{
	// Token: 0x17001226 RID: 4646
	// (get) Token: 0x06002D94 RID: 11668 RVA: 0x00019189 File Offset: 0x00017389
	public override TraitType TraitType
	{
		get
		{
			return TraitType.MagicBoost;
		}
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x00019190 File Offset: 0x00017390
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

	// Token: 0x06002D96 RID: 11670 RVA: 0x00018CF5 File Offset: 0x00016EF5
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
