using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class DelayHitboxProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B8C RID: 15244 RVA: 0x00020B89 File Offset: 0x0001ED89
	private void OnEnable()
	{
		base.StartCoroutine(this.DelayCoroutine());
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x00020B98 File Offset: 0x0001ED98
	private IEnumerator DelayCoroutine()
	{
		this.m_hitboxDisabled = true;
		base.SourceProjectile.HitboxController.SetHitboxActiveState(this.m_hitboxToDelay, false);
		float delay = Time.time + this.m_duration;
		while (Time.time < delay)
		{
			yield return null;
		}
		this.m_hitboxDisabled = false;
		base.SourceProjectile.HitboxController.SetHitboxActiveState(this.m_hitboxToDelay, true);
		yield break;
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x00020BA7 File Offset: 0x0001EDA7
	private void OnDisable()
	{
		if (this.m_hitboxDisabled && base.SourceProjectile)
		{
			base.SourceProjectile.HitboxController.SetHitboxActiveState(this.m_hitboxToDelay, true);
		}
		this.m_hitboxDisabled = false;
	}

	// Token: 0x04002F49 RID: 12105
	[SerializeField]
	private HitboxType m_hitboxToDelay;

	// Token: 0x04002F4A RID: 12106
	[SerializeField]
	private float m_duration;

	// Token: 0x04002F4B RID: 12107
	private bool m_hitboxDisabled;
}
