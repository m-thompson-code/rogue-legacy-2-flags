using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class DecelerateProjectileLogic : BaseProjectileLogic
{
	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06002B39 RID: 11065 RVA: 0x0009293D File Offset: 0x00090B3D
	public string Description
	{
		get
		{
			return this.ToString();
		}
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x00092945 File Offset: 0x00090B45
	private void OnEnable()
	{
		this.m_firedDelay = Time.time + this.m_returnDelay;
		base.SourceProjectile.UpdateMovement();
		base.SourceProjectile.TurnSpeed = 0f;
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x00092974 File Offset: 0x00090B74
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

	// Token: 0x0400232F RID: 9007
	[SerializeField]
	private float m_returnDelay = 0.15f;

	// Token: 0x04002330 RID: 9008
	[SerializeField]
	private float m_decelerationSpeed = 15f;

	// Token: 0x04002331 RID: 9009
	private float m_firedDelay;
}
