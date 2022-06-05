using System;

// Token: 0x02000C32 RID: 3122
public enum SkillTreeType
{
	// Token: 0x04004960 RID: 18784
	None,
	// Token: 0x04004961 RID: 18785
	Smithy = 10,
	// Token: 0x04004962 RID: 18786
	Enchantress = 20,
	// Token: 0x04004963 RID: 18787
	Architect = 30,
	// Token: 0x04004964 RID: 18788
	Banker = 40,
	// Token: 0x04004965 RID: 18789
	Health_Up = 50,
	// Token: 0x04004966 RID: 18790
	Health_Up2,
	// Token: 0x04004967 RID: 18791
	Health_Up3,
	// Token: 0x04004968 RID: 18792
	Death_Dodge = 60,
	// Token: 0x04004969 RID: 18793
	Potion_Up = 70,
	// Token: 0x0400496A RID: 18794
	Invuln_Time_Up = 80,
	// Token: 0x0400496B RID: 18795
	Attack_Up = 90,
	// Token: 0x0400496C RID: 18796
	Attack_Up2,
	// Token: 0x0400496D RID: 18797
	Attack_Up3,
	// Token: 0x0400496E RID: 18798
	Down_Strike_Up = 100,
	// Token: 0x0400496F RID: 18799
	Dexterity_Add1 = 110,
	// Token: 0x04004970 RID: 18800
	Dexterity_Add2,
	// Token: 0x04004971 RID: 18801
	Dexterity_Add3,
	// Token: 0x04004972 RID: 18802
	Crit_Damage_Up = 120,
	// Token: 0x04004973 RID: 18803
	Magic_Attack_Up = 130,
	// Token: 0x04004974 RID: 18804
	Magic_Attack_Up2,
	// Token: 0x04004975 RID: 18805
	Magic_Attack_Up3,
	// Token: 0x04004976 RID: 18806
	Focus_Up1 = 140,
	// Token: 0x04004977 RID: 18807
	Focus_Up2,
	// Token: 0x04004978 RID: 18808
	Focus_Up3,
	// Token: 0x04004979 RID: 18809
	Magic_Crit_Damage_Up = 150,
	// Token: 0x0400497A RID: 18810
	Cooldown_Reduction_Up = 160,
	// Token: 0x0400497B RID: 18811
	Equip_Up = 170,
	// Token: 0x0400497C RID: 18812
	Equip_Up2,
	// Token: 0x0400497D RID: 18813
	Equip_Up3,
	// Token: 0x0400497E RID: 18814
	Rune_Equip_Up = 180,
	// Token: 0x0400497F RID: 18815
	Rune_Equip_Up2,
	// Token: 0x04004980 RID: 18816
	Rune_Equip_Up3,
	// Token: 0x04004981 RID: 18817
	Armor_Up = 190,
	// Token: 0x04004982 RID: 18818
	Armor_Up2,
	// Token: 0x04004983 RID: 18819
	Armor_Up3,
	// Token: 0x04004984 RID: 18820
	Traits_Give_Gold = 200,
	// Token: 0x04004985 RID: 18821
	Traits_Give_Gold_Gain_Mod = 205,
	// Token: 0x04004986 RID: 18822
	Equipment_Ore_Find_Up = 210,
	// Token: 0x04004987 RID: 18823
	Rune_Ore_Find_Up = 212,
	// Token: 0x04004988 RID: 18824
	Gold_Gain_Up = 220,
	// Token: 0x04004989 RID: 18825
	Gold_Gain_Up_2 = 230,
	// Token: 0x0400498A RID: 18826
	Gold_Gain_Up_3 = 240,
	// Token: 0x0400498B RID: 18827
	Gold_Gain_Up_4 = 250,
	// Token: 0x0400498C RID: 18828
	Gold_Gain_Up_5 = 260,
	// Token: 0x0400498D RID: 18829
	Randomize_Children = 270,
	// Token: 0x0400498E RID: 18830
	Weight_CD_Reduce = 280,
	// Token: 0x0400498F RID: 18831
	Mana_Cost_Down = 290,
	// Token: 0x04004990 RID: 18832
	Gold_Saved_Unlock = 295,
	// Token: 0x04004991 RID: 18833
	Gold_Saved_Cap_Up = 300,
	// Token: 0x04004992 RID: 18834
	Gold_Saved_Amount_Saved = 310,
	// Token: 0x04004993 RID: 18835
	BoxingGlove_Class_Unlock = 320,
	// Token: 0x04004994 RID: 18836
	Saber_Class_Unlock = 330,
	// Token: 0x04004995 RID: 18837
	DualBlades_Class_Unlock = 340,
	// Token: 0x04004996 RID: 18838
	Architect_Cost_Down = 350,
	// Token: 0x04004997 RID: 18839
	Polymorph_Class_Unlock = 360,
	// Token: 0x04004998 RID: 18840
	More_Children = 370,
	// Token: 0x04004999 RID: 18841
	Ladle_Class_Unlock = 380,
	// Token: 0x0400499A RID: 18842
	Chakram_Class_Unlock = 390,
	// Token: 0x0400499B RID: 18843
	Tonfa_Class_Unlock = 400,
	// Token: 0x0400499C RID: 18844
	Sword_Class_Unlock = 410,
	// Token: 0x0400499D RID: 18845
	Axe_Class_Unlock = 420,
	// Token: 0x0400499E RID: 18846
	Wand_Class_Unlock = 430,
	// Token: 0x0400499F RID: 18847
	Bow_Class_Unlock = 440,
	// Token: 0x040049A0 RID: 18848
	Spear_Class_Unlock = 450,
	// Token: 0x040049A1 RID: 18849
	Kunai_Class_Unlock = 460,
	// Token: 0x040049A2 RID: 18850
	Siphon_Class_Unlock = 470,
	// Token: 0x040049A3 RID: 18851
	Cane_Class_Unlock = 480,
	// Token: 0x040049A4 RID: 18852
	Gun_Class_Unlock = 490,
	// Token: 0x040049A5 RID: 18853
	Samurai_Class_Unlock = 492,
	// Token: 0x040049A6 RID: 18854
	Music_Class_Unlock,
	// Token: 0x040049A7 RID: 18855
	Pirate_Class_Unlock,
	// Token: 0x040049A8 RID: 18856
	Astro_Class_Unlock,
	// Token: 0x040049A9 RID: 18857
	Weapon_Master_Upgrade = 500,
	// Token: 0x040049AA RID: 18858
	Knight_Upgrade = 510,
	// Token: 0x040049AB RID: 18859
	Lancer_Class_Unlock = 520,
	// Token: 0x040049AC RID: 18860
	XP_Up = 530,
	// Token: 0x040049AD RID: 18861
	Equipment_Ore_Gain_Up = 540,
	// Token: 0x040049AE RID: 18862
	Rune_Ore_Gain_Up = 550,
	// Token: 0x040049AF RID: 18863
	Potions_Free_Cast_Up = 560,
	// Token: 0x040049B0 RID: 18864
	Unlock_Dummy = 570,
	// Token: 0x040049B1 RID: 18865
	Boss_Health_Restore = 580,
	// Token: 0x040049B2 RID: 18866
	Unlock_Totem = 590,
	// Token: 0x040049B3 RID: 18867
	Relic_Cost_Down = 600,
	// Token: 0x040049B4 RID: 18868
	Reroll_Relic = 610,
	// Token: 0x040049B5 RID: 18869
	Potion_Recharge_Talent = 620,
	// Token: 0x040049B6 RID: 18870
	Resolve_Up = 630,
	// Token: 0x040049B7 RID: 18871
	Dash_Strike_Up = 640,
	// Token: 0x040049B8 RID: 18872
	Charon_Gold_Stat_Bonus = 650,
	// Token: 0x040049B9 RID: 18873
	Crit_Chance_Flat_Up = 660,
	// Token: 0x040049BA RID: 18874
	Magic_Crit_Chance_Flat_Up = 670,
	// Token: 0x040049BB RID: 18875
	Reroll_Relic_Room_Cap = 680,
	// Token: 0x040049BC RID: 18876
	LabourCosts_Unlocked = 10000,
	// Token: 0x040049BD RID: 18877
	PizzaGirl_Unlocked = 100010
}
