using System;

namespace Rewired
{
	// Token: 0x02000EAB RID: 3755
	public sealed class HOTASTemplate : ControllerTemplate, IHOTASTemplate, IControllerTemplate
	{
		// Token: 0x170022DA RID: 8922
		// (get) Token: 0x06006AE4 RID: 27364 RVA: 0x0003AC30 File Offset: 0x00038E30
		IControllerTemplateButton IHOTASTemplate.stickTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(3);
			}
		}

		// Token: 0x170022DB RID: 8923
		// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x0003AA38 File Offset: 0x00038C38
		IControllerTemplateButton IHOTASTemplate.stickTriggerStage2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(4);
			}
		}

		// Token: 0x170022DC RID: 8924
		// (get) Token: 0x06006AE6 RID: 27366 RVA: 0x0003AA41 File Offset: 0x00038C41
		IControllerTemplateButton IHOTASTemplate.stickPinkyButton
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(5);
			}
		}

		// Token: 0x170022DD RID: 8925
		// (get) Token: 0x06006AE7 RID: 27367 RVA: 0x0003AC39 File Offset: 0x00038E39
		IControllerTemplateButton IHOTASTemplate.stickPinkyTrigger
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(154);
			}
		}

		// Token: 0x170022DE RID: 8926
		// (get) Token: 0x06006AE8 RID: 27368 RVA: 0x0003AA4A File Offset: 0x00038C4A
		IControllerTemplateButton IHOTASTemplate.stickButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(6);
			}
		}

		// Token: 0x170022DF RID: 8927
		// (get) Token: 0x06006AE9 RID: 27369 RVA: 0x0003AA53 File Offset: 0x00038C53
		IControllerTemplateButton IHOTASTemplate.stickButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(7);
			}
		}

		// Token: 0x170022E0 RID: 8928
		// (get) Token: 0x06006AEA RID: 27370 RVA: 0x0003AA5C File Offset: 0x00038C5C
		IControllerTemplateButton IHOTASTemplate.stickButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(8);
			}
		}

		// Token: 0x170022E1 RID: 8929
		// (get) Token: 0x06006AEB RID: 27371 RVA: 0x0003AA65 File Offset: 0x00038C65
		IControllerTemplateButton IHOTASTemplate.stickButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(9);
			}
		}

		// Token: 0x170022E2 RID: 8930
		// (get) Token: 0x06006AEC RID: 27372 RVA: 0x0003AA6F File Offset: 0x00038C6F
		IControllerTemplateButton IHOTASTemplate.stickButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(10);
			}
		}

		// Token: 0x170022E3 RID: 8931
		// (get) Token: 0x06006AED RID: 27373 RVA: 0x0003AB11 File Offset: 0x00038D11
		IControllerTemplateButton IHOTASTemplate.stickButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(11);
			}
		}

		// Token: 0x170022E4 RID: 8932
		// (get) Token: 0x06006AEE RID: 27374 RVA: 0x0003AA83 File Offset: 0x00038C83
		IControllerTemplateButton IHOTASTemplate.stickButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(12);
			}
		}

		// Token: 0x170022E5 RID: 8933
		// (get) Token: 0x06006AEF RID: 27375 RVA: 0x0003AB1B File Offset: 0x00038D1B
		IControllerTemplateButton IHOTASTemplate.stickButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(13);
			}
		}

		// Token: 0x170022E6 RID: 8934
		// (get) Token: 0x06006AF0 RID: 27376 RVA: 0x0003AA97 File Offset: 0x00038C97
		IControllerTemplateButton IHOTASTemplate.stickButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(14);
			}
		}

		// Token: 0x170022E7 RID: 8935
		// (get) Token: 0x06006AF1 RID: 27377 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		IControllerTemplateButton IHOTASTemplate.stickButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(15);
			}
		}

		// Token: 0x170022E8 RID: 8936
		// (get) Token: 0x06006AF2 RID: 27378 RVA: 0x0003AB2F File Offset: 0x00038D2F
		IControllerTemplateButton IHOTASTemplate.stickBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(18);
			}
		}

		// Token: 0x170022E9 RID: 8937
		// (get) Token: 0x06006AF3 RID: 27379 RVA: 0x0003AB39 File Offset: 0x00038D39
		IControllerTemplateButton IHOTASTemplate.stickBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(19);
			}
		}

		// Token: 0x170022EA RID: 8938
		// (get) Token: 0x06006AF4 RID: 27380 RVA: 0x0003AB43 File Offset: 0x00038D43
		IControllerTemplateButton IHOTASTemplate.stickBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(20);
			}
		}

		// Token: 0x170022EB RID: 8939
		// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x0003AB4D File Offset: 0x00038D4D
		IControllerTemplateButton IHOTASTemplate.stickBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(21);
			}
		}

		// Token: 0x170022EC RID: 8940
		// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x0003AB57 File Offset: 0x00038D57
		IControllerTemplateButton IHOTASTemplate.stickBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(22);
			}
		}

		// Token: 0x170022ED RID: 8941
		// (get) Token: 0x06006AF7 RID: 27383 RVA: 0x0003AB61 File Offset: 0x00038D61
		IControllerTemplateButton IHOTASTemplate.stickBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(23);
			}
		}

		// Token: 0x170022EE RID: 8942
		// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x0003AB6B File Offset: 0x00038D6B
		IControllerTemplateButton IHOTASTemplate.stickBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(24);
			}
		}

		// Token: 0x170022EF RID: 8943
		// (get) Token: 0x06006AF9 RID: 27385 RVA: 0x0003AB75 File Offset: 0x00038D75
		IControllerTemplateButton IHOTASTemplate.stickBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(25);
			}
		}

		// Token: 0x170022F0 RID: 8944
		// (get) Token: 0x06006AFA RID: 27386 RVA: 0x0003AB7F File Offset: 0x00038D7F
		IControllerTemplateButton IHOTASTemplate.stickBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(26);
			}
		}

		// Token: 0x170022F1 RID: 8945
		// (get) Token: 0x06006AFB RID: 27387 RVA: 0x0003AB89 File Offset: 0x00038D89
		IControllerTemplateButton IHOTASTemplate.stickBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(27);
			}
		}

		// Token: 0x170022F2 RID: 8946
		// (get) Token: 0x06006AFC RID: 27388 RVA: 0x0003AC46 File Offset: 0x00038E46
		IControllerTemplateButton IHOTASTemplate.stickBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(161);
			}
		}

		// Token: 0x170022F3 RID: 8947
		// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0003AC53 File Offset: 0x00038E53
		IControllerTemplateButton IHOTASTemplate.stickBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(162);
			}
		}

		// Token: 0x170022F4 RID: 8948
		// (get) Token: 0x06006AFE RID: 27390 RVA: 0x0003ABE3 File Offset: 0x00038DE3
		IControllerTemplateButton IHOTASTemplate.mode1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(44);
			}
		}

		// Token: 0x170022F5 RID: 8949
		// (get) Token: 0x06006AFF RID: 27391 RVA: 0x0003AC60 File Offset: 0x00038E60
		IControllerTemplateButton IHOTASTemplate.mode2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(45);
			}
		}

		// Token: 0x170022F6 RID: 8950
		// (get) Token: 0x06006B00 RID: 27392 RVA: 0x0003AC6A File Offset: 0x00038E6A
		IControllerTemplateButton IHOTASTemplate.mode3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(46);
			}
		}

		// Token: 0x170022F7 RID: 8951
		// (get) Token: 0x06006B01 RID: 27393 RVA: 0x0003AC74 File Offset: 0x00038E74
		IControllerTemplateButton IHOTASTemplate.throttleButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(50);
			}
		}

		// Token: 0x170022F8 RID: 8952
		// (get) Token: 0x06006B02 RID: 27394 RVA: 0x0003AC7E File Offset: 0x00038E7E
		IControllerTemplateButton IHOTASTemplate.throttleButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(51);
			}
		}

		// Token: 0x170022F9 RID: 8953
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x0003AC88 File Offset: 0x00038E88
		IControllerTemplateButton IHOTASTemplate.throttleButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(52);
			}
		}

		// Token: 0x170022FA RID: 8954
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x0003AC92 File Offset: 0x00038E92
		IControllerTemplateButton IHOTASTemplate.throttleButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(53);
			}
		}

		// Token: 0x170022FB RID: 8955
		// (get) Token: 0x06006B05 RID: 27397 RVA: 0x0003AC9C File Offset: 0x00038E9C
		IControllerTemplateButton IHOTASTemplate.throttleButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(54);
			}
		}

		// Token: 0x170022FC RID: 8956
		// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0003ACA6 File Offset: 0x00038EA6
		IControllerTemplateButton IHOTASTemplate.throttleButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(55);
			}
		}

		// Token: 0x170022FD RID: 8957
		// (get) Token: 0x06006B07 RID: 27399 RVA: 0x0003ACB0 File Offset: 0x00038EB0
		IControllerTemplateButton IHOTASTemplate.throttleButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(56);
			}
		}

		// Token: 0x170022FE RID: 8958
		// (get) Token: 0x06006B08 RID: 27400 RVA: 0x0003ACBA File Offset: 0x00038EBA
		IControllerTemplateButton IHOTASTemplate.throttleButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(57);
			}
		}

		// Token: 0x170022FF RID: 8959
		// (get) Token: 0x06006B09 RID: 27401 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		IControllerTemplateButton IHOTASTemplate.throttleButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(58);
			}
		}

		// Token: 0x17002300 RID: 8960
		// (get) Token: 0x06006B0A RID: 27402 RVA: 0x0003ACCE File Offset: 0x00038ECE
		IControllerTemplateButton IHOTASTemplate.throttleButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(59);
			}
		}

		// Token: 0x17002301 RID: 8961
		// (get) Token: 0x06006B0B RID: 27403 RVA: 0x0003ACD8 File Offset: 0x00038ED8
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton1
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(60);
			}
		}

		// Token: 0x17002302 RID: 8962
		// (get) Token: 0x06006B0C RID: 27404 RVA: 0x0003ACE2 File Offset: 0x00038EE2
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton2
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(61);
			}
		}

		// Token: 0x17002303 RID: 8963
		// (get) Token: 0x06006B0D RID: 27405 RVA: 0x0003ACEC File Offset: 0x00038EEC
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton3
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(62);
			}
		}

		// Token: 0x17002304 RID: 8964
		// (get) Token: 0x06006B0E RID: 27406 RVA: 0x0003ACF6 File Offset: 0x00038EF6
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton4
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(63);
			}
		}

		// Token: 0x17002305 RID: 8965
		// (get) Token: 0x06006B0F RID: 27407 RVA: 0x0003AD00 File Offset: 0x00038F00
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton5
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(64);
			}
		}

		// Token: 0x17002306 RID: 8966
		// (get) Token: 0x06006B10 RID: 27408 RVA: 0x0003AD0A File Offset: 0x00038F0A
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton6
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(65);
			}
		}

		// Token: 0x17002307 RID: 8967
		// (get) Token: 0x06006B11 RID: 27409 RVA: 0x0003AD14 File Offset: 0x00038F14
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton7
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(66);
			}
		}

		// Token: 0x17002308 RID: 8968
		// (get) Token: 0x06006B12 RID: 27410 RVA: 0x0003AD1E File Offset: 0x00038F1E
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton8
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(67);
			}
		}

		// Token: 0x17002309 RID: 8969
		// (get) Token: 0x06006B13 RID: 27411 RVA: 0x0003AD28 File Offset: 0x00038F28
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton9
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(68);
			}
		}

		// Token: 0x1700230A RID: 8970
		// (get) Token: 0x06006B14 RID: 27412 RVA: 0x0003AD32 File Offset: 0x00038F32
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton10
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(69);
			}
		}

		// Token: 0x1700230B RID: 8971
		// (get) Token: 0x06006B15 RID: 27413 RVA: 0x0003AD3C File Offset: 0x00038F3C
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton11
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(132);
			}
		}

		// Token: 0x1700230C RID: 8972
		// (get) Token: 0x06006B16 RID: 27414 RVA: 0x0003AD49 File Offset: 0x00038F49
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton12
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(133);
			}
		}

		// Token: 0x1700230D RID: 8973
		// (get) Token: 0x06006B17 RID: 27415 RVA: 0x0003AD56 File Offset: 0x00038F56
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton13
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(134);
			}
		}

		// Token: 0x1700230E RID: 8974
		// (get) Token: 0x06006B18 RID: 27416 RVA: 0x0003AD63 File Offset: 0x00038F63
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton14
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(135);
			}
		}

		// Token: 0x1700230F RID: 8975
		// (get) Token: 0x06006B19 RID: 27417 RVA: 0x0003AD70 File Offset: 0x00038F70
		IControllerTemplateButton IHOTASTemplate.throttleBaseButton15
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(136);
			}
		}

		// Token: 0x17002310 RID: 8976
		// (get) Token: 0x06006B1A RID: 27418 RVA: 0x0003AD7D File Offset: 0x00038F7D
		IControllerTemplateAxis IHOTASTemplate.throttleSlider1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(70);
			}
		}

		// Token: 0x17002311 RID: 8977
		// (get) Token: 0x06006B1B RID: 27419 RVA: 0x0003AD87 File Offset: 0x00038F87
		IControllerTemplateAxis IHOTASTemplate.throttleSlider2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(71);
			}
		}

		// Token: 0x17002312 RID: 8978
		// (get) Token: 0x06006B1C RID: 27420 RVA: 0x0003AD91 File Offset: 0x00038F91
		IControllerTemplateAxis IHOTASTemplate.throttleSlider3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(72);
			}
		}

		// Token: 0x17002313 RID: 8979
		// (get) Token: 0x06006B1D RID: 27421 RVA: 0x0003AD9B File Offset: 0x00038F9B
		IControllerTemplateAxis IHOTASTemplate.throttleSlider4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(73);
			}
		}

		// Token: 0x17002314 RID: 8980
		// (get) Token: 0x06006B1E RID: 27422 RVA: 0x0003ADA5 File Offset: 0x00038FA5
		IControllerTemplateAxis IHOTASTemplate.throttleDial1
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(74);
			}
		}

		// Token: 0x17002315 RID: 8981
		// (get) Token: 0x06006B1F RID: 27423 RVA: 0x0003ADAF File Offset: 0x00038FAF
		IControllerTemplateAxis IHOTASTemplate.throttleDial2
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(142);
			}
		}

		// Token: 0x17002316 RID: 8982
		// (get) Token: 0x06006B20 RID: 27424 RVA: 0x0003ADBC File Offset: 0x00038FBC
		IControllerTemplateAxis IHOTASTemplate.throttleDial3
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(143);
			}
		}

		// Token: 0x17002317 RID: 8983
		// (get) Token: 0x06006B21 RID: 27425 RVA: 0x0003ADC9 File Offset: 0x00038FC9
		IControllerTemplateAxis IHOTASTemplate.throttleDial4
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(144);
			}
		}

		// Token: 0x17002318 RID: 8984
		// (get) Token: 0x06006B22 RID: 27426 RVA: 0x0003ADD6 File Offset: 0x00038FD6
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(145);
			}
		}

		// Token: 0x17002319 RID: 8985
		// (get) Token: 0x06006B23 RID: 27427 RVA: 0x0003ADE3 File Offset: 0x00038FE3
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(146);
			}
		}

		// Token: 0x1700231A RID: 8986
		// (get) Token: 0x06006B24 RID: 27428 RVA: 0x0003ADF0 File Offset: 0x00038FF0
		IControllerTemplateButton IHOTASTemplate.throttleWheel1Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(147);
			}
		}

		// Token: 0x1700231B RID: 8987
		// (get) Token: 0x06006B25 RID: 27429 RVA: 0x0003ADFD File Offset: 0x00038FFD
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(148);
			}
		}

		// Token: 0x1700231C RID: 8988
		// (get) Token: 0x06006B26 RID: 27430 RVA: 0x0003AE0A File Offset: 0x0003900A
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(149);
			}
		}

		// Token: 0x1700231D RID: 8989
		// (get) Token: 0x06006B27 RID: 27431 RVA: 0x0003AE17 File Offset: 0x00039017
		IControllerTemplateButton IHOTASTemplate.throttleWheel2Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(150);
			}
		}

		// Token: 0x1700231E RID: 8990
		// (get) Token: 0x06006B28 RID: 27432 RVA: 0x0003AE24 File Offset: 0x00039024
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Forward
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(151);
			}
		}

		// Token: 0x1700231F RID: 8991
		// (get) Token: 0x06006B29 RID: 27433 RVA: 0x0003AE31 File Offset: 0x00039031
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Back
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(152);
			}
		}

		// Token: 0x17002320 RID: 8992
		// (get) Token: 0x06006B2A RID: 27434 RVA: 0x0003AE3E File Offset: 0x0003903E
		IControllerTemplateButton IHOTASTemplate.throttleWheel3Press
		{
			get
			{
				return base.GetElement<IControllerTemplateButton>(153);
			}
		}

		// Token: 0x17002321 RID: 8993
		// (get) Token: 0x06006B2B RID: 27435 RVA: 0x0003AE4B File Offset: 0x0003904B
		IControllerTemplateAxis IHOTASTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(168);
			}
		}

		// Token: 0x17002322 RID: 8994
		// (get) Token: 0x06006B2C RID: 27436 RVA: 0x0003AE58 File Offset: 0x00039058
		IControllerTemplateAxis IHOTASTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(169);
			}
		}

		// Token: 0x17002323 RID: 8995
		// (get) Token: 0x06006B2D RID: 27437 RVA: 0x0003AE65 File Offset: 0x00039065
		IControllerTemplateAxis IHOTASTemplate.slidePedals
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(170);
			}
		}

		// Token: 0x17002324 RID: 8996
		// (get) Token: 0x06006B2E RID: 27438 RVA: 0x0003AE72 File Offset: 0x00039072
		IControllerTemplateStick IHOTASTemplate.stick
		{
			get
			{
				return base.GetElement<IControllerTemplateStick>(171);
			}
		}

		// Token: 0x17002325 RID: 8997
		// (get) Token: 0x06006B2F RID: 27439 RVA: 0x0003AE7F File Offset: 0x0003907F
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick1
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(172);
			}
		}

		// Token: 0x17002326 RID: 8998
		// (get) Token: 0x06006B30 RID: 27440 RVA: 0x0003AE8C File Offset: 0x0003908C
		IControllerTemplateThumbStick IHOTASTemplate.stickMiniStick2
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(173);
			}
		}

		// Token: 0x17002327 RID: 8999
		// (get) Token: 0x06006B31 RID: 27441 RVA: 0x0003AE99 File Offset: 0x00039099
		IControllerTemplateHat IHOTASTemplate.stickHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(174);
			}
		}

		// Token: 0x17002328 RID: 9000
		// (get) Token: 0x06006B32 RID: 27442 RVA: 0x0003AEA6 File Offset: 0x000390A6
		IControllerTemplateHat IHOTASTemplate.stickHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(175);
			}
		}

		// Token: 0x17002329 RID: 9001
		// (get) Token: 0x06006B33 RID: 27443 RVA: 0x0003AEB3 File Offset: 0x000390B3
		IControllerTemplateHat IHOTASTemplate.stickHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(176);
			}
		}

		// Token: 0x1700232A RID: 9002
		// (get) Token: 0x06006B34 RID: 27444 RVA: 0x0003AEC0 File Offset: 0x000390C0
		IControllerTemplateHat IHOTASTemplate.stickHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(177);
			}
		}

		// Token: 0x1700232B RID: 9003
		// (get) Token: 0x06006B35 RID: 27445 RVA: 0x0003AECD File Offset: 0x000390CD
		IControllerTemplateThrottle IHOTASTemplate.throttle1
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(178);
			}
		}

		// Token: 0x1700232C RID: 9004
		// (get) Token: 0x06006B36 RID: 27446 RVA: 0x0003AEDA File Offset: 0x000390DA
		IControllerTemplateThrottle IHOTASTemplate.throttle2
		{
			get
			{
				return base.GetElement<IControllerTemplateThrottle>(179);
			}
		}

		// Token: 0x1700232D RID: 9005
		// (get) Token: 0x06006B37 RID: 27447 RVA: 0x0003AEE7 File Offset: 0x000390E7
		IControllerTemplateThumbStick IHOTASTemplate.throttleMiniStick
		{
			get
			{
				return base.GetElement<IControllerTemplateThumbStick>(180);
			}
		}

		// Token: 0x1700232E RID: 9006
		// (get) Token: 0x06006B38 RID: 27448 RVA: 0x0003AEF4 File Offset: 0x000390F4
		IControllerTemplateHat IHOTASTemplate.throttleHat1
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(181);
			}
		}

		// Token: 0x1700232F RID: 9007
		// (get) Token: 0x06006B39 RID: 27449 RVA: 0x0003AF01 File Offset: 0x00039101
		IControllerTemplateHat IHOTASTemplate.throttleHat2
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(182);
			}
		}

		// Token: 0x17002330 RID: 9008
		// (get) Token: 0x06006B3A RID: 27450 RVA: 0x0003AF0E File Offset: 0x0003910E
		IControllerTemplateHat IHOTASTemplate.throttleHat3
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(183);
			}
		}

		// Token: 0x17002331 RID: 9009
		// (get) Token: 0x06006B3B RID: 27451 RVA: 0x0003AF1B File Offset: 0x0003911B
		IControllerTemplateHat IHOTASTemplate.throttleHat4
		{
			get
			{
				return base.GetElement<IControllerTemplateHat>(184);
			}
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public HOTASTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04005642 RID: 22082
		public static readonly Guid typeGuid = new Guid("061a00cf-d8c2-4f8d-8cb5-a15a010bc53e");

		// Token: 0x04005643 RID: 22083
		public const int elementId_stickX = 0;

		// Token: 0x04005644 RID: 22084
		public const int elementId_stickY = 1;

		// Token: 0x04005645 RID: 22085
		public const int elementId_stickRotate = 2;

		// Token: 0x04005646 RID: 22086
		public const int elementId_stickMiniStick1X = 78;

		// Token: 0x04005647 RID: 22087
		public const int elementId_stickMiniStick1Y = 79;

		// Token: 0x04005648 RID: 22088
		public const int elementId_stickMiniStick1Press = 80;

		// Token: 0x04005649 RID: 22089
		public const int elementId_stickMiniStick2X = 81;

		// Token: 0x0400564A RID: 22090
		public const int elementId_stickMiniStick2Y = 82;

		// Token: 0x0400564B RID: 22091
		public const int elementId_stickMiniStick2Press = 83;

		// Token: 0x0400564C RID: 22092
		public const int elementId_stickTrigger = 3;

		// Token: 0x0400564D RID: 22093
		public const int elementId_stickTriggerStage2 = 4;

		// Token: 0x0400564E RID: 22094
		public const int elementId_stickPinkyButton = 5;

		// Token: 0x0400564F RID: 22095
		public const int elementId_stickPinkyTrigger = 154;

		// Token: 0x04005650 RID: 22096
		public const int elementId_stickButton1 = 6;

		// Token: 0x04005651 RID: 22097
		public const int elementId_stickButton2 = 7;

		// Token: 0x04005652 RID: 22098
		public const int elementId_stickButton3 = 8;

		// Token: 0x04005653 RID: 22099
		public const int elementId_stickButton4 = 9;

		// Token: 0x04005654 RID: 22100
		public const int elementId_stickButton5 = 10;

		// Token: 0x04005655 RID: 22101
		public const int elementId_stickButton6 = 11;

		// Token: 0x04005656 RID: 22102
		public const int elementId_stickButton7 = 12;

		// Token: 0x04005657 RID: 22103
		public const int elementId_stickButton8 = 13;

		// Token: 0x04005658 RID: 22104
		public const int elementId_stickButton9 = 14;

		// Token: 0x04005659 RID: 22105
		public const int elementId_stickButton10 = 15;

		// Token: 0x0400565A RID: 22106
		public const int elementId_stickBaseButton1 = 18;

		// Token: 0x0400565B RID: 22107
		public const int elementId_stickBaseButton2 = 19;

		// Token: 0x0400565C RID: 22108
		public const int elementId_stickBaseButton3 = 20;

		// Token: 0x0400565D RID: 22109
		public const int elementId_stickBaseButton4 = 21;

		// Token: 0x0400565E RID: 22110
		public const int elementId_stickBaseButton5 = 22;

		// Token: 0x0400565F RID: 22111
		public const int elementId_stickBaseButton6 = 23;

		// Token: 0x04005660 RID: 22112
		public const int elementId_stickBaseButton7 = 24;

		// Token: 0x04005661 RID: 22113
		public const int elementId_stickBaseButton8 = 25;

		// Token: 0x04005662 RID: 22114
		public const int elementId_stickBaseButton9 = 26;

		// Token: 0x04005663 RID: 22115
		public const int elementId_stickBaseButton10 = 27;

		// Token: 0x04005664 RID: 22116
		public const int elementId_stickBaseButton11 = 161;

		// Token: 0x04005665 RID: 22117
		public const int elementId_stickBaseButton12 = 162;

		// Token: 0x04005666 RID: 22118
		public const int elementId_stickHat1Up = 28;

		// Token: 0x04005667 RID: 22119
		public const int elementId_stickHat1UpRight = 29;

		// Token: 0x04005668 RID: 22120
		public const int elementId_stickHat1Right = 30;

		// Token: 0x04005669 RID: 22121
		public const int elementId_stickHat1DownRight = 31;

		// Token: 0x0400566A RID: 22122
		public const int elementId_stickHat1Down = 32;

		// Token: 0x0400566B RID: 22123
		public const int elementId_stickHat1DownLeft = 33;

		// Token: 0x0400566C RID: 22124
		public const int elementId_stickHat1Left = 34;

		// Token: 0x0400566D RID: 22125
		public const int elementId_stickHat1Up_Left = 35;

		// Token: 0x0400566E RID: 22126
		public const int elementId_stickHat2Up = 36;

		// Token: 0x0400566F RID: 22127
		public const int elementId_stickHat2Up_right = 37;

		// Token: 0x04005670 RID: 22128
		public const int elementId_stickHat2Right = 38;

		// Token: 0x04005671 RID: 22129
		public const int elementId_stickHat2Down_Right = 39;

		// Token: 0x04005672 RID: 22130
		public const int elementId_stickHat2Down = 40;

		// Token: 0x04005673 RID: 22131
		public const int elementId_stickHat2Down_Left = 41;

		// Token: 0x04005674 RID: 22132
		public const int elementId_stickHat2Left = 42;

		// Token: 0x04005675 RID: 22133
		public const int elementId_stickHat2Up_Left = 43;

		// Token: 0x04005676 RID: 22134
		public const int elementId_stickHat3Up = 84;

		// Token: 0x04005677 RID: 22135
		public const int elementId_stickHat3Up_Right = 85;

		// Token: 0x04005678 RID: 22136
		public const int elementId_stickHat3Right = 86;

		// Token: 0x04005679 RID: 22137
		public const int elementId_stickHat3Down_Right = 87;

		// Token: 0x0400567A RID: 22138
		public const int elementId_stickHat3Down = 88;

		// Token: 0x0400567B RID: 22139
		public const int elementId_stickHat3Down_Left = 89;

		// Token: 0x0400567C RID: 22140
		public const int elementId_stickHat3Left = 90;

		// Token: 0x0400567D RID: 22141
		public const int elementId_stickHat3Up_Left = 91;

		// Token: 0x0400567E RID: 22142
		public const int elementId_stickHat4Up = 92;

		// Token: 0x0400567F RID: 22143
		public const int elementId_stickHat4Up_Right = 93;

		// Token: 0x04005680 RID: 22144
		public const int elementId_stickHat4Right = 94;

		// Token: 0x04005681 RID: 22145
		public const int elementId_stickHat4Down_Right = 95;

		// Token: 0x04005682 RID: 22146
		public const int elementId_stickHat4Down = 96;

		// Token: 0x04005683 RID: 22147
		public const int elementId_stickHat4Down_Left = 97;

		// Token: 0x04005684 RID: 22148
		public const int elementId_stickHat4Left = 98;

		// Token: 0x04005685 RID: 22149
		public const int elementId_stickHat4Up_Left = 99;

		// Token: 0x04005686 RID: 22150
		public const int elementId_mode1 = 44;

		// Token: 0x04005687 RID: 22151
		public const int elementId_mode2 = 45;

		// Token: 0x04005688 RID: 22152
		public const int elementId_mode3 = 46;

		// Token: 0x04005689 RID: 22153
		public const int elementId_throttle1Axis = 49;

		// Token: 0x0400568A RID: 22154
		public const int elementId_throttle2Axis = 155;

		// Token: 0x0400568B RID: 22155
		public const int elementId_throttle1MinDetent = 166;

		// Token: 0x0400568C RID: 22156
		public const int elementId_throttle2MinDetent = 167;

		// Token: 0x0400568D RID: 22157
		public const int elementId_throttleButton1 = 50;

		// Token: 0x0400568E RID: 22158
		public const int elementId_throttleButton2 = 51;

		// Token: 0x0400568F RID: 22159
		public const int elementId_throttleButton3 = 52;

		// Token: 0x04005690 RID: 22160
		public const int elementId_throttleButton4 = 53;

		// Token: 0x04005691 RID: 22161
		public const int elementId_throttleButton5 = 54;

		// Token: 0x04005692 RID: 22162
		public const int elementId_throttleButton6 = 55;

		// Token: 0x04005693 RID: 22163
		public const int elementId_throttleButton7 = 56;

		// Token: 0x04005694 RID: 22164
		public const int elementId_throttleButton8 = 57;

		// Token: 0x04005695 RID: 22165
		public const int elementId_throttleButton9 = 58;

		// Token: 0x04005696 RID: 22166
		public const int elementId_throttleButton10 = 59;

		// Token: 0x04005697 RID: 22167
		public const int elementId_throttleBaseButton1 = 60;

		// Token: 0x04005698 RID: 22168
		public const int elementId_throttleBaseButton2 = 61;

		// Token: 0x04005699 RID: 22169
		public const int elementId_throttleBaseButton3 = 62;

		// Token: 0x0400569A RID: 22170
		public const int elementId_throttleBaseButton4 = 63;

		// Token: 0x0400569B RID: 22171
		public const int elementId_throttleBaseButton5 = 64;

		// Token: 0x0400569C RID: 22172
		public const int elementId_throttleBaseButton6 = 65;

		// Token: 0x0400569D RID: 22173
		public const int elementId_throttleBaseButton7 = 66;

		// Token: 0x0400569E RID: 22174
		public const int elementId_throttleBaseButton8 = 67;

		// Token: 0x0400569F RID: 22175
		public const int elementId_throttleBaseButton9 = 68;

		// Token: 0x040056A0 RID: 22176
		public const int elementId_throttleBaseButton10 = 69;

		// Token: 0x040056A1 RID: 22177
		public const int elementId_throttleBaseButton11 = 132;

		// Token: 0x040056A2 RID: 22178
		public const int elementId_throttleBaseButton12 = 133;

		// Token: 0x040056A3 RID: 22179
		public const int elementId_throttleBaseButton13 = 134;

		// Token: 0x040056A4 RID: 22180
		public const int elementId_throttleBaseButton14 = 135;

		// Token: 0x040056A5 RID: 22181
		public const int elementId_throttleBaseButton15 = 136;

		// Token: 0x040056A6 RID: 22182
		public const int elementId_throttleSlider1 = 70;

		// Token: 0x040056A7 RID: 22183
		public const int elementId_throttleSlider2 = 71;

		// Token: 0x040056A8 RID: 22184
		public const int elementId_throttleSlider3 = 72;

		// Token: 0x040056A9 RID: 22185
		public const int elementId_throttleSlider4 = 73;

		// Token: 0x040056AA RID: 22186
		public const int elementId_throttleDial1 = 74;

		// Token: 0x040056AB RID: 22187
		public const int elementId_throttleDial2 = 142;

		// Token: 0x040056AC RID: 22188
		public const int elementId_throttleDial3 = 143;

		// Token: 0x040056AD RID: 22189
		public const int elementId_throttleDial4 = 144;

		// Token: 0x040056AE RID: 22190
		public const int elementId_throttleMiniStickX = 75;

		// Token: 0x040056AF RID: 22191
		public const int elementId_throttleMiniStickY = 76;

		// Token: 0x040056B0 RID: 22192
		public const int elementId_throttleMiniStickPress = 77;

		// Token: 0x040056B1 RID: 22193
		public const int elementId_throttleWheel1Forward = 145;

		// Token: 0x040056B2 RID: 22194
		public const int elementId_throttleWheel1Back = 146;

		// Token: 0x040056B3 RID: 22195
		public const int elementId_throttleWheel1Press = 147;

		// Token: 0x040056B4 RID: 22196
		public const int elementId_throttleWheel2Forward = 148;

		// Token: 0x040056B5 RID: 22197
		public const int elementId_throttleWheel2Back = 149;

		// Token: 0x040056B6 RID: 22198
		public const int elementId_throttleWheel2Press = 150;

		// Token: 0x040056B7 RID: 22199
		public const int elementId_throttleWheel3Forward = 151;

		// Token: 0x040056B8 RID: 22200
		public const int elementId_throttleWheel3Back = 152;

		// Token: 0x040056B9 RID: 22201
		public const int elementId_throttleWheel3Press = 153;

		// Token: 0x040056BA RID: 22202
		public const int elementId_throttleHat1Up = 100;

		// Token: 0x040056BB RID: 22203
		public const int elementId_throttleHat1Up_Right = 101;

		// Token: 0x040056BC RID: 22204
		public const int elementId_throttleHat1Right = 102;

		// Token: 0x040056BD RID: 22205
		public const int elementId_throttleHat1Down_Right = 103;

		// Token: 0x040056BE RID: 22206
		public const int elementId_throttleHat1Down = 104;

		// Token: 0x040056BF RID: 22207
		public const int elementId_throttleHat1Down_Left = 105;

		// Token: 0x040056C0 RID: 22208
		public const int elementId_throttleHat1Left = 106;

		// Token: 0x040056C1 RID: 22209
		public const int elementId_throttleHat1Up_Left = 107;

		// Token: 0x040056C2 RID: 22210
		public const int elementId_throttleHat2Up = 108;

		// Token: 0x040056C3 RID: 22211
		public const int elementId_throttleHat2Up_Right = 109;

		// Token: 0x040056C4 RID: 22212
		public const int elementId_throttleHat2Right = 110;

		// Token: 0x040056C5 RID: 22213
		public const int elementId_throttleHat2Down_Right = 111;

		// Token: 0x040056C6 RID: 22214
		public const int elementId_throttleHat2Down = 112;

		// Token: 0x040056C7 RID: 22215
		public const int elementId_throttleHat2Down_Left = 113;

		// Token: 0x040056C8 RID: 22216
		public const int elementId_throttleHat2Left = 114;

		// Token: 0x040056C9 RID: 22217
		public const int elementId_throttleHat2Up_Left = 115;

		// Token: 0x040056CA RID: 22218
		public const int elementId_throttleHat3Up = 116;

		// Token: 0x040056CB RID: 22219
		public const int elementId_throttleHat3Up_Right = 117;

		// Token: 0x040056CC RID: 22220
		public const int elementId_throttleHat3Right = 118;

		// Token: 0x040056CD RID: 22221
		public const int elementId_throttleHat3Down_Right = 119;

		// Token: 0x040056CE RID: 22222
		public const int elementId_throttleHat3Down = 120;

		// Token: 0x040056CF RID: 22223
		public const int elementId_throttleHat3Down_Left = 121;

		// Token: 0x040056D0 RID: 22224
		public const int elementId_throttleHat3Left = 122;

		// Token: 0x040056D1 RID: 22225
		public const int elementId_throttleHat3Up_Left = 123;

		// Token: 0x040056D2 RID: 22226
		public const int elementId_throttleHat4Up = 124;

		// Token: 0x040056D3 RID: 22227
		public const int elementId_throttleHat4Up_Right = 125;

		// Token: 0x040056D4 RID: 22228
		public const int elementId_throttleHat4Right = 126;

		// Token: 0x040056D5 RID: 22229
		public const int elementId_throttleHat4Down_Right = 127;

		// Token: 0x040056D6 RID: 22230
		public const int elementId_throttleHat4Down = 128;

		// Token: 0x040056D7 RID: 22231
		public const int elementId_throttleHat4Down_Left = 129;

		// Token: 0x040056D8 RID: 22232
		public const int elementId_throttleHat4Left = 130;

		// Token: 0x040056D9 RID: 22233
		public const int elementId_throttleHat4Up_Left = 131;

		// Token: 0x040056DA RID: 22234
		public const int elementId_leftPedal = 168;

		// Token: 0x040056DB RID: 22235
		public const int elementId_rightPedal = 169;

		// Token: 0x040056DC RID: 22236
		public const int elementId_slidePedals = 170;

		// Token: 0x040056DD RID: 22237
		public const int elementId_stick = 171;

		// Token: 0x040056DE RID: 22238
		public const int elementId_stickMiniStick1 = 172;

		// Token: 0x040056DF RID: 22239
		public const int elementId_stickMiniStick2 = 173;

		// Token: 0x040056E0 RID: 22240
		public const int elementId_stickHat1 = 174;

		// Token: 0x040056E1 RID: 22241
		public const int elementId_stickHat2 = 175;

		// Token: 0x040056E2 RID: 22242
		public const int elementId_stickHat3 = 176;

		// Token: 0x040056E3 RID: 22243
		public const int elementId_stickHat4 = 177;

		// Token: 0x040056E4 RID: 22244
		public const int elementId_throttle1 = 178;

		// Token: 0x040056E5 RID: 22245
		public const int elementId_throttle2 = 179;

		// Token: 0x040056E6 RID: 22246
		public const int elementId_throttleMiniStick = 180;

		// Token: 0x040056E7 RID: 22247
		public const int elementId_throttleHat1 = 181;

		// Token: 0x040056E8 RID: 22248
		public const int elementId_throttleHat2 = 182;

		// Token: 0x040056E9 RID: 22249
		public const int elementId_throttleHat3 = 183;

		// Token: 0x040056EA RID: 22250
		public const int elementId_throttleHat4 = 184;
	}
}
