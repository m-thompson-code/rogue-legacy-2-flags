using System;
using System.Collections;

// Token: 0x020005BA RID: 1466
public class MegaHealth_Trait : BaseTrait
{
	// Token: 0x1700122E RID: 4654
	// (get) Token: 0x06002DAF RID: 11695 RVA: 0x00019212 File Offset: 0x00017412
	public override TraitType TraitType
	{
		get
		{
			return TraitType.MegaHealth;
		}
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x00019219 File Offset: 0x00017419
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

	// Token: 0x06002DB1 RID: 11697 RVA: 0x00018A1E File Offset: 0x00016C1E
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
