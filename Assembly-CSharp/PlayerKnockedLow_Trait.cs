using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class PlayerKnockedLow_Trait : BaseTrait
{
	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x0600208C RID: 8332 RVA: 0x00066B00 File Offset: 0x00064D00
	public override TraitType TraitType
	{
		get
		{
			return TraitType.PlayerKnockedLow;
		}
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x00066B04 File Offset: 0x00064D04
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 2f);
		yield break;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x00066B0C File Offset: 0x00064D0C
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 0f);
		}
	}
}
