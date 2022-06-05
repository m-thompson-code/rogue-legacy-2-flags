using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008CA RID: 2250
	public interface ITransition
	{
		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x060049D9 RID: 18905
		GameObject GameObject { get; }

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x060049DA RID: 18906
		TransitionID ID { get; }

		// Token: 0x060049DB RID: 18907
		IEnumerator Run();
	}
}
