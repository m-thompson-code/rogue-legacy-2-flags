using System;

namespace Rewired
{
	// Token: 0x02000EA3 RID: 3747
	public interface IGamepadTemplate : IControllerTemplate
	{
		// Token: 0x17002197 RID: 8599
		// (get) Token: 0x0600699D RID: 27037
		IControllerTemplateButton actionBottomRow1 { get; }

		// Token: 0x17002198 RID: 8600
		// (get) Token: 0x0600699E RID: 27038
		IControllerTemplateButton a { get; }

		// Token: 0x17002199 RID: 8601
		// (get) Token: 0x0600699F RID: 27039
		IControllerTemplateButton actionBottomRow2 { get; }

		// Token: 0x1700219A RID: 8602
		// (get) Token: 0x060069A0 RID: 27040
		IControllerTemplateButton b { get; }

		// Token: 0x1700219B RID: 8603
		// (get) Token: 0x060069A1 RID: 27041
		IControllerTemplateButton actionBottomRow3 { get; }

		// Token: 0x1700219C RID: 8604
		// (get) Token: 0x060069A2 RID: 27042
		IControllerTemplateButton c { get; }

		// Token: 0x1700219D RID: 8605
		// (get) Token: 0x060069A3 RID: 27043
		IControllerTemplateButton actionTopRow1 { get; }

		// Token: 0x1700219E RID: 8606
		// (get) Token: 0x060069A4 RID: 27044
		IControllerTemplateButton x { get; }

		// Token: 0x1700219F RID: 8607
		// (get) Token: 0x060069A5 RID: 27045
		IControllerTemplateButton actionTopRow2 { get; }

		// Token: 0x170021A0 RID: 8608
		// (get) Token: 0x060069A6 RID: 27046
		IControllerTemplateButton y { get; }

		// Token: 0x170021A1 RID: 8609
		// (get) Token: 0x060069A7 RID: 27047
		IControllerTemplateButton actionTopRow3 { get; }

		// Token: 0x170021A2 RID: 8610
		// (get) Token: 0x060069A8 RID: 27048
		IControllerTemplateButton z { get; }

		// Token: 0x170021A3 RID: 8611
		// (get) Token: 0x060069A9 RID: 27049
		IControllerTemplateButton leftShoulder1 { get; }

		// Token: 0x170021A4 RID: 8612
		// (get) Token: 0x060069AA RID: 27050
		IControllerTemplateButton leftBumper { get; }

		// Token: 0x170021A5 RID: 8613
		// (get) Token: 0x060069AB RID: 27051
		IControllerTemplateAxis leftShoulder2 { get; }

		// Token: 0x170021A6 RID: 8614
		// (get) Token: 0x060069AC RID: 27052
		IControllerTemplateAxis leftTrigger { get; }

		// Token: 0x170021A7 RID: 8615
		// (get) Token: 0x060069AD RID: 27053
		IControllerTemplateButton rightShoulder1 { get; }

		// Token: 0x170021A8 RID: 8616
		// (get) Token: 0x060069AE RID: 27054
		IControllerTemplateButton rightBumper { get; }

		// Token: 0x170021A9 RID: 8617
		// (get) Token: 0x060069AF RID: 27055
		IControllerTemplateAxis rightShoulder2 { get; }

		// Token: 0x170021AA RID: 8618
		// (get) Token: 0x060069B0 RID: 27056
		IControllerTemplateAxis rightTrigger { get; }

		// Token: 0x170021AB RID: 8619
		// (get) Token: 0x060069B1 RID: 27057
		IControllerTemplateButton center1 { get; }

		// Token: 0x170021AC RID: 8620
		// (get) Token: 0x060069B2 RID: 27058
		IControllerTemplateButton back { get; }

		// Token: 0x170021AD RID: 8621
		// (get) Token: 0x060069B3 RID: 27059
		IControllerTemplateButton center2 { get; }

		// Token: 0x170021AE RID: 8622
		// (get) Token: 0x060069B4 RID: 27060
		IControllerTemplateButton start { get; }

		// Token: 0x170021AF RID: 8623
		// (get) Token: 0x060069B5 RID: 27061
		IControllerTemplateButton center3 { get; }

		// Token: 0x170021B0 RID: 8624
		// (get) Token: 0x060069B6 RID: 27062
		IControllerTemplateButton guide { get; }

		// Token: 0x170021B1 RID: 8625
		// (get) Token: 0x060069B7 RID: 27063
		IControllerTemplateThumbStick leftStick { get; }

		// Token: 0x170021B2 RID: 8626
		// (get) Token: 0x060069B8 RID: 27064
		IControllerTemplateThumbStick rightStick { get; }

		// Token: 0x170021B3 RID: 8627
		// (get) Token: 0x060069B9 RID: 27065
		IControllerTemplateDPad dPad { get; }
	}
}
