using System;

namespace Rewired
{
	// Token: 0x02000EAC RID: 3756
	public sealed class FlightYokeTemplate : ControllerTemplate, IFlightYokeTemplate, IControllerTemplate
	{
		// Token: 0x17002332 RID: 9010
		// (get) Token: 0x06006B3E RID: 27454 RVA: 0x0003ACCE File Offset: 0x00038ECE
		IControllerTemplateButton IFlightYokeTemplate.leftPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17002333 RID: 9011
		// (get) Token: 0x06006B3F RID: 27455 RVA: 0x0003ACD8 File Offset: 0x00038ED8
		IControllerTemplateButton IFlightYokeTemplate.rightPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17002334 RID: 9012
		// (get) Token: 0x06006B40 RID: 27456 RVA: 0x0003AA53 File Offset: 0x00038C53
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17002335 RID: 9013
		// (get) Token: 0x06006B41 RID: 27457 RVA: 0x0003AA5C File Offset: 0x00038C5C
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x17002336 RID: 9014
		// (get) Token: 0x06006B42 RID: 27458 RVA: 0x0003AA65 File Offset: 0x00038C65
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17002337 RID: 9015
		// (get) Token: 0x06006B43 RID: 27459 RVA: 0x0003AA6F File Offset: 0x00038C6F
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17002338 RID: 9016
		// (get) Token: 0x06006B44 RID: 27460 RVA: 0x0003AB11 File Offset: 0x00038D11
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x17002339 RID: 9017
		// (get) Token: 0x06006B45 RID: 27461 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x1700233A RID: 9018
		// (get) Token: 0x06006B46 RID: 27462 RVA: 0x0003AB1B File Offset: 0x00038D1B
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x1700233B RID: 9019
		// (get) Token: 0x06006B47 RID: 27463 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700233C RID: 9020
		// (get) Token: 0x06006B48 RID: 27464 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700233D RID: 9021
		// (get) Token: 0x06006B49 RID: 27465 RVA: 0x0003AAAB File Offset: 0x00038CAB
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700233E RID: 9022
		// (get) Token: 0x06006B4A RID: 27466 RVA: 0x0003AB25 File Offset: 0x00038D25
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x1700233F RID: 9023
		// (get) Token: 0x06006B4B RID: 27467 RVA: 0x0003AB2F File Offset: 0x00038D2F
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17002340 RID: 9024
		// (get) Token: 0x06006B4C RID: 27468 RVA: 0x0003AB39 File Offset: 0x00038D39
		IControllerTemplateButton IFlightYokeTemplate.centerButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17002341 RID: 9025
		// (get) Token: 0x06006B4D RID: 27469 RVA: 0x0003AB43 File Offset: 0x00038D43
		IControllerTemplateButton IFlightYokeTemplate.centerButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17002342 RID: 9026
		// (get) Token: 0x06006B4E RID: 27470 RVA: 0x0003AB4D File Offset: 0x00038D4D
		IControllerTemplateButton IFlightYokeTemplate.centerButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17002343 RID: 9027
		// (get) Token: 0x06006B4F RID: 27471 RVA: 0x0003AB57 File Offset: 0x00038D57
		IControllerTemplateButton IFlightYokeTemplate.centerButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17002344 RID: 9028
		// (get) Token: 0x06006B50 RID: 27472 RVA: 0x0003AB61 File Offset: 0x00038D61
		IControllerTemplateButton IFlightYokeTemplate.centerButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17002345 RID: 9029
		// (get) Token: 0x06006B51 RID: 27473 RVA: 0x0003AB6B File Offset: 0x00038D6B
		IControllerTemplateButton IFlightYokeTemplate.centerButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17002346 RID: 9030
		// (get) Token: 0x06006B52 RID: 27474 RVA: 0x0003AB75 File Offset: 0x00038D75
		IControllerTemplateButton IFlightYokeTemplate.centerButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17002347 RID: 9031
		// (get) Token: 0x06006B53 RID: 27475 RVA: 0x0003AB7F File Offset: 0x00038D7F
		IControllerTemplateButton IFlightYokeTemplate.centerButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17002348 RID: 9032
		// (get) Token: 0x06006B54 RID: 27476 RVA: 0x0003AC92 File Offset: 0x00038E92
		IControllerTemplateButton IFlightYokeTemplate.wheel1Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x17002349 RID: 9033
		// (get) Token: 0x06006B55 RID: 27477 RVA: 0x0003AC9C File Offset: 0x00038E9C
		IControllerTemplateButton IFlightYokeTemplate.wheel1Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x1700234A RID: 9034
		// (get) Token: 0x06006B56 RID: 27478 RVA: 0x0003ACA6 File Offset: 0x00038EA6
		IControllerTemplateButton IFlightYokeTemplate.wheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x1700234B RID: 9035
		// (get) Token: 0x06006B57 RID: 27479 RVA: 0x0003ACB0 File Offset: 0x00038EB0
		IControllerTemplateButton IFlightYokeTemplate.wheel2Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x1700234C RID: 9036
		// (get) Token: 0x06006B58 RID: 27480 RVA: 0x0003ACBA File Offset: 0x00038EBA
		IControllerTemplateButton IFlightYokeTemplate.wheel2Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x1700234D RID: 9037
		// (get) Token: 0x06006B59 RID: 27481 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		IControllerTemplateButton IFlightYokeTemplate.wheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x1700234E RID: 9038
		// (get) Token: 0x06006B5A RID: 27482 RVA: 0x0003AC0B File Offset: 0x00038E0B
		IControllerTemplateButton IFlightYokeTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x1700234F RID: 9039
		// (get) Token: 0x06006B5B RID: 27483 RVA: 0x0003ABE3 File Offset: 0x00038DE3
		IControllerTemplateButton IFlightYokeTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17002350 RID: 9040
		// (get) Token: 0x06006B5C RID: 27484 RVA: 0x0003AC60 File Offset: 0x00038E60
		IControllerTemplateButton IFlightYokeTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x17002351 RID: 9041
		// (get) Token: 0x06006B5D RID: 27485 RVA: 0x0003AC6A File Offset: 0x00038E6A
		IControllerTemplateButton IFlightYokeTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x17002352 RID: 9042
		// (get) Token: 0x06006B5E RID: 27486 RVA: 0x0003AF39 File Offset: 0x00039139
		IControllerTemplateButton IFlightYokeTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(47);
			}
		}

		// Token: 0x17002353 RID: 9043
		// (get) Token: 0x06006B5F RID: 27487 RVA: 0x0003AF43 File Offset: 0x00039143
		IControllerTemplateButton IFlightYokeTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(48);
			}
		}

		// Token: 0x17002354 RID: 9044
		// (get) Token: 0x06006B60 RID: 27488 RVA: 0x0003AF4D File Offset: 0x0003914D
		IControllerTemplateButton IFlightYokeTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(49);
			}
		}

		// Token: 0x17002355 RID: 9045
		// (get) Token: 0x06006B61 RID: 27489 RVA: 0x0003AC74 File Offset: 0x00038E74
		IControllerTemplateButton IFlightYokeTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x17002356 RID: 9046
		// (get) Token: 0x06006B62 RID: 27490 RVA: 0x0003AC7E File Offset: 0x00038E7E
		IControllerTemplateButton IFlightYokeTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x17002357 RID: 9047
		// (get) Token: 0x06006B63 RID: 27491 RVA: 0x0003AC88 File Offset: 0x00038E88
		IControllerTemplateButton IFlightYokeTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x17002358 RID: 9048
		// (get) Token: 0x06006B64 RID: 27492 RVA: 0x0003ACE2 File Offset: 0x00038EE2
		IControllerTemplateButton IFlightYokeTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17002359 RID: 9049
		// (get) Token: 0x06006B65 RID: 27493 RVA: 0x0003ACEC File Offset: 0x00038EEC
		IControllerTemplateButton IFlightYokeTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x1700235A RID: 9050
		// (get) Token: 0x06006B66 RID: 27494 RVA: 0x0003ACF6 File Offset: 0x00038EF6
		IControllerTemplateButton IFlightYokeTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x1700235B RID: 9051
		// (get) Token: 0x06006B67 RID: 27495 RVA: 0x0003AF57 File Offset: 0x00039157
		IControllerTemplateYoke IFlightYokeTemplate.yoke
		{
			get
			{
				return base.GetElement<IControllerTemplateYoke>(69);
			}
		}

		// Token: 0x1700235C RID: 9052
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x0003AF61 File Offset: 0x00039161
		IControllerTemplateThrottle IFlightYokeTemplate.lever1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(70);
			}
		}

		// Token: 0x1700235D RID: 9053
		// (get) Token: 0x06006B69 RID: 27497 RVA: 0x0003AF6B File Offset: 0x0003916B
		IControllerTemplateThrottle IFlightYokeTemplate.lever2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(71);
			}
		}

		// Token: 0x1700235E RID: 9054
		// (get) Token: 0x06006B6A RID: 27498 RVA: 0x0003AF75 File Offset: 0x00039175
		IControllerTemplateThrottle IFlightYokeTemplate.lever3
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(72);
			}
		}

		// Token: 0x1700235F RID: 9055
		// (get) Token: 0x06006B6B RID: 27499 RVA: 0x0003AF7F File Offset: 0x0003917F
		IControllerTemplateThrottle IFlightYokeTemplate.lever4
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(73);
			}
		}

		// Token: 0x17002360 RID: 9056
		// (get) Token: 0x06006B6C RID: 27500 RVA: 0x0003AF89 File Offset: 0x00039189
		IControllerTemplateThrottle IFlightYokeTemplate.lever5
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(74);
			}
		}

		// Token: 0x17002361 RID: 9057
		// (get) Token: 0x06006B6D RID: 27501 RVA: 0x0003AF93 File Offset: 0x00039193
		IControllerTemplateHat IFlightYokeTemplate.leftGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(75);
			}
		}

		// Token: 0x17002362 RID: 9058
		// (get) Token: 0x06006B6E RID: 27502 RVA: 0x0003AF9D File Offset: 0x0003919D
		IControllerTemplateHat IFlightYokeTemplate.rightGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(76);
			}
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public FlightYokeTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040056EB RID: 22251
		public static readonly Guid typeGuid = new Guid("f311fa16-0ccc-41c0-ac4b-50f7100bb8ff");

		// Token: 0x040056EC RID: 22252
		public const int elementId_rotateYoke = 0;

		// Token: 0x040056ED RID: 22253
		public const int elementId_yokeZ = 1;

		// Token: 0x040056EE RID: 22254
		public const int elementId_leftPaddle = 59;

		// Token: 0x040056EF RID: 22255
		public const int elementId_rightPaddle = 60;

		// Token: 0x040056F0 RID: 22256
		public const int elementId_lever1Axis = 2;

		// Token: 0x040056F1 RID: 22257
		public const int elementId_lever1MinDetent = 64;

		// Token: 0x040056F2 RID: 22258
		public const int elementId_lever2Axis = 3;

		// Token: 0x040056F3 RID: 22259
		public const int elementId_lever2MinDetent = 65;

		// Token: 0x040056F4 RID: 22260
		public const int elementId_lever3Axis = 4;

		// Token: 0x040056F5 RID: 22261
		public const int elementId_lever3MinDetent = 66;

		// Token: 0x040056F6 RID: 22262
		public const int elementId_lever4Axis = 5;

		// Token: 0x040056F7 RID: 22263
		public const int elementId_lever4MinDetent = 67;

		// Token: 0x040056F8 RID: 22264
		public const int elementId_lever5Axis = 6;

		// Token: 0x040056F9 RID: 22265
		public const int elementId_lever5MinDetent = 68;

		// Token: 0x040056FA RID: 22266
		public const int elementId_leftGripButton1 = 7;

		// Token: 0x040056FB RID: 22267
		public const int elementId_leftGripButton2 = 8;

		// Token: 0x040056FC RID: 22268
		public const int elementId_leftGripButton3 = 9;

		// Token: 0x040056FD RID: 22269
		public const int elementId_leftGripButton4 = 10;

		// Token: 0x040056FE RID: 22270
		public const int elementId_leftGripButton5 = 11;

		// Token: 0x040056FF RID: 22271
		public const int elementId_leftGripButton6 = 12;

		// Token: 0x04005700 RID: 22272
		public const int elementId_rightGripButton1 = 13;

		// Token: 0x04005701 RID: 22273
		public const int elementId_rightGripButton2 = 14;

		// Token: 0x04005702 RID: 22274
		public const int elementId_rightGripButton3 = 15;

		// Token: 0x04005703 RID: 22275
		public const int elementId_rightGripButton4 = 16;

		// Token: 0x04005704 RID: 22276
		public const int elementId_rightGripButton5 = 17;

		// Token: 0x04005705 RID: 22277
		public const int elementId_rightGripButton6 = 18;

		// Token: 0x04005706 RID: 22278
		public const int elementId_centerButton1 = 19;

		// Token: 0x04005707 RID: 22279
		public const int elementId_centerButton2 = 20;

		// Token: 0x04005708 RID: 22280
		public const int elementId_centerButton3 = 21;

		// Token: 0x04005709 RID: 22281
		public const int elementId_centerButton4 = 22;

		// Token: 0x0400570A RID: 22282
		public const int elementId_centerButton5 = 23;

		// Token: 0x0400570B RID: 22283
		public const int elementId_centerButton6 = 24;

		// Token: 0x0400570C RID: 22284
		public const int elementId_centerButton7 = 25;

		// Token: 0x0400570D RID: 22285
		public const int elementId_centerButton8 = 26;

		// Token: 0x0400570E RID: 22286
		public const int elementId_wheel1Up = 53;

		// Token: 0x0400570F RID: 22287
		public const int elementId_wheel1Down = 54;

		// Token: 0x04005710 RID: 22288
		public const int elementId_wheel1Press = 55;

		// Token: 0x04005711 RID: 22289
		public const int elementId_wheel2Up = 56;

		// Token: 0x04005712 RID: 22290
		public const int elementId_wheel2Down = 57;

		// Token: 0x04005713 RID: 22291
		public const int elementId_wheel2Press = 58;

		// Token: 0x04005714 RID: 22292
		public const int elementId_leftGripHatUp = 27;

		// Token: 0x04005715 RID: 22293
		public const int elementId_leftGripHatUpRight = 28;

		// Token: 0x04005716 RID: 22294
		public const int elementId_leftGripHatRight = 29;

		// Token: 0x04005717 RID: 22295
		public const int elementId_leftGripHatDownRight = 30;

		// Token: 0x04005718 RID: 22296
		public const int elementId_leftGripHatDown = 31;

		// Token: 0x04005719 RID: 22297
		public const int elementId_leftGripHatDownLeft = 32;

		// Token: 0x0400571A RID: 22298
		public const int elementId_leftGripHatLeft = 33;

		// Token: 0x0400571B RID: 22299
		public const int elementId_leftGripHatUpLeft = 34;

		// Token: 0x0400571C RID: 22300
		public const int elementId_rightGripHatUp = 35;

		// Token: 0x0400571D RID: 22301
		public const int elementId_rightGripHatUpRight = 36;

		// Token: 0x0400571E RID: 22302
		public const int elementId_rightGripHatRight = 37;

		// Token: 0x0400571F RID: 22303
		public const int elementId_rightGripHatDownRight = 38;

		// Token: 0x04005720 RID: 22304
		public const int elementId_rightGripHatDown = 39;

		// Token: 0x04005721 RID: 22305
		public const int elementId_rightGripHatDownLeft = 40;

		// Token: 0x04005722 RID: 22306
		public const int elementId_rightGripHatLeft = 41;

		// Token: 0x04005723 RID: 22307
		public const int elementId_rightGripHatUpLeft = 42;

		// Token: 0x04005724 RID: 22308
		public const int elementId_consoleButton1 = 43;

		// Token: 0x04005725 RID: 22309
		public const int elementId_consoleButton2 = 44;

		// Token: 0x04005726 RID: 22310
		public const int elementId_consoleButton3 = 45;

		// Token: 0x04005727 RID: 22311
		public const int elementId_consoleButton4 = 46;

		// Token: 0x04005728 RID: 22312
		public const int elementId_consoleButton5 = 47;

		// Token: 0x04005729 RID: 22313
		public const int elementId_consoleButton6 = 48;

		// Token: 0x0400572A RID: 22314
		public const int elementId_consoleButton7 = 49;

		// Token: 0x0400572B RID: 22315
		public const int elementId_consoleButton8 = 50;

		// Token: 0x0400572C RID: 22316
		public const int elementId_consoleButton9 = 51;

		// Token: 0x0400572D RID: 22317
		public const int elementId_consoleButton10 = 52;

		// Token: 0x0400572E RID: 22318
		public const int elementId_mode1 = 61;

		// Token: 0x0400572F RID: 22319
		public const int elementId_mode2 = 62;

		// Token: 0x04005730 RID: 22320
		public const int elementId_mode3 = 63;

		// Token: 0x04005731 RID: 22321
		public const int elementId_yoke = 69;

		// Token: 0x04005732 RID: 22322
		public const int elementId_lever1 = 70;

		// Token: 0x04005733 RID: 22323
		public const int elementId_lever2 = 71;

		// Token: 0x04005734 RID: 22324
		public const int elementId_lever3 = 72;

		// Token: 0x04005735 RID: 22325
		public const int elementId_lever4 = 73;

		// Token: 0x04005736 RID: 22326
		public const int elementId_lever5 = 74;

		// Token: 0x04005737 RID: 22327
		public const int elementId_leftGripHat = 75;

		// Token: 0x04005738 RID: 22328
		public const int elementId_rightGripHat = 76;
	}
}
