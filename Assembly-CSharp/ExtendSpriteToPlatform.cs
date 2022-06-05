using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class ExtendSpriteToPlatform : MonoBehaviour
{
	// Token: 0x06001562 RID: 5474 RVA: 0x000424AA File Offset: 0x000406AA
	private void Awake()
	{
		this.m_enemyController = base.GetComponent<EnemyController>();
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000424B8 File Offset: 0x000406B8
	private void OnEnable()
	{
		base.StartCoroutine(this.ExtendSprite());
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000424C7 File Offset: 0x000406C7
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

	// Token: 0x040014A5 RID: 5285
	[SerializeField]
	private SpriteRenderer m_spriteToExtend;

	// Token: 0x040014A6 RID: 5286
	[SerializeField]
	private bool m_collideWithOneWays;

	// Token: 0x040014A7 RID: 5287
	[SerializeField]
	private ExtendSpriteToPlatform.ExtendSpriteDirection m_extendSpriteDirection;

	// Token: 0x040014A8 RID: 5288
	private EnemyController m_enemyController;

	// Token: 0x02000B1A RID: 2842
	public enum ExtendSpriteDirection
	{
		// Token: 0x04004B57 RID: 19287
		None,
		// Token: 0x04004B58 RID: 19288
		Upward,
		// Token: 0x04004B59 RID: 19289
		Downward
	}
}
