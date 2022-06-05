using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CF8 RID: 3320
public class ZeroOutRotation : MonoBehaviour
{
	// Token: 0x06005EB8 RID: 24248 RVA: 0x00034392 File Offset: 0x00032592
	private IEnumerator Start()
	{
		Prop prop = base.GetComponentInParent<Prop>();
		if (prop)
		{
			while (!prop.IsInitialized)
			{
				yield return null;
			}
		}
		this.ZeroOut();
		if (this.m_counterNonUniformScaling)
		{
			this.AverageNonUniformScaling();
		}
		yield break;
	}

	// Token: 0x06005EB9 RID: 24249 RVA: 0x00162C4C File Offset: 0x00160E4C
	public void ZeroOut()
	{
		base.gameObject.transform.eulerAngles = Vector3.zero;
		if ((base.transform.lossyScale.x < 0f && base.transform.localScale.x > 0f) || (base.transform.lossyScale.x > 0f && base.transform.localScale.x < 0f))
		{
			Vector3 localScale = base.transform.localScale;
			localScale.x = -localScale.x;
			base.transform.localScale = localScale;
		}
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x00162CF0 File Offset: 0x00160EF0
	public void AverageNonUniformScaling()
	{
		Transform parent = base.transform.parent;
		base.transform.SetParent(null);
		float num = (base.transform.localScale.x + base.transform.localScale.y) / 2f;
		base.transform.localScale = new Vector3(num, num, base.transform.localScale.z);
		base.transform.SetParent(parent);
	}

	// Token: 0x04004DCA RID: 19914
	[SerializeField]
	private bool m_counterNonUniformScaling;
}
