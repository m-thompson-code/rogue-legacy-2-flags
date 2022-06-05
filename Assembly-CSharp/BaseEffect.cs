using System;
using Sigtrap.Relays;
using UnityEngine;

// Token: 0x02000407 RID: 1031
public abstract class BaseEffect : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x06002674 RID: 9844 RVA: 0x0007F8F9 File Offset: 0x0007DAF9
	public IRelayLink<BaseEffect> OnPlayRelay
	{
		get
		{
			return this.m_onPlayRelay.link;
		}
	}

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x06002675 RID: 9845 RVA: 0x0007F906 File Offset: 0x0007DB06
	// (set) Token: 0x06002676 RID: 9846 RVA: 0x0007F90E File Offset: 0x0007DB0E
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x06002677 RID: 9847 RVA: 0x0007F917 File Offset: 0x0007DB17
	// (set) Token: 0x06002678 RID: 9848 RVA: 0x0007F91F File Offset: 0x0007DB1F
	public bool IsAwakeCalled { get; protected set; }

	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x06002679 RID: 9849 RVA: 0x0007F928 File Offset: 0x0007DB28
	// (set) Token: 0x0600267A RID: 9850 RVA: 0x0007F930 File Offset: 0x0007DB30
	public int AnimatorLayer { get; set; }

	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x0600267B RID: 9851 RVA: 0x0007F939 File Offset: 0x0007DB39
	// (set) Token: 0x0600267C RID: 9852 RVA: 0x0007F941 File Offset: 0x0007DB41
	public bool DisableDestroyOnRoomChange { get; set; }

	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x0600267D RID: 9853 RVA: 0x0007F94A File Offset: 0x0007DB4A
	// (set) Token: 0x0600267E RID: 9854 RVA: 0x0007F952 File Offset: 0x0007DB52
	public bool IsPlaying { get; protected set; }

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x0600267F RID: 9855 RVA: 0x0007F95B File Offset: 0x0007DB5B
	// (set) Token: 0x06002680 RID: 9856 RVA: 0x0007F963 File Offset: 0x0007DB63
	public EffectTriggerDirection EffectDirection { get; set; }

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x06002681 RID: 9857 RVA: 0x0007F96C File Offset: 0x0007DB6C
	public bool IsFlipped
	{
		get
		{
			return base.gameObject.transform.localScale.x < 0f;
		}
	}

	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x06002682 RID: 9858 RVA: 0x0007F98A File Offset: 0x0007DB8A
	// (set) Token: 0x06002683 RID: 9859 RVA: 0x0007F992 File Offset: 0x0007DB92
	public BaseEffect EffectPrefab { get; set; }

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x06002684 RID: 9860 RVA: 0x0007F99B File Offset: 0x0007DB9B
	// (set) Token: 0x06002685 RID: 9861 RVA: 0x0007F9A3 File Offset: 0x0007DBA3
	public GameObject Source { get; set; }

	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06002686 RID: 9862 RVA: 0x0007F9AC File Offset: 0x0007DBAC
	// (set) Token: 0x06002687 RID: 9863 RVA: 0x0007F9B4 File Offset: 0x0007DBB4
	public Animator SourceAnimator { get; set; }

	// Token: 0x06002688 RID: 9864 RVA: 0x0007F9C0 File Offset: 0x0007DBC0
	protected virtual void Awake()
	{
		this.m_startingScale = base.transform.localScale;
		this.m_startingRotation = base.transform.localEulerAngles;
		this.m_startingPosition = base.transform.position;
		this.m_startingLayer = base.gameObject.layer;
		this.m_detachFromSource = new Action<IPreOnDisable>(this.DetachFromSource);
		this.IsAwakeCalled = true;
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x0007FA2A File Offset: 0x0007DC2A
	protected virtual void OnDisable()
	{
		if (this.m_disableObj != null)
		{
			this.RemoveDetachListener(this.m_disableObj);
		}
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x0007FA47 File Offset: 0x0007DC47
	public void AddDetachListener(IPreOnDisable disableObj)
	{
		this.m_disableObj = disableObj;
		disableObj.PreOnDisableRelay.AddOnce(this.m_detachFromSource, false);
	}

	// Token: 0x0600268B RID: 9867 RVA: 0x0007FA63 File Offset: 0x0007DC63
	public void RemoveDetachListener(IPreOnDisable disableObj)
	{
		this.m_disableObj = null;
		disableObj.PreOnDisableRelay.RemoveOnce(this.m_detachFromSource);
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x0007FA7E File Offset: 0x0007DC7E
	private void DetachFromSource(IPreOnDisable disableObj)
	{
		this.Source = null;
		this.SourceAnimator = null;
		base.transform.SetParent(null, true);
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x0007FA9B File Offset: 0x0007DC9B
	public virtual void Play(float duration, EffectStopType stopType)
	{
		this.IsPlaying = true;
		this.m_onPlayRelay.Dispatch(this);
	}

	// Token: 0x0600268E RID: 9870
	public abstract void Stop(EffectStopType stopType);

	// Token: 0x0600268F RID: 9871 RVA: 0x0007FAB0 File Offset: 0x0007DCB0
	protected virtual void PlayComplete()
	{
		this.IsPlaying = false;
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x0007FAD2 File Offset: 0x0007DCD2
	public virtual void Flip()
	{
		base.transform.localScale = Vector3.Scale(base.transform.localScale, new Vector3(-1f, 1f, 1f));
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x0007FB04 File Offset: 0x0007DD04
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

	// Token: 0x06002693 RID: 9875 RVA: 0x0007FBB1 File Offset: 0x0007DDB1
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002044 RID: 8260
	private Vector3 m_startingScale;

	// Token: 0x04002045 RID: 8261
	private Vector3 m_startingRotation;

	// Token: 0x04002046 RID: 8262
	private Vector3 m_startingPosition;

	// Token: 0x04002047 RID: 8263
	private int m_startingLayer;

	// Token: 0x04002048 RID: 8264
	private IPreOnDisable m_disableObj;

	// Token: 0x04002049 RID: 8265
	private Relay<BaseEffect> m_onPlayRelay = new Relay<BaseEffect>();

	// Token: 0x0400204A RID: 8266
	private Action<IPreOnDisable> m_detachFromSource;
}
