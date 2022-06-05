using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class HorizontalDarkness_Trait : BaseTrait
{
	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x06002031 RID: 8241 RVA: 0x000664FB File Offset: 0x000646FB
	public override TraitType TraitType
	{
		get
		{
			return TraitType.HorizontalDarkness;
		}
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x00066502 File Offset: 0x00064702
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

	// Token: 0x06002033 RID: 8243 RVA: 0x00066514 File Offset: 0x00064714
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x0006657E File Offset: 0x0006477E
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		this.m_traitMask.gameObject.SetActive(false);
	}
}
