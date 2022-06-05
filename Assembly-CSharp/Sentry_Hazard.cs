using System;
using System.Collections;
using RLAudio;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class Sentry_Hazard : Hazard, IPointHazard, IHazard, IRootObj, IHasProjectileNameArray
{
	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x06002908 RID: 10504 RVA: 0x000879E5 File Offset: 0x00085BE5
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

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x06002909 RID: 10505 RVA: 0x00087A09 File Offset: 0x00085C09
	// (set) Token: 0x0600290A RID: 10506 RVA: 0x00087A11 File Offset: 0x00085C11
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

	// Token: 0x0600290B RID: 10507 RVA: 0x00087A1A File Offset: 0x00085C1A
	protected override void Awake()
	{
		base.Awake();
		this.m_onAbilityCast = new Action<MonoBehaviour, EventArgs>(this.OnAbilityCast);
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x00087A48 File Offset: 0x00085C48
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeCast, this.m_onAbilityCast);
		this.m_timeThisBecameEnabled = Time.time;
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x00087A94 File Offset: 0x00085C94
	protected override void OnDisable()
	{
		base.OnDisable();
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerSpellAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerTalentAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onAbilityCast);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeCast, this.m_onAbilityCast);
		this.m_previousIsPlayerWithinRange = false;
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x00087AE4 File Offset: 0x00085CE4
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

	// Token: 0x0600290F RID: 10511 RVA: 0x00087B90 File Offset: 0x00085D90
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

	// Token: 0x06002910 RID: 10512 RVA: 0x00087C28 File Offset: 0x00085E28
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

	// Token: 0x06002911 RID: 10513 RVA: 0x00087C38 File Offset: 0x00085E38
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

	// Token: 0x06002912 RID: 10514 RVA: 0x00087DA1 File Offset: 0x00085FA1
	public void ActivateRestState()
	{
		base.StopAllCoroutines();
		this.m_sentryWarningAnimator.SetBool("Aware", false);
		base.Animator.Play("Idle");
		this.m_isShooting = false;
		base.StartCoroutine(this.RestCoroutine());
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x00087DDE File Offset: 0x00085FDE
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

	// Token: 0x06002914 RID: 10516 RVA: 0x00087DED File Offset: 0x00085FED
	public void StartInRestState(float duration)
	{
		base.StartCoroutine(this.StartInRestStateCoroutine(duration));
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x00087DFD File Offset: 0x00085FFD
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

	// Token: 0x06002916 RID: 10518 RVA: 0x00087E14 File Offset: 0x00086014
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

	// Token: 0x06002917 RID: 10519 RVA: 0x00087F1F File Offset: 0x0008611F
	public void SetRestDurationOverride(float duration)
	{
		this.m_restDurationOverride = duration;
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x00087F51 File Offset: 0x00086151
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040021D0 RID: 8656
	[SerializeField]
	private SpriteRenderer m_sentryEyeRenderer;

	// Token: 0x040021D1 RID: 8657
	[SerializeField]
	private SpriteRenderer m_sentryWarningSprite;

	// Token: 0x040021D2 RID: 8658
	[SerializeField]
	private Animator m_sentryWarningAnimator;

	// Token: 0x040021D3 RID: 8659
	[SerializeField]
	private Interactable m_interactable;

	// Token: 0x040021D4 RID: 8660
	[SerializeField]
	private SentryHazardAudioEventController m_audioController;

	// Token: 0x040021D5 RID: 8661
	public Relay<Sentry_Hazard, bool> SentryRestingRelay = new Relay<Sentry_Hazard, bool>();

	// Token: 0x040021D6 RID: 8662
	public Relay<bool> PlayerWithinRangeChangeRelay = new Relay<bool>();

	// Token: 0x040021D7 RID: 8663
	private float m_attackStartTime;

	// Token: 0x040021D8 RID: 8664
	private float m_radius;

	// Token: 0x040021D9 RID: 8665
	private bool m_lookingAtPlayer;

	// Token: 0x040021DA RID: 8666
	private bool m_isShooting;

	// Token: 0x040021DB RID: 8667
	private float m_storedWarningScaleAmount;

	// Token: 0x040021DC RID: 8668
	private float m_timeThisBecameEnabled;

	// Token: 0x040021DD RID: 8669
	private bool m_isResting;

	// Token: 0x040021DE RID: 8670
	private Tween m_restShrinkTween;

	// Token: 0x040021DF RID: 8671
	private Tween m_restGrowTween;

	// Token: 0x040021E0 RID: 8672
	private float m_restDurationOverride = -1f;

	// Token: 0x040021E1 RID: 8673
	private bool m_previousIsPlayerWithinRange;

	// Token: 0x040021E2 RID: 8674
	private WaitRL_Yield m_waitYield;

	// Token: 0x040021E3 RID: 8675
	private Action<MonoBehaviour, EventArgs> m_onAbilityCast;

	// Token: 0x040021E4 RID: 8676
	[NonSerialized]
	private string[] m_projectileNameArray;
}
