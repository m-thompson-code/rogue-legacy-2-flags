using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005CD RID: 1485
public class PlayerKnockedFar_Trait : BaseTrait
{
	// Token: 0x17001252 RID: 4690
	// (get) Token: 0x06002E09 RID: 11785 RVA: 0x000193B9 File Offset: 0x000175B9
	public override TraitType TraitType
	{
		get
		{
			return TraitType.PlayerKnockedFar;
		}
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x000193BD File Offset: 0x000175BD
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 1f);
		yield break;
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x000193C5 File Offset: 0x000175C5
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 0f);
		}
	}
}
