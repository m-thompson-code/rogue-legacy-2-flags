using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056B RID: 1387
public class BlurryClose_Trait : BaseTrait
{
	// Token: 0x170011D1 RID: 4561
	// (get) Token: 0x06002C53 RID: 11347 RVA: 0x0001898F File Offset: 0x00016B8F
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurryClose;
		}
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x00018996 File Offset: 0x00016B96
	protected IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_traitMask.transform.SetParent(playerController.transform);
		Vector3 zero = Vector3.zero;
		zero.y = playerController.Midpoint.y - playerController.transform.position.y;
		this.m_traitMask.transform.localPosition = zero;
		this.m_traitMask.transform.localScale = new Vector3(20f, 20f, 1f);
		yield break;
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000C57C4 File Offset: 0x000C39C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideBlurGreenChannel = true;
		this.m_postProcessOverrideController.Profile.BlurGreenChannel = this.m_postProcessOverrideController.Profile.BlurRedChannel;
		this.m_postProcessOverrideController.Profile.BlurRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideBlurRedChannel = false;
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000189A5 File Offset: 0x00016BA5
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}
}
