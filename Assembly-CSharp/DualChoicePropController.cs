using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public abstract class DualChoicePropController : BaseSpecialPropController
{
	// Token: 0x17001700 RID: 5888
	// (get) Token: 0x06003F42 RID: 16194 RVA: 0x00022FC7 File Offset: 0x000211C7
	public GenericInfoTextBox LeftInfoTextBox
	{
		get
		{
			return this.m_leftInfoTextBox;
		}
	}

	// Token: 0x17001701 RID: 5889
	// (get) Token: 0x06003F43 RID: 16195 RVA: 0x00022FCF File Offset: 0x000211CF
	public SpriteRenderer LeftIcon
	{
		get
		{
			return this.m_leftIcon;
		}
	}

	// Token: 0x17001702 RID: 5890
	// (get) Token: 0x06003F44 RID: 16196 RVA: 0x00022FD7 File Offset: 0x000211D7
	public GenericInfoTextBox RightInfoTextBox
	{
		get
		{
			return this.m_rightInfoTextBox;
		}
	}

	// Token: 0x17001703 RID: 5891
	// (get) Token: 0x06003F45 RID: 16197 RVA: 0x00022FDF File Offset: 0x000211DF
	public SpriteRenderer RightIcon
	{
		get
		{
			return this.m_rightIcon;
		}
	}

	// Token: 0x06003F46 RID: 16198 RVA: 0x000FCFF8 File Offset: 0x000FB1F8
	protected override void Awake()
	{
		base.Awake();
		this.m_startingIconY = this.LeftIcon.transform.localPosition.y;
		this.m_startingInfoBoxY = this.m_leftBoxBGObj.transform.localPosition.y;
		if (this.LeftInfoTextBox)
		{
			this.LeftInfoTextBox.GetComponents<Interactable>(this.m_leftBoxInteractableList);
			this.LeftInfoTextBox.HideOnPause = true;
		}
		if (this.RightInfoTextBox)
		{
			this.RightInfoTextBox.GetComponents<Interactable>(this.m_rightBoxInteractableList);
			this.RightInfoTextBox.HideOnPause = true;
		}
	}

	// Token: 0x06003F47 RID: 16199 RVA: 0x000FD098 File Offset: 0x000FB298
	protected override void InitializePooledPropOnEnter()
	{
		this.LeftInfoTextBox.CanvasGroup.alpha = 0f;
		this.RightInfoTextBox.CanvasGroup.alpha = 0f;
		this.LeftInfoTextBox.gameObject.SetActive(true);
		this.RightInfoTextBox.gameObject.SetActive(true);
		this.m_leftIcon.gameObject.SetActive(true);
		this.m_rightIcon.gameObject.SetActive(true);
		foreach (Interactable interactable in this.m_leftBoxInteractableList)
		{
			interactable.SetIsInteractableActive(true);
		}
		foreach (Interactable interactable2 in this.m_rightBoxInteractableList)
		{
			interactable2.SetIsInteractableActive(true);
		}
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x000FD198 File Offset: 0x000FB398
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		Canvas[] componentsInChildren = base.GetComponentsInChildren<Canvas>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder = -1;
		}
	}

	// Token: 0x06003F49 RID: 16201 RVA: 0x000FD1CC File Offset: 0x000FB3CC
	public void DisplayLeftTextBox()
	{
		if (!base.IsDisabled)
		{
			if (this.m_textBoxLeftInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftInCoroutine);
			}
			if (this.m_textBoxLeftOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftOutCoroutine);
			}
			TweenManager.StopAllTweensContaining(this.LeftInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_leftBoxBGObj.transform, false);
			this.LeftHoverRelay.Dispatch(this.m_leftIcon.gameObject);
			this.m_textBoxLeftInCoroutine = base.StartCoroutine(this.TextBoxAnimInCoroutine(this.LeftInfoTextBox, this.m_leftBoxBGObj));
		}
	}

	// Token: 0x06003F4A RID: 16202 RVA: 0x000FD260 File Offset: 0x000FB460
	public void DisplayRightTextBox()
	{
		if (!base.IsDisabled)
		{
			if (this.m_textBoxRightInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightInCoroutine);
			}
			if (this.m_textBoxRightOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightOutCoroutine);
			}
			TweenManager.StopAllTweensContaining(this.RightInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_rightBoxBGObj.transform, false);
			this.RightHoverRelay.Dispatch(this.m_rightIcon.gameObject);
			this.m_textBoxRightInCoroutine = base.StartCoroutine(this.TextBoxAnimInCoroutine(this.RightInfoTextBox, this.m_rightBoxBGObj));
		}
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x000FD2F4 File Offset: 0x000FB4F4
	public void HideLeftTextBox()
	{
		if (!base.IsDisabled)
		{
			if (this.m_textBoxLeftInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftInCoroutine);
			}
			if (this.m_textBoxLeftOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftOutCoroutine);
			}
			TweenManager.StopAllTweensContaining(this.LeftInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_leftBoxBGObj.transform, false);
			this.m_textBoxLeftOutCoroutine = base.StartCoroutine(this.TextBoxAnimOutCoroutine(this.LeftInfoTextBox, this.m_leftBoxBGObj));
		}
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x000FD374 File Offset: 0x000FB574
	public void HideRightTextBox()
	{
		if (!base.IsDisabled)
		{
			if (this.m_textBoxRightInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightInCoroutine);
			}
			if (this.m_textBoxRightOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightOutCoroutine);
			}
			TweenManager.StopAllTweensContaining(this.RightInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_rightBoxBGObj.transform, false);
			this.m_textBoxRightOutCoroutine = base.StartCoroutine(this.TextBoxAnimOutCoroutine(this.RightInfoTextBox, this.m_rightBoxBGObj));
		}
	}

	// Token: 0x06003F4D RID: 16205 RVA: 0x00022FE7 File Offset: 0x000211E7
	private IEnumerator TextBoxAnimInCoroutine(GenericInfoTextBox textBox, GameObject textBoxNoHB)
	{
		textBox.CanvasGroup.alpha = 0f;
		TweenManager.TweenTo(textBox.CanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			1
		});
		Vector3 localPosition = textBoxNoHB.transform.localPosition;
		localPosition.y = this.m_startingInfoBoxY - 0.5f;
		textBoxNoHB.transform.localPosition = localPosition;
		yield return TweenManager.TweenBy(textBoxNoHB.transform, 0.15f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06003F4E RID: 16206 RVA: 0x00023004 File Offset: 0x00021204
	private IEnumerator TextBoxAnimOutCoroutine(GenericInfoTextBox textBox, GameObject textBoxNoHB)
	{
		textBox.CanvasGroup.alpha = 1f;
		TweenManager.TweenTo(textBox.CanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenBy(textBoxNoHB.transform, 0.15f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		yield break;
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x000FD3F4 File Offset: 0x000FB5F4
	protected override void OnDisable()
	{
		base.OnDisable();
		if (!GameManager.IsApplicationClosing && TweenManager.IsInitialized)
		{
			TweenManager.StopAllTweensContaining(this.LeftInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_leftBoxBGObj.transform, false);
			TweenManager.StopAllTweensContaining(this.RightInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_rightBoxBGObj.transform, false);
		}
	}

	// Token: 0x06003F50 RID: 16208 RVA: 0x0002301A File Offset: 0x0002121A
	private IEnumerator DisableTextboxAnimOutCoroutine()
	{
		this.m_leftIcon.gameObject.SetActive(false);
		this.m_rightIcon.gameObject.SetActive(false);
		TweenManager.TweenTo(this.LeftInfoTextBox.CanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		TweenManager.TweenBy(this.m_leftBoxBGObj.transform, 0.15f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			0.5f
		});
		TweenManager.TweenTo(this.RightInfoTextBox.CanvasGroup, 0.15f, new EaseDelegate(Ease.None), new object[]
		{
			"alpha",
			0
		});
		yield return TweenManager.TweenBy(this.m_rightBoxBGObj.transform, 0.15f, new EaseDelegate(Ease.Quad.EaseIn), new object[]
		{
			"localPosition.y",
			0.5f
		}).TweenCoroutine;
		this.LeftInfoTextBox.gameObject.SetActive(false);
		this.RightInfoTextBox.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06003F51 RID: 16209 RVA: 0x000FD45C File Offset: 0x000FB65C
	protected override void DisableProp(bool firstTimeDisabled)
	{
		foreach (Interactable interactable in this.m_leftBoxInteractableList)
		{
			interactable.SetIsInteractableActive(false);
		}
		foreach (Interactable interactable2 in this.m_rightBoxInteractableList)
		{
			interactable2.SetIsInteractableActive(false);
		}
		if (firstTimeDisabled && !base.IsDisabled)
		{
			if (this.m_textBoxLeftInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftInCoroutine);
			}
			if (this.m_textBoxLeftOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxLeftOutCoroutine);
			}
			if (this.m_textBoxRightInCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightInCoroutine);
			}
			if (this.m_textBoxRightOutCoroutine != null)
			{
				base.StopCoroutine(this.m_textBoxRightOutCoroutine);
			}
			TweenManager.StopAllTweensContaining(this.RightInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_rightBoxBGObj.transform, false);
			TweenManager.StopAllTweensContaining(this.LeftInfoTextBox.CanvasGroup, false);
			TweenManager.StopAllTweensContaining(this.m_leftBoxBGObj.transform, false);
			base.StartCoroutine(this.DisableTextboxAnimOutCoroutine());
		}
		else
		{
			this.m_leftIcon.gameObject.SetActive(false);
			this.m_rightIcon.gameObject.SetActive(false);
			this.LeftInfoTextBox.gameObject.SetActive(false);
			this.RightInfoTextBox.gameObject.SetActive(false);
		}
		base.DisableProp(firstTimeDisabled);
	}

	// Token: 0x06003F52 RID: 16210 RVA: 0x000FD5EC File Offset: 0x000FB7EC
	protected virtual void Update()
	{
		bool flag = this.m_leftIcon;
		bool flag2 = this.m_rightIcon;
		if (flag || flag2)
		{
			float y = this.m_startingIconY + Mathf.Sin(Time.timeSinceLevelLoad * 2f) / 8f;
			if (flag)
			{
				Vector3 localPosition = this.m_leftIcon.transform.localPosition;
				localPosition.y = y;
				this.m_leftIcon.gameObject.transform.localPosition = localPosition;
			}
			if (flag2)
			{
				Vector3 localPosition2 = this.m_rightIcon.transform.localPosition;
				localPosition2.y = y;
				this.m_rightIcon.gameObject.transform.localPosition = localPosition2;
			}
		}
	}

	// Token: 0x04003177 RID: 12663
	[SerializeField]
	private GenericInfoTextBox m_leftInfoTextBox;

	// Token: 0x04003178 RID: 12664
	[SerializeField]
	private GameObject m_leftBoxBGObj;

	// Token: 0x04003179 RID: 12665
	[SerializeField]
	private SpriteRenderer m_leftIcon;

	// Token: 0x0400317A RID: 12666
	[SerializeField]
	private GenericInfoTextBox m_rightInfoTextBox;

	// Token: 0x0400317B RID: 12667
	[SerializeField]
	private GameObject m_rightBoxBGObj;

	// Token: 0x0400317C RID: 12668
	[SerializeField]
	private SpriteRenderer m_rightIcon;

	// Token: 0x0400317D RID: 12669
	public Relay<GameObject> LeftHoverRelay = new Relay<GameObject>();

	// Token: 0x0400317E RID: 12670
	public Relay<GameObject> RightHoverRelay = new Relay<GameObject>();

	// Token: 0x0400317F RID: 12671
	protected float m_startingIconY;

	// Token: 0x04003180 RID: 12672
	private float m_startingInfoBoxY;

	// Token: 0x04003181 RID: 12673
	private Coroutine m_textBoxLeftInCoroutine;

	// Token: 0x04003182 RID: 12674
	private Coroutine m_textBoxLeftOutCoroutine;

	// Token: 0x04003183 RID: 12675
	private Coroutine m_textBoxRightInCoroutine;

	// Token: 0x04003184 RID: 12676
	private Coroutine m_textBoxRightOutCoroutine;

	// Token: 0x04003185 RID: 12677
	private float m_storedLeftTextBoxAlpha;

	// Token: 0x04003186 RID: 12678
	private float m_storedRightTextBoxAlpha;

	// Token: 0x04003187 RID: 12679
	private List<Interactable> m_leftBoxInteractableList = new List<Interactable>(2);

	// Token: 0x04003188 RID: 12680
	private List<Interactable> m_rightBoxInteractableList = new List<Interactable>(2);
}
