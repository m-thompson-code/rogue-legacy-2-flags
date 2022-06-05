using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class ManaBurnStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D61 RID: 3425
	// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x00063B31 File Offset: 0x00061D31
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ManaBurn;
		}
	}

	// Token: 0x17000D62 RID: 3426
	// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x00063B35 File Offset: 0x00061D35
	public override float StartingDurationOverride
	{
		get
		{
			return 2.55f;
		}
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x00063B3C File Offset: 0x00061D3C
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		this.m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x00063B57 File Offset: 0x00061D57
	private IEnumerator PulseCoroutine()
	{
		for (;;)
		{
			this.m_waitYield.CreateNew(0.1f, false);
			yield return this.m_waitYield;
			if (this.m_pulseOn)
			{
				using (List<Renderer>.Enumerator enumerator = this.m_charController.RendererArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Renderer renderer = enumerator.Current;
						renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
						BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, ManaBurnStatusEffect.m_addColorPulseOn);
						renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					}
					goto IL_10B;
				}
				goto IL_AF;
			}
			goto IL_AF;
			IL_10B:
			this.m_pulseOn = !this.m_pulseOn;
			continue;
			IL_AF:
			foreach (Renderer renderer2 in this.m_charController.RendererArray)
			{
				renderer2.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, ManaBurnStatusEffect.m_addColorPulseOff);
				renderer2.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			}
			goto IL_10B;
		}
		yield break;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x00063B66 File Offset: 0x00061D66
	private IEnumerator BurnManaCoroutine()
	{
		for (;;)
		{
			float delayTime = Time.time + 0.1f - this.m_manaBurnTicRemaining;
			while (Time.time < delayTime)
			{
				this.m_manaBurnTicRemaining = 0.1f - (delayTime - Time.time);
				yield return null;
			}
			this.m_manaBurnTicRemaining = 0f;
			this.m_regenEventArgs.Initialise(1f, false);
			EffectManager.PlayEffect(this.m_charController.gameObject, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
		}
		yield break;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x00063B75 File Offset: 0x00061D75
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.ManaBurn, base.Duration);
		this.m_manaBurnEffect = EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemyBurnBlueFlames_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.m_manaBurnEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_manaBurnEffect.transform.position = this.m_charController.Midpoint;
		base.StartCoroutine(this.BurnManaCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x00063B84 File Offset: 0x00061D84
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			this.m_manaBurnTicRemaining = 0f;
			if (this.m_charController.IsDead)
			{
				this.m_regenEventArgs.Initialise(20f, false);
				EffectManager.PlayEffect(this.m_charController.gameObject, null, "ManaRegenBurst_Effect", Vector3.zero, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerForceManaRegen, this, this.m_regenEventArgs);
			}
		}
		if (this.m_manaBurnEffect && this.m_manaBurnEffect.isActiveAndEnabled)
		{
			this.m_manaBurnEffect.Stop(EffectStopType.Gracefully);
		}
	}

	// Token: 0x04001BE2 RID: 7138
	private ForceManaRegenEventArgs m_regenEventArgs;

	// Token: 0x04001BE3 RID: 7139
	private static Color m_addColorPulseOn = new Color(0f, 0.14f, 0.3f, 1f);

	// Token: 0x04001BE4 RID: 7140
	private static Color m_addColorPulseOff = new Color(0f, 0f, 0f, 1f);

	// Token: 0x04001BE5 RID: 7141
	private bool m_pulseOn;

	// Token: 0x04001BE6 RID: 7142
	private BaseEffect m_manaBurnEffect;

	// Token: 0x04001BE7 RID: 7143
	private float m_manaBurnTicRemaining;
}
