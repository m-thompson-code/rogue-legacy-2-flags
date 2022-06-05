using System;
using System.Collections;

namespace SceneManagement_RL
{
	// Token: 0x02000E35 RID: 3637
	public interface ISceneLoadingTransition : ITransition
	{
		// Token: 0x06006677 RID: 26231
		IEnumerator TransitionOut();

		// Token: 0x06006678 RID: 26232
		IEnumerator TransitionIn();
	}
}
