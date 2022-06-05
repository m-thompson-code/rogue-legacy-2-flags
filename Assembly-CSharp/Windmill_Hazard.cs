using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class Windmill_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x0600296B RID: 10603 RVA: 0x00089032 File Offset: 0x00087232
	// (set) Token: 0x0600296C RID: 10604 RVA: 0x0008903A File Offset: 0x0008723A
	public float CurrentRotation { get; private set; }

	// Token: 0x0600296D RID: 10605 RVA: 0x00089043 File Offset: 0x00087243
	private void OnEnable()
	{
		if (TraitManager.IsTraitActive(TraitType.ColorTrails))
		{
			this.ApplyColorTrails();
		}
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x00089058 File Offset: 0x00087258
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

	// Token: 0x0600296F RID: 10607 RVA: 0x00089134 File Offset: 0x00087334
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

	// Token: 0x06002970 RID: 10608 RVA: 0x000891B8 File Offset: 0x000873B8
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

	// Token: 0x06002971 RID: 10609 RVA: 0x00089242 File Offset: 0x00087442
	public override void ResetHazard()
	{
		this.CurrentRotation = 0f;
		this.m_pivot.transform.localEulerAngles = Vector3.zero;
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x00089264 File Offset: 0x00087464
	protected override void OnDisable()
	{
		base.OnDisable();
		this.m_colorTrailApplied = false;
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x00089282 File Offset: 0x00087482
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002220 RID: 8736
	[SerializeField]
	private GameObject m_bladeTip;

	// Token: 0x04002221 RID: 8737
	private bool m_rotateClockwise = true;

	// Token: 0x04002222 RID: 8738
	private float m_rotationSpeed;

	// Token: 0x04002223 RID: 8739
	private bool m_colorTrailApplied;
}
