using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
public class DeathToLineage_Timer : MonoBehaviour
{
	// Token: 0x1700160F RID: 5647
	// (get) Token: 0x0600408A RID: 16522 RVA: 0x000E4DFE File Offset: 0x000E2FFE
	// (set) Token: 0x0600408B RID: 16523 RVA: 0x000E4E05 File Offset: 0x000E3005
	private static DeathToLineage_Timer Instance { get; set; }

	// Token: 0x0600408C RID: 16524 RVA: 0x000E4E10 File Offset: 0x000E3010
	private void Awake()
	{
		if (DeathToLineage_Timer.Instance == null)
		{
			DeathToLineage_Timer.Instance = this;
			this.m_onDeathWindowClosed = new Action<MonoBehaviour, EventArgs>(this.OnDeathWindowClosed);
			this.m_onEnterLineageScreen = new Action<MonoBehaviour, EventArgs>(this.OnEnterLineageScreen);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600408D RID: 16525 RVA: 0x000E4E60 File Offset: 0x000E3060
	private void Start()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PlayerDeathWindow_Closed, this.m_onDeathWindowClosed);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x0600408E RID: 16526 RVA: 0x000E4E7B File Offset: 0x000E307B
	private void OnEnterLineageScreen(MonoBehaviour sender, EventArgs eventArgs)
	{
		DeathToLineage_Timer.StopTimer();
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x000E4E82 File Offset: 0x000E3082
	private void OnDeathWindowClosed(MonoBehaviour sender, EventArgs eventArgs)
	{
		DeathToLineage_Timer.StartTimer();
	}

	// Token: 0x06004090 RID: 16528 RVA: 0x000E4E89 File Offset: 0x000E3089
	private void OnDestroy()
	{
		if (DeathToLineage_Timer.Instance == this)
		{
			DeathToLineage_Timer.Instance = null;
			DeathToLineage_Timer.m_stopwatch = null;
		}
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.PlayerDeathWindow_Closed, this.m_onDeathWindowClosed);
		Messenger<SceneMessenger, SceneEvent>.RemoveListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x06004091 RID: 16529 RVA: 0x000E4EBD File Offset: 0x000E30BD
	public static void StartTimer()
	{
		if (DeathToLineage_Timer.m_stopwatch == null)
		{
			DeathToLineage_Timer.m_stopwatch = new Stopwatch();
		}
		DeathToLineage_Timer.m_stopwatch.Restart();
	}

	// Token: 0x06004092 RID: 16530 RVA: 0x000E4EDC File Offset: 0x000E30DC
	public static void StopTimer()
	{
		if (DeathToLineage_Timer.m_stopwatch != null && DeathToLineage_Timer.m_stopwatch.IsRunning)
		{
			DeathToLineage_Timer.m_stopwatch.Stop();
			UnityEngine.Debug.LogFormat("<color=blue>Time from <b>Exiting Death Screen</b> to <b>Being Able to Choose an Heir in Lineage Screen</b> = {0} ms</color>", new object[]
			{
				DeathToLineage_Timer.m_stopwatch.ElapsedMilliseconds
			});
		}
	}

	// Token: 0x040031CB RID: 12747
	private Action<MonoBehaviour, EventArgs> m_onDeathWindowClosed;

	// Token: 0x040031CC RID: 12748
	private Action<MonoBehaviour, EventArgs> m_onEnterLineageScreen;

	// Token: 0x040031CD RID: 12749
	private static Stopwatch m_stopwatch;
}
