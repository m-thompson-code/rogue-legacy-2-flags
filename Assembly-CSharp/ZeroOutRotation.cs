using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000821 RID: 2081
public class ZeroOutRotation : MonoBehaviour
{
	// Token: 0x060044EC RID: 17644 RVA: 0x000F50CA File Offset: 0x000F32CA
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

	// Token: 0x060044ED RID: 17645 RVA: 0x000F50DC File Offset: 0x000F32DC
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

	// Token: 0x060044EE RID: 17646 RVA: 0x000F5180 File Offset: 0x000F3380
	public void AverageNonUniformScaling()
	{
		Transform parent = base.transform.parent;
		base.transform.SetParent(null);
		float num = (base.transform.localScale.x + base.transform.localScale.y) / 2f;
		base.transform.localScale = new Vector3(num, num, base.transform.localScale.z);
		base.transform.SetParent(parent);
	}

	// Token: 0x04003AC6 RID: 15046
	[SerializeField]
	private bool m_counterNonUniformScaling;
}
