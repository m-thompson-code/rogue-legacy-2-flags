using System;
using System.Collections;

// Token: 0x02000570 RID: 1392
public class BonusHealth_Trait : BaseTrait
{
	// Token: 0x170011D8 RID: 4568
	// (get) Token: 0x06002C6B RID: 11371 RVA: 0x00018A0F File Offset: 0x00016C0F
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusHealth;
		}
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x00018A16 File Offset: 0x00016C16
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

	// Token: 0x06002C6D RID: 11373 RVA: 0x00018A1E File Offset: 0x00016C1E
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
