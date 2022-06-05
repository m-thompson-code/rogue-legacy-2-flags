using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class RLTimeScale
{
	// Token: 0x17001007 RID: 4103
	// (get) Token: 0x0600265C RID: 9820 RVA: 0x000155E2 File Offset: 0x000137E2
	public static TimeScaleType[] TypeArray
	{
		get
		{
			if (RLTimeScale.m_typeArray == null)
			{
				RLTimeScale.m_typeArray = (Enum.GetValues(typeof(TimeScaleType)) as TimeScaleType[]);
			}
			return RLTimeScale.m_typeArray;
		}
	}

	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x0600265D RID: 9821 RVA: 0x00015609 File Offset: 0x00013809
	// (set) Token: 0x0600265E RID: 9822 RVA: 0x00015610 File Offset: 0x00013810
	public static bool IsInitialized { get; private set; }

	// Token: 0x0600265F RID: 9823 RVA: 0x000B6128 File Offset: 0x000B4328
	public static TimeScaleType GetAvailableSlowTimeStack()
	{
		if (!RLTimeScale.IsInitialized)
		{
			RLTimeScale.Initialize();
		}
		int num = 1;
		int num2 = 5;
		for (int i = num; i < num + num2; i++)
		{
			TimeScaleType timeScaleType = (TimeScaleType)i;
			if (RLTimeScale.m_timeScaleDict[timeScaleType] == 1f)
			{
				return timeScaleType;
			}
		}
		Debug.Log("No available SlowTimeScale available.  Returning first on stack.");
		return TimeScaleType.SlowTimeEffectStack1;
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000B6174 File Offset: 0x000B4374
	private static void Initialize()
	{
		RLTimeScale.m_timeScaleDict = new Dictionary<TimeScaleType, float>(RLTimeScale.TypeArray.Length);
		foreach (TimeScaleType key in RLTimeScale.TypeArray)
		{
			RLTimeScale.m_timeScaleDict.Add(key, 1f);
		}
		RLTimeScale.IsInitialized = true;
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x000B61C0 File Offset: 0x000B43C0
	public static void SetTimeScale(TimeScaleType timeScaleType, float value)
	{
		if (!RLTimeScale.IsInitialized)
		{
			RLTimeScale.Initialize();
		}
		RLTimeScale.m_timeScaleDict[timeScaleType] = value;
		float num = float.MaxValue;
		bool flag = false;
		foreach (TimeScaleType key in RLTimeScale.TypeArray)
		{
			float num2 = RLTimeScale.m_timeScaleDict[key];
			if (num2 != 1f && num2 < num)
			{
				num = RLTimeScale.m_timeScaleDict[key];
				flag = true;
			}
		}
		if (!flag && Time.timeScale != 1f)
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.016666668f;
			Time.maximumDeltaTime = 0.033333335f;
			return;
		}
		if (flag && Time.timeScale != num)
		{
			Time.timeScale = num;
			if (num != 0f)
			{
				Time.fixedDeltaTime = 0.016666668f * Time.timeScale;
				return;
			}
			Time.fixedDeltaTime = 0.016666668f;
			Time.maximumDeltaTime = 0.033333335f;
		}
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x00015618 File Offset: 0x00013818
	public static float GetTimeScale(TimeScaleType timeScaleType)
	{
		if (!RLTimeScale.IsInitialized)
		{
			RLTimeScale.Initialize();
		}
		return RLTimeScale.m_timeScaleDict[timeScaleType];
	}

	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x06002663 RID: 9827 RVA: 0x00015631 File Offset: 0x00013831
	public static float TimeScale
	{
		get
		{
			return Time.timeScale;
		}
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x000B62A0 File Offset: 0x000B44A0
	public static void SetAllTimeScale(float value)
	{
		if (!RLTimeScale.IsInitialized)
		{
			RLTimeScale.Initialize();
		}
		TimeScaleType[] typeArray = RLTimeScale.TypeArray;
		for (int i = 0; i < typeArray.Length; i++)
		{
			RLTimeScale.SetTimeScale(typeArray[i], value);
		}
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x00015638 File Offset: 0x00013838
	public static void Reset()
	{
		RLTimeScale.SetAllTimeScale(1f);
	}

	// Token: 0x04002147 RID: 8519
	private static Dictionary<TimeScaleType, float> m_timeScaleDict;

	// Token: 0x04002148 RID: 8520
	private static TimeScaleType[] m_typeArray;
}
