using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class InvulnDash_Trait : BaseTrait
{
	// Token: 0x1700121C RID: 4636
	// (get) Token: 0x06002D75 RID: 11637 RVA: 0x00017970 File Offset: 0x00015B70
	public override TraitType TraitType
	{
		get
		{
			return TraitType.InvulnDash;
		}
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x0001910B File Offset: 0x0001730B
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

	// Token: 0x06002D77 RID: 11639 RVA: 0x00018A1E File Offset: 0x00016C1E
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
