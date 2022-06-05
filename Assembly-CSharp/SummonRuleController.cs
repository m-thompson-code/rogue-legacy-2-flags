using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
public class SummonRuleController : MonoBehaviour, ISerializationCallbackReceiver, IRoomConsumer
{
	// Token: 0x17001845 RID: 6213
	// (get) Token: 0x060043FE RID: 17406 RVA: 0x0010DE38 File Offset: 0x0010C038
	public bool StillSummoning
	{
		get
		{
			int num = -1;
			for (int i = 0; i < this.SummonRuleArray.Length; i++)
			{
				if (this.SummonRuleArray[i] is SummonEnemy_SummonRule)
				{
					num = i;
				}
			}
			return this.m_ruleExecutionIndex < num;
		}
	}

	// Token: 0x17001846 RID: 6214
	// (get) Token: 0x060043FF RID: 17407 RVA: 0x000257DE File Offset: 0x000239DE
	// (set) Token: 0x06004400 RID: 17408 RVA: 0x000257E6 File Offset: 0x000239E6
	public BaseRoom Room { get; private set; }

	// Token: 0x17001847 RID: 6215
	// (get) Token: 0x06004401 RID: 17409 RVA: 0x000257EF File Offset: 0x000239EF
	public bool IsArenaComplete
	{
		get
		{
			return this.m_isComplete;
		}
	}

	// Token: 0x17001848 RID: 6216
	// (get) Token: 0x06004402 RID: 17410 RVA: 0x000257F7 File Offset: 0x000239F7
	public bool HasArenaStarted
	{
		get
		{
			return this.m_isArenaRunning;
		}
	}

	// Token: 0x17001849 RID: 6217
	// (get) Token: 0x06004403 RID: 17411 RVA: 0x000257FF File Offset: 0x000239FF
	// (set) Token: 0x06004404 RID: 17412 RVA: 0x00025807 File Offset: 0x00023A07
	public BaseSummonRule[] SummonRuleArray
	{
		get
		{
			return this.m_summonRuleArray;
		}
		set
		{
			this.m_summonRuleArray = value;
		}
	}

	// Token: 0x06004405 RID: 17413 RVA: 0x00025810 File Offset: 0x00023A10
	public void ForceControllerComplete()
	{
		base.StopAllCoroutines();
		this.m_isComplete = true;
		this.m_isArenaRunning = false;
	}

	// Token: 0x06004406 RID: 17414 RVA: 0x0010DE74 File Offset: 0x0010C074
	private void OnEnable()
	{
		if (!GameUtility.IsInLevelEditor && WorldBuilder.State != BiomeBuildStateID.Complete)
		{
			return;
		}
		base.StopAllCoroutines();
		if (!this.m_isInitialized)
		{
			this.Initialize();
		}
		this.ResetSummonRuleController();
		BaseSummonRule[] summonRuleArray = this.m_summonRuleArray;
		for (int i = 0; i < summonRuleArray.Length; i++)
		{
			summonRuleArray[i].OnEnable();
		}
		this.SummonPool.Clear();
		this.SpawnPoints.Clear();
		this.SummonDifficultyOverride = EnemyRank.None;
		this.SummonLevelOverride = 0;
		this.SavedCurrentEnemyHP = 0f;
		this.PoolIsBiomeSpecific = false;
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x0010DF00 File Offset: 0x0010C100
	private void OnDisable()
	{
		foreach (BaseSummonRule baseSummonRule in this.m_summonRuleArray)
		{
			baseSummonRule.OnDisable();
			baseSummonRule.ResetRule();
		}
		if (EnemyManager.IsInitialized)
		{
			EnemyManager.DisableAllSummonedEnemies();
		}
		this.StopArena(this.IsArenaComplete);
	}

	// Token: 0x06004408 RID: 17416 RVA: 0x0010DF48 File Offset: 0x0010C148
	private void Initialize()
	{
		this.m_isInitialized = true;
		BaseSummonRule[] summonRuleArray = this.m_summonRuleArray;
		for (int i = 0; i < summonRuleArray.Length; i++)
		{
			summonRuleArray[i].Initialize(this);
		}
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x00025826 File Offset: 0x00023A26
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x0002584D File Offset: 0x00023A4D
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x00025879 File Offset: 0x00023A79
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		base.StartCoroutine(this.SummonCoroutine());
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x00025888 File Offset: 0x00023A88
	public void StartArena()
	{
		this.m_isArenaRunning = true;
		if (this.PlayerInvincibilityActive)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.PlayerInvincibilityActive = false;
		}
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x000258AB File Offset: 0x00023AAB
	public void StopArena(bool arenaComplete)
	{
		this.m_isArenaRunning = false;
		this.m_isComplete = arenaComplete;
		if (this.PlayerInvincibilityActive)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.PlayerInvincibilityActive = false;
		}
		base.StopAllCoroutines();
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x0010DF7C File Offset: 0x0010C17C
	public void ResetSummonRuleController()
	{
		BaseSummonRule[] summonRuleArray = this.m_summonRuleArray;
		for (int i = 0; i < summonRuleArray.Length; i++)
		{
			summonRuleArray[i].ResetRule();
		}
		this.m_isComplete = false;
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x000258DB File Offset: 0x00023ADB
	private IEnumerator SummonCoroutine()
	{
		this.m_ruleExecutionIndex = 0;
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		if (this.m_summonRuleArray != null)
		{
			int num;
			for (int i = 0; i < this.m_summonRuleArray.Length; i = num + 1)
			{
				this.m_ruleExecutionIndex = i;
				BaseSummonRule baseSummonRule = this.m_summonRuleArray[i];
				if (!baseSummonRule.IsRuleComplete)
				{
					yield return baseSummonRule.RunSummonRule();
				}
				num = i;
			}
		}
		this.m_isComplete = true;
		this.m_isArenaRunning = false;
		yield break;
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x0010DFB0 File Offset: 0x0010C1B0
	public void AddRule(SummonRuleType ruleType)
	{
		BaseSummonRule summonRule = SummonRuleType_RL.GetSummonRule(ruleType);
		this.AddNewRule(summonRule);
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x0010DFCC File Offset: 0x0010C1CC
	private void AddNewRule(BaseSummonRule ruleToAdd)
	{
		if (ruleToAdd != null)
		{
			List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
			list.Add(ruleToAdd);
			this.m_summonRuleArray = list.ToArray();
		}
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x0010DFFC File Offset: 0x0010C1FC
	private void InsertNewRule(BaseSummonRule ruleToAdd, int index)
	{
		if (ruleToAdd != null)
		{
			List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
			list.Insert(index, ruleToAdd);
			this.m_summonRuleArray = list.ToArray();
		}
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x0010E02C File Offset: 0x0010C22C
	public void RemoveRule(int index)
	{
		List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
		list.RemoveAt(index);
		this.m_summonRuleArray = list.ToArray();
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x000258EA File Offset: 0x00023AEA
	public BaseSummonRule GetRule(int index)
	{
		return this.m_summonRuleArray[index];
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x0010E058 File Offset: 0x0010C258
	public void OnBeforeSerialize()
	{
		if (this.m_serializedObjArray.Length != this.SummonRuleArray.Length)
		{
			Array.Clear(this.m_serializedObjArray, 0, this.m_serializedObjArray.Length);
			this.m_serializedObjArray = new UnityEngine.Object[this.SummonRuleArray.Length];
		}
		if (this.m_serializedData.Length != this.SummonRuleArray.Length)
		{
			Array.Clear(this.m_serializedData, 0, this.m_serializedData.Length);
			this.m_serializedData = new EnemySummonSerializedData[this.SummonRuleArray.Length];
		}
		for (int i = 0; i < this.SummonRuleArray.Length; i++)
		{
			BaseSummonRule baseSummonRule = this.SummonRuleArray[i];
			string dataType = baseSummonRule.GetType().ToString();
			string data = JsonUtility.ToJson(baseSummonRule);
			if (this.m_serializedData[i] == null)
			{
				this.m_serializedData[i] = new EnemySummonSerializedData();
			}
			EnemySummonSerializedData enemySummonSerializedData = this.m_serializedData[i];
			enemySummonSerializedData.DataType = dataType;
			enemySummonSerializedData.Data = data;
			this.m_serializedObjArray[i] = baseSummonRule.SerializedObject;
		}
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x0010E140 File Offset: 0x0010C340
	public void OnAfterDeserialize()
	{
		List<BaseSummonRule> list = new List<BaseSummonRule>();
		foreach (EnemySummonSerializedData enemySummonSerializedData in this.m_serializedData)
		{
			Type type = Type.GetType(enemySummonSerializedData.DataType);
			list.Add((BaseSummonRule)JsonUtility.FromJson(enemySummonSerializedData.Data, type));
		}
		this.m_summonRuleArray = list.ToArray();
		if (this.m_summonRuleArray.Length == this.m_serializedObjArray.Length)
		{
			for (int j = 0; j < this.m_summonRuleArray.Length; j++)
			{
				this.SummonRuleArray[j].SetSerializedObject(this.m_serializedObjArray[j]);
			}
			return;
		}
		Debug.Log("<color=red>Could not load serialized objects into the summon rules. For some reason the array sizes do not match.</color>");
	}

	// Token: 0x040034DE RID: 13534
	[NonSerialized]
	public List<EnemyTypeAndRank> SummonPool = new List<EnemyTypeAndRank>();

	// Token: 0x040034DF RID: 13535
	[NonSerialized]
	public List<int> SpawnPoints = new List<int>();

	// Token: 0x040034E0 RID: 13536
	[NonSerialized]
	public List<int> AvailableSpawnPoints = new List<int>();

	// Token: 0x040034E1 RID: 13537
	[NonSerialized]
	public EnemyRank SummonDifficultyOverride = EnemyRank.None;

	// Token: 0x040034E2 RID: 13538
	[NonSerialized]
	public int SummonLevelOverride;

	// Token: 0x040034E3 RID: 13539
	[NonSerialized]
	public float SavedCurrentEnemyHP;

	// Token: 0x040034E4 RID: 13540
	[NonSerialized]
	public bool PoolIsBiomeSpecific;

	// Token: 0x040034E5 RID: 13541
	[NonSerialized]
	public bool PlayerInvincibilityActive;

	// Token: 0x040034E6 RID: 13542
	[SerializeField]
	private EnemySummonSerializedData[] m_serializedData = new EnemySummonSerializedData[0];

	// Token: 0x040034E7 RID: 13543
	[SerializeField]
	private UnityEngine.Object[] m_serializedObjArray = new UnityEngine.Object[0];

	// Token: 0x040034E8 RID: 13544
	[NonSerialized]
	public BaseSummonRule[] m_summonRuleArray = new BaseSummonRule[0];

	// Token: 0x040034E9 RID: 13545
	private bool m_isComplete;

	// Token: 0x040034EA RID: 13546
	private int m_ruleExecutionIndex;

	// Token: 0x040034EB RID: 13547
	private bool m_isArenaRunning;

	// Token: 0x040034EC RID: 13548
	private bool m_isInitialized;
}
