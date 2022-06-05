using System;
using UnityEngine;

// Token: 0x02000A54 RID: 2644
public class PointHazardSpawnController : HazardSpawnControllerBase
{
	// Token: 0x17001B6B RID: 7019
	// (get) Token: 0x06004FAE RID: 20398 RVA: 0x000046FA File Offset: 0x000028FA
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Point;
		}
	}

	// Token: 0x17001B6C RID: 7020
	// (get) Token: 0x06004FAF RID: 20399 RVA: 0x0002B78F File Offset: 0x0002998F
	// (set) Token: 0x06004FB0 RID: 20400 RVA: 0x0002B797 File Offset: 0x00029997
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

	// Token: 0x17001B6D RID: 7021
	// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x0002B7A7 File Offset: 0x000299A7
	public bool OverrideExpansionDuration
	{
		get
		{
			return this.m_overrideExpansionDuration;
		}
	}

	// Token: 0x17001B6E RID: 7022
	// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x0002B7AF File Offset: 0x000299AF
	public bool OverrideRotationSpeed
	{
		get
		{
			return this.m_overrideRotationSpeed;
		}
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x0002B7B7 File Offset: 0x000299B7
	protected override void Reset()
	{
		if (base.Hazard != null && base.Hazard.gameObject != null)
		{
			base.Hazard.gameObject.SetActive(false);
		}
		base.Hazard = null;
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x00130A14 File Offset: 0x0012EC14
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

	// Token: 0x04003C6B RID: 15467
	[SerializeField]
	[Range(0f, 100f)]
	private float m_radius = 9f;

	// Token: 0x04003C6C RID: 15468
	[SerializeField]
	private bool m_overrideRotationSpeed;

	// Token: 0x04003C6D RID: 15469
	[SerializeField]
	private float m_rotationSpeedOverride = 1f;

	// Token: 0x04003C6E RID: 15470
	[SerializeField]
	private bool m_overrideExpansionDuration;

	// Token: 0x04003C6F RID: 15471
	[SerializeField]
	private float m_expansionDurationOverride = 1f;
}
