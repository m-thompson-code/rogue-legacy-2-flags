using System;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class Triple_Orbiter_Hazard : Hazard, IPointHazard, IHazard, IRootObj
{
	// Token: 0x06002949 RID: 10569 RVA: 0x000887B4 File Offset: 0x000869B4
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

	// Token: 0x0600294A RID: 10570 RVA: 0x000888C8 File Offset: 0x00086AC8
	public override void ResetHazard()
	{
		for (int i = 0; i < this.m_orbiters.Length; i++)
		{
			this.m_orbiters[i].ResetHazard();
		}
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000888F8 File Offset: 0x00086AF8
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

	// Token: 0x0600294E RID: 10574 RVA: 0x00088980 File Offset: 0x00086B80
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002200 RID: 8704
	public const int NUM_ORBITERS = 3;

	// Token: 0x04002201 RID: 8705
	private const float SPEED_MOD = 0.75f;

	// Token: 0x04002202 RID: 8706
	private static readonly float[] m_angleOffsets = new float[]
	{
		0f,
		120f,
		240f
	};

	// Token: 0x04002203 RID: 8707
	private IHazard[] m_orbiters = new IHazard[3];
}
