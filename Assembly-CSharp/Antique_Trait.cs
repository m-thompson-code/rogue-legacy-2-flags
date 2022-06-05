using System;

// Token: 0x02000565 RID: 1381
public class Antique_Trait : BaseTrait
{
	// Token: 0x170011C3 RID: 4547
	// (get) Token: 0x06002C26 RID: 11302 RVA: 0x0001887D File Offset: 0x00016A7D
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Antique;
		}
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x00018884 File Offset: 0x00016A84
	private void Start()
	{
		this.GiveAntique();
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000C5328 File Offset: 0x000C3528
	private void GiveAntique()
	{
		RelicType relicType = base.IsFirstTrait ? SaveManager.PlayerSaveData.CurrentCharacter.AntiqueOneOwned : SaveManager.PlayerSaveData.CurrentCharacter.AntiqueTwoOwned;
		if (relicType <= RelicType.None)
		{
			relicType = RelicLibrary.GetRandomRelic(RngID.None, true, Antique_Trait.RelicExceptionArray);
			if (base.IsFirstTrait)
			{
				SaveManager.PlayerSaveData.CurrentCharacter.AntiqueOneOwned = relicType;
			}
			else
			{
				SaveManager.PlayerSaveData.CurrentCharacter.AntiqueTwoOwned = relicType;
			}
		}
		RelicObj relic = SaveManager.PlayerSaveData.GetRelic(relicType);
		if (relic != null && !relic.IsFreeRelic)
		{
			relic.SetLevel(1, false, true);
			relic.IsFreeRelic = true;
		}
	}

	// Token: 0x04002550 RID: 9552
	public static RelicType[] RelicExceptionArray = new RelicType[]
	{
		RelicType.None,
		RelicType.GoldDeathCurse,
		RelicType.BonusDamageCurse,
		RelicType.AttackCooldown,
		RelicType.ResolveCombatChallenge,
		RelicType.NoGoldXPBonus,
		RelicType.GoldCombatChallenge,
		RelicType.FoodChallenge,
		RelicType.FlightBonusCurse,
		RelicType.RangeDamageBonusCurse
	};
}
