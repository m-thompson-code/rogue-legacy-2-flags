using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class BiomeRuleManager : MonoBehaviour
{
	// Token: 0x060036BD RID: 14013 RVA: 0x000BC3C0 File Offset: 0x000BA5C0
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

	// Token: 0x060036BE RID: 14014 RVA: 0x000BC490 File Offset: 0x000BA690
	private void Start()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeExit, this.m_onBiomeExit);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x000BC4DC File Offset: 0x000BA6DC
	private void OnDestroy()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeEnter, this.m_onBiomeEnter);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeExit, this.m_onBiomeExit);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.WorldCreationComplete, this.m_onWorldCreationComplete);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.BiomeCreationComplete, this.m_onBiomeCreationComplete);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.LevelEditorWorldCreationComplete, this.m_onLevelEditorWorldCreationComplete);
		this.ResetAllRules();
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x000BC530 File Offset: 0x000BA730
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

	// Token: 0x060036C1 RID: 14017 RVA: 0x000BC578 File Offset: 0x000BA778
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

	// Token: 0x060036C2 RID: 14018 RVA: 0x000BC5F8 File Offset: 0x000BA7F8
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

	// Token: 0x060036C3 RID: 14019 RVA: 0x000BC63C File Offset: 0x000BA83C
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

	// Token: 0x060036C4 RID: 14020 RVA: 0x000BC67C File Offset: 0x000BA87C
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

	// Token: 0x060036C5 RID: 14021 RVA: 0x000BC6B8 File Offset: 0x000BA8B8
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

	// Token: 0x060036C6 RID: 14022 RVA: 0x000BC784 File Offset: 0x000BA984
	public static SpawnConditionOverride GetSpawnConditionOverride(BiomeType biome)
	{
		if (!BiomeRuleManager.m_spawnConditionOverrideTable.ContainsKey(biome))
		{
			BiomeRuleManager.m_spawnConditionOverrideTable.Add(biome, new SpawnConditionOverride(biome));
		}
		return BiomeRuleManager.m_spawnConditionOverrideTable[biome];
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x000BC7B0 File Offset: 0x000BA9B0
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

	// Token: 0x060036C8 RID: 14024 RVA: 0x000BC820 File Offset: 0x000BAA20
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

	// Token: 0x060036C9 RID: 14025 RVA: 0x000BC888 File Offset: 0x000BAA88
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

	// Token: 0x04002A29 RID: 10793
	[SerializeField]
	private List<BiomeRuleManagerEntry> m_biomeRules;

	// Token: 0x04002A2A RID: 10794
	private static Dictionary<BiomeType, SpawnConditionOverride> m_spawnConditionOverrideTable = new Dictionary<BiomeType, SpawnConditionOverride>();

	// Token: 0x04002A2B RID: 10795
	private Action<MonoBehaviour, EventArgs> m_onBiomeEnter;

	// Token: 0x04002A2C RID: 10796
	private Action<MonoBehaviour, EventArgs> m_onBiomeExit;

	// Token: 0x04002A2D RID: 10797
	private Action<MonoBehaviour, EventArgs> m_onWorldCreationComplete;

	// Token: 0x04002A2E RID: 10798
	private Action<MonoBehaviour, EventArgs> m_onBiomeCreationComplete;

	// Token: 0x04002A2F RID: 10799
	private Action<MonoBehaviour, EventArgs> m_onLevelEditorWorldCreationComplete;

	// Token: 0x04002A30 RID: 10800
	private List<BiomeRule> m_ruleHelper_STATIC = new List<BiomeRule>();
}
