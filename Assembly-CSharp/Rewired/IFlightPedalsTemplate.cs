using System;

namespace Rewired
{
	// Token: 0x02000EA7 RID: 3751
	public interface IFlightPedalsTemplate : IControllerTemplate
	{
		// Token: 0x17002267 RID: 8807
		// (get) Token: 0x06006A6D RID: 27245
		IControllerTemplateAxis leftPedal { get; }

		// Token: 0x17002268 RID: 8808
		// (get) Token: 0x06006A6E RID: 27246
		IControllerTemplateAxis rightPedal { get; }

		// Token: 0x17002269 RID: 8809
		// (get) Token: 0x06006A6F RID: 27247
		IControllerTemplateAxis slide { get; }
	}
}
