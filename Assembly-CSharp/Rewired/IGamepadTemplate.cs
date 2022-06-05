using System;

namespace Rewired
{
	// Token: 0x02000924 RID: 2340
	public interface IGamepadTemplate : IControllerTemplate
	{
		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06004C96 RID: 19606
		IControllerTemplateButton actionBottomRow1 { get; }

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06004C97 RID: 19607
		IControllerTemplateButton a { get; }

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06004C98 RID: 19608
		IControllerTemplateButton actionBottomRow2 { get; }

		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06004C99 RID: 19609
		IControllerTemplateButton b { get; }

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06004C9A RID: 19610
		IControllerTemplateButton actionBottomRow3 { get; }

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06004C9B RID: 19611
		IControllerTemplateButton c { get; }

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06004C9C RID: 19612
		IControllerTemplateButton actionTopRow1 { get; }

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06004C9D RID: 19613
		IControllerTemplateButton x { get; }

		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06004C9E RID: 19614
		IControllerTemplateButton actionTopRow2 { get; }

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06004C9F RID: 19615
		IControllerTemplateButton y { get; }

		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x06004CA0 RID: 19616
		IControllerTemplateButton actionTopRow3 { get; }

		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06004CA1 RID: 19617
		IControllerTemplateButton z { get; }

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06004CA2 RID: 19618
		IControllerTemplateButton leftShoulder1 { get; }

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06004CA3 RID: 19619
		IControllerTemplateButton leftBumper { get; }

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06004CA4 RID: 19620
		IControllerTemplateAxis leftShoulder2 { get; }

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06004CA5 RID: 19621
		IControllerTemplateAxis leftTrigger { get; }

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06004CA6 RID: 19622
		IControllerTemplateButton rightShoulder1 { get; }

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06004CA7 RID: 19623
		IControllerTemplateButton rightBumper { get; }

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06004CA8 RID: 19624
		IControllerTemplateAxis rightShoulder2 { get; }

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x06004CA9 RID: 19625
		IControllerTemplateAxis rightTrigger { get; }

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06004CAA RID: 19626
		IControllerTemplateButton center1 { get; }

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06004CAB RID: 19627
		IControllerTemplateButton back { get; }

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x06004CAC RID: 19628
		IControllerTemplateButton center2 { get; }

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x06004CAD RID: 19629
		IControllerTemplateButton start { get; }

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x06004CAE RID: 19630
		IControllerTemplateButton center3 { get; }

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x06004CAF RID: 19631
		IControllerTemplateButton guide { get; }

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x06004CB0 RID: 19632
		IControllerTemplateThumbStick leftStick { get; }

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x06004CB1 RID: 19633
		IControllerTemplateThumbStick rightStick { get; }

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06004CB2 RID: 19634
		IControllerTemplateDPad dPad { get; }
	}
}
