using System;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200068A RID: 1674
public class TutorialTextBoxController : MonoBehaviour
{
	// Token: 0x17001385 RID: 4997
	// (get) Token: 0x06003323 RID: 13091 RVA: 0x0001C0B3 File Offset: 0x0001A2B3
	// (set) Token: 0x06003324 RID: 13092 RVA: 0x0001C0BB File Offset: 0x0001A2BB
	public bool StickToCamera
	{
		get
		{
			return this.m_stickToCamera;
		}
		set
		{
			this.m_stickToCamera = value;
		}
	}

	// Token: 0x17001386 RID: 4998
	// (get) Token: 0x06003325 RID: 13093 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
	// (set) Token: 0x06003326 RID: 13094 RVA: 0x0001C0CC File Offset: 0x0001A2CC
	public ConditionFlag MustHaveConditions
	{
		get
		{
			return this.m_mustHaveConditions;
		}
		set
		{
			this.m_mustHaveConditions = value;
		}
	}

	// Token: 0x17001387 RID: 4999
	// (get) Token: 0x06003327 RID: 13095 RVA: 0x0001C0D5 File Offset: 0x0001A2D5
	// (set) Token: 0x06003328 RID: 13096 RVA: 0x0001C0DD File Offset: 0x0001A2DD
	public ConditionFlag MustNotHaveConditions
	{
		get
		{
			return this.m_mustNotHaveConditions;
		}
		set
		{
			this.m_mustNotHaveConditions = value;
		}
	}

	// Token: 0x06003329 RID: 13097 RVA: 0x000DA7F8 File Offset: 0x000D89F8
	private void Awake()
	{
		this.m_disableCanvasAction = new Action(this.DisableCanvas);
		this.m_text.gameObject.SetActive(false);
		if (!this.m_displayText)
		{
			this.m_canvasGroup.gameObject.SetActive(false);
		}
		this.m_canvasGroup.alpha = 0f;
		this.m_onPlayerExitRoom = new Action<MonoBehaviour, EventArgs>(this.OnPlayerExitRoom);
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x0001C0E6 File Offset: 0x0001A2E6
	private IEnumerator Start()
	{
		while (!SceneManager.GetActiveScene().isLoaded)
		{
			yield return null;
		}
		this.m_text.gameObject.SetActive(true);
		TextGlyphConverter glyphConverter = this.m_text.GetComponent<TextGlyphConverter>();
		while (!glyphConverter.IsInitialized)
		{
			yield return null;
		}
		if (this.m_stickToCamera)
		{
			base.gameObject.transform.SetParent(CameraController.GameCamera.transform, false);
			base.gameObject.transform.localPosition = this.TUTORIAL_TEXT_POSITION;
			SceneManager.activeSceneChanged += this.RemoveFromCamera;
		}
		yield break;
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x000DA864 File Offset: 0x000D8A64
	private void OnEnable()
	{
		if (this.m_stickToCamera)
		{
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		if (TraitManager.IsTraitActive(TraitType.UpsideDown))
		{
			if (base.transform.localScale.y > 0f)
			{
				base.transform.SetLocalScaleY(base.transform.localScale.y * -1f);
				return;
			}
		}
		else if (base.transform.localScale.y < 0f)
		{
			base.transform.SetLocalScaleY(base.transform.localScale.y * -1f);
		}
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x0001C0F5 File Offset: 0x0001A2F5
	private void OnDisable()
	{
		if (this.m_stickToCamera)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		this.m_displayText = false;
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x0001C113 File Offset: 0x0001A313
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.HideText();
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x0001C11B File Offset: 0x0001A31B
	private void RemoveFromCamera(Scene arg0, Scene arg1)
	{
		SceneManager.activeSceneChanged -= this.RemoveFromCamera;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x000DA904 File Offset: 0x000D8B04
	public void HideText()
	{
		if (!this.m_canvasGroup.gameObject.activeSelf)
		{
			return;
		}
		this.m_hideEventEmitter.Play();
		if (this.m_fadeTween != null)
		{
			this.m_fadeTween.StopTweenWithConditionChecks(false, this.m_canvasGroup, "TutorialTextTween");
		}
		this.m_fadeTween = TweenManager.TweenTo(this.m_canvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		this.m_fadeTween.OnTweenCompleteRelay.AddListener(this.m_disableCanvasAction, false);
		this.m_fadeTween.ID = "TutorialTextTween";
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x0001C139 File Offset: 0x0001A339
	private void DisableCanvas()
	{
		this.m_canvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x000DA9B8 File Offset: 0x000D8BB8
	public void DisplayText()
	{
		if (!this.AllMustHaveConditionsMet() || !this.AllMustNotHaveConditionsMet())
		{
			return;
		}
		this.m_showEventEmitter.Play();
		if (this.m_fadeTween != null)
		{
			this.m_fadeTween.StopTweenWithConditionChecks(false, this.m_canvasGroup, "TutorialTextTween");
		}
		this.m_displayText = true;
		if (!this.m_canvasGroup.gameObject.activeSelf)
		{
			this.m_canvasGroup.gameObject.SetActive(true);
		}
		this.m_fadeTween = TweenManager.TweenTo(this.m_canvasGroup, 0.25f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		this.m_fadeTween.ID = "TutorialTextTween";
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x000DAA7C File Offset: 0x000D8C7C
	private bool AllMustHaveConditionsMet()
	{
		if (this.m_mustHaveConditions == ConditionFlag.None)
		{
			return true;
		}
		foreach (ConditionFlag conditionFlag in ConditionFlag_RL.TypeArray)
		{
			if ((this.m_mustHaveConditions & conditionFlag) != ConditionFlag.None && !ConditionFlag_RL.IsConditionFulfilled(conditionFlag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x000DAAC0 File Offset: 0x000D8CC0
	private bool AllMustNotHaveConditionsMet()
	{
		if (this.m_mustNotHaveConditions == ConditionFlag.None)
		{
			return true;
		}
		foreach (ConditionFlag conditionFlag in ConditionFlag_RL.TypeArray)
		{
			if ((this.m_mustNotHaveConditions & conditionFlag) != ConditionFlag.None && ConditionFlag_RL.IsConditionFulfilled(conditionFlag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040029BB RID: 10683
	private const float FADE_DURATION = 0.25f;

	// Token: 0x040029BC RID: 10684
	private const float BACKGROUND_OPACITY = 0.425f;

	// Token: 0x040029BD RID: 10685
	private Vector3 TUTORIAL_TEXT_POSITION = new Vector3(0f, 6f, 1f);

	// Token: 0x040029BE RID: 10686
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x040029BF RID: 10687
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x040029C0 RID: 10688
	[SerializeField]
	private bool m_stickToCamera;

	// Token: 0x040029C1 RID: 10689
	[SerializeField]
	private ConditionFlag m_mustHaveConditions;

	// Token: 0x040029C2 RID: 10690
	[SerializeField]
	private ConditionFlag m_mustNotHaveConditions;

	// Token: 0x040029C3 RID: 10691
	[SerializeField]
	private StudioEventEmitter m_showEventEmitter;

	// Token: 0x040029C4 RID: 10692
	[SerializeField]
	private StudioEventEmitter m_hideEventEmitter;

	// Token: 0x040029C5 RID: 10693
	private Tween m_fadeTween;

	// Token: 0x040029C6 RID: 10694
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x040029C7 RID: 10695
	private Action m_disableCanvasAction;

	// Token: 0x040029C8 RID: 10696
	private bool m_displayText;
}
