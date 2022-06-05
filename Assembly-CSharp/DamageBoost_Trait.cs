using System;
using System.Collections;

// Token: 0x0200032F RID: 815
public class DamageBoost_Trait : BaseTrait
{
	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00065B27 File Offset: 0x00063D27
	public override TraitType TraitType
	{
		get
		{
			return TraitType.DamageBoost;
		}
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x00065B2E File Offset: 0x00063D2E
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

	// Token: 0x06001FDE RID: 8158 RVA: 0x00065B36 File Offset: 0x00063D36
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
