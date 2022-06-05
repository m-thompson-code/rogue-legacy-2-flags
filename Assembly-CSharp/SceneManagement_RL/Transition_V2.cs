using System;
using System.Collections;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008CC RID: 2252
	public abstract class Transition_V2 : MonoBehaviour, ITransition
	{
		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x0010A497 File Offset: 0x00108697
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x0010A49F File Offset: 0x0010869F
		protected virtual void Awake()
		{
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x060049E0 RID: 18912
		public abstract TransitionID ID { get; }

		// Token: 0x060049E1 RID: 18913
		public abstract IEnumerator Run();
	}
}
