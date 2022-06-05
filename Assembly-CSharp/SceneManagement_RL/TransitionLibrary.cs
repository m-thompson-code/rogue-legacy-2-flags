using System;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement_RL
{
	// Token: 0x02000E0C RID: 3596
	[CreateAssetMenu(menuName = "Custom/Transition Library")]
	public class TransitionLibrary : ScriptableObject
	{
		// Token: 0x1700208F RID: 8335
		// (get) Token: 0x06006564 RID: 25956 RVA: 0x00037E67 File Offset: 0x00036067
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

		// Token: 0x06006565 RID: 25957 RVA: 0x00178D30 File Offset: 0x00176F30
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

		// Token: 0x06006566 RID: 25958 RVA: 0x00178D9C File Offset: 0x00176F9C
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

		// Token: 0x0400527D RID: 21117
		[SerializeField]
		private Transition_V2[] m_transitions;

		// Token: 0x0400527E RID: 21118
		private const string RESOURCES_PATH = "Scriptable Objects/Libraries/TransitionLibrary";

		// Token: 0x0400527F RID: 21119
		public static string ASSET_PATH = "Assets/Content/Scriptable Objects/Libraries/TransitionLibrary.asset";

		// Token: 0x04005280 RID: 21120
		private static Dictionary<TransitionID, ITransition> m_transitionTable = null;

		// Token: 0x04005281 RID: 21121
		private static TransitionLibrary m_instance = null;
	}
}
