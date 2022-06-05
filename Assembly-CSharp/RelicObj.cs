using System;
using UnityEngine;

// Token: 0x020002B9 RID: 697
[Serializable]
public class RelicObj
{
	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x00059693 File Offset: 0x00057893
	// (set) Token: 0x06001BBA RID: 7098 RVA: 0x0005969B File Offset: 0x0005789B
	public bool WasSeen
	{
		get
		{
			return this.m_wasSeen;
		}
		private set
		{
			this.m_wasSeen = value;
		}
	}

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000596A4 File Offset: 0x000578A4
	public RelicModType RelicModType
	{
		get
		{
			return this.m_relicModType;
		}
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000596AC File Offset: 0x000578AC
	public void SetRelicMod(RelicModType relicMod)
	{
		this.m_relicModType = relicMod;
	}

	// Token: 0x17000C89 RID: 3209
	// (get) Token: 0x06001BBD RID: 7101 RVA: 0x000596B5 File Offset: 0x000578B5
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x000596BD File Offset: 0x000578BD
	public void DebugForceLevel(int value)
	{
		this.m_level = value;
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000596C8 File Offset: 0x000578C8
	public void SetLevel(int value, bool additive, bool broadcast = true)
	{
		int level = this.m_level;
		if (additive)
		{
			this.m_level += value;
		}
		else
		{
			this.m_level = value;
		}
		this.m_level = Mathf.Clamp(this.m_level, 0, 99999);
		if (this.m_level > 0 && !this.WasSeen && !ChallengeManager.IsInChallenge)
		{
			this.WasSeen = true;
		}
		if (PlayerManager.IsInstantiated)
		{
			if (this.m_level > level)
			{
				this.ApplyRelic(this.m_level - level);
			}
			else if (this.m_level == 0)
			{
				this.StopRelic();
			}
		}
		if (broadcast)
		{
			RelicObj.m_relicChangedArgs_STATIC.Initialize(this.RelicType);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicLevelChanged, null, RelicObj.m_relicChangedArgs_STATIC);
		}
	}

	// Token: 0x17000C8A RID: 3210
	// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x0005977A File Offset: 0x0005797A
	public int IntValue
	{
		get
		{
			return this.m_intValue;
		}
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00059782 File Offset: 0x00057982
	public void SetIntValue(int value, bool additive, bool broadcast = true)
	{
		if (additive)
		{
			this.m_intValue += value;
		}
		else
		{
			this.m_intValue = value;
		}
		if (broadcast)
		{
			RelicObj.m_relicChangedArgs_STATIC.Initialize(this.RelicType);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicStatsChanged, null, RelicObj.m_relicChangedArgs_STATIC);
		}
	}

	// Token: 0x17000C8B RID: 3211
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x000597BE File Offset: 0x000579BE
	public float FloatValue
	{
		get
		{
			return this.m_floatValue;
		}
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000597C6 File Offset: 0x000579C6
	public void SetFloatValue(float value, bool additive, bool broadcast = true)
	{
		if (additive)
		{
			this.m_floatValue += value;
		}
		else
		{
			this.m_floatValue = value;
		}
		if (broadcast)
		{
			RelicObj.m_relicChangedArgs_STATIC.Initialize(this.RelicType);
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RelicStatsChanged, null, RelicObj.m_relicChangedArgs_STATIC);
		}
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x00059804 File Offset: 0x00057A04
	public void ApplyRelic(int levelChange)
	{
		if (this.m_onPlayerHitLowHealth_nonSerialized == null)
		{
			this.m_onPlayerHitLowHealth_nonSerialized = new Action<object, HealthChangeEventArgs>(this.OnPlayerHitLowHealth);
		}
		if (this.m_onPlayerHitMaxHealth_nonSerialized == null)
		{
			this.m_onPlayerHitMaxHealth_nonSerialized = new Action<object, HealthChangeEventArgs>(this.OnPlayerHitMaxHealth);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		RelicType relicType = this.RelicType;
		if (relicType <= RelicType.LowHealthStatBonus)
		{
			if (relicType <= RelicType.TakeNoDamage)
			{
				if (relicType == RelicType.GodMode)
				{
					playerController.StatusEffectController.StartStatusEffect(StatusEffectType.Player_GodMode, 25f, null);
					return;
				}
				if (relicType != RelicType.TakeNoDamage)
				{
					return;
				}
				this.m_level += levelChange * 3 - levelChange;
				return;
			}
			else
			{
				if (relicType == RelicType.MaxHealthStatBonus)
				{
					playerController.HealthChangeRelay.AddListener(this.m_onPlayerHitMaxHealth_nonSerialized, false);
					return;
				}
				if (relicType != RelicType.LowHealthStatBonus)
				{
					return;
				}
				playerController.HealthChangeRelay.AddListener(this.m_onPlayerHitLowHealth_nonSerialized, false);
				return;
			}
		}
		else
		{
			if (relicType > RelicType.AttackExhaust)
			{
				if (relicType != RelicType.OnHitAreaDamage)
				{
					if (relicType != RelicType.SpinKickLeavesCaltrops)
					{
						if (relicType != RelicType.NoAttackDamageBonus)
						{
							return;
						}
						if (playerController.isActiveAndEnabled)
						{
							playerController.StartNoAttackDamageBonusTimer();
							return;
						}
					}
					else if (playerController.isActiveAndEnabled)
					{
						playerController.StartSpinKicksDropCaltropsTimer();
					}
				}
				else if (playerController.isActiveAndEnabled)
				{
					playerController.StartOnHitAreaDamageTimer();
					return;
				}
				return;
			}
			if (relicType == RelicType.ManaDamageReduction)
			{
				this.CountsBackwards = true;
				return;
			}
			if (relicType != RelicType.AttackExhaust)
			{
				return;
			}
			playerController.InitializeExhaustMods();
			return;
		}
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x00059940 File Offset: 0x00057B40
	public void StopRelic()
	{
		if (this.m_onPlayerHitLowHealth_nonSerialized == null)
		{
			this.m_onPlayerHitLowHealth_nonSerialized = new Action<object, HealthChangeEventArgs>(this.OnPlayerHitLowHealth);
		}
		if (this.m_onPlayerHitMaxHealth_nonSerialized == null)
		{
			this.m_onPlayerHitMaxHealth_nonSerialized = new Action<object, HealthChangeEventArgs>(this.OnPlayerHitMaxHealth);
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		RelicType relicType = this.RelicType;
		if (relicType <= RelicType.LowHealthStatBonus)
		{
			if (relicType == RelicType.GodMode)
			{
				playerController.StatusEffectController.StopStatusEffect(StatusEffectType.Player_GodMode, true);
				return;
			}
			if (relicType == RelicType.MaxHealthStatBonus)
			{
				this.m_maxHealthStatBonusApplied = false;
				playerController.HealthChangeRelay.RemoveListener(this.m_onPlayerHitMaxHealth_nonSerialized);
				return;
			}
			if (relicType != RelicType.LowHealthStatBonus)
			{
				return;
			}
			this.m_lowHealthStatBonusApplied = false;
			playerController.HealthChangeRelay.RemoveListener(this.m_onPlayerHitLowHealth_nonSerialized);
			return;
		}
		else
		{
			if (relicType == RelicType.OnHitAreaDamage)
			{
				playerController.StopOnHitAreaDamageTimer();
				return;
			}
			if (relicType == RelicType.SpinKickLeavesCaltrops)
			{
				playerController.StopSpinKicksDropCaltropsTimer();
				return;
			}
			if (relicType != RelicType.NoAttackDamageBonus)
			{
				return;
			}
			playerController.StopNoAttackDamageBonusTimer();
			return;
		}
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x00059A20 File Offset: 0x00057C20
	private void OnPlayerHitLowHealth(object sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		bool lowHealthStatBonusApplied = this.m_lowHealthStatBonusApplied;
		if (!this.m_lowHealthStatBonusApplied)
		{
			if ((float)playerController.CurrentHealthAsInt <= 0.5f * (float)playerController.ActualMaxHealth)
			{
				this.m_lowHealthStatBonusApplied = true;
			}
		}
		else if (this.m_lowHealthStatBonusApplied && (float)playerController.CurrentHealthAsInt > 0.5f * (float)playerController.ActualMaxHealth)
		{
			this.m_lowHealthStatBonusApplied = false;
		}
		if (lowHealthStatBonusApplied != this.m_lowHealthStatBonusApplied)
		{
			playerController.InitializeStrengthMods();
			playerController.InitializeMagicMods();
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x00059A9C File Offset: 0x00057C9C
	private void OnPlayerHitMaxHealth(object sender, EventArgs args)
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		bool maxHealthStatBonusApplied = this.m_maxHealthStatBonusApplied;
		if (!this.m_maxHealthStatBonusApplied)
		{
			if ((float)playerController.CurrentHealthAsInt >= 0.5f * (float)playerController.ActualMaxHealth)
			{
				this.m_maxHealthStatBonusApplied = true;
			}
		}
		else if (this.m_maxHealthStatBonusApplied && (float)playerController.CurrentHealthAsInt < 0.5f * (float)playerController.ActualMaxHealth)
		{
			this.m_maxHealthStatBonusApplied = false;
		}
		if (maxHealthStatBonusApplied != this.m_maxHealthStatBonusApplied)
		{
			playerController.InitializeStrengthMods();
			playerController.InitializeMagicMods();
		}
	}

	// Token: 0x17000C8C RID: 3212
	// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x00059B15 File Offset: 0x00057D15
	// (set) Token: 0x06001BC9 RID: 7113 RVA: 0x00059B1D File Offset: 0x00057D1D
	public RelicType RelicType { get; private set; }

	// Token: 0x06001BCA RID: 7114 RVA: 0x00059B26 File Offset: 0x00057D26
	public RelicObj(RelicType relicType)
	{
		this.RelicType = relicType;
	}

	// Token: 0x04001960 RID: 6496
	private int m_level;

	// Token: 0x04001961 RID: 6497
	private int m_intValue;

	// Token: 0x04001962 RID: 6498
	private float m_floatValue;

	// Token: 0x04001963 RID: 6499
	private bool m_wasSeen;

	// Token: 0x04001964 RID: 6500
	public bool IsFreeRelic;

	// Token: 0x04001965 RID: 6501
	private RelicModType m_relicModType;

	// Token: 0x04001966 RID: 6502
	[NonSerialized]
	public bool CountsBackwards;

	// Token: 0x04001967 RID: 6503
	private static RelicChangedEventArgs m_relicChangedArgs_STATIC = new RelicChangedEventArgs(RelicType.None);

	// Token: 0x04001968 RID: 6504
	[NonSerialized]
	private Action<object, HealthChangeEventArgs> m_onPlayerHitLowHealth_nonSerialized;

	// Token: 0x04001969 RID: 6505
	[NonSerialized]
	private Action<object, HealthChangeEventArgs> m_onPlayerHitMaxHealth_nonSerialized;

	// Token: 0x0400196A RID: 6506
	private bool m_lowHealthStatBonusApplied;

	// Token: 0x0400196B RID: 6507
	private bool m_maxHealthStatBonusApplied;

	// Token: 0x0400196D RID: 6509
	[NonSerialized]
	private RelicChangedEventArgs m_relicChangedEventArgs;
}
