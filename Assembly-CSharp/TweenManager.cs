using System;
using UnityEngine;

// Token: 0x0200081D RID: 2077
public class TweenManager : MonoBehaviour
{
	// Token: 0x060044C6 RID: 17606 RVA: 0x000F486F File Offset: 0x000F2A6F
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x060044C7 RID: 17607 RVA: 0x000F4878 File Offset: 0x000F2A78
	private void Initialize()
	{
		Tween tween = new GameObject("Tween").AddComponent<Tween>();
		this.m_tweenPool = new GenericPool_RL<Tween>();
		this.m_tweenPool.Initialize(tween, this.m_poolSize, false, true);
		tween.transform.SetParent(base.transform);
		tween.gameObject.SetActive(false);
		TweenManager.m_isInitialized = true;
	}

	// Token: 0x1700170E RID: 5902
	// (get) Token: 0x060044C8 RID: 17608 RVA: 0x000F48D7 File Offset: 0x000F2AD7
	public static bool IsInitialized
	{
		get
		{
			return TweenManager.m_isInitialized;
		}
	}

	// Token: 0x1700170F RID: 5903
	// (get) Token: 0x060044C9 RID: 17609 RVA: 0x000F48DE File Offset: 0x000F2ADE
	public static TweenManager Instance
	{
		get
		{
			if (TweenManager.m_tweenManager == null)
			{
				TweenManager.m_tweenManager = CDGHelper.FindStaticInstance<TweenManager>(false);
			}
			return TweenManager.m_tweenManager;
		}
	}

	// Token: 0x060044CA RID: 17610 RVA: 0x000F48FD File Offset: 0x000F2AFD
	public static Tween RunFunction(float delay, object methodObject, string functionName, params object[] args)
	{
		Tween tween = TweenManager.TweenTo(methodObject, delay, new EaseDelegate(Ease.None), Array.Empty<object>());
		tween.AddCustomEndHandler(methodObject, functionName, args);
		return tween;
	}

	// Token: 0x060044CB RID: 17611 RVA: 0x000F4920 File Offset: 0x000F2B20
	public static Tween RunFunction_UnscaledTime(float delay, object methodObject, string functionName, params object[] args)
	{
		Tween tween = TweenManager.TweenTo_UnscaledTime(methodObject, delay, new EaseDelegate(Ease.None), Array.Empty<object>());
		tween.AddCustomEndHandler(methodObject, functionName, args);
		return tween;
	}

	// Token: 0x060044CC RID: 17612 RVA: 0x000F4944 File Offset: 0x000F2B44
	public static Tween TweenBy(object TweenRLObj, float duration, EaseDelegate ease, params object[] properties)
	{
		if (properties.Length % 2 != 0)
		{
			throw new Exception("TweenRL.By parameters must be submitted as follows - <property name>, <property value>");
		}
		Tween freeObj = TweenManager.Instance.m_tweenPool.GetFreeObj();
		freeObj.gameObject.SetActive(true);
		if (ease == null)
		{
			ease = new EaseDelegate(Ease.None);
		}
		freeObj.SetValues(TweenRLObj, duration, false, ease, false, properties);
		TweenManager.Instance.m_lastTweenedObj = freeObj;
		freeObj.StartTween();
		return freeObj;
	}

	// Token: 0x060044CD RID: 17613 RVA: 0x000F49B0 File Offset: 0x000F2BB0
	public static Tween TweenBy_UnscaledTime(object TweenRLObj, float duration, EaseDelegate ease, params object[] properties)
	{
		if (properties.Length % 2 != 0)
		{
			throw new Exception("TweenRL.By parameters must be submitted as follows - <property name>, <property value>");
		}
		Tween freeObj = TweenManager.Instance.m_tweenPool.GetFreeObj();
		freeObj.gameObject.SetActive(true);
		if (ease == null)
		{
			ease = new EaseDelegate(Ease.None);
		}
		freeObj.SetValues(TweenRLObj, duration, true, ease, false, properties);
		TweenManager.Instance.m_lastTweenedObj = freeObj;
		freeObj.StartTween();
		return freeObj;
	}

	// Token: 0x060044CE RID: 17614 RVA: 0x000F4A1C File Offset: 0x000F2C1C
	public static Tween TweenTo(object TweenRLObj, float duration, EaseDelegate ease, params object[] properties)
	{
		if (properties.Length % 2 != 0)
		{
			throw new Exception("TweenRL.To parameters must be submitted as follows - <property name>, <property value>");
		}
		Tween freeObj = TweenManager.Instance.m_tweenPool.GetFreeObj();
		freeObj.gameObject.SetActive(true);
		if (ease == null)
		{
			ease = new EaseDelegate(Ease.None);
		}
		freeObj.SetValues(TweenRLObj, duration, false, ease, true, properties);
		TweenManager.Instance.m_lastTweenedObj = freeObj;
		freeObj.StartTween();
		return freeObj;
	}

	// Token: 0x060044CF RID: 17615 RVA: 0x000F4A88 File Offset: 0x000F2C88
	public static Tween TweenTo_UnscaledTime(object TweenRLObj, float duration, EaseDelegate ease, params object[] properties)
	{
		if (properties.Length % 2 != 0)
		{
			throw new Exception("TweenRL.To parameters must be submitted as follows - <property name>, <property value>");
		}
		Tween freeObj = TweenManager.Instance.m_tweenPool.GetFreeObj();
		freeObj.gameObject.SetActive(true);
		if (ease == null)
		{
			ease = new EaseDelegate(Ease.None);
		}
		freeObj.SetValues(TweenRLObj, duration, true, ease, true, properties);
		TweenManager.Instance.m_lastTweenedObj = freeObj;
		freeObj.StartTween();
		return freeObj;
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x000F4AF4 File Offset: 0x000F2CF4
	public static void StopAllTweens(bool runEndHandlers)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled)
			{
				tween.StopTween(runEndHandlers);
			}
		}
	}

	// Token: 0x060044D1 RID: 17617 RVA: 0x000F4B58 File Offset: 0x000F2D58
	public static void StopAllTweensContaining(object tweenedObj, bool runEndHandlers)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled && tween.TweenedObj == tweenedObj)
			{
				tween.StopTween(runEndHandlers);
			}
		}
	}

	// Token: 0x060044D2 RID: 17618 RVA: 0x000F4BC8 File Offset: 0x000F2DC8
	public static void StopAllTweensContaining(string ID, bool runEndHandlers)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled && tween.ID == ID)
			{
				tween.StopTween(runEndHandlers);
			}
		}
	}

	// Token: 0x060044D3 RID: 17619 RVA: 0x000F4C3C File Offset: 0x000F2E3C
	public static void StopAllTweensContaining(object tweenedObj, string ID, bool runEndHandlers)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled && tween.TweenedObj == tweenedObj && tween.ID == ID)
			{
				tween.StopTween(runEndHandlers);
			}
		}
	}

	// Token: 0x060044D4 RID: 17620 RVA: 0x000F4CB8 File Offset: 0x000F2EB8
	public static void SetPauseAllTweensContaining(object tweenedObj, bool isPaused)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled && tween.TweenedObj == tweenedObj)
			{
				tween.SetPaused(isPaused);
			}
		}
	}

	// Token: 0x060044D5 RID: 17621 RVA: 0x000F4D28 File Offset: 0x000F2F28
	public static void SetPauseAllTweensContaining(object tweenedObj, string ID, bool isPaused)
	{
		foreach (Tween tween in TweenManager.Instance.m_tweenPool.ObjectList)
		{
			if (tween.isActiveAndEnabled && tween.TweenedObj == tweenedObj && tween.ID == ID)
			{
				tween.SetPaused(isPaused);
			}
		}
	}

	// Token: 0x060044D6 RID: 17622 RVA: 0x000F4DA4 File Offset: 0x000F2FA4
	private void OnDestroy()
	{
		TweenManager.m_isInitialized = false;
	}

	// Token: 0x04003AB2 RID: 15026
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_poolSize = 20;

	// Token: 0x04003AB3 RID: 15027
	private GenericPool_RL<Tween> m_tweenPool;

	// Token: 0x04003AB4 RID: 15028
	private Tween m_lastTweenedObj;

	// Token: 0x04003AB5 RID: 15029
	private static bool m_isInitialized;

	// Token: 0x04003AB6 RID: 15030
	private static TweenManager m_tweenManager;
}
