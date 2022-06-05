using System;
using System.Collections;

// Token: 0x02000495 RID: 1173
public class AxeSpellProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06002B28 RID: 11048 RVA: 0x000923F9 File Offset: 0x000905F9
	private void OnEnable()
	{
		if (base.SourceProjectile.CompareTag("PlayerProjectile"))
		{
			base.StartCoroutine(this.TriggerSkillCritCoroutine());
		}
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x0009241A File Offset: 0x0009061A
	private IEnumerator TriggerSkillCritCoroutine()
	{
		yield return null;
		while (base.SourceProjectile.Velocity.y >= 0f)
		{
			yield return null;
		}
		if (base.SourceProjectile.ActualCritChance < 100f)
		{
			base.SourceProjectile.ActualCritChance += 100f;
		}
		yield break;
	}
}
