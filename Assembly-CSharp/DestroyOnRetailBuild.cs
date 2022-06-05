using System;
using UnityEngine;

// Token: 0x02000CB9 RID: 3257
public class DestroyOnRetailBuild : MonoBehaviour
{
	// Token: 0x17001EE7 RID: 7911
	// (get) Token: 0x06005D3A RID: 23866 RVA: 0x000334B8 File Offset: 0x000316B8
	// (set) Token: 0x06005D3B RID: 23867 RVA: 0x000334C0 File Offset: 0x000316C0
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

	// Token: 0x06005D3C RID: 23868 RVA: 0x0015AA2C File Offset: 0x00158C2C
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

	// Token: 0x04004CA8 RID: 19624
	[HelpBox("This monobehaviour will trigger because #RETAIL_BUILD is ON. For security reasons, this monobehaviour will only destroy objects that are on this GameObject.", HelpBoxMessageType.Warning)]
	[SerializeField]
	private bool m_destroyGameObject;

	// Token: 0x04004CA9 RID: 19625
	[SerializeField]
	private Component[] m_componentsToDestroy = new Component[0];
}
