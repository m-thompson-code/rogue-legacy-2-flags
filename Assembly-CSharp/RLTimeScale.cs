using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B7 RID: 695
public class RLTimeScale
{
	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x00059269 File Offset: 0x00057469
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

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x00059290 File Offset: 0x00057490
	// (set) Token: 0x06001BAA RID: 7082 RVA: 0x00059297 File Offset: 0x00057497
	public static bool IsInitialized { get; private set; }

	// Token: 0x06001BAB RID: 7083 RVA: 0x000592A0 File Offset: 0x000574A0
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

	// Token: 0x06001BAC RID: 7084 RVA: 0x000592EC File Offset: 0x000574EC
	private static void Initialize()
	{
		RLTimeScale.m_timeScaleDict = new Dictionary<TimeScaleType, float>(RLTimeScale.TypeArray.Length);
		foreach (TimeScaleType key in RLTimeScale.TypeArray)
		{
			RLTimeScale.m_timeScaleDict.Add(key, 1f);
		}
		RLTimeScale.IsInitialized = true;
	}

	// Token: 0x06001BAD RID: 7085 RVA: 0x00059338 File Offset: 0x00057538
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

	// Token: 0x06001BAE RID: 7086 RVA: 0x00059415 File Offset: 0x00057615
	public static float GetTimeScale(TimeScaleType timeScaleType)
	{
		if (!RLTimeScale.IsInitialized)
		{
			RLTimeScale.Initialize();
		}
		return RLTimeScale.m_timeScaleDict[timeScaleType];
	}

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06001BAF RID: 7087 RVA: 0x0005942E File Offset: 0x0005762E
	public static float TimeScale
	{
		get
		{
			return Time.timeScale;
		}
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00059438 File Offset: 0x00057638
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

	// Token: 0x06001BB1 RID: 7089 RVA: 0x0005946E File Offset: 0x0005766E
	public static void Reset()
	{
		RLTimeScale.SetAllTimeScale(1f);
	}

	// Token: 0x0400195A RID: 6490
	private static Dictionary<TimeScaleType, float> m_timeScaleDict;

	// Token: 0x0400195B RID: 6491
	private static TimeScaleType[] m_typeArray;
}
