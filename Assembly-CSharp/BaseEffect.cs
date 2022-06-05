using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x020006BD RID: 1725
public abstract class BaseEffect : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x0600351D RID: 13597 RVA: 0x0001D1E2 File Offset: 0x0001B3E2
	public IRelayLink<BaseEffect> OnPlayRelay
	{
		get
		{
			return this.m_onPlayRelay.link;
		}
	}

	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x0600351E RID: 13598 RVA: 0x0001D1EF File Offset: 0x0001B3EF
	// (set) Token: 0x0600351F RID: 13599 RVA: 0x0001D1F7 File Offset: 0x0001B3F7
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001426 RID: 5158
	// (get) Token: 0x06003520 RID: 13600 RVA: 0x0001D200 File Offset: 0x0001B400
	// (set) Token: 0x06003521 RID: 13601 RVA: 0x0001D208 File Offset: 0x0001B408
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17001427 RID: 5159
	// (get) Token: 0x06003522 RID: 13602 RVA: 0x0001D211 File Offset: 0x0001B411
	// (set) Token: 0x06003523 RID: 13603 RVA: 0x0001D219 File Offset: 0x0001B419
	public int AnimatorLayer { get; set; }

	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x06003524 RID: 13604 RVA: 0x0001D222 File Offset: 0x0001B422
	// (set) Token: 0x06003525 RID: 13605 RVA: 0x0001D22A File Offset: 0x0001B42A
	public bool DisableDestroyOnRoomChange { get; set; }

	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x06003526 RID: 13606 RVA: 0x0001D233 File Offset: 0x0001B433
	// (set) Token: 0x06003527 RID: 13607 RVA: 0x0001D23B File Offset: 0x0001B43B
	public bool IsPlaying { get; protected set; }

	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x06003528 RID: 13608 RVA: 0x0001D244 File Offset: 0x0001B444
	// (set) Token: 0x06003529 RID: 13609 RVA: 0x0001D24C File Offset: 0x0001B44C
	public EffectTriggerDirection EffectDirection { get; set; }

	// Token: 0x1700142B RID: 5163
	// (get) Token: 0x0600352A RID: 13610 RVA: 0x0001D255 File Offset: 0x0001B455
	public bool IsFlipped
	{
		get
		{
			return base.gameObject.transform.localScale.x < 0f;
		}
	}

	// Token: 0x1700142C RID: 5164
	// (get) Token: 0x0600352B RID: 13611 RVA: 0x0001D273 File Offset: 0x0001B473
	// (set) Token: 0x0600352C RID: 13612 RVA: 0x0001D27B File Offset: 0x0001B47B
	public BaseEffect EffectPrefab { get; set; }

	// Token: 0x1700142D RID: 5165
	// (get) Token: 0x0600352D RID: 13613 RVA: 0x0001D284 File Offset: 0x0001B484
	// (set) Token: 0x0600352E RID: 13614 RVA: 0x0001D28C File Offset: 0x0001B48C
	public GameObject Source { get; set; }

	// Token: 0x1700142E RID: 5166
	// (get) Token: 0x0600352F RID: 13615 RVA: 0x0001D295 File Offset: 0x0001B495
	// (set) Token: 0x06003530 RID: 13616 RVA: 0x0001D29D File Offset: 0x0001B49D
	public Animator SourceAnimator { get; set; }

	// Token: 0x06003531 RID: 13617 RVA: 0x000E00E4 File Offset: 0x000DE2E4
	protected virtual void Awake()
	{
		this.m_startingScale = base.transform.localScale;
		this.m_startingRotation = base.transform.localEulerAngles;
		this.m_startingPosition = base.transform.position;
		this.m_startingLayer = base.gameObject.layer;
		this.m_detachFromSource = new Action<IPreOnDisable>(this.DetachFromSource);
		this.IsAwakeCalled = true;
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x0001D2A6 File Offset: 0x0001B4A6
	protected virtual void OnDisable()
	{
		if (this.m_disableObj != null)
		{
			this.RemoveDetachListener(this.m_disableObj);
		}
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0001D2C3 File Offset: 0x0001B4C3
	public void AddDetachListener(IPreOnDisable disableObj)
	{
		this.m_disableObj = disableObj;
		disableObj.PreOnDisableRelay.AddOnce(this.m_detachFromSource, false);
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x0001D2DF File Offset: 0x0001B4DF
	public void RemoveDetachListener(IPreOnDisable disableObj)
	{
		this.m_disableObj = null;
		disableObj.PreOnDisableRelay.RemoveOnce(this.m_detachFromSource);
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x0001D2FA File Offset: 0x0001B4FA
	private void DetachFromSource(IPreOnDisable disableObj)
	{
		this.Source = null;
		this.SourceAnimator = null;
		base.transform.SetParent(null, true);
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x0001D317 File Offset: 0x0001B517
	public virtual void Play(float duration, EffectStopType stopType)
	{
		this.IsPlaying = true;
		this.m_onPlayRelay.Dispatch(this);
	}

	// Token: 0x06003537 RID: 13623
	public abstract void Stop(EffectStopType stopType);

	// Token: 0x06003538 RID: 13624 RVA: 0x0001D32C File Offset: 0x0001B52C
	protected virtual void PlayComplete()
	{
		this.IsPlaying = false;
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x0001D34E File Offset: 0x0001B54E
	public virtual void Flip()
	{
		base.transform.localScale = Vector3.Scale(base.transform.localScale, new Vector3(-1f, 1f, 1f));
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x000E0150 File Offset: 0x000DE350
	public virtual void ResetValues()
	{
		if (this.m_disableObj != null)
		{
			this.RemoveDetachListener(this.m_disableObj);
		}
		base.transform.localScale = this.m_startingScale;
		base.transform.localEulerAngles = this.m_startingRotation;
		base.transform.position = this.m_startingPosition;
		this.Source = null;
		this.SourceAnimator = null;
		this.EffectDirection = EffectTriggerDirection.None;
		this.DisableDestroyOnRoomChange = false;
		if (base.gameObject.layer != this.m_startingLayer)
		{
			base.gameObject.SetLayerRecursively(this.m_startingLayer, true);
		}
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002B1E RID: 11038
	private Vector3 m_startingScale;

	// Token: 0x04002B1F RID: 11039
	private Vector3 m_startingRotation;

	// Token: 0x04002B20 RID: 11040
	private Vector3 m_startingPosition;

	// Token: 0x04002B21 RID: 11041
	private int m_startingLayer;

	// Token: 0x04002B22 RID: 11042
	private IPreOnDisable m_disableObj;

	// Token: 0x04002B23 RID: 11043
	private Relay<BaseEffect> m_onPlayRelay = new Relay<BaseEffect>();

	// Token: 0x04002B24 RID: 11044
	private Action<IPreOnDisable> m_detachFromSource;
}
