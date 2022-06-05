using System;

namespace Rewired
{
	// Token: 0x0200092B RID: 2347
	public sealed class RacingWheelTemplate : ControllerTemplate, IRacingWheelTemplate, IControllerTemplate
	{
		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x00112FA6 File Offset: 0x001111A6
		IControllerTemplateAxis IRacingWheelTemplate.wheel
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x00112FAF File Offset: 0x001111AF
		IControllerTemplateAxis IRacingWheelTemplate.accelerator
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x00112FB8 File Offset: 0x001111B8
		IControllerTemplateAxis IRacingWheelTemplate.brake
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00112FC1 File Offset: 0x001111C1
		IControllerTemplateAxis IRacingWheelTemplate.clutch
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(3);
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x00112FCA File Offset: 0x001111CA
		IControllerTemplateButton IRacingWheelTemplate.shiftDown
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00112FD3 File Offset: 0x001111D3
		IControllerTemplateButton IRacingWheelTemplate.shiftUp
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x00112FDC File Offset: 0x001111DC
		IControllerTemplateButton IRacingWheelTemplate.wheelButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00112FE5 File Offset: 0x001111E5
		IControllerTemplateButton IRacingWheelTemplate.wheelButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x06004DB9 RID: 19897 RVA: 0x00112FEE File Offset: 0x001111EE
		IControllerTemplateButton IRacingWheelTemplate.wheelButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x00112FF7 File Offset: 0x001111F7
		IControllerTemplateButton IRacingWheelTemplate.wheelButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x00113001 File Offset: 0x00111201
		IControllerTemplateButton IRacingWheelTemplate.wheelButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x0011300B File Offset: 0x0011120B
		IControllerTemplateButton IRacingWheelTemplate.wheelButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x00113015 File Offset: 0x00111215
		IControllerTemplateButton IRacingWheelTemplate.wheelButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x06004DBE RID: 19902 RVA: 0x0011301F File Offset: 0x0011121F
		IControllerTemplateButton IRacingWheelTemplate.wheelButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x00113029 File Offset: 0x00111229
		IControllerTemplateButton IRacingWheelTemplate.wheelButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x00113033 File Offset: 0x00111233
		IControllerTemplateButton IRacingWheelTemplate.wheelButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x0011303D File Offset: 0x0011123D
		IControllerTemplateButton IRacingWheelTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x06004DC2 RID: 19906 RVA: 0x00113047 File Offset: 0x00111247
		IControllerTemplateButton IRacingWheelTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x06004DC3 RID: 19907 RVA: 0x00113051 File Offset: 0x00111251
		IControllerTemplateButton IRacingWheelTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x06004DC4 RID: 19908 RVA: 0x0011305B File Offset: 0x0011125B
		IControllerTemplateButton IRacingWheelTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x00113065 File Offset: 0x00111265
		IControllerTemplateButton IRacingWheelTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x0011306F File Offset: 0x0011126F
		IControllerTemplateButton IRacingWheelTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x00113079 File Offset: 0x00111279
		IControllerTemplateButton IRacingWheelTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x00113083 File Offset: 0x00111283
		IControllerTemplateButton IRacingWheelTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x0011308D File Offset: 0x0011128D
		IControllerTemplateButton IRacingWheelTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x06004DCA RID: 19914 RVA: 0x00113097 File Offset: 0x00111297
		IControllerTemplateButton IRacingWheelTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x06004DCB RID: 19915 RVA: 0x001130A1 File Offset: 0x001112A1
		IControllerTemplateButton IRacingWheelTemplate.shifter1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x06004DCC RID: 19916 RVA: 0x001130AB File Offset: 0x001112AB
		IControllerTemplateButton IRacingWheelTemplate.shifter2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x06004DCD RID: 19917 RVA: 0x001130B5 File Offset: 0x001112B5
		IControllerTemplateButton IRacingWheelTemplate.shifter3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x06004DCE RID: 19918 RVA: 0x001130BF File Offset: 0x001112BF
		IControllerTemplateButton IRacingWheelTemplate.shifter4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x06004DCF RID: 19919 RVA: 0x001130C9 File Offset: 0x001112C9
		IControllerTemplateButton IRacingWheelTemplate.shifter5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x06004DD0 RID: 19920 RVA: 0x001130D3 File Offset: 0x001112D3
		IControllerTemplateButton IRacingWheelTemplate.shifter6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x170019D3 RID: 6611
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001130DD File Offset: 0x001112DD
		IControllerTemplateButton IRacingWheelTemplate.shifter7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(32);
			}
		}

		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x001130E7 File Offset: 0x001112E7
		IControllerTemplateButton IRacingWheelTemplate.shifter8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(33);
			}
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x06004DD3 RID: 19923 RVA: 0x001130F1 File Offset: 0x001112F1
		IControllerTemplateButton IRacingWheelTemplate.shifter9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(34);
			}
		}

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x001130FB File Offset: 0x001112FB
		IControllerTemplateButton IRacingWheelTemplate.shifter10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(35);
			}
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x06004DD5 RID: 19925 RVA: 0x00113105 File Offset: 0x00111305
		IControllerTemplateButton IRacingWheelTemplate.reverseGear
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0011310F File Offset: 0x0011130F
		IControllerTemplateButton IRacingWheelTemplate.select
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(36);
			}
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x00113119 File Offset: 0x00111319
		IControllerTemplateButton IRacingWheelTemplate.start
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(37);
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x00113123 File Offset: 0x00111323
		IControllerTemplateButton IRacingWheelTemplate.systemButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(38);
			}
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x06004DD9 RID: 19929 RVA: 0x0011312D File Offset: 0x0011132D
		IControllerTemplateButton IRacingWheelTemplate.horn
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x06004DDA RID: 19930 RVA: 0x00113137 File Offset: 0x00111337
		IControllerTemplateDPad IRacingWheelTemplate.dPad
		{
			get
			{
				return base.GetElement<IControllerTemplateDPad>(45);
			}
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x00113141 File Offset: 0x00111341
		public RacingWheelTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040040A7 RID: 16551
		public static readonly Guid typeGuid = new Guid("104e31d8-9115-4dd5-a398-2e54d35e6c83");

		// Token: 0x040040A8 RID: 16552
		public const int elementId_wheel = 0;

		// Token: 0x040040A9 RID: 16553
		public const int elementId_accelerator = 1;

		// Token: 0x040040AA RID: 16554
		public const int elementId_brake = 2;

		// Token: 0x040040AB RID: 16555
		public const int elementId_clutch = 3;

		// Token: 0x040040AC RID: 16556
		public const int elementId_shiftDown = 4;

		// Token: 0x040040AD RID: 16557
		public const int elementId_shiftUp = 5;

		// Token: 0x040040AE RID: 16558
		public const int elementId_wheelButton1 = 6;

		// Token: 0x040040AF RID: 16559
		public const int elementId_wheelButton2 = 7;

		// Token: 0x040040B0 RID: 16560
		public const int elementId_wheelButton3 = 8;

		// Token: 0x040040B1 RID: 16561
		public const int elementId_wheelButton4 = 9;

		// Token: 0x040040B2 RID: 16562
		public const int elementId_wheelButton5 = 10;

		// Token: 0x040040B3 RID: 16563
		public const int elementId_wheelButton6 = 11;

		// Token: 0x040040B4 RID: 16564
		public const int elementId_wheelButton7 = 12;

		// Token: 0x040040B5 RID: 16565
		public const int elementId_wheelButton8 = 13;

		// Token: 0x040040B6 RID: 16566
		public const int elementId_wheelButton9 = 14;

		// Token: 0x040040B7 RID: 16567
		public const int elementId_wheelButton10 = 15;

		// Token: 0x040040B8 RID: 16568
		public const int elementId_consoleButton1 = 16;

		// Token: 0x040040B9 RID: 16569
		public const int elementId_consoleButton2 = 17;

		// Token: 0x040040BA RID: 16570
		public const int elementId_consoleButton3 = 18;

		// Token: 0x040040BB RID: 16571
		public const int elementId_consoleButton4 = 19;

		// Token: 0x040040BC RID: 16572
		public const int elementId_consoleButton5 = 20;

		// Token: 0x040040BD RID: 16573
		public const int elementId_consoleButton6 = 21;

		// Token: 0x040040BE RID: 16574
		public const int elementId_consoleButton7 = 22;

		// Token: 0x040040BF RID: 16575
		public const int elementId_consoleButton8 = 23;

		// Token: 0x040040C0 RID: 16576
		public const int elementId_consoleButton9 = 24;

		// Token: 0x040040C1 RID: 16577
		public const int elementId_consoleButton10 = 25;

		// Token: 0x040040C2 RID: 16578
		public const int elementId_shifter1 = 26;

		// Token: 0x040040C3 RID: 16579
		public const int elementId_shifter2 = 27;

		// Token: 0x040040C4 RID: 16580
		public const int elementId_shifter3 = 28;

		// Token: 0x040040C5 RID: 16581
		public const int elementId_shifter4 = 29;

		// Token: 0x040040C6 RID: 16582
		public const int elementId_shifter5 = 30;

		// Token: 0x040040C7 RID: 16583
		public const int elementId_shifter6 = 31;

		// Token: 0x040040C8 RID: 16584
		public const int elementId_shifter7 = 32;

		// Token: 0x040040C9 RID: 16585
		public const int elementId_shifter8 = 33;

		// Token: 0x040040CA RID: 16586
		public const int elementId_shifter9 = 34;

		// Token: 0x040040CB RID: 16587
		public const int elementId_shifter10 = 35;

		// Token: 0x040040CC RID: 16588
		public const int elementId_reverseGear = 44;

		// Token: 0x040040CD RID: 16589
		public const int elementId_select = 36;

		// Token: 0x040040CE RID: 16590
		public const int elementId_start = 37;

		// Token: 0x040040CF RID: 16591
		public const int elementId_systemButton = 38;

		// Token: 0x040040D0 RID: 16592
		public const int elementId_horn = 43;

		// Token: 0x040040D1 RID: 16593
		public const int elementId_dPadUp = 39;

		// Token: 0x040040D2 RID: 16594
		public const int elementId_dPadRight = 40;

		// Token: 0x040040D3 RID: 16595
		public const int elementId_dPadDown = 41;

		// Token: 0x040040D4 RID: 16596
		public const int elementId_dPadLeft = 42;

		// Token: 0x040040D5 RID: 16597
		public const int elementId_dPad = 45;
	}
}
