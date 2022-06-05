using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using SceneManagement_RL;
using UnityEngine;

// Token: 0x02000696 RID: 1686
public class EffectManager : MonoBehaviour, ILoadable
{
	// Token: 0x06003D36 RID: 15670 RVA: 0x000D4504 File Offset: 0x000D2704
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

	// Token: 0x06003D37 RID: 15671 RVA: 0x000D455A File Offset: 0x000D275A
	private void OnDestroy()
	{
		EffectManager.m_effectManager = null;
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitRoom, this.m_disableExitRoomEffects);
		EffectManager.m_isInitialized = false;
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x000D4575 File Offset: 0x000D2775
	public void LoadSync()
	{
		this.PopulateDictionary();
		EffectManager.m_isInitialized = true;
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x000D4584 File Offset: 0x000D2784
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

	// Token: 0x06003D3A RID: 15674 RVA: 0x000D4620 File Offset: 0x000D2820
	public IEnumerator LoadAsync()
	{
		yield return EffectLibrary.LoadAsync();
		yield return this.PopulateDictionaryAsync();
		EffectManager.m_isInitialized = true;
		yield break;
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x000D462F File Offset: 0x000D282F
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

	// Token: 0x06003D3C RID: 15676 RVA: 0x000D4640 File Offset: 0x000D2840
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

	// Token: 0x1700153C RID: 5436
	// (get) Token: 0x06003D3D RID: 15677 RVA: 0x000D46A9 File Offset: 0x000D28A9
	public static bool IsInitialized
	{
		get
		{
			return EffectManager.m_isInitialized;
		}
	}

	// Token: 0x1700153D RID: 5437
	// (get) Token: 0x06003D3E RID: 15678 RVA: 0x000D46B0 File Offset: 0x000D28B0
	public static EffectManager Instance
	{
		get
		{
			return EffectManager.m_effectManager;
		}
	}

	// Token: 0x06003D3F RID: 15679 RVA: 0x000D46B7 File Offset: 0x000D28B7
	public static bool HasEffect(string effectName)
	{
		return EffectManager.Instance.m_effectStringDict.ContainsKey(effectName);
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x000D46C9 File Offset: 0x000D28C9
	public static List<BaseEffect> GetEffectList(string effectName)
	{
		if (EffectManager.HasEffect(effectName))
		{
			return EffectManager.Instance.m_effectStringDict[effectName].ObjectList;
		}
		return null;
	}

	// Token: 0x06003D41 RID: 15681 RVA: 0x000D46EA File Offset: 0x000D28EA
	public static void SetEffectParams(string effectName, params object[] args)
	{
		if (args.Length % 2 != 0)
		{
			throw new Exception("Effect params must follow the pattern of <VAR NAME>, <VAR TYPE>");
		}
		EffectManager.m_effectArgsName = effectName;
		EffectManager.m_effectArgs = args;
	}

	// Token: 0x06003D42 RID: 15682 RVA: 0x000D470C File Offset: 0x000D290C
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

	// Token: 0x06003D43 RID: 15683 RVA: 0x000D47AC File Offset: 0x000D29AC
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

	// Token: 0x06003D44 RID: 15684 RVA: 0x000D4858 File Offset: 0x000D2A58
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

	// Token: 0x06003D45 RID: 15685 RVA: 0x000D4978 File Offset: 0x000D2B78
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

	// Token: 0x06003D46 RID: 15686 RVA: 0x000D49DF File Offset: 0x000D2BDF
	public static BaseEffect PlayHitEffectCamShake(float duration)
	{
		return EffectManager.PlayEffect(PlayerManager.GetCurrentPlayerRoom().gameObject, null, "CameraShakeVerySmall_Effect", Vector3.zero, duration, EffectStopType.Gracefully, EffectTriggerDirection.None);
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x000D49FE File Offset: 0x000D2BFE
	public static void StopEffect(BaseEffect effect, EffectStopType stopMode)
	{
		effect.Stop(stopMode);
	}

	// Token: 0x06003D48 RID: 15688 RVA: 0x000D4A08 File Offset: 0x000D2C08
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

	// Token: 0x06003D49 RID: 15689 RVA: 0x000D4A40 File Offset: 0x000D2C40
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

	// Token: 0x06003D4A RID: 15690 RVA: 0x000D4AB8 File Offset: 0x000D2CB8
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

	// Token: 0x06003D4B RID: 15691 RVA: 0x000D4B78 File Offset: 0x000D2D78
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

	// Token: 0x06003D4C RID: 15692 RVA: 0x000D4BA7 File Offset: 0x000D2DA7
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

	// Token: 0x06003D4D RID: 15693 RVA: 0x000D4BD6 File Offset: 0x000D2DD6
	public static bool AnimatorEffectsDisabled(Animator animator)
	{
		return animator && EffectManager.Instance.m_globalEffectDisableList.Contains(animator);
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x000D4BF2 File Offset: 0x000D2DF2
	public static void Reset()
	{
		EffectManager.DisableAllEffects();
	}

	// Token: 0x04002DD7 RID: 11735
	private const string EFFECTSMANAGER_NAME = "EffectManager";

	// Token: 0x04002DD8 RID: 11736
	private const string RESOURCE_PATH = "Prefabs/Managers/EffectManager";

	// Token: 0x04002DD9 RID: 11737
	private const bool LOG_CALLER_STACK = false;

	// Token: 0x04002DDA RID: 11738
	private Dictionary<string, GenericPool_RL<BaseEffect>> m_effectStringDict;

	// Token: 0x04002DDB RID: 11739
	private HashSet<Animator> m_globalEffectDisableList = new HashSet<Animator>();

	// Token: 0x04002DDC RID: 11740
	private static bool m_isInitialized;

	// Token: 0x04002DDD RID: 11741
	private static EffectManager m_effectManager;

	// Token: 0x04002DDE RID: 11742
	private Action<MonoBehaviour, EventArgs> m_disableExitRoomEffects;

	// Token: 0x04002DDF RID: 11743
	private static object[] m_effectArgs;

	// Token: 0x04002DE0 RID: 11744
	private static string m_effectArgsName;
}
