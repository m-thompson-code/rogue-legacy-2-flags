using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class DelayHitboxProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B3D RID: 11069 RVA: 0x000929EC File Offset: 0x00090BEC
	private void OnEnable()
	{
		base.StartCoroutine(this.DelayCoroutine());
	}

	// Token: 0x06002B3E RID: 11070 RVA: 0x000929FB File Offset: 0x00090BFB
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

	// Token: 0x06002B3F RID: 11071 RVA: 0x00092A0A File Offset: 0x00090C0A
	private void OnDisable()
	{
		if (this.m_hitboxDisabled && base.SourceProjectile)
		{
			base.SourceProjectile.HitboxController.SetHitboxActiveState(this.m_hitboxToDelay, true);
		}
		this.m_hitboxDisabled = false;
	}

	// Token: 0x04002332 RID: 9010
	[SerializeField]
	private HitboxType m_hitboxToDelay;

	// Token: 0x04002333 RID: 9011
	[SerializeField]
	private float m_duration;

	// Token: 0x04002334 RID: 9012
	private bool m_hitboxDisabled;
}
