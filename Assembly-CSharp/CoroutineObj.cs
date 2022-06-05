using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class CoroutineObj : MonoBehaviour
{
	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x0003BF31 File Offset: 0x0003A131
	// (set) Token: 0x060013BB RID: 5051 RVA: 0x0003BF39 File Offset: 0x0003A139
	public UnityEngine.Object BaseObject { get; set; }

	// Token: 0x060013BC RID: 5052 RVA: 0x0003BF42 File Offset: 0x0003A142
	public virtual void Initialize()
	{
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x0003BF44 File Offset: 0x0003A144
	public virtual void StartLogic()
	{
		this.m_coroutine = base.StartCoroutine(this.Execute());
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x0003BF58 File Offset: 0x0003A158
	protected virtual IEnumerator Execute()
	{
		yield break;
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x0003BF60 File Offset: 0x0003A160
	public virtual void StopLogic()
	{
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
		}
	}

	// Token: 0x04001394 RID: 5012
	protected Coroutine m_coroutine;
}
