using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class HorizontalDarkness_Trait : BaseTrait
{
	// Token: 0x17001219 RID: 4633
	// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000190DE File Offset: 0x000172DE
	public override TraitType TraitType
	{
		get
		{
			return TraitType.HorizontalDarkness;
		}
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x000190E5 File Offset: 0x000172E5
	protected IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		float y = playerController.Midpoint.y - playerController.transform.position.y;
		this.m_traitMask.transform.SetParent(playerController.transform);
		this.m_traitMask.transform.localPosition = new Vector3(0f, y, 0f);
		Vector3 localScale = this.m_traitMask.transform.localScale;
		localScale.y = 6f;
		this.m_traitMask.transform.localScale = localScale;
		yield break;
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x000C53C4 File Offset: 0x000C35C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x00018D40 File Offset: 0x00016F40
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		this.m_traitMask.gameObject.SetActive(false);
	}
}
