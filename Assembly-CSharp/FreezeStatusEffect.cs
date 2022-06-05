using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000533 RID: 1331
public class FreezeStatusEffect : BaseStatusEffect
{
	// Token: 0x17001149 RID: 4425
	// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000046FA File Offset: 0x000028FA
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Freeze;
		}
	}

	// Token: 0x1700114A RID: 4426
	// (get) Token: 0x06002ACC RID: 10956 RVA: 0x00003C54 File Offset: 0x00001E54
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x000C26C0 File Offset: 0x000C08C0
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#9CE6FF", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#7382FF", out this.m_addColor);
		base.AppliesTint = true;
		this.m_onFrozenEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnFrozenEnemyHit);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x00017E74 File Offset: 0x00016074
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x00017E7E File Offset: 0x0001607E
	protected override IEnumerator StartEffectCoroutine(IDamageObj caster, bool justCasted)
	{
		this.m_shakeStarted = false;
		if (this.m_shakeCoroutine != null)
		{
			base.StopCoroutine(this.m_shakeCoroutine);
		}
		this.m_charController.StatusBarController.ApplyUIEffect(StatusBarEntryType.Freeze, base.Duration);
		this.m_unbreakableFreezeTime = Time.time + 0.25f;
		this.m_storedKnockback = this.m_charController.BaseKnockbackDefense;
		this.m_charController.BaseKnockbackDefense = this.m_storedKnockback + 0f;
		this.m_hasGravity = this.m_charController.ControllerCorgi.IsGravityActive;
		this.m_charController.CharacterCorgi.Freeze();
		this.m_charController.Animator.enabled = false;
		this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
		EnemyController enemyController = this.m_charController as EnemyController;
		if (enemyController)
		{
			if (enemyController.IsBoss)
			{
				base.Duration *= 0.15f;
			}
			enemyController.LogicController.Pause();
		}
		foreach (Renderer renderer in this.m_charController.RendererArray)
		{
			renderer.GetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._MultiplyColor, this.m_multiplyColor);
			BaseStatusEffect.m_matBlockHelper_STATIC.SetColor(ShaderID_RL._AddColor, this.m_addColor);
			renderer.SetPropertyBlock(BaseStatusEffect.m_matBlockHelper_STATIC);
		}
		yield return null;
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.AddListener(this.m_onFrozenEnemyHit, false);
		while (Time.time < base.EndTime)
		{
			if (!this.m_shakeStarted && Time.time > base.EndTime - this.m_shakeDuration)
			{
				this.m_shakeStarted = true;
				this.m_shakeCoroutine = base.StartCoroutine(this.ShakeCoroutine());
			}
			yield return null;
		}
		this.m_statusEffectController.StartStatusEffect(StatusEffectType.Enemy_FreezeImmunity, 0f, null);
		EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "FreezeThawParticles_Effect", this.m_charController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		this.StopEffect(false);
		yield break;
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x00017E8D File Offset: 0x0001608D
	private IEnumerator ShakeCoroutine()
	{
		this.m_storedVisualsPos = this.m_charController.Visuals.transform.localPosition;
		float startTime = Time.time;
		for (;;)
		{
			if (Time.time > startTime + this.m_shakeInterval)
			{
				startTime = Time.time;
				Vector3 storedVisualsPos = this.m_storedVisualsPos;
				float num = UnityEngine.Random.Range(this.m_shakeAmountMin, this.m_shakeAmountMax);
				num *= (float)CDGHelper.RandomPlusMinus();
				float num2 = UnityEngine.Random.Range(this.m_shakeAmountMin, this.m_shakeAmountMax);
				num2 *= (float)CDGHelper.RandomPlusMinus();
				storedVisualsPos.x += num;
				storedVisualsPos.y += num2;
				this.m_charController.Visuals.transform.localPosition = storedVisualsPos;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000C2710 File Offset: 0x000C0910
	public override void StopEffect(bool interrupted = false)
	{
		base.StopEffect(interrupted);
		if (this.m_shakeCoroutine != null)
		{
			base.StopCoroutine(this.m_shakeCoroutine);
		}
		if (this.m_shakeStarted)
		{
			this.m_shakeStarted = false;
			this.m_charController.Visuals.transform.localPosition = this.m_storedVisualsPos;
		}
		if (this.m_charController.ConditionState == CharacterStates.CharacterConditions.Frozen)
		{
			this.m_charController.CharacterCorgi.UnFreeze();
			this.m_charController.ControllerCorgi.GravityActive(this.m_hasGravity);
			EnemyController enemyController = this.m_charController as EnemyController;
			if (enemyController)
			{
				enemyController.LogicController.Unpause();
			}
		}
		if (!this.m_charController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Phased))
		{
			this.m_charController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
		}
		this.m_charController.Animator.enabled = true;
		this.m_charController.BaseKnockbackDefense = this.m_storedKnockback;
		this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onFrozenEnemyHit);
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000C281C File Offset: 0x000C0A1C
	private void OnFrozenEnemyHit(object sender, EventArgs args)
	{
		if (Time.time > this.m_unbreakableFreezeTime)
		{
			CharacterHitEventArgs characterHitEventArgs = args as CharacterHitEventArgs;
			if (characterHitEventArgs != null)
			{
				Projectile_RL projectile_RL = characterHitEventArgs.Attacker as Projectile_RL;
				if (projectile_RL && projectile_RL.BaseDamage <= 0f)
				{
					return;
				}
			}
			this.StopEffect(false);
			EffectManager.PlayEffect(this.m_charController.gameObject, this.m_charController.Animator, "FreezeThawParticles_Effect", this.m_charController.Midpoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x00017E9C File Offset: 0x0001609C
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController && this.m_charController.IsInitialized)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onFrozenEnemyHit);
		}
	}

	// Token: 0x0400248D RID: 9357
	private const string MULTIPLY_COLOR = "#9CE6FF";

	// Token: 0x0400248E RID: 9358
	private const string ADD_COLOR = "#7382FF";

	// Token: 0x0400248F RID: 9359
	private Color m_multiplyColor;

	// Token: 0x04002490 RID: 9360
	private Color m_addColor;

	// Token: 0x04002491 RID: 9361
	private float m_shakeAmountMin = 0.025f;

	// Token: 0x04002492 RID: 9362
	private float m_shakeAmountMax = 0.025f;

	// Token: 0x04002493 RID: 9363
	private float m_shakeInterval = 0.05f;

	// Token: 0x04002494 RID: 9364
	private float m_shakeDuration = 0.5f;

	// Token: 0x04002495 RID: 9365
	private bool m_shakeStarted;

	// Token: 0x04002496 RID: 9366
	private Coroutine m_shakeCoroutine;

	// Token: 0x04002497 RID: 9367
	private Vector3 m_storedVisualsPos;

	// Token: 0x04002498 RID: 9368
	private bool m_hasGravity;

	// Token: 0x04002499 RID: 9369
	private float m_unbreakableFreezeTime;

	// Token: 0x0400249A RID: 9370
	private float m_storedKnockback;

	// Token: 0x0400249B RID: 9371
	private Action<object, CharacterHitEventArgs> m_onFrozenEnemyHit;
}
