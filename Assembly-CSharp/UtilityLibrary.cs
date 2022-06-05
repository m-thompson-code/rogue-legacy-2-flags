using System;
using UnityEngine;

// Token: 0x02000417 RID: 1047
[CreateAssetMenu(menuName = "Custom/Libraries/Utility Library")]
public class UtilityLibrary : ScriptableObject
{
	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06002158 RID: 8536 RVA: 0x00011C43 File Offset: 0x0000FE43
	private static UtilityLibrary Instance
	{
		get
		{
			if (!UtilityLibrary.m_instance)
			{
				UtilityLibrary.m_instance = CDGResources.Load<UtilityLibrary>("Scriptable Objects/Libraries/UtilityLibrary", "", true);
			}
			return UtilityLibrary.m_instance;
		}
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x00011C6B File Offset: 0x0000FE6B
	public static GameObject GetHitControllerBoxColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerBoxColliderGO;
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x00011C77 File Offset: 0x0000FE77
	public static GameObject GetHitControllerCircleColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerCircleColliderGO;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x00011C83 File Offset: 0x0000FE83
	public static GameObject GetHitControllerCapsuleColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerCapsuleColliderGO;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x00011C8F File Offset: 0x0000FE8F
	public static GameObject GetHitControllerPolygonColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerPolygonColliderGO;
	}

	// Token: 0x04001E40 RID: 7744
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/UtilityLibrary";

	// Token: 0x04001E41 RID: 7745
	[SerializeField]
	private GameObject m_hbControllerBoxColliderGO;

	// Token: 0x04001E42 RID: 7746
	[SerializeField]
	private GameObject m_hbControllerCircleColliderGO;

	// Token: 0x04001E43 RID: 7747
	[SerializeField]
	private GameObject m_hbControllerCapsuleColliderGO;

	// Token: 0x04001E44 RID: 7748
	[SerializeField]
	private GameObject m_hbControllerPolygonColliderGO;

	// Token: 0x04001E45 RID: 7749
	private static UtilityLibrary m_instance;
}
