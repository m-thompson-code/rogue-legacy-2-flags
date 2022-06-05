using System;
using UnityEngine;

// Token: 0x02000643 RID: 1603
[Serializable]
public abstract class SpawnScenario
{
	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x060039E7 RID: 14823 RVA: 0x000C5005 File Offset: 0x000C3205
	public virtual SpawnScenarioCheckStage CheckStage
	{
		get
		{
			return this.m_checkStage;
		}
	}

	// Token: 0x17001473 RID: 5235
	// (get) Token: 0x060039E8 RID: 14824
	public abstract string GizmoDescription { get; }

	// Token: 0x17001474 RID: 5236
	// (get) Token: 0x060039E9 RID: 14825 RVA: 0x000C500D File Offset: 0x000C320D
	// (set) Token: 0x060039EA RID: 14826 RVA: 0x000C5015 File Offset: 0x000C3215
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

	// Token: 0x17001475 RID: 5237
	// (get) Token: 0x060039EB RID: 14827
	public abstract SpawnScenarioType Type { get; }

	// Token: 0x060039EC RID: 14828 RVA: 0x000C501E File Offset: 0x000C321E
	public string GetDataAsString()
	{
		return JsonUtility.ToJson(this);
	}

	// Token: 0x060039ED RID: 14829 RVA: 0x000C5026 File Offset: 0x000C3226
	public string GetTypeAsString()
	{
		return base.GetType().ToString();
	}

	// Token: 0x060039EE RID: 14830 RVA: 0x000C5033 File Offset: 0x000C3233
	public virtual void RunIsTrueCheck(BaseRoom room)
	{
	}

	// Token: 0x060039EF RID: 14831 RVA: 0x000C5035 File Offset: 0x000C3235
	public virtual void RunIsTrueCheck(GridPointManager gridPointManager)
	{
	}

	// Token: 0x060039F0 RID: 14832 RVA: 0x000C5037 File Offset: 0x000C3237
	public virtual void Start()
	{
	}

	// Token: 0x04002C7E RID: 11390
	[SerializeField]
	protected SpawnScenarioCheckStage m_checkStage;

	// Token: 0x04002C7F RID: 11391
	[SerializeField]
	[ReadOnly]
	private bool m_isTrue;
}
