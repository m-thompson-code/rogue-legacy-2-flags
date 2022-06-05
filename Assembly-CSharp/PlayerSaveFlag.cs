using System;

// Token: 0x020002D6 RID: 726
[Serializable]
public enum PlayerSaveFlag
{
	// Token: 0x04001A12 RID: 6674
	None,
	// Token: 0x04001A13 RID: 6675
	JournalReadOnce = 10,
	// Token: 0x04001A14 RID: 6676
	BlacksmithDialogue_Intro = 100,
	// Token: 0x04001A15 RID: 6677
	BlacksmithDialogue_Upgrades = 120,
	// Token: 0x04001A16 RID: 6678
	BlacksmithDialogue_EquipmentSets = 140,
	// Token: 0x04001A17 RID: 6679
	EnchantressDialogue_Intro = 200,
	// Token: 0x04001A18 RID: 6680
	EnchantressDialogue_Stacking = 220,
	// Token: 0x04001A19 RID: 6681
	ArchitectDialogue_Intro = 300,
	// Token: 0x04001A1A RID: 6682
	DummyDialogue_Intro = 350,
	// Token: 0x04001A1B RID: 6683
	CharonDialogue_Intro = 400,
	// Token: 0x04001A1C RID: 6684
	OffshoreBankDialogue_Intro = 410,
	// Token: 0x04001A1D RID: 6685
	HubtownPlaque_Intro = 415,
	// Token: 0x04001A1E RID: 6686
	TotemDialogue_Intro = 420,
	// Token: 0x04001A1F RID: 6687
	PizzaGirl_Dock_Dialogue_Intro = 430,
	// Token: 0x04001A20 RID: 6688
	PizzaGirl_UnlockTeleporter_Dialogue_Intro,
	// Token: 0x04001A21 RID: 6689
	CastleBoss_Defeated = 500,
	// Token: 0x04001A22 RID: 6690
	CastleBoss_Defeated_FirstTime,
	// Token: 0x04001A23 RID: 6691
	CastleBoss_FreeHeal_Used,
	// Token: 0x04001A24 RID: 6692
	CastleBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A25 RID: 6693
	ForestBoss_Defeated = 510,
	// Token: 0x04001A26 RID: 6694
	ForestBoss_Defeated_FirstTime,
	// Token: 0x04001A27 RID: 6695
	ForestBoss_FreeHeal_Used,
	// Token: 0x04001A28 RID: 6696
	ForestBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A29 RID: 6697
	BridgeBoss_Defeated = 520,
	// Token: 0x04001A2A RID: 6698
	BridgeBoss_Defeated_FirstTime,
	// Token: 0x04001A2B RID: 6699
	BridgeBoss_FreeHeal_Used,
	// Token: 0x04001A2C RID: 6700
	BridgeBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A2D RID: 6701
	StudyBoss_Defeated = 530,
	// Token: 0x04001A2E RID: 6702
	StudyMiniboss_SwordKnight_Defeated,
	// Token: 0x04001A2F RID: 6703
	StudyMiniboss_SpearKnight_Defeated,
	// Token: 0x04001A30 RID: 6704
	StudyBoss_Defeated_FirstTime,
	// Token: 0x04001A31 RID: 6705
	StudyBoss_FreeHeal_Used,
	// Token: 0x04001A32 RID: 6706
	StudyBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A33 RID: 6707
	TowerBoss_Defeated = 540,
	// Token: 0x04001A34 RID: 6708
	TowerBoss_Defeated_FirstTime,
	// Token: 0x04001A35 RID: 6709
	TowerBoss_FreeHeal_Used,
	// Token: 0x04001A36 RID: 6710
	TowerBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A37 RID: 6711
	CaveBoss_Defeated = 550,
	// Token: 0x04001A38 RID: 6712
	CaveBoss_Defeated_FirstTime,
	// Token: 0x04001A39 RID: 6713
	CaveBoss_FreeHeal_Used,
	// Token: 0x04001A3A RID: 6714
	CaveBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A3B RID: 6715
	CaveMiniboss_White_Defeated,
	// Token: 0x04001A3C RID: 6716
	CaveMiniboss_Black_Defeated,
	// Token: 0x04001A3D RID: 6717
	CaveMiniboss_WhiteDoor_Opened,
	// Token: 0x04001A3E RID: 6718
	CaveMiniboss_BlackDoor_Opened,
	// Token: 0x04001A3F RID: 6719
	FinalBoss_Defeated = 560,
	// Token: 0x04001A40 RID: 6720
	FinalBoss_Defeated_FirstTime,
	// Token: 0x04001A41 RID: 6721
	FinalBoss_FreeHeal_Used,
	// Token: 0x04001A42 RID: 6722
	FinalBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A43 RID: 6723
	GardenBoss_Defeated = 570,
	// Token: 0x04001A44 RID: 6724
	GardenBoss_Defeated_FirstTime,
	// Token: 0x04001A45 RID: 6725
	GardenBoss_FreeHeal_Used,
	// Token: 0x04001A46 RID: 6726
	GardenBoss_Prime_Defeated_FirstTime,
	// Token: 0x04001A47 RID: 6727
	BlackChest_Weapon_Opened = 600,
	// Token: 0x04001A48 RID: 6728
	BlackChest_Helm_Opened,
	// Token: 0x04001A49 RID: 6729
	BlackChest_Chest_Opened,
	// Token: 0x04001A4A RID: 6730
	BlackChest_Cape_Opened,
	// Token: 0x04001A4B RID: 6731
	BlackChest_Trinket_Opened,
	// Token: 0x04001A4C RID: 6732
	DragonDialogue_Intro = 650,
	// Token: 0x04001A4D RID: 6733
	DragonDialogue_BossDoorOpen,
	// Token: 0x04001A4E RID: 6734
	DragonDialogue_AfterDefeatingTubal,
	// Token: 0x04001A4F RID: 6735
	DragonDialogue_Sleep,
	// Token: 0x04001A50 RID: 6736
	Play_Tree_DeathCutscene = 700,
	// Token: 0x04001A51 RID: 6737
	Play_Hestia_DeathCutscene,
	// Token: 0x04001A52 RID: 6738
	CaveTuningForkTriggered = 710,
	// Token: 0x04001A53 RID: 6739
	LabourCostsUnlocked = 1000,
	// Token: 0x04001A54 RID: 6740
	PizzaGirlUnlocked = 1100,
	// Token: 0x04001A55 RID: 6741
	DriftHouseUnlocked = 1200,
	// Token: 0x04001A56 RID: 6742
	EnteredDriftHouseOnce = 1210,
	// Token: 0x04001A57 RID: 6743
	ChallengeDialogue_Intro = 1300,
	// Token: 0x04001A58 RID: 6744
	Timeline_Unlocked = 1400,
	// Token: 0x04001A59 RID: 6745
	TimelineDialogue_Intro = 1410,
	// Token: 0x04001A5A RID: 6746
	SoulShop_Unlocked = 1500,
	// Token: 0x04001A5B RID: 6747
	SoulShopDialogue_Intro = 1510,
	// Token: 0x04001A5C RID: 6748
	Johan_First_Death_Intro = 2000,
	// Token: 0x04001A5D RID: 6749
	Johan_Getting_Memory_Heirloom = 2010,
	// Token: 0x04001A5E RID: 6750
	Johan_Entering_Secret_Tower = 2020,
	// Token: 0x04001A5F RID: 6751
	Johan_After_Beating_Castle_Boss = 2030,
	// Token: 0x04001A60 RID: 6752
	Johan_Finding_On_Bridge = 2040,
	// Token: 0x04001A61 RID: 6753
	Johan_After_Beating_Bridge_Boss = 2050,
	// Token: 0x04001A62 RID: 6754
	Johan_Sitting_At_Far_Shore = 2060,
	// Token: 0x04001A63 RID: 6755
	Johan_After_Beating_Forest_Boss = 2070,
	// Token: 0x04001A64 RID: 6756
	Johan_Reaching_Sun_Tower_Top = 2080,
	// Token: 0x04001A65 RID: 6757
	Johan_After_Beating_Study_Boss = 2090,
	// Token: 0x04001A66 RID: 6758
	Johan_After_Beating_Tower_Boss = 2100,
	// Token: 0x04001A67 RID: 6759
	Johan_After_Giving_Insight_Quest_For_Lantern = 2109,
	// Token: 0x04001A68 RID: 6760
	Johan_After_Getting_Heirloom_Lantern,
	// Token: 0x04001A69 RID: 6761
	Rebel_Door_Opened = 3000,
	// Token: 0x04001A6A RID: 6762
	FoundEggplant_Basic = 4000,
	// Token: 0x04001A6B RID: 6763
	FoundEggplant_Advanced = 4010,
	// Token: 0x04001A6C RID: 6764
	FoundEggplant_Expert = 4020,
	// Token: 0x04001A6D RID: 6765
	FoundEggplant_Miniboss = 4030,
	// Token: 0x04001A6E RID: 6766
	SeenParade = 5000,
	// Token: 0x04001A6F RID: 6767
	SeenTrueEnding = 5005,
	// Token: 0x04001A70 RID: 6768
	SeenTrueEnding_FirstTime,
	// Token: 0x04001A71 RID: 6769
	SeenNGConfirmWindow = 5010,
	// Token: 0x04001A72 RID: 6770
	PlayFarShoresWarning = 10000,
	// Token: 0x04001A73 RID: 6771
	PlayArcaneHallowsWarning,
	// Token: 0x04001A74 RID: 6772
	PlayDriftingWorldsWarning,
	// Token: 0x04001A75 RID: 6773
	PlayPizzaMundiWarning,
	// Token: 0x04001A76 RID: 6774
	PlayDragonsVowWarning
}
