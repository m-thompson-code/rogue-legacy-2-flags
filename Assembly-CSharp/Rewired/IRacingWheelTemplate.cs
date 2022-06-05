using System;

namespace Rewired
{
	// Token: 0x02000925 RID: 2341
	public interface IRacingWheelTemplate : IControllerTemplate
	{
		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06004CB3 RID: 19635
		IControllerTemplateAxis wheel { get; }

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x06004CB4 RID: 19636
		IControllerTemplateAxis accelerator { get; }

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x06004CB5 RID: 19637
		IControllerTemplateAxis brake { get; }

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x06004CB6 RID: 19638
		IControllerTemplateAxis clutch { get; }

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x06004CB7 RID: 19639
		IControllerTemplateButton shiftDown { get; }

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x06004CB8 RID: 19640
		IControllerTemplateButton shiftUp { get; }

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x06004CB9 RID: 19641
		IControllerTemplateButton wheelButton1 { get; }

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x06004CBA RID: 19642
		IControllerTemplateButton wheelButton2 { get; }

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x06004CBB RID: 19643
		IControllerTemplateButton wheelButton3 { get; }

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x06004CBC RID: 19644
		IControllerTemplateButton wheelButton4 { get; }

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x06004CBD RID: 19645
		IControllerTemplateButton wheelButton5 { get; }

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x06004CBE RID: 19646
		IControllerTemplateButton wheelButton6 { get; }

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x06004CBF RID: 19647
		IControllerTemplateButton wheelButton7 { get; }

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x06004CC0 RID: 19648
		IControllerTemplateButton wheelButton8 { get; }

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x06004CC1 RID: 19649
		IControllerTemplateButton wheelButton9 { get; }

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x06004CC2 RID: 19650
		IControllerTemplateButton wheelButton10 { get; }

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x06004CC3 RID: 19651
		IControllerTemplateButton consoleButton1 { get; }

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x06004CC4 RID: 19652
		IControllerTemplateButton consoleButton2 { get; }

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x06004CC5 RID: 19653
		IControllerTemplateButton consoleButton3 { get; }

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x06004CC6 RID: 19654
		IControllerTemplateButton consoleButton4 { get; }

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x06004CC7 RID: 19655
		IControllerTemplateButton consoleButton5 { get; }

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x06004CC8 RID: 19656
		IControllerTemplateButton consoleButton6 { get; }

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x06004CC9 RID: 19657
		IControllerTemplateButton consoleButton7 { get; }

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x06004CCA RID: 19658
		IControllerTemplateButton consoleButton8 { get; }

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x06004CCB RID: 19659
		IControllerTemplateButton consoleButton9 { get; }

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x06004CCC RID: 19660
		IControllerTemplateButton consoleButton10 { get; }

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x06004CCD RID: 19661
		IControllerTemplateButton shifter1 { get; }

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x06004CCE RID: 19662
		IControllerTemplateButton shifter2 { get; }

		// Token: 0x170018D3 RID: 6355
		// (get) Token: 0x06004CCF RID: 19663
		IControllerTemplateButton shifter3 { get; }

		// Token: 0x170018D4 RID: 6356
		// (get) Token: 0x06004CD0 RID: 19664
		IControllerTemplateButton shifter4 { get; }

		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x06004CD1 RID: 19665
		IControllerTemplateButton shifter5 { get; }

		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x06004CD2 RID: 19666
		IControllerTemplateButton shifter6 { get; }

		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x06004CD3 RID: 19667
		IControllerTemplateButton shifter7 { get; }

		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x06004CD4 RID: 19668
		IControllerTemplateButton shifter8 { get; }

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x06004CD5 RID: 19669
		IControllerTemplateButton shifter9 { get; }

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x06004CD6 RID: 19670
		IControllerTemplateButton shifter10 { get; }

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x06004CD7 RID: 19671
		IControllerTemplateButton reverseGear { get; }

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x06004CD8 RID: 19672
		IControllerTemplateButton select { get; }

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x06004CD9 RID: 19673
		IControllerTemplateButton start { get; }

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x06004CDA RID: 19674
		IControllerTemplateButton systemButton { get; }

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x06004CDB RID: 19675
		IControllerTemplateButton horn { get; }

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06004CDC RID: 19676
		IControllerTemplateDPad dPad { get; }
	}
}
