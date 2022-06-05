using System;
using System.Collections;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003E9 RID: 1001
public class TutorialTextBoxController : MonoBehaviour
{
	// Token: 0x17000EE4 RID: 3812
	// (get) Token: 0x060024E1 RID: 9441 RVA: 0x0007A8A5 File Offset: 0x00078AA5
	// (set) Token: 0x060024E2 RID: 9442 RVA: 0x0007A8AD File Offset: 0x00078AAD
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

	// Token: 0x17000EE5 RID: 3813
	// (get) Token: 0x060024E3 RID: 9443 RVA: 0x0007A8B6 File Offset: 0x00078AB6
	// (set) Token: 0x060024E4 RID: 9444 RVA: 0x0007A8BE File Offset: 0x00078ABE
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

	// Token: 0x17000EE6 RID: 3814
	// (get) Token: 0x060024E5 RID: 9445 RVA: 0x0007A8C7 File Offset: 0x00078AC7
	// (set) Token: 0x060024E6 RID: 9446 RVA: 0x0007A8CF File Offset: 0x00078ACF
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

	// Token: 0x060024E7 RID: 9447 RVA: 0x0007A8D8 File Offset: 0x00078AD8
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

	// Token: 0x060024E8 RID: 9448 RVA: 0x0007A943 File Offset: 0x00078B43
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

	// Token: 0x060024E9 RID: 9449 RVA: 0x0007A954 File Offset: 0x00078B54
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

	// Token: 0x060024EA RID: 9450 RVA: 0x0007A9F3 File Offset: 0x00078BF3
	private void OnDisable()
	{
		if (this.m_stickToCamera)
		{
			Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_onPlayerExitRoom);
		}
		this.m_displayText = false;
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x0007AA11 File Offset: 0x00078C11
	private void OnPlayerExitRoom(object sender, EventArgs args)
	{
		this.HideText();
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x0007AA19 File Offset: 0x00078C19
	private void RemoveFromCamera(Scene arg0, Scene arg1)
	{
		SceneManager.activeSceneChanged -= this.RemoveFromCamera;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x0007AA38 File Offset: 0x00078C38
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

	// Token: 0x060024EE RID: 9454 RVA: 0x0007AAE9 File Offset: 0x00078CE9
	private void DisableCanvas()
	{
		this.m_canvasGroup.gameObject.SetActive(false);
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x0007AAFC File Offset: 0x00078CFC
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

	// Token: 0x060024F0 RID: 9456 RVA: 0x0007ABC0 File Offset: 0x00078DC0
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

	// Token: 0x060024F1 RID: 9457 RVA: 0x0007AC04 File Offset: 0x00078E04
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

	// Token: 0x04001F3D RID: 7997
	private const float FADE_DURATION = 0.25f;

	// Token: 0x04001F3E RID: 7998
	private const float BACKGROUND_OPACITY = 0.425f;

	// Token: 0x04001F3F RID: 7999
	private Vector3 TUTORIAL_TEXT_POSITION = new Vector3(0f, 6f, 1f);

	// Token: 0x04001F40 RID: 8000
	[SerializeField]
	private CanvasGroup m_canvasGroup;

	// Token: 0x04001F41 RID: 8001
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04001F42 RID: 8002
	[SerializeField]
	private bool m_stickToCamera;

	// Token: 0x04001F43 RID: 8003
	[SerializeField]
	private ConditionFlag m_mustHaveConditions;

	// Token: 0x04001F44 RID: 8004
	[SerializeField]
	private ConditionFlag m_mustNotHaveConditions;

	// Token: 0x04001F45 RID: 8005
	[SerializeField]
	private StudioEventEmitter m_showEventEmitter;

	// Token: 0x04001F46 RID: 8006
	[SerializeField]
	private StudioEventEmitter m_hideEventEmitter;

	// Token: 0x04001F47 RID: 8007
	private Tween m_fadeTween;

	// Token: 0x04001F48 RID: 8008
	private Action<MonoBehaviour, EventArgs> m_onPlayerExitRoom;

	// Token: 0x04001F49 RID: 8009
	private Action m_disableCanvasAction;

	// Token: 0x04001F4A RID: 8010
	private bool m_displayText;
}
