using System;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000200 RID: 512
public class GameTimer : MonoBehaviour
{
	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x0004352F File Offset: 0x0004172F
	// (set) Token: 0x060015A2 RID: 5538 RVA: 0x00043536 File Offset: 0x00041736
	private static GameTimer Instance { get; set; }

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x060015A3 RID: 5539 RVA: 0x0004353E File Offset: 0x0004173E
	public static float TotalSessionAccumulatedTime
	{
		get
		{
			return GameTimer.SessionAccumulatedTime - GameTimer.SessionAccumulatedPauseTime;
		}
	}

	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x060015A4 RID: 5540 RVA: 0x0004354B File Offset: 0x0004174B
	public static float SessionAccumulatedPauseTime
	{
		get
		{
			return GameTimer.Instance.m_sessionAccumulatedPauseTime;
		}
	}

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x060015A5 RID: 5541 RVA: 0x00043557 File Offset: 0x00041757
	public static float SessionAccumulatedTime
	{
		get
		{
			return Time.unscaledTime - GameTimer.Instance.m_sessionStartTime;
		}
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x0004356C File Offset: 0x0004176C
	private void Awake()
	{
		if (!GameTimer.Instance)
		{
			GameTimer.Instance = this;
			this.m_onGamePauseStateChanged = new Action<MonoBehaviour, EventArgs>(this.OnGamePauseStateChanged);
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			this.m_onTransitionStarted = new Action(this.OnTransitionStarted);
			this.m_onTransitionEnded = new Action(this.OnTransitionEnded);
			SceneLoader_RL.TransitionStartRelay.AddListener(this.m_onTransitionStarted, false);
			SceneLoader_RL.TransitionCompleteRelay.AddListener(this.m_onTransitionEnded, false);
			return;
		}
		throw new Exception("GameTimer instantiated twice. Should not happen.");
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x00043604 File Offset: 0x00041804
	private void OnDestroy()
	{
		GameTimer.Instance = null;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
		if (!GameManager.IsApplicationClosing)
		{
			SceneLoader_RL.TransitionStartRelay.RemoveListener(this.m_onTransitionStarted);
			SceneLoader_RL.TransitionCompleteRelay.RemoveListener(this.m_onTransitionEnded);
		}
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x00043654 File Offset: 0x00041854
	private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
	{
		bool flag = false;
		foreach (SceneID sceneID in this.SCENES_TO_TRACK_SESSION_TIME)
		{
			if (scene.name == SceneLoadingUtility.GetSceneName(sceneID))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (!this.m_sessionStarted)
			{
				this.m_sessionStarted = true;
				this.m_sessionPaused = false;
				Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.GamePauseStateChange, this.m_onGamePauseStateChanged);
				Debug.Log("Starting GameTimer playtime session.");
				GameTimer.ClearSessionAccumulatedTime();
				return;
			}
		}
		else if (this.m_sessionStarted)
		{
			this.m_sessionStarted = false;
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.GamePauseStateChange, this.m_onGamePauseStateChanged);
			if (!(SceneLoader_RL.CurrentScene == SceneLoadingUtility.GetSceneName(SceneID.MainMenu)))
			{
				Debug.Log("Ending GameTimer playtime session.");
				SaveManager.PlayerSaveData.SecondsPlayed += (uint)GameTimer.TotalSessionAccumulatedTime;
				Debug.Log("Saving session playtime: " + GameTimer.TotalSessionAccumulatedTime.ToString() + " to profile " + SaveManager.CurrentProfile.ToString());
				SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
			}
		}
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x00043754 File Offset: 0x00041954
	public static void ClearSessionAccumulatedTime()
	{
		GameTimer.Instance.m_sessionAccumulatedPauseTime = 0f;
		GameTimer.Instance.m_sessionStartTime = Time.unscaledTime;
		GameTimer.Instance.m_sessionPauseStartTime = Time.unscaledTime;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x00043783 File Offset: 0x00041983
	private void OnTransitionStarted()
	{
		if (!GameManager.IsGamePaused)
		{
			this.m_transitionPauseChangeEventArgs.Initialize(true);
			this.OnGamePauseStateChanged(null, this.m_transitionPauseChangeEventArgs);
		}
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x000437A5 File Offset: 0x000419A5
	private void OnTransitionEnded()
	{
		if (!GameManager.IsGamePaused)
		{
			this.m_transitionPauseChangeEventArgs.Initialize(false);
			this.OnGamePauseStateChanged(null, this.m_transitionPauseChangeEventArgs);
		}
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x000437C8 File Offset: 0x000419C8
	private void OnGamePauseStateChanged(object sender, EventArgs args)
	{
		if (this.m_sessionStarted)
		{
			if ((args as GamePauseStateChangeEventArgs).IsPaused)
			{
				if (!this.m_sessionPaused)
				{
					this.m_sessionPaused = true;
					this.m_sessionPauseStartTime = Time.unscaledTime;
					return;
				}
			}
			else if (this.m_sessionPaused)
			{
				this.m_sessionPaused = false;
				this.m_sessionAccumulatedPauseTime += Time.unscaledTime - this.m_sessionPauseStartTime;
			}
		}
	}

	// Token: 0x040014D4 RID: 5332
	private SceneID[] SCENES_TO_TRACK_SESSION_TIME = new SceneID[]
	{
		SceneID.World,
		SceneID.Town
	};

	// Token: 0x040014D6 RID: 5334
	private bool m_sessionStarted;

	// Token: 0x040014D7 RID: 5335
	private bool m_sessionPaused;

	// Token: 0x040014D8 RID: 5336
	private float m_sessionStartTime;

	// Token: 0x040014D9 RID: 5337
	private float m_sessionPauseStartTime;

	// Token: 0x040014DA RID: 5338
	private float m_sessionAccumulatedPauseTime;

	// Token: 0x040014DB RID: 5339
	private Action<MonoBehaviour, EventArgs> m_onGamePauseStateChanged;

	// Token: 0x040014DC RID: 5340
	private Action m_onTransitionStarted;

	// Token: 0x040014DD RID: 5341
	private Action m_onTransitionEnded;

	// Token: 0x040014DE RID: 5342
	private GamePauseStateChangeEventArgs m_transitionPauseChangeEventArgs = new GamePauseStateChangeEventArgs(false);
}
