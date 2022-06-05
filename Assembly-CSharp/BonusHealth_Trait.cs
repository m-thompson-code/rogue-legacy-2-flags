using System;
using System.Collections;

// Token: 0x02000325 RID: 805
public class BonusHealth_Trait : BaseTrait
{
	// Token: 0x17000DA9 RID: 3497
	// (get) Token: 0x06001FAC RID: 8108 RVA: 0x00065331 File Offset: 0x00063531
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusHealth;
		}
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x00065338 File Offset: 0x00063538
	public IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.InitializeHealthMods();
		playerController.ResetHealth();
		yield break;
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x00065340 File Offset: 0x00063540
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
