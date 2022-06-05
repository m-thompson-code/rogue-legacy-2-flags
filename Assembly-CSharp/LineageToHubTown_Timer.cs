using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
public class LineageToHubTown_Timer : MonoBehaviour
{
	// Token: 0x17001611 RID: 5649
	// (get) Token: 0x0600409E RID: 16542 RVA: 0x000E5091 File Offset: 0x000E3291
	// (set) Token: 0x0600409F RID: 16543 RVA: 0x000E5098 File Offset: 0x000E3298
	private static LineageToHubTown_Timer Instance { get; set; }

	// Token: 0x060040A0 RID: 16544 RVA: 0x000E50A0 File Offset: 0x000E32A0
	private void Awake()
	{
		if (!LineageToHubTown_Timer.Instance)
		{
			LineageToHubTown_Timer.Instance = this;
			this.m_onConfirmHeirInLineage = new Action<MonoBehaviour, EventArgs>(this.OnConfirmHeirInLineage);
			this.m_onSkillTreeOpened = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeOpened);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x000E50EF File Offset: 0x000E32EF
	private void Start()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.Lineage_ConfirmHeir, this.m_onConfirmHeirInLineage);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x000E510B File Offset: 0x000E330B
	private void OnSkillTreeOpened(MonoBehaviour sender, EventArgs eventArgs)
	{
		LineageToHubTown_Timer.StopTimer();
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x000E5112 File Offset: 0x000E3312
	private void OnConfirmHeirInLineage(MonoBehaviour sender, EventArgs eventArgs)
	{
		LineageToHubTown_Timer.StartTimer();
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x000E5119 File Offset: 0x000E3319
	private void OnDestroy()
	{
		if (LineageToHubTown_Timer.Instance == this)
		{
			LineageToHubTown_Timer.Instance = null;
			LineageToHubTown_Timer.m_stopwatch = null;
		}
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.Lineage_ConfirmHeir, this.m_onConfirmHeirInLineage);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x000E514E File Offset: 0x000E334E
	public static void StartTimer()
	{
		if (LineageToHubTown_Timer.m_stopwatch == null)
		{
			LineageToHubTown_Timer.m_stopwatch = new Stopwatch();
		}
		LineageToHubTown_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x000E516C File Offset: 0x000E336C
	public static void StopTimer()
	{
		if (LineageToHubTown_Timer.m_stopwatch != null && LineageToHubTown_Timer.m_stopwatch.IsRunning)
		{
			LineageToHubTown_Timer.m_stopwatch.Stop();
			UnityEngine.Debug.LogFormat("<color=blue>Time from <b>Confirm Heir</b> in Lineage to <b>Skill Tree</b> in Hub Town = {0} ms</color>", new object[]
			{
				LineageToHubTown_Timer.m_stopwatch.ElapsedMilliseconds
			});
		}
	}

	// Token: 0x040031D3 RID: 12755
	private Action<MonoBehaviour, EventArgs> m_onConfirmHeirInLineage;

	// Token: 0x040031D4 RID: 12756
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x040031D5 RID: 12757
	private static Stopwatch m_stopwatch;
}
