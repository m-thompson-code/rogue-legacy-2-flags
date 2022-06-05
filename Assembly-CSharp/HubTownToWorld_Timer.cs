using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006F5 RID: 1781
public class HubTownToWorld_Timer : MonoBehaviour
{
	// Token: 0x17001610 RID: 5648
	// (get) Token: 0x06004094 RID: 16532 RVA: 0x000E4F30 File Offset: 0x000E3130
	// (set) Token: 0x06004095 RID: 16533 RVA: 0x000E4F37 File Offset: 0x000E3137
	private static HubTownToWorld_Timer Instance { get; set; }

	// Token: 0x06004096 RID: 16534 RVA: 0x000E4F40 File Offset: 0x000E3140
	private void Awake()
	{
		if (!HubTownToWorld_Timer.Instance)
		{
			HubTownToWorld_Timer.Instance = this;
			this.m_onExitHubTown = new Action<MonoBehaviour, EventArgs>(this.OnExitHubTown);
			this.m_onLevelManagerStateChange = new Action<MonoBehaviour, EventArgs>(this.OnLevelManagerStateChange);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x000E4F8F File Offset: 0x000E318F
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelManagerStateChange, this.m_onLevelManagerStateChange);
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x000E4FAB File Offset: 0x000E31AB
	private void OnLevelManagerStateChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (eventArgs is LevelManagerStateChangeEventArgs && (eventArgs as LevelManagerStateChangeEventArgs).State == LevelManagerState.BuildComplete)
		{
			HubTownToWorld_Timer.StopTimer();
		}
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x000E4FC8 File Offset: 0x000E31C8
	private void OnExitHubTown(MonoBehaviour sender, EventArgs eventArgs)
	{
		HubTownToWorld_Timer.StartTimer();
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x000E4FCF File Offset: 0x000E31CF
	private void OnDestroy()
	{
		if (HubTownToWorld_Timer.Instance == this)
		{
			HubTownToWorld_Timer.Instance = null;
			HubTownToWorld_Timer.m_stopwatch = null;
		}
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelManagerStateChange, this.m_onLevelManagerStateChange);
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x000E5004 File Offset: 0x000E3204
	public static void StartTimer()
	{
		if (HubTownToWorld_Timer.m_stopwatch == null)
		{
			HubTownToWorld_Timer.m_stopwatch = new Stopwatch();
		}
		HubTownToWorld_Timer.m_stopwatch.Restart();
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x000E5024 File Offset: 0x000E3224
	public static void StopTimer()
	{
		if (SceneManager.GetActiveScene().name.StartsWith("World") && HubTownToWorld_Timer.m_stopwatch != null && HubTownToWorld_Timer.m_stopwatch.IsRunning)
		{
			HubTownToWorld_Timer.m_stopwatch.Stop();
			UnityEngine.Debug.LogFormat("<color=blue>Time from <b>Charon</b> in Hub Town to <b>Gate Transition</b> in World = {0} ms</color>", new object[]
			{
				HubTownToWorld_Timer.m_stopwatch.ElapsedMilliseconds
			});
		}
	}

	// Token: 0x040031CF RID: 12751
	private Action<MonoBehaviour, EventArgs> m_onExitHubTown;

	// Token: 0x040031D0 RID: 12752
	private Action<MonoBehaviour, EventArgs> m_onLevelManagerStateChange;

	// Token: 0x040031D1 RID: 12753
	private static Stopwatch m_stopwatch;
}
