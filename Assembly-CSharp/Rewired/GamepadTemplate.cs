using System;

namespace Rewired
{
	// Token: 0x02000EA9 RID: 3753
	public sealed class GamepadTemplate : ControllerTemplate, IGamepadTemplate, IControllerTemplate
	{
		// Token: 0x17002293 RID: 8851
		// (get) Token: 0x06006A99 RID: 27289 RVA: 0x0003AA38 File Offset: 0x00038C38
		IControllerTemplateButton IGamepadTemplate.actionBottomRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17002294 RID: 8852
		// (get) Token: 0x06006A9A RID: 27290 RVA: 0x0003AA38 File Offset: 0x00038C38
		IControllerTemplateButton IGamepadTemplate.a
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x17002295 RID: 8853
		// (get) Token: 0x06006A9B RID: 27291 RVA: 0x0003AA41 File Offset: 0x00038C41
		IControllerTemplateButton IGamepadTemplate.actionBottomRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17002296 RID: 8854
		// (get) Token: 0x06006A9C RID: 27292 RVA: 0x0003AA41 File Offset: 0x00038C41
		IControllerTemplateButton IGamepadTemplate.b
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x17002297 RID: 8855
		// (get) Token: 0x06006A9D RID: 27293 RVA: 0x0003AA4A File Offset: 0x00038C4A
		IControllerTemplateButton IGamepadTemplate.actionBottomRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17002298 RID: 8856
		// (get) Token: 0x06006A9E RID: 27294 RVA: 0x0003AA4A File Offset: 0x00038C4A
		IControllerTemplateButton IGamepadTemplate.c
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x17002299 RID: 8857
		// (get) Token: 0x06006A9F RID: 27295 RVA: 0x0003AA53 File Offset: 0x00038C53
		IControllerTemplateButton IGamepadTemplate.actionTopRow1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700229A RID: 8858
		// (get) Token: 0x06006AA0 RID: 27296 RVA: 0x0003AA53 File Offset: 0x00038C53
		IControllerTemplateButton IGamepadTemplate.x
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x1700229B RID: 8859
		// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x0003AA5C File Offset: 0x00038C5C
		IControllerTemplateButton IGamepadTemplate.actionTopRow2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700229C RID: 8860
		// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0003AA5C File Offset: 0x00038C5C
		IControllerTemplateButton IGamepadTemplate.y
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x1700229D RID: 8861
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0003AA65 File Offset: 0x00038C65
		IControllerTemplateButton IGamepadTemplate.actionTopRow3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x1700229E RID: 8862
		// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x0003AA65 File Offset: 0x00038C65
		IControllerTemplateButton IGamepadTemplate.z
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x1700229F RID: 8863
		// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x0003AA6F File Offset: 0x00038C6F
		IControllerTemplateButton IGamepadTemplate.leftShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170022A0 RID: 8864
		// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x0003AA6F File Offset: 0x00038C6F
		IControllerTemplateButton IGamepadTemplate.leftBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170022A1 RID: 8865
		// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x0003AA79 File Offset: 0x00038C79
		IControllerTemplateAxis IGamepadTemplate.leftShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x170022A2 RID: 8866
		// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x0003AA79 File Offset: 0x00038C79
		IControllerTemplateAxis IGamepadTemplate.leftTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x170022A3 RID: 8867
		// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton IGamepadTemplate.rightShoulder1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170022A4 RID: 8868
		// (get) Token: 0x06006AAA RID: 27306 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton IGamepadTemplate.rightBumper
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170022A5 RID: 8869
		// (get) Token: 0x06006AAB RID: 27307 RVA: 0x0003AA8D File Offset: 0x00038C8D
		IControllerTemplateAxis IGamepadTemplate.rightShoulder2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x170022A6 RID: 8870
		// (get) Token: 0x06006AAC RID: 27308 RVA: 0x0003AA8D File Offset: 0x00038C8D
		IControllerTemplateAxis IGamepadTemplate.rightTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(13);
			}
		}

		// Token: 0x170022A7 RID: 8871
		// (get) Token: 0x06006AAD RID: 27309 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton IGamepadTemplate.center1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170022A8 RID: 8872
		// (get) Token: 0x06006AAE RID: 27310 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton IGamepadTemplate.back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170022A9 RID: 8873
		// (get) Token: 0x06006AAF RID: 27311 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton IGamepadTemplate.center2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170022AA RID: 8874
		// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton IGamepadTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170022AB RID: 8875
		// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x0003AAAB File Offset: 0x00038CAB
		IControllerTemplateButton IGamepadTemplate.center3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170022AC RID: 8876
		// (get) Token: 0x06006AB2 RID: 27314 RVA: 0x0003AAAB File Offset: 0x00038CAB
		IControllerTemplateButton IGamepadTemplate.guide
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170022AD RID: 8877
		// (get) Token: 0x06006AB3 RID: 27315 RVA: 0x0003AAB5 File Offset: 0x00038CB5
		IControllerTemplateThumbStick IGamepadTemplate.leftStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(23);
			}
		}

		// Token: 0x170022AE RID: 8878
		// (get) Token: 0x06006AB4 RID: 27316 RVA: 0x0003AABF File Offset: 0x00038CBF
		IControllerTemplateThumbStick IGamepadTemplate.rightStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(24);
			}
		}

		// Token: 0x170022AF RID: 8879
		// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x0003AAC9 File Offset: 0x00038CC9
		IControllerTemplateDPad IGamepadTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(25);
			}
		}

		// Token: 0x06006AB6 RID: 27318 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public GamepadTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040055EB RID: 21995
		public static readonly Guid typeGuid = new Guid("83b427e4-086f-47f3-bb06-be266abd1ca5");

		// Token: 0x040055EC RID: 21996
		public const int elementId_leftStickX = 0;

		// Token: 0x040055ED RID: 21997
		public const int elementId_leftStickY = 1;

		// Token: 0x040055EE RID: 21998
		public const int elementId_rightStickX = 2;

		// Token: 0x040055EF RID: 21999
		public const int elementId_rightStickY = 3;

		// Token: 0x040055F0 RID: 22000
		public const int elementId_actionBottomRow1 = 4;

		// Token: 0x040055F1 RID: 22001
		public const int elementId_a = 4;

		// Token: 0x040055F2 RID: 22002
		public const int elementId_actionBottomRow2 = 5;

		// Token: 0x040055F3 RID: 22003
		public const int elementId_b = 5;

		// Token: 0x040055F4 RID: 22004
		public const int elementId_actionBottomRow3 = 6;

		// Token: 0x040055F5 RID: 22005
		public const int elementId_c = 6;

		// Token: 0x040055F6 RID: 22006
		public const int elementId_actionTopRow1 = 7;

		// Token: 0x040055F7 RID: 22007
		public const int elementId_x = 7;

		// Token: 0x040055F8 RID: 22008
		public const int elementId_actionTopRow2 = 8;

		// Token: 0x040055F9 RID: 22009
		public const int elementId_y = 8;

		// Token: 0x040055FA RID: 22010
		public const int elementId_actionTopRow3 = 9;

		// Token: 0x040055FB RID: 22011
		public const int elementId_z = 9;

		// Token: 0x040055FC RID: 22012
		public const int elementId_leftShoulder1 = 10;

		// Token: 0x040055FD RID: 22013
		public const int elementId_leftBumper = 10;

		// Token: 0x040055FE RID: 22014
		public const int elementId_leftShoulder2 = 11;

		// Token: 0x040055FF RID: 22015
		public const int elementId_leftTrigger = 11;

		// Token: 0x04005600 RID: 22016
		public const int elementId_rightShoulder1 = 12;

		// Token: 0x04005601 RID: 22017
		public const int elementId_rightBumper = 12;

		// Token: 0x04005602 RID: 22018
		public const int elementId_rightShoulder2 = 13;

		// Token: 0x04005603 RID: 22019
		public const int elementId_rightTrigger = 13;

		// Token: 0x04005604 RID: 22020
		public const int elementId_center1 = 14;

		// Token: 0x04005605 RID: 22021
		public const int elementId_back = 14;

		// Token: 0x04005606 RID: 22022
		public const int elementId_center2 = 15;

		// Token: 0x04005607 RID: 22023
		public const int elementId_start = 15;

		// Token: 0x04005608 RID: 22024
		public const int elementId_center3 = 16;

		// Token: 0x04005609 RID: 22025
		public const int elementId_guide = 16;

		// Token: 0x0400560A RID: 22026
		public const int elementId_leftStickButton = 17;

		// Token: 0x0400560B RID: 22027
		public const int elementId_rightStickButton = 18;

		// Token: 0x0400560C RID: 22028
		public const int elementId_dPadUp = 19;

		// Token: 0x0400560D RID: 22029
		public const int elementId_dPadRight = 20;

		// Token: 0x0400560E RID: 22030
		public const int elementId_dPadDown = 21;

		// Token: 0x0400560F RID: 22031
		public const int elementId_dPadLeft = 22;

		// Token: 0x04005610 RID: 22032
		public const int elementId_leftStick = 23;

		// Token: 0x04005611 RID: 22033
		public const int elementId_rightStick = 24;

		// Token: 0x04005612 RID: 22034
		public const int elementId_dPad = 25;
	}
}
