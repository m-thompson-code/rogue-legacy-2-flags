using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class EnemyKnockedFar_Trait : BaseTrait
{
	// Token: 0x17001200 RID: 4608
	// (get) Token: 0x06002D0D RID: 11533 RVA: 0x00018E60 File Offset: 0x00017060
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemyKnockedFar;
		}
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x00018E67 File Offset: 0x00017067
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 2f);
		yield break;
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x00018E6F File Offset: 0x0001706F
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 0f);
		}
	}
}
