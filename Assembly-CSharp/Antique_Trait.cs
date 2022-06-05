using System;

// Token: 0x0200031E RID: 798
public class Antique_Trait : BaseTrait
{
	// Token: 0x17000D9C RID: 3484
	// (get) Token: 0x06001F7F RID: 8063 RVA: 0x00064E37 File Offset: 0x00063037
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Antique;
		}
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x00064E3E File Offset: 0x0006303E
	private void Start()
	{
		this.GiveAntique();
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x00064E48 File Offset: 0x00063048
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

	// Token: 0x04001C2C RID: 7212
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
