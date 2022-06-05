using System;
using System.Collections;
using System.Collections.Generic;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public abstract class DualChoicePropController : BaseSpecialPropController
{
	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x06002DFA RID: 11770 RVA: 0x0009AEAF File Offset: 0x000990AF
	public GenericInfoTextBox LeftInfoTextBox
	{
		get
		{
			return this.m_leftInfoTextBox;
		}
	}

	// Token: 0x17001166 RID: 4454
	// (get) Token: 0x06002DFB RID: 11771 RVA: 0x0009AEB7 File Offset: 0x000990B7
	public SpriteRenderer LeftIcon
	{
		get
		{
			return this.m_leftIcon;
		}
	}

	// Token: 0x17001167 RID: 4455
	// (get) Token: 0x06002DFC RID: 11772 RVA: 0x0009AEBF File Offset: 0x000990BF
	public GenericInfoTextBox RightInfoTextBox
	{
		get
		{
			return this.m_rightInfoTextBox;
		}
	}

	// Token: 0x17001168 RID: 4456
	// (get) Token: 0x06002DFD RID: 11773 RVA: 0x0009AEC7 File Offset: 0x000990C7
	public SpriteRenderer RightIcon
	{
		get
		{
			return this.m_rightIcon;
		}
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x0009AED0 File Offset: 0x000990D0
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

	// Token: 0x06002DFF RID: 11775 RVA: 0x0009AF70 File Offset: 0x00099170
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

	// Token: 0x06002E00 RID: 11776 RVA: 0x0009B070 File Offset: 0x00099270
	public override void SetRoom(BaseRoom room)
	{
		base.SetRoom(room);
		Canvas[] componentsInChildren = base.GetComponentsInChildren<Canvas>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sortingOrder = -1;
		}
	}

	// Token: 0x06002E01 RID: 11777 RVA: 0x0009B0A4 File Offset: 0x000992A4
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

	// Token: 0x06002E02 RID: 11778 RVA: 0x0009B138 File Offset: 0x00099338
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

	// Token: 0x06002E03 RID: 11779 RVA: 0x0009B1CC File Offset: 0x000993CC
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

	// Token: 0x06002E04 RID: 11780 RVA: 0x0009B24C File Offset: 0x0009944C
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

	// Token: 0x06002E05 RID: 11781 RVA: 0x0009B2C9 File Offset: 0x000994C9
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

	// Token: 0x06002E06 RID: 11782 RVA: 0x0009B2E6 File Offset: 0x000994E6
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

	// Token: 0x06002E07 RID: 11783 RVA: 0x0009B2FC File Offset: 0x000994FC
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

	// Token: 0x06002E08 RID: 11784 RVA: 0x0009B361 File Offset: 0x00099561
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

	// Token: 0x06002E09 RID: 11785 RVA: 0x0009B370 File Offset: 0x00099570
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

	// Token: 0x06002E0A RID: 11786 RVA: 0x0009B500 File Offset: 0x00099700
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

	// Token: 0x040024B5 RID: 9397
	[SerializeField]
	private GenericInfoTextBox m_leftInfoTextBox;

	// Token: 0x040024B6 RID: 9398
	[SerializeField]
	private GameObject m_leftBoxBGObj;

	// Token: 0x040024B7 RID: 9399
	[SerializeField]
	private SpriteRenderer m_leftIcon;

	// Token: 0x040024B8 RID: 9400
	[SerializeField]
	private GenericInfoTextBox m_rightInfoTextBox;

	// Token: 0x040024B9 RID: 9401
	[SerializeField]
	private GameObject m_rightBoxBGObj;

	// Token: 0x040024BA RID: 9402
	[SerializeField]
	private SpriteRenderer m_rightIcon;

	// Token: 0x040024BB RID: 9403
	public Relay<GameObject> LeftHoverRelay = new Relay<GameObject>();

	// Token: 0x040024BC RID: 9404
	public Relay<GameObject> RightHoverRelay = new Relay<GameObject>();

	// Token: 0x040024BD RID: 9405
	protected float m_startingIconY;

	// Token: 0x040024BE RID: 9406
	private float m_startingInfoBoxY;

	// Token: 0x040024BF RID: 9407
	private Coroutine m_textBoxLeftInCoroutine;

	// Token: 0x040024C0 RID: 9408
	private Coroutine m_textBoxLeftOutCoroutine;

	// Token: 0x040024C1 RID: 9409
	private Coroutine m_textBoxRightInCoroutine;

	// Token: 0x040024C2 RID: 9410
	private Coroutine m_textBoxRightOutCoroutine;

	// Token: 0x040024C3 RID: 9411
	private float m_storedLeftTextBoxAlpha;

	// Token: 0x040024C4 RID: 9412
	private float m_storedRightTextBoxAlpha;

	// Token: 0x040024C5 RID: 9413
	private List<Interactable> m_leftBoxInteractableList = new List<Interactable>(2);

	// Token: 0x040024C6 RID: 9414
	private List<Interactable> m_rightBoxInteractableList = new List<Interactable>(2);
}
