using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MoreMountains.CorgiEngine;
using UnityEngine;

// Token: 0x02000421 RID: 1057
[RequireComponent(typeof(EnemyController))]
[Serializable]
public class LogicController : MonoBehaviour
{
	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x0600218B RID: 8587 RVA: 0x00011E25 File Offset: 0x00010025
	// (set) Token: 0x0600218C RID: 8588 RVA: 0x00011E2D File Offset: 0x0001002D
	public bool DisableRestLogicInterrupt { get; set; }

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x0600218D RID: 8589 RVA: 0x00011E36 File Offset: 0x00010036
	// (set) Token: 0x0600218E RID: 8590 RVA: 0x00011E3E File Offset: 0x0001003E
	public bool DisableDamageDuringInitialDelay { get; set; } = true;

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x0600218F RID: 8591 RVA: 0x00011E47 File Offset: 0x00010047
	// (set) Token: 0x06002190 RID: 8592 RVA: 0x00011E4F File Offset: 0x0001004F
	public bool ExecuteLogicInAir { get; set; } = true;

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x06002191 RID: 8593 RVA: 0x00011E58 File Offset: 0x00010058
	public bool LogicIsActivated
	{
		get
		{
			return this.m_logicIsActivated && base.enabled;
		}
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x00011E6A File Offset: 0x0001006A
	public void OverrideLogicDelay(float delayOverride)
	{
		this.m_logicDelayYield.CreateNew(delayOverride, false);
	}

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x06002193 RID: 8595 RVA: 0x00011E79 File Offset: 0x00010079
	// (set) Token: 0x06002194 RID: 8596 RVA: 0x00011E81 File Offset: 0x00010081
	public bool DisableLogicActivationByDistance { get; set; }

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x06002195 RID: 8597 RVA: 0x00011E8A File Offset: 0x0001008A
	public bool IsAggroed
	{
		get
		{
			return this.m_aggroTimer > 0f;
		}
	}

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x06002196 RID: 8598 RVA: 0x00011E99 File Offset: 0x00010099
	// (set) Token: 0x06002197 RID: 8599 RVA: 0x00011EA1 File Offset: 0x000100A1
	public string ForceExecuteLogicBlockName_OnceOnly { get; set; }

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x06002198 RID: 8600 RVA: 0x00011EAA File Offset: 0x000100AA
	// (set) Token: 0x06002199 RID: 8601 RVA: 0x00011EB2 File Offset: 0x000100B2
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

	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x0600219A RID: 8602 RVA: 0x00011EBB File Offset: 0x000100BB
	public string PreviousLogicBlockName
	{
		get
		{
			return this.m_previousLogicBlockName;
		}
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x0600219B RID: 8603 RVA: 0x00011EC3 File Offset: 0x000100C3
	// (set) Token: 0x0600219C RID: 8604 RVA: 0x00011ECB File Offset: 0x000100CB
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

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x0600219D RID: 8605 RVA: 0x00011ED4 File Offset: 0x000100D4
	// (set) Token: 0x0600219E RID: 8606 RVA: 0x00011EDC File Offset: 0x000100DC
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

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x0600219F RID: 8607 RVA: 0x00011EE5 File Offset: 0x000100E5
	public bool IsExecutingLogic
	{
		get
		{
			return this.m_isExecuting;
		}
	}

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x060021A0 RID: 8608 RVA: 0x00011EED File Offset: 0x000100ED
	// (set) Token: 0x060021A1 RID: 8609 RVA: 0x00011EF5 File Offset: 0x000100F5
	public float CurrentLogicBlockPercentChance { get; private set; }

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x060021A2 RID: 8610 RVA: 0x00011EFE File Offset: 0x000100FE
	public string CurrentLogicBlockName
	{
		get
		{
			return this.m_currentLogicBlockName;
		}
	}

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x060021A3 RID: 8611 RVA: 0x00011F06 File Offset: 0x00010106
	// (set) Token: 0x060021A4 RID: 8612 RVA: 0x00011F0E File Offset: 0x0001010E
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

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x060021A5 RID: 8613 RVA: 0x00011F17 File Offset: 0x00010117
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

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x060021A6 RID: 8614 RVA: 0x00011F38 File Offset: 0x00010138
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

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x060021A7 RID: 8615 RVA: 0x00011F59 File Offset: 0x00010159
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

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x060021A8 RID: 8616 RVA: 0x00011F7A File Offset: 0x0001017A
	public LogicController_SO LogicControllerSO
	{
		get
		{
			return this.m_logicControllerSO;
		}
	}

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x060021A9 RID: 8617 RVA: 0x00011F82 File Offset: 0x00010182
	public float RestCounter
	{
		get
		{
			return this.m_restStateCounter;
		}
	}

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x060021AA RID: 8618 RVA: 0x00011F8A File Offset: 0x0001018A
	public BaseAIScript LogicScript
	{
		get
		{
			return this.m_logicScript;
		}
	}

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x060021AB RID: 8619 RVA: 0x00011F92 File Offset: 0x00010192
	public GameObject Player
	{
		get
		{
			return PlayerManager.GetPlayer();
		}
	}

	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x060021AC RID: 8620 RVA: 0x00011F99 File Offset: 0x00010199
	public PlayerController PlayerController
	{
		get
		{
			return PlayerManager.GetPlayerController();
		}
	}

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x060021AD RID: 8621 RVA: 0x00011FA0 File Offset: 0x000101A0
	public bool IsInitialized
	{
		get
		{
			return this.m_isInitialized;
		}
	}

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x060021AE RID: 8622 RVA: 0x00011FA8 File Offset: 0x000101A8
	// (set) Token: 0x060021AF RID: 8623 RVA: 0x00011FB0 File Offset: 0x000101B0
	public Func<float, bool> IsInRange { get; set; }

	// Token: 0x060021B0 RID: 8624 RVA: 0x000A7B24 File Offset: 0x000A5D24
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

	// Token: 0x060021B1 RID: 8625 RVA: 0x000A7BE8 File Offset: 0x000A5DE8
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

	// Token: 0x060021B2 RID: 8626 RVA: 0x00011FB9 File Offset: 0x000101B9
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

	// Token: 0x060021B3 RID: 8627 RVA: 0x00011FC8 File Offset: 0x000101C8
	private void OnEnable()
	{
		this.AssignAnimParamRank();
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000A7C6C File Offset: 0x000A5E6C
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

	// Token: 0x060021B5 RID: 8629 RVA: 0x00011FD0 File Offset: 0x000101D0
	private void OnDestroy()
	{
		this.m_enemyController.CharacterHitResponse.OnCharacterHitRelay.RemoveListener(new Action<object, CharacterHitEventArgs>(this.TriggerAggro));
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x00011FF4 File Offset: 0x000101F4
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

	// Token: 0x060021B7 RID: 8631 RVA: 0x000A7CF4 File Offset: 0x000A5EF4
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

	// Token: 0x060021B8 RID: 8632 RVA: 0x00012003 File Offset: 0x00010203
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

	// Token: 0x060021B9 RID: 8633 RVA: 0x0001203E File Offset: 0x0001023E
	public void TriggerAggro(object sender, EventArgs args)
	{
		this.m_aggroTimer = Enemy_EV.MIN_AGGRO_DURATION;
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000A7D48 File Offset: 0x000A5F48
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

	// Token: 0x060021BB RID: 8635 RVA: 0x0001204B File Offset: 0x0001024B
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

	// Token: 0x060021BC RID: 8636 RVA: 0x000A7DE4 File Offset: 0x000A5FE4
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

	// Token: 0x060021BD RID: 8637 RVA: 0x000A7E48 File Offset: 0x000A6048
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

	// Token: 0x060021BE RID: 8638 RVA: 0x000A7FF4 File Offset: 0x000A61F4
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

	// Token: 0x060021BF RID: 8639 RVA: 0x000A807C File Offset: 0x000A627C
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

	// Token: 0x060021C0 RID: 8640 RVA: 0x000A80A8 File Offset: 0x000A62A8
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

	// Token: 0x060021C1 RID: 8641 RVA: 0x000A810C File Offset: 0x000A630C
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

	// Token: 0x060021C2 RID: 8642 RVA: 0x000A8150 File Offset: 0x000A6350
	public void ResetAllLogicBlockOddsOverrides()
	{
		foreach (KeyValuePair<LogicState, Dictionary<string, int>> keyValuePair in this.m_logicBlockOddsOverrideDict)
		{
			keyValuePair.Value.Clear();
		}
		this.m_logicBlockOddsOverrideDict.Clear();
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x000A81B4 File Offset: 0x000A63B4
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

	// Token: 0x060021C4 RID: 8644 RVA: 0x000A8318 File Offset: 0x000A6518
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

	// Token: 0x060021C5 RID: 8645 RVA: 0x000A84E4 File Offset: 0x000A66E4
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

	// Token: 0x060021C6 RID: 8646 RVA: 0x0001206A File Offset: 0x0001026A
	private void UpdateAggroTimer(float elapsedTime)
	{
		if (this.m_aggroTimer > 0f)
		{
			this.m_aggroTimer -= elapsedTime;
		}
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x000A85B8 File Offset: 0x000A67B8
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

	// Token: 0x060021C8 RID: 8648 RVA: 0x000A867C File Offset: 0x000A687C
	private bool IsInRange_Standard(float rangeRadius)
	{
		float num = rangeRadius * 0.6f;
		this.m_rangeRect.width = rangeRadius * 2f;
		this.m_rangeRect.height = num * 2f;
		this.m_rangeRect.x = this.m_enemyController.Midpoint.x - rangeRadius;
		this.m_rangeRect.y = this.m_enemyController.Midpoint.y - num;
		return this.m_rangeRect.Contains(PlayerManager.GetPlayerController().Midpoint);
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x00012087 File Offset: 0x00010287
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

	// Token: 0x060021CA RID: 8650 RVA: 0x000120A4 File Offset: 0x000102A4
	public void TriggerDeath()
	{
		this.m_isExecutingDeathLogic = true;
		this.m_enemyController.HitboxController.gameObject.SetActive(false);
		this.StopAllLogic(false);
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x000A870C File Offset: 0x000A690C
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

	// Token: 0x060021CC RID: 8652 RVA: 0x000A8788 File Offset: 0x000A6988
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

	// Token: 0x060021CD RID: 8653 RVA: 0x000A8818 File Offset: 0x000A6A18
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

	// Token: 0x060021CE RID: 8654 RVA: 0x000A888C File Offset: 0x000A6A8C
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

	// Token: 0x060021CF RID: 8655 RVA: 0x000A88DC File Offset: 0x000A6ADC
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

	// Token: 0x060021D0 RID: 8656 RVA: 0x000120CA File Offset: 0x000102CA
	public void Pause()
	{
		this.LogicScript.Pause();
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000120D7 File Offset: 0x000102D7
	public void Unpause()
	{
		this.LogicScript.Unpause();
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000A8A74 File Offset: 0x000A6C74
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

	// Token: 0x04001E71 RID: 7793
	[SerializeField]
	private bool m_hasNoLogic;

	// Token: 0x04001E72 RID: 7794
	[SerializeField]
	[HideInInspector]
	private string m_forceExecuteLogicBlockName;

	// Token: 0x04001E73 RID: 7795
	[SerializeField]
	[HideInInspector]
	private bool m_printDebug;

	// Token: 0x04001E74 RID: 7796
	private BaseAIScript m_logicScript;

	// Token: 0x04001E75 RID: 7797
	private LogicController_SO m_logicControllerSO;

	// Token: 0x04001E76 RID: 7798
	private EnemyLogicType m_enemyLogicType;

	// Token: 0x04001E77 RID: 7799
	private bool m_isExecuting;

	// Token: 0x04001E78 RID: 7800
	private string m_currentLogicBlockName;

	// Token: 0x04001E79 RID: 7801
	private string m_previousLogicBlockName;

	// Token: 0x04001E7A RID: 7802
	private bool m_isInitialized;

	// Token: 0x04001E7B RID: 7803
	private LogicState m_currentLogicState;

	// Token: 0x04001E7C RID: 7804
	private float m_restStateCounter;

	// Token: 0x04001E7D RID: 7805
	private bool m_hasExecutedBefore;

	// Token: 0x04001E7E RID: 7806
	private float m_iterateUpdateTime;

	// Token: 0x04001E7F RID: 7807
	private bool m_logicIsActivated;

	// Token: 0x04001E80 RID: 7808
	private float m_reactivationTimeOutTimer;

	// Token: 0x04001E81 RID: 7809
	private float m_aggroTimer;

	// Token: 0x04001E82 RID: 7810
	private EnemyController m_enemyController;

	// Token: 0x04001E83 RID: 7811
	private bool m_isExecutingDeathLogic;

	// Token: 0x04001E84 RID: 7812
	private Dictionary<string, float> m_cooldownLogicTimersDict;

	// Token: 0x04001E85 RID: 7813
	private Coroutine m_delayLogicCoroutine;

	// Token: 0x04001E86 RID: 7814
	private WaitRL_Yield m_logicDelayYield;

	// Token: 0x04001E87 RID: 7815
	private bool m_isDelayed = true;

	// Token: 0x04001E88 RID: 7816
	private List<string> m_disabledLogicBlocksList = new List<string>();

	// Token: 0x04001E89 RID: 7817
	private List<string> m_debugAttackList;

	// Token: 0x04001E8A RID: 7818
	private int m_debugAttackIndex;

	// Token: 0x04001E8B RID: 7819
	private Dictionary<LogicState, Dictionary<string, int>> m_logicBlockOddsOverrideDict = new Dictionary<LogicState, Dictionary<string, int>>();

	// Token: 0x04001E93 RID: 7827
	private static int RANK_STRING_HASH = Animator.StringToHash("Rank");

	// Token: 0x04001E94 RID: 7828
	private static int ISBOSS_STRING_HASH = Animator.StringToHash("IsBoss");

	// Token: 0x04001E95 RID: 7829
	private WaitForFixedUpdate m_waitFixedUpdateYield = new WaitForFixedUpdate();

	// Token: 0x04001E96 RID: 7830
	private bool m_hitboxesDisabled;

	// Token: 0x04001E97 RID: 7831
	private static Dictionary<Type, Dictionary<LogicState, StringMethodInfo_Dictionary>> m_staticMethodInfoDict = new Dictionary<Type, Dictionary<LogicState, StringMethodInfo_Dictionary>>();

	// Token: 0x04001E98 RID: 7832
	private static List<string> m_cooldownReductionHelper = new List<string>();

	// Token: 0x04001E99 RID: 7833
	private Rect m_rangeRect;
}
