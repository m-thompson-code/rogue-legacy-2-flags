using System;
using System.Collections;

// Token: 0x02000798 RID: 1944
public class AxeSpellProjectileLogic : BaseProjectileLogic
{
	// Token: 0x06003B71 RID: 15217 RVA: 0x00020A87 File Offset: 0x0001EC87
	private void OnEnable()
	{
		if (base.SourceProjectile.CompareTag("PlayerProjectile"))
		{
			base.StartCoroutine(this.TriggerSkillCritCoroutine());
		}
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x00020AA8 File Offset: 0x0001ECA8
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
