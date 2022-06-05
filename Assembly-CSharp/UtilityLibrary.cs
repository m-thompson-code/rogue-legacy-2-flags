using System;
using UnityEngine;

// Token: 0x02000258 RID: 600
[CreateAssetMenu(menuName = "Custom/Libraries/Utility Library")]
public class UtilityLibrary : ScriptableObject
{
	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x0600179F RID: 6047 RVA: 0x00049786 File Offset: 0x00047986
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

	// Token: 0x060017A0 RID: 6048 RVA: 0x000497AE File Offset: 0x000479AE
	public static GameObject GetHitControllerBoxColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerBoxColliderGO;
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x000497BA File Offset: 0x000479BA
	public static GameObject GetHitControllerCircleColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerCircleColliderGO;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000497C6 File Offset: 0x000479C6
	public static GameObject GetHitControllerCapsuleColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerCapsuleColliderGO;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000497D2 File Offset: 0x000479D2
	public static GameObject GetHitControllerPolygonColliderGO()
	{
		return UtilityLibrary.Instance.m_hbControllerPolygonColliderGO;
	}

	// Token: 0x04001724 RID: 5924
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/UtilityLibrary";

	// Token: 0x04001725 RID: 5925
	[SerializeField]
	private GameObject m_hbControllerBoxColliderGO;

	// Token: 0x04001726 RID: 5926
	[SerializeField]
	private GameObject m_hbControllerCircleColliderGO;

	// Token: 0x04001727 RID: 5927
	[SerializeField]
	private GameObject m_hbControllerCapsuleColliderGO;

	// Token: 0x04001728 RID: 5928
	[SerializeField]
	private GameObject m_hbControllerPolygonColliderGO;

	// Token: 0x04001729 RID: 5929
	private static UtilityLibrary m_instance;
}
