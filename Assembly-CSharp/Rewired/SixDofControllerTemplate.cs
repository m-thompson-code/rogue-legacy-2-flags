using System;

namespace Rewired
{
	// Token: 0x02000EAE RID: 3758
	public sealed class SixDofControllerTemplate : ControllerTemplate, ISixDofControllerTemplate, IControllerTemplate
	{
		// Token: 0x17002366 RID: 9062
		// (get) Token: 0x06006B76 RID: 27510 RVA: 0x0003AFC9 File Offset: 0x000391C9
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(8);
			}
		}

		// Token: 0x17002367 RID: 9063
		// (get) Token: 0x06006B77 RID: 27511 RVA: 0x0003AFD2 File Offset: 0x000391D2
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(9);
			}
		}

		// Token: 0x17002368 RID: 9064
		// (get) Token: 0x06006B78 RID: 27512 RVA: 0x0003AFDC File Offset: 0x000391DC
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(10);
			}
		}

		// Token: 0x17002369 RID: 9065
		// (get) Token: 0x06006B79 RID: 27513 RVA: 0x0003AA79 File Offset: 0x00038C79
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x1700236A RID: 9066
		// (get) Token: 0x06006B7A RID: 27514 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton ISixDofControllerTemplate.button1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x1700236B RID: 9067
		// (get) Token: 0x06006B7B RID: 27515 RVA: 0x0003AB1B File Offset: 0x00038D1B
		IControllerTemplateButton ISixDofControllerTemplate.button2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x1700236C RID: 9068
		// (get) Token: 0x06006B7C RID: 27516 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton ISixDofControllerTemplate.button3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x1700236D RID: 9069
		// (get) Token: 0x06006B7D RID: 27517 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton ISixDofControllerTemplate.button4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x1700236E RID: 9070
		// (get) Token: 0x06006B7E RID: 27518 RVA: 0x0003AAAB File Offset: 0x00038CAB
		IControllerTemplateButton ISixDofControllerTemplate.button5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x1700236F RID: 9071
		// (get) Token: 0x06006B7F RID: 27519 RVA: 0x0003AB25 File Offset: 0x00038D25
		IControllerTemplateButton ISixDofControllerTemplate.button6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17002370 RID: 9072
		// (get) Token: 0x06006B80 RID: 27520 RVA: 0x0003AB2F File Offset: 0x00038D2F
		IControllerTemplateButton ISixDofControllerTemplate.button7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17002371 RID: 9073
		// (get) Token: 0x06006B81 RID: 27521 RVA: 0x0003AB39 File Offset: 0x00038D39
		IControllerTemplateButton ISixDofControllerTemplate.button8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17002372 RID: 9074
		// (get) Token: 0x06006B82 RID: 27522 RVA: 0x0003AB43 File Offset: 0x00038D43
		IControllerTemplateButton ISixDofControllerTemplate.button9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17002373 RID: 9075
		// (get) Token: 0x06006B83 RID: 27523 RVA: 0x0003AB4D File Offset: 0x00038D4D
		IControllerTemplateButton ISixDofControllerTemplate.button10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17002374 RID: 9076
		// (get) Token: 0x06006B84 RID: 27524 RVA: 0x0003AB57 File Offset: 0x00038D57
		IControllerTemplateButton ISixDofControllerTemplate.button11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17002375 RID: 9077
		// (get) Token: 0x06006B85 RID: 27525 RVA: 0x0003AB61 File Offset: 0x00038D61
		IControllerTemplateButton ISixDofControllerTemplate.button12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17002376 RID: 9078
		// (get) Token: 0x06006B86 RID: 27526 RVA: 0x0003AB6B File Offset: 0x00038D6B
		IControllerTemplateButton ISixDofControllerTemplate.button13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17002377 RID: 9079
		// (get) Token: 0x06006B87 RID: 27527 RVA: 0x0003AB75 File Offset: 0x00038D75
		IControllerTemplateButton ISixDofControllerTemplate.button14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17002378 RID: 9080
		// (get) Token: 0x06006B88 RID: 27528 RVA: 0x0003AB7F File Offset: 0x00038D7F
		IControllerTemplateButton ISixDofControllerTemplate.button15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17002379 RID: 9081
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x0003AB89 File Offset: 0x00038D89
		IControllerTemplateButton ISixDofControllerTemplate.button16
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x1700237A RID: 9082
		// (get) Token: 0x06006B8A RID: 27530 RVA: 0x0003AB93 File Offset: 0x00038D93
		IControllerTemplateButton ISixDofControllerTemplate.button17
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x1700237B RID: 9083
		// (get) Token: 0x06006B8B RID: 27531 RVA: 0x0003AB9D File Offset: 0x00038D9D
		IControllerTemplateButton ISixDofControllerTemplate.button18
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x1700237C RID: 9084
		// (get) Token: 0x06006B8C RID: 27532 RVA: 0x0003ABA7 File Offset: 0x00038DA7
		IControllerTemplateButton ISixDofControllerTemplate.button19
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x1700237D RID: 9085
		// (get) Token: 0x06006B8D RID: 27533 RVA: 0x0003ABB1 File Offset: 0x00038DB1
		IControllerTemplateButton ISixDofControllerTemplate.button20
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x1700237E RID: 9086
		// (get) Token: 0x06006B8E RID: 27534 RVA: 0x0003ACA6 File Offset: 0x00038EA6
		IControllerTemplateButton ISixDofControllerTemplate.button21
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x1700237F RID: 9087
		// (get) Token: 0x06006B8F RID: 27535 RVA: 0x0003ACB0 File Offset: 0x00038EB0
		IControllerTemplateButton ISixDofControllerTemplate.button22
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x17002380 RID: 9088
		// (get) Token: 0x06006B90 RID: 27536 RVA: 0x0003ACBA File Offset: 0x00038EBA
		IControllerTemplateButton ISixDofControllerTemplate.button23
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x17002381 RID: 9089
		// (get) Token: 0x06006B91 RID: 27537 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		IControllerTemplateButton ISixDofControllerTemplate.button24
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17002382 RID: 9090
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x0003ACCE File Offset: 0x00038ECE
		IControllerTemplateButton ISixDofControllerTemplate.button25
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17002383 RID: 9091
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x0003ACD8 File Offset: 0x00038ED8
		IControllerTemplateButton ISixDofControllerTemplate.button26
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17002384 RID: 9092
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x0003ACE2 File Offset: 0x00038EE2
		IControllerTemplateButton ISixDofControllerTemplate.button27
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17002385 RID: 9093
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x0003ACEC File Offset: 0x00038EEC
		IControllerTemplateButton ISixDofControllerTemplate.button28
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17002386 RID: 9094
		// (get) Token: 0x06006B96 RID: 27542 RVA: 0x0003ACF6 File Offset: 0x00038EF6
		IControllerTemplateButton ISixDofControllerTemplate.button29
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17002387 RID: 9095
		// (get) Token: 0x06006B97 RID: 27543 RVA: 0x0003AD00 File Offset: 0x00038F00
		IControllerTemplateButton ISixDofControllerTemplate.button30
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x17002388 RID: 9096
		// (get) Token: 0x06006B98 RID: 27544 RVA: 0x0003AD0A File Offset: 0x00038F0A
		IControllerTemplateButton ISixDofControllerTemplate.button31
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x17002389 RID: 9097
		// (get) Token: 0x06006B99 RID: 27545 RVA: 0x0003AD14 File Offset: 0x00038F14
		IControllerTemplateButton ISixDofControllerTemplate.button32
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x1700238A RID: 9098
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x0003AFE6 File Offset: 0x000391E6
		IControllerTemplateHat ISixDofControllerTemplate.hat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(48);
			}
		}

		// Token: 0x1700238B RID: 9099
		// (get) Token: 0x06006B9B RID: 27547 RVA: 0x0003AFF0 File Offset: 0x000391F0
		IControllerTemplateHat ISixDofControllerTemplate.hat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(49);
			}
		}

		// Token: 0x1700238C RID: 9100
		// (get) Token: 0x06006B9C RID: 27548 RVA: 0x0003AFFA File Offset: 0x000391FA
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(52);
			}
		}

		// Token: 0x1700238D RID: 9101
		// (get) Token: 0x06006B9D RID: 27549 RVA: 0x0003B004 File Offset: 0x00039204
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(53);
			}
		}

		// Token: 0x1700238E RID: 9102
		// (get) Token: 0x06006B9E RID: 27550 RVA: 0x0003B00E File Offset: 0x0003920E
		IControllerTemplateStick6D ISixDofControllerTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick6D>(54);
			}
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public SixDofControllerTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x0400573D RID: 22333
		public static readonly Guid typeGuid = new Guid("2599beb3-522b-43dd-a4ef-93fd60e5eafa");

		// Token: 0x0400573E RID: 22334
		public const int elementId_positionX = 1;

		// Token: 0x0400573F RID: 22335
		public const int elementId_positionY = 2;

		// Token: 0x04005740 RID: 22336
		public const int elementId_positionZ = 0;

		// Token: 0x04005741 RID: 22337
		public const int elementId_rotationX = 3;

		// Token: 0x04005742 RID: 22338
		public const int elementId_rotationY = 5;

		// Token: 0x04005743 RID: 22339
		public const int elementId_rotationZ = 4;

		// Token: 0x04005744 RID: 22340
		public const int elementId_throttle1Axis = 6;

		// Token: 0x04005745 RID: 22341
		public const int elementId_throttle1MinDetent = 50;

		// Token: 0x04005746 RID: 22342
		public const int elementId_throttle2Axis = 7;

		// Token: 0x04005747 RID: 22343
		public const int elementId_throttle2MinDetent = 51;

		// Token: 0x04005748 RID: 22344
		public const int elementId_extraAxis1 = 8;

		// Token: 0x04005749 RID: 22345
		public const int elementId_extraAxis2 = 9;

		// Token: 0x0400574A RID: 22346
		public const int elementId_extraAxis3 = 10;

		// Token: 0x0400574B RID: 22347
		public const int elementId_extraAxis4 = 11;

		// Token: 0x0400574C RID: 22348
		public const int elementId_button1 = 12;

		// Token: 0x0400574D RID: 22349
		public const int elementId_button2 = 13;

		// Token: 0x0400574E RID: 22350
		public const int elementId_button3 = 14;

		// Token: 0x0400574F RID: 22351
		public const int elementId_button4 = 15;

		// Token: 0x04005750 RID: 22352
		public const int elementId_button5 = 16;

		// Token: 0x04005751 RID: 22353
		public const int elementId_button6 = 17;

		// Token: 0x04005752 RID: 22354
		public const int elementId_button7 = 18;

		// Token: 0x04005753 RID: 22355
		public const int elementId_button8 = 19;

		// Token: 0x04005754 RID: 22356
		public const int elementId_button9 = 20;

		// Token: 0x04005755 RID: 22357
		public const int elementId_button10 = 21;

		// Token: 0x04005756 RID: 22358
		public const int elementId_button11 = 22;

		// Token: 0x04005757 RID: 22359
		public const int elementId_button12 = 23;

		// Token: 0x04005758 RID: 22360
		public const int elementId_button13 = 24;

		// Token: 0x04005759 RID: 22361
		public const int elementId_button14 = 25;

		// Token: 0x0400575A RID: 22362
		public const int elementId_button15 = 26;

		// Token: 0x0400575B RID: 22363
		public const int elementId_button16 = 27;

		// Token: 0x0400575C RID: 22364
		public const int elementId_button17 = 28;

		// Token: 0x0400575D RID: 22365
		public const int elementId_button18 = 29;

		// Token: 0x0400575E RID: 22366
		public const int elementId_button19 = 30;

		// Token: 0x0400575F RID: 22367
		public const int elementId_button20 = 31;

		// Token: 0x04005760 RID: 22368
		public const int elementId_button21 = 55;

		// Token: 0x04005761 RID: 22369
		public const int elementId_button22 = 56;

		// Token: 0x04005762 RID: 22370
		public const int elementId_button23 = 57;

		// Token: 0x04005763 RID: 22371
		public const int elementId_button24 = 58;

		// Token: 0x04005764 RID: 22372
		public const int elementId_button25 = 59;

		// Token: 0x04005765 RID: 22373
		public const int elementId_button26 = 60;

		// Token: 0x04005766 RID: 22374
		public const int elementId_button27 = 61;

		// Token: 0x04005767 RID: 22375
		public const int elementId_button28 = 62;

		// Token: 0x04005768 RID: 22376
		public const int elementId_button29 = 63;

		// Token: 0x04005769 RID: 22377
		public const int elementId_button30 = 64;

		// Token: 0x0400576A RID: 22378
		public const int elementId_button31 = 65;

		// Token: 0x0400576B RID: 22379
		public const int elementId_button32 = 66;

		// Token: 0x0400576C RID: 22380
		public const int elementId_hat1Up = 32;

		// Token: 0x0400576D RID: 22381
		public const int elementId_hat1UpRight = 33;

		// Token: 0x0400576E RID: 22382
		public const int elementId_hat1Right = 34;

		// Token: 0x0400576F RID: 22383
		public const int elementId_hat1DownRight = 35;

		// Token: 0x04005770 RID: 22384
		public const int elementId_hat1Down = 36;

		// Token: 0x04005771 RID: 22385
		public const int elementId_hat1DownLeft = 37;

		// Token: 0x04005772 RID: 22386
		public const int elementId_hat1Left = 38;

		// Token: 0x04005773 RID: 22387
		public const int elementId_hat1UpLeft = 39;

		// Token: 0x04005774 RID: 22388
		public const int elementId_hat2Up = 40;

		// Token: 0x04005775 RID: 22389
		public const int elementId_hat2UpRight = 41;

		// Token: 0x04005776 RID: 22390
		public const int elementId_hat2Right = 42;

		// Token: 0x04005777 RID: 22391
		public const int elementId_hat2DownRight = 43;

		// Token: 0x04005778 RID: 22392
		public const int elementId_hat2Down = 44;

		// Token: 0x04005779 RID: 22393
		public const int elementId_hat2DownLeft = 45;

		// Token: 0x0400577A RID: 22394
		public const int elementId_hat2Left = 46;

		// Token: 0x0400577B RID: 22395
		public const int elementId_hat2UpLeft = 47;

		// Token: 0x0400577C RID: 22396
		public const int elementId_hat1 = 48;

		// Token: 0x0400577D RID: 22397
		public const int elementId_hat2 = 49;

		// Token: 0x0400577E RID: 22398
		public const int elementId_throttle1 = 52;

		// Token: 0x0400577F RID: 22399
		public const int elementId_throttle2 = 53;

		// Token: 0x04005780 RID: 22400
		public const int elementId_stick = 54;
	}
}
