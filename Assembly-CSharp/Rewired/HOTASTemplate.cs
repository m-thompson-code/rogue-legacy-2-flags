using System;

namespace Rewired
{
	// Token: 0x0200092C RID: 2348
	public sealed class HOTASTemplate : ControllerTemplate, IHOTASTemplate, IControllerTemplate
	{
		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x06004DDD RID: 19933 RVA: 0x0011315B File Offset: 0x0011135B
		IControllerTemplateButton IHOTASTemplate.stickTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(3);
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x06004DDE RID: 19934 RVA: 0x00113164 File Offset: 0x00111364
		IControllerTemplateButton IHOTASTemplate.stickTriggerStage2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x06004DDF RID: 19935 RVA: 0x0011316D File Offset: 0x0011136D
		IControllerTemplateButton IHOTASTemplate.stickPinkyButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x06004DE0 RID: 19936 RVA: 0x00113176 File Offset: 0x00111376
		IControllerTemplateButton IHOTASTemplate.stickPinkyTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(154);
			}
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x00113183 File Offset: 0x00111383
		IControllerTemplateButton IHOTASTemplate.stickButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x170019E2 RID: 6626
		// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x0011318C File Offset: 0x0011138C
		IControllerTemplateButton IHOTASTemplate.stickButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x06004DE3 RID: 19939 RVA: 0x00113195 File Offset: 0x00111395
		IControllerTemplateButton IHOTASTemplate.stickButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x0011319E File Offset: 0x0011139E
		IControllerTemplateButton IHOTASTemplate.stickButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x001131A8 File Offset: 0x001113A8
		IControllerTemplateButton IHOTASTemplate.stickButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x001131B2 File Offset: 0x001113B2
		IControllerTemplateButton IHOTASTemplate.stickButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x06004DE7 RID: 19943 RVA: 0x001131BC File Offset: 0x001113BC
		IControllerTemplateButton IHOTASTemplate.stickButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x001131C6 File Offset: 0x001113C6
		IControllerTemplateButton IHOTASTemplate.stickButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x06004DE9 RID: 19945 RVA: 0x001131D0 File Offset: 0x001113D0
		IControllerTemplateButton IHOTASTemplate.stickButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x06004DEA RID: 19946 RVA: 0x001131DA File Offset: 0x001113DA
		IControllerTemplateButton IHOTASTemplate.stickButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x001131E4 File Offset: 0x001113E4
		IControllerTemplateButton IHOTASTemplate.stickBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001131EE File Offset: 0x001113EE
		IControllerTemplateButton IHOTASTemplate.stickBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x001131F8 File Offset: 0x001113F8
		IControllerTemplateButton IHOTASTemplate.stickBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x06004DEE RID: 19950 RVA: 0x00113202 File Offset: 0x00111402
		IControllerTemplateButton IHOTASTemplate.stickBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x06004DEF RID: 19951 RVA: 0x0011320C File Offset: 0x0011140C
		IControllerTemplateButton IHOTASTemplate.stickBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x06004DF0 RID: 19952 RVA: 0x00113216 File Offset: 0x00111416
		IControllerTemplateButton IHOTASTemplate.stickBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x06004DF1 RID: 19953 RVA: 0x00113220 File Offset: 0x00111420
		IControllerTemplateButton IHOTASTemplate.stickBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x06004DF2 RID: 19954 RVA: 0x0011322A File Offset: 0x0011142A
		IControllerTemplateButton IHOTASTemplate.stickBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x06004DF3 RID: 19955 RVA: 0x00113234 File Offset: 0x00111434
		IControllerTemplateButton IHOTASTemplate.stickBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x06004DF4 RID: 19956 RVA: 0x0011323E File Offset: 0x0011143E
		IControllerTemplateButton IHOTASTemplate.stickBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x06004DF5 RID: 19957 RVA: 0x00113248 File Offset: 0x00111448
		IControllerTemplateButton IHOTASTemplate.stickBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(161);
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x06004DF6 RID: 19958 RVA: 0x00113255 File Offset: 0x00111455
		IControllerTemplateButton IHOTASTemplate.stickBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(162);
			}
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x06004DF7 RID: 19959 RVA: 0x00113262 File Offset: 0x00111462
		IControllerTemplateButton IHOTASTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x06004DF8 RID: 19960 RVA: 0x0011326C File Offset: 0x0011146C
		IControllerTemplateButton IHOTASTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x00113276 File Offset: 0x00111476
		IControllerTemplateButton IHOTASTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x06004DFA RID: 19962 RVA: 0x00113280 File Offset: 0x00111480
		IControllerTemplateButton IHOTASTemplate.throttleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x06004DFB RID: 19963 RVA: 0x0011328A File Offset: 0x0011148A
		IControllerTemplateButton IHOTASTemplate.throttleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x06004DFC RID: 19964 RVA: 0x00113294 File Offset: 0x00111494
		IControllerTemplateButton IHOTASTemplate.throttleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x06004DFD RID: 19965 RVA: 0x0011329E File Offset: 0x0011149E
		IControllerTemplateButton IHOTASTemplate.throttleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x06004DFE RID: 19966 RVA: 0x001132A8 File Offset: 0x001114A8
		IControllerTemplateButton IHOTASTemplate.throttleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x06004DFF RID: 19967 RVA: 0x001132B2 File Offset: 0x001114B2
		IControllerTemplateButton IHOTASTemplate.throttleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x06004E00 RID: 19968 RVA: 0x001132BC File Offset: 0x001114BC
		IControllerTemplateButton IHOTASTemplate.throttleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x17001A01 RID: 6657
		// (get) Token: 0x06004E01 RID: 19969 RVA: 0x001132C6 File Offset: 0x001114C6
		IControllerTemplateButton IHOTASTemplate.throttleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001132D0 File Offset: 0x001114D0
		IControllerTemplateButton IHOTASTemplate.throttleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x06004E03 RID: 19971 RVA: 0x001132DA File Offset: 0x001114DA
		IControllerTemplateButton IHOTASTemplate.throttleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x06004E04 RID: 19972 RVA: 0x001132E4 File Offset: 0x001114E4
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x06004E05 RID: 19973 RVA: 0x001132EE File Offset: 0x001114EE
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x06004E06 RID: 19974 RVA: 0x001132F8 File Offset: 0x001114F8
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x06004E07 RID: 19975 RVA: 0x00113302 File Offset: 0x00111502
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x06004E08 RID: 19976 RVA: 0x0011330C File Offset: 0x0011150C
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x06004E09 RID: 19977 RVA: 0x00113316 File Offset: 0x00111516
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x06004E0A RID: 19978 RVA: 0x00113320 File Offset: 0x00111520
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x06004E0B RID: 19979 RVA: 0x0011332A File Offset: 0x0011152A
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(67);
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x00113334 File Offset: 0x00111534
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(68);
			}
		}

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x06004E0D RID: 19981 RVA: 0x0011333E File Offset: 0x0011153E
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(69);
			}
		}

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x06004E0E RID: 19982 RVA: 0x00113348 File Offset: 0x00111548
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(132);
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x06004E0F RID: 19983 RVA: 0x00113355 File Offset: 0x00111555
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(133);
			}
		}

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x06004E10 RID: 19984 RVA: 0x00113362 File Offset: 0x00111562
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(134);
			}
		}

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x06004E11 RID: 19985 RVA: 0x0011336F File Offset: 0x0011156F
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(135);
			}
		}

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x0011337C File Offset: 0x0011157C
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(136);
			}
		}

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x00113389 File Offset: 0x00111589
		IControllerTemplateAxis IHOTASTemplate.throttleSlider1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(70);
			}
		}

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x06004E14 RID: 19988 RVA: 0x00113393 File Offset: 0x00111593
		IControllerTemplateAxis IHOTASTemplate.throttleSlider2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(71);
			}
		}

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0011339D File Offset: 0x0011159D
		IControllerTemplateAxis IHOTASTemplate.throttleSlider3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(72);
			}
		}

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x06004E16 RID: 19990 RVA: 0x001133A7 File Offset: 0x001115A7
		IControllerTemplateAxis IHOTASTemplate.throttleSlider4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(73);
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x06004E17 RID: 19991 RVA: 0x001133B1 File Offset: 0x001115B1
		IControllerTemplateAxis IHOTASTemplate.throttleDial1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(74);
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x001133BB File Offset: 0x001115BB
		IControllerTemplateAxis IHOTASTemplate.throttleDial2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(142);
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x06004E19 RID: 19993 RVA: 0x001133C8 File Offset: 0x001115C8
		IControllerTemplateAxis IHOTASTemplate.throttleDial3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(143);
			}
		}

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x001133D5 File Offset: 0x001115D5
		IControllerTemplateAxis IHOTASTemplate.throttleDial4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(144);
			}
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x06004E1B RID: 19995 RVA: 0x001133E2 File Offset: 0x001115E2
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(145);
			}
		}

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x001133EF File Offset: 0x001115EF
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(146);
			}
		}

		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x06004E1D RID: 19997 RVA: 0x001133FC File Offset: 0x001115FC
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(147);
			}
		}

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x06004E1E RID: 19998 RVA: 0x00113409 File Offset: 0x00111609
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(148);
			}
		}

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x00113416 File Offset: 0x00111616
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(149);
			}
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x06004E20 RID: 20000 RVA: 0x00113423 File Offset: 0x00111623
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(150);
			}
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x06004E21 RID: 20001 RVA: 0x00113430 File Offset: 0x00111630
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(151);
			}
		}

		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x06004E22 RID: 20002 RVA: 0x0011343D File Offset: 0x0011163D
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(152);
			}
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x06004E23 RID: 20003 RVA: 0x0011344A File Offset: 0x0011164A
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(153);
			}
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x06004E24 RID: 20004 RVA: 0x00113457 File Offset: 0x00111657
		IControllerTemplateAxis IHOTASTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(168);
			}
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x06004E25 RID: 20005 RVA: 0x00113464 File Offset: 0x00111664
		IControllerTemplateAxis IHOTASTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(169);
			}
		}

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x06004E26 RID: 20006 RVA: 0x00113471 File Offset: 0x00111671
		IControllerTemplateAxis IHOTASTemplate.slidePedals
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(170);
			}
		}

		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x06004E27 RID: 20007 RVA: 0x0011347E File Offset: 0x0011167E
		IControllerTemplateStick IHOTASTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick>(171);
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x06004E28 RID: 20008 RVA: 0x0011348B File Offset: 0x0011168B
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick1
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(172);
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x06004E29 RID: 20009 RVA: 0x00113498 File Offset: 0x00111698
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick2
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(173);
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x06004E2A RID: 20010 RVA: 0x001134A5 File Offset: 0x001116A5
		IControllerTemplateHat IHOTASTemplate.stickHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(174);
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x06004E2B RID: 20011 RVA: 0x001134B2 File Offset: 0x001116B2
		IControllerTemplateHat IHOTASTemplate.stickHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(175);
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x06004E2C RID: 20012 RVA: 0x001134BF File Offset: 0x001116BF
		IControllerTemplateHat IHOTASTemplate.stickHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(176);
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x06004E2D RID: 20013 RVA: 0x001134CC File Offset: 0x001116CC
		IControllerTemplateHat IHOTASTemplate.stickHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(177);
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x06004E2E RID: 20014 RVA: 0x001134D9 File Offset: 0x001116D9
		IControllerTemplateThrottle IHOTASTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(178);
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x06004E2F RID: 20015 RVA: 0x001134E6 File Offset: 0x001116E6
		IControllerTemplateThrottle IHOTASTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(179);
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x06004E30 RID: 20016 RVA: 0x001134F3 File Offset: 0x001116F3
		IControllerTemplateThumbStick IHOTASTemplate.throttleMiniStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(180);
			}
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x06004E31 RID: 20017 RVA: 0x00113500 File Offset: 0x00111700
		IControllerTemplateHat IHOTASTemplate.throttleHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(181);
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x06004E32 RID: 20018 RVA: 0x0011350D File Offset: 0x0011170D
		IControllerTemplateHat IHOTASTemplate.throttleHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(182);
			}
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x0011351A File Offset: 0x0011171A
		IControllerTemplateHat IHOTASTemplate.throttleHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(183);
			}
		}

		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x00113527 File Offset: 0x00111727
		IControllerTemplateHat IHOTASTemplate.throttleHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(184);
			}
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00113534 File Offset: 0x00111734
		public HOTASTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040040D6 RID: 16598
		public static readonly Guid typeGuid = new Guid("061a00cf-d8c2-4f8d-8cb5-a15a010bc53e");

		// Token: 0x040040D7 RID: 16599
		public const int elementId_stickX = 0;

		// Token: 0x040040D8 RID: 16600
		public const int elementId_stickY = 1;

		// Token: 0x040040D9 RID: 16601
		public const int elementId_stickRotate = 2;

		// Token: 0x040040DA RID: 16602
		public const int elementId_stickMiniStick1X = 78;

		// Token: 0x040040DB RID: 16603
		public const int elementId_stickMiniStick1Y = 79;

		// Token: 0x040040DC RID: 16604
		public const int elementId_stickMiniStick1Press = 80;

		// Token: 0x040040DD RID: 16605
		public const int elementId_stickMiniStick2X = 81;

		// Token: 0x040040DE RID: 16606
		public const int elementId_stickMiniStick2Y = 82;

		// Token: 0x040040DF RID: 16607
		public const int elementId_stickMiniStick2Press = 83;

		// Token: 0x040040E0 RID: 16608
		public const int elementId_stickTrigger = 3;

		// Token: 0x040040E1 RID: 16609
		public const int elementId_stickTriggerStage2 = 4;

		// Token: 0x040040E2 RID: 16610
		public const int elementId_stickPinkyButton = 5;

		// Token: 0x040040E3 RID: 16611
		public const int elementId_stickPinkyTrigger = 154;

		// Token: 0x040040E4 RID: 16612
		public const int elementId_stickButton1 = 6;

		// Token: 0x040040E5 RID: 16613
		public const int elementId_stickButton2 = 7;

		// Token: 0x040040E6 RID: 16614
		public const int elementId_stickButton3 = 8;

		// Token: 0x040040E7 RID: 16615
		public const int elementId_stickButton4 = 9;

		// Token: 0x040040E8 RID: 16616
		public const int elementId_stickButton5 = 10;

		// Token: 0x040040E9 RID: 16617
		public const int elementId_stickButton6 = 11;

		// Token: 0x040040EA RID: 16618
		public const int elementId_stickButton7 = 12;

		// Token: 0x040040EB RID: 16619
		public const int elementId_stickButton8 = 13;

		// Token: 0x040040EC RID: 16620
		public const int elementId_stickButton9 = 14;

		// Token: 0x040040ED RID: 16621
		public const int elementId_stickButton10 = 15;

		// Token: 0x040040EE RID: 16622
		public const int elementId_stickBaseButton1 = 18;

		// Token: 0x040040EF RID: 16623
		public const int elementId_stickBaseButton2 = 19;

		// Token: 0x040040F0 RID: 16624
		public const int elementId_stickBaseButton3 = 20;

		// Token: 0x040040F1 RID: 16625
		public const int elementId_stickBaseButton4 = 21;

		// Token: 0x040040F2 RID: 16626
		public const int elementId_stickBaseButton5 = 22;

		// Token: 0x040040F3 RID: 16627
		public const int elementId_stickBaseButton6 = 23;

		// Token: 0x040040F4 RID: 16628
		public const int elementId_stickBaseButton7 = 24;

		// Token: 0x040040F5 RID: 16629
		public const int elementId_stickBaseButton8 = 25;

		// Token: 0x040040F6 RID: 16630
		public const int elementId_stickBaseButton9 = 26;

		// Token: 0x040040F7 RID: 16631
		public const int elementId_stickBaseButton10 = 27;

		// Token: 0x040040F8 RID: 16632
		public const int elementId_stickBaseButton11 = 161;

		// Token: 0x040040F9 RID: 16633
		public const int elementId_stickBaseButton12 = 162;

		// Token: 0x040040FA RID: 16634
		public const int elementId_stickHat1Up = 28;

		// Token: 0x040040FB RID: 16635
		public const int elementId_stickHat1UpRight = 29;

		// Token: 0x040040FC RID: 16636
		public const int elementId_stickHat1Right = 30;

		// Token: 0x040040FD RID: 16637
		public const int elementId_stickHat1DownRight = 31;

		// Token: 0x040040FE RID: 16638
		public const int elementId_stickHat1Down = 32;

		// Token: 0x040040FF RID: 16639
		public const int elementId_stickHat1DownLeft = 33;

		// Token: 0x04004100 RID: 16640
		public const int elementId_stickHat1Left = 34;

		// Token: 0x04004101 RID: 16641
		public const int elementId_stickHat1Up_Left = 35;

		// Token: 0x04004102 RID: 16642
		public const int elementId_stickHat2Up = 36;

		// Token: 0x04004103 RID: 16643
		public const int elementId_stickHat2Up_right = 37;

		// Token: 0x04004104 RID: 16644
		public const int elementId_stickHat2Right = 38;

		// Token: 0x04004105 RID: 16645
		public const int elementId_stickHat2Down_Right = 39;

		// Token: 0x04004106 RID: 16646
		public const int elementId_stickHat2Down = 40;

		// Token: 0x04004107 RID: 16647
		public const int elementId_stickHat2Down_Left = 41;

		// Token: 0x04004108 RID: 16648
		public const int elementId_stickHat2Left = 42;

		// Token: 0x04004109 RID: 16649
		public const int elementId_stickHat2Up_Left = 43;

		// Token: 0x0400410A RID: 16650
		public const int elementId_stickHat3Up = 84;

		// Token: 0x0400410B RID: 16651
		public const int elementId_stickHat3Up_Right = 85;

		// Token: 0x0400410C RID: 16652
		public const int elementId_stickHat3Right = 86;

		// Token: 0x0400410D RID: 16653
		public const int elementId_stickHat3Down_Right = 87;

		// Token: 0x0400410E RID: 16654
		public const int elementId_stickHat3Down = 88;

		// Token: 0x0400410F RID: 16655
		public const int elementId_stickHat3Down_Left = 89;

		// Token: 0x04004110 RID: 16656
		public const int elementId_stickHat3Left = 90;

		// Token: 0x04004111 RID: 16657
		public const int elementId_stickHat3Up_Left = 91;

		// Token: 0x04004112 RID: 16658
		public const int elementId_stickHat4Up = 92;

		// Token: 0x04004113 RID: 16659
		public const int elementId_stickHat4Up_Right = 93;

		// Token: 0x04004114 RID: 16660
		public const int elementId_stickHat4Right = 94;

		// Token: 0x04004115 RID: 16661
		public const int elementId_stickHat4Down_Right = 95;

		// Token: 0x04004116 RID: 16662
		public const int elementId_stickHat4Down = 96;

		// Token: 0x04004117 RID: 16663
		public const int elementId_stickHat4Down_Left = 97;

		// Token: 0x04004118 RID: 16664
		public const int elementId_stickHat4Left = 98;

		// Token: 0x04004119 RID: 16665
		public const int elementId_stickHat4Up_Left = 99;

		// Token: 0x0400411A RID: 16666
		public const int elementId_mode1 = 44;

		// Token: 0x0400411B RID: 16667
		public const int elementId_mode2 = 45;

		// Token: 0x0400411C RID: 16668
		public const int elementId_mode3 = 46;

		// Token: 0x0400411D RID: 16669
		public const int elementId_throttle1Axis = 49;

		// Token: 0x0400411E RID: 16670
		public const int elementId_throttle2Axis = 155;

		// Token: 0x0400411F RID: 16671
		public const int elementId_throttle1MinDetent = 166;

		// Token: 0x04004120 RID: 16672
		public const int elementId_throttle2MinDetent = 167;

		// Token: 0x04004121 RID: 16673
		public const int elementId_throttleButton1 = 50;

		// Token: 0x04004122 RID: 16674
		public const int elementId_throttleButton2 = 51;

		// Token: 0x04004123 RID: 16675
		public const int elementId_throttleButton3 = 52;

		// Token: 0x04004124 RID: 16676
		public const int elementId_throttleButton4 = 53;

		// Token: 0x04004125 RID: 16677
		public const int elementId_throttleButton5 = 54;

		// Token: 0x04004126 RID: 16678
		public const int elementId_throttleButton6 = 55;

		// Token: 0x04004127 RID: 16679
		public const int elementId_throttleButton7 = 56;

		// Token: 0x04004128 RID: 16680
		public const int elementId_throttleButton8 = 57;

		// Token: 0x04004129 RID: 16681
		public const int elementId_throttleButton9 = 58;

		// Token: 0x0400412A RID: 16682
		public const int elementId_throttleButton10 = 59;

		// Token: 0x0400412B RID: 16683
		public const int elementId_throttleBaseButton1 = 60;

		// Token: 0x0400412C RID: 16684
		public const int elementId_throttleBaseButton2 = 61;

		// Token: 0x0400412D RID: 16685
		public const int elementId_throttleBaseButton3 = 62;

		// Token: 0x0400412E RID: 16686
		public const int elementId_throttleBaseButton4 = 63;

		// Token: 0x0400412F RID: 16687
		public const int elementId_throttleBaseButton5 = 64;

		// Token: 0x04004130 RID: 16688
		public const int elementId_throttleBaseButton6 = 65;

		// Token: 0x04004131 RID: 16689
		public const int elementId_throttleBaseButton7 = 66;

		// Token: 0x04004132 RID: 16690
		public const int elementId_throttleBaseButton8 = 67;

		// Token: 0x04004133 RID: 16691
		public const int elementId_throttleBaseButton9 = 68;

		// Token: 0x04004134 RID: 16692
		public const int elementId_throttleBaseButton10 = 69;

		// Token: 0x04004135 RID: 16693
		public const int elementId_throttleBaseButton11 = 132;

		// Token: 0x04004136 RID: 16694
		public const int elementId_throttleBaseButton12 = 133;

		// Token: 0x04004137 RID: 16695
		public const int elementId_throttleBaseButton13 = 134;

		// Token: 0x04004138 RID: 16696
		public const int elementId_throttleBaseButton14 = 135;

		// Token: 0x04004139 RID: 16697
		public const int elementId_throttleBaseButton15 = 136;

		// Token: 0x0400413A RID: 16698
		public const int elementId_throttleSlider1 = 70;

		// Token: 0x0400413B RID: 16699
		public const int elementId_throttleSlider2 = 71;

		// Token: 0x0400413C RID: 16700
		public const int elementId_throttleSlider3 = 72;

		// Token: 0x0400413D RID: 16701
		public const int elementId_throttleSlider4 = 73;

		// Token: 0x0400413E RID: 16702
		public const int elementId_throttleDial1 = 74;

		// Token: 0x0400413F RID: 16703
		public const int elementId_throttleDial2 = 142;

		// Token: 0x04004140 RID: 16704
		public const int elementId_throttleDial3 = 143;

		// Token: 0x04004141 RID: 16705
		public const int elementId_throttleDial4 = 144;

		// Token: 0x04004142 RID: 16706
		public const int elementId_throttleMiniStickX = 75;

		// Token: 0x04004143 RID: 16707
		public const int elementId_throttleMiniStickY = 76;

		// Token: 0x04004144 RID: 16708
		public const int elementId_throttleMiniStickPress = 77;

		// Token: 0x04004145 RID: 16709
		public const int elementId_throttleWheel1Forward = 145;

		// Token: 0x04004146 RID: 16710
		public const int elementId_throttleWheel1Back = 146;

		// Token: 0x04004147 RID: 16711
		public const int elementId_throttleWheel1Press = 147;

		// Token: 0x04004148 RID: 16712
		public const int elementId_throttleWheel2Forward = 148;

		// Token: 0x04004149 RID: 16713
		public const int elementId_throttleWheel2Back = 149;

		// Token: 0x0400414A RID: 16714
		public const int elementId_throttleWheel2Press = 150;

		// Token: 0x0400414B RID: 16715
		public const int elementId_throttleWheel3Forward = 151;

		// Token: 0x0400414C RID: 16716
		public const int elementId_throttleWheel3Back = 152;

		// Token: 0x0400414D RID: 16717
		public const int elementId_throttleWheel3Press = 153;

		// Token: 0x0400414E RID: 16718
		public const int elementId_throttleHat1Up = 100;

		// Token: 0x0400414F RID: 16719
		public const int elementId_throttleHat1Up_Right = 101;

		// Token: 0x04004150 RID: 16720
		public const int elementId_throttleHat1Right = 102;

		// Token: 0x04004151 RID: 16721
		public const int elementId_throttleHat1Down_Right = 103;

		// Token: 0x04004152 RID: 16722
		public const int elementId_throttleHat1Down = 104;

		// Token: 0x04004153 RID: 16723
		public const int elementId_throttleHat1Down_Left = 105;

		// Token: 0x04004154 RID: 16724
		public const int elementId_throttleHat1Left = 106;

		// Token: 0x04004155 RID: 16725
		public const int elementId_throttleHat1Up_Left = 107;

		// Token: 0x04004156 RID: 16726
		public const int elementId_throttleHat2Up = 108;

		// Token: 0x04004157 RID: 16727
		public const int elementId_throttleHat2Up_Right = 109;

		// Token: 0x04004158 RID: 16728
		public const int elementId_throttleHat2Right = 110;

		// Token: 0x04004159 RID: 16729
		public const int elementId_throttleHat2Down_Right = 111;

		// Token: 0x0400415A RID: 16730
		public const int elementId_throttleHat2Down = 112;

		// Token: 0x0400415B RID: 16731
		public const int elementId_throttleHat2Down_Left = 113;

		// Token: 0x0400415C RID: 16732
		public const int elementId_throttleHat2Left = 114;

		// Token: 0x0400415D RID: 16733
		public const int elementId_throttleHat2Up_Left = 115;

		// Token: 0x0400415E RID: 16734
		public const int elementId_throttleHat3Up = 116;

		// Token: 0x0400415F RID: 16735
		public const int elementId_throttleHat3Up_Right = 117;

		// Token: 0x04004160 RID: 16736
		public const int elementId_throttleHat3Right = 118;

		// Token: 0x04004161 RID: 16737
		public const int elementId_throttleHat3Down_Right = 119;

		// Token: 0x04004162 RID: 16738
		public const int elementId_throttleHat3Down = 120;

		// Token: 0x04004163 RID: 16739
		public const int elementId_throttleHat3Down_Left = 121;

		// Token: 0x04004164 RID: 16740
		public const int elementId_throttleHat3Left = 122;

		// Token: 0x04004165 RID: 16741
		public const int elementId_throttleHat3Up_Left = 123;

		// Token: 0x04004166 RID: 16742
		public const int elementId_throttleHat4Up = 124;

		// Token: 0x04004167 RID: 16743
		public const int elementId_throttleHat4Up_Right = 125;

		// Token: 0x04004168 RID: 16744
		public const int elementId_throttleHat4Right = 126;

		// Token: 0x04004169 RID: 16745
		public const int elementId_throttleHat4Down_Right = 127;

		// Token: 0x0400416A RID: 16746
		public const int elementId_throttleHat4Down = 128;

		// Token: 0x0400416B RID: 16747
		public const int elementId_throttleHat4Down_Left = 129;

		// Token: 0x0400416C RID: 16748
		public const int elementId_throttleHat4Left = 130;

		// Token: 0x0400416D RID: 16749
		public const int elementId_throttleHat4Up_Left = 131;

		// Token: 0x0400416E RID: 16750
		public const int elementId_leftPedal = 168;

		// Token: 0x0400416F RID: 16751
		public const int elementId_rightPedal = 169;

		// Token: 0x04004170 RID: 16752
		public const int elementId_slidePedals = 170;

		// Token: 0x04004171 RID: 16753
		public const int elementId_stick = 171;

		// Token: 0x04004172 RID: 16754
		public const int elementId_stickMiniStick1 = 172;

		// Token: 0x04004173 RID: 16755
		public const int elementId_stickMiniStick2 = 173;

		// Token: 0x04004174 RID: 16756
		public const int elementId_stickHat1 = 174;

		// Token: 0x04004175 RID: 16757
		public const int elementId_stickHat2 = 175;

		// Token: 0x04004176 RID: 16758
		public const int elementId_stickHat3 = 176;

		// Token: 0x04004177 RID: 16759
		public const int elementId_stickHat4 = 177;

		// Token: 0x04004178 RID: 16760
		public const int elementId_throttle1 = 178;

		// Token: 0x04004179 RID: 16761
		public const int elementId_throttle2 = 179;

		// Token: 0x0400417A RID: 16762
		public const int elementId_throttleMiniStick = 180;

		// Token: 0x0400417B RID: 16763
		public const int elementId_throttleHat1 = 181;

		// Token: 0x0400417C RID: 16764
		public const int elementId_throttleHat2 = 182;

		// Token: 0x0400417D RID: 16765
		public const int elementId_throttleHat3 = 183;

		// Token: 0x0400417E RID: 16766
		public const int elementId_throttleHat4 = 184;
	}
}
