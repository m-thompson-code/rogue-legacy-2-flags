using System;
using SceneManagement_RL;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003AC RID: 940
public class GameTimer : MonoBehaviour
{
	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x06001F15 RID: 7957 RVA: 0x000104BF File Offset: 0x0000E6BF
	// (set) Token: 0x06001F16 RID: 7958 RVA: 0x000104C6 File Offset: 0x0000E6C6
	private static GameTimer Instance { get; set; }

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000104CE File Offset: 0x0000E6CE
	public static float TotalSessionAccumulatedTime
	{
		get
		{
			return GameTimer.SessionAccumulatedTime - GameTimer.SessionAccumulatedPauseTime;
		}
	}

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x06001F18 RID: 7960 RVA: 0x000104DB File Offset: 0x0000E6DB
	public static float SessionAccumulatedPauseTime
	{
		get
		{
			return GameTimer.Instance.m_sessionAccumulatedPauseTime;
		}
	}

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06001F19 RID: 7961 RVA: 0x000104E7 File Offset: 0x0000E6E7
	public static float SessionAccumulatedTime
	{
		get
		{
			return Time.unscaledTime - GameTimer.Instance.m_sessionStartTime;
		}
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x000A1C08 File Offset: 0x0009FE08
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

	// Token: 0x06001F1B RID: 7963 RVA: 0x000A1CA0 File Offset: 0x0009FEA0
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

	// Token: 0x06001F1C RID: 7964 RVA: 0x000A1CF0 File Offset: 0x0009FEF0
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

	// Token: 0x06001F1D RID: 7965 RVA: 0x000104F9 File Offset: 0x0000E6F9
	public static void ClearSessionAccumulatedTime()
	{
		GameTimer.Instance.m_sessionAccumulatedPauseTime = 0f;
		GameTimer.Instance.m_sessionStartTime = Time.unscaledTime;
		GameTimer.Instance.m_sessionPauseStartTime = Time.unscaledTime;
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x00010528 File Offset: 0x0000E728
	private void OnTransitionStarted()
	{
		if (!GameManager.IsGamePaused)
		{
			this.m_transitionPauseChangeEventArgs.Initialize(true);
			this.OnGamePauseStateChanged(null, this.m_transitionPauseChangeEventArgs);
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x0001054A File Offset: 0x0000E74A
	private void OnTransitionEnded()
	{
		if (!GameManager.IsGamePaused)
		{
			this.m_transitionPauseChangeEventArgs.Initialize(false);
			this.OnGamePauseStateChanged(null, this.m_transitionPauseChangeEventArgs);
		}
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x000A1DF0 File Offset: 0x0009FFF0
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

	// Token: 0x04001BBB RID: 7099
	private SceneID[] SCENES_TO_TRACK_SESSION_TIME = new SceneID[]
	{
		SceneID.World,
		SceneID.Town
	};

	// Token: 0x04001BBD RID: 7101
	private bool m_sessionStarted;

	// Token: 0x04001BBE RID: 7102
	private bool m_sessionPaused;

	// Token: 0x04001BBF RID: 7103
	private float m_sessionStartTime;

	// Token: 0x04001BC0 RID: 7104
	private float m_sessionPauseStartTime;

	// Token: 0x04001BC1 RID: 7105
	private float m_sessionAccumulatedPauseTime;

	// Token: 0x04001BC2 RID: 7106
	private Action<MonoBehaviour, EventArgs> m_onGamePauseStateChanged;

	// Token: 0x04001BC3 RID: 7107
	private Action m_onTransitionStarted;

	// Token: 0x04001BC4 RID: 7108
	private Action m_onTransitionEnded;

	// Token: 0x04001BC5 RID: 7109
	private GamePauseStateChangeEventArgs m_transitionPauseChangeEventArgs = new GamePauseStateChangeEventArgs(false);
}
