using System;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x020008C1 RID: 2241
	[CreateAssetMenu(menuName = "Custom/Transition Library")]
	public class TransitionLibrary : ScriptableObject
	{
		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x06004983 RID: 18819 RVA: 0x00109877 File Offset: 0x00107A77
		private static TransitionLibrary Instance
		{
			get
			{
				if (TransitionLibrary.m_instance == null)
				{
					TransitionLibrary.m_instance = CDGResources.Load<TransitionLibrary>("Scriptable Objects/Libraries/TransitionLibrary", "", true);
				}
				return TransitionLibrary.m_instance;
			}
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x001098A0 File Offset: 0x00107AA0
		public static void InitializeTransitionInstances()
		{
			if (TransitionLibrary.m_transitionTable == null)
			{
				TransitionLibrary.m_transitionTable = new Dictionary<TransitionID, ITransition>();
				Transition_V2[] transitions = TransitionLibrary.Instance.m_transitions;
				for (int i = 0; i < transitions.Length; i++)
				{
					Transition_V2 transition_V = UnityEngine.Object.Instantiate<Transition_V2>(transitions[i] as Transition_V2, null);
					TransitionLibrary.m_transitionTable.Add(transition_V.ID, transition_V);
					transition_V.gameObject.SetActive(false);
					UnityEngine.Object.DontDestroyOnLoad(transition_V);
				}
			}
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x0010990C File Offset: 0x00107B0C
		public static ITransition GetTransitionInstance(TransitionID transitionID)
		{
			if (TransitionLibrary.m_transitionTable == null)
			{
				TransitionLibrary.InitializeTransitionInstances();
			}
			if (TransitionLibrary.m_transitionTable.ContainsKey(transitionID))
			{
				return TransitionLibrary.m_transitionTable[transitionID];
			}
			if (transitionID != TransitionID.None)
			{
				Debug.LogFormat("<color=red>|{0}| Transition Table does not contain any entry for Transition ID ({1})</color>", new object[]
				{
					TransitionLibrary.Instance,
					transitionID
				});
			}
			return null;
		}

		// Token: 0x04003DEA RID: 15850
		[SerializeField]
		private Transition_V2[] m_transitions;

		// Token: 0x04003DEB RID: 15851
		private const string RESOURCES_PATH = "Scriptable Objects/Libraries/TransitionLibrary";

		// Token: 0x04003DEC RID: 15852
		public static string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/TransitionLibrary.asset";

		// Token: 0x04003DED RID: 15853
		private static Dictionary<TransitionID, ITransition> m_transitionTable = null;

		// Token: 0x04003DEE RID: 15854
		private static TransitionLibrary m_instance = null;
	}
}
