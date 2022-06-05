using System;
using RL_Windows;
using TMPro;
using UnityEngine;

// Token: 0x02000518 RID: 1304
public class SpeechBubbleController : MonoBehaviour
{
	// Token: 0x170011D1 RID: 4561
	// (get) Token: 0x0600304E RID: 12366 RVA: 0x000A554D File Offset: 0x000A374D
	// (set) Token: 0x0600304F RID: 12367 RVA: 0x000A5555 File Offset: 0x000A3755
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

	// Token: 0x170011D2 RID: 4562
	// (get) Token: 0x06003050 RID: 12368 RVA: 0x000A555E File Offset: 0x000A375E
	// (set) Token: 0x06003051 RID: 12369 RVA: 0x000A5566 File Offset: 0x000A3766
	public SpeechBubbleType SpeechBubbleType { get; private set; }

	// Token: 0x06003052 RID: 12370 RVA: 0x000A5570 File Offset: 0x000A3770
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

	// Token: 0x06003053 RID: 12371 RVA: 0x000A5618 File Offset: 0x000A3818
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

	// Token: 0x06003054 RID: 12372 RVA: 0x000A5694 File Offset: 0x000A3894
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

	// Token: 0x06003055 RID: 12373 RVA: 0x000A5728 File Offset: 0x000A3928
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

	// Token: 0x06003056 RID: 12374 RVA: 0x000A575B File Offset: 0x000A395B
	private void OnDisable()
	{
		if (this.m_displayOffscreen)
		{
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Opened, this.m_onSkillTreeOpened);
			Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_onSkillTreeClosed);
		}
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x000A577F File Offset: 0x000A397F
	private void OnSkillTreeOpened(object sender, EventArgs args)
	{
		this.m_isInSkillTree = true;
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x000A5788 File Offset: 0x000A3988
	private void OnSkillTreeClosed(object sender, EventArgs args)
	{
		this.m_isInSkillTree = false;
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x000A5791 File Offset: 0x000A3991
	public void SetBGColor(Color color)
	{
		if (this.m_bg)
		{
			this.m_bg.color = color;
		}
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x000A57AC File Offset: 0x000A39AC
	public void SetOverrideText(string text)
	{
		this.m_overrideText = text;
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x000A57B8 File Offset: 0x000A39B8
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

	// Token: 0x0600305C RID: 12380 RVA: 0x000A58A4 File Offset: 0x000A3AA4
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

	// Token: 0x0400265F RID: 9823
	[SerializeField]
	private TMP_Text m_speechBubbleText;

	// Token: 0x04002660 RID: 9824
	[SerializeField]
	private SpriteRenderer m_bg;

	// Token: 0x04002661 RID: 9825
	[SerializeField]
	private GameObject m_arrowGO;

	// Token: 0x04002662 RID: 9826
	[SerializeField]
	private GameObject m_offscreenArrowGO;

	// Token: 0x04002663 RID: 9827
	[SerializeField]
	private bool m_displayOffscreen;

	// Token: 0x04002664 RID: 9828
	private Tween m_appearTween;

	// Token: 0x04002665 RID: 9829
	private Tween m_bounceTween;

	// Token: 0x04002666 RID: 9830
	private Tween m_scaleTween;

	// Token: 0x04002667 RID: 9831
	private Vector3 m_startingPos;

	// Token: 0x04002668 RID: 9832
	private Vector3 m_absStartingPos;

	// Token: 0x04002669 RID: 9833
	private Vector3 m_offscreenArrowStartingPos;

	// Token: 0x0400266A RID: 9834
	private int m_storedLayer;

	// Token: 0x0400266B RID: 9835
	private bool m_isInSkillTree;

	// Token: 0x0400266C RID: 9836
	private IDisplaySpeechBubble[] m_displaySpeechBubbleConditions;

	// Token: 0x0400266D RID: 9837
	private string m_overrideText;

	// Token: 0x0400266E RID: 9838
	private bool m_isFlipped;

	// Token: 0x0400266F RID: 9839
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeOpened;

	// Token: 0x04002670 RID: 9840
	private Action<MonoBehaviour, EventArgs> m_onSkillTreeClosed;
}
