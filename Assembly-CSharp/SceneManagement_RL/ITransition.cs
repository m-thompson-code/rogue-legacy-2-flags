using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E34 RID: 3636
	public interface ITransition
	{
		// Token: 0x170020DD RID: 8413
		// (get) Token: 0x06006674 RID: 26228
		GameObject GameObject { get; }

		// Token: 0x170020DE RID: 8414
		// (get) Token: 0x06006675 RID: 26229
		TransitionID ID { get; }

		// Token: 0x06006676 RID: 26230
		IEnumerator Run();
	}
}
