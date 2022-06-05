using System;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class DestroyOnRetailBuild : MonoBehaviour
{
	// Token: 0x170016E9 RID: 5865
	// (get) Token: 0x060043B1 RID: 17329 RVA: 0x000ECAA2 File Offset: 0x000EACA2
	// (set) Token: 0x060043B2 RID: 17330 RVA: 0x000ECAAA File Offset: 0x000EACAA
	public Component[] ComponentsToDestroyArray
	{
		get
		{
			return this.m_componentsToDestroy;
		}
		set
		{
			this.m_componentsToDestroy = value;
		}
	}

	// Token: 0x060043B3 RID: 17331 RVA: 0x000ECAB4 File Offset: 0x000EACB4
	private void Awake()
	{
		if (this.m_destroyGameObject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		foreach (Component component in this.m_componentsToDestroy)
		{
			if (component.gameObject != base.gameObject)
			{
				throw new Exception(string.Concat(new string[]
				{
					"Cannot destroy component: ",
					component.ToString(),
					" on GameObject: ",
					base.name,
					". Component MUST reside on same GameObject as DestroyOnRetailBuild."
				}));
			}
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x040039E3 RID: 14819
	[HelpBox("This monobehaviour will trigger because #RETAIL_BUILD is ON. For security reasons, this monobehaviour will only destroy objects that are on this GameObject.", HelpBoxMessageType.Warning)]
	[SerializeField]
	private bool m_destroyGameObject;

	// Token: 0x040039E4 RID: 14820
	[SerializeField]
	private Component[] m_componentsToDestroy = new Component[0];
}
