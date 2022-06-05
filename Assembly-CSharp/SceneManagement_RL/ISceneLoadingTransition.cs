using System;
using System.Collections;

namespace SceneManagement_RL
{
	// Token: 0x020008CB RID: 2251
	public interface ISceneLoadingTransition : ITransition
	{
		// Token: 0x060049DC RID: 18908
		IEnumerator TransitionOut();

		// Token: 0x060049DD RID: 18909
		IEnumerator TransitionIn();
	}
}
