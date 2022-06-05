using System;

// Token: 0x02000771 RID: 1905
public enum SkillTreeType
{
	// Token: 0x040036B0 RID: 14000
	None,
	// Token: 0x040036B1 RID: 14001
	Smithy = 10,
	// Token: 0x040036B2 RID: 14002
	Enchantress = 20,
	// Token: 0x040036B3 RID: 14003
	Architect = 30,
	// Token: 0x040036B4 RID: 14004
	Banker = 40,
	// Token: 0x040036B5 RID: 14005
	Health_Up = 50,
	// Token: 0x040036B6 RID: 14006
	Health_Up2,
	// Token: 0x040036B7 RID: 14007
	Health_Up3,
	// Token: 0x040036B8 RID: 14008
	Death_Dodge = 60,
	// Token: 0x040036B9 RID: 14009
	Potion_Up = 70,
	// Token: 0x040036BA RID: 14010
	Invuln_Time_Up = 80,
	// Token: 0x040036BB RID: 14011
	Attack_Up = 90,
	// Token: 0x040036BC RID: 14012
	Attack_Up2,
	// Token: 0x040036BD RID: 14013
	Attack_Up3,
	// Token: 0x040036BE RID: 14014
	Down_Strike_Up = 100,
	// Token: 0x040036BF RID: 14015
	Dexterity_Add1 = 110,
	// Token: 0x040036C0 RID: 14016
	Dexterity_Add2,
	// Token: 0x040036C1 RID: 14017
	Dexterity_Add3,
	// Token: 0x040036C2 RID: 14018
	Crit_Damage_Up = 120,
	// Token: 0x040036C3 RID: 14019
	Magic_Attack_Up = 130,
	// Token: 0x040036C4 RID: 14020
	Magic_Attack_Up2,
	// Token: 0x040036C5 RID: 14021
	Magic_Attack_Up3,
	// Token: 0x040036C6 RID: 14022
	Focus_Up1 = 140,
	// Token: 0x040036C7 RID: 14023
	Focus_Up2,
	// Token: 0x040036C8 RID: 14024
	Focus_Up3,
	// Token: 0x040036C9 RID: 14025
	Magic_Crit_Damage_Up = 150,
	// Token: 0x040036CA RID: 14026
	Cooldown_Reduction_Up = 160,
	// Token: 0x040036CB RID: 14027
	Equip_Up = 170,
	// Token: 0x040036CC RID: 14028
	Equip_Up2,
	// Token: 0x040036CD RID: 14029
	Equip_Up3,
	// Token: 0x040036CE RID: 14030
	Rune_Equip_Up = 180,
	// Token: 0x040036CF RID: 14031
	Rune_Equip_Up2,
	// Token: 0x040036D0 RID: 14032
	Rune_Equip_Up3,
	// Token: 0x040036D1 RID: 14033
	Armor_Up = 190,
	// Token: 0x040036D2 RID: 14034
	Armor_Up2,
	// Token: 0x040036D3 RID: 14035
	Armor_Up3,
	// Token: 0x040036D4 RID: 14036
	Traits_Give_Gold = 200,
	// Token: 0x040036D5 RID: 14037
	Traits_Give_Gold_Gain_Mod = 205,
	// Token: 0x040036D6 RID: 14038
	Equipment_Ore_Find_Up = 210,
	// Token: 0x040036D7 RID: 14039
	Rune_Ore_Find_Up = 212,
	// Token: 0x040036D8 RID: 14040
	Gold_Gain_Up = 220,
	// Token: 0x040036D9 RID: 14041
	Gold_Gain_Up_2 = 230,
	// Token: 0x040036DA RID: 14042
	Gold_Gain_Up_3 = 240,
	// Token: 0x040036DB RID: 14043
	Gold_Gain_Up_4 = 250,
	// Token: 0x040036DC RID: 14044
	Gold_Gain_Up_5 = 260,
	// Token: 0x040036DD RID: 14045
	Randomize_Children = 270,
	// Token: 0x040036DE RID: 14046
	Weight_CD_Reduce = 280,
	// Token: 0x040036DF RID: 14047
	Mana_Cost_Down = 290,
	// Token: 0x040036E0 RID: 14048
	Gold_Saved_Unlock = 295,
	// Token: 0x040036E1 RID: 14049
	Gold_Saved_Cap_Up = 300,
	// Token: 0x040036E2 RID: 14050
	Gold_Saved_Amount_Saved = 310,
	// Token: 0x040036E3 RID: 14051
	BoxingGlove_Class_Unlock = 320,
	// Token: 0x040036E4 RID: 14052
	Saber_Class_Unlock = 330,
	// Token: 0x040036E5 RID: 14053
	DualBlades_Class_Unlock = 340,
	// Token: 0x040036E6 RID: 14054
	Architect_Cost_Down = 350,
	// Token: 0x040036E7 RID: 14055
	Polymorph_Class_Unlock = 360,
	// Token: 0x040036E8 RID: 14056
	More_Children = 370,
	// Token: 0x040036E9 RID: 14057
	Ladle_Class_Unlock = 380,
	// Token: 0x040036EA RID: 14058
	Chakram_Class_Unlock = 390,
	// Token: 0x040036EB RID: 14059
	Tonfa_Class_Unlock = 400,
	// Token: 0x040036EC RID: 14060
	Sword_Class_Unlock = 410,
	// Token: 0x040036ED RID: 14061
	Axe_Class_Unlock = 420,
	// Token: 0x040036EE RID: 14062
	Wand_Class_Unlock = 430,
	// Token: 0x040036EF RID: 14063
	Bow_Class_Unlock = 440,
	// Token: 0x040036F0 RID: 14064
	Spear_Class_Unlock = 450,
	// Token: 0x040036F1 RID: 14065
	Kunai_Class_Unlock = 460,
	// Token: 0x040036F2 RID: 14066
	Siphon_Class_Unlock = 470,
	// Token: 0x040036F3 RID: 14067
	Cane_Class_Unlock = 480,
	// Token: 0x040036F4 RID: 14068
	Gun_Class_Unlock = 490,
	// Token: 0x040036F5 RID: 14069
	Samurai_Class_Unlock = 492,
	// Token: 0x040036F6 RID: 14070
	Music_Class_Unlock,
	// Token: 0x040036F7 RID: 14071
	Pirate_Class_Unlock,
	// Token: 0x040036F8 RID: 14072
	Astro_Class_Unlock,
	// Token: 0x040036F9 RID: 14073
	Weapon_Master_Upgrade = 500,
	// Token: 0x040036FA RID: 14074
	Knight_Upgrade = 510,
	// Token: 0x040036FB RID: 14075
	Lancer_Class_Unlock = 520,
	// Token: 0x040036FC RID: 14076
	XP_Up = 530,
	// Token: 0x040036FD RID: 14077
	Equipment_Ore_Gain_Up = 540,
	// Token: 0x040036FE RID: 14078
	Rune_Ore_Gain_Up = 550,
	// Token: 0x040036FF RID: 14079
	Potions_Free_Cast_Up = 560,
	// Token: 0x04003700 RID: 14080
	Unlock_Dummy = 570,
	// Token: 0x04003701 RID: 14081
	Boss_Health_Restore = 580,
	// Token: 0x04003702 RID: 14082
	Unlock_Totem = 590,
	// Token: 0x04003703 RID: 14083
	Relic_Cost_Down = 600,
	// Token: 0x04003704 RID: 14084
	Reroll_Relic = 610,
	// Token: 0x04003705 RID: 14085
	Potion_Recharge_Talent = 620,
	// Token: 0x04003706 RID: 14086
	Resolve_Up = 630,
	// Token: 0x04003707 RID: 14087
	Dash_Strike_Up = 640,
	// Token: 0x04003708 RID: 14088
	Charon_Gold_Stat_Bonus = 650,
	// Token: 0x04003709 RID: 14089
	Crit_Chance_Flat_Up = 660,
	// Token: 0x0400370A RID: 14090
	Magic_Crit_Chance_Flat_Up = 670,
	// Token: 0x0400370B RID: 14091
	Reroll_Relic_Room_Cap = 680,
	// Token: 0x0400370C RID: 14092
	LabourCosts_Unlocked = 10000,
	// Token: 0x0400370D RID: 14093
	PizzaGirl_Unlocked = 100010
}
