using System;
using System.Collections;

// Token: 0x02000359 RID: 857
public class OmniDash_Trait : BaseTrait
{
	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x0600207E RID: 8318 RVA: 0x00066A61 File Offset: 0x00064C61
	public override TraitType TraitType
	{
		get
		{
			return TraitType.OmniDash;
		}
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x00066A68 File Offset: 0x00064C68
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().CharacterDash.EnableOmnidash = true;
		yield break;
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x00066A70 File Offset: 0x00064C70
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterDash.EnableOmnidash = false;
		}
	}
}
