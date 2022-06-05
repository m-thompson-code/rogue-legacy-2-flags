using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006F7 RID: 1783
public class MainMenuToLineage_Timer : MonoBehaviour
{
	// Token: 0x17001612 RID: 5650
	// (get) Token: 0x060040A8 RID: 16552 RVA: 0x000E51C0 File Offset: 0x000E33C0
	// (set) Token: 0x060040A9 RID: 16553 RVA: 0x000E51C7 File Offset: 0x000E33C7
	private static MainMenuToLineage_Timer Instance { get; set; }

	// Token: 0x060040AA RID: 16554 RVA: 0x000E51D0 File Offset: 0x000E33D0
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

	// Token: 0x060040AB RID: 16555 RVA: 0x000E5220 File Offset: 0x000E3420
	private void Start()
	{
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.ExitMainMenu, this.m_onExitMainMenu);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x000E523A File Offset: 0x000E343A
	private void OnEnterLineageScreen(MonoBehaviour sender, EventArgs eventArgs)
	{
		MainMenuToLineage_Timer.StopTimer();
	}

	// Token: 0x060040AD RID: 16557 RVA: 0x000E5241 File Offset: 0x000E3441
	private void OnExitMainMenu(MonoBehaviour sender, EventArgs eventArgs)
	{
		MainMenuToLineage_Timer.StartTimer();
	}

	// Token: 0x060040AE RID: 16558 RVA: 0x000E5248 File Offset: 0x000E3448
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

	// Token: 0x060040AF RID: 16559 RVA: 0x000E527B File Offset: 0x000E347B
	public static void StartTimer()
	{
		if (MainMenuToLineage_Timer.m_stopwatch == null)
		{
			MainMenuToLineage_Timer.m_stopwatch = new Stopwatch();
		}
		MainMenuToLineage_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x000E5298 File Offset: 0x000E3498
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

	// Token: 0x040031D7 RID: 12759
	private Action<MonoBehaviour, EventArgs> m_onExitMainMenu;

	// Token: 0x040031D8 RID: 12760
	private Action<MonoBehaviour, EventArgs> m_onEnterLineageScreen;

	// Token: 0x040031D9 RID: 12761
	private static Stopwatch m_stopwatch;
}
