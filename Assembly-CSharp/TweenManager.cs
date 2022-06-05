using System;
using UnityEngine;

// Token: 0x02000CF4 RID: 3316
public class TweenManager : MonoBehaviour
{
	// Token: 0x06005E92 RID: 24210 RVA: 0x00034204 File Offset: 0x00032404
	private void Awake()
	{
		this.Initialize();
	}

	// Token: 0x06005E93 RID: 24211 RVA: 0x0016258C File Offset: 0x0016078C
	private void Initialize()
	{
		Tween tween = new GameObject("Tween").AddComponent<Tween>();
		this.m_tweenPool = new GenericPool_RL<Tween>();
		this.m_tweenPool.Initialize(tween, this.m_poolSize, false, true);
		tween.transform.SetParent(base.transform);
		tween.gameObject.SetActive(false);
		TweenManager.m_isInitialized = true;
	}

	// Token: 0x17001F1C RID: 7964
	// (get) Token: 0x06005E94 RID: 24212 RVA: 0x0003420C File Offset: 0x0003240C
	public static bool IsInitialized
	{
		get
		{
			return TweenManager.m_isInitialized;
		}
	}

	// Token: 0x17001F1D RID: 7965
	// (get) Token: 0x06005E95 RID: 24213 RVA: 0x00034213 File Offset: 0x00032413
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

	// Token: 0x06005E96 RID: 24214 RVA: 0x00034232 File Offset: 0x00032432
	public static Tween RunFunction(float delay, object methodObject, string functionName, params object[] args)
	{
		Tween tween = TweenManager.TweenTo(methodObject, delay, new EaseDelegate(Ease.None), Array.Empty<object>());
		tween.AddCustomEndHandler(methodObject, functionName, args);
		return tween;
	}

	// Token: 0x06005E97 RID: 24215 RVA: 0x00034255 File Offset: 0x00032455
	public static Tween RunFunction_UnscaledTime(float delay, object methodObject, string functionName, params object[] args)
	{
		Tween tween = TweenManager.TweenTo_UnscaledTime(methodObject, delay, new EaseDelegate(Ease.None), Array.Empty<object>());
		tween.AddCustomEndHandler(methodObject, functionName, args);
		return tween;
	}

	// Token: 0x06005E98 RID: 24216 RVA: 0x001625EC File Offset: 0x001607EC
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

	// Token: 0x06005E99 RID: 24217 RVA: 0x00162658 File Offset: 0x00160858
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

	// Token: 0x06005E9A RID: 24218 RVA: 0x001626C4 File Offset: 0x001608C4
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

	// Token: 0x06005E9B RID: 24219 RVA: 0x00162730 File Offset: 0x00160930
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

	// Token: 0x06005E9C RID: 24220 RVA: 0x0016279C File Offset: 0x0016099C
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

	// Token: 0x06005E9D RID: 24221 RVA: 0x00162800 File Offset: 0x00160A00
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

	// Token: 0x06005E9E RID: 24222 RVA: 0x00162870 File Offset: 0x00160A70
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

	// Token: 0x06005E9F RID: 24223 RVA: 0x001628E4 File Offset: 0x00160AE4
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

	// Token: 0x06005EA0 RID: 24224 RVA: 0x00162960 File Offset: 0x00160B60
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

	// Token: 0x06005EA1 RID: 24225 RVA: 0x001629D0 File Offset: 0x00160BD0
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

	// Token: 0x06005EA2 RID: 24226 RVA: 0x00034278 File Offset: 0x00032478
	private void OnDestroy()
	{
		TweenManager.m_isInitialized = false;
	}

	// Token: 0x04004DB6 RID: 19894
	[SerializeField]
	[ReadOnlyOnPlay]
	private int m_poolSize = 20;

	// Token: 0x04004DB7 RID: 19895
	private GenericPool_RL<Tween> m_tweenPool;

	// Token: 0x04004DB8 RID: 19896
	private Tween m_lastTweenedObj;

	// Token: 0x04004DB9 RID: 19897
	private static bool m_isInitialized;

	// Token: 0x04004DBA RID: 19898
	private static TweenManager m_tweenManager;
}
