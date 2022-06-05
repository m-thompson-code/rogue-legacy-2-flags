using System;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
public class DestroyOnOneWayCollisionProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B96 RID: 15254 RVA: 0x00020BF3 File Offset: 0x0001EDF3
	private void Start()
	{
		this.m_collider = base.SourceProjectile.HitboxController.GetCollider(HitboxType.Terrain);
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x000F444C File Offset: 0x000F264C
	private void FixedUpdate()
	{
		RaycastHit2D hit = Physics2D.Raycast(this.m_collider.bounds.min, Vector2.right, this.m_collider.bounds.extents.x * 2f, LayerMask.GetMask(new string[]
		{
			"Platform_OneWay"
		}));
		if (hit)
		{
			bool flag = true;
			if (this.m_passThroughBreakables && (hit.collider.CompareTag("Breakable") || hit.collider.CompareTag("FlimsyBreakable")))
			{
				flag = false;
			}
			if (flag)
			{
				base.SourceProjectile.HitboxController.LastCollidedWith = hit.collider;
				base.SourceProjectile.PerformHitEffectCheck(null);
				base.SourceProjectile.FlagForDestruction(null);
			}
		}
	}

	// Token: 0x04002F50 RID: 12112
	[SerializeField]
	private bool m_passThroughBreakables;

	// Token: 0x04002F51 RID: 12113
	private Collider2D m_collider;
}
