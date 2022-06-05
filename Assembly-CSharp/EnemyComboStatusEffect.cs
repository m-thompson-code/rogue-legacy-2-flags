using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class EnemyComboStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D37 RID: 3383
	// (get) Token: 0x06001E72 RID: 7794 RVA: 0x00062E34 File Offset: 0x00061034
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Combo;
		}
	}

	// Token: 0x17000D38 RID: 3384
	// (get) Token: 0x06001E73 RID: 7795 RVA: 0x00062E3B File Offset: 0x0006103B
	public override float StartingDurationOverride
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000D39 RID: 3385
	// (get) Token: 0x06001E74 RID: 7796 RVA: 0x00062E44 File Offset: 0x00061044
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

	// Token: 0x06001E75 RID: 7797 RVA: 0x00062E84 File Offset: 0x00061084
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

	// Token: 0x06001E76 RID: 7798 RVA: 0x00062EB1 File Offset: 0x000610B1
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

	// Token: 0x04001BB4 RID: 7092
	public const float ADD_RELIC_STACKS_HACK = 3.4028235E+38f;

	// Token: 0x04001BB5 RID: 7093
	private bool m_addRelicStacks;
}
