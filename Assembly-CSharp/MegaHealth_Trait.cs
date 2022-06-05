using System;
using System.Collections;

// Token: 0x02000350 RID: 848
public class MegaHealth_Trait : BaseTrait
{
	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x06002052 RID: 8274 RVA: 0x0006670E File Offset: 0x0006490E
	public override TraitType TraitType
	{
		get
		{
			return TraitType.MegaHealth;
		}
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x00066715 File Offset: 0x00064915
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

	// Token: 0x06002054 RID: 8276 RVA: 0x0006671D File Offset: 0x0006491D
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
