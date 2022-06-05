using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class EnemySizeStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D52 RID: 3410
	// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0006362E File Offset: 0x0006182E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Size;
		}
	}

	// Token: 0x17000D53 RID: 3411
	// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x00063635 File Offset: 0x00061835
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x0006363C File Offset: 0x0006183C
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Size);
		EnemyController enemyController = this.m_charController as EnemyController;
		if (enemyController)
		{
			enemyController.InitializeLevelMods();
			enemyController.UpdateEnemyDataScale();
			int num = Mathf.CeilToInt((float)(enemyController.BaseMaxHealth + enemyController.MaxHealthAdd + enemyController.MaxHealthTemporaryAdd) * 2f);
			enemyController.SetHealth((float)num, true, false);
		}
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x0006364C File Offset: 0x0006184C
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		EnemyController enemyController = this.m_charController as EnemyController;
		if (enemyController)
		{
			enemyController.UpdateEnemyDataScale();
		}
	}
}
