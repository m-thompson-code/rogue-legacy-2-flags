using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class DarkScreen_Trait : BaseTrait
{
	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x00065B63 File Offset: 0x00063D63
	public override TraitType TraitType
	{
		get
		{
			return TraitType.DarkScreen;
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x00065B67 File Offset: 0x00063D67
	protected IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		this.m_traitMask.transform.position = PlayerManager.GetPlayerController().Midpoint;
		this.m_traitMask.transform.SetParent(PlayerManager.GetPlayerController().transform, true);
		this.m_traitMask.transform.localScale = new Vector3(31f, 31f, 1f);
		yield break;
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x00065B78 File Offset: 0x00063D78
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x00065BE2 File Offset: 0x00063DE2
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		this.m_traitMask.gameObject.SetActive(false);
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x00065BFB File Offset: 0x00063DFB
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}
}
