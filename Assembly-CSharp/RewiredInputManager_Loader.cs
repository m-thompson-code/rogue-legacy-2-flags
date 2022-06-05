using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200080C RID: 2060
public class RewiredInputManager_Loader : MonoBehaviour, ILoadable
{
	// Token: 0x0600441B RID: 17435 RVA: 0x000F11E6 File Offset: 0x000EF3E6
	private string GetPath()
	{
		return this.m_retailRewiredInputManagerPath;
	}

	// Token: 0x0600441C RID: 17436 RVA: 0x000F11EE File Offset: 0x000EF3EE
	public void LoadSync()
	{
		UnityEngine.Object.Instantiate<GameObject>(CDGResources.Load<GameObject>(this.GetPath(), "", true), null);
	}

	// Token: 0x0600441D RID: 17437 RVA: 0x000F1208 File Offset: 0x000EF408
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

	// Token: 0x04003A3E RID: 14910
	[SerializeField]
	private string m_debugRewiredInputManagerPath;

	// Token: 0x04003A3F RID: 14911
	[SerializeField]
	private string m_retailRewiredInputManagerPath;

	// Token: 0x04003A40 RID: 14912
	[SerializeField]
	private string m_gameCoreRewiredInputManagerPath;
}
