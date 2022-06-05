using System;
using System.Collections;

// Token: 0x020005C7 RID: 1479
public class OmniDash_Trait : BaseTrait
{
	// Token: 0x1700124B RID: 4683
	// (get) Token: 0x06002DF0 RID: 11760 RVA: 0x000179D1 File Offset: 0x00015BD1
	public override TraitType TraitType
	{
		get
		{
			return TraitType.OmniDash;
		}
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x00019343 File Offset: 0x00017543
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().CharacterDash.EnableOmnidash = true;
		yield break;
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x0001934B File Offset: 0x0001754B
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterDash.EnableOmnidash = false;
		}
	}
}
