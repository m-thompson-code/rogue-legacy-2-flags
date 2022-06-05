using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052D RID: 1325
public class EnemySizeStatusEffect : BaseStatusEffect
{
	// Token: 0x1700113D RID: 4413
	// (get) Token: 0x06002AAB RID: 10923 RVA: 0x00017DE0 File Offset: 0x00015FE0
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Size;
		}
	}

	// Token: 0x1700113E RID: 4414
	// (get) Token: 0x06002AAC RID: 10924 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x00017DE7 File Offset: 0x00015FE7
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

	// Token: 0x06002AAE RID: 10926 RVA: 0x000C24F0 File Offset: 0x000C06F0
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
