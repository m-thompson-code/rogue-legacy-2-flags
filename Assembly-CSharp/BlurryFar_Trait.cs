using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000323 RID: 803
public class BlurryFar_Trait : BaseTrait
{
	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x00065279 File Offset: 0x00063479
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurryFar;
		}
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00065280 File Offset: 0x00063480
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
		this.m_traitMask.transform.localScale = new Vector3(16f, 16f, 1f);
		yield break;
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x0006528F File Offset: 0x0006348F
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000652B0 File Offset: 0x000634B0
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideBlurGreenChannel = true;
		this.m_postProcessOverrideController.Profile.BlurGreenChannel = this.m_postProcessOverrideController.Profile.BlurRedChannel;
		this.m_postProcessOverrideController.Profile.BlurRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideBlurRedChannel = false;
	}
}
