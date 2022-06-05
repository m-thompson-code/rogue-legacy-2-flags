using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006AD RID: 1709
public class DisclaimerPositionController : MonoBehaviour
{
	// Token: 0x06003494 RID: 13460 RVA: 0x000DDB18 File Offset: 0x000DBD18
	public void OnEnable()
	{
		if (this.m_disclaimerPositionObj)
		{
			if (base.transform.parent != this.m_disclaimerPositionObj.transform)
			{
				base.transform.SetParent(this.m_disclaimerPositionObj.transform);
			}
			if (this.m_disclaimerPositionObj.activeSelf)
			{
				this.m_isFirstObj = true;
			}
			base.transform.position = this.m_disclaimerPositionObj.transform.position;
		}
		if (this.m_isFirstObj)
		{
			base.StartCoroutine(this.UpdatePositionHack());
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0001CDDB File Offset: 0x0001AFDB
	private IEnumerator UpdatePositionHack()
	{
		yield return null;
		base.transform.position = this.m_disclaimerPositionObj.transform.position;
		yield break;
	}

	// Token: 0x04002A81 RID: 10881
	[SerializeField]
	private GameObject m_disclaimerPositionObj;

	// Token: 0x04002A82 RID: 10882
	private bool m_isFirstObj;
}
