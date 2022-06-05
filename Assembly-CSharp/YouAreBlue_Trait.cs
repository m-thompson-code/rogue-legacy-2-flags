using System;
using System.Collections;

// Token: 0x02000369 RID: 873
public class YouAreBlue_Trait : BaseTrait
{
	// Token: 0x17000DFB RID: 3579
	// (get) Token: 0x060020BC RID: 8380 RVA: 0x00066F78 File Offset: 0x00065178
	public override TraitType TraitType
	{
		get
		{
			return TraitType.YouAreBlue;
		}
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x00066F7F File Offset: 0x0006517F
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
