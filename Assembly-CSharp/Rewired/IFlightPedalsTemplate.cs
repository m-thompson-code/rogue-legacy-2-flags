using System;

namespace Rewired
{
	// Token: 0x02000928 RID: 2344
	public interface IFlightPedalsTemplate : IControllerTemplate
	{
		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x06004D66 RID: 19814
		IControllerTemplateAxis leftPedal { get; }

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x06004D67 RID: 19815
		IControllerTemplateAxis rightPedal { get; }

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x06004D68 RID: 19816
		IControllerTemplateAxis slide { get; }
	}
}
