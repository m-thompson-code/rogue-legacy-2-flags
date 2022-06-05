using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020008DB RID: 2267
public abstract class TextPopupObj : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001881 RID: 6273
	// (get) Token: 0x060044C0 RID: 17600 RVA: 0x00025CB0 File Offset: 0x00023EB0
	public float Alpha
	{
		get
		{
			return this.m_tmpText.alpha;
		}
	}

	// Token: 0x17001882 RID: 6274
	// (get) Token: 0x060044C1 RID: 17601 RVA: 0x00025CBD File Offset: 0x00023EBD
	// (set) Token: 0x060044C2 RID: 17602 RVA: 0x00025CC5 File Offset: 0x00023EC5
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001883 RID: 6275
	// (get) Token: 0x060044C3 RID: 17603 RVA: 0x00025CCE File Offset: 0x00023ECE
	// (set) Token: 0x060044C4 RID: 17604 RVA: 0x00025CD6 File Offset: 0x00023ED6
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001884 RID: 6276
	// (get) Token: 0x060044C5 RID: 17605 RVA: 0x00025CDF File Offset: 0x00023EDF
	// (set) Token: 0x060044C6 RID: 17606 RVA: 0x00025CE7 File Offset: 0x00023EE7
	public float InitialDelay { get; set; }

	// Token: 0x17001885 RID: 6277
	// (get) Token: 0x060044C7 RID: 17607 RVA: 0x00025CF0 File Offset: 0x00023EF0
	// (set) Token: 0x060044C8 RID: 17608 RVA: 0x00025CF8 File Offset: 0x00023EF8
	public GameObject Source { get; set; }

	// Token: 0x17001886 RID: 6278
	// (get) Token: 0x060044C9 RID: 17609 RVA: 0x00025D01 File Offset: 0x00023F01
	public TMP_Text TMPText
	{
		get
		{
			return this.m_tmpText;
		}
	}

	// Token: 0x17001887 RID: 6279
	// (get) Token: 0x060044CA RID: 17610 RVA: 0x00025D09 File Offset: 0x00023F09
	public string Text
	{
		get
		{
			return this.m_tmpText.text;
		}
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x00025D16 File Offset: 0x00023F16
	public void SetText(string text, TextAlignmentOptions alignmentOptions)
	{
		this.m_locItem.enabled = false;
		this.m_tmpText.text = text;
		this.m_tmpText.alignment = alignmentOptions;
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x00025D3C File Offset: 0x00023F3C
	public void SetLocIDText(string locID, StringGenderType genderType)
	{
		this.m_locItem.enabled = true;
		this.m_locItem.StringGenderType = genderType;
		this.m_locItem.SetString(locID);
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x0010F298 File Offset: 0x0010D498
	private void Awake()
	{
		this.m_tmpText = base.GetComponentInChildren<TMP_Text>();
		this.m_locItem = this.m_tmpText.GetComponent<LocalizationItem>();
		this.m_textGlyphConverter = this.m_tmpText.GetComponent<TextGlyphConverter>();
		this.m_storedLayer = base.gameObject.layer;
		this.ResetValues();
		this.IsAwakeCalled = true;
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x00025D62 File Offset: 0x00023F62
	public void Spawn()
	{
		base.StartCoroutine(this.SpawnCoroutine());
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x00025D71 File Offset: 0x00023F71
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

	// Token: 0x060044D0 RID: 17616
	protected abstract IEnumerator SpawnEffectCoroutine();

	// Token: 0x060044D1 RID: 17617 RVA: 0x0001BE85 File Offset: 0x0001A085
	protected virtual void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x0010F2F4 File Offset: 0x0010D4F4
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

	// Token: 0x060044D4 RID: 17620 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003548 RID: 13640
	[SerializeField]
	private bool m_disableTweenAnimations;

	// Token: 0x04003549 RID: 13641
	[SerializeField]
	[ConditionalHide("m_disableTweenAnimations", true)]
	protected float m_animationClipDuration = 1f;

	// Token: 0x0400354A RID: 13642
	[SerializeField]
	private bool m_useUnscaledTime;

	// Token: 0x0400354B RID: 13643
	protected TMP_Text m_tmpText;

	// Token: 0x0400354C RID: 13644
	protected LocalizationItem m_locItem;

	// Token: 0x0400354D RID: 13645
	protected TextGlyphConverter m_textGlyphConverter;

	// Token: 0x0400354E RID: 13646
	protected int m_storedLayer;
}
