using System;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public class SetSortOrderLayer : MonoBehaviour
{
	// Token: 0x060028C8 RID: 10440 RVA: 0x00016E19 File Offset: 0x00015019
	private void Start()
	{
		this.m_meshRenderer.sortingLayerName = this.m_layerName;
	}

	// Token: 0x040023BE RID: 9150
	[SerializeField]
	private MeshRenderer m_meshRenderer;

	// Token: 0x040023BF RID: 9151
	[SerializeField]
	private string m_layerName = "";
}
