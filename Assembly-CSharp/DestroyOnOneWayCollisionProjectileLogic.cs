using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class DestroyOnOneWayCollisionProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B41 RID: 11073 RVA: 0x00092A47 File Offset: 0x00090C47
	private void Start()
	{
		this.m_collider = base.SourceProjectile.HitboxController.GetCollider(HitboxType.Terrain);
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x00092A60 File Offset: 0x00090C60
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

	// Token: 0x04002335 RID: 9013
	[SerializeField]
	private bool m_passThroughBreakables;

	// Token: 0x04002336 RID: 9014
	private Collider2D m_collider;
}
