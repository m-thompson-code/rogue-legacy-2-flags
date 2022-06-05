using System;
using System.Collections;
using Rewired;
using RLAudio;
using RL_Windows;
using Sigtrap.Relays;
using TMPro;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class BossIntroWindowController : WindowController, IAudioEventEmitter, ILocalizable
{
	// Token: 0x17001274 RID: 4724
	// (get) Token: 0x06003316 RID: 13078 RVA: 0x000ACB6B File Offset: 0x000AAD6B
	public IRelayLink StartSpeedUpRelay
	{
		get
		{
			return this.m_speedUpRelay.link;
		}
	}

	// Token: 0x17001275 RID: 4725
	// (get) Token: 0x06003317 RID: 13079 RVA: 0x000ACB78 File Offset: 0x000AAD78
	public IRelayLink StopSpeedUpRelay
	{
		get
		{
			return this.m_stopSpeedUpRelay.link;
		}
	}

	// Token: 0x17001276 RID: 4726
	// (get) Token: 0x06003318 RID: 13080 RVA: 0x000ACB85 File Offset: 0x000AAD85
	// (set) Token: 0x06003319 RID: 13081 RVA: 0x000ACB8D File Offset: 0x000AAD8D
	public bool WindowFadingIn { get; private set; }

	// Token: 0x17001277 RID: 4727
	// (get) Token: 0x0600331A RID: 13082 RVA: 0x000ACB96 File Offset: 0x000AAD96
	// (set) Token: 0x0600331B RID: 13083 RVA: 0x000ACB9E File Offset: 0x000AAD9E
	public bool FadeInBarsAtStart { get; set; }

	// Token: 0x17001278 RID: 4728
	// (get) Token: 0x0600331C RID: 13084 RVA: 0x000ACBA7 File Offset: 0x000AADA7
	// (set) Token: 0x0600331D RID: 13085 RVA: 0x000ACBAF File Offset: 0x000AADAF
	public bool DisplayBossName { get; set; }

	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x0600331E RID: 13086 RVA: 0x000ACBB8 File Offset: 0x000AADB8
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

	// Token: 0x1700127A RID: 4730
	// (get) Token: 0x0600331F RID: 13087 RVA: 0x000ACBD9 File Offset: 0x000AADD9
	public override WindowID ID
	{
		get
		{
			return WindowID.BossIntro;
		}
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x000ACBDD File Offset: 0x000AADDD
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
		this.m_toggleSpeedUp = new Action<InputActionEventData>(this.ToggleSpeedUp);
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x000ACC04 File Offset: 0x000AAE04
	public override void Initialize()
	{
		base.Initialize();
		this.m_waitYield = new WaitRL_Yield(0f, false);
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000ACC20 File Offset: 0x000AAE20
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

	// Token: 0x06003323 RID: 13091 RVA: 0x000ACC74 File Offset: 0x000AAE74
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

	// Token: 0x06003324 RID: 13092 RVA: 0x000ACD2B File Offset: 0x000AAF2B
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

	// Token: 0x06003325 RID: 13093 RVA: 0x000ACD3A File Offset: 0x000AAF3A
	protected override void OnClose()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
		this.m_windowCanvas.gameObject.SetActive(false);
		this.FadeInBarsAtStart = false;
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x000ACD61 File Offset: 0x000AAF61
	protected override void OnFocus()
	{
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.AddInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x000ACD93 File Offset: 0x000AAF93
	protected override void OnLostFocus()
	{
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Window_Confirm");
		base.RewiredPlayer.RemoveInputEventDelegate(this.m_toggleSpeedUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Window_Confirm");
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x000ACDC8 File Offset: 0x000AAFC8
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

	// Token: 0x06003329 RID: 13097 RVA: 0x000ACE2C File Offset: 0x000AB02C
	public void StopSpeedUp()
	{
		RLTimeScale.SetTimeScale(TimeScaleType.Cutscene, 1f);
		this.m_fastForwardObj.SetActive(false);
		this.m_stopSpeedUpRelay.Dispatch();
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x000ACE50 File Offset: 0x000AB050
	public void SetEnemyType(EnemyType type, EnemyRank rank)
	{
		this.m_typeAndRank = new EnemyTypeAndRank(type, rank);
		EnemyData enemyData = EnemyClassLibrary.GetEnemyClassData(type).GetEnemyData(rank);
		this.m_subTitleText.text = LocalizationManager.GetString(enemyData.Description02, false, false);
		this.m_titleText.text = LocalizationManager.GetString(enemyData.Title, false, false);
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x000ACEA8 File Offset: 0x000AB0A8
	private void PlayAudio(string audioPath)
	{
		if (!string.IsNullOrEmpty(audioPath))
		{
			AudioManager.Play(this, audioPath, default(Vector3));
		}
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x000ACECD File Offset: 0x000AB0CD
	public void RefreshText(object sender, EventArgs args)
	{
		this.SetEnemyType(this.m_typeAndRank.Type, this.m_typeAndRank.Rank);
	}

	// Token: 0x040027D2 RID: 10194
	private const string ON_ENTER_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_enter";

	// Token: 0x040027D3 RID: 10195
	private const string DISPLAY_NAME_AUDIO = "event:/SFX/Enemies/sfx_dancingBoss_intro_title";

	// Token: 0x040027D4 RID: 10196
	[SerializeField]
	private CanvasGroup m_topCanvasGroup;

	// Token: 0x040027D5 RID: 10197
	[SerializeField]
	private CanvasGroup m_bottomCanvasGroup;

	// Token: 0x040027D6 RID: 10198
	[SerializeField]
	private CanvasGroup m_topContentCanvasGroup;

	// Token: 0x040027D7 RID: 10199
	[SerializeField]
	private CanvasGroup m_bottomContentCanvasGroup;

	// Token: 0x040027D8 RID: 10200
	[SerializeField]
	private TMP_Text m_subTitleText;

	// Token: 0x040027D9 RID: 10201
	[SerializeField]
	private TMP_Text m_titleText;

	// Token: 0x040027DA RID: 10202
	[SerializeField]
	private RectTransform m_bottomFrillMask;

	// Token: 0x040027DB RID: 10203
	[SerializeField]
	private CanvasGroup m_bottomFrillCanvasGroup;

	// Token: 0x040027DC RID: 10204
	[SerializeField]
	private GameObject m_fastForwardObj;

	// Token: 0x040027DD RID: 10205
	private WaitRL_Yield m_waitYield;

	// Token: 0x040027DE RID: 10206
	private bool m_disableInput;

	// Token: 0x040027DF RID: 10207
	private string m_description;

	// Token: 0x040027E0 RID: 10208
	private EnemyTypeAndRank m_typeAndRank;

	// Token: 0x040027E1 RID: 10209
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x040027E2 RID: 10210
	private Action<InputActionEventData> m_toggleSpeedUp;

	// Token: 0x040027E3 RID: 10211
	private Relay m_speedUpRelay = new Relay();

	// Token: 0x040027E4 RID: 10212
	private Relay m_stopSpeedUpRelay = new Relay();
}
