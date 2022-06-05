using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000586 RID: 1414
public class DarkScreen_Trait : BaseTrait
{
	// Token: 0x170011F4 RID: 4596
	// (get) Token: 0x06002CDB RID: 11483 RVA: 0x00017FA0 File Offset: 0x000161A0
	public override TraitType TraitType
	{
		get
		{
			return TraitType.DarkScreen;
		}
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x00018D31 File Offset: 0x00016F31
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

	// Token: 0x06002CDD RID: 11485 RVA: 0x000C53C4 File Offset: 0x000C35C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideTintGreenChannel = true;
		this.m_postProcessOverrideController.Profile.TintGreenChannel = this.m_postProcessOverrideController.Profile.TintRedChannel;
		this.m_postProcessOverrideController.Profile.TintRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideTintRedChannel = false;
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x00018D40 File Offset: 0x00016F40
	public override void DisableOnDeath()
	{
		base.DisableOnDeath();
		this.m_traitMask.gameObject.SetActive(false);
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x000189A5 File Offset: 0x00016BA5
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}
}
