using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class ExtendSpriteToPlatform : MonoBehaviour
{
	// Token: 0x06001EAD RID: 7853 RVA: 0x000100D4 File Offset: 0x0000E2D4
	private void Awake()
	{
		this.m_enemyController = base.GetComponent<EnemyController>();
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x000100E2 File Offset: 0x0000E2E2
	private void OnEnable()
	{
		base.StartCoroutine(this.ExtendSprite());
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000100F1 File Offset: 0x0000E2F1
	private IEnumerator ExtendSprite()
	{
		this.m_spriteToExtend.size = new Vector2(this.m_spriteToExtend.size.x, 0f);
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		while (!this.m_enemyController.IsInitialized)
		{
			yield return null;
		}
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		LayerMask mask = playerController.ControllerCorgi.SavedPlatformMask;
		if (this.m_collideWithOneWays)
		{
			mask |= playerController.ControllerCorgi.OneWayPlatformMask;
		}
		Vector2 vector = Vector2.zero;
		ExtendSpriteToPlatform.ExtendSpriteDirection extendSpriteDirection = this.m_extendSpriteDirection;
		if (extendSpriteDirection != ExtendSpriteToPlatform.ExtendSpriteDirection.Upward)
		{
			if (extendSpriteDirection == ExtendSpriteToPlatform.ExtendSpriteDirection.Downward)
			{
				vector = Vector2.down;
			}
		}
		else
		{
			vector = Vector2.up;
		}
		RaycastHit2D hit = Physics2D.Raycast(base.transform.position, vector, 20f, mask);
		if (hit)
		{
			Vector2 size = this.m_spriteToExtend.size;
			size.y = hit.distance * vector.y;
			this.m_spriteToExtend.size = size;
		}
		yield break;
	}

	// Token: 0x04001B66 RID: 7014
	[SerializeField]
	private SpriteRenderer m_spriteToExtend;

	// Token: 0x04001B67 RID: 7015
	[SerializeField]
	private bool m_collideWithOneWays;

	// Token: 0x04001B68 RID: 7016
	[SerializeField]
	private ExtendSpriteToPlatform.ExtendSpriteDirection m_extendSpriteDirection;

	// Token: 0x04001B69 RID: 7017
	private EnemyController m_enemyController;

	// Token: 0x0200039A RID: 922
	public enum ExtendSpriteDirection
	{
		// Token: 0x04001B6B RID: 7019
		None,
		// Token: 0x04001B6C RID: 7020
		Upward,
		// Token: 0x04001B6D RID: 7021
		Downward
	}
}
