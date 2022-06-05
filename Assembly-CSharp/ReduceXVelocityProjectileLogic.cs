using System;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public class ReduceXVelocityProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C07 RID: 15367 RVA: 0x00021223 File Offset: 0x0001F423
	private void OnEnable()
	{
		this.m_movingRight = (base.SourceProjectile.Velocity.x > 0f);
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x000F5810 File Offset: 0x000F3A10
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

	// Token: 0x04002FAA RID: 12202
	[SerializeField]
	private float m_xAccelerationReductionSpeed = 1f;

	// Token: 0x04002FAB RID: 12203
	private bool m_movingRight;
}
