using System;
using System.Collections;

// Token: 0x02000584 RID: 1412
public class DamageBoost_Trait : BaseTrait
{
	// Token: 0x170011F1 RID: 4593
	// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x0001865E File Offset: 0x0001685E
	public override TraitType TraitType
	{
		get
		{
			return TraitType.DamageBoost;
		}
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x00018CED File Offset: 0x00016EED
	public IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.InitializeStrengthMods();
		playerController.InitializeManaMods();
		playerController.InitializeMagicMods();
		playerController.ResetMana();
		yield break;
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x00018CF5 File Offset: 0x00016EF5
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
