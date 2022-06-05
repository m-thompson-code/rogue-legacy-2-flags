using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000314 RID: 788
public class SporeBurstStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D83 RID: 3459
	// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000640D2 File Offset: 0x000622D2
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_SporeBurst;
		}
	}

	// Token: 0x17000D84 RID: 3460
	// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000640D6 File Offset: 0x000622D6
	public override float StartingDurationOverride
	{
		get
		{
			return 1.25f;
		}
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x000640E0 File Offset: 0x000622E0
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#9AA633", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#3E4500", out this.m_addColorPulseOn);
		ColorUtility.TryParseHtmlString("#000000", out this.m_addColorPulseOff);
		base.AppliesTint = true;
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x0006412F File Offset: 0x0006232F
	public override void StartEffect(float duration, IDamageObj caster)
	{
		this.m_sporeEffectFired = false;
		if (!this.m_statusEffectController.HasStatusEffect(StatusEffectType.Enemy_SporeBurst))
		{
			base.StartEffect(duration, caster);
		}
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x0006414F File Offset: 0x0006234F
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

	// Token: 0x06001F30 RID: 7984 RVA: 0x00064165 File Offset: 0x00062365
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

	// Token: 0x06001F31 RID: 7985 RVA: 0x00064174 File Offset: 0x00062374
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

	// Token: 0x06001F32 RID: 7986 RVA: 0x00064184 File Offset: 0x00062384
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

	// Token: 0x04001BF9 RID: 7161
	private const string MULTIPLY_COLOR = "#9AA633";

	// Token: 0x04001BFA RID: 7162
	private const string PULSE_ON_COLOR = "#3E4500";

	// Token: 0x04001BFB RID: 7163
	private const string PULSE_OFF_COLOR = "#000000";

	// Token: 0x04001BFC RID: 7164
	private bool m_sporeEffectFired;

	// Token: 0x04001BFD RID: 7165
	private GenericEffect m_sporeEffect;

	// Token: 0x04001BFE RID: 7166
	private Color m_multiplyColor;

	// Token: 0x04001BFF RID: 7167
	private Color m_addColorPulseOn;

	// Token: 0x04001C00 RID: 7168
	private Color m_addColorPulseOff;

	// Token: 0x04001C01 RID: 7169
	private bool m_pulseOn;

	// Token: 0x04001C02 RID: 7170
	private float m_burnPulseRate = 0.1f;
}
