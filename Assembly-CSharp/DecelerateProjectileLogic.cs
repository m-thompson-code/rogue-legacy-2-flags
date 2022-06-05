using System;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public class DecelerateProjectileLogic : BaseProjectileLogic
{
	// Token: 0x170015E3 RID: 5603
	// (get) Token: 0x06003B88 RID: 15240 RVA: 0x00009A7B File Offset: 0x00007C7B
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x00020B3C File Offset: 0x0001ED3C
	private void OnEnable()
	{
		this.m_firedDelay = Time.time + this.m_returnDelay;
		base.SourceProjectile.UpdateMovement();
		base.SourceProjectile.TurnSpeed = 0f;
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x000F4350 File Offset: 0x000F2550
	private void LateUpdate()
	{
		if (Time.time > this.m_firedDelay)
		{
			base.SourceProjectile.Speed -= this.m_decelerationSpeed * Time.deltaTime;
			if (base.SourceProjectile.Speed < 0f)
			{
				base.SourceProjectile.Speed = 0f;
			}
		}
	}

	// Token: 0x04002F46 RID: 12102
	[SerializeField]
	private float m_returnDelay = 0.15f;

	// Token: 0x04002F47 RID: 12103
	[SerializeField]
	private float m_decelerationSpeed = 15f;

	// Token: 0x04002F48 RID: 12104
	private float m_firedDelay;
}
