using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class SummonRuleController : MonoBehaviour, ISerializationCallbackReceiver, IRoomConsumer
{
	// Token: 0x1700120A RID: 4618
	// (get) Token: 0x060030CD RID: 12493 RVA: 0x000A60A8 File Offset: 0x000A42A8
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

	// Token: 0x1700120B RID: 4619
	// (get) Token: 0x060030CE RID: 12494 RVA: 0x000A60E4 File Offset: 0x000A42E4
	// (set) Token: 0x060030CF RID: 12495 RVA: 0x000A60EC File Offset: 0x000A42EC
	public BaseRoom Room { get; private set; }

	// Token: 0x1700120C RID: 4620
	// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000A60F5 File Offset: 0x000A42F5
	public bool IsArenaComplete
	{
		get
		{
			return this.m_isComplete;
		}
	}

	// Token: 0x1700120D RID: 4621
	// (get) Token: 0x060030D1 RID: 12497 RVA: 0x000A60FD File Offset: 0x000A42FD
	public bool HasArenaStarted
	{
		get
		{
			return this.m_isArenaRunning;
		}
	}

	// Token: 0x1700120E RID: 4622
	// (get) Token: 0x060030D2 RID: 12498 RVA: 0x000A6105 File Offset: 0x000A4305
	// (set) Token: 0x060030D3 RID: 12499 RVA: 0x000A610D File Offset: 0x000A430D
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

	// Token: 0x060030D4 RID: 12500 RVA: 0x000A6116 File Offset: 0x000A4316
	public void ForceControllerComplete()
	{
		base.StopAllCoroutines();
		this.m_isComplete = true;
		this.m_isArenaRunning = false;
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x000A612C File Offset: 0x000A432C
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

	// Token: 0x060030D6 RID: 12502 RVA: 0x000A61B8 File Offset: 0x000A43B8
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

	// Token: 0x060030D7 RID: 12503 RVA: 0x000A6200 File Offset: 0x000A4400
	private void Initialize()
	{
		this.m_isInitialized = true;
		BaseSummonRule[] summonRuleArray = this.m_summonRuleArray;
		for (int i = 0; i < summonRuleArray.Length; i++)
		{
			summonRuleArray[i].Initialize(this);
		}
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x000A6232 File Offset: 0x000A4432
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x000A6259 File Offset: 0x000A4459
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
		}
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x000A6285 File Offset: 0x000A4485
	private void OnPlayerEnter(object sender, EventArgs args)
	{
		base.StartCoroutine(this.SummonCoroutine());
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000A6294 File Offset: 0x000A4494
	public void StartArena()
	{
		this.m_isArenaRunning = true;
		if (this.PlayerInvincibilityActive)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.PlayerInvincibilityActive = false;
		}
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000A62B7 File Offset: 0x000A44B7
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

	// Token: 0x060030DD RID: 12509 RVA: 0x000A62E8 File Offset: 0x000A44E8
	public void ResetSummonRuleController()
	{
		BaseSummonRule[] summonRuleArray = this.m_summonRuleArray;
		for (int i = 0; i < summonRuleArray.Length; i++)
		{
			summonRuleArray[i].ResetRule();
		}
		this.m_isComplete = false;
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x000A6319 File Offset: 0x000A4519
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

	// Token: 0x060030DF RID: 12511 RVA: 0x000A6328 File Offset: 0x000A4528
	public void AddRule(SummonRuleType ruleType)
	{
		BaseSummonRule summonRule = SummonRuleType_RL.GetSummonRule(ruleType);
		this.AddNewRule(summonRule);
	}

	// Token: 0x060030E0 RID: 12512 RVA: 0x000A6344 File Offset: 0x000A4544
	private void AddNewRule(BaseSummonRule ruleToAdd)
	{
		if (ruleToAdd != null)
		{
			List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
			list.Add(ruleToAdd);
			this.m_summonRuleArray = list.ToArray();
		}
	}

	// Token: 0x060030E1 RID: 12513 RVA: 0x000A6374 File Offset: 0x000A4574
	private void InsertNewRule(BaseSummonRule ruleToAdd, int index)
	{
		if (ruleToAdd != null)
		{
			List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
			list.Insert(index, ruleToAdd);
			this.m_summonRuleArray = list.ToArray();
		}
	}

	// Token: 0x060030E2 RID: 12514 RVA: 0x000A63A4 File Offset: 0x000A45A4
	public void RemoveRule(int index)
	{
		List<BaseSummonRule> list = this.m_summonRuleArray.ToList<BaseSummonRule>();
		list.RemoveAt(index);
		this.m_summonRuleArray = list.ToArray();
	}

	// Token: 0x060030E3 RID: 12515 RVA: 0x000A63D0 File Offset: 0x000A45D0
	public BaseSummonRule GetRule(int index)
	{
		return this.m_summonRuleArray[index];
	}

	// Token: 0x060030E4 RID: 12516 RVA: 0x000A63DC File Offset: 0x000A45DC
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

	// Token: 0x060030E5 RID: 12517 RVA: 0x000A64C4 File Offset: 0x000A46C4
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

	// Token: 0x040026AE RID: 9902
	[NonSerialized]
	public List<EnemyTypeAndRank> SummonPool = new List<EnemyTypeAndRank>();

	// Token: 0x040026AF RID: 9903
	[NonSerialized]
	public List<int> SpawnPoints = new List<int>();

	// Token: 0x040026B0 RID: 9904
	[NonSerialized]
	public List<int> AvailableSpawnPoints = new List<int>();

	// Token: 0x040026B1 RID: 9905
	[NonSerialized]
	public EnemyRank SummonDifficultyOverride = EnemyRank.None;

	// Token: 0x040026B2 RID: 9906
	[NonSerialized]
	public int SummonLevelOverride;

	// Token: 0x040026B3 RID: 9907
	[NonSerialized]
	public float SavedCurrentEnemyHP;

	// Token: 0x040026B4 RID: 9908
	[NonSerialized]
	public bool PoolIsBiomeSpecific;

	// Token: 0x040026B5 RID: 9909
	[NonSerialized]
	public bool PlayerInvincibilityActive;

	// Token: 0x040026B6 RID: 9910
	[SerializeField]
	private EnemySummonSerializedData[] m_serializedData = new EnemySummonSerializedData[0];

	// Token: 0x040026B7 RID: 9911
	[SerializeField]
	private UnityEngine.Object[] m_serializedObjArray = new UnityEngine.Object[0];

	// Token: 0x040026B8 RID: 9912
	[NonSerialized]
	public BaseSummonRule[] m_summonRuleArray = new BaseSummonRule[0];

	// Token: 0x040026B9 RID: 9913
	private bool m_isComplete;

	// Token: 0x040026BA RID: 9914
	private int m_ruleExecutionIndex;

	// Token: 0x040026BB RID: 9915
	private bool m_isArenaRunning;

	// Token: 0x040026BC RID: 9916
	private bool m_isInitialized;
}
