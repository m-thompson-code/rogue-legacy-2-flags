using System;

// Token: 0x020004CB RID: 1227
[Serializable]
public enum PlayerSaveFlag
{
	// Token: 0x04002219 RID: 8729
	None,
	// Token: 0x0400221A RID: 8730
	JournalReadOnce = 10,
	// Token: 0x0400221B RID: 8731
	BlacksmithDialogue_Intro = 100,
	// Token: 0x0400221C RID: 8732
	BlacksmithDialogue_Upgrades = 120,
	// Token: 0x0400221D RID: 8733
	BlacksmithDialogue_EquipmentSets = 140,
	// Token: 0x0400221E RID: 8734
	EnchantressDialogue_Intro = 200,
	// Token: 0x0400221F RID: 8735
	EnchantressDialogue_Stacking = 220,
	// Token: 0x04002220 RID: 8736
	ArchitectDialogue_Intro = 300,
	// Token: 0x04002221 RID: 8737
	DummyDialogue_Intro = 350,
	// Token: 0x04002222 RID: 8738
	CharonDialogue_Intro = 400,
	// Token: 0x04002223 RID: 8739
	OffshoreBankDialogue_Intro = 410,
	// Token: 0x04002224 RID: 8740
	HubtownPlaque_Intro = 415,
	// Token: 0x04002225 RID: 8741
	TotemDialogue_Intro = 420,
	// Token: 0x04002226 RID: 8742
	PizzaGirl_Dock_Dialogue_Intro = 430,
	// Token: 0x04002227 RID: 8743
	PizzaGirl_UnlockTeleporter_Dialogue_Intro,
	// Token: 0x04002228 RID: 8744
	CastleBoss_Defeated = 500,
	// Token: 0x04002229 RID: 8745
	CastleBoss_Defeated_FirstTime,
	// Token: 0x0400222A RID: 8746
	CastleBoss_FreeHeal_Used,
	// Token: 0x0400222B RID: 8747
	CastleBoss_Prime_Defeated_FirstTime,
	// Token: 0x0400222C RID: 8748
	ForestBoss_Defeated = 510,
	// Token: 0x0400222D RID: 8749
	ForestBoss_Defeated_FirstTime,
	// Token: 0x0400222E RID: 8750
	ForestBoss_FreeHeal_Used,
	// Token: 0x0400222F RID: 8751
	ForestBoss_Prime_Defeated_FirstTime,
	// Token: 0x04002230 RID: 8752
	BridgeBoss_Defeated = 520,
	// Token: 0x04002231 RID: 8753
	BridgeBoss_Defeated_FirstTime,
	// Token: 0x04002232 RID: 8754
	BridgeBoss_FreeHeal_Used,
	// Token: 0x04002233 RID: 8755
	BridgeBoss_Prime_Defeated_FirstTime,
	// Token: 0x04002234 RID: 8756
	StudyBoss_Defeated = 530,
	// Token: 0x04002235 RID: 8757
	StudyMiniboss_SwordKnight_Defeated,
	// Token: 0x04002236 RID: 8758
	StudyMiniboss_SpearKnight_Defeated,
	// Token: 0x04002237 RID: 8759
	StudyBoss_Defeated_FirstTime,
	// Token: 0x04002238 RID: 8760
	StudyBoss_FreeHeal_Used,
	// Token: 0x04002239 RID: 8761
	StudyBoss_Prime_Defeated_FirstTime,
	// Token: 0x0400223A RID: 8762
	TowerBoss_Defeated = 540,
	// Token: 0x0400223B RID: 8763
	TowerBoss_Defeated_FirstTime,
	// Token: 0x0400223C RID: 8764
	TowerBoss_FreeHeal_Used,
	// Token: 0x0400223D RID: 8765
	TowerBoss_Prime_Defeated_FirstTime,
	// Token: 0x0400223E RID: 8766
	CaveBoss_Defeated = 550,
	// Token: 0x0400223F RID: 8767
	CaveBoss_Defeated_FirstTime,
	// Token: 0x04002240 RID: 8768
	CaveBoss_FreeHeal_Used,
	// Token: 0x04002241 RID: 8769
	CaveBoss_Prime_Defeated_FirstTime,
	// Token: 0x04002242 RID: 8770
	CaveMiniboss_White_Defeated,
	// Token: 0x04002243 RID: 8771
	CaveMiniboss_Black_Defeated,
	// Token: 0x04002244 RID: 8772
	CaveMiniboss_WhiteDoor_Opened,
	// Token: 0x04002245 RID: 8773
	CaveMiniboss_BlackDoor_Opened,
	// Token: 0x04002246 RID: 8774
	FinalBoss_Defeated = 560,
	// Token: 0x04002247 RID: 8775
	FinalBoss_Defeated_FirstTime,
	// Token: 0x04002248 RID: 8776
	FinalBoss_FreeHeal_Used,
	// Token: 0x04002249 RID: 8777
	FinalBoss_Prime_Defeated_FirstTime,
	// Token: 0x0400224A RID: 8778
	GardenBoss_Defeated = 570,
	// Token: 0x0400224B RID: 8779
	GardenBoss_Defeated_FirstTime,
	// Token: 0x0400224C RID: 8780
	GardenBoss_FreeHeal_Used,
	// Token: 0x0400224D RID: 8781
	GardenBoss_Prime_Defeated_FirstTime,
	// Token: 0x0400224E RID: 8782
	BlackChest_Weapon_Opened = 600,
	// Token: 0x0400224F RID: 8783
	BlackChest_Helm_Opened,
	// Token: 0x04002250 RID: 8784
	BlackChest_Chest_Opened,
	// Token: 0x04002251 RID: 8785
	BlackChest_Cape_Opened,
	// Token: 0x04002252 RID: 8786
	BlackChest_Trinket_Opened,
	// Token: 0x04002253 RID: 8787
	DragonDialogue_Intro = 650,
	// Token: 0x04002254 RID: 8788
	DragonDialogue_BossDoorOpen,
	// Token: 0x04002255 RID: 8789
	DragonDialogue_AfterDefeatingTubal,
	// Token: 0x04002256 RID: 8790
	DragonDialogue_Sleep,
	// Token: 0x04002257 RID: 8791
	Play_Tree_DeathCutscene = 700,
	// Token: 0x04002258 RID: 8792
	Play_Hestia_DeathCutscene,
	// Token: 0x04002259 RID: 8793
	CaveTuningForkTriggered = 710,
	// Token: 0x0400225A RID: 8794
	LabourCostsUnlocked = 1000,
	// Token: 0x0400225B RID: 8795
	PizzaGirlUnlocked = 1100,
	// Token: 0x0400225C RID: 8796
	DriftHouseUnlocked = 1200,
	// Token: 0x0400225D RID: 8797
	EnteredDriftHouseOnce = 1210,
	// Token: 0x0400225E RID: 8798
	ChallengeDialogue_Intro = 1300,
	// Token: 0x0400225F RID: 8799
	Timeline_Unlocked = 1400,
	// Token: 0x04002260 RID: 8800
	TimelineDialogue_Intro = 1410,
	// Token: 0x04002261 RID: 8801
	SoulShop_Unlocked = 1500,
	// Token: 0x04002262 RID: 8802
	SoulShopDialogue_Intro = 1510,
	// Token: 0x04002263 RID: 8803
	Johan_First_Death_Intro = 2000,
	// Token: 0x04002264 RID: 8804
	Johan_Getting_Memory_Heirloom = 2010,
	// Token: 0x04002265 RID: 8805
	Johan_Entering_Secret_Tower = 2020,
	// Token: 0x04002266 RID: 8806
	Johan_After_Beating_Castle_Boss = 2030,
	// Token: 0x04002267 RID: 8807
	Johan_Finding_On_Bridge = 2040,
	// Token: 0x04002268 RID: 8808
	Johan_After_Beating_Bridge_Boss = 2050,
	// Token: 0x04002269 RID: 8809
	Johan_Sitting_At_Far_Shore = 2060,
	// Token: 0x0400226A RID: 8810
	Johan_After_Beating_Forest_Boss = 2070,
	// Token: 0x0400226B RID: 8811
	Johan_Reaching_Sun_Tower_Top = 2080,
	// Token: 0x0400226C RID: 8812
	Johan_After_Beating_Study_Boss = 2090,
	// Token: 0x0400226D RID: 8813
	Johan_After_Beating_Tower_Boss = 2100,
	// Token: 0x0400226E RID: 8814
	Johan_After_Giving_Insight_Quest_For_Lantern = 2109,
	// Token: 0x0400226F RID: 8815
	Johan_After_Getting_Heirloom_Lantern,
	// Token: 0x04002270 RID: 8816
	Rebel_Door_Opened = 3000,
	// Token: 0x04002271 RID: 8817
	FoundEggplant_Basic = 4000,
	// Token: 0x04002272 RID: 8818
	FoundEggplant_Advanced = 4010,
	// Token: 0x04002273 RID: 8819
	FoundEggplant_Expert = 4020,
	// Token: 0x04002274 RID: 8820
	FoundEggplant_Miniboss = 4030,
	// Token: 0x04002275 RID: 8821
	SeenParade = 5000,
	// Token: 0x04002276 RID: 8822
	SeenTrueEnding = 5005,
	// Token: 0x04002277 RID: 8823
	SeenTrueEnding_FirstTime,
	// Token: 0x04002278 RID: 8824
	SeenNGConfirmWindow = 5010,
	// Token: 0x04002279 RID: 8825
	PlayFarShoresWarning = 10000,
	// Token: 0x0400227A RID: 8826
	PlayArcaneHallowsWarning,
	// Token: 0x0400227B RID: 8827
	PlayDriftingWorldsWarning,
	// Token: 0x0400227C RID: 8828
	PlayPizzaMundiWarning,
	// Token: 0x0400227D RID: 8829
	PlayDragonsVowWarning
}
