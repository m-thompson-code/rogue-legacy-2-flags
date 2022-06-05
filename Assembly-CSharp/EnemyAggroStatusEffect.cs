using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class EnemyAggroStatusEffect : BaseStatusEffect
{
	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x060029F4 RID: 10740 RVA: 0x00017831 File Offset: 0x00015A31
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Aggro;
		}
	}

	// Token: 0x170010FE RID: 4350
	// (get) Token: 0x060029F5 RID: 10741 RVA: 0x00017838 File Offset: 0x00015A38
	public override float StartingDurationOverride
	{
		get
		{
			return float.MaxValue;
		}
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x0001783F File Offset: 0x00015A3F
	protected override void Awake()
	{
		base.Awake();
		this.m_onEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnEnemyHit);
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x00017859 File Offset: 0x00015A59
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

	// Token: 0x060029F8 RID: 10744 RVA: 0x00017868 File Offset: 0x00015A68
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x0001788D File Offset: 0x00015A8D
	private void OnEnemyHit(object sender, CharacterHitEventArgs args)
	{
		if (args != null && args.Attacker.BaseDamage > 0f)
		{
			this.m_charController.StatusEffectController.StartStatusEffect(StatusEffectType.Enemy_Aggro, 0f, null);
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000178BF File Offset: 0x00015ABF
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onEnemyHit);
		}
	}

	// Token: 0x04002431 RID: 9265
	private Action<object, CharacterHitEventArgs> m_onEnemyHit;
}
