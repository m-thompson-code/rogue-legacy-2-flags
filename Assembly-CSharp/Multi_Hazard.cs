using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class Multi_Hazard : Hazard
{
	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x060028AB RID: 10411 RVA: 0x000864FD File Offset: 0x000846FD
	public List<IHazard> Hazards
	{
		get
		{
			return this.m_hazards;
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x00086505 File Offset: 0x00084705
	public override void Initialize(HazardArgs hazardArgs)
	{
		Debug.LogFormat("<color=red>| {0} | This method should not have been called. Let Paul know</color>", new object[]
		{
			this
		});
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x0008651C File Offset: 0x0008471C
	public virtual void Initialize(PivotPoint pivot, int width, HazardArgs hazardArgs)
	{
		this.m_hazards.Clear();
		Vector3 a = base.transform.right;
		Vector3 a2 = base.transform.position;
		if (pivot == PivotPoint.Right)
		{
			a *= -1f;
		}
		else if (pivot == PivotPoint.Center)
		{
			a2 -= 0.5f * (float)width * base.transform.right;
		}
		for (int i = 0; i < width; i++)
		{
			IHazard hazard = HazardManager.GetHazard(this.m_hazardType);
			hazard.gameObject.SetActive(true);
			this.m_hazards.Add(hazard);
			IMultiHazardConsumer multiHazardConsumer = hazard as IMultiHazardConsumer;
			if (multiHazardConsumer != null)
			{
				multiHazardConsumer.MultiHazard = this;
			}
			this.m_hazards[i].gameObject.transform.rotation = base.transform.rotation;
			if (this.m_rotationOffset != 0f)
			{
				this.m_hazards[i].gameObject.transform.Rotate(new Vector3(0f, 0f, this.m_rotationOffset));
			}
			this.m_hazards[i].gameObject.transform.position = a2 + (float)i * a + this.m_positionOffset * a;
			this.m_hazards[i].Initialize(hazardArgs);
			this.m_hazards[i].ResetHazard();
		}
		foreach (IHazard hazard2 in this.m_hazards)
		{
			((Hazard)hazard2).SetRoom(base.Room);
		}
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x000866DC File Offset: 0x000848DC
	public override void ResetHazard()
	{
		if (!Application.isEditor)
		{
			this.ResetHazards();
		}
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x000866EC File Offset: 0x000848EC
	private void ResetHazards()
	{
		if (base.gameObject != null && this.m_hazards != null)
		{
			foreach (IHazard hazard in this.m_hazards)
			{
				Hazard hazard2 = (Hazard)hazard;
				if (hazard2 != null)
				{
					hazard2.ResetHazard();
				}
			}
		}
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x00086764 File Offset: 0x00084964
	public void Reset()
	{
		if (this.m_hazards != null)
		{
			foreach (IHazard hazard in this.m_hazards)
			{
				Hazard hazard2 = (Hazard)hazard;
				if (hazard2)
				{
					hazard2.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000867D4 File Offset: 0x000849D4
	public override void SetIsCulled(bool culled)
	{
		if (!this)
		{
			return;
		}
		if (this.m_hazards != null)
		{
			foreach (IHazard hazard in this.m_hazards)
			{
				((Hazard)hazard).SetIsCulled(culled);
			}
		}
	}

	// Token: 0x04002196 RID: 8598
	[SerializeField]
	private HazardType m_hazardType = HazardType.None;

	// Token: 0x04002197 RID: 8599
	[SerializeField]
	private float m_rotationOffset;

	// Token: 0x04002198 RID: 8600
	[SerializeField]
	private float m_positionOffset = 0.5f;

	// Token: 0x04002199 RID: 8601
	protected List<IHazard> m_hazards = new List<IHazard>();
}
