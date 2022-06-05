using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
public class OneHitDeath_Trait : BaseTrait
{
	// Token: 0x1700124E RID: 4686
	// (get) Token: 0x06002DFA RID: 11770 RVA: 0x00017CC0 File Offset: 0x00015EC0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.OneHitDeath;
		}
	}

	// Token: 0x06002DFB RID: 11771 RVA: 0x0001937B File Offset: 0x0001757B
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().ResetHealth();
		yield break;
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x00019383 File Offset: 0x00017583
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().ResetHealth();
		}
	}
}
