using System;
using UnityEngine;

// Token: 0x02000B0B RID: 2827
public class ZPositionOverride : MonoBehaviour
{
	// Token: 0x060054D3 RID: 21715 RVA: 0x0002E064 File Offset: 0x0002C264
	public void SetZPosition()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, this.m_zPosition);
	}

	// Token: 0x04003F2A RID: 16170
	[SerializeField]
	private float m_zPosition;
}
