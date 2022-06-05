using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x02000943 RID: 2371
public class BossIntroWindowController : WindowController, IAudioEventEmitter, ILocalizable
{
	// Token: 0x1700193F RID: 6463
	// (get) Token: 0x060047FD RID: 18429 RVA: 0x00027776 File Offset: 0x00025976
	public IRelayLink StartSpeedUpRelay
	{
		get
		{
			return this.m_speedUpRelay.link;
		}
	}

	// Token: 0x17001940 RID: 6464
	// (get) Token: 0x060047FE RID: 18430 RVA: 0x00027783 File Offset: 0x00025983
	public IRelayLink StopSpeedUpRelay
	{
		get
		{
			return this.m_stopSpeedUpRelay.link;
		}
	}

	// Token: 0x17001941 RID: 6465
	// (get) Token: 0x060047FF RID: 18431 RVA: 0x00027790 File Offset: 0x00025990
	// (set) Token: 0x06004800 RID: 18432 RVA: 0x00027798 File Offset: 0x00025998
	public bool WindowFadingIn { get; private set; }

	// Token: 0x17001942 RID: 6466
	// (get) Token: 0x06004801 RID: 18433 RVA: 0x000277A1 File Offset: 0x000259A1
	// (set) Token: 0x06004802 RID: 18434 RVA: 0x000277A9 File Offset: 0x000259A9
	public bool FadeInBarsAtStart { get; set; }

	// Token: 0x17001943 RID: 6467
	// (get) Token: 0x06004803 RID: 18435 RVA: 0x000277B2 File Offset: 0x000259B2
	// (set) Token: 0x06004804 RID: 18436 RVA: 0x000277BA File Offset: 0x000259BA
	public bool DisplayBossName { get; set; }

	// Token: 0x17001944 RID: 6468
	// (get) Token: 0x06004805 RID: 18437 RVA: 0x000277C3 File Offset: 0x000259C3
	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(this.m_description))
			{
				this.m_description = this.ToString();
			}
			return this.m_description;
		}
	}

	// Token: 0x17001945 RID: 6469
	// (get) Token: 0x06004806 RID: 18438 RVA: 0x000277E4 File Offset: 0x000259E4
	public override WindowID ID
	{
		get
		{
			return WindowID.BossIntro;
		}
	}

	// Token: 0x06004807 RID: 18439 RVA: 0x000277E8 File Offset: 0x000259E8
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
	}

	// Token: 0x06004808 RID: 18440 RVA: 0x0002780F File Offset: 0x00025A0F
	public override void Initialize()
	{
		base.Initialize();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x001171E0 File Offset: 0x001153E0
	protected override void OnOpen()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.InitializeOnOpen();
		this.DisplayBossName = false;
		this.m_disableInput = false;
		this.WindowFadingIn = true;
		this.m_windowCanvas.gameObject.SetActive(true);
		base.StartCoroutine(this.OnOpenCoroutine());
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x00117234 File Offset: 0x00115434
	private void InitializeOnOpen()
	{
		this.m_fastForwardObj.SetActive(false);
		if (!this.FadeInBarsAtStart)
		{
			this.m_topCanvasGroup.alpha = 1f;
			this.m_bottomCanvasGroup.alpha = 1f;
		}
		else
		{
			this.m_topCanvasGroup.alpha = 0f;
			this.m_bottomCanvasGroup.alpha = 0f;
		}
		this.m_topContentCanvasGroup.alpha = 0f;
		this.m_bottomContentCanvasGroup.alpha = 0f;
		this.m_bottomFrillCanvasGroup.alpha = 0f;
		Vector2 sizeDelta = this.m_bottomFrillMask.sizeDelta;
		sizeDelta.x = 0f;
		this.m_bottomFrillMask.sizeDelta = sizeDelta;
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x00027828 File Offset: 0x00025A28
	private IEnumerator OnOpenCoroutine()
	{
		this.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_enter");
		if (this.FadeInBarsAtStart)
		{
			TweenManager.TweenTo(this.m_topCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
			TweenManager.TweenTo(this.m_bottomCanvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
			{
				"alpha",
				1
			});
		}
		while (!this.DisplayBossName)
		{
			yield return null;
		}
		this.PlayAudio("event:/SFX/Enemies/sfx_dancingBoss_intro_title");
		TweenManager.TweenTo(this.m_topContentCanvasGroup, 1.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo(this.m_bottomContentCanvasGroup, 1.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		TweenManager.TweenTo(this.m_bottomFrillCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"delay",
			0.75f,
			"alpha",
			1
		});
		yield return TweenManager.TweenTo(this.m_bottomFrillMask, 2f, new EaseDelegate(Ease.Quad.EaseInOut), new object[]
		{
			"delay",
			0.5f,
			"sizeDelta.x",
			200
		}).TweenCoroutine;
		this.m_waitYield.CreateNew(2.5f, false);
		yield return this.m_waitYield;
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_disableInput = true;
		this.WindowFadingIn = false;
		TweenManager.TweenTo(this.m_topCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenTo(this.m_bottomCanvasGroup, 0.5f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		}).TweenCoroutine;
		WindowManager.SetWindowIsOpen(WindowID.BossIntro, false);
		yield break;
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x00027837 File Offset: 0x00025A37
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
		this.FadeInBarsAtStart = false;
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x0002785E File Offset: 0x00025A5E
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x00027890 File Offset: 0x00025A90
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x001172EC File Offset: 0x001154EC
	private void ToggleSpeedUp(InputActionEventData eventData)
	{
		if (this.m_disableInput)
		{
			return;
		}
		if (!eventData.GetButtonUp())
		{
			RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 10f);
			this.m_fastForwardObj.SetActive(true);
			this.m_speedUpRelay.Dispatch();
			return;
		}
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
		this.m_stopSpeedUpRelay.Dispatch();
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x000278C2 File Offset: 0x00025AC2
	public void StopSpeedUp()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
		this.m_stopSpeedUpRelay.Dispatch();
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x00117350 File Offset: 0x00115550
	public void SetEnemyType(EnemyType type, EnemyRank rank)
	{
		this.m_typeAndRank = new EnemyTypeAndRank(type, rank);
		EnemyData enemyData = EnemyClassLibrary.GetEnemyClassData(type).GetEnemyData(rank);
		this.m_subTitleText.text = LocalizationManager.GetString(enemyData.Description02, false, false);
		this.m_titleText.text = LocalizationManager.GetString(enemyData.Title, false, false);
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x00106DD4 File Offset: 0x00104FD4
	private void PlayAudio(string audioPath)
	{
		if (!string.IsNullOrEmpty(audioPath))
		{
			AudioManager.Play(this, audioPath, default(Vector3));
		}
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x000278E6 File Offset: 0x00025AE6
	public void RefreshText(object sender, EventArgs args)
	{
		this.SetEnemyType(this.m_typeAndRank.Type, this.m_typeAndRank.Rank);
	}

	// Token: 0x0400370A RID: 14090
	private const string ON_ENTER_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_enter";

	// Token: 0x0400370B RID: 14091
	private const string DISPLAY_NAME_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_title";

	// Token: 0x0400370C RID: 14092
	[SerializeField]
	private CanvasGroup m_topCanvasGroup;

	// Token: 0x0400370D RID: 14093
	[SerializeField]
	private CanvasGroup m_bottomCanvasGroup;

	// Token: 0x0400370E RID: 14094
	[SerializeField]
	private CanvasGroup m_topContentCanvasGroup;

	// Token: 0x0400370F RID: 14095
	[SerializeField]
	private CanvasGroup m_bottomContentCanvasGroup;

	// Token: 0x04003710 RID: 14096
	[SerializeField]
	private TMP_Text m_subTitleText;

	// Token: 0x04003711 RID: 14097
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x04003712 RID: 14098
	[SerializeField]
	private RectTransform m_bottomFrillMask;

	// Token: 0x04003713 RID: 14099
	[SerializeField]
	private CanvasGroup m_bottomFrillCanvasGroup;

	// Token: 0x04003714 RID: 14100
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x04003715 RID: 14101
	private WaitRL_Yield m_waitYield;

	// Token: 0x04003716 RID: 14102
	private bool m_disableInput;

	// Token: 0x04003717 RID: 14103
	private string m_description;

	// Token: 0x04003718 RID: 14104
	private EnemyTypeAndRank m_typeAndRank;

	// Token: 0x04003719 RID: 14105
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x0400371A RID: 14106
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x0400371B RID: 14107
	private Relay m_speedUpRelay = new Relay();

	// Token: 0x0400371C RID: 14108
	private Relay m_stopSpeedUpRelay = new Relay();
}
