using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class ReduceXVelocityProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B94 RID: 11156 RVA: 0x0009411C File Offset: 0x0009231C
	private void OnEnable()
	{
		this.m_movingRight = (base.SourceProjectile.Velocity.x > 0f);
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x0009413C File Offset: 0x0009233C
	private void LateUpdate()
	{
		if (this.m_movingRight)
		{
			if (base.SourceProjectile.Velocity.x > 0f)
			{
				base.SourceProjectile.HeadingX -= this.m_xAccelerationReductionSpeed * Time.deltaTime;
				base.SourceProjectile.SetCorgiVelocity(new Vector2(base.SourceProjectile.Velocity.x - this.m_xAccelerationReductionSpeed * Time.deltaTime, base.SourceProjectile.Velocity.y));
				return;
			}
			base.SourceProjectile.HeadingX = 0f;
			base.SourceProjectile.SetCorgiVelocity(new Vector2(0f, base.SourceProjectile.Velocity.y));
			return;
		}
		else
		{
			if (base.SourceProjectile.Velocity.x < 0f)
			{
				base.SourceProjectile.HeadingX += this.m_xAccelerationReductionSpeed * Time.deltaTime;
				base.SourceProjectile.SetCorgiVelocity(new Vector2(base.SourceProjectile.Velocity.x + this.m_xAccelerationReductionSpeed * Time.deltaTime, base.SourceProjectile.Velocity.y));
				return;
			}
			base.SourceProjectile.HeadingX = 0f;
			base.SourceProjectile.SetCorgiVelocity(new Vector2(0f, base.SourceProjectile.Velocity.y));
			return;
		}
	}

	// Token: 0x04002370 RID: 9072
	[SerializeField]
	private float m_xAccelerationReductionSpeed = 1f;

	// Token: 0x04002371 RID: 9073
	private bool m_movingRight;
}
