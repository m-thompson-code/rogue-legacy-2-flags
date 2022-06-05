using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000518 RID: 1304
public class EnemyComboStatusEffect : BaseStatusEffect
{
	// Token: 0x1700110A RID: 4362
	// (get) Token: 0x06002A1D RID: 10781 RVA: 0x0001799D File Offset: 0x00015B9D
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Combo;
		}
	}

	// Token: 0x1700110B RID: 4363
	// (get) Token: 0x06002A1E RID: 10782 RVA: 0x00004536 File Offset: 0x00002736
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700110C RID: 4364
	// (get) Token: 0x06002A1F RID: 10783 RVA: 0x000C11B8 File Offset: 0x000BF3B8
	public override int StacksPerHit
	{
		get
		{
			int num = base.StacksPerHit;
			int level = SaveManager.PlayerSaveData.GetRelic(RelicType.WeaponsComboAdd).Level;
			if (level > 0)
			{
				if (this.m_addRelicStacks)
				{
					num += level;
				}
				else
				{
					num = level;
				}
			}
			return num;
		}
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000C11F8 File Offset: 0x000BF3F8
	public override void StartEffect(float duration, IDamageObj caster)
	{
		float duration2 = duration;
		if (duration == 3.4028235E+38f)
		{
			this.m_addRelicStacks = true;
			duration2 = this.StartingDurationOverride;
		}
		base.StartEffect(duration2, caster);
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000179A4 File Offset: 0x00015BA4
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		base.TimesStacked = Mathf.Clamp(base.TimesStacked, 0, 30);
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Combo, base.Duration, 30, base.TimesStacked);
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x04002440 RID: 9280
	public const float ADD_RELIC_STACKS_HACK = 3.4028235E+38f;

	// Token: 0x04002441 RID: 9281
	private bool m_addRelicStacks;
}
