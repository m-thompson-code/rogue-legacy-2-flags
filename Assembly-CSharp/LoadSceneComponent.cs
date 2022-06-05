using System;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class LoadSceneComponent : MonoBehaviour
{
	// Token: 0x0600215F RID: 8543 RVA: 0x00011C9B File Offset: 0x0000FE9B
	public void LoadScene()
	{
		SceneLoader_RL.LoadScene(this.m_sceneToLoad, this.m_transitionType);
	}

	// Token: 0x04001E46 RID: 7750
	[SerializeField]
	private SceneID m_sceneToLoad;

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	private TransitionID m_transitionType;
}
