using System;
using System.Collections;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x0200073E RID: 1854
public class Sentry_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray
{
	// Token: 0x1700152A RID: 5418
	// (get) Token: 0x060038B8 RID: 14520 RVA: 0x0001F258 File Offset: 0x0001D458
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				this.m_projectileNameArray = new string[]
				{
					"SentryBounceBoltProjectile"
				};
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x1700152B RID: 5419
	// (get) Token: 0x060038B9 RID: 14521 RVA: 0x0001F27C File Offset: 0x0001D47C
	// (set) Token: 0x060038BA RID: 14522 RVA: 0x0001F284 File Offset: 0x0001D484
	public bool IsResting
	{
		get
		{
			return this.m_isResting;
		}
		private set
		{
			this.m_isResting = value;
		}
	}

	// Token: 0x060038BB RID: 14523 RVA: 0x0001F28D File Offset: 0x0001D48D
	protected override void Awake()
	{
		base.Awake();
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x060038BC RID: 14524 RVA: 0x000E8F60 File Offset: 0x000E7160
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeCast, this.m_onAbilityCast);
		this.m_timeThisBecameEnabled = Time.time;
	}

	// Token: 0x060038BD RID: 14525 RVA: 0x000E8FAC File Offset: 0x000E71AC
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeCast, this.m_onAbilityCast);
		this.m_previousIsPlayerWithinRange = false;
	}

	// Token: 0x060038BE RID: 14526 RVA: 0x000E8FFC File Offset: 0x000E71FC
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.Initialize(hazardArgs);
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		if (pointHazardArgs != null)
		{
			this.m_radius = pointHazardArgs.Radius;
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
		}
		this.m_sentryWarningSprite.gameObject.transform.localScale = Vector3.one;
		float num = 4f;
		float num2 = this.m_radius * 2f * 1.75f;
		this.m_storedWarningScaleAmount = num2 / num;
		Vector3 localScale = new Vector3(this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount);
		this.m_sentryWarningSprite.gameObject.transform.localScale = localScale;
		this.ResetHazard();
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x000E90A8 File Offset: 0x000E72A8
	private void OnAbilityCast(object sender, EventArgs args)
	{
		if (this.m_isShooting)
		{
			return;
		}
		if (this.IsResting)
		{
			return;
		}
		AbilityUsedEventArgs abilityUsedEventArgs = args as AbilityUsedEventArgs;
		if (abilityUsedEventArgs != null && abilityUsedEventArgs.Ability.DealsNoDamage && abilityUsedEventArgs.Ability.AbilityType != AbilityType.PacifistWeapon)
		{
			return;
		}
		if (CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f && Time.time > this.m_attackStartTime + 0.5f)
		{
			base.StartCoroutine(this.FireProjectile());
		}
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
	private IEnumerator FireProjectile()
	{
		while (Time.time < this.m_timeThisBecameEnabled + 0.875f)
		{
			yield return null;
		}
		this.m_isShooting = true;
		base.Animator.Play("Shoot_Tell_Intro");
		this.m_waitYield.CreateNew(0.25f, false);
		yield return this.m_waitYield;
		base.Animator.Play("Shoot_Attack_Intro");
		this.m_attackStartTime = Time.time;
		PlayerController playerController = PlayerManager.GetPlayerController();
		float angleInDeg = CDGHelper.AngleBetweenPts(base.transform.localPosition, playerController.Midpoint);
		ProjectileManager.FireProjectile(base.gameObject, "SentryBounceBoltProjectile", Hazard_EV.SENTRY_PROJECTILE_OFFSET, false, angleInDeg, 1f, false, true, true, true);
		this.m_waitYield.CreateNew(0.1f, false);
		yield return this.m_waitYield;
		base.Animator.Play("Shoot_Exit");
		this.m_sentryEyeRenderer.color = Color.grey;
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		base.Animator.Play("Idle");
		if (CDGHelper.DistanceBetweenPts(playerController.Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f)
		{
			this.m_sentryEyeRenderer.color = Color.red;
		}
		else
		{
			this.m_sentryEyeRenderer.color = Color.grey;
		}
		this.m_isShooting = false;
		yield break;
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x000E9140 File Offset: 0x000E7340
	private void FixedUpdate()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			if (CDGHelper.DistanceBetweenPts(playerController.Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f && !this.m_isResting)
			{
				if (!this.m_lookingAtPlayer)
				{
					this.m_sentryEyeRenderer.color = Color.red;
					this.m_sentryWarningAnimator.SetBool("Aware", true);
					BaseAbility_RL ability = playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
					if (ability && ability.AbilityType == AbilityType.AxeSpinnerWeapon && ability.AbilityActive)
					{
						base.StartCoroutine(this.FireProjectile());
					}
				}
				this.m_lookingAtPlayer = true;
			}
			else if (this.m_lookingAtPlayer)
			{
				this.m_sentryWarningAnimator.SetBool("Aware", false);
				this.m_sentryEyeRenderer.color = Color.grey;
				this.m_lookingAtPlayer = false;
			}
			if (this.m_previousIsPlayerWithinRange != this.m_lookingAtPlayer)
			{
				this.PlayerWithinRangeChangeRelay.Dispatch(this.m_lookingAtPlayer);
				this.m_previousIsPlayerWithinRange = this.m_lookingAtPlayer;
			}
			Vector2 vector = CDGHelper.VectorBetweenPts(base.transform.localPosition, playerController.Midpoint);
			vector.Normalize();
			base.Animator.SetFloat("LookDirectionX", vector.x);
			base.Animator.SetFloat("LookDirectionY", vector.y);
		}
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x0001F2C7 File Offset: 0x0001D4C7
	public void ActivateRestState()
	{
		base.StopAllCoroutines();
		this.m_sentryWarningAnimator.SetBool("Aware", false);
		base.Animator.Play("Idle");
		this.m_isShooting = false;
		base.StartCoroutine(this.RestCoroutine());
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x0001F304 File Offset: 0x0001D504
	private IEnumerator RestCoroutine()
	{
		this.SentryRestingRelay.Dispatch(this, true);
		this.IsResting = true;
		this.m_interactable.SetIsInteractableActive(false);
		this.m_sentryEyeRenderer.color = Color.grey;
		this.m_audioController.OnShrink();
		this.m_restShrinkTween = TweenManager.TweenTo(this.m_sentryWarningSprite.gameObject.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"localScale.x",
			0,
			"localScale.y",
			0,
			"localScale.z",
			0
		});
		yield return this.m_restShrinkTween.TweenCoroutine;
		float waitTime = 8.5f;
		if (this.m_restDurationOverride != -1f)
		{
			waitTime = this.m_restDurationOverride;
		}
		this.m_waitYield.CreateNew(waitTime, false);
		yield return this.m_waitYield;
		base.Animator.SetBool("ReactivationTell", true);
		this.m_audioController.OnGrowPrep();
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		base.Animator.SetBool("ReactivationTell", false);
		this.m_audioController.OnGrow();
		this.m_restGrowTween = TweenManager.TweenTo(this.m_sentryWarningSprite.gameObject.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"localScale.x",
			this.m_storedWarningScaleAmount,
			"localScale.y",
			this.m_storedWarningScaleAmount,
			"localScale.z",
			this.m_storedWarningScaleAmount
		});
		yield return this.m_restGrowTween.TweenCoroutine;
		this.m_sentryEyeRenderer.color = Color.grey;
		if (CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f)
		{
			this.m_sentryEyeRenderer.color = Color.red;
		}
		else
		{
			this.m_sentryEyeRenderer.color = Color.grey;
		}
		this.m_interactable.SetIsInteractableActive(true);
		this.SentryRestingRelay.Dispatch(this, false);
		this.IsResting = false;
		yield break;
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x0001F313 File Offset: 0x0001D513
	public void StartInRestState(float duration)
	{
		base.StartCoroutine(this.StartInRestStateCoroutine(duration));
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x0001F323 File Offset: 0x0001D523
	private IEnumerator StartInRestStateCoroutine(float duration)
	{
		this.IsResting = true;
		this.m_interactable.SetIsInteractableActive(false);
		this.m_sentryEyeRenderer.color = Color.grey;
		this.m_sentryWarningSprite.gameObject.transform.localScale = Vector3.zero;
		float waitTime = Mathf.Clamp(duration - 0.5f, 0f, float.MaxValue);
		this.m_waitYield.CreateNew(waitTime, false);
		yield return this.m_waitYield;
		base.Animator.SetBool("ReactivationTell", true);
		this.m_waitYield.CreateNew(0.5f, false);
		yield return this.m_waitYield;
		base.Animator.SetBool("ReactivationTell", false);
		this.m_restGrowTween = TweenManager.TweenTo(this.m_sentryWarningSprite.gameObject.transform, 0.25f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"localScale.x",
			this.m_storedWarningScaleAmount,
			"localScale.y",
			this.m_storedWarningScaleAmount,
			"localScale.z",
			this.m_storedWarningScaleAmount
		});
		yield return this.m_restGrowTween.TweenCoroutine;
		this.m_sentryEyeRenderer.color = Color.grey;
		if (CDGHelper.DistanceBetweenPts(PlayerManager.GetPlayerController().Midpoint, base.transform.localPosition) <= this.m_radius * 1.75f)
		{
			this.m_sentryEyeRenderer.color = Color.red;
		}
		else
		{
			this.m_sentryEyeRenderer.color = Color.grey;
		}
		this.m_interactable.SetIsInteractableActive(true);
		this.SentryRestingRelay.Dispatch(this, false);
		this.IsResting = false;
		yield break;
	}

	// Token: 0x060038C6 RID: 14534 RVA: 0x000E92AC File Offset: 0x000E74AC
	public override void ResetHazard()
	{
		if (this.m_restShrinkTween != null)
		{
			this.m_restShrinkTween.StopTweenWithConditionChecks(false, this.m_sentryWarningSprite.gameObject.transform, null);
		}
		if (this.m_restGrowTween != null)
		{
			this.m_restGrowTween.StopTweenWithConditionChecks(false, this.m_sentryWarningSprite.gameObject.transform, null);
		}
		this.m_attackStartTime = 0f;
		this.m_isShooting = false;
		base.Animator.Play("Idle");
		base.Animator.SetBool("ReactivationTell", false);
		this.m_lookingAtPlayer = false;
		this.m_sentryEyeRenderer.color = Color.grey;
		this.IsResting = false;
		Vector3 localScale = new Vector3(this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount, this.m_storedWarningScaleAmount);
		this.m_sentryWarningSprite.gameObject.transform.localScale = localScale;
		if (SaveManager.PlayerSaveData.GetHeirloomLevel(HeirloomType.UnlockMemory) > 0)
		{
			this.m_interactable.SetIsInteractableActive(true);
			return;
		}
		this.m_interactable.SetIsInteractableActive(false);
	}

	// Token: 0x060038C7 RID: 14535 RVA: 0x0001F339 File Offset: 0x0001D539
	public void SetRestDurationOverride(float duration)
	{
		this.m_restDurationOverride = duration;
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002D73 RID: 11635
	[SerializeField]
	private SpriteRenderer m_sentryEyeRenderer;

	// Token: 0x04002D74 RID: 11636
	[SerializeField]
	private SpriteRenderer m_sentryWarningSprite;

	// Token: 0x04002D75 RID: 11637
	[SerializeField]
	private Animator m_sentryWarningAnimator;

	// Token: 0x04002D76 RID: 11638
	[SerializeField]
	private Interactable m_interactable;

	// Token: 0x04002D77 RID: 11639
	[SerializeField]
	private SentryHazardAudioEventController m_audioController;

	// Token: 0x04002D78 RID: 11640
	public Relay<Sentry_Hazard, bool> SentryRestingRelay = new Relay<Sentry_Hazard, bool>();

	// Token: 0x04002D79 RID: 11641
	public Relay<bool> PlayerWithinRangeChangeRelay = new Relay<bool>();

	// Token: 0x04002D7A RID: 11642
	private float m_attackStartTime;

	// Token: 0x04002D7B RID: 11643
	private float m_radius;

	// Token: 0x04002D7C RID: 11644
	private bool m_lookingAtPlayer;

	// Token: 0x04002D7D RID: 11645
	private bool m_isShooting;

	// Token: 0x04002D7E RID: 11646
	private float m_storedWarningScaleAmount;

	// Token: 0x04002D7F RID: 11647
	private float m_timeThisBecameEnabled;

	// Token: 0x04002D80 RID: 11648
	private bool m_isResting;

	// Token: 0x04002D81 RID: 11649
	private Tween m_restShrinkTween;

	// Token: 0x04002D82 RID: 11650
	private Tween m_restGrowTween;

	// Token: 0x04002D83 RID: 11651
	private float m_restDurationOverride = -1f;

	// Token: 0x04002D84 RID: 11652
	private bool m_previousIsPlayerWithinRange;

	// Token: 0x04002D85 RID: 11653
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002D86 RID: 11654
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;

	// Token: 0x04002D87 RID: 11655
	[NonSerialized]
	private string[] m_projectileNameArray;
}
