using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053C RID: 1340
public class ManaBurnStatusEffect : BaseStatusEffect
{
	// Token: 0x1700115C RID: 4444
	// (get) Token: 0x06002B04 RID: 11012 RVA: 0x000065B4 File Offset: 0x000047B4
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_ManaBurn;
		}
	}

	// Token: 0x1700115D RID: 4445
	// (get) Token: 0x06002B05 RID: 11013 RVA: 0x00018023 File Offset: 0x00016223
	public override float StartingDurationOverride
	{
		get
		{
			return 2.55f;
		}
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x0001802A File Offset: 0x0001622A
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		this.m_regenEventArgs = new ForceManaRegenEventArgs(0f, false);
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x00018045 File Offset: 0x00016245
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

	// Token: 0x06002B08 RID: 11016 RVA: 0x00018054 File Offset: 0x00016254
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

	// Token: 0x06002B09 RID: 11017 RVA: 0x00018063 File Offset: 0x00016263
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

	// Token: 0x06002B0A RID: 11018 RVA: 0x000C3334 File Offset: 0x000C1534
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

	// Token: 0x040024B7 RID: 9399
	private ForceManaRegenEventArgs m_regenEventArgs;

	// Token: 0x040024B8 RID: 9400
	private static Color m_addColorPulseOn = new Color(0f, 0.14f, 0.3f, 1f);

	// Token: 0x040024B9 RID: 9401
	private static Color m_addColorPulseOff = new Color(0f, 0f, 0f, 1f);

	// Token: 0x040024BA RID: 9402
	private bool m_pulseOn;

	// Token: 0x040024BB RID: 9403
	private BaseEffect m_manaBurnEffect;

	// Token: 0x040024BC RID: 9404
	private float m_manaBurnTicRemaining;
}
