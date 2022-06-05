using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class SmallHitbox_Trait : BaseTrait
{
	// Token: 0x1700125A RID: 4698
	// (get) Token: 0x06002E27 RID: 11815 RVA: 0x0001943C File Offset: 0x0001763C
	public override TraitType TraitType
	{
		get
		{
			return TraitType.SmallHitbox;
		}
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x00019443 File Offset: 0x00017643
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

	// Token: 0x06002E29 RID: 11817 RVA: 0x00019452 File Offset: 0x00017652
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed && PlayerManager.GetPlayerController())
		{
			PlayerManager.GetPlayerController().BlinkPulseEffect.DisableBlackFill(BlackFillType.SmallHitbox_Trait, 0f);
		}
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x000C78C8 File Offset: 0x000C5AC8
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

	// Token: 0x040025F5 RID: 9717
	[SerializeField]
	private GameObject m_traitSprite;

	// Token: 0x040025F6 RID: 9718
	private Vector2 m_storedBodyColliderSize;

	// Token: 0x040025F7 RID: 9719
	private bool m_traitInitialized;
}
