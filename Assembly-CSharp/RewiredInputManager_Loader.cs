using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CD6 RID: 3286
public class RewiredInputManager_Loader : MonoBehaviour, ILoadable
{
	// Token: 0x06005DA6 RID: 23974 RVA: 0x0003388E File Offset: 0x00031A8E
	private string GetPath()
	{
		return this.m_retailRewiredInputManagerPath;
	}

	// Token: 0x06005DA7 RID: 23975 RVA: 0x00033896 File Offset: 0x00031A96
	public void LoadSync()
	{
		UnityEngine.Object.Instantiate<GameObject>(CDGResources.Load<GameObject>(this.GetPath(), "", true), null);
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x000338B0 File Offset: 0x00031AB0
	public IEnumerator LoadAsync()
	{
		CDGAsyncLoadRequest<GameObject> req = CDGResources.LoadAsync<GameObject>(this.GetPath(), "");
		while (!req.IsDone)
		{
			yield return null;
		}
		UnityEngine.Object.Instantiate<GameObject>(req.Asset, null);
		yield break;
	}

	// Token: 0x04004D16 RID: 19734
	[SerializeField]
	private string m_debugRewiredInputManagerPath;

	// Token: 0x04004D17 RID: 19735
	[SerializeField]
	private string m_retailRewiredInputManagerPath;

	// Token: 0x04004D18 RID: 19736
	[SerializeField]
	private string m_gameCoreRewiredInputManagerPath;
}
