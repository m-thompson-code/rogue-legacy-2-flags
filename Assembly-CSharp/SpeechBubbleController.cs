using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public class SpeechBubbleController : MonoBehaviour
{
	// Token: 0x170017E6 RID: 6118
	// (get) Token: 0x0600430A RID: 17162 RVA: 0x00025161 File Offset: 0x00023361
	// (set) Token: 0x0600430B RID: 17163 RVA: 0x00025169 File Offset: 0x00023369
	public bool DisplayOffscreen
	{
		get
		{
			return this.m_displayOffscreen;
		}
		set
		{
			this.m_displayOffscreen = value;
		}
	}

	// Token: 0x170017E7 RID: 6119
	// (get) Token: 0x0600430C RID: 17164 RVA: 0x00025172 File Offset: 0x00023372
	// (set) Token: 0x0600430D RID: 17165 RVA: 0x0002517A File Offset: 0x0002337A
	public SpeechBubbleType SpeechBubbleType { get; private set; }

	// Token: 0x0600430E RID: 17166 RVA: 0x0010C820 File Offset: 0x0010AA20
	public void SetSpeechBubbleType(SpeechBubbleType bubbleType)
	{
		this.SpeechBubbleType = bubbleType;
		switch (bubbleType)
		{
		case SpeechBubbleType.Sleeping:
			this.m_speechBubbleText.text = "Zzz";
			return;
		case SpeechBubbleType.GearAvailable:
			this.m_speechBubbleText.text = LocalizationManager.GetString("LOC_ID_GENERAL_UI_NEW_1", false, false).ToUpper();
			return;
		case SpeechBubbleType.Dialogue:
			this.m_speechBubbleText.text = "...";
			return;
		case SpeechBubbleType.TeleportToManor_PizzaGirlOnly:
			this.m_speechBubbleText.text = "!!";
			return;
		case SpeechBubbleType.OverrideText:
			this.m_speechBubbleText.text = this.m_overrideText;
			return;
		}
		this.m_speechBubbleText.text = "!";
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x0010C8C8 File Offset: 0x0010AAC8
	private void Awake()
	{
		this.m_startingPos = base.transform.localPosition;
		this.m_startingPos.z = -1f;
		this.m_displaySpeechBubbleConditions = base.gameObject.GetRoot(false).GetComponentsInChildren<IDisplaySpeechBubble>();
		this.SetSpeechBubbleEnabled(false);
		this.m_offscreenArrowGO.SetActive(false);
		this.m_onSkillTreeOpened = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeOpened);
		this.m_onSkillTreeClosed = new Action<MonoBehaviour, EventArgs>(this.OnSkillTreeClosed);
	}

	// Token: 0x06004310 RID: 17168 RVA: 0x0010C944 File Offset: 0x0010AB44
	private void Start()
	{
		this.m_offscreenArrowStartingPos = this.m_offscreenArrowGO.transform.localPosition;
		this.m_absStartingPos = base.transform.position;
		this.m_absStartingPos.z = -1f;
		this.m_storedLayer = base.gameObject.layer;
		if (base.transform.lossyScale.x < 0f)
		{
			base.transform.SetLocalScaleX(base.transform.localScale.x * -1f);
			this.m_isFlipped = true;
		}
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x00025183 File Offset: 0x00023383
	private void OnEnable()
	{
		if (this.m_displayOffscreen)
		{
			if (WindowManager.GetIsWindowOpen(WindowID.SkillTree))
			{
				this.m_isInSkillTree = true;
			}
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
			Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
		}
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x000251B6 File Offset: 0x000233B6
	private void OnDisable()
	{
		if (this.m_displayOffscreen)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
		}
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x000251DA File Offset: 0x000233DA
	private void OnSkillTreeOpened(object sender, EventArgs args)
	{
		this.m_isInSkillTree = true;
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x000251E3 File Offset: 0x000233E3
	private void OnSkillTreeClosed(object sender, EventArgs args)
	{
		this.m_isInSkillTree = false;
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x000251EC File Offset: 0x000233EC
	public void SetBGColor(Color color)
	{
		if (this.m_bg)
		{
			this.m_bg.color = color;
		}
	}

	// Token: 0x06004316 RID: 17174 RVA: 0x00025207 File Offset: 0x00023407
	public void SetOverrideText(string text)
	{
		this.m_overrideText = text;
	}

	// Token: 0x06004317 RID: 17175 RVA: 0x0010C9D8 File Offset: 0x0010ABD8
	public void SetSpeechBubbleEnabled(bool enable)
	{
		if (GameManager.IsApplicationClosing)
		{
			return;
		}
		if (enable)
		{
			if (!this.m_displaySpeechBubbleConditions.IsNativeNull() && this.m_displaySpeechBubbleConditions.Length != 0)
			{
				bool flag = false;
				foreach (IDisplaySpeechBubble displaySpeechBubble in this.m_displaySpeechBubbleConditions)
				{
					if (displaySpeechBubble.ShouldDisplaySpeechBubble)
					{
						flag = true;
						SpeechBubbleType bubbleType = displaySpeechBubble.BubbleType;
						if (bubbleType > this.SpeechBubbleType)
						{
							this.SetSpeechBubbleType(bubbleType);
						}
					}
				}
				if (!flag)
				{
					if (base.gameObject.activeSelf)
					{
						base.gameObject.SetActive(false);
					}
					this.SetSpeechBubbleType(SpeechBubbleType.None);
					return;
				}
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
					return;
				}
			}
			else if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
				return;
			}
		}
		else
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			this.SetSpeechBubbleType(SpeechBubbleType.None);
		}
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x0010CAC4 File Offset: 0x0010ACC4
	private void LateUpdate()
	{
		if (this.m_displayOffscreen)
		{
			float num = 1f;
			float num2 = 0.5f;
			float num3 = 1.2f;
			float orthographicSize = CameraController.GameCamera.orthographicSize;
			float num4 = orthographicSize * (float)Screen.width / (float)Screen.height;
			float num5 = CameraController.GameCamera.transform.localPosition.x - num4;
			float num6 = CameraController.GameCamera.transform.localPosition.y - orthographicSize;
			Rect rect = new Rect(num5 + num, num6 + num2, (num4 - num) * 2f, (orthographicSize - num3) * 2f);
			if (!rect.Contains(this.m_absStartingPos) && !this.m_isInSkillTree)
			{
				Vector3 absStartingPos = this.m_absStartingPos;
				Vector3 vector = absStartingPos;
				float xMin = rect.xMin;
				float xMax = rect.xMax;
				float yMin = rect.yMin;
				float yMax = rect.yMax;
				if (absStartingPos.x < xMin)
				{
					vector.x = xMin;
				}
				else if (absStartingPos.x > xMax)
				{
					vector.x = xMax;
				}
				if (absStartingPos.y < yMin)
				{
					vector.y = yMin;
				}
				else if (absStartingPos.y > yMax)
				{
					vector.y = yMax;
				}
				base.transform.position = vector;
				if (this.m_arrowGO.activeSelf)
				{
					this.m_arrowGO.SetActive(false);
				}
				if (!this.m_offscreenArrowGO.activeSelf)
				{
					this.m_offscreenArrowGO.SetActive(true);
				}
				float zRot = CDGHelper.AngleBetweenPts(vector, this.m_absStartingPos);
				this.m_offscreenArrowGO.transform.SetLocalEulerZ(zRot);
				float num7 = 0.4f;
				float num8 = vector.x - absStartingPos.x;
				float num9 = Mathf.Clamp(num8, -num7, num7);
				this.m_offscreenArrowGO.transform.SetLocalPositionX(this.m_offscreenArrowStartingPos.x - num9);
				float num10 = 0.45f;
				float num11 = vector.y - absStartingPos.y;
				float num12 = Mathf.Clamp(num11, -num10, num10);
				this.m_offscreenArrowGO.transform.SetLocalPositionY(this.m_offscreenArrowStartingPos.y - num12);
				if (((num8 > -num7 && num8 < num7 && num11 == 0f) || (num11 > -num10 && num11 < num10 && num8 == 0f)) && this.m_offscreenArrowGO.activeSelf)
				{
					this.m_offscreenArrowGO.SetActive(false);
				}
				if (base.gameObject.layer != 23)
				{
					base.gameObject.SetLayerRecursively(23, false);
					return;
				}
			}
			else
			{
				base.transform.position = this.m_absStartingPos;
				if (!this.m_arrowGO.activeSelf)
				{
					this.m_arrowGO.SetActive(true);
				}
				if (this.m_offscreenArrowGO.activeSelf)
				{
					this.m_offscreenArrowGO.SetActive(false);
				}
				if (base.gameObject.layer != this.m_storedLayer)
				{
					base.gameObject.SetLayerRecursively(this.m_storedLayer, false);
				}
			}
		}
	}

	// Token: 0x04003447 RID: 13383
	[SerializeField]
	private TMP_Text m_speechBubbleText;

	// Token: 0x04003448 RID: 13384
	[SerializeField]
	private SpriteRenderer m_bg;

	// Token: 0x04003449 RID: 13385
	[SerializeField]
	private GameObject m_arrowGO;

	// Token: 0x0400344A RID: 13386
	[SerializeField]
	private GameObject m_offscreenArrowGO;

	// Token: 0x0400344B RID: 13387
	[SerializeField]
	private bool m_displayOffscreen;

	// Token: 0x0400344C RID: 13388
	private Tween m_appearTween;

	// Token: 0x0400344D RID: 13389
	private Tween m_bounceTween;

	// Token: 0x0400344E RID: 13390
	private Tween m_scaleTween;

	// Token: 0x0400344F RID: 13391
	private Vector3 m_startingPos;

	// Token: 0x04003450 RID: 13392
	private Vector3 m_absStartingPos;

	// Token: 0x04003451 RID: 13393
	private Vector3 m_offscreenArrowStartingPos;

	// Token: 0x04003452 RID: 13394
	private int m_storedLayer;

	// Token: 0x04003453 RID: 13395
	private bool m_isInSkillTree;

	// Token: 0x04003454 RID: 13396
	private IDisplaySpeechBubble[] m_displaySpeechBubbleConditions;

	// Token: 0x04003455 RID: 13397
	private string m_overrideText;

	// Token: 0x04003456 RID: 13398
	private bool m_isFlipped;

	// Token: 0x04003457 RID: 13399
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x04003458 RID: 13400
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;
}
