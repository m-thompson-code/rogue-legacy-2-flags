using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class EnemyKnockedLow_Trait : BaseTrait
{
	// Token: 0x17001203 RID: 4611
	// (get) Token: 0x06002D1A RID: 11546 RVA: 0x00017799 File Offset: 0x00015999
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemyKnockedLow;
		}
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x00018EB4 File Offset: 0x000170B4
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 1f);
		yield break;
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x00018E6F File Offset: 0x0001706F
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 0f);
		}
	}
}
