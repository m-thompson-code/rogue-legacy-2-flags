using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class GlobalTimerHUDController : MonoBehaviour
{
	// Token: 0x17000E15 RID: 3605
	// (get) Token: 0x0600215A RID: 8538 RVA: 0x0006909F File Offset: 0x0006729F
	// (set) Token: 0x0600215B RID: 8539 RVA: 0x000690A6 File Offset: 0x000672A6
	public static float ElapsedTime { get; set; }

	// Token: 0x17000E16 RID: 3606
	// (get) Token: 0x0600215C RID: 8540 RVA: 0x000690AE File Offset: 0x000672AE
	// (set) Token: 0x0600215D RID: 8541 RVA: 0x000690B5 File Offset: 0x000672B5
	public static bool IsRunning { get; private set; }

	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x0600215E RID: 8542 RVA: 0x000690BD File Offset: 0x000672BD
	// (set) Token: 0x0600215F RID: 8543 RVA: 0x000690C4 File Offset: 0x000672C4
	public static bool ReverseTimer { get; set; }

	// Token: 0x17000E18 RID: 3608
	// (get) Token: 0x06002160 RID: 8544 RVA: 0x000690CC File Offset: 0x000672CC
	// (set) Token: 0x06002161 RID: 8545 RVA: 0x000690D3 File Offset: 0x000672D3
	public static bool SlowTime { get; set; }

	// Token: 0x17000E19 RID: 3609
	// (get) Token: 0x06002162 RID: 8546 RVA: 0x000690DB File Offset: 0x000672DB
	// (set) Token: 0x06002163 RID: 8547 RVA: 0x000690E2 File Offset: 0x000672E2
	public static float ReverseStartTime { get; set; }

	// Token: 0x17000E1A RID: 3610
	// (get) Token: 0x06002164 RID: 8548 RVA: 0x000690EA File Offset: 0x000672EA
	// (set) Token: 0x06002165 RID: 8549 RVA: 0x000690F1 File Offset: 0x000672F1
	public static bool TrackNegativeTimeAchievement { get; set; }

	// Token: 0x06002166 RID: 8550 RVA: 0x000690FC File Offset: 0x000672FC
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

	// Token: 0x06002167 RID: 8551 RVA: 0x00069174 File Offset: 0x00067374
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.DisplayGlobalTimer, this.m_onDisplayTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.HideGlobalTimer, this.m_onHideTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StartGlobalTimer, this.m_onStartTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.StopGlobalTimer, this.m_onStopTimer);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ResetGlobalTimer, this.m_onResetTimer);
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000691C4 File Offset: 0x000673C4
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.DisplayGlobalTimer, this.m_onDisplayTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.HideGlobalTimer, this.m_onHideTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StartGlobalTimer, this.m_onStartTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.StopGlobalTimer, this.m_onStopTimer);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ResetGlobalTimer, this.m_onResetTimer);
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x00069212 File Offset: 0x00067412
	private void OnDisplayTimer(object sender, EventArgs args)
	{
		if (!this.m_canvasGO.activeSelf)
		{
			this.m_canvasGO.SetActive(true);
		}
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x0006922D File Offset: 0x0006742D
	private void OnHideTimer(object sender, EventArgs args)
	{
		if (this.m_canvasGO.activeSelf)
		{
			this.m_canvasGO.SetActive(false);
		}
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x00069248 File Offset: 0x00067448
	private void OnStartTimer(object sender, EventArgs args)
	{
		base.StopAllCoroutines();
		GlobalTimerHUDController.IsRunning = true;
		base.StartCoroutine(this.TimerCoroutine());
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x00069263 File Offset: 0x00067463
	private void OnStopTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.IsRunning = false;
		base.StopAllCoroutines();
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x00069274 File Offset: 0x00067474
	private void OnResetTimer(object sender, EventArgs args)
	{
		GlobalTimerHUDController.ElapsedTime = 0f;
		GlobalTimerHUDController.ReverseTimer = false;
		GlobalTimerHUDController.ReverseStartTime = 0f;
		GlobalTimerHUDController.SlowTime = false;
		GlobalTimerHUDController.TrackNegativeTimeAchievement = false;
		this.m_timerTextObj.text = string.Format("{0:D2}:{1:D2}:{2:D2}", 0, 0, 0);
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000692CE File Offset: 0x000674CE
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

	// Token: 0x0600216F RID: 8559 RVA: 0x000692E0 File Offset: 0x000674E0
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

	// Token: 0x04001CDE RID: 7390
	private const string INITIAL_TIMER_FORMAT = "{0:D2}:{1:D2}:{2:D2}";

	// Token: 0x04001CDF RID: 7391
	private const string TIMER_FORMAT_A = "{3}{0:D2}:{1:D2}:{2:D2}";

	// Token: 0x04001CE0 RID: 7392
	private const string TIMER_FORMAT_B = "{4}{0}:{1:D2}:{2:D2}:{3:D2}";

	// Token: 0x04001CE1 RID: 7393
	[SerializeField]
	private GameObject m_canvasGO;

	// Token: 0x04001CE2 RID: 7394
	[SerializeField]
	private TMP_Text m_timerTextObj;

	// Token: 0x04001CE9 RID: 7401
	private Action<MonoBehaviour, EventArgs> m_onDisplayTimer;

	// Token: 0x04001CEA RID: 7402
	private Action<MonoBehaviour, EventArgs> m_onHideTimer;

	// Token: 0x04001CEB RID: 7403
	private Action<MonoBehaviour, EventArgs> m_onStartTimer;

	// Token: 0x04001CEC RID: 7404
	private Action<MonoBehaviour, EventArgs> m_onStopTimer;

	// Token: 0x04001CED RID: 7405
	private Action<MonoBehaviour, EventArgs> m_onResetTimer;
}
