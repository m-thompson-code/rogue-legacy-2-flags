using System;
using System.Collections;
using RL_Windows;
using Steamworks;
using UnityEngine;

// Token: 0x02000699 RID: 1689
public class GameManager : MonoBehaviour
{
	// Token: 0x17001546 RID: 5446
	// (get) Token: 0x06003D9B RID: 15771 RVA: 0x000D6F42 File Offset: 0x000D5142
	// (set) Token: 0x06003D9C RID: 15772 RVA: 0x000D6F49 File Offset: 0x000D5149
	public static bool IsApplicationClosing { get; private set; }

	// Token: 0x17001547 RID: 5447
	// (get) Token: 0x06003D9D RID: 15773 RVA: 0x000D6F51 File Offset: 0x000D5151
	// (set) Token: 0x06003D9E RID: 15774 RVA: 0x000D6F58 File Offset: 0x000D5158
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

	// Token: 0x17001548 RID: 5448
	// (get) Token: 0x06003D9F RID: 15775 RVA: 0x000D6F60 File Offset: 0x000D5160
	public static bool IsGameManagerInstantiated
	{
		get
		{
			return GameManager.m_instance;
		}
	}

	// Token: 0x17001549 RID: 5449
	// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x000D6F6C File Offset: 0x000D516C
	// (set) Token: 0x06003DA1 RID: 15777 RVA: 0x000D6F78 File Offset: 0x000D5178
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

	// Token: 0x06003DA2 RID: 15778 RVA: 0x000D6F85 File Offset: 0x000D5185
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

	// Token: 0x06003DA3 RID: 15779 RVA: 0x000D6FBE File Offset: 0x000D51BE
	private IEnumerator Start()
	{
		while (!SteamManager.Initialized)
		{
			yield return null;
		}
		this.m_steamOverlayActivated = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnSteamOverlayActivated));
		yield break;
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x000D6FCD File Offset: 0x000D51CD
	private void OnSteamOverlayActivated(GameOverlayActivated_t overlayCallback)
	{
		if (overlayCallback.m_bActive != 0)
		{
			WindowManager.PauseWhenPossible(true, false);
			return;
		}
		WindowManager.PauseWhenPossible(false, false);
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x000D6FE6 File Offset: 0x000D51E6
	private void OnApplicationFocus(bool focus)
	{
		WindowManager.PauseWhenPossible(!focus, false);
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x000D6FF2 File Offset: 0x000D51F2
	private void OnApplicationQuit()
	{
		Debug.Log("Application is closing...");
		GameManager.IsApplicationClosing = true;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x000D7004 File Offset: 0x000D5204
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

	// Token: 0x04002DF9 RID: 11769
	private static GameManager m_instance;

	// Token: 0x04002DFA RID: 11770
	private static GamePauseStateChangeEventArgs m_pauseEventArgs;

	// Token: 0x04002DFB RID: 11771
	private bool m_isGamePaused;

	// Token: 0x04002DFC RID: 11772
	protected Callback<GameOverlayActivated_t> m_steamOverlayActivated;
}
