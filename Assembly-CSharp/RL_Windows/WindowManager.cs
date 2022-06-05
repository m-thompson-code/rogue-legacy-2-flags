using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace RL_Windows
{
	// Token: 0x020008BF RID: 2239
	public class WindowManager : MonoBehaviour
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06004956 RID: 18774 RVA: 0x0010889C File Offset: 0x00106A9C
		// (remove) Token: 0x06004957 RID: 18775 RVA: 0x001088D0 File Offset: 0x00106AD0
		public static event EventHandler<WindowStateChangeEventArgs> WindowStateChangeEvent;

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06004958 RID: 18776 RVA: 0x00108903 File Offset: 0x00106B03
		// (set) Token: 0x06004959 RID: 18777 RVA: 0x0010890A File Offset: 0x00106B0A
		private static WindowManager Instance { get; set; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x0600495A RID: 18778 RVA: 0x00108914 File Offset: 0x00106B14
		public static EventSystem EventSystem
		{
			get
			{
				if (WindowManager.m_eventSystem == null)
				{
					WindowManager.m_eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
					if (WindowManager.m_eventSystem == null)
					{
						Debug.LogFormat("<color=red>[WindowManager] Failed to find EventSystem</color>", new object[]
						{
							WindowManager.PREFAB_PATH
						});
					}
				}
				return WindowManager.m_eventSystem;
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x0600495B RID: 18779 RVA: 0x00108962 File Offset: 0x00106B62
		public static Dictionary<SceneID, List<WindowID>> SceneIDToLoadedWindowIDsTable
		{
			get
			{
				return WindowManager.SCENE_ID_TO_LOADED_WINDOW_IDS_TABLE;
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x00108969 File Offset: 0x00106B69
		public static WindowController ActiveWindow
		{
			get
			{
				if (WindowManager.OpenWindows != null && WindowManager.OpenWindows.Count > 0)
				{
					return WindowManager.OpenWindows.Last<WindowController>();
				}
				return null;
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x0600495D RID: 18781 RVA: 0x0010898B File Offset: 0x00106B8B
		// (set) Token: 0x0600495E RID: 18782 RVA: 0x00108992 File Offset: 0x00106B92
		public static List<WindowController> OpenWindows
		{
			get
			{
				return WindowManager.m_openWindows;
			}
			private set
			{
				WindowManager.m_openWindows = value;
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x0010899A File Offset: 0x00106B9A
		// (set) Token: 0x06004960 RID: 18784 RVA: 0x001089A1 File Offset: 0x00106BA1
		public static List<WindowController> LoadedWindows
		{
			get
			{
				return WindowManager.m_loadedWindows;
			}
			private set
			{
				WindowManager.m_loadedWindows = value;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x06004961 RID: 18785 RVA: 0x001089A9 File Offset: 0x00106BA9
		public static bool IsInstantiated
		{
			get
			{
				return WindowManager.Instance != null;
			}
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x001089B8 File Offset: 0x00106BB8
		private void Awake()
		{
			if (WindowManager.Instance && WindowManager.Instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			WindowManager.Instance = this;
			if (!base.transform.parent)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x00108A10 File Offset: 0x00106C10
		private void Start()
		{
			SceneManager.sceneLoaded -= WindowManager.OnSceneLoaded;
			SceneManager.sceneLoaded += WindowManager.OnSceneLoaded;
			if (ReInput.isReady)
			{
				ReInput.players.GetPlayer(0).AddInputEventDelegate(new Action<InputActionEventData>(this.OnStartButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Pause");
				ReInput.players.GetPlayer(0).AddInputEventDelegate(new Action<InputActionEventData>(this.OnSelectButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Select");
			}
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x00108A8C File Offset: 0x00106C8C
		private void OnDestroy()
		{
			if (WindowManager.Instance != null && WindowManager.Instance == this)
			{
				SceneManager.sceneLoaded -= WindowManager.OnSceneLoaded;
				if (ReInput.isReady)
				{
					ReInput.players.GetPlayer(0).RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnStartButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Pause");
					ReInput.players.GetPlayer(0).RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnSelectButtonDown), UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Select");
				}
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x00108B11 File Offset: 0x00106D11
		private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			WindowManager.LoadSceneWindows(scene);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00108B1C File Offset: 0x00106D1C
		private void OnSelectButtonDown(InputActionEventData obj)
		{
			if (WindowManager.DISABLED_PAUSE_SCENES.Contains(SceneManager.GetActiveScene().name))
			{
				return;
			}
			if (WindowManager.GetIsWindowLoaded(WindowID.Map))
			{
				(WindowManager.LoadedWindows.Single((WindowController x) => x.ID == WindowID.Pause) as PauseWindowController).SetStartingSubWindow(WindowID.Map);
				WindowManager.SetWindowIsOpen(WindowID.Pause, true);
				return;
			}
			WindowManager.SetWindowIsOpen(WindowID.Pause, true);
			(WindowManager.LoadedWindows.Single((WindowController x) => x.ID == WindowID.Pause) as PauseWindowController).SetSelectedTab(WindowID.Map);
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00108BC4 File Offset: 0x00106DC4
		private void OnStartButtonDown(InputActionEventData obj)
		{
			if (WindowManager.DISABLED_PAUSE_SCENES.Contains(SceneManager.GetActiveScene().name))
			{
				return;
			}
			WindowManager.SetWindowIsOpen(WindowID.Pause, true);
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x00108BF4 File Offset: 0x00106DF4
		public static void CloseAllOpenWindows()
		{
			WindowManager.m_openWindowHelper.Clear();
			WindowManager.m_openWindowHelper.AddRange(WindowManager.OpenWindows);
			foreach (WindowController windowController in WindowManager.m_openWindowHelper)
			{
				WindowManager.SetWindowIsOpen(windowController.ID, false);
			}
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x00108C64 File Offset: 0x00106E64
		private static void CloseWindow(WindowID windowID)
		{
			WindowController windowController = WindowManager.OpenWindows.Single((WindowController x) => x.ID == windowID);
			if (WindowManager.OpenWindows.Count <= 1)
			{
				RewiredMapController.SetMap(GameInputMode.Game);
			}
			if (SceneLoader_RL.IsLoading)
			{
				RewiredMapController.SetCurrentMapEnabled(false);
			}
			windowController.SetIsOpen(false);
			windowController.SetHasFocus(false);
			WindowManager.OpenWindows.Remove(windowController);
			WindowManager.UpdateOpenWindowsSortOrder();
			bool flag = false;
			if (WindowManager.OpenWindows.Count > 0)
			{
				WindowManager.EventSystem.SetSelectedGameObject(null);
				flag = WindowManager.OpenWindows.Any((WindowController openWindow) => openWindow.PauseGameWhenOpen);
				WindowManager.OpenWindows.Last<WindowController>().SetHasFocus(true);
			}
			if (GameManager.IsGamePaused && !flag)
			{
				GameManager.SetIsPaused(false);
			}
			if (WindowManager.WindowStateChangeEvent != null)
			{
				WindowManager.WindowStateChangeEvent(WindowManager.Instance, new WindowStateChangeEventArgs(windowID, true));
			}
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x00108D5C File Offset: 0x00106F5C
		private static void CreateWindowInstanceAndAddToLoadedWindows(WindowID windowID)
		{
			WindowController windowController = WindowManager.Instance.m_windowControllerPrefabs.SingleOrDefault((WindowController prefab) => prefab.ID == windowID);
			if (windowController != null)
			{
				WindowController windowController2 = UnityEngine.Object.Instantiate<WindowController>(windowController, WindowManager.Instance.transform);
				windowController2.Initialize();
				WindowManager.LoadedWindows.Add(windowController2);
				return;
			}
			Debug.LogFormat("<color=red>[{0}] Couldn't find WindowController Prefab with ID ({1}) in m_windowControllerPrefabs. Did you add the Window's Prefab to the WindowManager Prefab's m_windowControllerPrefabs list?</color>", new object[]
			{
				WindowManager.Instance,
				windowID
			});
			foreach (WindowController windowController3 in WindowManager.Instance.m_windowControllerPrefabs)
			{
				Debug.LogFormat("<color=red>[{0}] ID = {1}</color>", new object[]
				{
					WindowManager.Instance,
					windowController3.ID
				});
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x00108E30 File Offset: 0x00107030
		private static void UpdateOpenWindowsSortOrder()
		{
			for (int i = 0; i < WindowManager.OpenWindows.Count; i++)
			{
				WindowController windowController = WindowManager.OpenWindows[i];
				if (windowController.SortOrderOverride == -1)
				{
					windowController.WindowCanvas.sortingOrder = (i + 1) * 10;
				}
				else
				{
					windowController.WindowCanvas.sortingOrder = windowController.SortOrderOverride;
				}
			}
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00108E8C File Offset: 0x0010708C
		public static bool GetIsWindowLoaded(WindowID windowID)
		{
			using (List<WindowController>.Enumerator enumerator = WindowManager.LoadedWindows.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == windowID)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00108EE8 File Offset: 0x001070E8
		public static bool GetIsWindowOpen(WindowID windowID)
		{
			using (List<WindowController>.Enumerator enumerator = WindowManager.OpenWindows.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ID == windowID)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x0600496E RID: 18798 RVA: 0x00108F44 File Offset: 0x00107144
		public static bool IsAnyWindowOpen
		{
			get
			{
				return WindowManager.OpenWindows.Count > 0;
			}
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x00108F54 File Offset: 0x00107154
		public static WindowController GetWindowController(WindowID windowID)
		{
			if (WindowManager.GetIsWindowLoaded(windowID))
			{
				return WindowManager.LoadedWindows.Single((WindowController x) => x.ID == windowID);
			}
			return null;
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00108F94 File Offset: 0x00107194
		private static List<WindowID> GetWindowsToLoad(SceneID sceneID)
		{
			List<WindowID> result = null;
			if (WindowManager.SceneIDToLoadedWindowIDsTable.TryGetValue(sceneID, out result))
			{
				return result;
			}
			return new List<WindowID>();
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x00108FBC File Offset: 0x001071BC
		private static List<WindowID> GetWindowsToUnload(List<WindowID> windowsToLoad)
		{
			List<WindowID> list = new List<WindowID>();
			foreach (WindowController windowController in WindowManager.LoadedWindows)
			{
				if (!windowsToLoad.Contains(windowController.ID))
				{
					list.Add(windowController.ID);
				}
			}
			return list;
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00109028 File Offset: 0x00107228
		private static void InitialiseWindowsInActiveScene()
		{
			WindowController[] array = (from windowController in UnityEngine.Object.FindObjectsOfType<WindowController>()
			where windowController.gameObject.scene.buildIndex != -1
			select windowController).ToArray<WindowController>();
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].transform.IsChildOf(WindowManager.Instance.transform))
				{
					array[i].transform.SetParent(WindowManager.Instance.transform);
				}
				if (!WindowManager.LoadedWindows.Contains(array[i]))
				{
					WindowManager.LoadedWindows.Add(array[i]);
				}
				array[i].Initialize();
			}
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x001090C8 File Offset: 0x001072C8
		public static void LoadSceneWindows(Scene scene)
		{
			WindowManager.InitialiseWindowsInActiveScene();
			SceneID sceneID = SceneLoadingUtility.GetSceneID(scene.name);
			if (sceneID != SceneID.None && WindowManager.SceneIDToLoadedWindowIDsTable.ContainsKey(sceneID))
			{
				RewiredMapController.SetMap(GameInputMode.Game);
				if (SceneLoader_RL.IsLoading)
				{
					RewiredMapController.SetCurrentMapEnabled(false);
				}
				List<WindowID> windowsToLoad = WindowManager.GetWindowsToLoad(sceneID);
				WindowManager.UnloadWindows(WindowManager.GetWindowsToUnload(windowsToLoad));
				foreach (WindowController windowController in WindowManager.LoadedWindows)
				{
					WindowManager.SetWindowIsOpen(windowController.ID, false);
				}
				using (List<WindowID>.Enumerator enumerator2 = windowsToLoad.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						WindowID windowID = enumerator2.Current;
						if (!WindowManager.GetIsWindowLoaded(windowID))
						{
							WindowManager.CreateWindowInstanceAndAddToLoadedWindows(windowID);
						}
						WindowManager.SetWindowIsOpen(windowID, WindowManager.LoadedWindows.Single((WindowController x) => x.ID == windowID).OpenOnSceneLoad);
					}
				}
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x001091F0 File Offset: 0x001073F0
		public static void LoadWindow(WindowID windowID)
		{
			WindowManager.CreateWindowInstanceAndAddToLoadedWindows(windowID);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x001091F8 File Offset: 0x001073F8
		private static void OpenWindow(WindowID windowID)
		{
			if (WindowManager.LoadedWindows.Exists((WindowController x) => x.ID == windowID))
			{
				WindowManager.EventSystem.SetSelectedGameObject(null);
				if (WindowManager.ActiveWindow != null)
				{
					WindowManager.ActiveWindow.SetHasFocus(false);
				}
				WindowController windowController = WindowManager.LoadedWindows.Single((WindowController x) => x.ID == windowID);
				WindowManager.OpenWindows.Add(windowController);
				WindowManager.UpdateOpenWindowsSortOrder();
				if (!GameManager.IsGamePaused && windowController.PauseGameWhenOpen)
				{
					GameManager.SetIsPaused(true);
				}
				RewiredMapController.SetMap(GameInputMode.Window);
				if (SceneLoader_RL.IsLoading)
				{
					RewiredMapController.SetCurrentMapEnabled(false);
				}
				windowController.SetIsOpen(true);
				windowController.SetHasFocus(true);
				if (WindowManager.WindowStateChangeEvent != null)
				{
					WindowManager.WindowStateChangeEvent(WindowManager.Instance, new WindowStateChangeEventArgs(windowID, true));
					return;
				}
			}
			else
			{
				Debug.LogFormat("<color=red>[{0}] LoadedWindows does not contain an entry with Key ({1})</color>", new object[]
				{
					WindowManager.Instance,
					windowID
				});
			}
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x001092F7 File Offset: 0x001074F7
		public static void SetWindowIsOpen(WindowID windowID, bool isOpen)
		{
			if (isOpen)
			{
				if (!WindowManager.GetIsWindowLoaded(windowID))
				{
					WindowManager.CreateWindowInstanceAndAddToLoadedWindows(windowID);
				}
				if (WindowManager.GetIsWindowLoaded(windowID) && !WindowManager.GetIsWindowOpen(windowID))
				{
					WindowManager.OpenWindow(windowID);
					return;
				}
			}
			else if (WindowManager.GetIsWindowLoaded(windowID) && WindowManager.GetIsWindowOpen(windowID))
			{
				WindowManager.CloseWindow(windowID);
			}
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00109338 File Offset: 0x00107538
		private static void UnloadWindows(List<WindowID> windowsToUnload)
		{
			foreach (WindowID windowID in windowsToUnload)
			{
				WindowController windowController = WindowManager.GetWindowController(windowID);
				if (windowController != null && windowController.gameObject != null)
				{
					if (WindowManager.GetIsWindowOpen(windowID))
					{
						WindowManager.CloseWindow(windowID);
					}
					if (WindowManager.GetIsWindowLoaded(windowID))
					{
						WindowManager.LoadedWindows.Remove(windowController);
					}
					UnityEngine.Object.Destroy(windowController.gameObject);
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | Entry with ID ({1}) in Loaded Windows Table is null. If Window is Scene specific, its Destroy on Load Field should be set to True.</color>", new object[]
					{
						WindowManager.Instance,
						windowID
					});
				}
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x001093F0 File Offset: 0x001075F0
		private static bool CanPauseNow
		{
			get
			{
				return RewiredMapController.IsMapEnabled(GameInputMode.Game) && WorldBuilder.Instance != null && WorldBuilder.State == BiomeBuildStateID.Complete;
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x00109412 File Offset: 0x00107612
		private static bool IsAlreadyPaused
		{
			get
			{
				return RewiredMapController.IsMapEnabled(GameInputMode.Window) && !WindowManager.GetIsWindowOpen(WindowID.BossIntro);
			}
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00109429 File Offset: 0x00107629
		public static void PauseWhenPossible(bool shouldPause, bool gamepadDisconnected = false)
		{
			if (shouldPause)
			{
				if (WindowManager.CanPauseNow)
				{
					WindowManager.ForcePause();
					return;
				}
				if (WindowManager.IsAlreadyPaused)
				{
					WindowManager.ResetPauseRequestState();
					return;
				}
				WindowManager.m_pauseIfPossible = true;
				if (gamepadDisconnected)
				{
					WindowManager.m_pauseForGamepadDisconnected = true;
					return;
				}
			}
			else if (!WindowManager.m_pauseForGamepadDisconnected)
			{
				WindowManager.m_pauseIfPossible = false;
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00109465 File Offset: 0x00107665
		private void Update()
		{
			if (WindowManager.m_pauseIfPossible)
			{
				if (WindowManager.CanPauseNow)
				{
					WindowManager.ForcePause();
					return;
				}
				if (WindowManager.IsAlreadyPaused)
				{
					WindowManager.ResetPauseRequestState();
				}
			}
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00109487 File Offset: 0x00107687
		private static void ForcePause()
		{
			WindowManager.SetWindowIsOpen(WindowID.Pause, true);
			WindowManager.ResetPauseRequestState();
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00109495 File Offset: 0x00107695
		private static void ResetPauseRequestState()
		{
			WindowManager.m_pauseIfPossible = false;
			WindowManager.m_pauseForGamepadDisconnected = false;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x001094A3 File Offset: 0x001076A3
		public static void SetWindowIsOpen(WindowID windowID, bool isOpen, TransitionID transitionID)
		{
			if (WindowManager.m_loadWindowCoroutine == null)
			{
				WindowManager.m_loadWindowCoroutine = WindowManager.Instance.StartCoroutine(WindowManager.LoadWindowWithTransitionCoroutine(windowID, isOpen, transitionID));
				return;
			}
			Debug.LogFormat("<color=red>|{0}| Load Scene Coroutine is already running, but SHOULD NOT be</color>", new object[]
			{
				WindowManager.Instance
			});
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x001094DC File Offset: 0x001076DC
		private static IEnumerator LoadWindowWithTransitionCoroutine(WindowID windowID, bool isOpen, TransitionID transitionID)
		{
			yield return null;
			WindowManager.SetInputIsEnabled(false);
			ISceneLoadingTransition transition = TransitionLibrary.GetTransitionInstance(transitionID) as ISceneLoadingTransition;
			transition.GameObject.SetActive(true);
			yield return transition.TransitionIn();
			WindowManager.SetWindowIsOpen(windowID, isOpen);
			WindowManager.SetInputIsEnabled(false);
			yield return transition.TransitionOut();
			WindowManager.SetInputIsEnabled(true);
			transition.GameObject.SetActive(false);
			WindowManager.m_loadWindowCoroutine = null;
			yield break;
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x001094F9 File Offset: 0x001076F9
		private static void SetInputIsEnabled(bool isEnabled)
		{
			if (ReInput.isReady && PlayerManager.IsInstantiated)
			{
				RewiredMapController.SetCurrentMapEnabled(isEnabled);
			}
		}

		// Token: 0x04003DCE RID: 15822
		[SerializeField]
		private WindowController[] m_windowControllerPrefabs;

		// Token: 0x04003DD0 RID: 15824
		private static List<WindowController> m_openWindows = new List<WindowController>();

		// Token: 0x04003DD1 RID: 15825
		private static List<WindowController> m_loadedWindows = new List<WindowController>();

		// Token: 0x04003DD2 RID: 15826
		private static Coroutine m_loadWindowCoroutine = null;

		// Token: 0x04003DD3 RID: 15827
		private static EventSystem m_eventSystem = null;

		// Token: 0x04003DD4 RID: 15828
		private static bool m_pauseIfPossible = false;

		// Token: 0x04003DD5 RID: 15829
		private static bool m_pauseForGamepadDisconnected = false;

		// Token: 0x04003DD6 RID: 15830
		private static string PREFAB_PATH = "Prefabs/Managers/WindowManager";

		// Token: 0x04003DD7 RID: 15831
		private static Dictionary<SceneID, List<WindowID>> SCENE_ID_TO_LOADED_WINDOW_IDS_TABLE = new Dictionary<SceneID, List<WindowID>>
		{
			{
				SceneID.MainMenu,
				new List<WindowID>
				{
					WindowID.MainMenu,
					WindowID.Options,
					WindowID.Suboptions,
					WindowID.ProfileSelect,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig,
					WindowID.Backup
				}
			},
			{
				SceneID.Tutorial,
				new List<WindowID>
				{
					WindowID.Pause,
					WindowID.Options,
					WindowID.Suboptions,
					WindowID.Map,
					WindowID.PlayerCard,
					WindowID.GearCard,
					WindowID.Glossary,
					WindowID.Quest,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig
				}
			},
			{
				SceneID.Town,
				new List<WindowID>
				{
					WindowID.Pause,
					WindowID.Options,
					WindowID.Suboptions,
					WindowID.Map,
					WindowID.PlayerCard,
					WindowID.GearCard,
					WindowID.Glossary,
					WindowID.Quest,
					WindowID.Blacksmith,
					WindowID.Enchantress,
					WindowID.SkillTree,
					WindowID.SkillTreePopUp,
					WindowID.Dialogue,
					WindowID.PlayerDeath,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig,
					WindowID.Totem,
					WindowID.NewGamePlusNPC,
					WindowID.ChallengeNPC,
					WindowID.SoulShop
				}
			},
			{
				SceneID.World,
				new List<WindowID>
				{
					WindowID.Pause,
					WindowID.Options,
					WindowID.Suboptions,
					WindowID.Map,
					WindowID.PlayerCard,
					WindowID.GearCard,
					WindowID.Glossary,
					WindowID.Quest,
					WindowID.SpecialItemDrop,
					WindowID.PlayerDeath,
					WindowID.DeathDefy,
					WindowID.Dialogue,
					WindowID.Teleporter,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig,
					WindowID.BossIntro,
					WindowID.SkillTreePopUp
				}
			},
			{
				SceneID.SkillTree,
				new List<WindowID>
				{
					WindowID.SkillTree,
					WindowID.SkillTreePopUp,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig
				}
			},
			{
				SceneID.Lineage,
				new List<WindowID>
				{
					WindowID.Lineage,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig
				}
			},
			{
				SceneID.Disclaimer,
				new List<WindowID>
				{
					WindowID.Disclaimer
				}
			},
			{
				SceneID.Pause,
				new List<WindowID>
				{
					WindowID.Pause,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig
				}
			},
			{
				SceneID.Parade,
				new List<WindowID>
				{
					WindowID.Pause,
					WindowID.Options,
					WindowID.Suboptions,
					WindowID.Map,
					WindowID.PlayerCard,
					WindowID.GearCard,
					WindowID.Glossary,
					WindowID.Quest,
					WindowID.ConfirmMenu,
					WindowID.ConfirmMenuBig
				}
			},
			{
				SceneID.Credits,
				new List<WindowID>()
			},
			{
				SceneID.Testing,
				new List<WindowID>()
			}
		};

		// Token: 0x04003DD8 RID: 15832
		private static string[] DISABLED_PAUSE_SCENES = new string[]
		{
			"MainMenu",
			"Lineage",
			"Credits",
			"Parade",
			"Splash",
			"Disclaimer"
		};

		// Token: 0x04003DDA RID: 15834
		private static List<WindowController> m_openWindowHelper = new List<WindowController>();
	}
}
