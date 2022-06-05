using System;
using UnityEngine;

// Token: 0x02000695 RID: 1685
public class DisablePooledObjectManager : MonoBehaviour
{
	// Token: 0x1700153A RID: 5434
	// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000D447D File Offset: 0x000D267D
	// (set) Token: 0x06003D2F RID: 15663 RVA: 0x000D4484 File Offset: 0x000D2684
	public static DisablePooledObjectManager Instance { get; private set; }

	// Token: 0x1700153B RID: 5435
	// (get) Token: 0x06003D30 RID: 15664 RVA: 0x000D448C File Offset: 0x000D268C
	// (set) Token: 0x06003D31 RID: 15665 RVA: 0x000D4493 File Offset: 0x000D2693
	public static bool IsInitialized { get; private set; }

	// Token: 0x06003D32 RID: 15666 RVA: 0x000D449B File Offset: 0x000D269B
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

	// Token: 0x06003D33 RID: 15667 RVA: 0x000D44C1 File Offset: 0x000D26C1
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

	// Token: 0x06003D34 RID: 15668 RVA: 0x000D44EB File Offset: 0x000D26EB
	private void OnDestroy()
	{
		DisablePooledObjectManager.Instance = null;
		DisablePooledObjectManager.IsInitialized = false;
	}

	// Token: 0x04002DD4 RID: 11732
	public static GameObject DisablePoolObjsHelper_STATIC;
}
