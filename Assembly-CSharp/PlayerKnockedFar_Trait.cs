using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035C RID: 860
public class PlayerKnockedFar_Trait : BaseTrait
{
	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x06002088 RID: 8328 RVA: 0x00066ACA File Offset: 0x00064CCA
	public override TraitType TraitType
	{
		get
		{
			return TraitType.PlayerKnockedFar;
		}
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x00066ACE File Offset: 0x00064CCE
	private IEnumerator Start()
	{
		if (!PlayerManager.IsInstantiated)
		{
			yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		}
		PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 1f);
		yield break;
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x00066AD6 File Offset: 0x00064CD6
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().Animator.SetFloat("BoneStructureType", 0f);
		}
	}
}
