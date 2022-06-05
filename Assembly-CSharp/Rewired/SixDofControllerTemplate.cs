using System;

namespace Rewired
{
	// Token: 0x0200092F RID: 2351
	public sealed class SixDofControllerTemplate : ControllerTemplate, ISixDofControllerTemplate, IControllerTemplate
	{
		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x00113785 File Offset: 0x00111985
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(8);
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x0011378E File Offset: 0x0011198E
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(9);
			}
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x06004E71 RID: 20081 RVA: 0x00113798 File Offset: 0x00111998
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(10);
			}
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001137A2 File Offset: 0x001119A2
		IControllerTemplateAxis ISixDofControllerTemplate.extraAxis4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(11);
			}
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x001137AC File Offset: 0x001119AC
		IControllerTemplateButton ISixDofControllerTemplate.button1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x001137B6 File Offset: 0x001119B6
		IControllerTemplateButton ISixDofControllerTemplate.button2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x06004E75 RID: 20085 RVA: 0x001137C0 File Offset: 0x001119C0
		IControllerTemplateButton ISixDofControllerTemplate.button3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x06004E76 RID: 20086 RVA: 0x001137CA File Offset: 0x001119CA
		IControllerTemplateButton ISixDofControllerTemplate.button4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x06004E77 RID: 20087 RVA: 0x001137D4 File Offset: 0x001119D4
		IControllerTemplateButton ISixDofControllerTemplate.button5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(16);
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x06004E78 RID: 20088 RVA: 0x001137DE File Offset: 0x001119DE
		IControllerTemplateButton ISixDofControllerTemplate.button6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(17);
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x06004E79 RID: 20089 RVA: 0x001137E8 File Offset: 0x001119E8
		IControllerTemplateButton ISixDofControllerTemplate.button7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x06004E7A RID: 20090 RVA: 0x001137F2 File Offset: 0x001119F2
		IControllerTemplateButton ISixDofControllerTemplate.button8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001137FC File Offset: 0x001119FC
		IControllerTemplateButton ISixDofControllerTemplate.button9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x06004E7C RID: 20092 RVA: 0x00113806 File Offset: 0x00111A06
		IControllerTemplateButton ISixDofControllerTemplate.button10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x06004E7D RID: 20093 RVA: 0x00113810 File Offset: 0x00111A10
		IControllerTemplateButton ISixDofControllerTemplate.button11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x06004E7E RID: 20094 RVA: 0x0011381A File Offset: 0x00111A1A
		IControllerTemplateButton ISixDofControllerTemplate.button12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x06004E7F RID: 20095 RVA: 0x00113824 File Offset: 0x00111A24
		IControllerTemplateButton ISixDofControllerTemplate.button13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x06004E80 RID: 20096 RVA: 0x0011382E File Offset: 0x00111A2E
		IControllerTemplateButton ISixDofControllerTemplate.button14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x00113838 File Offset: 0x00111A38
		IControllerTemplateButton ISixDofControllerTemplate.button15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x06004E82 RID: 20098 RVA: 0x00113842 File Offset: 0x00111A42
		IControllerTemplateButton ISixDofControllerTemplate.button16
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x06004E83 RID: 20099 RVA: 0x0011384C File Offset: 0x00111A4C
		IControllerTemplateButton ISixDofControllerTemplate.button17
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(28);
			}
		}

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x06004E84 RID: 20100 RVA: 0x00113856 File Offset: 0x00111A56
		IControllerTemplateButton ISixDofControllerTemplate.button18
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(29);
			}
		}

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x06004E85 RID: 20101 RVA: 0x00113860 File Offset: 0x00111A60
		IControllerTemplateButton ISixDofControllerTemplate.button19
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(30);
			}
		}

		// Token: 0x17001A80 RID: 6784
		// (get) Token: 0x06004E86 RID: 20102 RVA: 0x0011386A File Offset: 0x00111A6A
		IControllerTemplateButton ISixDofControllerTemplate.button20
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(31);
			}
		}

		// Token: 0x17001A81 RID: 6785
		// (get) Token: 0x06004E87 RID: 20103 RVA: 0x00113874 File Offset: 0x00111A74
		IControllerTemplateButton ISixDofControllerTemplate.button21
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x06004E88 RID: 20104 RVA: 0x0011387E File Offset: 0x00111A7E
		IControllerTemplateButton ISixDofControllerTemplate.button22
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x06004E89 RID: 20105 RVA: 0x00113888 File Offset: 0x00111A88
		IControllerTemplateButton ISixDofControllerTemplate.button23
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x06004E8A RID: 20106 RVA: 0x00113892 File Offset: 0x00111A92
		IControllerTemplateButton ISixDofControllerTemplate.button24
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x06004E8B RID: 20107 RVA: 0x0011389C File Offset: 0x00111A9C
		IControllerTemplateButton ISixDofControllerTemplate.button25
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x001138A6 File Offset: 0x00111AA6
		IControllerTemplateButton ISixDofControllerTemplate.button26
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17001A87 RID: 6791
		// (get) Token: 0x06004E8D RID: 20109 RVA: 0x001138B0 File Offset: 0x00111AB0
		IControllerTemplateButton ISixDofControllerTemplate.button27
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x06004E8E RID: 20110 RVA: 0x001138BA File Offset: 0x00111ABA
		IControllerTemplateButton ISixDofControllerTemplate.button28
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x06004E8F RID: 20111 RVA: 0x001138C4 File Offset: 0x00111AC4
		IControllerTemplateButton ISixDofControllerTemplate.button29
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x06004E90 RID: 20112 RVA: 0x001138CE File Offset: 0x00111ACE
		IControllerTemplateButton ISixDofControllerTemplate.button30
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x06004E91 RID: 20113 RVA: 0x001138D8 File Offset: 0x00111AD8
		IControllerTemplateButton ISixDofControllerTemplate.button31
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x001138E2 File Offset: 0x00111AE2
		IControllerTemplateButton ISixDofControllerTemplate.button32
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06004E93 RID: 20115 RVA: 0x001138EC File Offset: 0x00111AEC
		IControllerTemplateHat ISixDofControllerTemplate.hat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(48);
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06004E94 RID: 20116 RVA: 0x001138F6 File Offset: 0x00111AF6
		IControllerTemplateHat ISixDofControllerTemplate.hat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(49);
			}
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x06004E95 RID: 20117 RVA: 0x00113900 File Offset: 0x00111B00
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(52);
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06004E96 RID: 20118 RVA: 0x0011390A File Offset: 0x00111B0A
		IControllerTemplateThrottle ISixDofControllerTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(53);
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x00113914 File Offset: 0x00111B14
		IControllerTemplateStick6D ISixDofControllerTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick6D>(54);
			}
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x0011391E File Offset: 0x00111B1E
		public SixDofControllerTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040041D1 RID: 16849
		public static readonly Guid typeGuid = new Guid("2599beb3-522b-43dd-a4ef-93fd60e5eafa");

		// Token: 0x040041D2 RID: 16850
		public const int elementId_positionX = 1;

		// Token: 0x040041D3 RID: 16851
		public const int elementId_positionY = 2;

		// Token: 0x040041D4 RID: 16852
		public const int elementId_positionZ = 0;

		// Token: 0x040041D5 RID: 16853
		public const int elementId_rotationX = 3;

		// Token: 0x040041D6 RID: 16854
		public const int elementId_rotationY = 5;

		// Token: 0x040041D7 RID: 16855
		public const int elementId_rotationZ = 4;

		// Token: 0x040041D8 RID: 16856
		public const int elementId_throttle1Axis = 6;

		// Token: 0x040041D9 RID: 16857
		public const int elementId_throttle1MinDetent = 50;

		// Token: 0x040041DA RID: 16858
		public const int elementId_throttle2Axis = 7;

		// Token: 0x040041DB RID: 16859
		public const int elementId_throttle2MinDetent = 51;

		// Token: 0x040041DC RID: 16860
		public const int elementId_extraAxis1 = 8;

		// Token: 0x040041DD RID: 16861
		public const int elementId_extraAxis2 = 9;

		// Token: 0x040041DE RID: 16862
		public const int elementId_extraAxis3 = 10;

		// Token: 0x040041DF RID: 16863
		public const int elementId_extraAxis4 = 11;

		// Token: 0x040041E0 RID: 16864
		public const int elementId_button1 = 12;

		// Token: 0x040041E1 RID: 16865
		public const int elementId_button2 = 13;

		// Token: 0x040041E2 RID: 16866
		public const int elementId_button3 = 14;

		// Token: 0x040041E3 RID: 16867
		public const int elementId_button4 = 15;

		// Token: 0x040041E4 RID: 16868
		public const int elementId_button5 = 16;

		// Token: 0x040041E5 RID: 16869
		public const int elementId_button6 = 17;

		// Token: 0x040041E6 RID: 16870
		public const int elementId_button7 = 18;

		// Token: 0x040041E7 RID: 16871
		public const int elementId_button8 = 19;

		// Token: 0x040041E8 RID: 16872
		public const int elementId_button9 = 20;

		// Token: 0x040041E9 RID: 16873
		public const int elementId_button10 = 21;

		// Token: 0x040041EA RID: 16874
		public const int elementId_button11 = 22;

		// Token: 0x040041EB RID: 16875
		public const int elementId_button12 = 23;

		// Token: 0x040041EC RID: 16876
		public const int elementId_button13 = 24;

		// Token: 0x040041ED RID: 16877
		public const int elementId_button14 = 25;

		// Token: 0x040041EE RID: 16878
		public const int elementId_button15 = 26;

		// Token: 0x040041EF RID: 16879
		public const int elementId_button16 = 27;

		// Token: 0x040041F0 RID: 16880
		public const int elementId_button17 = 28;

		// Token: 0x040041F1 RID: 16881
		public const int elementId_button18 = 29;

		// Token: 0x040041F2 RID: 16882
		public const int elementId_button19 = 30;

		// Token: 0x040041F3 RID: 16883
		public const int elementId_button20 = 31;

		// Token: 0x040041F4 RID: 16884
		public const int elementId_button21 = 55;

		// Token: 0x040041F5 RID: 16885
		public const int elementId_button22 = 56;

		// Token: 0x040041F6 RID: 16886
		public const int elementId_button23 = 57;

		// Token: 0x040041F7 RID: 16887
		public const int elementId_button24 = 58;

		// Token: 0x040041F8 RID: 16888
		public const int elementId_button25 = 59;

		// Token: 0x040041F9 RID: 16889
		public const int elementId_button26 = 60;

		// Token: 0x040041FA RID: 16890
		public const int elementId_button27 = 61;

		// Token: 0x040041FB RID: 16891
		public const int elementId_button28 = 62;

		// Token: 0x040041FC RID: 16892
		public const int elementId_button29 = 63;

		// Token: 0x040041FD RID: 16893
		public const int elementId_button30 = 64;

		// Token: 0x040041FE RID: 16894
		public const int elementId_button31 = 65;

		// Token: 0x040041FF RID: 16895
		public const int elementId_button32 = 66;

		// Token: 0x04004200 RID: 16896
		public const int elementId_hat1Up = 32;

		// Token: 0x04004201 RID: 16897
		public const int elementId_hat1UpRight = 33;

		// Token: 0x04004202 RID: 16898
		public const int elementId_hat1Right = 34;

		// Token: 0x04004203 RID: 16899
		public const int elementId_hat1DownRight = 35;

		// Token: 0x04004204 RID: 16900
		public const int elementId_hat1Down = 36;

		// Token: 0x04004205 RID: 16901
		public const int elementId_hat1DownLeft = 37;

		// Token: 0x04004206 RID: 16902
		public const int elementId_hat1Left = 38;

		// Token: 0x04004207 RID: 16903
		public const int elementId_hat1UpLeft = 39;

		// Token: 0x04004208 RID: 16904
		public const int elementId_hat2Up = 40;

		// Token: 0x04004209 RID: 16905
		public const int elementId_hat2UpRight = 41;

		// Token: 0x0400420A RID: 16906
		public const int elementId_hat2Right = 42;

		// Token: 0x0400420B RID: 16907
		public const int elementId_hat2DownRight = 43;

		// Token: 0x0400420C RID: 16908
		public const int elementId_hat2Down = 44;

		// Token: 0x0400420D RID: 16909
		public const int elementId_hat2DownLeft = 45;

		// Token: 0x0400420E RID: 16910
		public const int elementId_hat2Left = 46;

		// Token: 0x0400420F RID: 16911
		public const int elementId_hat2UpLeft = 47;

		// Token: 0x04004210 RID: 16912
		public const int elementId_hat1 = 48;

		// Token: 0x04004211 RID: 16913
		public const int elementId_hat2 = 49;

		// Token: 0x04004212 RID: 16914
		public const int elementId_throttle1 = 52;

		// Token: 0x04004213 RID: 16915
		public const int elementId_throttle2 = 53;

		// Token: 0x04004214 RID: 16916
		public const int elementId_stick = 54;
	}
}
