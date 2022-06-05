using System;
using UnityEngine;

// Token: 0x02000C65 RID: 3173
public struct CDGAsyncLoadRequest<T> where T : UnityEngine.Object
{
	// Token: 0x17001E5E RID: 7774
	// (get) Token: 0x06005B85 RID: 23429 RVA: 0x000322C4 File Offset: 0x000304C4
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

	// Token: 0x17001E5F RID: 7775
	// (get) Token: 0x06005B86 RID: 23430 RVA: 0x000322EA File Offset: 0x000304EA
	public bool IsDone
	{
		get
		{
			return this.m_isAssetDatabase || this.m_bundleRequest.isDone;
		}
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x00032301 File Offset: 0x00030501
	public CDGAsyncLoadRequest(bool isAssetDatabase, T asset, AssetBundleRequest request)
	{
		this.m_isAssetDatabase = isAssetDatabase;
		this.m_asset = asset;
		this.m_bundleRequest = request;
	}

	// Token: 0x04004C07 RID: 19463
	private bool m_isAssetDatabase;

	// Token: 0x04004C08 RID: 19464
	private T m_asset;

	// Token: 0x04004C09 RID: 19465
	private AssetBundleRequest m_bundleRequest;
}
