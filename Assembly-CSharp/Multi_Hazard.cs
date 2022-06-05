using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200072B RID: 1835
public class Multi_Hazard : Hazard
{
	// Token: 0x17001506 RID: 5382
	// (get) Token: 0x0600382B RID: 14379 RVA: 0x0001ED32 File Offset: 0x0001CF32
	public List<IHazard> Hazards
	{
		get
		{
			return this.m_hazards;
		}
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0001ED3A File Offset: 0x0001CF3A
	public override void Initialize(HazardArgs hazardArgs)
	{
		Debug.LogFormat("<color=red>| {0} | This method should not have been called. Let Paul know</color>", new object[]
		{
			this
		});
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x000E76D0 File Offset: 0x000E58D0
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

	// Token: 0x0600382E RID: 14382 RVA: 0x0001ED50 File Offset: 0x0001CF50
	public override void ResetHazard()
	{
		if (!Application.isEditor)
		{
			this.ResetHazards();
		}
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x000E7890 File Offset: 0x000E5A90
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

	// Token: 0x06003830 RID: 14384 RVA: 0x000E7908 File Offset: 0x000E5B08
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

	// Token: 0x06003831 RID: 14385 RVA: 0x000E7978 File Offset: 0x000E5B78
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

	// Token: 0x04002D16 RID: 11542
	[SerializeField]
	private HazardType m_hazardType = HazardType.None;

	// Token: 0x04002D17 RID: 11543
	[SerializeField]
	private float m_rotationOffset;

	// Token: 0x04002D18 RID: 11544
	[SerializeField]
	private float m_positionOffset = 0.5f;

	// Token: 0x04002D19 RID: 11545
	protected List<IHazard> m_hazards = new List<IHazard>();
}
