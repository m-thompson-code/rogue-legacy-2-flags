using System;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class InvisibleCeiling : MonoBehaviour, IMirror
{
	// Token: 0x0600397A RID: 14714 RVA: 0x0001F99F File Offset: 0x0001DB9F
	private void Awake()
	{
		this.m_meshRenderer.enabled = false;
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x0001F9AD File Offset: 0x0001DBAD
	public void Mirror()
	{
		if (base.transform.right != Vector3.right)
		{
			base.transform.right = Vector3.Reflect(base.transform.right, Vector3.up);
		}
	}

	// Token: 0x04002DFE RID: 11774
	[SerializeField]
	private MeshRenderer m_meshRenderer;
}
