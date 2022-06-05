using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class RemoveGuaranteedCritAfterHitProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B97 RID: 11159 RVA: 0x000942B8 File Offset: 0x000924B8
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.DisableGuaranteedCrit), false);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000942D8 File Offset: 0x000924D8
	private void DisableGuaranteedCrit(Projectile_RL projectile, GameObject obj)
	{
		base.StartCoroutine(this.DisableGuaranteedCritCoroutine());
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000942E7 File Offset: 0x000924E7
	private IEnumerator DisableGuaranteedCritCoroutine()
	{
		yield return null;
		if (base.SourceProjectile.ActualCritChance >= 100f)
		{
			base.SourceProjectile.ActualCritChance -= 100f;
		}
		yield break;
	}
}
