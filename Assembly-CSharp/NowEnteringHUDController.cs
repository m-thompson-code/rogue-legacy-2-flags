using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class NowEnteringHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x060021BC RID: 8636 RVA: 0x0006B2D9 File Offset: 0x000694D9
	public static int CurrentRiskLevel
	{
		get
		{
			return NowEnteringHUDController.m_currentRiskLevel;
		}
	}

	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x060021BD RID: 8637 RVA: 0x0006B2E0 File Offset: 0x000694E0
	public static string CurrentLocID
	{
		get
		{
			return NowEnteringHUDController.m_currentLocID;
		}
	}

	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x060021BE RID: 8638 RVA: 0x0006B2E7 File Offset: 0x000694E7
	// (set) Token: 0x060021BF RID: 8639 RVA: 0x0006B2EF File Offset: 0x000694EF
	public float TextMaskRectWidth
	{
		get
		{
			return this.m_textMaskRectWidth;
		}
		set
		{
			this.m_textMaskRectWidth = value;
			if (this.m_textMaskRectTransform != null)
			{
				this.m_textMaskRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_textMaskRectWidth);
				this.m_textMaskRectTransform.ForceUpdateRectTransforms();
			}
		}
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x0006B324 File Offset: 0x00069524
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		this.m_biomeTextCanvasGroup.gameObject.SetActive(false);
		this.m_onPlayerEnterBiome = new Action<MonoBehaviour, EventArgs>(this.OnPlayerEnterBiome);
		this.m_onPlayerDeath = new Action<MonoBehaviour, EventArgs>(this.OnPlayerDeath);
		this.m_onForceNowEntering = new Action<MonoBehaviour, EventArgs>(this.OnForceNowEntering);
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x0006B3B0 File Offset: 0x000695B0
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onPlayerEnterBiome);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ForceNowEntering, this.m_onForceNowEntering);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x0006B400 File Offset: 0x00069600
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onPlayerEnterBiome);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ForceNowEntering, this.m_onForceNowEntering);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x0006B44E File Offset: 0x0006964E
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.ClearNowEnteringText();
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x0006B458 File Offset: 0x00069658
	private void OnForceNowEntering(object sender, EventArgs args)
	{
		ForceNowEnteringEventArgs forceNowEnteringEventArgs = args as ForceNowEnteringEventArgs;
		if (forceNowEnteringEventArgs != null)
		{
			this.TriggerNowEnteringText(forceNowEnteringEventArgs.LocID, forceNowEnteringEventArgs.RiskLevel);
		}
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x0006B484 File Offset: 0x00069684
	private void OnPlayerEnterBiome(object sender, EventArgs args)
	{
		this.ClearNowEnteringText();
		BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
		if (!this.m_allowedBiomes.Contains(biomeEventArgs.Biome) || (biomeEventArgs.Biome == BiomeType.Garden && SaveManager.PlayerSaveData.EndingSpawnRoom > EndingSpawnRoomType.None))
		{
			return;
		}
		BiomeData data = BiomeDataLibrary.GetData(biomeEventArgs.Biome);
		if (!data)
		{
			return;
		}
		string biomeNameLocID = data.BiomeNameLocID;
		int biomeLevel = BiomeCreation_EV.GetBiomeLevel(biomeEventArgs.Biome);
		this.TriggerNowEnteringText(biomeNameLocID, biomeLevel);
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x0006B504 File Offset: 0x00069704
	private void TriggerNowEnteringText(string locID, int riskLevel)
	{
		if (CutsceneManager.IsCutsceneActive)
		{
			return;
		}
		if (locID == NowEnteringHUDController.m_currentLocID)
		{
			return;
		}
		NowEnteringHUDController.m_currentLocID = locID;
		NowEnteringHUDController.m_currentRiskLevel = riskLevel;
		this.m_biomeTitleText.text = LocalizationManager.GetString(NowEnteringHUDController.m_currentLocID, false, false);
		if (NowEnteringHUDController.m_currentRiskLevel == -2)
		{
			this.m_grayStarsGO.SetActive(false);
			this.m_yellowStarsGO.SetActive(false);
			this.m_unknownStarsGO.SetActive(false);
			this.m_dangerGO.SetActive(false);
		}
		else if (NowEnteringHUDController.m_currentRiskLevel == -1)
		{
			this.m_grayStarsGO.SetActive(false);
			this.m_yellowStarsGO.SetActive(false);
			this.m_unknownStarsGO.SetActive(true);
			this.m_dangerGO.SetActive(true);
		}
		else
		{
			this.m_grayStarsGO.SetActive(true);
			this.m_yellowStarsGO.SetActive(true);
			this.m_unknownStarsGO.SetActive(false);
			this.m_dangerGO.SetActive(true);
			GameObject gameObject = this.m_grayStarsGO.transform.GetChild(6).gameObject;
			GameObject gameObject2 = this.m_yellowStarsGO.transform.GetChild(6).gameObject;
			if (NowEnteringHUDController.m_currentRiskLevel > 6)
			{
				gameObject.gameObject.SetActive(true);
				gameObject2.gameObject.SetActive(true);
			}
			else
			{
				gameObject.gameObject.SetActive(false);
				gameObject2.gameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_yellowStarsGO.transform.childCount; i++)
			{
				GameObject gameObject3 = this.m_yellowStarsGO.transform.GetChild(i).gameObject;
				if (i < NowEnteringHUDController.m_currentRiskLevel)
				{
					gameObject3.SetActive(true);
				}
				else
				{
					gameObject3.SetActive(false);
				}
			}
		}
		this.ClearNowEnteringText();
		this.m_bgCanvasGroup.gameObject.SetActive(true);
		this.m_biomeTextCanvasGroup.gameObject.SetActive(true);
		this.TextMaskRectWidth = 1f;
		this.m_bgCanvasGroup.alpha = 0f;
		this.m_biomeTextCanvasGroup.alpha = 1f;
		this.RefreshText(null, null);
		base.StartCoroutine(this.DisplayBiomeTitleCoroutine());
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x0006B710 File Offset: 0x00069910
	private void ClearNowEnteringText()
	{
		base.StopAllCoroutines();
		this.m_bgCanvasGroup.alpha = 0f;
		this.m_biomeTextCanvasGroup.alpha = 0f;
		if (this.m_bgCanvasGroup.gameObject.activeSelf)
		{
			this.m_bgCanvasGroup.gameObject.SetActive(false);
		}
		if (this.m_biomeTextCanvasGroup.gameObject.activeSelf)
		{
			this.m_biomeTextCanvasGroup.gameObject.SetActive(false);
		}
		if (this.m_bgTween != null)
		{
			this.m_bgTween.StopTweenWithConditionChecks(false, this.m_bgCanvasGroup, "NowEnteringTween");
		}
		if (this.m_biomeTitleTween != null)
		{
			this.m_biomeTitleTween.StopTweenWithConditionChecks(false, this, "NowEnteringTween");
		}
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x0006B7CE File Offset: 0x000699CE
	private IEnumerator DisplayBiomeTitleCoroutine()
	{
		float fadeSpeed = 0.5f;
		this.m_bgTween = TweenManager.TweenTo(this.m_bgCanvasGroup, fadeSpeed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"alpha",
			1
		});
		this.m_bgTween.ID = "NowEnteringTween";
		float num = Mathf.Max(this.m_biomeTitleText.preferredWidth + 200f, 1000f);
		this.m_biomeTitleTween = TweenManager.TweenTo(this, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"delay",
			0.25f,
			"TextMaskRectWidth",
			num
		});
		this.m_biomeTitleTween.ID = "NowEnteringTween";
		yield return this.m_biomeTitleTween.TweenCoroutine;
		this.m_waitYield.CreateNew(this.m_displayDuration, false);
		yield return this.m_waitYield;
		this.m_bgTween = TweenManager.TweenTo(this.m_bgCanvasGroup, fadeSpeed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"alpha",
			0
		});
		this.m_bgTween.ID = "NowEnteringTween";
		this.m_biomeTitleTween = TweenManager.TweenTo(this.m_biomeTextCanvasGroup, fadeSpeed, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"delay",
			0.25f,
			"alpha",
			0
		});
		this.m_biomeTitleTween.ID = "NowEnteringTween";
		yield return this.m_biomeTitleTween.TweenCoroutine;
		this.m_bgCanvasGroup.gameObject.SetActive(false);
		this.m_biomeTextCanvasGroup.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x0006B7DD File Offset: 0x000699DD
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_biomeTitleText.text = LocalizationManager.GetString(NowEnteringHUDController.m_currentLocID, false, false);
	}

	// Token: 0x04001D44 RID: 7492
	[SerializeField]
	private float m_displayDuration = 3f;

	// Token: 0x04001D45 RID: 7493
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04001D46 RID: 7494
	[SerializeField]
	private TMP_Text m_biomeTitleText;

	// Token: 0x04001D47 RID: 7495
	[SerializeField]
	private RectTransform m_textMaskRectTransform;

	// Token: 0x04001D48 RID: 7496
	[SerializeField]
	private GameObject m_dangerGO;

	// Token: 0x04001D49 RID: 7497
	[SerializeField]
	private GameObject m_grayStarsGO;

	// Token: 0x04001D4A RID: 7498
	[SerializeField]
	private GameObject m_yellowStarsGO;

	// Token: 0x04001D4B RID: 7499
	[SerializeField]
	private GameObject m_unknownStarsGO;

	// Token: 0x04001D4C RID: 7500
	[SerializeField]
	private CanvasGroup m_biomeTextCanvasGroup;

	// Token: 0x04001D4D RID: 7501
	[SerializeField]
	private BiomeType[] m_allowedBiomes;

	// Token: 0x04001D4E RID: 7502
	private Tween m_bgTween;

	// Token: 0x04001D4F RID: 7503
	private Tween m_biomeTitleTween;

	// Token: 0x04001D50 RID: 7504
	private WaitRL_Yield m_waitYield;

	// Token: 0x04001D51 RID: 7505
	private float m_textMaskRectWidth;

	// Token: 0x04001D52 RID: 7506
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterBiome;

	// Token: 0x04001D53 RID: 7507
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x04001D54 RID: 7508
	private Action<MonoBehaviour, EventArgs> m_onForceNowEntering;

	// Token: 0x04001D55 RID: 7509
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04001D56 RID: 7510
	private static int m_currentRiskLevel;

	// Token: 0x04001D57 RID: 7511
	private static string m_currentLocID;
}
