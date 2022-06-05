using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class GainDownStrike_Trait : BaseTrait
{
	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x0600201F RID: 8223 RVA: 0x00066380 File Offset: 0x00064580
	public override TraitType TraitType
	{
		get
		{
			return TraitType.GainDownStrike;
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x00066387 File Offset: 0x00064587
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().CharacterDownStrike.SpinKickInstead = false;
		yield break;
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x0006638F File Offset: 0x0006458F
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().CharacterDownStrike.SpinKickInstead = true;
		}
	}
}
