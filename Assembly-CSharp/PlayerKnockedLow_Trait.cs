using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class PlayerKnockedLow_Trait : BaseTrait
{
	// Token: 0x17001255 RID: 4693
	// (get) Token: 0x06002E16 RID: 11798 RVA: 0x00006CB3 File Offset: 0x00004EB3
	public override TraitType TraitType
	{
		get
		{
			return TraitType.PlayerKnockedLow;
		}
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x0001940A File Offset: 0x0001760A
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 2f);
		yield break;
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x000193C5 File Offset: 0x000175C5
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 0f);
		}
	}
}
