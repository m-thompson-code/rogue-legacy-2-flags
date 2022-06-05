using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class InvulnDash_Trait : BaseTrait
{
	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06002036 RID: 8246 RVA: 0x0006659F File Offset: 0x0006479F
	public override TraitType TraitType
	{
		get
		{
			return TraitType.InvulnDash;
		}
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000665A6 File Offset: 0x000647A6
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.InitializeHealthMods();
		playerController.ResetHealth();
		yield break;
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000665AE File Offset: 0x000647AE
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.InitializeHealthMods();
			playerController.ResetHealth();
		}
	}
}
