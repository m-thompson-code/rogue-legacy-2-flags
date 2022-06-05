using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class Orbiter_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x060028B3 RID: 10419 RVA: 0x00086865 File Offset: 0x00084A65
	public override float BaseDamage
	{
		get
		{
			return 30f;
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x060028B4 RID: 10420 RVA: 0x0008686C File Offset: 0x00084A6C
	// (set) Token: 0x060028B5 RID: 10421 RVA: 0x00086874 File Offset: 0x00084A74
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

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0008687D File Offset: 0x00084A7D
	// (set) Token: 0x060028B7 RID: 10423 RVA: 0x00086885 File Offset: 0x00084A85
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

	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x060028B8 RID: 10424 RVA: 0x0008688E File Offset: 0x00084A8E
	public float InitialRotation
	{
		get
		{
			return this.m_initialRotation.z;
		}
	}

	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x060028B9 RID: 10425 RVA: 0x0008689B File Offset: 0x00084A9B
	public GameObject OrbiterBall
	{
		get
		{
			return this.m_orbiterBall;
		}
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000868A3 File Offset: 0x00084AA3
	protected override void Awake()
	{
		base.Awake();
		this.m_hbController = base.GetComponentInChildren<IHitboxController>();
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x000868B8 File Offset: 0x00084AB8
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

	// Token: 0x060028BC RID: 10428 RVA: 0x00086908 File Offset: 0x00084B08
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

	// Token: 0x060028BD RID: 10429 RVA: 0x000869E1 File Offset: 0x00084BE1
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

	// Token: 0x060028BE RID: 10430 RVA: 0x000869F0 File Offset: 0x00084BF0
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

	// Token: 0x060028BF RID: 10431 RVA: 0x00086A00 File Offset: 0x00084C00
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

	// Token: 0x060028C0 RID: 10432 RVA: 0x00086B6C File Offset: 0x00084D6C
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

	// Token: 0x060028C1 RID: 10433 RVA: 0x00086C08 File Offset: 0x00084E08
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

	// Token: 0x060028C2 RID: 10434 RVA: 0x00086CC2 File Offset: 0x00084EC2
	private void SetExpansionDuration(float expansionDuration)
	{
		this.m_orbiterExpansionTimer = expansionDuration;
		this.m_orbiterExpansionDuration = expansionDuration;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x00086CD4 File Offset: 0x00084ED4
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

	// Token: 0x060028C4 RID: 10436 RVA: 0x00086D78 File Offset: 0x00084F78
	private void SetRotationSpeed(float rotationSpeed)
	{
		this.m_orbiterRotationSpeed = rotationSpeed;
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x00086D81 File Offset: 0x00084F81
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_colorTrailApplied = false;
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x00086DB5 File Offset: 0x00084FB5
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400219A RID: 8602
	[SerializeField]
	private SpriteRenderer m_orbiterChain;

	// Token: 0x0400219B RID: 8603
	[SerializeField]
	private GameObject m_orbiterBall;

	// Token: 0x0400219C RID: 8604
	[SerializeField]
	private SpriteRenderer m_orbiterRadius;

	// Token: 0x0400219D RID: 8605
	private bool m_rotateClockwise = true;

	// Token: 0x0400219E RID: 8606
	private float m_orbiterRotationSpeed = 100f;

	// Token: 0x0400219F RID: 8607
	private float m_orbiterExpansionDuration = 2.5f;

	// Token: 0x040021A0 RID: 8608
	private Vector3 m_initialRotation;

	// Token: 0x040021A1 RID: 8609
	private Vector3 m_initialPosition;

	// Token: 0x040021A2 RID: 8610
	private float m_initialRadius;

	// Token: 0x040021A3 RID: 8611
	private float m_currentRotation;

	// Token: 0x040021A4 RID: 8612
	private float m_orbiterExpansionTimer;

	// Token: 0x040021A5 RID: 8613
	private float m_expansionPercent;

	// Token: 0x040021A6 RID: 8614
	private bool m_colorTrailApplied;

	// Token: 0x040021A7 RID: 8615
	private IHitboxController m_hbController;
}
