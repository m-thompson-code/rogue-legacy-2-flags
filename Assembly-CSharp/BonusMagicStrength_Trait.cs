using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class BonusMagicStrength_Trait : BaseTrait
{
	// Token: 0x17000DAA RID: 3498
	// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x00065361 File Offset: 0x00063561
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusMagicStrength;
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x00065368 File Offset: 0x00063568
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().InitializeMagicMods();
		yield break;
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x00065370 File Offset: 0x00063570
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeMagicMods();
		}
	}
}
