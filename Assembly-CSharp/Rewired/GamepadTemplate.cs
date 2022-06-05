using System;

namespace Rewired
{
	// Token: 0x0200092A RID: 2346
	public sealed class GamepadTemplate : ControllerTemplate, IGamepadTemplate, IControllerTemplate
	{
		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x06004D92 RID: 19858 RVA: 0x00112E74 File Offset: 0x00111074
		IControllerTemplateButton IGamepadTemplate.actionBottomRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x06004D93 RID: 19859 RVA: 0x00112E7D File Offset: 0x0011107D
		IControllerTemplateButton IGamepadTemplate.a
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x06004D94 RID: 19860 RVA: 0x00112E86 File Offset: 0x00111086
		IControllerTemplateButton IGamepadTemplate.actionBottomRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x06004D95 RID: 19861 RVA: 0x00112E8F File Offset: 0x0011108F
		IControllerTemplateButton IGamepadTemplate.b
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x06004D96 RID: 19862 RVA: 0x00112E98 File Offset: 0x00111098
		IControllerTemplateButton IGamepadTemplate.actionBottomRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x06004D97 RID: 19863 RVA: 0x00112EA1 File Offset: 0x001110A1
		IControllerTemplateButton IGamepadTemplate.c
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x00112EAA File Offset: 0x001110AA
		IControllerTemplateButton IGamepadTemplate.actionTopRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x06004D99 RID: 19865 RVA: 0x00112EB3 File Offset: 0x001110B3
		IControllerTemplateButton IGamepadTemplate.x
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06004D9A RID: 19866 RVA: 0x00112EBC File Offset: 0x001110BC
		IControllerTemplateButton IGamepadTemplate.actionTopRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x00112EC5 File Offset: 0x001110C5
		IControllerTemplateButton IGamepadTemplate.y
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x00112ECE File Offset: 0x001110CE
		IControllerTemplateButton IGamepadTemplate.actionTopRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x00112ED8 File Offset: 0x001110D8
		IControllerTemplateButton IGamepadTemplate.z
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x00112EE2 File Offset: 0x001110E2
		IControllerTemplateButton IGamepadTemplate.leftShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x00112EEC File Offset: 0x001110EC
		IControllerTemplateButton IGamepadTemplate.leftBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x06004DA0 RID: 19872 RVA: 0x00112EF6 File Offset: 0x001110F6
		IControllerTemplateAxis IGamepadTemplate.leftShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x00112F00 File Offset: 0x00111100
		IControllerTemplateAxis IGamepadTemplate.leftTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x06004DA2 RID: 19874 RVA: 0x00112F0A File Offset: 0x0011110A
		IControllerTemplateButton IGamepadTemplate.rightShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x00112F14 File Offset: 0x00111114
		IControllerTemplateButton IGamepadTemplate.rightBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x06004DA4 RID: 19876 RVA: 0x00112F1E File Offset: 0x0011111E
		IControllerTemplateAxis IGamepadTemplate.rightShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x06004DA5 RID: 19877 RVA: 0x00112F28 File Offset: 0x00111128
		IControllerTemplateAxis IGamepadTemplate.rightTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x06004DA6 RID: 19878 RVA: 0x00112F32 File Offset: 0x00111132
		IControllerTemplateButton IGamepadTemplate.center1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x06004DA7 RID: 19879 RVA: 0x00112F3C File Offset: 0x0011113C
		IControllerTemplateButton IGamepadTemplate.back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x00112F46 File Offset: 0x00111146
		IControllerTemplateButton IGamepadTemplate.center2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x00112F50 File Offset: 0x00111150
		IControllerTemplateButton IGamepadTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x06004DAA RID: 19882 RVA: 0x00112F5A File Offset: 0x0011115A
		IControllerTemplateButton IGamepadTemplate.center3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x00112F64 File Offset: 0x00111164
		IControllerTemplateButton IGamepadTemplate.guide
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x06004DAC RID: 19884 RVA: 0x00112F6E File Offset: 0x0011116E
		IControllerTemplateThumbStick IGamepadTemplate.leftStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(23);
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x06004DAD RID: 19885 RVA: 0x00112F78 File Offset: 0x00111178
		IControllerTemplateThumbStick IGamepadTemplate.rightStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(24);
			}
		}

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x06004DAE RID: 19886 RVA: 0x00112F82 File Offset: 0x00111182
		IControllerTemplateDPad IGamepadTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(25);
			}
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x00112F8C File Offset: 0x0011118C
		public GamepadTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x0400407F RID: 16511
		public static readonly Guid typeGuid = new Guid("83b427e4-086f-47f3-bb06-be266abd1ca5");

		// Token: 0x04004080 RID: 16512
		public const int elementId_leftStickX = 0;

		// Token: 0x04004081 RID: 16513
		public const int elementId_leftStickY = 1;

		// Token: 0x04004082 RID: 16514
		public const int elementId_rightStickX = 2;

		// Token: 0x04004083 RID: 16515
		public const int elementId_rightStickY = 3;

		// Token: 0x04004084 RID: 16516
		public const int elementId_actionBottomRow1 = 4;

		// Token: 0x04004085 RID: 16517
		public const int elementId_a = 4;

		// Token: 0x04004086 RID: 16518
		public const int elementId_actionBottomRow2 = 5;

		// Token: 0x04004087 RID: 16519
		public const int elementId_b = 5;

		// Token: 0x04004088 RID: 16520
		public const int elementId_actionBottomRow3 = 6;

		// Token: 0x04004089 RID: 16521
		public const int elementId_c = 6;

		// Token: 0x0400408A RID: 16522
		public const int elementId_actionTopRow1 = 7;

		// Token: 0x0400408B RID: 16523
		public const int elementId_x = 7;

		// Token: 0x0400408C RID: 16524
		public const int elementId_actionTopRow2 = 8;

		// Token: 0x0400408D RID: 16525
		public const int elementId_y = 8;

		// Token: 0x0400408E RID: 16526
		public const int elementId_actionTopRow3 = 9;

		// Token: 0x0400408F RID: 16527
		public const int elementId_z = 9;

		// Token: 0x04004090 RID: 16528
		public const int elementId_leftShoulder1 = 10;

		// Token: 0x04004091 RID: 16529
		public const int elementId_leftBumper = 10;

		// Token: 0x04004092 RID: 16530
		public const int elementId_leftShoulder2 = 11;

		// Token: 0x04004093 RID: 16531
		public const int elementId_leftTrigger = 11;

		// Token: 0x04004094 RID: 16532
		public const int elementId_rightShoulder1 = 12;

		// Token: 0x04004095 RID: 16533
		public const int elementId_rightBumper = 12;

		// Token: 0x04004096 RID: 16534
		public const int elementId_rightShoulder2 = 13;

		// Token: 0x04004097 RID: 16535
		public const int elementId_rightTrigger = 13;

		// Token: 0x04004098 RID: 16536
		public const int elementId_center1 = 14;

		// Token: 0x04004099 RID: 16537
		public const int elementId_back = 14;

		// Token: 0x0400409A RID: 16538
		public const int elementId_center2 = 15;

		// Token: 0x0400409B RID: 16539
		public const int elementId_start = 15;

		// Token: 0x0400409C RID: 16540
		public const int elementId_center3 = 16;

		// Token: 0x0400409D RID: 16541
		public const int elementId_guide = 16;

		// Token: 0x0400409E RID: 16542
		public const int elementId_leftStickButton = 17;

		// Token: 0x0400409F RID: 16543
		public const int elementId_rightStickButton = 18;

		// Token: 0x040040A0 RID: 16544
		public const int elementId_dPadUp = 19;

		// Token: 0x040040A1 RID: 16545
		public const int elementId_dPadRight = 20;

		// Token: 0x040040A2 RID: 16546
		public const int elementId_dPadDown = 21;

		// Token: 0x040040A3 RID: 16547
		public const int elementId_dPadLeft = 22;

		// Token: 0x040040A4 RID: 16548
		public const int elementId_leftStick = 23;

		// Token: 0x040040A5 RID: 16549
		public const int elementId_rightStick = 24;

		// Token: 0x040040A6 RID: 16550
		public const int elementId_dPad = 25;
	}
}
