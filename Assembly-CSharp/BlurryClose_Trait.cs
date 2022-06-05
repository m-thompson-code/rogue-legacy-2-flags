using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class BlurryClose_Trait : BaseTrait
{
	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x000651D0 File Offset: 0x000633D0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurryClose;
		}
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000651D7 File Offset: 0x000633D7
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

	// Token: 0x06001FA2 RID: 8098 RVA: 0x000651E8 File Offset: 0x000633E8
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideBlurGreenChannel = true;
		this.m_postProcessOverrideController.Profile.BlurGreenChannel = this.m_postProcessOverrideController.Profile.BlurRedChannel;
		this.m_postProcessOverrideController.Profile.BlurRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideBlurRedChannel = false;
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x00065252 File Offset: 0x00063452
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}
}
