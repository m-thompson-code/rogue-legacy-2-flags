using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class EnemyAggroStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D30 RID: 3376
	// (get) Token: 0x06001E5B RID: 7771 RVA: 0x00062A0E File Offset: 0x00060C0E
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Aggro;
		}
	}

	// Token: 0x17000D31 RID: 3377
	// (get) Token: 0x06001E5C RID: 7772 RVA: 0x00062A15 File Offset: 0x00060C15
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x00062A1C File Offset: 0x00060C1C
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x00062A36 File Offset: 0x00060C36
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onEnemyHit, false);
		base.TimesStacked = Mathf.Min(base.TimesStacked, 4);
		if (base.TimesStacked <= 1)
		{
			this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Aggro);
		}
		else
		{
			for (;;)
			{
				float delay = Time.time + 2.75f;
				this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Aggro, 2.75f, 3, base.TimesStacked - 1);
				while (Time.time < delay)
				{
					yield return null;
				}
				base.TimesStacked = 1;
				if (base.TimesStacked <= 1)
				{
					break;
				}
				yield return null;
			}
			this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Aggro);
		}
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x00062A45 File Offset: 0x00060C45
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x00062A6A File Offset: 0x00060C6A
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (args != null && args.Attacker.BaseDamage > 0f)
		{
			this.m_charController.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_Aggro, 0f, null);
		}
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x00062A9C File Offset: 0x00060C9C
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04001BB1 RID: 7089
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
