using System;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public struct CDGAsyncLoadRequest<T> where T : UnityEngine.Object
{
	// Token: 0x17001660 RID: 5728
	// (get) Token: 0x060041FC RID: 16892 RVA: 0x000EB3C7 File Offset: 0x000E95C7
	public T Asset
	{
		get
		{
			if (this.m_isAssetDatabase)
			{
				return this.m_asset;
			}
			return this.m_bundleRequest.asset as T;
		}
	}

	// Token: 0x17001661 RID: 5729
	// (get) Token: 0x060041FD RID: 16893 RVA: 0x000EB3ED File Offset: 0x000E95ED
	public bool IsDone
	{
		get
		{
			return this.m_isAssetDatabase || this.m_bundleRequest.isDone;
		}
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x000EB404 File Offset: 0x000E9604
	public CDGAsyncLoadRequest(bool isAssetDatabase, T asset, AssetBundleRequest request)
	{
		this.m_isAssetDatabase = isAssetDatabase;
		this.m_asset = asset;
		this.m_bundleRequest = request;
	}

	// Token: 0x04003942 RID: 14658
	private bool m_isAssetDatabase;

	// Token: 0x04003943 RID: 14659
	private T m_asset;

	// Token: 0x04003944 RID: 14660
	private AssetBundleRequest m_bundleRequest;
}
