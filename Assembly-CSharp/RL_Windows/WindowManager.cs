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
	// Token: 0x02000E03 RID: 3587
	public class WindowManager : MonoBehaviour
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06006520 RID: 25888 RVA: 0x00177E28 File Offset: 0x00176028
		// (remove) Token: 0x06006521 RID: 25889 RVA: 0x00177E5C File Offset: 0x0017605C
		public static event EventHandler<WindowStateChangeEventArgs> WindowStateChangeEvent;

		// Token: 0x17002083 RID: 8323
		// (get) Token: 0x06006522 RID: 25890 RVA: 0x00037C00 File Offset: 0x00035E00
		// (set) Token: 0x06006523 RID: 25891 RVA: 0x00037C07 File Offset: 0x00035E07
		private static WindowManager Instance { get; set; }

		// Token: 0x17002084 RID: 8324
		// (get) Token: 0x06006524 RID: 25892 RVA: 0x00177E90 File Offset: 0x00176090
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

		// Token: 0x17002085 RID: 8325
		// (get) Token: 0x06006525 RID: 25893 RVA: 0x00037C0F File Offset: 0x00035E0F
		public static Dictionary<SceneID, List<WindowID>> SceneIDToLoadedWindowIDsTable
		{
			get
			{
				return WindowManager.SCENE_ID_TO_LOADED_WINDOW_IDS_TABLE;
			}
		}

		// Token: 0x17002086 RID: 8326
		// (get) Token: 0x06006526 RID: 25894 RVA: 0x00037C16 File Offset: 0x00035E16
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

		// Token: 0x17002087 RID: 8327
		// (get) Token: 0x06006527 RID: 25895 RVA: 0x00037C38 File Offset: 0x00035E38
		// (set) Token: 0x06006528 RID: 25896 RVA: 0x00037C3F File Offset: 0x00035E3F
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

		// Token: 0x17002088 RID: 8328
		// (get) Token: 0x06006529 RID: 25897 RVA: 0x00037C47 File Offset: 0x00035E47
		// (set) Token: 0x0600652A RID: 25898 RVA: 0x00037C4E File Offset: 0x00035E4E
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

		// Token: 0x17002089 RID: 8329
		// (get) Token: 0x0600652B RID: 25899 RVA: 0x00037C56 File Offset: 0x00035E56
		public static bool IsInstantiated
		{
			get
			{
				return WindowManager.Instance != null;
			}
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x00177EE0 File Offset: 0x001760E0
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

		// Token: 0x0600652D RID: 25901 RVA: 0x00177F38 File Offset: 0x00176138
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

		// Token: 0x0600652E RID: 25902 RVA: 0x00177FB4 File Offset: 0x001761B4
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

		// Token: 0x0600652F RID: 25903 RVA: 0x00037C63 File Offset: 0x00035E63
		private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			WindowManager.LoadSceneWindows(scene);
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x0017803C File Offset: 0x0017623C
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

		// Token: 0x06006531 RID: 25905 RVA: 0x001780E4 File Offset: 0x001762E4
		private void OnStartButtonDown(InputActionEventData obj)
		{
			if (WindowManager.DISABLED_PAUSE_SCENES.Contains(SceneManager.GetActiveScene().name))
			{
				return;
			}
			WindowManager.SetWindowIsOpen(WindowID.Pause, true);
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x00178114 File Offset: 0x00176314
		public static void CloseAllOpenWindows()
		{
			WindowManager.m_openWindowHelper.Clear();
			WindowManager.m_openWindowHelper.AddRange(WindowManager.OpenWindows);
			foreach (WindowController windowController in WindowManager.m_openWindowHelper)
			{
				WindowManager.SetWindowIsOpen(windowController.ID, false);
			}
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x00178184 File Offset: 0x00176384
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

		// Token: 0x06006534 RID: 25908 RVA: 0x0017827C File Offset: 0x0017647C
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

		// Token: 0x06006535 RID: 25909 RVA: 0x00178350 File Offset: 0x00176550
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

		// Token: 0x06006536 RID: 25910 RVA: 0x001783AC File Offset: 0x001765AC
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

		// Token: 0x06006537 RID: 25911 RVA: 0x00178408 File Offset: 0x00176608
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

		// Token: 0x1700208A RID: 8330
		// (get) Token: 0x06006538 RID: 25912 RVA: 0x00037C6B File Offset: 0x00035E6B
		public static bool IsAnyWindowOpen
		{
			get
			{
				return WindowManager.OpenWindows.Count > 0;
			}
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x00178464 File Offset: 0x00176664
		public static WindowController GetWindowController(WindowID windowID)
		{
			if (WindowManager.GetIsWindowLoaded(windowID))
			{
				return WindowManager.LoadedWindows.Single((WindowController x) => x.ID == windowID);
			}
			return null;
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x001784A4 File Offset: 0x001766A4
		private static List<WindowID> GetWindowsToLoad(SceneID sceneID)
		{
			List<WindowID> result = null;
			if (WindowManager.SceneIDToLoadedWindowIDsTable.TryGetValue(sceneID, out result))
			{
				return result;
			}
			return new List<WindowID>();
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x001784CC File Offset: 0x001766CC
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

		// Token: 0x0600653C RID: 25916 RVA: 0x00178538 File Offset: 0x00176738
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

		// Token: 0x0600653D RID: 25917 RVA: 0x001785D8 File Offset: 0x001767D8
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

		// Token: 0x0600653E RID: 25918 RVA: 0x00037C7A File Offset: 0x00035E7A
		public static void LoadWindow(WindowID windowID)
		{
			WindowManager.CreateWindowInstanceAndAddToLoadedWindows(windowID);
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x00178700 File Offset: 0x00176900
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

		// Token: 0x06006540 RID: 25920 RVA: 0x00037C82 File Offset: 0x00035E82
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

		// Token: 0x06006541 RID: 25921 RVA: 0x00178800 File Offset: 0x00176A00
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

		// Token: 0x1700208B RID: 8331
		// (get) Token: 0x06006542 RID: 25922 RVA: 0x00037CC2 File Offset: 0x00035EC2
		private static bool CanPauseNow
		{
			get
			{
				return RewiredMapController.IsMapEnabled(GameInputMode.Game) && WorldBuilder.Instance != null && WorldBuilder.State == BiomeBuildStateID.Complete;
			}
		}

		// Token: 0x1700208C RID: 8332
		// (get) Token: 0x06006543 RID: 25923 RVA: 0x00037CE4 File Offset: 0x00035EE4
		private static bool IsAlreadyPaused
		{
			get
			{
				return RewiredMapController.IsMapEnabled(GameInputMode.Window) && !WindowManager.GetIsWindowOpen(WindowID.BossIntro);
			}
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x00037CFB File Offset: 0x00035EFB
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

		// Token: 0x06006545 RID: 25925 RVA: 0x00037D37 File Offset: 0x00035F37
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

		// Token: 0x06006546 RID: 25926 RVA: 0x00037D59 File Offset: 0x00035F59
		private static void ForcePause()
		{
			WindowManager.SetWindowIsOpen(WindowID.Pause, true);
			WindowManager.ResetPauseRequestState();
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x00037D67 File Offset: 0x00035F67
		private static void ResetPauseRequestState()
		{
			WindowManager.m_pauseIfPossible = false;
			WindowManager.m_pauseForGamepadDisconnected = false;
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x00037D75 File Offset: 0x00035F75
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

		// Token: 0x06006549 RID: 25929 RVA: 0x00037DAE File Offset: 0x00035FAE
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

		// Token: 0x0600654A RID: 25930 RVA: 0x00037DCB File Offset: 0x00035FCB
		private static void SetInputIsEnabled(bool isEnabled)
		{
			if (ReInput.isReady && PlayerManager.IsInstantiated)
			{
				RewiredMapController.SetCurrentMapEnabled(isEnabled);
			}
		}

		// Token: 0x04005251 RID: 21073
		[SerializeField]
		private WindowController[] m_windowControllerPrefabs;

		// Token: 0x04005253 RID: 21075
		private static List<WindowController> m_openWindows = new List<WindowController>();

		// Token: 0x04005254 RID: 21076
		private static List<WindowController> m_loadedWindows = new List<WindowController>();

		// Token: 0x04005255 RID: 21077
		private static Coroutine m_loadWindowCoroutine = null;

		// Token: 0x04005256 RID: 21078
		private static EventSystem m_eventSystem = null;

		// Token: 0x04005257 RID: 21079
		private static bool m_pauseIfPossible = false;

		// Token: 0x04005258 RID: 21080
		private static bool m_pauseForGamepadDisconnected = false;

		// Token: 0x04005259 RID: 21081
		private static string PREFAB_PATH = "Prefabs/Managers/WindowManager";

		// Token: 0x0400525A RID: 21082
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

		// Token: 0x0400525B RID: 21083
		private static string[] DISABLED_PAUSE_SCENES = new string[]
		{
			"MainMenu",
			"Lineage",
			"Credits",
			"Parade",
			"Splash",
			"Disclaimer"
		};

		// Token: 0x0400525D RID: 21085
		private static List<WindowController> m_openWindowHelper = new List<WindowController>();
	}
}
