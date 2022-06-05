using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000806 RID: 2054
public class PrefabInstantiator : MonoBehaviour
{
	// Token: 0x06004402 RID: 17410 RVA: 0x000F0AC8 File Offset: 0x000EECC8
	private void Awake()
	{
		foreach (GameObject original in this.m_prefabs)
		{
			Transform parent = null;
			if (this.m_parentToThisGameObject)
			{
				parent = base.transform;
			}
			UnityEngine.Object.Instantiate<GameObject>(original, parent);
		}
	}

	// Token: 0x04003A1B RID: 14875
	[SerializeField]
	private List<GameObject> m_prefabs;

	// Token: 0x04003A1C RID: 14876
	[SerializeField]
	private bool m_parentToThisGameObject = true;
}
