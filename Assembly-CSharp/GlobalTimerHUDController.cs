using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class GlobalTimerHUDController : MonoBehaviour
{
	// Token: 0x17001298 RID: 4760
	// (get) Token: 0x06002F41 RID: 12097 RVA: 0x00019DEC File Offset: 0x00017FEC
	// (set) Token: 0x06002F42 RID: 12098 RVA: 0x00019DF3 File Offset: 0x00017FF3
	public static float ElapsedTime { get; set; }

	// Token: 0x17001299 RID: 4761
	// (get) Token: 0x06002F43 RID: 12099 RVA: 0x00019DFB File Offset: 0x00017FFB
	// (set) Token: 0x06002F44 RID: 12100 RVA: 0x00019E02 File Offset: 0x00018002
	public static bool IsRunning { get; private set; }

	// Token: 0x1700129A RID: 4762
	// (get) Token: 0x06002F45 RID: 12101 RVA: 0x00019E0A File Offset: 0x0001800A
	// (set) Token: 0x06002F46 RID: 12102 RVA: 0x00019E11 File Offset: 0x00018011
	public static bool ReverseTimer { get; set; }

	// Token: 0x1700129B RID: 4763
	// (get) Token: 0x06002F47 RID: 12103 RVA: 0x00019E19 File Offset: 0x00018019
	// (set) Token: 0x06002F48 RID: 12104 RVA: 0x00019E20 File Offset: 0x00018020
	public static bool SlowTime { get; set; }

	// Token: 0x1700129C RID: 4764
	// (get) Token: 0x06002F49 RID: 12105 RVA: 0x00019E28 File Offset: 0x00018028
	// (set) Token: 0x06002F4A RID: 12106 RVA: 0x00019E2F File Offset: 0x0001802F
	public static float ReverseStartTime { get; set; }

	// Token: 0x1700129D RID: 4765
	// (get) Token: 0x06002F4B RID: 12107 RVA: 0x00019E37 File Offset: 0x00018037
	// (set) Token: 0x06002F4C RID: 12108 RVA: 0x00019E3E File Offset: 0x0001803E
	public static bool TrackNegativeTimeAchievement { get; set; }

	// Token: 0x06002F4D RID: 12109 RVA: 0x000C9F2C File Offset: 0x000C812C
	private void Awake()
	{
		this.OnResetTimer(null, null);
		this.OnHideTimer(null, null);
		this.m_onDisplayTimer = new Action<MonoBehaviour, EventArgs>(this.OnDisplayTimer);
		this.m_onHideTimer = new Action<MonoBehaviour, EventArgs>(this.OnHideTimer);
		this.m_onStartTimer = new Action<MonoBehaviour, EventArgs>(this.OnStartTimer);
		this.m_onStopTimer = new Action<MonoBehaviour, EventArgs>(this.OnStopTimer);
		this.m_onResetTimer = new Action<MonoBehaviour, EventArgs>(this.OnResetTimer);
	}

	// Token: 0x06002F4E RID: 12110 RVA: 0x000C9FA4 File Offset: 0x000C81A4
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.DisplayGlobalTimer, this.m_onDisplayTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.HideGlobalTimer, this.m_onHideTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StartGlobalTimer, this.m_onStartTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StopGlobalTimer, this.m_onStopTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ResetGlobalTimer, this.m_onResetTimer);
	}

	// Token: 0x06002F4F RID: 12111 RVA: 0x000C9FF4 File Offset: 0x000C81F4
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.DisplayGlobalTimer, this.m_onDisplayTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.HideGlobalTimer, this.m_onHideTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StartGlobalTimer, this.m_onStartTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StopGlobalTimer, this.m_onStopTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ResetGlobalTimer, this.m_onResetTimer);
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x00019E46 File Offset: 0x00018046
	private void OnDisplayTimer(object sender, EventArgs args)
	{
		if (!this.m_canvasGO.activeSelf)
		{
			this.m_canvasGO.SetActive(true);
		}
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x00019E61 File Offset: 0x00018061
	private void OnHideTimer(object sender, EventArgs args)
	{
		if (this.m_canvasGO.activeSelf)
		{
			this.m_canvasGO.SetActive(false);
		}
	}

	// Token: 0x06002F52 RID: 12114 RVA: 0x00019E7C File Offset: 0x0001807C
	private void OnStartTimer(object sender, EventArgs args)
	{
		base.StopAllCoroutines();
		GlobalTimerHUDController.IsRunning = true;
		base.StartCoroutine(this.TimerCoroutine());
	}

	// Token: 0x06002F53 RID: 12115 RVA: 0x00019E97 File Offset: 0x00018097
	private void OnStopTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.IsRunning = false;
		base.StopAllCoroutines();
	}

	// Token: 0x06002F54 RID: 12116 RVA: 0x000CA044 File Offset: 0x000C8244
	private void OnResetTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.ElapsedTime = 0f;
		GlobalTimerHUDController.ReverseTimer = false;
		GlobalTimerHUDController.ReverseStartTime = 0f;
		GlobalTimerHUDController.SlowTime = false;
		GlobalTimerHUDController.TrackNegativeTimeAchievement = false;
		this.m_timerTextObj.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 0, 0, 0);
	}

	// Token: 0x06002F55 RID: 12117 RVA: 0x00019EA5 File Offset: 0x000180A5
	private IEnumerator TimerCoroutine()
	{
		for (;;)
		{
			if (GlobalTimerHUDController.SlowTime)
			{
				float slowTimeDelay = Time.time + 0.75f;
				while (Time.time <= slowTimeDelay)
				{
					yield return null;
				}
			}
			yield return null;
			bool flag = GlobalTimerHUDController.ElapsedTime < 0f;
			if (GlobalTimerHUDController.ReverseTimer)
			{
				flag = (GlobalTimerHUDController.ReverseStartTime - Mathf.Abs(GlobalTimerHUDController.ElapsedTime) < 0f);
			}
			if (GlobalTimerHUDController.SlowTime)
			{
				GlobalTimerHUDController.ElapsedTime += 0.1373f;
			}
			else
			{
				GlobalTimerHUDController.ElapsedTime += Time.deltaTime;
			}
			if (GlobalTimerHUDController.TrackNegativeTimeAchievement)
			{
				bool flag2 = GlobalTimerHUDController.ElapsedTime < 0f;
				if (GlobalTimerHUDController.ReverseTimer)
				{
					flag2 = (GlobalTimerHUDController.ReverseStartTime - Mathf.Abs(GlobalTimerHUDController.ElapsedTime) < 0f);
				}
				if (!flag && flag2)
				{
					StoreAPIManager.GiveAchievement(AchievementType.EndTimerFail, StoreType.All);
				}
			}
			this.m_timerTextObj.text = GlobalTimerHUDController.GetTimerString();
		}
		yield break;
	}

	// Token: 0x06002F56 RID: 12118 RVA: 0x000CA0A0 File Offset: 0x000C82A0
	public static string GetTimerString()
	{
		float num = Mathf.Abs(GlobalTimerHUDController.ElapsedTime);
		bool flag = GlobalTimerHUDController.ElapsedTime < 0f;
		if (GlobalTimerHUDController.ReverseTimer)
		{
			num = GlobalTimerHUDController.ReverseStartTime - num;
			flag = (num < 0f);
			num = Mathf.Abs(num);
		}
		int num2 = (int)(num / 3600f);
		int num3 = (int)(num % 3600f / 60f);
		int num4 = (int)(num % 60f);
		int num5 = (int)((num - (float)((int)num)) * 100f);
		if (num2 == 0)
		{
			return string.Format("{3}{0:D2}:{1:D2}:{2:D2}", new object[]
			{
				num3,
				num4,
				num5,
				(!flag) ? "" : "-"
			});
		}
		return string.Format("{4}{0}:{1:D2}:{2:D2}:{3:D2}", new object[]
		{
			num2,
			num3,
			num4,
			num5,
			(!flag) ? "" : "-"
		});
	}

	// Token: 0x040026AE RID: 9902
	private const string INITIAL_TIMER_FORMAT = "{0:D2}:{1:D2}:{2:D2}";

	// Token: 0x040026AF RID: 9903
	private const string TIMER_FORMAT_A = "{3}{0:D2}:{1:D2}:{2:D2}";

	// Token: 0x040026B0 RID: 9904
	private const string TIMER_FORMAT_B = "{4}{0}:{1:D2}:{2:D2}:{3:D2}";

	// Token: 0x040026B1 RID: 9905
	[SerializeField]
	private GameObject m_canvasGO;

	// Token: 0x040026B2 RID: 9906
	[SerializeField]
	private TMP_Text m_timerTextObj;

	// Token: 0x040026B9 RID: 9913
	private Action<MonoBehaviour, EventArgs> m_onDisplayTimer;

	// Token: 0x040026BA RID: 9914
	private Action<MonoBehaviour, EventArgs> m_onHideTimer;

	// Token: 0x040026BB RID: 9915
	private Action<MonoBehaviour, EventArgs> m_onStartTimer;

	// Token: 0x040026BC RID: 9916
	private Action<MonoBehaviour, EventArgs> m_onStopTimer;

	// Token: 0x040026BD RID: 9917
	private Action<MonoBehaviour, EventArgs> m_onResetTimer;
}
