using System;
using UnityEngine;

// Token: 0x02000752 RID: 1874
public class Triple_Orbiter_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x06003947 RID: 14663 RVA: 0x000EA8C0 File Offset: 0x000E8AC0
	public override void Initialize(HazardArgs hazardArgs)
	{
		PointHazardArgs pointHazardArgs = hazardArgs as PointHazardArgs;
		StateID stateID = hazardArgs.InitialState;
		if (stateID == StateID.Random)
		{
			if (CDGHelper.RandomPlusMinus() > 0)
			{
				stateID = StateID.One;
			}
			else
			{
				stateID = StateID.Two;
			}
		}
		PointHazardArgs hazardArgs2 = new PointHazardArgs(stateID, pointHazardArgs.Radius, pointHazardArgs.RotationSpeed * 0.75f, pointHazardArgs.ExpansionDuration);
		for (int i = 0; i < this.m_orbiters.Length; i++)
		{
			this.m_orbiters[i] = HazardManager.GetHazard(HazardType.Orbiter);
			this.m_orbiters[i].gameObject.SetActive(true);
			this.m_orbiters[i].gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
			this.m_orbiters[i].gameObject.transform.Rotate(0f, 0f, Triple_Orbiter_Hazard.m_angleOffsets[i]);
			this.m_orbiters[i].Initialize(hazardArgs2);
			this.m_orbiters[i].ResetHazard();
			(this.m_orbiters[i] as Hazard).SetRoom(base.Room);
		}
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x000EA9D4 File Offset: 0x000E8BD4
	public override void ResetHazard()
	{
		for (int i = 0; i < this.m_orbiters.Length; i++)
		{
			this.m_orbiters[i].ResetHazard();
		}
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x000EAA04 File Offset: 0x000E8C04
	protected override void OnDisable()
	{
		for (int i = 0; i < this.m_orbiters.Length; i++)
		{
			if (this.m_orbiters[i] != null && this.m_orbiters[i].gameObject)
			{
				this.m_orbiters[i].gameObject.SetActive(false);
			}
		}
		base.OnDisable();
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002DD1 RID: 11729
	public const int NUM_ORBITERS = 3;

	// Token: 0x04002DD2 RID: 11730
	private const float SPEED_MOD = 0.75f;

	// Token: 0x04002DD3 RID: 11731
	private static readonly float[] m_angleOffsets = new float[]
	{
		0f,
		120f,
		240f
	};

	// Token: 0x04002DD4 RID: 11732
	private IHazard[] m_orbiters = new IHazard[3];
}
