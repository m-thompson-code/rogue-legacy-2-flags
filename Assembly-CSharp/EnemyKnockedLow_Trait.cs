using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class EnemyKnockedLow_Trait : BaseTrait
{
	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x06001FFF RID: 8191 RVA: 0x000660B5 File Offset: 0x000642B5
	public override TraitType TraitType
	{
		get
		{
			return TraitType.EnemyKnockedLow;
		}
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000660BC File Offset: 0x000642BC
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 1f);
		yield break;
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000660C4 File Offset: 0x000642C4
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("LimbType", 0f);
		}
	}
}
