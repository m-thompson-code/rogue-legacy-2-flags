using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000575 RID: 1397
public class BonusStrength_Trait : BaseTrait
{
	// Token: 0x170011DE RID: 4574
	// (get) Token: 0x06002C82 RID: 11394 RVA: 0x00018A93 File Offset: 0x00016C93
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusStrength;
		}
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x00018A9A File Offset: 0x00016C9A
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().InitializeStrengthMods();
		yield break;
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x00018AA2 File Offset: 0x00016CA2
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeStrengthMods();
		}
	}
}
