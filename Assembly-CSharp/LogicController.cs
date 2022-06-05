using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000260 RID: 608
[RequireComponent(typeof(EnemyController))]
[Serializable]
public class LogicController : MonoBehaviour
{
	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x060017C6 RID: 6086 RVA: 0x00049C66 File Offset: 0x00047E66
	// (set) Token: 0x060017C7 RID: 6087 RVA: 0x00049C6E File Offset: 0x00047E6E
	public bool DisableRestLogicInterrupt { get; set; }

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x060017C8 RID: 6088 RVA: 0x00049C77 File Offset: 0x00047E77
	// (set) Token: 0x060017C9 RID: 6089 RVA: 0x00049C7F File Offset: 0x00047E7F
	public bool DisableDamageDuringInitialDelay { get; set; } = true;

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x060017CA RID: 6090 RVA: 0x00049C88 File Offset: 0x00047E88
	// (set) Token: 0x060017CB RID: 6091 RVA: 0x00049C90 File Offset: 0x00047E90
	public bool ExecuteLogicInAir { get; set; } = true;

	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x060017CC RID: 6092 RVA: 0x00049C99 File Offset: 0x00047E99
	public bool LogicIsActivated
	{
		get
		{
			return this.m_logicIsActivated && base.enabled;
		}
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x00049CAB File Offset: 0x00047EAB
	public void OverrideLogicDelay(float delayOverride)
	{
		this.m_logicDelayYield.CreateNew(delayOverride, false);
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x060017CE RID: 6094 RVA: 0x00049CBA File Offset: 0x00047EBA
	// (set) Token: 0x060017CF RID: 6095 RVA: 0x00049CC2 File Offset: 0x00047EC2
	public bool DisableLogicActivationByDistance { get; set; }

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00049CCB File Offset: 0x00047ECB
	public bool IsAggroed
	{
		get
		{
			return this.m_aggroTimer > 0f;
		}
	}

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00049CDA File Offset: 0x00047EDA
	// (set) Token: 0x060017D2 RID: 6098 RVA: 0x00049CE2 File Offset: 0x00047EE2
	public string ForceExecuteLogicBlockName_OnceOnly { get; set; }

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00049CEB File Offset: 0x00047EEB
	// (set) Token: 0x060017D4 RID: 6100 RVA: 0x00049CF3 File Offset: 0x00047EF3
	public string ForceExecuteLogicBlockName
	{
		get
		{
			return this.m_forceExecuteLogicBlockName;
		}
		set
		{
			this.m_forceExecuteLogicBlockName = value;
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00049CFC File Offset: 0x00047EFC
	public string PreviousLogicBlockName
	{
		get
		{
			return this.m_previousLogicBlockName;
		}
	}

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x060017D6 RID: 6102 RVA: 0x00049D04 File Offset: 0x00047F04
	// (set) Token: 0x060017D7 RID: 6103 RVA: 0x00049D0C File Offset: 0x00047F0C
	public EnemyLogicType EnemyLogicType
	{
		get
		{
			return this.m_enemyLogicType;
		}
		set
		{
			this.m_enemyLogicType = value;
		}
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x060017D8 RID: 6104 RVA: 0x00049D15 File Offset: 0x00047F15
	// (set) Token: 0x060017D9 RID: 6105 RVA: 0x00049D1D File Offset: 0x00047F1D
	public bool PrintDebug
	{
		get
		{
			return this.m_printDebug;
		}
		set
		{
			this.m_printDebug = value;
		}
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x060017DA RID: 6106 RVA: 0x00049D26 File Offset: 0x00047F26
	public bool IsExecutingLogic
	{
		get
		{
			return this.m_isExecuting;
		}
	}

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x060017DB RID: 6107 RVA: 0x00049D2E File Offset: 0x00047F2E
	// (set) Token: 0x060017DC RID: 6108 RVA: 0x00049D36 File Offset: 0x00047F36
	public float CurrentLogicBlockPercentChance { get; private set; }

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x060017DD RID: 6109 RVA: 0x00049D3F File Offset: 0x00047F3F
	public string CurrentLogicBlockName
	{
		get
		{
			return this.m_currentLogicBlockName;
		}
	}

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x060017DE RID: 6110 RVA: 0x00049D47 File Offset: 0x00047F47
	// (set) Token: 0x060017DF RID: 6111 RVA: 0x00049D4F File Offset: 0x00047F4F
	public LogicState CurrentRangeState
	{
		get
		{
			return this.m_currentLogicState;
		}
		private set
		{
			this.m_currentLogicState = value;
		}
	}

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x060017E0 RID: 6112 RVA: 0x00049D58 File Offset: 0x00047F58
	public float CloseRange
	{
		get
		{
			if (this.m_enemyController != null)
			{
				return this.m_enemyController.CloseRangeRadius;
			}
			return 0f;
		}
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x060017E1 RID: 6113 RVA: 0x00049D79 File Offset: 0x00047F79
	public float MediumRange
	{
		get
		{
			if (this.m_enemyController != null)
			{
				return this.m_enemyController.MediumRangeRadius;
			}
			return 0f;
		}
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x060017E2 RID: 6114 RVA: 0x00049D9A File Offset: 0x00047F9A
	public float FarRange
	{
		get
		{
			if (this.m_enemyController != null)
			{
				return this.m_enemyController.FarRangeRadius;
			}
			return 0f;
		}
	}

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x060017E3 RID: 6115 RVA: 0x00049DBB File Offset: 0x00047FBB
	public LogicController_SO LogicControllerSO
	{
		get
		{
			return this.m_logicControllerSO;
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x060017E4 RID: 6116 RVA: 0x00049DC3 File Offset: 0x00047FC3
	public float RestCounter
	{
		get
		{
			return this.m_restStateCounter;
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x060017E5 RID: 6117 RVA: 0x00049DCB File Offset: 0x00047FCB
	public BaseAIScript LogicScript
	{
		get
		{
			return this.m_logicScript;
		}
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x060017E6 RID: 6118 RVA: 0x00049DD3 File Offset: 0x00047FD3
	public GameObject Player
	{
		get
		{
			return PlayerManager.GetPlayer();
		}
	}

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00049DDA File Offset: 0x00047FDA
	public PlayerController PlayerController
	{
		get
		{
			return PlayerManager.GetPlayerController();
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00049DE1 File Offset: 0x00047FE1
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x060017E9 RID: 6121 RVA: 0x00049DE9 File Offset: 0x00047FE9
	// (set) Token: 0x060017EA RID: 6122 RVA: 0x00049DF1 File Offset: 0x00047FF1
	public Func<float, bool> IsInRange { get; set; }

	// Token: 0x060017EB RID: 6123 RVA: 0x00049DFC File Offset: 0x00047FFC
	public void Awake()
	{
		if (Application.isPlaying)
		{
			this.UpdateLogicReferences();
			if (!this.LogicScript)
			{
				return;
			}
			if (!this.LogicControllerSO)
			{
				return;
			}
			this.m_logicScript = UnityEngine.Object.Instantiate<BaseAIScript>(this.LogicScript, base.transform);
			this.m_cooldownLogicTimersDict = new Dictionary<string, float>();
			this.PopulateStaticMethodInfoDict(this.EnemyLogicType, this.LogicScript.GetType());
			if (!this.m_enemyController)
			{
				this.m_enemyController = base.GetComponentInChildren<EnemyController>();
			}
			this.m_logicDelayYield = new WaitRL_Yield(0.175f, false);
			if (!GameUtility.IsInGame && !GameUtility.IsInLevelEditor)
			{
				this.m_isDelayed = false;
			}
			this.IsInRange = new Func<float, bool>(this.IsInRange_Standard);
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x00049EC0 File Offset: 0x000480C0
	private void CreateDebugAttackList(LogicState logicState)
	{
		foreach (KeyValuePair<string, int> keyValuePair in this.GetMethodNameDictionary(this.EnemyLogicType, logicState))
		{
			if (keyValuePair.Value > 0 && !this.m_debugAttackList.Contains(keyValuePair.Key))
			{
				this.m_debugAttackList.Add(keyValuePair.Key);
			}
		}
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x00049F44 File Offset: 0x00048144
	private IEnumerator Start()
	{
		while (!this.m_enemyController.IsInitialized)
		{
			yield return null;
		}
		this.LogicScript.Initialize(this.m_enemyController);
		this.m_enemyController.CharacterHitResponse.OnCharacterHitRelay.AddListener(new Action<object, CharacterHitEventArgs>(this.TriggerAggro), false);
		this.m_isInitialized = true;
		this.AssignAnimParamRank();
		yield break;
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x00049F53 File Offset: 0x00048153
	private void OnEnable()
	{
		this.AssignAnimParamRank();
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x00049F5C File Offset: 0x0004815C
	public void AssignAnimParamRank()
	{
		if (this.m_enemyController.IsInitialized)
		{
			if (global::AnimatorUtility.HasParameter(this.m_enemyController.Animator, LogicController.RANK_STRING_HASH))
			{
				this.m_enemyController.Animator.SetInteger(LogicController.RANK_STRING_HASH, (int)this.m_enemyController.EnemyRank);
			}
			if (global::AnimatorUtility.HasParameter(this.m_enemyController.Animator, LogicController.ISBOSS_STRING_HASH))
			{
				this.m_enemyController.Animator.SetBool(LogicController.ISBOSS_STRING_HASH, this.m_enemyController.IsBoss);
			}
		}
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x00049FE4 File Offset: 0x000481E4
	private void OnDestroy()
	{
		this.m_enemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.TriggerAggro));
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x0004A008 File Offset: 0x00048208
	private IEnumerator DelayLogic()
	{
		this.m_isDelayed = true;
		yield return this.m_waitFixedUpdateYield;
		yield return null;
		if (this.m_logicDelayYield.WaitTime > 0f)
		{
			if (this.DisableDamageDuringInitialDelay && !this.m_hitboxesDisabled)
			{
				this.m_enemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
				this.m_hitboxesDisabled = true;
			}
			this.m_logicDelayYield.CreateNew(this.m_logicDelayYield.WaitTime, false);
			yield return this.m_logicDelayYield;
			if (this.DisableDamageDuringInitialDelay && this.m_hitboxesDisabled)
			{
				this.m_enemyController.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
				this.m_hitboxesDisabled = false;
			}
		}
		this.m_isDelayed = false;
		this.m_isExecuting = false;
		yield break;
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x0004A018 File Offset: 0x00048218
	private void OnDisable()
	{
		if (this.m_delayLogicCoroutine != null)
		{
			base.StopCoroutine(this.m_delayLogicCoroutine);
			this.m_delayLogicCoroutine = null;
		}
		this.m_isDelayed = false;
		this.m_hitboxesDisabled = false;
		this.m_logicIsActivated = false;
		this.m_aggroTimer = 0f;
		this.m_enemyController.DeactivateEnemy();
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x0004A06B File Offset: 0x0004826B
	public void SetLogicBlockEnabled(string logicBlockName, bool enabled)
	{
		if (enabled)
		{
			if (this.m_disabledLogicBlocksList.Contains(logicBlockName))
			{
				this.m_disabledLogicBlocksList.Remove(logicBlockName);
				return;
			}
		}
		else if (!this.m_disabledLogicBlocksList.Contains(logicBlockName))
		{
			this.m_disabledLogicBlocksList.Add(logicBlockName);
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0004A0A6 File Offset: 0x000482A6
	public void TriggerAggro(object sender, EventArgs args)
	{
		this.m_aggroTimer = Enemy_EV.MIN_AGGRO_DURATION;
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x0004A0B4 File Offset: 0x000482B4
	public void UpdateLogicReferences()
	{
		if (Application.isPlaying && this.m_logicScript && this.m_logicControllerSO)
		{
			return;
		}
		if (!this.m_enemyController)
		{
			this.m_enemyController = base.GetComponent<EnemyController>();
		}
		EnemyClassData enemyClassData = EnemyClassLibrary.GetEnemyClassData(this.m_enemyController.EnemyType);
		if (enemyClassData)
		{
			this.m_logicScript = enemyClassData.GetAIScript(this.m_enemyController.EnemyRank);
			this.m_logicControllerSO = enemyClassData.GetLogicController();
			this.EnemyLogicType = this.ConvertRankToLogicType(this.m_enemyController.EnemyRank);
		}
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0004A14F File Offset: 0x0004834F
	private EnemyLogicType ConvertRankToLogicType(EnemyRank rank)
	{
		switch (rank)
		{
		default:
			return EnemyLogicType.Basic;
		case EnemyRank.Advanced:
			return EnemyLogicType.Advanced;
		case EnemyRank.Expert:
			return EnemyLogicType.Expert;
		case EnemyRank.Miniboss:
			return EnemyLogicType.Miniboss;
		}
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x0004A170 File Offset: 0x00048370
	private Type GetLogicStateAttributeType(LogicState logicState)
	{
		switch (logicState)
		{
		case LogicState.Close:
			return typeof(CloseLogicAttribute);
		case LogicState.Medium:
			return typeof(MediumLogicAttribute);
		case LogicState.Far:
			return typeof(FarLogicAttribute);
		case LogicState.Wander:
			return typeof(WanderLogicAttribute);
		}
		return typeof(RestLogicAttribute);
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0004A1D4 File Offset: 0x000483D4
	public StringInt_Dictionary GetMethodNameDictionary(EnemyLogicType enemyLogicType, LogicState logicState)
	{
		if (!this.LogicControllerSO)
		{
			return null;
		}
		switch (enemyLogicType)
		{
		case EnemyLogicType.Basic:
			switch (logicState)
			{
			case LogicState.Close:
				return this.LogicControllerSO.BasicCloseLogic;
			case LogicState.Medium:
				return this.LogicControllerSO.BasicMediumLogic;
			case LogicState.Far:
				return this.LogicControllerSO.BasicFarLogic;
			case LogicState.Resting:
				return this.LogicControllerSO.BasicRestLogic;
			case LogicState.Wander:
				return this.LogicControllerSO.BasicWanderLogic;
			}
			break;
		case EnemyLogicType.Advanced:
			switch (logicState)
			{
			case LogicState.Close:
				return this.LogicControllerSO.AdvancedCloseLogic;
			case LogicState.Medium:
				return this.LogicControllerSO.AdvancedMediumLogic;
			case LogicState.Far:
				return this.LogicControllerSO.AdvancedFarLogic;
			case LogicState.Resting:
				return this.LogicControllerSO.AdvancedRestLogic;
			case LogicState.Wander:
				return this.LogicControllerSO.AdvancerWanderLogic;
			}
			break;
		case EnemyLogicType.Expert:
			switch (logicState)
			{
			case LogicState.Close:
				return this.LogicControllerSO.ExpertCloseLogic;
			case LogicState.Medium:
				return this.LogicControllerSO.ExpertMediumLogic;
			case LogicState.Far:
				return this.LogicControllerSO.ExpertFarLogic;
			case LogicState.Resting:
				return this.LogicControllerSO.ExpertRestLogic;
			case LogicState.Wander:
				return this.LogicControllerSO.ExpertWanderLogic;
			}
			break;
		case EnemyLogicType.Miniboss:
			switch (logicState)
			{
			case LogicState.Close:
				return this.LogicControllerSO.MinibossCloseLogic;
			case LogicState.Medium:
				return this.LogicControllerSO.MinibossMediumLogic;
			case LogicState.Far:
				return this.LogicControllerSO.MinibossFarLogic;
			case LogicState.Resting:
				return this.LogicControllerSO.MinibossRestLogic;
			case LogicState.Wander:
				return this.LogicControllerSO.MinibossWanderLogic;
			}
			break;
		}
		return null;
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x0004A380 File Offset: 0x00048580
	private static StringMethodInfo_Dictionary GetMethodInfoDictionary(Type logicType, LogicState logicState)
	{
		Dictionary<LogicState, StringMethodInfo_Dictionary> dictionary;
		if (!LogicController.m_staticMethodInfoDict.TryGetValue(logicType, out dictionary))
		{
			throw new Exception("Could not find method info dict for AIScript type: " + ((logicType != null) ? logicType.ToString() : null) + " with logic state:" + logicState.ToString());
		}
		StringMethodInfo_Dictionary result;
		if (dictionary.TryGetValue(logicState, out result))
		{
			return result;
		}
		throw new Exception("Could not find method info dict for AIScript type: " + ((logicType != null) ? logicType.ToString() : null) + " with logic state:" + logicState.ToString());
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x0004A408 File Offset: 0x00048608
	public int GetLogicBlockOddsOverride(LogicState rangeState, string logicBlockName)
	{
		Dictionary<string, int> dictionary;
		int result;
		if (this.m_logicBlockOddsOverrideDict.TryGetValue(rangeState, out dictionary) && dictionary.TryGetValue(logicBlockName, out result))
		{
			return result;
		}
		return -1;
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x0004A434 File Offset: 0x00048634
	public void ChangeLogicBlockOdds(LogicState rangeState, string logicBlockName, int newOdds)
	{
		if (this.GetMethodNameDictionary(this.EnemyLogicType, rangeState).ContainsKey(logicBlockName))
		{
			Dictionary<string, int> dictionary;
			if (!this.m_logicBlockOddsOverrideDict.TryGetValue(rangeState, out dictionary))
			{
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				this.m_logicBlockOddsOverrideDict.Add(rangeState, dictionary2);
				dictionary = dictionary2;
			}
			if (dictionary.ContainsKey(logicBlockName))
			{
				dictionary[logicBlockName] = newOdds;
				return;
			}
			dictionary.Add(logicBlockName, newOdds);
		}
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x0004A498 File Offset: 0x00048698
	public void ResetLogicBlockOddsOverrides(LogicState rangeState, string logicBlockName)
	{
		Dictionary<string, int> dictionary;
		if (!this.m_logicBlockOddsOverrideDict.TryGetValue(rangeState, out dictionary))
		{
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			this.m_logicBlockOddsOverrideDict.Add(rangeState, dictionary2);
			dictionary = dictionary2;
		}
		if (dictionary.ContainsKey(logicBlockName))
		{
			dictionary.Remove(logicBlockName);
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x0004A4DC File Offset: 0x000486DC
	public void ResetAllLogicBlockOddsOverrides()
	{
		foreach (KeyValuePair<LogicState, Dictionary<string, int>> keyValuePair in this.m_logicBlockOddsOverrideDict)
		{
			keyValuePair.Value.Clear();
		}
		this.m_logicBlockOddsOverrideDict.Clear();
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x0004A540 File Offset: 0x00048740
	public void InternalUpdate()
	{
		if (!this.IsInitialized || !this.LogicScript.IsInitialized || !this.m_enemyController.IsInitialized)
		{
			return;
		}
		if (this.LogicScript.IsPaused)
		{
			return;
		}
		if (this.m_isExecutingDeathLogic)
		{
			return;
		}
		this.UpdateLogicActivation();
		if (this.m_hasNoLogic)
		{
			return;
		}
		if (!this.m_logicIsActivated)
		{
			return;
		}
		if (this.m_enemyController != null && this.m_enemyController.ConditionState != CharacterStates.CharacterConditions.Normal && this.m_enemyController.ConditionState != CharacterStates.CharacterConditions.DisableHorizontalMovement)
		{
			if (this.IsExecutingLogic)
			{
				this.StopAllLogic(false);
			}
			return;
		}
		if (Application.isPlaying)
		{
			this.UpdateLogicState(Time.deltaTime);
			this.UpdateLBCooldowns(Time.deltaTime);
			this.UpdateAggroTimer(Time.deltaTime);
			if (!this.m_isExecuting && !this.m_isDelayed && (this.m_enemyController.IsGrounded || this.m_enemyController.IsFlying || (!this.m_enemyController.IsGrounded && this.ExecuteLogicInAir)))
			{
				this.StopAllLogic(false);
				string forcedMethodName = string.IsNullOrEmpty(this.ForceExecuteLogicBlockName_OnceOnly) ? this.ForceExecuteLogicBlockName : this.ForceExecuteLogicBlockName_OnceOnly;
				this.ForceExecuteLogicBlockName_OnceOnly = null;
				if (this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Dizzy))
				{
					forcedMethodName = "Idle";
				}
				base.StartCoroutine(this.ExecuteLogic(this.m_currentLogicState, forcedMethodName));
			}
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0004A6A4 File Offset: 0x000488A4
	private void UpdateLogicActivation()
	{
		if (this.m_enemyController && (this.m_enemyController.ForceActivate || this.IsAggroed))
		{
			if (!this.m_logicIsActivated)
			{
				this.m_reactivationTimeOutTimer = 0f;
				this.m_logicIsActivated = true;
				this.m_enemyController.ActivateEnemy();
				this.m_logicScript.OnEnemyActivated();
			}
			return;
		}
		if (this.IsAggroed)
		{
			return;
		}
		if (this.DisableLogicActivationByDistance)
		{
			return;
		}
		Vector3 position = CameraController.GameCamera.transform.position;
		float num = CameraController.GameCamera.orthographicSize * CameraController.GameCamera.aspect;
		float num2 = CameraController.GameCamera.orthographicSize;
		Vector3 midpoint = this.m_enemyController.Midpoint;
		if (!this.m_logicIsActivated)
		{
			num += 9f;
			num2 += 5.5f;
			if (midpoint.x > position.x - num && midpoint.x < position.x + num && midpoint.y > position.y - num2 && midpoint.y < position.y + num2)
			{
				this.m_reactivationTimeOutTimer = 0f;
				this.m_logicIsActivated = true;
				this.m_enemyController.ActivateEnemy();
				this.m_logicScript.OnEnemyActivated();
				return;
			}
		}
		else if (this.CurrentLogicBlockName == "Idle" && !this.m_enemyController.IsBoss)
		{
			num += 16f;
			num2 += 12f;
			if (midpoint.x > position.x + num || midpoint.x < position.x - num || midpoint.y > position.y + num2 || midpoint.y < position.y - num2)
			{
				this.m_reactivationTimeOutTimer = 5f;
				this.m_logicIsActivated = false;
				this.m_enemyController.DeactivateEnemy();
			}
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0004A870 File Offset: 0x00048A70
	private void UpdateLBCooldowns(float elapsedTime)
	{
		LogicController.m_cooldownReductionHelper.Clear();
		foreach (KeyValuePair<string, float> keyValuePair in this.m_cooldownLogicTimersDict)
		{
			if (this.m_cooldownLogicTimersDict[keyValuePair.Key] > 0f)
			{
				LogicController.m_cooldownReductionHelper.Add(keyValuePair.Key);
			}
		}
		foreach (string text in LogicController.m_cooldownReductionHelper)
		{
			Dictionary<string, float> cooldownLogicTimersDict = this.m_cooldownLogicTimersDict;
			string key = text;
			cooldownLogicTimersDict[key] -= elapsedTime;
		}
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0004A944 File Offset: 0x00048B44
	private void UpdateAggroTimer(float elapsedTime)
	{
		if (this.m_aggroTimer > 0f)
		{
			this.m_aggroTimer -= elapsedTime;
		}
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0004A964 File Offset: 0x00048B64
	private void UpdateLogicState(float elapsedTime)
	{
		if (this.IsAggroed && this.m_currentLogicState == LogicState.Wander)
		{
			this.m_currentLogicState = LogicState.Far;
			return;
		}
		if (this.m_currentLogicState == LogicState.Resting)
		{
			this.m_restStateCounter -= elapsedTime;
			if (this.m_restStateCounter > 0f)
			{
				return;
			}
			if (!this.DisableRestLogicInterrupt)
			{
				this.m_currentLogicState = LogicState.None;
				this.StopAllLogic(false);
			}
		}
		if (PlayerManager.IsInstantiated)
		{
			if (this.IsInRange(this.CloseRange))
			{
				this.m_currentLogicState = LogicState.Close;
				return;
			}
			if (this.IsInRange(this.MediumRange))
			{
				this.m_currentLogicState = LogicState.Medium;
				return;
			}
			if (this.IsInRange(this.FarRange))
			{
				this.m_currentLogicState = LogicState.Far;
				return;
			}
			this.m_currentLogicState = LogicState.Wander;
		}
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x0004AA28 File Offset: 0x00048C28
	private bool IsInRange_Standard(float rangeRadius)
	{
		float num = rangeRadius * 0.6f;
		this.m_rangeRect.width = rangeRadius * 2f;
		this.m_rangeRect.height = num * 2f;
		this.m_rangeRect.x = this.m_enemyController.Midpoint.x - rangeRadius;
		this.m_rangeRect.y = this.m_enemyController.Midpoint.y - num;
		return this.m_rangeRect.Contains(PlayerManager.GetPlayerController().Midpoint);
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x0004AAB5 File Offset: 0x00048CB5
	private IEnumerator ExecuteLogic(LogicState logicState, string forcedMethodName)
	{
		StringInt_Dictionary methodNameDictionary = this.GetMethodNameDictionary(this.EnemyLogicType, logicState);
		StringMethodInfo_Dictionary methodInfoDictionary = LogicController.GetMethodInfoDictionary(this.LogicScript.GetType(), logicState);
		string text = null;
		int num = 0;
		if (!string.IsNullOrEmpty(forcedMethodName))
		{
			if (methodInfoDictionary.ContainsKey(forcedMethodName))
			{
				text = forcedMethodName;
				this.CurrentLogicBlockPercentChance = 1f;
			}
			else
			{
				Debug.Log("<color=red>Cannot run forced logic block: " + forcedMethodName + ". Logic block does not exist.  Please make sure to disable this before pushing.</color>");
			}
		}
		else
		{
			foreach (KeyValuePair<string, int> keyValuePair in methodNameDictionary)
			{
				bool flag = this.m_disabledLogicBlocksList.Contains(keyValuePair.Key);
				bool flag2 = this.m_cooldownLogicTimersDict.ContainsKey(keyValuePair.Key);
				if (!flag && (!flag2 || (flag2 && this.m_cooldownLogicTimersDict[keyValuePair.Key] <= 0f)))
				{
					int num2 = this.GetLogicBlockOddsOverride(logicState, keyValuePair.Key);
					if (num2 == -1)
					{
						num2 = keyValuePair.Value;
					}
					num += num2;
				}
			}
			int num3 = UnityEngine.Random.Range(1, num + 1);
			if (this.PrintDebug)
			{
				MonoBehaviour.print(string.Concat(new string[]
				{
					"Choosing logic to execute from ",
					this.m_currentLogicState.ToString().ToUpper(),
					" state. Roll is: ",
					num3.ToString(),
					" out of ",
					num.ToString()
				}));
			}
			int num4 = 0;
			foreach (KeyValuePair<string, int> keyValuePair2 in methodNameDictionary)
			{
				if ((!this.m_cooldownLogicTimersDict.ContainsKey(keyValuePair2.Key) || this.m_cooldownLogicTimersDict[keyValuePair2.Key] <= 0f) && !this.m_disabledLogicBlocksList.Contains(keyValuePair2.Key))
				{
					int num5 = this.GetLogicBlockOddsOverride(logicState, keyValuePair2.Key);
					if (num5 == -1)
					{
						num5 = keyValuePair2.Value;
					}
					if (num5 > 0)
					{
						num4 += num5;
						if (num3 <= num4)
						{
							text = keyValuePair2.Key;
							this.CurrentLogicBlockPercentChance = (float)num5 / (float)num;
							break;
						}
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (this.PrintDebug)
			{
				MonoBehaviour.print("Logic choice made. Executing logic: " + text);
			}
			if (!this.m_hasExecutedBefore)
			{
				text = "Idle";
				this.m_hasExecutedBefore = true;
			}
			this.m_currentLogicBlockName = text;
			this.m_isExecuting = true;
			IEnumerator enumerator2 = (IEnumerator)methodInfoDictionary[text].Invoke(this.LogicScript, null);
			yield return enumerator2;
			this.m_isExecuting = false;
			this.m_previousLogicBlockName = this.m_currentLogicBlockName;
			this.m_currentLogicBlockName = null;
			if (this.PrintDebug)
			{
				MonoBehaviour.print("Logic execution complete...");
			}
		}
		else if (this.PrintDebug)
		{
			MonoBehaviour.print("Cannot choose logic to execute. Ensure " + this.m_currentLogicState.ToString().ToUpper() + " state has assigned logic, and the chance of executing is greater than 0%");
		}
		yield break;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x0004AAD2 File Offset: 0x00048CD2
	public void TriggerDeath()
	{
		this.m_isExecutingDeathLogic = true;
		this.m_enemyController.HitboxController.gameObject.SetActive(false);
		this.StopAllLogic(false);
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x0004AAF8 File Offset: 0x00048CF8
	public void StopAllLogic(bool resetCurrentState = false)
	{
		if (!this.IsInitialized)
		{
			return;
		}
		this.m_isExecuting = false;
		this.LogicScript.StopAllCoroutines();
		this.LogicScript.OnLBCompleteOrCancelled();
		base.StopAllCoroutines();
		if (resetCurrentState)
		{
			this.m_currentLogicState = LogicState.None;
		}
		if (this.m_isDelayed && base.isActiveAndEnabled && !this.m_isExecutingDeathLogic)
		{
			this.m_delayLogicCoroutine = base.StartCoroutine(this.DelayLogic());
		}
		this.m_enemyController.AttackingWithContactDamage = false;
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0004AB74 File Offset: 0x00048D74
	public void SetLBCooldown(string logicBlockName, float cooldown, bool ignoreMods)
	{
		if (!ignoreMods)
		{
			float num = BurdenManager.GetBurdenStatGain(BurdenType.EnemyAggression);
			if (this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed) || this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorShred))
			{
				num += 0.25f;
			}
			num = Mathf.Max(1f - num, 0.25f);
			cooldown *= num;
		}
		if (!this.m_cooldownLogicTimersDict.ContainsKey(logicBlockName))
		{
			this.m_cooldownLogicTimersDict.Add(logicBlockName, cooldown);
		}
		this.m_cooldownLogicTimersDict[logicBlockName] = cooldown;
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0004AC04 File Offset: 0x00048E04
	public void EnterRestState(float duration, bool ignoreMods)
	{
		this.m_currentLogicState = LogicState.Resting;
		float num = BurdenManager.GetBurdenStatGain(BurdenType.EnemyAggression);
		if (!ignoreMods)
		{
			if (this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_Speed) || this.m_enemyController.StatusEffectController.HasStatusEffect(StatusEffectType.Enemy_ArmorShred))
			{
				num += 0.25f;
			}
			num = Mathf.Max(1f - num, 0.25f);
			duration *= num;
		}
		this.m_restStateCounter = duration;
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0004AC78 File Offset: 0x00048E78
	private Type GetEnemyLogicTypeAttribute(EnemyLogicType logicType)
	{
		switch (logicType)
		{
		default:
			return typeof(BasicEnemyAttribute);
		case EnemyLogicType.Advanced:
			return typeof(AdvancedEnemyAttribute);
		case EnemyLogicType.Expert:
			return typeof(ExpertEnemyAttribute);
		case EnemyLogicType.Miniboss:
			return typeof(MinibossEnemyAttribute);
		}
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0004ACC8 File Offset: 0x00048EC8
	private void PopulateStaticMethodInfoDict(EnemyLogicType enemyLogicType, Type logicScriptType)
	{
		if (LogicController.m_staticMethodInfoDict.ContainsKey(logicScriptType))
		{
			return;
		}
		LogicController.m_staticMethodInfoDict.Add(logicScriptType, new Dictionary<LogicState, StringMethodInfo_Dictionary>());
		MethodInfo[] methods = logicScriptType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		StringInt_Dictionary methodNameDictionary = this.GetMethodNameDictionary(enemyLogicType, LogicState.Close);
		StringInt_Dictionary methodNameDictionary2 = this.GetMethodNameDictionary(enemyLogicType, LogicState.Medium);
		StringInt_Dictionary methodNameDictionary3 = this.GetMethodNameDictionary(enemyLogicType, LogicState.Far);
		StringInt_Dictionary methodNameDictionary4 = this.GetMethodNameDictionary(enemyLogicType, LogicState.Wander);
		StringInt_Dictionary methodNameDictionary5 = this.GetMethodNameDictionary(enemyLogicType, LogicState.Resting);
		Dictionary<LogicState, StringMethodInfo_Dictionary> dictionary = LogicController.m_staticMethodInfoDict[logicScriptType];
		StringMethodInfo_Dictionary stringMethodInfo_Dictionary = dictionary[LogicState.Close] = new StringMethodInfo_Dictionary();
		StringMethodInfo_Dictionary stringMethodInfo_Dictionary2 = dictionary[LogicState.Medium] = new StringMethodInfo_Dictionary();
		StringMethodInfo_Dictionary stringMethodInfo_Dictionary3 = dictionary[LogicState.Far] = new StringMethodInfo_Dictionary();
		StringMethodInfo_Dictionary stringMethodInfo_Dictionary4 = dictionary[LogicState.Wander] = new StringMethodInfo_Dictionary();
		StringMethodInfo_Dictionary stringMethodInfo_Dictionary5 = dictionary[LogicState.Resting] = new StringMethodInfo_Dictionary();
		foreach (MethodInfo methodInfo in methods)
		{
			string name = methodInfo.Name;
			if (methodNameDictionary.ContainsKey(name) && !stringMethodInfo_Dictionary.ContainsKey(name))
			{
				stringMethodInfo_Dictionary.Add(name, methodInfo);
			}
			if (methodNameDictionary2.ContainsKey(name) && !stringMethodInfo_Dictionary2.ContainsKey(name))
			{
				stringMethodInfo_Dictionary2.Add(name, methodInfo);
			}
			if (methodNameDictionary3.ContainsKey(name) && !stringMethodInfo_Dictionary3.ContainsKey(name))
			{
				stringMethodInfo_Dictionary3.Add(name, methodInfo);
			}
			if (methodNameDictionary4.ContainsKey(name) && !stringMethodInfo_Dictionary4.ContainsKey(name))
			{
				stringMethodInfo_Dictionary4.Add(name, methodInfo);
			}
			if (methodNameDictionary5.ContainsKey(name) && !stringMethodInfo_Dictionary5.ContainsKey(name))
			{
				stringMethodInfo_Dictionary5.Add(name, methodInfo);
			}
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x0004AE5E File Offset: 0x0004905E
	public void Pause()
	{
		this.LogicScript.Pause();
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x0004AE6B File Offset: 0x0004906B
	public void Unpause()
	{
		this.LogicScript.Unpause();
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x0004AE78 File Offset: 0x00049078
	public void ResetLogic()
	{
		if (!this.IsInitialized)
		{
			return;
		}
		this.m_restStateCounter = 0f;
		this.m_currentLogicState = LogicState.Wander;
		this.StopAllLogic(false);
		this.LogicScript.ResetScript();
		this.LogicScript.Unpause();
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			base.StartCoroutine(this.ExecuteLogic(LogicState.Close, "Idle"));
			this.m_delayLogicCoroutine = base.StartCoroutine(this.DelayLogic());
		}
		this.m_isExecutingDeathLogic = false;
		this.ResetAllLogicBlockOddsOverrides();
		if (this.m_enemyController && this.m_enemyController.HitboxController != null && !this.m_enemyController.HitboxController.gameObject.activeInHierarchy)
		{
			this.m_enemyController.HitboxController.gameObject.SetActive(true);
		}
	}

	// Token: 0x0400174A RID: 5962
	[SerializeField]
	private bool m_hasNoLogic;

	// Token: 0x0400174B RID: 5963
	[SerializeField]
	[HideInInspector]
	private string m_forceExecuteLogicBlockName;

	// Token: 0x0400174C RID: 5964
	[SerializeField]
	[HideInInspector]
	private bool m_printDebug;

	// Token: 0x0400174D RID: 5965
	private BaseAIScript m_logicScript;

	// Token: 0x0400174E RID: 5966
	private LogicController_SO m_logicControllerSO;

	// Token: 0x0400174F RID: 5967
	private EnemyLogicType m_enemyLogicType;

	// Token: 0x04001750 RID: 5968
	private bool m_isExecuting;

	// Token: 0x04001751 RID: 5969
	private string m_currentLogicBlockName;

	// Token: 0x04001752 RID: 5970
	private string m_previousLogicBlockName;

	// Token: 0x04001753 RID: 5971
	private bool m_isInitialized;

	// Token: 0x04001754 RID: 5972
	private LogicState m_currentLogicState;

	// Token: 0x04001755 RID: 5973
	private float m_restStateCounter;

	// Token: 0x04001756 RID: 5974
	private bool m_hasExecutedBefore;

	// Token: 0x04001757 RID: 5975
	private float m_iterateUpdateTime;

	// Token: 0x04001758 RID: 5976
	private bool m_logicIsActivated;

	// Token: 0x04001759 RID: 5977
	private float m_reactivationTimeOutTimer;

	// Token: 0x0400175A RID: 5978
	private float m_aggroTimer;

	// Token: 0x0400175B RID: 5979
	private EnemyController m_enemyController;

	// Token: 0x0400175C RID: 5980
	private bool m_isExecutingDeathLogic;

	// Token: 0x0400175D RID: 5981
	private Dictionary<string, float> m_cooldownLogicTimersDict;

	// Token: 0x0400175E RID: 5982
	private Coroutine m_delayLogicCoroutine;

	// Token: 0x0400175F RID: 5983
	private WaitRL_Yield m_logicDelayYield;

	// Token: 0x04001760 RID: 5984
	private bool m_isDelayed = true;

	// Token: 0x04001761 RID: 5985
	private List<string> m_disabledLogicBlocksList = new List<string>();

	// Token: 0x04001762 RID: 5986
	private List<string> m_debugAttackList;

	// Token: 0x04001763 RID: 5987
	private int m_debugAttackIndex;

	// Token: 0x04001764 RID: 5988
	private Dictionary<LogicState, Dictionary<string, int>> m_logicBlockOddsOverrideDict = new Dictionary<LogicState, Dictionary<string, int>>();

	// Token: 0x0400176C RID: 5996
	private static int RANK_STRING_HASH = Animator.StringToHash("Rank");

	// Token: 0x0400176D RID: 5997
	private static int ISBOSS_STRING_HASH = Animator.StringToHash("IsBoss");

	// Token: 0x0400176E RID: 5998
	private WaitForFixedUpdate m_waitFixedUpdateYield = new WaitForFixedUpdate();

	// Token: 0x0400176F RID: 5999
	private bool m_hitboxesDisabled;

	// Token: 0x04001770 RID: 6000
	private static Dictionary<Type, Dictionary<LogicState, StringMethodInfo_Dictionary>> m_staticMethodInfoDict = new Dictionary<Type, Dictionary<LogicState, StringMethodInfo_Dictionary>>();

	// Token: 0x04001771 RID: 6001
	private static List<string> m_cooldownReductionHelper = new List<string>();

	// Token: 0x04001772 RID: 6002
	private Rect m_rangeRect;
}
