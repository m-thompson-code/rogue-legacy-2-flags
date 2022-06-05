using System;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public class Windmill_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x17001552 RID: 5458
	// (get) Token: 0x0600396F RID: 14703 RVA: 0x0001F93A File Offset: 0x0001DB3A
	// (set) Token: 0x06003970 RID: 14704 RVA: 0x0001F942 File Offset: 0x0001DB42
	public float CurrentRotation { get; private set; }

	// Token: 0x06003971 RID: 14705 RVA: 0x0001F94B File Offset: 0x0001DB4B
	private void OnEnable()
	{
		if (TraitManager.IsTraitActive(TraitType.ColorTrails))
		{
			this.ApplyColorTrails();
		}
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x000EB0D4 File Offset: 0x000E92D4
	private void ApplyColorTrails()
	{
		if (!this.m_colorTrailApplied)
		{
			Debug.Log("Applying color trails to windmill");
			BaseEffect baseEffect = EffectManager.PlayEffect(this.m_bladeTip, null, "ColorTrails_Trait_Effect", this.m_bladeTip.transform.position, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			TrailRenderer componentInChildren = baseEffect.GetComponentInChildren<TrailRenderer>();
			componentInChildren.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.endColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			componentInChildren.widthMultiplier = 1.5f;
			componentInChildren.time = 18f;
			baseEffect.transform.SetParent(this.m_bladeTip.gameObject.transform, true);
			this.m_colorTrailApplied = true;
		}
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x000EB1B0 File Offset: 0x000E93B0
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
			this.m_rotationSpeed = pointHazardArgs.RotationSpeed;
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Failed to cast hazardArgs as PointHazardArgs. If you see this message please bug it on Pivotal.</color>", Array.Empty<object>());
	}

	// Token: 0x06003974 RID: 14708 RVA: 0x000EB234 File Offset: 0x000E9434
	private void Update()
	{
		if (this.m_rotateClockwise)
		{
			this.CurrentRotation -= this.m_rotationSpeed * Time.deltaTime;
		}
		else
		{
			this.CurrentRotation += this.m_rotationSpeed * Time.deltaTime;
		}
		this.CurrentRotation = CDGHelper.WrapAngleDegrees(this.CurrentRotation, false);
		Vector3 localEulerAngles = this.m_pivot.transform.localEulerAngles;
		localEulerAngles.z = this.CurrentRotation;
		this.m_pivot.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06003975 RID: 14709 RVA: 0x0001F95F File Offset: 0x0001DB5F
	public override void ResetHazard()
	{
		this.CurrentRotation = 0f;
		this.m_pivot.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06003976 RID: 14710 RVA: 0x0001F981 File Offset: 0x0001DB81
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_colorTrailApplied = false;
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002DF9 RID: 11769
	[SerializeField]
	private GameObject m_bladeTip;

	// Token: 0x04002DFA RID: 11770
	private bool m_rotateClockwise = true;

	// Token: 0x04002DFB RID: 11771
	private float m_rotationSpeed;

	// Token: 0x04002DFC RID: 11772
	private bool m_colorTrailApplied;
}
