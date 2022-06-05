using System;
using UnityEngine;

// Token: 0x02000B1E RID: 2846
public class DisablePooledObjectManager : MonoBehaviour
{
	// Token: 0x17001D06 RID: 7430
	// (get) Token: 0x060055C3 RID: 21955 RVA: 0x0002E9FC File Offset: 0x0002CBFC
	// (set) Token: 0x060055C4 RID: 21956 RVA: 0x0002EA03 File Offset: 0x0002CC03
	public static DisablePooledObjectManager Instance { get; private set; }

	// Token: 0x17001D07 RID: 7431
	// (get) Token: 0x060055C5 RID: 21957 RVA: 0x0002EA0B File Offset: 0x0002CC0B
	// (set) Token: 0x060055C6 RID: 21958 RVA: 0x0002EA12 File Offset: 0x0002CC12
	public static bool IsInitialized { get; private set; }

	// Token: 0x060055C7 RID: 21959 RVA: 0x0002EA1A File Offset: 0x0002CC1A
	private void Awake()
	{
		if (!DisablePooledObjectManager.Instance)
		{
			DisablePooledObjectManager.Instance = this;
			DisablePooledObjectManager.IsInitialized = true;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x0002EA40 File Offset: 0x0002CC40
	public static void DisablePooledObject(IGenericPoolObj pooledObj, bool reattachToOwnerPool = false)
	{
		if (!DisablePooledObjectManager.IsInitialized)
		{
			return;
		}
		if (pooledObj.gameObject.activeSelf)
		{
			pooledObj.gameObject.SetActive(false);
		}
		pooledObj.IsFreePoolObj = true;
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x0002EA6A File Offset: 0x0002CC6A
	private void OnDestroy()
	{
		DisablePooledObjectManager.Instance = null;
		DisablePooledObjectManager.IsInitialized = false;
	}

	// Token: 0x04003F9F RID: 16287
	public static GameObject DisablePoolObjsHelper_STATIC;
}
