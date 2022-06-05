using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class OneHitDeath_Trait : BaseTrait
{
	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x06002082 RID: 8322 RVA: 0x00066A91 File Offset: 0x00064C91
	public override TraitType TraitType
	{
		get
		{
			return TraitType.OneHitDeath;
		}
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x00066A98 File Offset: 0x00064C98
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().ResetHealth();
		yield break;
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x00066AA0 File Offset: 0x00064CA0
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().ResetHealth();
		}
	}
}
