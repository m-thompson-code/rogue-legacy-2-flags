using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E36 RID: 3638
	public abstract class Transition_V2 : MonoBehaviour, ITransition
	{
		// Token: 0x170020DF RID: 8415
		// (get) Token: 0x06006679 RID: 26233 RVA: 0x00003713 File Offset: 0x00001913
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x00002FCA File Offset: 0x000011CA
		protected virtual void Awake()
		{
		}

		// Token: 0x170020E0 RID: 8416
		// (get) Token: 0x0600667B RID: 26235
		public abstract TransitionID ID { get; }

		// Token: 0x0600667C RID: 26236
		public abstract IEnumerator Run();
	}
}
