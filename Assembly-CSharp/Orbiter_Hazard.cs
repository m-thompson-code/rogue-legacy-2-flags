using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200072C RID: 1836
public class Orbiter_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x17001507 RID: 5383
	// (get) Token: 0x06003833 RID: 14387 RVA: 0x00005391 File Offset: 0x00003591
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x17001508 RID: 5384
	// (get) Token: 0x06003834 RID: 14388 RVA: 0x0001ED88 File Offset: 0x0001CF88
	// (set) Token: 0x06003835 RID: 14389 RVA: 0x0001ED90 File Offset: 0x0001CF90
	public float ExpansionPercent
	{
		get
		{
			return this.m_expansionPercent;
		}
		private set
		{
			this.m_expansionPercent = value;
		}
	}

	// Token: 0x17001509 RID: 5385
	// (get) Token: 0x06003836 RID: 14390 RVA: 0x0001ED99 File Offset: 0x0001CF99
	// (set) Token: 0x06003837 RID: 14391 RVA: 0x0001EDA1 File Offset: 0x0001CFA1
	public float CurrentRotation
	{
		get
		{
			return this.m_currentRotation;
		}
		private set
		{
			this.m_currentRotation = value;
		}
	}

	// Token: 0x1700150A RID: 5386
	// (get) Token: 0x06003838 RID: 14392 RVA: 0x0001EDAA File Offset: 0x0001CFAA
	public float InitialRotation
	{
		get
		{
			return this.m_initialRotation.z;
		}
	}

	// Token: 0x1700150B RID: 5387
	// (get) Token: 0x06003839 RID: 14393 RVA: 0x0001EDB7 File Offset: 0x0001CFB7
	public GameObject OrbiterBall
	{
		get
		{
			return this.m_orbiterBall;
		}
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x0001EDBF File Offset: 0x0001CFBF
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x000E79E0 File Offset: 0x000E5BE0
	private void OnEnable()
	{
		if (!this.m_hbController.IsInitialized)
		{
			base.StartCoroutine(this.InitializeHBControllerCoroutine());
		}
		else
		{
			base.StartCoroutine(this.ActivateWeaponHitboxCoroutine());
		}
		this.ResetHazard();
		if (TraitManager.IsTraitActive(TraitType.ColorTrails))
		{
			this.ApplyColorTrails();
		}
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x000E7A30 File Offset: 0x000E5C30
	private void ApplyColorTrails()
	{
		if (!this.m_colorTrailApplied)
		{
			Debug.Log("Applying color trails to orbiter");
			BaseEffect baseEffect = EffectManager.PlayEffect(this.m_orbiterBall, null, "ColorTrails_Trait_Effect", this.m_orbiterBall.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
			componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.widthMultiplier = 1.5f;
			componentInChildren.time = 18f;
			baseEffect.transform.SetParent(this.m_orbiterBall.gameObject.transform, true);
			this.m_colorTrailApplied = true;
		}
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x0001EDD3 File Offset: 0x0001CFD3
	private IEnumerator InitializeHBControllerCoroutine()
	{
		while (!this.m_hbController.IsInitialized)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		yield return this.ActivateWeaponHitboxCoroutine();
		yield break;
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0001EDE2 File Offset: 0x0001CFE2
	private IEnumerator ActivateWeaponHitboxCoroutine()
	{
		float startTime = Time.time;
		while (Time.time < startTime + 0.25f)
		{
			yield return null;
		}
		this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, true);
		yield break;
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x000E7B0C File Offset: 0x000E5D0C
	private void Update()
	{
		if (this.m_rotateClockwise)
		{
			this.CurrentRotation -= this.m_orbiterRotationSpeed * Time.deltaTime;
		}
		else
		{
			this.CurrentRotation += this.m_orbiterRotationSpeed * Time.deltaTime;
		}
		this.CurrentRotation = CDGHelper.WrapAngleDegrees(this.CurrentRotation, false);
		Vector3 localEulerAngles = this.m_pivot.transform.localEulerAngles;
		localEulerAngles.z = this.CurrentRotation;
		this.m_pivot.transform.localEulerAngles = localEulerAngles;
		if (this.m_orbiterExpansionTimer > 0f)
		{
			this.m_orbiterExpansionTimer -= Time.deltaTime;
			if (this.m_orbiterExpansionTimer < 0f)
			{
				this.m_orbiterExpansionTimer = 0f;
			}
		}
		this.ExpansionPercent = 1f;
		if (this.m_orbiterExpansionDuration != 0f)
		{
			this.ExpansionPercent = Mathf.Clamp(1f - this.m_orbiterExpansionTimer / this.m_orbiterExpansionDuration, 0f, 1f);
			this.m_orbiterChain.size = new Vector2(this.m_orbiterChain.size.x, this.m_initialRadius * this.ExpansionPercent);
		}
		Vector3 localPosition = this.m_initialRadius * this.ExpansionPercent * Vector3.up;
		localPosition.z = this.m_initialPosition.z;
		this.OrbiterBall.transform.localPosition = localPosition;
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x000E7C78 File Offset: 0x000E5E78
	public override void Initialize(HazardArgs hazardArgs)
	{
		base.InitialState = hazardArgs.InitialState;
		StateID initialState = base.InitialState;
		if (initialState != StateID.One)
		{
			if (initialState != StateID.Two)
			{
				if (initialState == StateID.Random)
				{
					if (CDGHelper.RandomPlusMinus() > 0)
					{
						this.m_rotateClockwise = true;
					}
					else
					{
						this.m_rotateClockwise = false;
					}
				}
			}
			else
			{
				this.m_rotateClockwise = false;
			}
		}
		else
		{
			this.m_rotateClockwise = true;
		}
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		if (pointHazardArgs != null)
		{
			this.SetRadius(pointHazardArgs.Radius);
			this.SetRotationSpeed(pointHazardArgs.RotationSpeed);
			this.SetExpansionDuration(pointHazardArgs.ExpansionDuration);
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x000E7D14 File Offset: 0x000E5F14
	public override void ResetHazard()
	{
		this.CurrentRotation = 0f;
		this.m_pivot.transform.localEulerAngles = this.m_initialRotation;
		this.m_orbiterExpansionTimer = this.m_orbiterExpansionDuration;
		Vector3 localPosition = this.m_initialRadius * 0f * Vector3.up;
		localPosition.z = this.m_initialPosition.z;
		this.OrbiterBall.transform.localPosition = localPosition;
		this.m_orbiterChain.size = new Vector2(this.m_orbiterChain.size.x, this.m_initialRadius * 0f);
		if (this.m_hbController.IsInitialized)
		{
			this.m_hbController.SetHitboxActiveState(HitboxType.Weapon, false);
		}
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x0001EDF1 File Offset: 0x0001CFF1
	private void SetExpansionDuration(float expansionDuration)
	{
		this.m_orbiterExpansionTimer = expansionDuration;
		this.m_orbiterExpansionDuration = expansionDuration;
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x000E7DD0 File Offset: 0x000E5FD0
	private void SetRadius(float radius)
	{
		this.m_initialPosition = this.OrbiterBall.transform.localPosition;
		this.m_initialRotation = this.m_pivot.transform.localEulerAngles;
		float num = this.m_orbiterRadius.bounds.size.x / 2f;
		float num2 = radius / num;
		this.m_orbiterRadius.gameObject.transform.localScale = new Vector3(num2, num2, num2);
		Color color = this.m_orbiterRadius.color;
		color.a = 0.35f;
		this.m_orbiterRadius.color = color;
		this.m_initialRadius = radius;
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x0001EE01 File Offset: 0x0001D001
	private void SetRotationSpeed(float rotationSpeed)
	{
		this.m_orbiterRotationSpeed = rotationSpeed;
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x0001EE0A File Offset: 0x0001D00A
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_colorTrailApplied = false;
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002D1A RID: 11546
	[SerializeField]
	private SpriteRenderer m_orbiterChain;

	// Token: 0x04002D1B RID: 11547
	[SerializeField]
	private GameObject m_orbiterBall;

	// Token: 0x04002D1C RID: 11548
	[SerializeField]
	private SpriteRenderer m_orbiterRadius;

	// Token: 0x04002D1D RID: 11549
	private bool m_rotateClockwise = true;

	// Token: 0x04002D1E RID: 11550
	private float m_orbiterRotationSpeed = 100f;

	// Token: 0x04002D1F RID: 11551
	private float m_orbiterExpansionDuration = 2.5f;

	// Token: 0x04002D20 RID: 11552
	private Vector3 m_initialRotation;

	// Token: 0x04002D21 RID: 11553
	private Vector3 m_initialPosition;

	// Token: 0x04002D22 RID: 11554
	private float m_initialRadius;

	// Token: 0x04002D23 RID: 11555
	private float m_currentRotation;

	// Token: 0x04002D24 RID: 11556
	private float m_orbiterExpansionTimer;

	// Token: 0x04002D25 RID: 11557
	private float m_expansionPercent;

	// Token: 0x04002D26 RID: 11558
	private bool m_colorTrailApplied;

	// Token: 0x04002D27 RID: 11559
	private IHitboxController m_hbController;
}
