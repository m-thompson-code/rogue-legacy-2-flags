using System;
using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class FreezeStatusEffect : BaseStatusEffect
{
	// Token: 0x17000D58 RID: 3416
	// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000636DD File Offset: 0x000618DD
	public override StatusEffectType StatusEffectType
	{
		get
		{
			return StatusEffectType.Enemy_Freeze;
		}
	}

	// Token: 0x17000D59 RID: 3417
	// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000636E1 File Offset: 0x000618E1
	public override float StartingDurationOverride
	{
		get
		{
			return 3f;
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000636E8 File Offset: 0x000618E8
	public override void Initialize(StatusEffectController statusEffectController, BaseCharacterController charController)
	{
		base.Initialize(statusEffectController, charController);
		ColorUtility.TryParseHtmlString("#9CE6FF", out this.m_multiplyColor);
		ColorUtility.TryParseHtmlString("#7382FF", out this.m_addColor);
		base.AppliesTint = true;
		this.m_onFrozenEnemyHit = new Action<object, CharacterHitEventArgs>(this.OnFrozenEnemyHit);
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x00063738 File Offset: 0x00061938
	public override void StartEffect(float duration, IDamageObj caster)
	{
		base.StartEffect(duration, caster);
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x00063742 File Offset: 0x00061942
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

	// Token: 0x06001ECB RID: 7883 RVA: 0x00063751 File Offset: 0x00061951
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

	// Token: 0x06001ECC RID: 7884 RVA: 0x00063760 File Offset: 0x00061960
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

	// Token: 0x06001ECD RID: 7885 RVA: 0x0006386C File Offset: 0x00061A6C
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

	// Token: 0x06001ECE RID: 7886 RVA: 0x000638EC File Offset: 0x00061AEC
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_charController && this.m_charController.IsInitialized)
		{
			this.m_charController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(this.m_onFrozenEnemyHit);
		}
	}

	// Token: 0x04001BCD RID: 7117
	private const string MULTIPLY_COLOR = "#9CE6FF";

	// Token: 0x04001BCE RID: 7118
	private const string ADD_COLOR = "#7382FF";

	// Token: 0x04001BCF RID: 7119
	private Color m_multiplyColor;

	// Token: 0x04001BD0 RID: 7120
	private Color m_addColor;

	// Token: 0x04001BD1 RID: 7121
	private float m_shakeAmountMin = 0.025f;

	// Token: 0x04001BD2 RID: 7122
	private float m_shakeAmountMax = 0.025f;

	// Token: 0x04001BD3 RID: 7123
	private float m_shakeInterval = 0.05f;

	// Token: 0x04001BD4 RID: 7124
	private float m_shakeDuration = 0.5f;

	// Token: 0x04001BD5 RID: 7125
	private bool m_shakeStarted;

	// Token: 0x04001BD6 RID: 7126
	private Coroutine m_shakeCoroutine;

	// Token: 0x04001BD7 RID: 7127
	private Vector3 m_storedVisualsPos;

	// Token: 0x04001BD8 RID: 7128
	private bool m_hasGravity;

	// Token: 0x04001BD9 RID: 7129
	private float m_unbreakableFreezeTime;

	// Token: 0x04001BDA RID: 7130
	private float m_storedKnockback;

	// Token: 0x04001BDB RID: 7131
	private Action<object, CharacterHitEventArgs> m_onFrozenEnemyHit;
}
