using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000BA8 RID: 2984
public class HubTownToWorld_Timer : MonoBehaviour
{
	// Token: 0x17001E0C RID: 7692
	// (get) Token: 0x060059DD RID: 23005 RVA: 0x00031080 File Offset: 0x0002F280
	// (set) Token: 0x060059DE RID: 23006 RVA: 0x00031087 File Offset: 0x0002F287
	private static HubTownToWorld_Timer Instance { get; set; }

	// Token: 0x060059DF RID: 23007 RVA: 0x0015480C File Offset: 0x00152A0C
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

	// Token: 0x060059E0 RID: 23008 RVA: 0x0003108F File Offset: 0x0002F28F
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelManagerStateChange, this.m_onLevelManagerStateChange);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x000310AB File Offset: 0x0002F2AB
	private void OnLevelManagerStateChange(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (eventArgs is LevelManagerStateChangeEventArgs && (eventArgs as LevelManagerStateChangeEventArgs).State == LevelManagerState.BuildComplete)
		{
			HubTownToWorld_Timer.StopTimer();
		}
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x000310C8 File Offset: 0x0002F2C8
	private void OnExitHubTown(MonoBehaviour sender, EventArgs eventArgs)
	{
		HubTownToWorld_Timer.StartTimer();
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x000310CF File Offset: 0x0002F2CF
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

	// Token: 0x060059E4 RID: 23012 RVA: 0x00031104 File Offset: 0x0002F304
	public static void StartTimer()
	{
		if (HubTownToWorld_Timer.m_stopwatch == null)
		{
			HubTownToWorld_Timer.m_stopwatch = new Stopwatch();
		}
		HubTownToWorld_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x0015485C File Offset: 0x00152A5C
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

	// Token: 0x0400444A RID: 17482
	private Action<MonoBehaviour, EventArgs> m_onExitHubTown;

	// Token: 0x0400444B RID: 17483
	private Action<MonoBehaviour, EventArgs> m_onLevelManagerStateChange;

	// Token: 0x0400444C RID: 17484
	private static Stopwatch m_stopwatch;
}
