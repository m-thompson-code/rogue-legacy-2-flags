using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009F3 RID: 2547
public class BiomeRuleManager : MonoBehaviour
{
	// Token: 0x06004CCF RID: 19663 RVA: 0x0012A2B0 File Offset: 0x001284B0
	private void Awake()
	{
		this.m_onBiomeEnter = new Action<MonoBehaviour, EventArgs>(this.OnBiomeEnter);
		this.m_onBiomeExit = new Action<MonoBehaviour, EventArgs>(this.OnBiomeExit);
		this.m_onWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnWorldCreationComplete);
		this.m_onBiomeCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnBiomeCreationComplete);
		this.m_onLevelEditorWorldCreationComplete = new Action<MonoBehaviour, EventArgs>(this.OnLevelEditorWorldCreationComplete);
		foreach (BiomeRuleManagerEntry biomeRuleManagerEntry in this.m_biomeRules)
		{
			for (int i = 0; i < biomeRuleManagerEntry.Rules.Length; i++)
			{
				biomeRuleManagerEntry.Rules[i] = UnityEngine.Object.Instantiate<BiomeRule>(biomeRuleManagerEntry.Rules[i]);
			}
		}
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x0012A380 File Offset: 0x00128580
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeExit, this.m_onBiomeExit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x0012A3CC File Offset: 0x001285CC
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeExit, this.m_onBiomeExit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
		this.ResetAllRules();
	}

	// Token: 0x06004CD2 RID: 19666 RVA: 0x0012A420 File Offset: 0x00128620
	private void OnLevelEditorWorldCreationComplete(MonoBehaviour sender, EventArgs args)
	{
		LevelEditorWorldCreationCompleteEventArgs levelEditorWorldCreationCompleteEventArgs = args as LevelEditorWorldCreationCompleteEventArgs;
		if (levelEditorWorldCreationCompleteEventArgs != null)
		{
			this.RunRules_V2(levelEditorWorldCreationCompleteEventArgs.BuiltRoom.AppearanceBiomeType, BiomeRuleExecutionTime.WorldCreationComplete);
			return;
		}
		Debug.LogFormat("<color=red>|{0}| Failed to cast args as ({1})</color>", new object[]
		{
			this,
			levelEditorWorldCreationCompleteEventArgs.GetType()
		});
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x0012A468 File Offset: 0x00128668
	private void OnWorldCreationComplete(MonoBehaviour sender, EventArgs args)
	{
		WorldBuildCompleteEventArgs worldBuildCompleteEventArgs = args as WorldBuildCompleteEventArgs;
		if (worldBuildCompleteEventArgs != null)
		{
			using (List<BiomeController>.Enumerator enumerator = worldBuildCompleteEventArgs.BiomeControllers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BiomeController biomeController = enumerator.Current;
					this.RunRules_V2(biomeController.Biome, BiomeRuleExecutionTime.WorldCreationComplete);
				}
				return;
			}
		}
		Debug.LogFormat("<color=red>|{0}| Failed to cast args as ({1})</color>", new object[]
		{
			this,
			worldBuildCompleteEventArgs.GetType()
		});
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x0012A4E8 File Offset: 0x001286E8
	private void OnBiomeCreationComplete(MonoBehaviour sender, EventArgs args)
	{
		BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
		if (biomeEventArgs != null)
		{
			BiomeType biome = biomeEventArgs.Biome;
			this.RunRules_V2(biome, BiomeRuleExecutionTime.BiomeCreationComplete);
			return;
		}
		Debug.LogFormat("<color=red>|{0}| Failed to cast args as ({1})</color>", new object[]
		{
			this,
			biomeEventArgs.GetType()
		});
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x0012A52C File Offset: 0x0012872C
	private void OnBiomeEnter(MonoBehaviour sender, EventArgs args)
	{
		this.ResetAllRules();
		BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
		if (biomeEventArgs != null)
		{
			this.RunRules_V2(biomeEventArgs.Biome, BiomeRuleExecutionTime.PlayerEnterBiome);
			return;
		}
		Debug.LogFormat("<color=red>|{0}| Failed to cast args as BiomeEventArgs</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06004CD6 RID: 19670 RVA: 0x0012A56C File Offset: 0x0012876C
	private void OnBiomeExit(MonoBehaviour sender, EventArgs args)
	{
		BiomeEventArgs biomeEventArgs = args as BiomeEventArgs;
		if (biomeEventArgs != null)
		{
			this.UndoRules_V2(biomeEventArgs.Biome, BiomeRuleExecutionTime.PlayerEnterBiome);
			return;
		}
		Debug.LogFormat("<color=red>|{0}| Failed to cast args as BiomeEventArgs</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06004CD7 RID: 19671 RVA: 0x0012A5A8 File Offset: 0x001287A8
	private List<BiomeRule> GetRules_V2(BiomeType biome, BiomeRuleExecutionTime executionTime)
	{
		this.m_ruleHelper_STATIC.Clear();
		BiomeRuleManagerEntry biomeRuleManagerEntry = null;
		foreach (BiomeRuleManagerEntry biomeRuleManagerEntry2 in this.m_biomeRules)
		{
			if (biomeRuleManagerEntry2.Biome == biome)
			{
				if (biomeRuleManagerEntry != null)
				{
					Debug.Log("<color=red>ERROR: Multiple entries for the same BiomeType: " + biome.ToString() + " found in BiomeRuleManager.</color>");
					break;
				}
				biomeRuleManagerEntry = biomeRuleManagerEntry2;
			}
		}
		if (biomeRuleManagerEntry != null)
		{
			foreach (BiomeRule biomeRule in biomeRuleManagerEntry.Rules)
			{
				if (biomeRule.ExecutionTime == executionTime)
				{
					this.m_ruleHelper_STATIC.Add(biomeRule);
				}
			}
		}
		return this.m_ruleHelper_STATIC;
	}

	// Token: 0x06004CD8 RID: 19672 RVA: 0x00029C1F File Offset: 0x00027E1F
	public static SpawnConditionOverride GetSpawnConditionOverride(BiomeType biome)
	{
		if (!BiomeRuleManager.m_spawnConditionOverrideTable.ContainsKey(biome))
		{
			BiomeRuleManager.m_spawnConditionOverrideTable.Add(biome, new SpawnConditionOverride(biome));
		}
		return BiomeRuleManager.m_spawnConditionOverrideTable[biome];
	}

	// Token: 0x06004CD9 RID: 19673 RVA: 0x0012A674 File Offset: 0x00128874
	private void RunRules_V2(BiomeType biomeType, BiomeRuleExecutionTime executionTime)
	{
		foreach (BiomeRule biomeRule in this.GetRules_V2(biomeType, executionTime))
		{
			base.StartCoroutine(biomeRule.RunRule(biomeType));
		}
		if (biomeType == BiomeType.Tower)
		{
			this.RunRules_V2(BiomeType.TowerExterior, executionTime);
		}
	}

	// Token: 0x06004CDA RID: 19674 RVA: 0x0012A6E4 File Offset: 0x001288E4
	private void UndoRules_V2(BiomeType biomeType, BiomeRuleExecutionTime executionTime)
	{
		foreach (BiomeRule biomeRule in this.GetRules_V2(biomeType, executionTime))
		{
			biomeRule.UndoRule(biomeType);
		}
		if (biomeType == BiomeType.Tower)
		{
			this.UndoRules_V2(BiomeType.TowerExterior, executionTime);
		}
	}

	// Token: 0x06004CDB RID: 19675 RVA: 0x0012A74C File Offset: 0x0012894C
	private void ResetAllRules()
	{
		if (this.m_biomeRules == null)
		{
			return;
		}
		foreach (BiomeRuleManagerEntry biomeRuleManagerEntry in this.m_biomeRules)
		{
			if (biomeRuleManagerEntry != null)
			{
				foreach (BiomeRule biomeRule in biomeRuleManagerEntry.Rules)
				{
					if (biomeRule != null)
					{
						biomeRule.UndoRule(biomeRuleManagerEntry.Biome);
					}
				}
			}
		}
	}

	// Token: 0x04003A27 RID: 14887
	[SerializeField]
	private List<BiomeRuleManagerEntry> m_biomeRules;

	// Token: 0x04003A28 RID: 14888
	private static Dictionary<BiomeType, SpawnConditionOverride> m_spawnConditionOverrideTable = new Dictionary<BiomeType, SpawnConditionOverride>();

	// Token: 0x04003A29 RID: 14889
	private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

	// Token: 0x04003A2A RID: 14890
	private Action<MonoBehaviour, EventArgs> m_onBiomeExit;

	// Token: 0x04003A2B RID: 14891
	private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

	// Token: 0x04003A2C RID: 14892
	private Action<MonoBehaviour, EventArgs> m_onBiomeCreationComplete;

	// Token: 0x04003A2D RID: 14893
	private Action<MonoBehaviour, EventArgs> m_onLevelEditorWorldCreationComplete;

	// Token: 0x04003A2E RID: 14894
	private List<BiomeRule> m_ruleHelper_STATIC = new List<BiomeRule>();
}
