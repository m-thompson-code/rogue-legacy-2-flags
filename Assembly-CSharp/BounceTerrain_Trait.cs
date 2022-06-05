using System;
using System.Collections;

// Token: 0x02000578 RID: 1400
public class BounceTerrain_Trait : BaseTrait
{
	// Token: 0x170011E1 RID: 4577
	// (get) Token: 0x06002C8F RID: 11407 RVA: 0x00018AD8 File Offset: 0x00016CD8
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BounceTerrain;
		}
	}

	// Token: 0x06002C90 RID: 11408 RVA: 0x00018ADF File Offset: 0x00016CDF
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().LookController.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		yield break;
	}
}
