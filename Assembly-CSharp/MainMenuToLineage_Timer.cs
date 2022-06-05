using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BAA RID: 2986
public class MainMenuToLineage_Timer : MonoBehaviour
{
	// Token: 0x17001E0E RID: 7694
	// (get) Token: 0x060059F1 RID: 23025 RVA: 0x000311AC File Offset: 0x0002F3AC
	// (set) Token: 0x060059F2 RID: 23026 RVA: 0x000311B3 File Offset: 0x0002F3B3
	private static MainMenuToLineage_Timer Instance { get; set; }

	// Token: 0x060059F3 RID: 23027 RVA: 0x00154960 File Offset: 0x00152B60
	private void Awake()
	{
		if (MainMenuToLineage_Timer.Instance == null)
		{
			MainMenuToLineage_Timer.Instance = this;
			this.m_onExitMainMenu = new Action<MonoBehaviour, EventArgs>(this.OnExitMainMenu);
			this.m_onEnterLineageScreen = new Action<MonoBehaviour, EventArgs>(this.OnEnterLineageScreen);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060059F4 RID: 23028 RVA: 0x000311BB File Offset: 0x0002F3BB
	private void Start()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.ExitMainMenu, this.m_onExitMainMenu);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x060059F5 RID: 23029 RVA: 0x000311D5 File Offset: 0x0002F3D5
	private void OnEnterLineageScreen(MonoBehaviour sender, EventArgs eventArgs)
	{
		MainMenuToLineage_Timer.StopTimer();
	}

	// Token: 0x060059F6 RID: 23030 RVA: 0x000311DC File Offset: 0x0002F3DC
	private void OnExitMainMenu(MonoBehaviour sender, EventArgs eventArgs)
	{
		MainMenuToLineage_Timer.StartTimer();
	}

	// Token: 0x060059F7 RID: 23031 RVA: 0x000311E3 File Offset: 0x0002F3E3
	private void OnDestroy()
	{
		if (MainMenuToLineage_Timer.Instance == this)
		{
			MainMenuToLineage_Timer.Instance = null;
			MainMenuToLineage_Timer.m_stopwatch = null;
		}
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.ExitMainMenu, this.m_onExitMainMenu);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x060059F8 RID: 23032 RVA: 0x00031216 File Offset: 0x0002F416
	public static void StartTimer()
	{
		if (MainMenuToLineage_Timer.m_stopwatch == null)
		{
			MainMenuToLineage_Timer.m_stopwatch = new Stopwatch();
		}
		MainMenuToLineage_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060059F9 RID: 23033 RVA: 0x001549B0 File Offset: 0x00152BB0
	public static void StopTimer()
	{
		if (MainMenuToLineage_Timer.m_stopwatch != null && MainMenuToLineage_Timer.m_stopwatch.IsRunning)
		{
			MainMenuToLineage_Timer.m_stopwatch.Stop();
			UnityEngine.Debug.LogFormat("<color=blue>Time from <b>Choosing Start New Legacy in Main Menu</b> to <b>Being Able to Choose an Heir in Lineage Screen</b> = {0} ms</color>", new object[]
			{
				MainMenuToLineage_Timer.m_stopwatch.ElapsedMilliseconds
			});
		}
	}

	// Token: 0x04004452 RID: 17490
	private Action<MonoBehaviour, EventArgs> m_onExitMainMenu;

	// Token: 0x04004453 RID: 17491
	private Action<MonoBehaviour, EventArgs> m_onEnterLineageScreen;

	// Token: 0x04004454 RID: 17492
	private static Stopwatch m_stopwatch;
}
