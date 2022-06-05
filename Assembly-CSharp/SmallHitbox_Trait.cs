using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class SmallHitbox_Trait : BaseTrait
{
	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x06002094 RID: 8340 RVA: 0x00066B51 File Offset: 0x00064D51
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SmallHitbox;
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x00066B58 File Offset: 0x00064D58
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.ControllerCorgi.SetRaysParameters();
		Vector3 position = new Vector3(playerController.Midpoint.x, playerController.Midpoint.y, this.m_traitSprite.transform.localPosition.z);
		this.m_traitSprite.transform.position = position;
		this.m_traitSprite.transform.SetParent(playerController.Visuals.transform, true);
		BoxCollider2D boxCollider2D = (BoxCollider2D)playerController.HitboxController.GetCollider(HitboxType.Body);
		this.m_storedBodyColliderSize = boxCollider2D.size;
		boxCollider2D.size = this.m_storedBodyColliderSize * 0.1f;
		playerController.InitializeHealthMods();
		playerController.ResetHealth();
		playerController.BlinkPulseEffect.ActivateBlackFill(BlackFillType.SmallHitbox_Trait, 0f);
		this.m_traitInitialized = true;
		yield break;
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x00066B67 File Offset: 0x00064D67
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed && PlayerManager.GetPlayerController())
		{
			PlayerManager.GetPlayerController().BlinkPulseEffect.DisableBlackFill(BlackFillType.SmallHitbox_Trait, 0f);
		}
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x00066B94 File Offset: 0x00064D94
	private void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(this.m_traitSprite);
		if (!PlayerManager.IsDisposed && this.m_traitInitialized)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			playerController.RecreateRendererArray();
			((BoxCollider2D)playerController.HitboxController.GetCollider(HitboxType.Body)).size = this.m_storedBodyColliderSize;
		}
	}

	// Token: 0x04001C5E RID: 7262
	[SerializeField]
	private GameObject m_traitSprite;

	// Token: 0x04001C5F RID: 7263
	private Vector2 m_storedBodyColliderSize;

	// Token: 0x04001C60 RID: 7264
	private bool m_traitInitialized;
}
