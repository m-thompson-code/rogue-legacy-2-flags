using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public class BonusMagicStrength_Trait : BaseTrait
{
	// Token: 0x170011DB RID: 4571
	// (get) Token: 0x06002C75 RID: 11381 RVA: 0x00018A4E File Offset: 0x00016C4E
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BonusMagicStrength;
		}
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x00018A55 File Offset: 0x00016C55
	public IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().InitializeMagicMods();
		yield break;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x00018A5D File Offset: 0x00016C5D
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().InitializeMagicMods();
		}
	}
}
