using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BA9 RID: 2985
public class LineageToHubTown_Timer : MonoBehaviour
{
	// Token: 0x17001E0D RID: 7693
	// (get) Token: 0x060059E7 RID: 23015 RVA: 0x00031121 File Offset: 0x0002F321
	// (set) Token: 0x060059E8 RID: 23016 RVA: 0x00031128 File Offset: 0x0002F328
	private static LineageToHubTown_Timer Instance { get; set; }

	// Token: 0x060059E9 RID: 23017 RVA: 0x001548C4 File Offset: 0x00152AC4
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

	// Token: 0x060059EA RID: 23018 RVA: 0x00031130 File Offset: 0x0002F330
	private void Start()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.Lineage_ConfirmHeir, this.m_onConfirmHeirInLineage);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
	}

	// Token: 0x060059EB RID: 23019 RVA: 0x0003114C File Offset: 0x0002F34C
	private void OnSkillTreeOpened(MonoBehaviour sender, EventArgs eventArgs)
	{
		LineageToHubTown_Timer.StopTimer();
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x00031153 File Offset: 0x0002F353
	private void OnConfirmHeirInLineage(MonoBehaviour sender, EventArgs eventArgs)
	{
		LineageToHubTown_Timer.StartTimer();
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x0003115A File Offset: 0x0002F35A
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

	// Token: 0x060059EE RID: 23022 RVA: 0x0003118F File Offset: 0x0002F38F
	public static void StartTimer()
	{
		if (LineageToHubTown_Timer.m_stopwatch == null)
		{
			LineageToHubTown_Timer.m_stopwatch = new Stopwatch();
		}
		LineageToHubTown_Timer.m_stopwatch.Restart();
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x00154914 File Offset: 0x00152B14
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

	// Token: 0x0400444E RID: 17486
	private Action<MonoBehaviour, EventArgs> m_onConfirmHeirInLineage;

	// Token: 0x0400444F RID: 17487
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x04004450 RID: 17488
	private static Stopwatch m_stopwatch;
}
