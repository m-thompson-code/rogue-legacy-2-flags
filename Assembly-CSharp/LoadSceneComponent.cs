using System;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class LoadSceneComponent : MonoBehaviour
{
	// Token: 0x060017A6 RID: 6054 RVA: 0x000497E8 File Offset: 0x000479E8
	public void LoadScene()
	{
		SceneLoader_RL.LoadScene(this.m_sceneToLoad, this.m_transitionType);
	}

	// Token: 0x0400172A RID: 5930
	[SerializeField]
	private SceneID m_sceneToLoad;

	// Token: 0x0400172B RID: 5931
	[SerializeField]
	private TransitionID m_transitionType;
}
