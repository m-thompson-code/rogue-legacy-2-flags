using System;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
[Serializable]
public abstract class SpawnScenario
{
	// Token: 0x17001BD9 RID: 7129
	// (get) Token: 0x060050C9 RID: 20681 RVA: 0x0002C1AD File Offset: 0x0002A3AD
	public virtual SpawnScenarioCheckStage CheckStage
	{
		get
		{
			return this.m_checkStage;
		}
	}

	// Token: 0x17001BDA RID: 7130
	// (get) Token: 0x060050CA RID: 20682
	public abstract string GizmoDescription { get; }

	// Token: 0x17001BDB RID: 7131
	// (get) Token: 0x060050CB RID: 20683 RVA: 0x0002C1B5 File Offset: 0x0002A3B5
	// (set) Token: 0x060050CC RID: 20684 RVA: 0x0002C1BD File Offset: 0x0002A3BD
	public virtual bool IsTrue
	{
		get
		{
			return this.m_isTrue;
		}
		protected set
		{
			this.m_isTrue = value;
		}
	}

	// Token: 0x17001BDC RID: 7132
	// (get) Token: 0x060050CD RID: 20685
	public abstract SpawnScenarioType Type { get; }

	// Token: 0x060050CE RID: 20686 RVA: 0x0002C1C6 File Offset: 0x0002A3C6
	public string GetDataAsString()
	{
		return JsonUtility.ToJson(this);
	}

	// Token: 0x060050CF RID: 20687 RVA: 0x0002C1CE File Offset: 0x0002A3CE
	public string GetTypeAsString()
	{
		return base.GetType().ToString();
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void RunIsTrueCheck(BaseRoom room)
	{
	}

	// Token: 0x060050D1 RID: 20689 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void RunIsTrueCheck(GridPointManager gridPointManager)
	{
	}

	// Token: 0x060050D2 RID: 20690 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void Start()
	{
	}

	// Token: 0x04003D12 RID: 15634
	[SerializeField]
	protected SpawnScenarioCheckStage m_checkStage;

	// Token: 0x04003D13 RID: 15635
	[SerializeField]
	[ReadOnly]
	private bool m_isTrue;
}
