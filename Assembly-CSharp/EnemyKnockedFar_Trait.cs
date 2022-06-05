using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class EnemyKnockedFar_Trait : BaseTrait
{
	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x06001FFB RID: 8187 RVA: 0x0006607C File Offset: 0x0006427C
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemyKnockedFar;
		}
	}

	// Token: 0x06001FFC RID: 8188 RVA: 0x00066083 File Offset: 0x00064283
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 2f);
		yield break;
	}

	// Token: 0x06001FFD RID: 8189 RVA: 0x0006608B File Offset: 0x0006428B
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 0f);
		}
	}
}
