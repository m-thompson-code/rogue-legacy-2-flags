using System;
using System.Collections;

// Token: 0x02000328 RID: 808
public class BounceTerrain_Trait : BaseTrait
{
	// Token: 0x17000DAC RID: 3500
	// (get) Token: 0x06001FB8 RID: 8120 RVA: 0x000653B5 File Offset: 0x000635B5
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BounceTerrain;
		}
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000653BC File Offset: 0x000635BC
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
