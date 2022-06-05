using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000B1F RID: 2847
public class EffectManager : MonoBehaviour, ILoadable
{
	// Token: 0x060055CB RID: 21963 RVA: 0x00144400 File Offset: 0x00142600
	private void Awake()
	{
		if (EffectManager.m_effectManager == null)
		{
			EffectManager.m_effectManager = this;
			this.m_disableExitRoomEffects = new Action<MonoBehaviour, EventArgs>(this.DisableExitRoomEffects);
			this.m_effectStringDict = new Dictionary<string, GenericPool_RL<BaseEffect>>();
			Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitRoom, this.m_disableExitRoomEffects);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x0002EA78 File Offset: 0x0002CC78
	private void OnDestroy()
	{
		EffectManager.m_effectManager = null;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_disableExitRoomEffects);
		EffectManager.m_isInitialized = false;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x0002EA93 File Offset: 0x0002CC93
	public void LoadSync()
	{
		this.PopulateDictionary();
		EffectManager.m_isInitialized = true;
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x00144458 File Offset: 0x00142658
	private void PopulateDictionary()
	{
		foreach (object obj in Enum.GetValues(typeof(EffectCategoryType)))
		{
			foreach (EffectEntry entry in EffectLibrary.GetEffectEntryList((EffectCategoryType)obj))
			{
				this.AddDictionaryEntry(entry);
			}
		}
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x0002EAA1 File Offset: 0x0002CCA1
	public IEnumerator LoadAsync()
	{
		yield return EffectLibrary.LoadAsync();
		yield return this.PopulateDictionaryAsync();
		EffectManager.m_isInitialized = true;
		yield break;
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x0002EAB0 File Offset: 0x0002CCB0
	private IEnumerator PopulateDictionaryAsync()
	{
		Stopwatch timer = new Stopwatch();
		timer.Start();
		foreach (object obj in Enum.GetValues(typeof(EffectCategoryType)))
		{
			EffectCategoryType category = (EffectCategoryType)obj;
			foreach (EffectEntry entry in EffectLibrary.GetEffectEntryList(category))
			{
				this.AddDictionaryEntry(entry);
				if (timer.Elapsed.TotalMilliseconds >= 33.0)
				{
					yield return null;
					timer.Restart();
				}
			}
			List<EffectEntry>.Enumerator enumerator2 = default(List<EffectEntry>.Enumerator);
		}
		IEnumerator enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x001444F4 File Offset: 0x001426F4
	private void AddDictionaryEntry(EffectEntry entry)
	{
		BaseEffect effectPrefab = entry.EffectPrefab;
		if (this.m_effectStringDict.ContainsKey(effectPrefab.name))
		{
			throw new Exception("Effect: " + effectPrefab.name + " already found in Effect Library.  Duplicates not allowed.");
		}
		GenericPool_RL<BaseEffect> genericPool_RL = new GenericPool_RL<BaseEffect>();
		genericPool_RL.Initialize(effectPrefab, entry.EffectPoolSize, false, false);
		this.m_effectStringDict.Add(effectPrefab.name, genericPool_RL);
	}

	// Token: 0x17001D08 RID: 7432
	// (get) Token: 0x060055D2 RID: 21970 RVA: 0x0002EABF File Offset: 0x0002CCBF
	public static bool IsInitialized
	{
		get
		{
			return EffectManager.m_isInitialized;
		}
	}

	// Token: 0x17001D09 RID: 7433
	// (get) Token: 0x060055D3 RID: 21971 RVA: 0x0002EAC6 File Offset: 0x0002CCC6
	public static EffectManager Instance
	{
		get
		{
			return EffectManager.m_effectManager;
		}
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x0002EACD File Offset: 0x0002CCCD
	public static bool HasEffect(string effectName)
	{
		return EffectManager.Instance.m_effectStringDict.ContainsKey(effectName);
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x0002EADF File Offset: 0x0002CCDF
	public static List<BaseEffect> GetEffectList(string effectName)
	{
		if (EffectManager.HasEffect(effectName))
		{
			return EffectManager.Instance.m_effectStringDict[effectName].ObjectList;
		}
		return null;
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x0002EB00 File Offset: 0x0002CD00
	public static void SetEffectParams(string effectName, params object[] args)
	{
		if (args.Length % 2 != 0)
		{
			throw new Exception("Effect params must follow the pattern of <VAR NAME>, <VAR TYPE>");
		}
		EffectManager.m_effectArgsName = effectName;
		EffectManager.m_effectArgs = args;
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x00144560 File Offset: 0x00142760
	private static void ApplyEffectParams(string effectName, BaseEffect effect)
	{
		if (EffectManager.m_effectArgs != null)
		{
			try
			{
				for (int i = 0; i < EffectManager.m_effectArgs.Length - 1; i += 2)
				{
					effect.GetType().GetField(EffectManager.m_effectArgs[i] as string, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(effect, EffectManager.m_effectArgs[i + 1]);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("Applying effect params of type: " + effectName + " failed. Some of the parameters were incorrect.  Full Error: " + ex.Message);
			}
			Array.Clear(EffectManager.m_effectArgs, 0, EffectManager.m_effectArgs.Length);
		}
		EffectManager.m_effectArgs = null;
		EffectManager.m_effectArgsName = null;
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x00144600 File Offset: 0x00142800
	public static BaseEffect PlayEffect(GameObject source, Animator sourceAnimator, string effectName, Vector3 effectPosition, float duration, EffectStopType stopType, EffectTriggerDirection direction = EffectTriggerDirection.None)
	{
		GenericPool_RL<BaseEffect> genericPool_RL = null;
		if (!EffectManager.Instance.m_effectStringDict.TryGetValue(effectName, out genericPool_RL))
		{
			throw new Exception("Effect: " + effectName + " cannot be found in EffectManager.  Please make sure the prefab is added to the Effect Library prefab.");
		}
		BaseEffect freeObj = genericPool_RL.GetFreeObj();
		EffectManager.ApplyEffectParams(effectName, freeObj);
		freeObj.EffectDirection = direction;
		freeObj.Source = source;
		freeObj.SourceAnimator = sourceAnimator;
		freeObj.gameObject.transform.position = new Vector3(effectPosition.x, effectPosition.y, freeObj.gameObject.transform.position.z);
		freeObj.gameObject.SetActive(true);
		freeObj.Play(duration, stopType);
		return freeObj;
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x001446AC File Offset: 0x001428AC
	public static BaseEffect PlayHitEffect(IDamageObj damageObj, Vector3 collisionPoint, string effectNameOverride, StrikeType strikeType, bool isUnscaled = false)
	{
		if (!string.IsNullOrEmpty(effectNameOverride))
		{
			return EffectManager.PlayEffect(damageObj.gameObject, null, effectNameOverride, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		}
		string str = "";
		if (isUnscaled)
		{
			str = "Unscaled";
		}
		if (strikeType <= StrikeType.NoDamage)
		{
			if (strikeType != StrikeType.Blunt)
			{
				if (strikeType == StrikeType.Sharp)
				{
					return EffectManager.PlayEffect(damageObj.gameObject, null, "HitSparksSlash_Effect" + str, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				}
				if (strikeType == StrikeType.NoDamage)
				{
					return EffectManager.PlayEffect(damageObj.gameObject, null, "HitSparksInvincible_Effect" + str, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
				}
			}
		}
		else
		{
			if (strikeType == StrikeType.Invincible_StatusEffect)
			{
				return EffectManager.PlayEffect(damageObj.gameObject, null, "ShieldHit_Effect" + str, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			if (strikeType == StrikeType.Critical)
			{
				return EffectManager.PlayEffect(damageObj.gameObject, null, "CriticalHit_Effect" + str, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
			if (strikeType == StrikeType.OnHitAreaRelic)
			{
				return EffectManager.PlayEffect(damageObj.gameObject, null, "RelicOnHitAreaDamage_Chains_Effect", collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
			}
		}
		return EffectManager.PlayEffect(damageObj.gameObject, null, "HitSparksGeneric_Effect" + str, collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x001447CC File Offset: 0x001429CC
	public static BaseEffect PlayDirectionalHitEffect(IDamageObj damageObj, GameObject damageRootObj, Vector3 collisionPoint)
	{
		Vector3 v = damageRootObj.transform.position;
		IMidpointObj component = damageRootObj.GetComponent<IMidpointObj>();
		if (component != null)
		{
			v = component.Midpoint;
		}
		BaseEffect baseEffect = EffectManager.PlayEffect(damageObj.gameObject, null, "DamageFlashDirectional_Effect", collisionPoint, 0f, EffectStopType.Gracefully, EffectTriggerDirection.None);
		float num = CDGHelper.AngleBetweenPts(v, collisionPoint);
		baseEffect.transform.SetLocalEulerZ(num - 90f);
		return baseEffect;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x0002EB20 File Offset: 0x0002CD20
	public static BaseEffect PlayHitEffectCamShake(float duration)
	{
		return EffectManager.PlayEffect(PlayerManager.GetCurrentPlayerRoom().gameObject, null, "CameraShakeVerySmall_Effect", Vector3.zero, duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x0002EB3F File Offset: 0x0002CD3F
	public static void StopEffect(BaseEffect effect, EffectStopType stopMode)
	{
		effect.Stop(stopMode);
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x00144834 File Offset: 0x00142A34
	public static void DisableAllEffectWithName(string effectName)
	{
		if (!EffectManager.Instance)
		{
			return;
		}
		GenericPool_RL<BaseEffect> genericPool_RL;
		if (EffectManager.Instance.m_effectStringDict.TryGetValue(effectName, out genericPool_RL))
		{
			genericPool_RL.DisableAll();
		}
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x0014486C File Offset: 0x00142A6C
	public static void DisableAllEffects()
	{
		foreach (KeyValuePair<string, GenericPool_RL<BaseEffect>> keyValuePair in EffectManager.Instance.m_effectStringDict)
		{
			if (!keyValuePair.Value.DisableAll())
			{
				throw new Exception("Failed to disable effect: " + keyValuePair.Key);
			}
		}
	}

	// Token: 0x060055DF RID: 21983 RVA: 0x001448E4 File Offset: 0x00142AE4
	private void DisableExitRoomEffects(object sender, EventArgs args)
	{
		if (SceneLoader_RL.IsRunningTransitionWithLogic)
		{
			EffectManager.DisableAllEffects();
			return;
		}
		foreach (KeyValuePair<string, GenericPool_RL<BaseEffect>> keyValuePair in EffectManager.Instance.m_effectStringDict)
		{
			foreach (BaseEffect baseEffect in keyValuePair.Value.ObjectList)
			{
				if (!baseEffect.DisableDestroyOnRoomChange && baseEffect.gameObject.activeSelf)
				{
					baseEffect.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060055E0 RID: 21984 RVA: 0x0002EB48 File Offset: 0x0002CD48
	public static void AddAnimatorToDisableList(Animator animator)
	{
		if (!animator)
		{
			return;
		}
		if (EffectManager.Instance.m_globalEffectDisableList.Contains(animator))
		{
			return;
		}
		EffectManager.Instance.m_globalEffectDisableList.Add(animator);
	}

	// Token: 0x060055E1 RID: 21985 RVA: 0x0002EB77 File Offset: 0x0002CD77
	public static void RemoveAnimatorFromDisableList(Animator animator)
	{
		if (!animator)
		{
			return;
		}
		if (!EffectManager.Instance.m_globalEffectDisableList.Contains(animator))
		{
			return;
		}
		EffectManager.Instance.m_globalEffectDisableList.Remove(animator);
	}

	// Token: 0x060055E2 RID: 21986 RVA: 0x0002EBA6 File Offset: 0x0002CDA6
	public static bool AnimatorEffectsDisabled(Animator animator)
	{
		return animator && EffectManager.Instance.m_globalEffectDisableList.Contains(animator);
	}

	// Token: 0x060055E3 RID: 21987 RVA: 0x0002EBC2 File Offset: 0x0002CDC2
	public static void Reset()
	{
		EffectManager.DisableAllEffects();
	}

	// Token: 0x04003FA2 RID: 16290
	private const string EFFECTSMANAGER_NAME = "EffectManager";

	// Token: 0x04003FA3 RID: 16291
	private const string RESOURCE_PATH = "Prefabs/Managers/EffectManager";

	// Token: 0x04003FA4 RID: 16292
	private const bool LOG_CALLER_STACK = false;

	// Token: 0x04003FA5 RID: 16293
	private Dictionary<string, GenericPool_RL<BaseEffect>> m_effectStringDict;

	// Token: 0x04003FA6 RID: 16294
	private HashSet<Animator> m_globalEffectDisableList = new HashSet<Animator>();

	// Token: 0x04003FA7 RID: 16295
	private static bool m_isInitialized;

	// Token: 0x04003FA8 RID: 16296
	private static EffectManager m_effectManager;

	// Token: 0x04003FA9 RID: 16297
	private Action<MonoBehaviour, EventArgs> m_disableExitRoomEffects;

	// Token: 0x04003FAA RID: 16298
	private static object[] m_effectArgs;

	// Token: 0x04003FAB RID: 16299
	private static string m_effectArgsName;
}
