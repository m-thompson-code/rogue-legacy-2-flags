using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BA7 RID: 2983
public class DeathToLineage_Timer : MonoBehaviour
{
	// Token: 0x17001E0B RID: 7691
	// (get) Token: 0x060059D3 RID: 22995 RVA: 0x00030FF7 File Offset: 0x0002F1F7
	// (set) Token: 0x060059D4 RID: 22996 RVA: 0x00030FFE File Offset: 0x0002F1FE
	private static DeathToLineage_Timer Instance { get; set; }

	// Token: 0x060059D5 RID: 22997 RVA: 0x00154770 File Offset: 0x00152970
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

	// Token: 0x060059D6 RID: 22998 RVA: 0x00031006 File Offset: 0x0002F206
	private void Start()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.PlayerDeathWindow_Closed, this.m_onDeathWindowClosed);
		Messenger<SceneMessenger, SceneEvent>.AddListener(SceneEvent.EnterLineageScreen, this.m_onEnterLineageScreen);
	}

	// Token: 0x060059D7 RID: 22999 RVA: 0x00031021 File Offset: 0x0002F221
	private void OnEnterLineageScreen(MonoBehaviour sender, EventArgs eventArgs)
	{
		DeathToLineage_Timer.StopTimer();
	}

	// Token: 0x060059D8 RID: 23000 RVA: 0x00031028 File Offset: 0x0002F228
	private void OnDeathWindowClosed(MonoBehaviour sender, EventArgs eventArgs)
	{
		DeathToLineage_Timer.StartTimer();
	}

	// Token: 0x060059D9 RID: 23001 RVA: 0x0003102F File Offset: 0x0002F22F
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

	// Token: 0x060059DA RID: 23002 RVA: 0x00031063 File Offset: 0x0002F263
	public static void StartTimer()
	{
		if (DeathToLineage_Timer.m_stopwatch == null)
		{
			DeathToLineage_Timer.m_stopwatch = new Stopwatch();
		}
		DeathToLineage_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x001547C0 File Offset: 0x001529C0
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

	// Token: 0x04004446 RID: 17478
	private Action<MonoBehaviour, EventArgs> m_onDeathWindowClosed;

	// Token: 0x04004447 RID: 17479
	private Action<MonoBehaviour, EventArgs> m_onEnterLineageScreen;

	// Token: 0x04004448 RID: 17480
	private static Stopwatch m_stopwatch;
}
