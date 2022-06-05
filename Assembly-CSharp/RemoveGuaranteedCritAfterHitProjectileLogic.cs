using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
public class RemoveGuaranteedCritAfterHitProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003C0A RID: 15370 RVA: 0x00021255 File Offset: 0x0001F455
	private void OnEnable()
	{
		base.SourceProjectile.OnCollisionRelay.AddListener(new Action<Projectile_RL, GameObject>(this.DisableGuaranteedCrit), false);
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00021275 File Offset: 0x0001F475
	private void DisableGuaranteedCrit(Projectile_RL projectile, GameObject obj)
	{
		base.StartCoroutine(this.DisableGuaranteedCritCoroutine());
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00021284 File Offset: 0x0001F484
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
