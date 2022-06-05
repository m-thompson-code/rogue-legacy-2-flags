using System;
using System.Collections;
using Rewired;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement_RL
{
	// Token: 0x02000E0D RID: 3597
	public class SceneLoader_RL : MonoBehaviour
	{
		// Token: 0x17002090 RID: 8336
		// (get) Token: 0x06006569 RID: 25961 RVA: 0x00037EA8 File Offset: 0x000360A8
		// (set) Token: 0x0600656A RID: 25962 RVA: 0x00037EAF File Offset: 0x000360AF
		public static TransitionID CurrentTransitionID { get; private set; } = TransitionID.None;

		// Token: 0x17002091 RID: 8337
		// (get) Token: 0x0600656B RID: 25963 RVA: 0x00037EB7 File Offset: 0x000360B7
		// (set) Token: 0x0600656C RID: 25964 RVA: 0x00037EBE File Offset: 0x000360BE
		private static SceneLoader_RL Instance { get; set; }

		// Token: 0x17002092 RID: 8338
		// (get) Token: 0x0600656D RID: 25965 RVA: 0x00037EC6 File Offset: 0x000360C6
		public static bool IsLoading
		{
			get
			{
				return SceneLoader_RL.m_loadSceneCoroutine != null;
			}
		}

		// Token: 0x17002093 RID: 8339
		// (get) Token: 0x0600656E RID: 25966 RVA: 0x00037ED0 File Offset: 0x000360D0
		public static bool IsRunningTransitionWithLogic
		{
			get
			{
				return SceneLoader_RL.m_runTransitionWithLogicCoroutine != null;
			}
		}

		// Token: 0x17002094 RID: 8340
		// (get) Token: 0x0600656F RID: 25967 RVA: 0x00037EDA File Offset: 0x000360DA
		// (set) Token: 0x06006570 RID: 25968 RVA: 0x00037EE1 File Offset: 0x000360E1
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

		// Token: 0x17002095 RID: 8341
		// (get) Token: 0x06006571 RID: 25969 RVA: 0x00037EE9 File Offset: 0x000360E9
		// (set) Token: 0x06006572 RID: 25970 RVA: 0x00037EF0 File Offset: 0x000360F0
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

		// Token: 0x06006573 RID: 25971 RVA: 0x00178DF4 File Offset: 0x00176FF4
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

		// Token: 0x06006574 RID: 25972 RVA: 0x00037EF8 File Offset: 0x000360F8
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

		// Token: 0x06006575 RID: 25973 RVA: 0x00037F07 File Offset: 0x00036107
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

		// Token: 0x06006576 RID: 25974 RVA: 0x00178E58 File Offset: 0x00177058
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

		// Token: 0x06006577 RID: 25975 RVA: 0x00037F16 File Offset: 0x00036116
		private static IEnumerator LoadSceneAsynchronously(string sceneName)
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
			asyncOperation.allowSceneActivation = true;
			yield return asyncOperation;
			yield break;
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x00037F25 File Offset: 0x00036125
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

		// Token: 0x06006579 RID: 25977 RVA: 0x00037F3B File Offset: 0x0003613B
		public static void RunTransitionWithLogic(IEnumerator logicCoroutine, TransitionID transitionID, bool cleanup)
		{
			if (SceneLoader_RL.m_runTransitionWithLogicCoroutine != null)
			{
				SceneLoader_RL.Instance.StopCoroutine(SceneLoader_RL.m_runTransitionWithLogicCoroutine);
			}
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = SceneLoader_RL.Instance.StartCoroutine(SceneLoader_RL.RunTransitionWithLogicCoroutine(logicCoroutine, transitionID, cleanup));
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x00037F6A File Offset: 0x0003616A
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

		// Token: 0x0600657B RID: 25979 RVA: 0x00037F87 File Offset: 0x00036187
		public static void RunTransitionWithLogic(Action action, TransitionID transitionID, bool cleanup)
		{
			if (SceneLoader_RL.m_runTransitionWithLogicCoroutine != null)
			{
				SceneLoader_RL.Instance.StopCoroutine(SceneLoader_RL.m_runTransitionWithLogicCoroutine);
			}
			SceneLoader_RL.m_runTransitionWithLogicCoroutine = SceneLoader_RL.Instance.StartCoroutine(SceneLoader_RL.RunTransitionWithLogicCoroutine(action, transitionID, cleanup));
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x00037FB6 File Offset: 0x000361B6
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

		// Token: 0x0600657D RID: 25981 RVA: 0x00037FD3 File Offset: 0x000361D3
		private static void SetInputIsEnabled(bool isEnabled)
		{
			if (ReInput.isReady)
			{
				RewiredMapController.SetCurrentMapEnabled(isEnabled);
			}
		}

		// Token: 0x04005282 RID: 21122
		private static Coroutine m_loadSceneCoroutine = null;

		// Token: 0x04005283 RID: 21123
		private static Coroutine m_runTransitionWithLogicCoroutine = null;

		// Token: 0x04005284 RID: 21124
		private static WaitForSeconds m_runTransitionOutYield;

		// Token: 0x04005285 RID: 21125
		private static string m_previousScene;

		// Token: 0x04005286 RID: 21126
		private static string m_currentScene;

		// Token: 0x04005287 RID: 21127
		public static Relay<string> SceneLoadingStartRelay = new Relay<string>();

		// Token: 0x04005288 RID: 21128
		public static Relay<string> SceneLoadingEndRelay = new Relay<string>();

		// Token: 0x04005289 RID: 21129
		public static Relay TransitionStartRelay = new Relay();

		// Token: 0x0400528A RID: 21130
		public static Relay TransitionCompleteRelay = new Relay();
	}
}
