using System;

namespace Rewired
{
	// Token: 0x0200092D RID: 2349
	public sealed class FlightYokeTemplate : ControllerTemplate, IFlightYokeTemplate, IControllerTemplate
	{
		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x06004E37 RID: 20023 RVA: 0x0011354E File Offset: 0x0011174E
		IControllerTemplateButton IFlightYokeTemplate.leftPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x06004E38 RID: 20024 RVA: 0x00113558 File Offset: 0x00111758
		IControllerTemplateButton IFlightYokeTemplate.rightPaddle
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x06004E39 RID: 20025 RVA: 0x00113562 File Offset: 0x00111762
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x0011356B File Offset: 0x0011176B
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x06004E3B RID: 20027 RVA: 0x00113574 File Offset: 0x00111774
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x06004E3C RID: 20028 RVA: 0x0011357E File Offset: 0x0011177E
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x06004E3D RID: 20029 RVA: 0x00113588 File Offset: 0x00111788
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x00113592 File Offset: 0x00111792
		IControllerTemplateButton IFlightYokeTemplate.leftGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17001A3D RID: 6717
		// (get) Token: 0x06004E3F RID: 20031 RVA: 0x0011359C File Offset: 0x0011179C
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x17001A3E RID: 6718
		// (get) Token: 0x06004E40 RID: 20032 RVA: 0x001135A6 File Offset: 0x001117A6
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17001A3F RID: 6719
		// (get) Token: 0x06004E41 RID: 20033 RVA: 0x001135B0 File Offset: 0x001117B0
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17001A40 RID: 6720
		// (get) Token: 0x06004E42 RID: 20034 RVA: 0x001135BA File Offset: 0x001117BA
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x17001A41 RID: 6721
		// (get) Token: 0x06004E43 RID: 20035 RVA: 0x001135C4 File Offset: 0x001117C4
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17001A42 RID: 6722
		// (get) Token: 0x06004E44 RID: 20036 RVA: 0x001135CE File Offset: 0x001117CE
		IControllerTemplateButton IFlightYokeTemplate.rightGripButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x06004E45 RID: 20037 RVA: 0x001135D8 File Offset: 0x001117D8
		IControllerTemplateButton IFlightYokeTemplate.centerButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x06004E46 RID: 20038 RVA: 0x001135E2 File Offset: 0x001117E2
		IControllerTemplateButton IFlightYokeTemplate.centerButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06004E47 RID: 20039 RVA: 0x001135EC File Offset: 0x001117EC
		IControllerTemplateButton IFlightYokeTemplate.centerButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06004E48 RID: 20040 RVA: 0x001135F6 File Offset: 0x001117F6
		IControllerTemplateButton IFlightYokeTemplate.centerButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x00113600 File Offset: 0x00111800
		IControllerTemplateButton IFlightYokeTemplate.centerButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x0011360A File Offset: 0x0011180A
		IControllerTemplateButton IFlightYokeTemplate.centerButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x06004E4B RID: 20043 RVA: 0x00113614 File Offset: 0x00111814
		IControllerTemplateButton IFlightYokeTemplate.centerButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x0011361E File Offset: 0x0011181E
		IControllerTemplateButton IFlightYokeTemplate.centerButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x00113628 File Offset: 0x00111828
		IControllerTemplateButton IFlightYokeTemplate.wheel1Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x06004E4E RID: 20046 RVA: 0x00113632 File Offset: 0x00111832
		IControllerTemplateButton IFlightYokeTemplate.wheel1Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x0011363C File Offset: 0x0011183C
		IControllerTemplateButton IFlightYokeTemplate.wheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x06004E50 RID: 20048 RVA: 0x00113646 File Offset: 0x00111846
		IControllerTemplateButton IFlightYokeTemplate.wheel2Up
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x06004E51 RID: 20049 RVA: 0x00113650 File Offset: 0x00111850
		IControllerTemplateButton IFlightYokeTemplate.wheel2Down
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x06004E52 RID: 20050 RVA: 0x0011365A File Offset: 0x0011185A
		IControllerTemplateButton IFlightYokeTemplate.wheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x06004E53 RID: 20051 RVA: 0x00113664 File Offset: 0x00111864
		IControllerTemplateButton IFlightYokeTemplate.consoleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(43);
			}
		}

		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x06004E54 RID: 20052 RVA: 0x0011366E File Offset: 0x0011186E
		IControllerTemplateButton IFlightYokeTemplate.consoleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x06004E55 RID: 20053 RVA: 0x00113678 File Offset: 0x00111878
		IControllerTemplateButton IFlightYokeTemplate.consoleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x00113682 File Offset: 0x00111882
		IControllerTemplateButton IFlightYokeTemplate.consoleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0011368C File Offset: 0x0011188C
		IControllerTemplateButton IFlightYokeTemplate.consoleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(47);
			}
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x00113696 File Offset: 0x00111896
		IControllerTemplateButton IFlightYokeTemplate.consoleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(48);
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x06004E59 RID: 20057 RVA: 0x001136A0 File Offset: 0x001118A0
		IControllerTemplateButton IFlightYokeTemplate.consoleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(49);
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x06004E5A RID: 20058 RVA: 0x001136AA File Offset: 0x001118AA
		IControllerTemplateButton IFlightYokeTemplate.consoleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x06004E5B RID: 20059 RVA: 0x001136B4 File Offset: 0x001118B4
		IControllerTemplateButton IFlightYokeTemplate.consoleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x001136BE File Offset: 0x001118BE
		IControllerTemplateButton IFlightYokeTemplate.consoleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x06004E5D RID: 20061 RVA: 0x001136C8 File Offset: 0x001118C8
		IControllerTemplateButton IFlightYokeTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x06004E5E RID: 20062 RVA: 0x001136D2 File Offset: 0x001118D2
		IControllerTemplateButton IFlightYokeTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x06004E5F RID: 20063 RVA: 0x001136DC File Offset: 0x001118DC
		IControllerTemplateButton IFlightYokeTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x001136E6 File Offset: 0x001118E6
		IControllerTemplateYoke IFlightYokeTemplate.yoke
		{
			get
			{
				return base.GetElement<IControllerTemplateYoke>(69);
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x001136F0 File Offset: 0x001118F0
		IControllerTemplateThrottle IFlightYokeTemplate.lever1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(70);
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x001136FA File Offset: 0x001118FA
		IControllerTemplateThrottle IFlightYokeTemplate.lever2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(71);
			}
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x00113704 File Offset: 0x00111904
		IControllerTemplateThrottle IFlightYokeTemplate.lever3
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(72);
			}
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x0011370E File Offset: 0x0011190E
		IControllerTemplateThrottle IFlightYokeTemplate.lever4
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(73);
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x06004E65 RID: 20069 RVA: 0x00113718 File Offset: 0x00111918
		IControllerTemplateThrottle IFlightYokeTemplate.lever5
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(74);
			}
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x00113722 File Offset: 0x00111922
		IControllerTemplateHat IFlightYokeTemplate.leftGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(75);
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x0011372C File Offset: 0x0011192C
		IControllerTemplateHat IFlightYokeTemplate.rightGripHat
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(76);
			}
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x00113736 File Offset: 0x00111936
		public FlightYokeTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x0400417F RID: 16767
		public static readonly Guid typeGuid = new Guid("f311fa16-0ccc-41c0-ac4b-50f7100bb8ff");

		// Token: 0x04004180 RID: 16768
		public const int elementId_rotateYoke = 0;

		// Token: 0x04004181 RID: 16769
		public const int elementId_yokeZ = 1;

		// Token: 0x04004182 RID: 16770
		public const int elementId_leftPaddle = 59;

		// Token: 0x04004183 RID: 16771
		public const int elementId_rightPaddle = 60;

		// Token: 0x04004184 RID: 16772
		public const int elementId_lever1Axis = 2;

		// Token: 0x04004185 RID: 16773
		public const int elementId_lever1MinDetent = 64;

		// Token: 0x04004186 RID: 16774
		public const int elementId_lever2Axis = 3;

		// Token: 0x04004187 RID: 16775
		public const int elementId_lever2MinDetent = 65;

		// Token: 0x04004188 RID: 16776
		public const int elementId_lever3Axis = 4;

		// Token: 0x04004189 RID: 16777
		public const int elementId_lever3MinDetent = 66;

		// Token: 0x0400418A RID: 16778
		public const int elementId_lever4Axis = 5;

		// Token: 0x0400418B RID: 16779
		public const int elementId_lever4MinDetent = 67;

		// Token: 0x0400418C RID: 16780
		public const int elementId_lever5Axis = 6;

		// Token: 0x0400418D RID: 16781
		public const int elementId_lever5MinDetent = 68;

		// Token: 0x0400418E RID: 16782
		public const int elementId_leftGripButton1 = 7;

		// Token: 0x0400418F RID: 16783
		public const int elementId_leftGripButton2 = 8;

		// Token: 0x04004190 RID: 16784
		public const int elementId_leftGripButton3 = 9;

		// Token: 0x04004191 RID: 16785
		public const int elementId_leftGripButton4 = 10;

		// Token: 0x04004192 RID: 16786
		public const int elementId_leftGripButton5 = 11;

		// Token: 0x04004193 RID: 16787
		public const int elementId_leftGripButton6 = 12;

		// Token: 0x04004194 RID: 16788
		public const int elementId_rightGripButton1 = 13;

		// Token: 0x04004195 RID: 16789
		public const int elementId_rightGripButton2 = 14;

		// Token: 0x04004196 RID: 16790
		public const int elementId_rightGripButton3 = 15;

		// Token: 0x04004197 RID: 16791
		public const int elementId_rightGripButton4 = 16;

		// Token: 0x04004198 RID: 16792
		public const int elementId_rightGripButton5 = 17;

		// Token: 0x04004199 RID: 16793
		public const int elementId_rightGripButton6 = 18;

		// Token: 0x0400419A RID: 16794
		public const int elementId_centerButton1 = 19;

		// Token: 0x0400419B RID: 16795
		public const int elementId_centerButton2 = 20;

		// Token: 0x0400419C RID: 16796
		public const int elementId_centerButton3 = 21;

		// Token: 0x0400419D RID: 16797
		public const int elementId_centerButton4 = 22;

		// Token: 0x0400419E RID: 16798
		public const int elementId_centerButton5 = 23;

		// Token: 0x0400419F RID: 16799
		public const int elementId_centerButton6 = 24;

		// Token: 0x040041A0 RID: 16800
		public const int elementId_centerButton7 = 25;

		// Token: 0x040041A1 RID: 16801
		public const int elementId_centerButton8 = 26;

		// Token: 0x040041A2 RID: 16802
		public const int elementId_wheel1Up = 53;

		// Token: 0x040041A3 RID: 16803
		public const int elementId_wheel1Down = 54;

		// Token: 0x040041A4 RID: 16804
		public const int elementId_wheel1Press = 55;

		// Token: 0x040041A5 RID: 16805
		public const int elementId_wheel2Up = 56;

		// Token: 0x040041A6 RID: 16806
		public const int elementId_wheel2Down = 57;

		// Token: 0x040041A7 RID: 16807
		public const int elementId_wheel2Press = 58;

		// Token: 0x040041A8 RID: 16808
		public const int elementId_leftGripHatUp = 27;

		// Token: 0x040041A9 RID: 16809
		public const int elementId_leftGripHatUpRight = 28;

		// Token: 0x040041AA RID: 16810
		public const int elementId_leftGripHatRight = 29;

		// Token: 0x040041AB RID: 16811
		public const int elementId_leftGripHatDownRight = 30;

		// Token: 0x040041AC RID: 16812
		public const int elementId_leftGripHatDown = 31;

		// Token: 0x040041AD RID: 16813
		public const int elementId_leftGripHatDownLeft = 32;

		// Token: 0x040041AE RID: 16814
		public const int elementId_leftGripHatLeft = 33;

		// Token: 0x040041AF RID: 16815
		public const int elementId_leftGripHatUpLeft = 34;

		// Token: 0x040041B0 RID: 16816
		public const int elementId_rightGripHatUp = 35;

		// Token: 0x040041B1 RID: 16817
		public const int elementId_rightGripHatUpRight = 36;

		// Token: 0x040041B2 RID: 16818
		public const int elementId_rightGripHatRight = 37;

		// Token: 0x040041B3 RID: 16819
		public const int elementId_rightGripHatDownRight = 38;

		// Token: 0x040041B4 RID: 16820
		public const int elementId_rightGripHatDown = 39;

		// Token: 0x040041B5 RID: 16821
		public const int elementId_rightGripHatDownLeft = 40;

		// Token: 0x040041B6 RID: 16822
		public const int elementId_rightGripHatLeft = 41;

		// Token: 0x040041B7 RID: 16823
		public const int elementId_rightGripHatUpLeft = 42;

		// Token: 0x040041B8 RID: 16824
		public const int elementId_consoleButton1 = 43;

		// Token: 0x040041B9 RID: 16825
		public const int elementId_consoleButton2 = 44;

		// Token: 0x040041BA RID: 16826
		public const int elementId_consoleButton3 = 45;

		// Token: 0x040041BB RID: 16827
		public const int elementId_consoleButton4 = 46;

		// Token: 0x040041BC RID: 16828
		public const int elementId_consoleButton5 = 47;

		// Token: 0x040041BD RID: 16829
		public const int elementId_consoleButton6 = 48;

		// Token: 0x040041BE RID: 16830
		public const int elementId_consoleButton7 = 49;

		// Token: 0x040041BF RID: 16831
		public const int elementId_consoleButton8 = 50;

		// Token: 0x040041C0 RID: 16832
		public const int elementId_consoleButton9 = 51;

		// Token: 0x040041C1 RID: 16833
		public const int elementId_consoleButton10 = 52;

		// Token: 0x040041C2 RID: 16834
		public const int elementId_mode1 = 61;

		// Token: 0x040041C3 RID: 16835
		public const int elementId_mode2 = 62;

		// Token: 0x040041C4 RID: 16836
		public const int elementId_mode3 = 63;

		// Token: 0x040041C5 RID: 16837
		public const int elementId_yoke = 69;

		// Token: 0x040041C6 RID: 16838
		public const int elementId_lever1 = 70;

		// Token: 0x040041C7 RID: 16839
		public const int elementId_lever2 = 71;

		// Token: 0x040041C8 RID: 16840
		public const int elementId_lever3 = 72;

		// Token: 0x040041C9 RID: 16841
		public const int elementId_lever4 = 73;

		// Token: 0x040041CA RID: 16842
		public const int elementId_lever5 = 74;

		// Token: 0x040041CB RID: 16843
		public const int elementId_leftGripHat = 75;

		// Token: 0x040041CC RID: 16844
		public const int elementId_rightGripHat = 76;
	}
}
