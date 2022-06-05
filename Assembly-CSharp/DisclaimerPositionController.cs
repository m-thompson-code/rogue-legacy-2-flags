using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class DisclaimerPositionController : MonoBehaviour
{
	// Token: 0x06002615 RID: 9749 RVA: 0x0007DB5C File Offset: 0x0007BD5C
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

	// Token: 0x06002616 RID: 9750 RVA: 0x0007DBED File Offset: 0x0007BDED
	private IEnumerator UpdatePositionHack()
	{
		yield return null;
		base.transform.position = this.m_disclaimerPositionObj.transform.position;
		yield break;
	}

	// Token: 0x04001FCD RID: 8141
	[SerializeField]
	private GameObject m_disclaimerPositionObj;

	// Token: 0x04001FCE RID: 8142
	private bool m_isFirstObj;
}
