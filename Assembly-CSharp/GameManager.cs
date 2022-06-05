using System;
using System.Collections;
using RL_Windows;
using Steamworks;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
public class GameManager : MonoBehaviour
{
	// Token: 0x17001D20 RID: 7456
	// (get) Token: 0x0600565C RID: 22108 RVA: 0x0002EF0C File Offset: 0x0002D10C
	// (set) Token: 0x0600565D RID: 22109 RVA: 0x0002EF13 File Offset: 0x0002D113
	public static bool IsApplicationClosing { get; private set; }

	// Token: 0x17001D21 RID: 7457
	// (get) Token: 0x0600565E RID: 22110 RVA: 0x0002EF1B File Offset: 0x0002D11B
	// (set) Token: 0x0600565F RID: 22111 RVA: 0x0002EF22 File Offset: 0x0002D122
	private static GameManager Instance
	{
		get
		{
			return GameManager.m_instance;
		}
		set
		{
			GameManager.m_instance = value;
		}
	}

	// Token: 0x17001D22 RID: 7458
	// (get) Token: 0x06005660 RID: 22112 RVA: 0x0002EF2A File Offset: 0x0002D12A
	public static bool IsGameManagerInstantiated
	{
		get
		{
			return GameManager.m_instance;
		}
	}

	// Token: 0x17001D23 RID: 7459
	// (get) Token: 0x06005661 RID: 22113 RVA: 0x0002EF36 File Offset: 0x0002D136
	// (set) Token: 0x06005662 RID: 22114 RVA: 0x0002EF42 File Offset: 0x0002D142
	public static bool IsGamePaused
	{
		get
		{
			return GameManager.Instance.m_isGamePaused;
		}
		private set
		{
			GameManager.Instance.m_isGamePaused = value;
		}
	}

	// Token: 0x06005663 RID: 22115 RVA: 0x0002EF4F File Offset: 0x0002D14F
	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			GameManager.Instance = this;
			GameManager.m_pauseEventArgs = new GamePauseStateChangeEventArgs(false);
			return;
		}
		if (GameManager.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06005664 RID: 22116 RVA: 0x0002EF88 File Offset: 0x0002D188
	private IEnumerator Start()
	{
		while (!SteamManager.Initialized)
		{
			yield return null;
		}
		this.m_steamOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnSteamOverlayActivated));
		yield break;
	}

	// Token: 0x06005665 RID: 22117 RVA: 0x0002EF97 File Offset: 0x0002D197
	private void OnSteamOverlayActivated(GameOverlayActivated_t overlayCallback)
	{
		if (overlayCallback.m_bActive != 0)
		{
			WindowManager.PauseWhenPossible(true, false);
			return;
		}
		WindowManager.PauseWhenPossible(false, false);
	}

	// Token: 0x06005666 RID: 22118 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
	private void OnApplicationFocus(bool focus)
	{
		WindowManager.PauseWhenPossible(!focus, false);
	}

	// Token: 0x06005667 RID: 22119 RVA: 0x0002EFBC File Offset: 0x0002D1BC
	private void OnApplicationQuit()
	{
		Debug.Log("Application is closing...");
		GameManager.IsApplicationClosing = true;
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x0014766C File Offset: 0x0014586C
	public static void SetIsPaused(bool isPaused)
	{
		if (GameManager.IsGamePaused != isPaused)
		{
			GameManager.IsGamePaused = isPaused;
			RumbleManager.SetRumblePaused(isPaused);
			if (GameManager.IsGamePaused)
			{
				RLTimeScale.SetTimeScale(TimeScaleType.Game, 0f);
				OnGameLoadManager.ConfineMouseToGameWindow(false);
			}
			else
			{
				RLTimeScale.SetTimeScale(TimeScaleType.Game, 1f);
				OnGameLoadManager.ConfineMouseToGameWindow(!SaveManager.ConfigData.DisableCursorConfine);
			}
			GameManager.m_pauseEventArgs.Initialize(GameManager.IsGamePaused);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GamePauseStateChange, GameManager.Instance, GameManager.m_pauseEventArgs);
		}
	}

	// Token: 0x04003FEA RID: 16362
	private static GameManager m_instance;

	// Token: 0x04003FEB RID: 16363
	private static GamePauseStateChangeEventArgs m_pauseEventArgs;

	// Token: 0x04003FEC RID: 16364
	private bool m_isGamePaused;

	// Token: 0x04003FED RID: 16365
	protected Callback<GameOverlayActivated_t> m_steamOverlayActivated;
}
