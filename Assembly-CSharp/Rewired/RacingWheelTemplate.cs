using System;

namespace Rewired
{
	// Token: 0x02000EAA RID: 3754
	public sealed class RacingWheelTemplate : ControllerTemplate, IRacingWheelTemplate, IControllerTemplate
	{
		// Token: 0x170022B0 RID: 8880
		// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x0003AAED File Offset: 0x00038CED
		IControllerTemplateAxis IRacingWheelTemplate.wheel
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x170022B1 RID: 8881
		// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x0003AAF6 File Offset: 0x00038CF6
		IControllerTemplateAxis IRacingWheelTemplate.accelerator
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x170022B2 RID: 8882
		// (get) Token: 0x06006ABA RID: 27322 RVA: 0x0003AAFF File Offset: 0x00038CFF
		IControllerTemplateAxis IRacingWheelTemplate.brake
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x170022B3 RID: 8883
		// (get) Token: 0x06006ABB RID: 27323 RVA: 0x0003AB08 File Offset: 0x00038D08
		IControllerTemplateAxis IRacingWheelTemplate.clutch
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(3);
			}
		}

		// Token: 0x170022B4 RID: 8884
		// (get) Token: 0x06006ABC RID: 27324 RVA: 0x0003AA38 File Offset: 0x00038C38
		IControllerTemplateButton IRacingWheelTemplate.shiftDown
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x170022B5 RID: 8885
		// (get) Token: 0x06006ABD RID: 27325 RVA: 0x0003AA41 File Offset: 0x00038C41
		IControllerTemplateButton IRacingWheelTemplate.shiftUp
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x170022B6 RID: 8886
		// (get) Token: 0x06006ABE RID: 27326 RVA: 0x0003AA4A File Offset: 0x00038C4A
		IControllerTemplateButton IRacingWheelTemplate.wheelButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x170022B7 RID: 8887
		// (get) Token: 0x06006ABF RID: 27327 RVA: 0x0003AA53 File Offset: 0x00038C53
		IControllerTemplateButton IRacingWheelTemplate.wheelButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170022B8 RID: 8888
		// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x0003AA5C File Offset: 0x00038C5C
		IControllerTemplateButton IRacingWheelTemplate.wheelButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170022B9 RID: 8889
		// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x0003AA65 File Offset: 0x00038C65
		IControllerTemplateButton IRacingWheelTemplate.wheelButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170022BA RID: 8890
		// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x0003AA6F File Offset: 0x00038C6F
		IControllerTemplateButton IRacingWheelTemplate.wheelButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170022BB RID: 8891
		// (get) Token: 0x06006AC3 RID: 27331 RVA: 0x0003AB11 File Offset: 0x00038D11
		IControllerTemplateButton IRacingWheelTemplate.wheelButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170022BC RID: 8892
		// (get) Token: 0x06006AC4 RID: 27332 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton IRacingWheelTemplate.wheelButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170022BD RID: 8893
		// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x0003AB1B File Offset: 0x00038D1B
		IControllerTemplateButton IRacingWheelTemplate.wheelButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170022BE RID: 8894
		// (get) Token: 0x06006AC6 RID: 27334 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton IRacingWheelTemplate.wheelButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170022BF RID: 8895
		// (get) Token: 0x06006AC7 RID: 27335 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton IRacingWheelTemplate.wheelButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170022C0 RID: 8896
		// (get) Token: 0x06006AC8 RID: 27336 RVA: 0x0003AAAB File Offset: 0x00038CAB
		IControllerTemplateButton IRacingWheelTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170022C1 RID: 8897
		// (get) Token: 0x06006AC9 RID: 27337 RVA: 0x0003AB25 File Offset: 0x00038D25
		IControllerTemplateButton IRacingWheelTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x170022C2 RID: 8898
		// (get) Token: 0x06006ACA RID: 27338 RVA: 0x0003AB2F File Offset: 0x00038D2F
		IControllerTemplateButton IRacingWheelTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170022C3 RID: 8899
		// (get) Token: 0x06006ACB RID: 27339 RVA: 0x0003AB39 File Offset: 0x00038D39
		IControllerTemplateButton IRacingWheelTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170022C4 RID: 8900
		// (get) Token: 0x06006ACC RID: 27340 RVA: 0x0003AB43 File Offset: 0x00038D43
		IControllerTemplateButton IRacingWheelTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170022C5 RID: 8901
		// (get) Token: 0x06006ACD RID: 27341 RVA: 0x0003AB4D File Offset: 0x00038D4D
		IControllerTemplateButton IRacingWheelTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170022C6 RID: 8902
		// (get) Token: 0x06006ACE RID: 27342 RVA: 0x0003AB57 File Offset: 0x00038D57
		IControllerTemplateButton IRacingWheelTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170022C7 RID: 8903
		// (get) Token: 0x06006ACF RID: 27343 RVA: 0x0003AB61 File Offset: 0x00038D61
		IControllerTemplateButton IRacingWheelTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170022C8 RID: 8904
		// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x0003AB6B File Offset: 0x00038D6B
		IControllerTemplateButton IRacingWheelTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170022C9 RID: 8905
		// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x0003AB75 File Offset: 0x00038D75
		IControllerTemplateButton IRacingWheelTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170022CA RID: 8906
		// (get) Token: 0x06006AD2 RID: 27346 RVA: 0x0003AB7F File Offset: 0x00038D7F
		IControllerTemplateButton IRacingWheelTemplate.shifter1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170022CB RID: 8907
		// (get) Token: 0x06006AD3 RID: 27347 RVA: 0x0003AB89 File Offset: 0x00038D89
		IControllerTemplateButton IRacingWheelTemplate.shifter2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x170022CC RID: 8908
		// (get) Token: 0x06006AD4 RID: 27348 RVA: 0x0003AB93 File Offset: 0x00038D93
		IControllerTemplateButton IRacingWheelTemplate.shifter3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x170022CD RID: 8909
		// (get) Token: 0x06006AD5 RID: 27349 RVA: 0x0003AB9D File Offset: 0x00038D9D
		IControllerTemplateButton IRacingWheelTemplate.shifter4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x170022CE RID: 8910
		// (get) Token: 0x06006AD6 RID: 27350 RVA: 0x0003ABA7 File Offset: 0x00038DA7
		IControllerTemplateButton IRacingWheelTemplate.shifter5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x170022CF RID: 8911
		// (get) Token: 0x06006AD7 RID: 27351 RVA: 0x0003ABB1 File Offset: 0x00038DB1
		IControllerTemplateButton IRacingWheelTemplate.shifter6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x170022D0 RID: 8912
		// (get) Token: 0x06006AD8 RID: 27352 RVA: 0x0003ABBB File Offset: 0x00038DBB
		IControllerTemplateButton IRacingWheelTemplate.shifter7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(32);
			}
		}

		// Token: 0x170022D1 RID: 8913
		// (get) Token: 0x06006AD9 RID: 27353 RVA: 0x0003ABC5 File Offset: 0x00038DC5
		IControllerTemplateButton IRacingWheelTemplate.shifter8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(33);
			}
		}

		// Token: 0x170022D2 RID: 8914
		// (get) Token: 0x06006ADA RID: 27354 RVA: 0x0003ABCF File Offset: 0x00038DCF
		IControllerTemplateButton IRacingWheelTemplate.shifter9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(34);
			}
		}

		// Token: 0x170022D3 RID: 8915
		// (get) Token: 0x06006ADB RID: 27355 RVA: 0x0003ABD9 File Offset: 0x00038DD9
		IControllerTemplateButton IRacingWheelTemplate.shifter10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(35);
			}
		}

		// Token: 0x170022D4 RID: 8916
		// (get) Token: 0x06006ADC RID: 27356 RVA: 0x0003ABE3 File Offset: 0x00038DE3
		IControllerTemplateButton IRacingWheelTemplate.reverseGear
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170022D5 RID: 8917
		// (get) Token: 0x06006ADD RID: 27357 RVA: 0x0003ABED File Offset: 0x00038DED
		IControllerTemplateButton IRacingWheelTemplate.select
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(36);
			}
		}

		// Token: 0x170022D6 RID: 8918
		// (get) Token: 0x06006ADE RID: 27358 RVA: 0x0003ABF7 File Offset: 0x00038DF7
		IControllerTemplateButton IRacingWheelTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(37);
			}
		}

		// Token: 0x170022D7 RID: 8919
		// (get) Token: 0x06006ADF RID: 27359 RVA: 0x0003AC01 File Offset: 0x00038E01
		IControllerTemplateButton IRacingWheelTemplate.systemButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(38);
			}
		}

		// Token: 0x170022D8 RID: 8920
		// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x0003AC0B File Offset: 0x00038E0B
		IControllerTemplateButton IRacingWheelTemplate.horn
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x170022D9 RID: 8921
		// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x0003AC15 File Offset: 0x00038E15
		IControllerTemplateDPad IRacingWheelTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(45);
			}
		}

		// Token: 0x06006AE2 RID: 27362 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public RacingWheelTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04005613 RID: 22035
		public static readonly Guid typeGuid = new Guid("104e31d8-9115-4dd5-a398-2e54d35e6c83");

		// Token: 0x04005614 RID: 22036
		public const int elementId_wheel = 0;

		// Token: 0x04005615 RID: 22037
		public const int elementId_accelerator = 1;

		// Token: 0x04005616 RID: 22038
		public const int elementId_brake = 2;

		// Token: 0x04005617 RID: 22039
		public const int elementId_clutch = 3;

		// Token: 0x04005618 RID: 22040
		public const int elementId_shiftDown = 4;

		// Token: 0x04005619 RID: 22041
		public const int elementId_shiftUp = 5;

		// Token: 0x0400561A RID: 22042
		public const int elementId_wheelButton1 = 6;

		// Token: 0x0400561B RID: 22043
		public const int elementId_wheelButton2 = 7;

		// Token: 0x0400561C RID: 22044
		public const int elementId_wheelButton3 = 8;

		// Token: 0x0400561D RID: 22045
		public const int elementId_wheelButton4 = 9;

		// Token: 0x0400561E RID: 22046
		public const int elementId_wheelButton5 = 10;

		// Token: 0x0400561F RID: 22047
		public const int elementId_wheelButton6 = 11;

		// Token: 0x04005620 RID: 22048
		public const int elementId_wheelButton7 = 12;

		// Token: 0x04005621 RID: 22049
		public const int elementId_wheelButton8 = 13;

		// Token: 0x04005622 RID: 22050
		public const int elementId_wheelButton9 = 14;

		// Token: 0x04005623 RID: 22051
		public const int elementId_wheelButton10 = 15;

		// Token: 0x04005624 RID: 22052
		public const int elementId_consoleButton1 = 16;

		// Token: 0x04005625 RID: 22053
		public const int elementId_consoleButton2 = 17;

		// Token: 0x04005626 RID: 22054
		public const int elementId_consoleButton3 = 18;

		// Token: 0x04005627 RID: 22055
		public const int elementId_consoleButton4 = 19;

		// Token: 0x04005628 RID: 22056
		public const int elementId_consoleButton5 = 20;

		// Token: 0x04005629 RID: 22057
		public const int elementId_consoleButton6 = 21;

		// Token: 0x0400562A RID: 22058
		public const int elementId_consoleButton7 = 22;

		// Token: 0x0400562B RID: 22059
		public const int elementId_consoleButton8 = 23;

		// Token: 0x0400562C RID: 22060
		public const int elementId_consoleButton9 = 24;

		// Token: 0x0400562D RID: 22061
		public const int elementId_consoleButton10 = 25;

		// Token: 0x0400562E RID: 22062
		public const int elementId_shifter1 = 26;

		// Token: 0x0400562F RID: 22063
		public const int elementId_shifter2 = 27;

		// Token: 0x04005630 RID: 22064
		public const int elementId_shifter3 = 28;

		// Token: 0x04005631 RID: 22065
		public const int elementId_shifter4 = 29;

		// Token: 0x04005632 RID: 22066
		public const int elementId_shifter5 = 30;

		// Token: 0x04005633 RID: 22067
		public const int elementId_shifter6 = 31;

		// Token: 0x04005634 RID: 22068
		public const int elementId_shifter7 = 32;

		// Token: 0x04005635 RID: 22069
		public const int elementId_shifter8 = 33;

		// Token: 0x04005636 RID: 22070
		public const int elementId_shifter9 = 34;

		// Token: 0x04005637 RID: 22071
		public const int elementId_shifter10 = 35;

		// Token: 0x04005638 RID: 22072
		public const int elementId_reverseGear = 44;

		// Token: 0x04005639 RID: 22073
		public const int elementId_select = 36;

		// Token: 0x0400563A RID: 22074
		public const int elementId_start = 37;

		// Token: 0x0400563B RID: 22075
		public const int elementId_systemButton = 38;

		// Token: 0x0400563C RID: 22076
		public const int elementId_horn = 43;

		// Token: 0x0400563D RID: 22077
		public const int elementId_dPadUp = 39;

		// Token: 0x0400563E RID: 22078
		public const int elementId_dPadRight = 40;

		// Token: 0x0400563F RID: 22079
		public const int elementId_dPadDown = 41;

		// Token: 0x04005640 RID: 22080
		public const int elementId_dPadLeft = 42;

		// Token: 0x04005641 RID: 22081
		public const int elementId_dPad = 45;
	}
}
