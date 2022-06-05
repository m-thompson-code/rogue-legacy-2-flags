using System;
using System.Collections;

// Token: 0x020005E3 RID: 1507
public class YouAreBlue_Trait : BaseTrait
{
	// Token: 0x1700126E RID: 4718
	// (get) Token: 0x06002E6D RID: 11885 RVA: 0x0001960C File Offset: 0x0001780C
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreBlue;
		}
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x00019613 File Offset: 0x00017813
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
