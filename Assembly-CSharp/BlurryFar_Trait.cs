using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class BlurryFar_Trait : BaseTrait
{
	// Token: 0x170011D4 RID: 4564
	// (get) Token: 0x06002C5E RID: 11358 RVA: 0x000189DB File Offset: 0x00016BDB
	public override TraitType TraitType
	{
		get
		{
			return TraitType.BlurryFar;
		}
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000189E2 File Offset: 0x00016BE2
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

	// Token: 0x06002C60 RID: 11360 RVA: 0x000189A5 File Offset: 0x00016BA5
	protected void OnDestroy()
	{
		if (this.m_traitMask)
		{
			UnityEngine.Object.Destroy(this.m_traitMask.gameObject);
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000C57C4 File Offset: 0x000C39C4
	public override void AssignGreenMask()
	{
		base.AssignGreenMask();
		this.m_postProcessOverrideController.Profile.OverrideBlurGreenChannel = true;
		this.m_postProcessOverrideController.Profile.BlurGreenChannel = this.m_postProcessOverrideController.Profile.BlurRedChannel;
		this.m_postProcessOverrideController.Profile.BlurRedChannel = 0f;
		this.m_postProcessOverrideController.Profile.OverrideBlurRedChannel = false;
	}
}
