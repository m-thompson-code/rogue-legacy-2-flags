using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
[Serializable]
public class RelicObj
{
	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x0600266D RID: 9837 RVA: 0x0001569C File Offset: 0x0001389C
	// (set) Token: 0x0600266E RID: 9838 RVA: 0x000156A4 File Offset: 0x000138A4
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

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x0600266F RID: 9839 RVA: 0x000156AD File Offset: 0x000138AD
	public RelicModType RelicModType
	{
		get
		{
			return this.m_relicModType;
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000156B5 File Offset: 0x000138B5
	public void SetRelicMod(RelicModType relicMod)
	{
		this.m_relicModType = relicMod;
	}

	// Token: 0x1700100C RID: 4108
	// (get) Token: 0x06002671 RID: 9841 RVA: 0x000156BE File Offset: 0x000138BE
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x000156C6 File Offset: 0x000138C6
	public void DebugForceLevel(int value)
	{
		this.m_level = value;
	}

	// Token: 0x06002673 RID: 9843 RVA: 0x000B6488 File Offset: 0x000B4688
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

	// Token: 0x1700100D RID: 4109
	// (get) Token: 0x06002674 RID: 9844 RVA: 0x000156CF File Offset: 0x000138CF
	public int IntValue
	{
		get
		{
			return this.m_intValue;
		}
	}

	// Token: 0x06002675 RID: 9845 RVA: 0x000156D7 File Offset: 0x000138D7
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

	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x06002676 RID: 9846 RVA: 0x00015713 File Offset: 0x00013913
	public float FloatValue
	{
		get
		{
			return this.m_floatValue;
		}
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x0001571B File Offset: 0x0001391B
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

	// Token: 0x06002678 RID: 9848 RVA: 0x000B653C File Offset: 0x000B473C
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

	// Token: 0x06002679 RID: 9849 RVA: 0x000B6678 File Offset: 0x000B4878
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

	// Token: 0x0600267A RID: 9850 RVA: 0x000B6758 File Offset: 0x000B4958
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

	// Token: 0x0600267B RID: 9851 RVA: 0x000B67D4 File Offset: 0x000B49D4
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

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x0600267C RID: 9852 RVA: 0x00015757 File Offset: 0x00013957
	// (set) Token: 0x0600267D RID: 9853 RVA: 0x0001575F File Offset: 0x0001395F
	public RelicType RelicType { get; private set; }

	// Token: 0x0600267E RID: 9854 RVA: 0x00015768 File Offset: 0x00013968
	public RelicObj(RelicType relicType)
	{
		this.RelicType = relicType;
	}

	// Token: 0x0400214D RID: 8525
	private int m_level;

	// Token: 0x0400214E RID: 8526
	private int m_intValue;

	// Token: 0x0400214F RID: 8527
	private float m_floatValue;

	// Token: 0x04002150 RID: 8528
	private bool m_wasSeen;

	// Token: 0x04002151 RID: 8529
	public bool IsFreeRelic;

	// Token: 0x04002152 RID: 8530
	private RelicModType m_relicModType;

	// Token: 0x04002153 RID: 8531
	[NonSerialized]
	public bool CountsBackwards;

	// Token: 0x04002154 RID: 8532
	private static RelicChangedEventArgs m_relicChangedArgs_STATIC = new RelicChangedEventArgs(RelicType.None);

	// Token: 0x04002155 RID: 8533
	[NonSerialized]
	private Action<object, HealthChangeEventArgs> m_onPlayerHitLowHealth_nonSerialized;

	// Token: 0x04002156 RID: 8534
	[NonSerialized]
	private Action<object, HealthChangeEventArgs> m_onPlayerHitMaxHealth_nonSerialized;

	// Token: 0x04002157 RID: 8535
	private bool m_lowHealthStatBonusApplied;

	// Token: 0x04002158 RID: 8536
	private bool m_maxHealthStatBonusApplied;

	// Token: 0x0400215A RID: 8538
	[NonSerialized]
	private RelicChangedEventArgs m_relicChangedEventArgs;
}
