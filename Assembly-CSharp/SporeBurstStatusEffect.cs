using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class SporeBurstStatusEffect : BaseStatusEffect
{
	// Token: 0x17001196 RID: 4502
	// (get) Token: 0x06002B96 RID: 11158 RVA: 0x0000452B File Offset: 0x0000272B
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_SporeBurst;
		}
	}

	// Token: 0x17001197 RID: 4503
	// (get) Token: 0x06002B97 RID: 11159 RVA: 0x00003DAB File Offset: 0x00001FAB
	public override float StartingDurationOverride
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000C3FD4 File Offset: 0x000C21D4
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#9AA633", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#3E4500", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x00018473 File Offset: 0x00016673
	public override void StartEffect(float duration, IDamageObj caster)
	{
		this.m_sporeEffectFired = false;
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_SporeBurst))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x00018493 File Offset: 0x00016693
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.SporeBurst, base.Duration);
		base.StartCoroutine(this.SporeEffectCoroutine());
		this.m_pulseOn = false;
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		base.StartCoroutine(this.PulseCoroutine());
		while (Time.time < base.EndTime)
		{
			yield return null;
		}
		Projectile_RL projectile_RL = ProjectileManager.FireProjectile(PlayerManager.GetPlayerController().gameObject, "SporeBurstProjectile", this.m_charController.Midpoint, false, 0f, 1f, true, true, true, true);
		Projectile_RL component = caster.gameObject.GetComponent<Projectile_RL>();
		if (component)
		{
			projectile_RL.CastAbilityType = component.CastAbilityType;
		}
		this.m_sporeEffectFired = true;
		if (this.m_sporeEffect != null && this.m_sporeEffect.isActiveAndEnabled)
		{
			this.m_sporeEffect.Animator.SetFloat("tellPercent", 1f);
		}
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000184A9 File Offset: 0x000166A9
	private IEnumerator SporeEffectCoroutine()
	{
		float startTime = Time.time;
		this.m_sporeEffect = (EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "EnemySpores_Effect", Vector3.zero, base.Duration, EffectStopType.Gracefully, EffectTriggerDirection.None) as GenericEffect);
		this.m_sporeEffect.transform.SetParent(this.m_charController.transform, false);
		this.m_sporeEffect.transform.position = this.m_charController.Midpoint;
		while (!this.m_sporeEffectFired)
		{
			float value = (Time.time - startTime) / base.Duration;
			if (this.m_sporeEffect && this.m_sporeEffect.isActiveAndEnabled)
			{
				this.m_sporeEffect.Animator.SetFloat("tellPercent", value);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000184B8 File Offset: 0x000166B8
	private IEnumerator PulseCoroutine()
	{
		for (;;)
		{
			this.m_waitYield.CreateNew(this.m_burnPulseRate, false);
			yield return this.m_waitYield;
			if (this.m_pulseOn)
			{
				using (List<Renderer>.Enumerator enumerator = this.m_charController.RendererArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Renderer renderer = enumerator.Current;
						renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
						BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOn);
						renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
					}
					goto IL_10E;
				}
				goto IL_B1;
			}
			goto IL_B1;
			IL_10E:
			this.m_pulseOn = !this.m_pulseOn;
			continue;
			IL_B1:
			foreach (Renderer renderer2 in this.m_charController.RendererArray)
			{
				renderer2.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
				BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColorPulseOff);
				renderer2.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			}
			goto IL_10E;
		}
		yield break;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000C4024 File Offset: 0x000C2224
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (!interrupted)
		{
			if (this.m_sporeEffect && this.m_sporeEffect.isActiveAndEnabled)
			{
				this.m_sporeEffect.Animator.SetFloat("tellPercent", 1f);
			}
			if (this.m_charController.IsDead && !this.m_sporeEffectFired && PlayerManager.IsInstantiated)
			{
				ProjectileManager.FireProjectile(PlayerManager.GetPlayerController().gameObject, "SporeBurstProjectile", this.m_charController.Midpoint, false, 0f, 1f, true, true, true, true);
				return;
			}
		}
		else if (this.m_sporeEffect && this.m_sporeEffect.isActiveAndEnabled)
		{
			this.m_sporeEffect.Stop(EffectStopType.Immediate);
		}
	}

	// Token: 0x040024F8 RID: 9464
	private const string MULTIPLY_COLOR = "#9AA633";

	// Token: 0x040024F9 RID: 9465
	private const string PULSE_ON_COLOR = "#3E4500";

	// Token: 0x040024FA RID: 9466
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x040024FB RID: 9467
	private bool m_sporeEffectFired;

	// Token: 0x040024FC RID: 9468
	private GenericEffect m_sporeEffect;

	// Token: 0x040024FD RID: 9469
	private Color m_multiplyColor;

	// Token: 0x040024FE RID: 9470
	private Color m_addColorPulseOn;

	// Token: 0x040024FF RID: 9471
	private Color m_addColorPulseOff;

	// Token: 0x04002500 RID: 9472
	private bool m_pulseOn;

	// Token: 0x04002501 RID: 9473
	private float m_burnPulseRate = 0.1f;
}
