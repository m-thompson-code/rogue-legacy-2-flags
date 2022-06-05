using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public class NowEnteringHUDController : MonoBehaviour, ILocalizable
{
	// Token: 0x170012B2 RID: 4786
	// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x0001A238 File Offset: 0x00018438
	public static int CurrentRiskLevel
	{
		get
		{
			return NowEnteringHUDController.m_currentRiskLevel;
		}
	}

	// Token: 0x170012B3 RID: 4787
	// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x0001A23F File Offset: 0x0001843F
	public static string CurrentLocID
	{
		get
		{
			return NowEnteringHUDController.m_currentLocID;
		}
	}

	// Token: 0x170012B4 RID: 4788
	// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x0001A246 File Offset: 0x00018446
	// (set) Token: 0x06002FC5 RID: 12229 RVA: 0x0001A24E File Offset: 0x0001844E
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

	// Token: 0x06002FC6 RID: 12230 RVA: 0x000CC2C4 File Offset: 0x000CA4C4
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

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000CC350 File Offset: 0x000CA550
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onPlayerEnterBiome);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.ForceNowEntering, this.m_onForceNowEntering);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x000CC3A0 File Offset: 0x000CA5A0
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onPlayerEnterBiome);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDeath, this.m_onPlayerDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerFakedDeath, this.m_onPlayerDeath);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.ForceNowEntering, this.m_onForceNowEntering);
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x0001A282 File Offset: 0x00018482
	private void OnPlayerDeath(object sender, EventArgs args)
	{
		this.ClearNowEnteringText();
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x000CC3F0 File Offset: 0x000CA5F0
	private void OnForceNowEntering(object sender, EventArgs args)
	{
		ForceNowEnteringEventArgs forceNowEnteringEventArgs = args as ForceNowEnteringEventArgs;
		if (forceNowEnteringEventArgs != null)
		{
			this.TriggerNowEnteringText(forceNowEnteringEventArgs.LocID, forceNowEnteringEventArgs.RiskLevel);
		}
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x000CC41C File Offset: 0x000CA61C
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

	// Token: 0x06002FCC RID: 12236 RVA: 0x000CC49C File Offset: 0x000CA69C
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

	// Token: 0x06002FCD RID: 12237 RVA: 0x000CC6A8 File Offset: 0x000CA8A8
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

	// Token: 0x06002FCE RID: 12238 RVA: 0x0001A28A File Offset: 0x0001848A
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

	// Token: 0x06002FCF RID: 12239 RVA: 0x0001A299 File Offset: 0x00018499
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_biomeTitleText.text = LocalizationManager.GetString(NowEnteringHUDController.m_currentLocID, false, false);
	}

	// Token: 0x04002736 RID: 10038
	[SerializeField]
	private float m_displayDuration = 3f;

	// Token: 0x04002737 RID: 10039
	[SerializeField]
	private CanvasGroup m_bgCanvasGroup;

	// Token: 0x04002738 RID: 10040
	[SerializeField]
	private TMP_Text m_biomeTitleText;

	// Token: 0x04002739 RID: 10041
	[SerializeField]
	private RectTransform m_textMaskRectTransform;

	// Token: 0x0400273A RID: 10042
	[SerializeField]
	private GameObject m_dangerGO;

	// Token: 0x0400273B RID: 10043
	[SerializeField]
	private GameObject m_grayStarsGO;

	// Token: 0x0400273C RID: 10044
	[SerializeField]
	private GameObject m_yellowStarsGO;

	// Token: 0x0400273D RID: 10045
	[SerializeField]
	private GameObject m_unknownStarsGO;

	// Token: 0x0400273E RID: 10046
	[SerializeField]
	private CanvasGroup m_biomeTextCanvasGroup;

	// Token: 0x0400273F RID: 10047
	[SerializeField]
	private BiomeType[] m_allowedBiomes;

	// Token: 0x04002740 RID: 10048
	private Tween m_bgTween;

	// Token: 0x04002741 RID: 10049
	private Tween m_biomeTitleTween;

	// Token: 0x04002742 RID: 10050
	private WaitRL_Yield m_waitYield;

	// Token: 0x04002743 RID: 10051
	private float m_textMaskRectWidth;

	// Token: 0x04002744 RID: 10052
	private Action<MonoBehaviour, EventArgs> m_onPlayerEnterBiome;

	// Token: 0x04002745 RID: 10053
	private Action<MonoBehaviour, EventArgs> m_onPlayerDeath;

	// Token: 0x04002746 RID: 10054
	private Action<MonoBehaviour, EventArgs> m_onForceNowEntering;

	// Token: 0x04002747 RID: 10055
	private Action<MonoBehaviour, EventArgs> m_refreshText;

	// Token: 0x04002748 RID: 10056
	private static int m_currentRiskLevel;

	// Token: 0x04002749 RID: 10057
	private static string m_currentLocID;
}
