using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public abstract class TextPopupObj : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001224 RID: 4644
	// (get) Token: 0x06003124 RID: 12580 RVA: 0x000A698E File Offset: 0x000A4B8E
	public float Alpha
	{
		get
		{
			return this.m_tmpText.alpha;
		}
	}

	// Token: 0x17001225 RID: 4645
	// (get) Token: 0x06003125 RID: 12581 RVA: 0x000A699B File Offset: 0x000A4B9B
	// (set) Token: 0x06003126 RID: 12582 RVA: 0x000A69A3 File Offset: 0x000A4BA3
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001226 RID: 4646
	// (get) Token: 0x06003127 RID: 12583 RVA: 0x000A69AC File Offset: 0x000A4BAC
	// (set) Token: 0x06003128 RID: 12584 RVA: 0x000A69B4 File Offset: 0x000A4BB4
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001227 RID: 4647
	// (get) Token: 0x06003129 RID: 12585 RVA: 0x000A69BD File Offset: 0x000A4BBD
	// (set) Token: 0x0600312A RID: 12586 RVA: 0x000A69C5 File Offset: 0x000A4BC5
	public float InitialDelay { get; set; }

	// Token: 0x17001228 RID: 4648
	// (get) Token: 0x0600312B RID: 12587 RVA: 0x000A69CE File Offset: 0x000A4BCE
	// (set) Token: 0x0600312C RID: 12588 RVA: 0x000A69D6 File Offset: 0x000A4BD6
	public GameObject Source { get; set; }

	// Token: 0x17001229 RID: 4649
	// (get) Token: 0x0600312D RID: 12589 RVA: 0x000A69DF File Offset: 0x000A4BDF
	public TMP_Text TMPText
	{
		get
		{
			return this.m_tmpText;
		}
	}

	// Token: 0x1700122A RID: 4650
	// (get) Token: 0x0600312E RID: 12590 RVA: 0x000A69E7 File Offset: 0x000A4BE7
	public string Text
	{
		get
		{
			return this.m_tmpText.text;
		}
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x000A69F4 File Offset: 0x000A4BF4
	public void SetText(string text, TextAlignmentOptions alignmentOptions)
	{
		this.m_locItem.enabled = false;
		this.m_tmpText.text = text;
		this.m_tmpText.alignment = alignmentOptions;
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x000A6A1A File Offset: 0x000A4C1A
	public void SetLocIDText(string locID, StringGenderType genderType)
	{
		this.m_locItem.enabled = true;
		this.m_locItem.StringGenderType = genderType;
		this.m_locItem.SetString(locID);
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x000A6A40 File Offset: 0x000A4C40
	private void Awake()
	{
		this.m_tmpText = base.GetComponentInChildren<TMP_Text>();
		this.m_locItem = this.m_tmpText.GetComponent<LocalizationItem>();
		this.m_textGlyphConverter = this.m_tmpText.GetComponent<TextGlyphConverter>();
		this.m_storedLayer = base.gameObject.layer;
		this.ResetValues();
		this.IsAwakeCalled = true;
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x000A6A99 File Offset: 0x000A4C99
	public void Spawn()
	{
		base.StartCoroutine(this.SpawnCoroutine());
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x000A6AA8 File Offset: 0x000A4CA8
	private IEnumerator SpawnCoroutine()
	{
		if (this.InitialDelay > 0f)
		{
			Vector2 posOffset = Vector2.zero;
			if (this.Source)
			{
				posOffset = base.transform.localPosition - this.Source.transform.position;
			}
			this.m_tmpText.alpha = 0f;
			if (!this.m_useUnscaledTime)
			{
				float startTime = Time.time;
				while (Time.time < startTime + this.InitialDelay)
				{
					yield return null;
				}
			}
			else
			{
				float startTime = Time.unscaledTime;
				while (Time.unscaledTime < startTime + this.InitialDelay)
				{
					yield return null;
				}
			}
			this.m_tmpText.alpha = 1f;
			if (this.Source && base.transform.parent != this.Source.transform)
			{
				posOffset += this.Source.transform.position;
				base.transform.localPosition = new Vector3(posOffset.x, posOffset.y, base.transform.localPosition.z);
			}
			posOffset = default(Vector2);
		}
		if (!this.m_disableTweenAnimations)
		{
			yield return this.SpawnEffectCoroutine();
		}
		else if (this.m_animationClipDuration >= 0f)
		{
			if (!this.m_useUnscaledTime)
			{
				float startTime = this.m_animationClipDuration + Time.time;
				while (Time.time < startTime)
				{
					yield return null;
				}
			}
			else
			{
				float startTime = this.m_animationClipDuration + Time.unscaledTime;
				while (Time.unscaledTime < startTime)
				{
					yield return null;
				}
			}
			base.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06003134 RID: 12596
	protected abstract IEnumerator SpawnEffectCoroutine();

	// Token: 0x06003135 RID: 12597 RVA: 0x000A6AB7 File Offset: 0x000A4CB7
	protected virtual void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000A6AC0 File Offset: 0x000A4CC0
	public virtual void ResetValues()
	{
		base.StopAllCoroutines();
		base.gameObject.transform.localPosition = Vector3.zero;
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		base.gameObject.transform.localEulerAngles = Vector3.zero;
		this.m_tmpText.alpha = 1f;
		this.m_locItem.enabled = true;
		if (base.gameObject.layer != this.m_storedLayer)
		{
			base.gameObject.SetLayerRecursively(this.m_storedLayer, false);
		}
		this.InitialDelay = 0f;
		this.Source = null;
	}

	// Token: 0x06003138 RID: 12600 RVA: 0x000A6B8C File Offset: 0x000A4D8C
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x040026D1 RID: 9937
	[SerializeField]
	private bool m_disableTweenAnimations;

	// Token: 0x040026D2 RID: 9938
	[SerializeField]
	[ConditionalHide("m_disableTweenAnimations", true)]
	protected float m_animationClipDuration = 1f;

	// Token: 0x040026D3 RID: 9939
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x040026D4 RID: 9940
	protected TMP_Text m_tmpText;

	// Token: 0x040026D5 RID: 9941
	protected LocalizationItem m_locItem;

	// Token: 0x040026D6 RID: 9942
	protected TextGlyphConverter m_textGlyphConverter;

	// Token: 0x040026D7 RID: 9943
	protected int m_storedLayer;
}
