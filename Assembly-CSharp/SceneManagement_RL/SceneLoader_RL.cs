using System;
using System.Collections;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement_RL
{
	// Token: 0x020008C2 RID: 2242
	public class SceneLoader_RL : MonoBehaviour
	{
		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x00109983 File Offset: 0x00107B83
		// (set) Token: 0x06004989 RID: 18825 RVA: 0x0010998A File Offset: 0x00107B8A
		public static TransitionID CurrentTransitionID { get; private set; } = TransitionID.None;

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x00109992 File Offset: 0x00107B92
		// (set) Token: 0x0600498B RID: 18827 RVA: 0x00109999 File Offset: 0x00107B99
		private static SceneLoader_RL Instance { get; set; }

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x001099A1 File Offset: 0x00107BA1
		public static bool IsLoading
		{
			get
			{
				return SceneLoader_RL.m_loadSceneCoroutine != null;
			}
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x001099AB File Offset: 0x00107BAB
		public static bool IsRunningTransitionWithLogic
		{
			get
			{
				return SceneLoader_RL.m_runTransitionWithLogicCoroutine != null;
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x0600498E RID: 18830 RVA: 0x001099B5 File Offset: 0x00107BB5
		// (set) Token: 0x0600498F RID: 18831 RVA: 0x001099BC File Offset: 0x00107BBC
		public static string PreviousScene
		{
			get
			{
				return SceneLoader_RL.m_previousScene;
			}
			private set
			{
				SceneLoader_RL.m_previousScene = value;
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x06004990 RID: 18832 RVA: 0x001099C4 File Offset: 0x00107BC4
		// (set) Token: 0x06004991 RID: 18833 RVA: 0x001099CB File Offset: 0x00107BCB
		public static string CurrentScene
		{
			get
			{
				return SceneLoader_RL.m_currentScene;
			}
			private set
			{
				SceneLoader_RL.m_currentScene = value;
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x001099D4 File Offset: 0x00107BD4
		private void Awake()
		{
			if (SceneLoader_RL.Instance == null)
			{
				SceneLoader_RL.Instance = this;
				TransitionLibrary.InitializeTransitionInstances();
				SceneLoader_RL.CurrentScene = SceneManager.GetActiveScene().name;
				if (base.transform.parent == null)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
					return;
				}
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00109A35 File Offset: 0x00107C35
		private static IEnumerator TransitionIn(ISceneLoadingTransition transition)
		{
			SceneLoader_RL.CurrentTransitionID = transition.ID;
			SceneLoader_RL.TransitionStartRelay.Dispatch();
			if (transition != null)
			{
				transition.GameObject.SetActive(true);
				yield return null;
				yield return transition.TransitionIn();
			}
			yield break;
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00109A44 File Offset: 0x00107C44
		private static IEnumerator TransitionOut(ISceneLoadingTransition transition)
		{
			if (transition != null)
			{
				LevelManager_RL levelManager = UnityEngine.Object.FindObjectOfType<LevelManager_RL>();
				if (levelManager)
				{
					while (!levelManager.IsComplete)
					{
						yield return null;
					}
				}
				yield return null;
				yield return transition.TransitionOut();
				transition.GameObject.SetActive(false);
				levelManager = null;
			}
			SceneLoader_RL.CurrentTransitionID = TransitionID.None;
			SceneLoader_RL.TransitionCompleteRelay.Dispatch();
			yield break;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00109A54 File Offset: 0x00107C54
		public static void LoadScene(SceneID sceneID, TransitionID transitionID)
		{
			SceneLoader_RL.PreviousScene = SceneLoadingUtility.ActiveScene.name;
			SceneLoader_RL.CurrentScene = SceneLoadingUtility.GetSceneName(sceneID);
			if (SceneLoader_RL.m_loadSceneCoroutine == null)
			{
				SceneLoader_RL.m_loadSceneCoroutine = SceneLoader_RL.Instance.StartCoroutine(SceneLoader_RL.LoadSceneCoroutine(SceneLoader_RL.CurrentScene, transitionID));
				return;
			}
			Debug.LogFormat("<color=red>|{0}| Load Scene Coroutine is already running, but SHOULD NOT be</color>", new object[]
			{
				SceneLoader_RL.Instance
			});
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00109AB8 File Offset: 0x00107CB8
		private static IEnumerator LoadSceneAsynchronously(string sceneName)
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
			asyncOperation.allowSceneActivation = true;
			yield return asyncOperation;
			yield break;
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00109AC7 File Offset: 0x00107CC7
		private static IEnumerator LoadSceneCoroutine(string sceneName, TransitionID transitionID)
		{
			SceneLoader_RL.SceneLoadingStartRelay.Dispatch(sceneName);
			SceneLoader_RL.SetInputIsEnabled(false);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(9999f, false, false);
			}
			ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(transitionID) as ISceneLoadingTransition;
			yield return SceneLoader_RL.TransitionIn(transition);
			TweenManager.StopAllTweens(false);
			EffectManager.DisableAllEffects();
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdatePools, null, null);
			yield return SceneLoader_RL.LoadSceneAsynchronously(sceneName);
			global::AnimatorUtility.ClearAnimatorTables();
			float storedScale = Time.timeScale;
			Time.timeScale = 1f;
			if (SceneLoader_RL.m_runTransitionOutYield == null)
			{
				SceneLoader_RL.m_runTransitionOutYield = new WaitForSeconds(0.1f);
			}
			yield return SceneLoader_RL.m_runTransitionOutYield;
			Time.timeScale = storedScale;
			RLTimeScale.Reset();
			yield return SceneLoader_RL.TransitionOut(transition);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(1f, false, false);
			}
			SceneLoader_RL.SetInputIsEnabled(true);
			SceneLoader_RL.SceneLoadingEndRelay.Dispatch(sceneName);
			SceneLoader_RL.m_loadSceneCoroutine = null;
			yield break;
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00109ADD File Offset: 0x00107CDD
		public static void RunTransitionWithLogic(IEnumerator logicCoroutine, TransitionID transitionID, bool cleanup)
		{
			if (SceneLoader_RL.m_runTransitionWithLogicCoroutine != null)
			{
				SceneLoader_RL.Instance.StopCoroutine(SceneLoader_RL.m_runTransitionWithLogicCoroutine);
			}
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = SceneLoader_RL.Instance.StartCoroutine(SceneLoader_RL.RunTransitionWithLogicCoroutine(logicCoroutine, transitionID, cleanup));
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00109B0C File Offset: 0x00107D0C
		private static IEnumerator RunTransitionWithLogicCoroutine(IEnumerator logicCoroutine, TransitionID transitionID, bool cleanup)
		{
			SceneLoader_RL.SetInputIsEnabled(false);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(9999f, false, false);
			}
			ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(transitionID) as ISceneLoadingTransition;
			yield return SceneLoader_RL.TransitionIn(transition);
			if (cleanup)
			{
				TweenManager.StopAllTweens(false);
				yield return logicCoroutine;
				global::AnimatorUtility.ClearAnimatorTables();
			}
			else
			{
				yield return logicCoroutine;
			}
			yield return SceneLoader_RL.TransitionOut(transition);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(1f, false, false);
			}
			SceneLoader_RL.SetInputIsEnabled(true);
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = null;
			yield break;
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00109B29 File Offset: 0x00107D29
		public static void RunTransitionWithLogic(Action action, TransitionID transitionID, bool cleanup)
		{
			if (SceneLoader_RL.m_runTransitionWithLogicCoroutine != null)
			{
				SceneLoader_RL.Instance.StopCoroutine(SceneLoader_RL.m_runTransitionWithLogicCoroutine);
			}
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = SceneLoader_RL.Instance.StartCoroutine(SceneLoader_RL.RunTransitionWithLogicCoroutine(action, transitionID, cleanup));
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00109B58 File Offset: 0x00107D58
		private static IEnumerator RunTransitionWithLogicCoroutine(Action action, TransitionID transitionID, bool cleanup)
		{
			SceneLoader_RL.SetInputIsEnabled(false);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(9999f, false, false);
			}
			ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(transitionID) as ISceneLoadingTransition;
			yield return SceneLoader_RL.TransitionIn(transition);
			if (cleanup)
			{
				TweenManager.StopAllTweens(false);
				action();
				global::AnimatorUtility.ClearAnimatorTables();
			}
			else
			{
				action();
			}
			yield return SceneLoader_RL.TransitionOut(transition);
			if (PlayerManager.IsInstantiated && !PlayerManager.IsDisposed)
			{
				PlayerManager.GetPlayerController().CharacterHitResponse.SetInvincibleTime(1f, false, false);
			}
			SceneLoader_RL.SetInputIsEnabled(true);
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = null;
			yield break;
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00109B75 File Offset: 0x00107D75
		private static void SetInputIsEnabled(bool isEnabled)
		{
			if (ReInput.isReady)
			{
				RewiredMapController.SetCurrentMapEnabled(isEnabled);
			}
		}

		// Token: 0x04003DEF RID: 15855
		private static Coroutine m_loadSceneCoroutine = null;

		// Token: 0x04003DF0 RID: 15856
		private static Coroutine m_runTransitionWithLogicCoroutine = null;

		// Token: 0x04003DF1 RID: 15857
		private static WaitForSeconds m_runTransitionOutYield;

		// Token: 0x04003DF2 RID: 15858
		private static string m_previousScene;

		// Token: 0x04003DF3 RID: 15859
		private static string m_currentScene;

		// Token: 0x04003DF4 RID: 15860
		public static Relay<string> SceneLoadingStartRelay = new Relay<string>();

		// Token: 0x04003DF5 RID: 15861
		public static Relay<string> SceneLoadingEndRelay = new Relay<string>();

		// Token: 0x04003DF6 RID: 15862
		public static Relay TransitionStartRelay = new Relay();

		// Token: 0x04003DF7 RID: 15863
		public static Relay TransitionCompleteRelay = new Relay();
	}
}
