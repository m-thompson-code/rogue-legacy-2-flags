using System;
using UnityEngine;

// Token: 0x02000628 RID: 1576
public class PointHazardSpawnController : HazardSpawnControllerBase
{
	// Token: 0x17001406 RID: 5126
	// (get) Token: 0x060038D5 RID: 14549 RVA: 0x000C1E2C File Offset: 0x000C002C
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Point;
		}
	}

	// Token: 0x17001407 RID: 5127
	// (get) Token: 0x060038D6 RID: 14550 RVA: 0x000C1E30 File Offset: 0x000C0030
	// (set) Token: 0x060038D7 RID: 14551 RVA: 0x000C1E38 File Offset: 0x000C0038
	public float Radius
	{
		get
		{
			return this.m_radius;
		}
		set
		{
			if (!Application.isPlaying)
			{
				this.m_radius = value;
			}
		}
	}

	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x060038D8 RID: 14552 RVA: 0x000C1E48 File Offset: 0x000C0048
	public bool OverrideExpansionDuration
	{
		get
		{
			return this.m_overrideExpansionDuration;
		}
	}

	// Token: 0x17001409 RID: 5129
	// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000C1E50 File Offset: 0x000C0050
	public bool OverrideRotationSpeed
	{
		get
		{
			return this.m_overrideRotationSpeed;
		}
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x000C1E58 File Offset: 0x000C0058
	protected override void Reset()
	{
		if (base.Hazard != null && base.Hazard.gameObject != null)
		{
			base.Hazard.gameObject.SetActive(false);
		}
		base.Hazard = null;
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x000C1E90 File Offset: 0x000C0090
	protected override void Spawn()
	{
		if (base.Type == HazardType.None)
		{
			return;
		}
		base.CreateHazard();
		if (this.m_hazardArgs == null)
		{
			float num = this.Radius;
			if (Hazard_EV.POINT_HAZARD_RADIUS_MULTIPLIER.ContainsKey(base.Room.AppearanceBiomeType))
			{
				if (Hazard_EV.POINT_HAZARD_RADIUS_MULTIPLIER[base.Room.AppearanceBiomeType] > 0f)
				{
					num *= Hazard_EV.POINT_HAZARD_RADIUS_MULTIPLIER[base.Room.AppearanceBiomeType];
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | The {1} Biome's entry in Hazard_EV's POINT_HAZARD_RADIUS_MULTIPLIER table has an invalid value. It must be greater than 0.</color>", new object[]
					{
						this,
						base.Room.AppearanceBiomeType
					});
				}
			}
			if (base.transform.localScale.x != 1f && base.transform.localScale.x != 0f)
			{
				num *= 1f / base.transform.localScale.x;
			}
			float rotationSpeed = 75f;
			if (this.OverrideRotationSpeed)
			{
				rotationSpeed = this.m_rotationSpeedOverride;
			}
			else if (base.Type != HazardType.Windmill)
			{
				rotationSpeed = 100f;
			}
			float expansionDuration = 0f;
			if (base.Type != HazardType.Windmill)
			{
				expansionDuration = 2.5f;
				if (this.OverrideExpansionDuration)
				{
					expansionDuration = this.m_expansionDurationOverride;
				}
			}
			this.m_hazardArgs = new PointHazardArgs(base.InitialState, num, rotationSpeed, expansionDuration);
		}
		base.Hazard.Initialize(this.m_hazardArgs);
	}

	// Token: 0x04002BDC RID: 11228
	[SerializeField]
	[Range(0f, 100f)]
	private float m_radius = 9f;

	// Token: 0x04002BDD RID: 11229
	[SerializeField]
	private bool m_overrideRotationSpeed;

	// Token: 0x04002BDE RID: 11230
	[SerializeField]
	private float m_rotationSpeedOverride = 1f;

	// Token: 0x04002BDF RID: 11231
	[SerializeField]
	private bool m_overrideExpansionDuration;

	// Token: 0x04002BE0 RID: 11232
	[SerializeField]
	private float m_expansionDurationOverride = 1f;
}
