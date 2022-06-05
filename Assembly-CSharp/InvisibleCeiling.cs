using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class InvisibleCeiling : MonoBehaviour, IMirror
{
	// Token: 0x06002976 RID: 10614 RVA: 0x00089292 File Offset: 0x00087492
	private void Awake()
	{
		this.m_meshRenderer.enabled = false;
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000892A0 File Offset: 0x000874A0
	public void Mirror()
	{
		if (base.transform.right != Vector3.right)
		{
			base.transform.right = Vector3.Reflect(base.transform.right, Vector3.up);
		}
	}

	// Token: 0x04002225 RID: 8741
	[SerializeField]
	private MeshRenderer m_meshRenderer;
}
