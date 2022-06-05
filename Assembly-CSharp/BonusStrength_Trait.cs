using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class BonusStrength_Trait : BaseTrait
{
	// Token: 0x17000DAB RID: 3499
	// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0006538B File Offset: 0x0006358B
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusStrength;
		}
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x00065392 File Offset: 0x00063592
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().InitializeStrengthMods();
		yield break;
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x0006539A File Offset: 0x0006359A
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeStrengthMods();
		}
	}
}
