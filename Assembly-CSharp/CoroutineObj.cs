using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class CoroutineObj : MonoBehaviour
{
	// Token: 0x17000D3D RID: 3389
	// (get) Token: 0x06001C65 RID: 7269 RVA: 0x0000EB11 File Offset: 0x0000CD11
	// (set) Token: 0x06001C66 RID: 7270 RVA: 0x0000EB19 File Offset: 0x0000CD19
	public UnityEngine.Object BaseObject { get; set; }

	// Token: 0x06001C67 RID: 7271 RVA: 0x00002FCA File Offset: 0x000011CA
	public virtual void Initialize()
	{
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x0000EB22 File Offset: 0x0000CD22
	public virtual void StartLogic()
	{
		this.m_coroutine = base.StartCoroutine(this.Execute());
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x0000EB36 File Offset: 0x0000CD36
	protected virtual IEnumerator Execute()
	{
		yield break;
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x0000EB3E File Offset: 0x0000CD3E
	public virtual void StopLogic()
	{
		if (this.m_coroutine != null)
		{
			base.StopCoroutine(this.m_coroutine);
		}
	}

	// Token: 0x040019EE RID: 6638
	protected Coroutine m_coroutine;
}
