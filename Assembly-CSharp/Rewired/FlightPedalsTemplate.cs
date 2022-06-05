using System;

namespace Rewired
{
	// Token: 0x02000EAD RID: 3757
	public sealed class FlightPedalsTemplate : ControllerTemplate, IFlightPedalsTemplate, IControllerTemplate
	{
		// Token: 0x17002363 RID: 9059
		// (get) Token: 0x06006B71 RID: 27505 RVA: 0x0003AAED File Offset: 0x00038CED
		IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x17002364 RID: 9060
		// (get) Token: 0x06006B72 RID: 27506 RVA: 0x0003AAF6 File Offset: 0x00038CF6
		IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x17002365 RID: 9061
		// (get) Token: 0x06006B73 RID: 27507 RVA: 0x0003AAFF File Offset: 0x00038CFF
		IControllerTemplateAxis IFlightPedalsTemplate.slide
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x0003AAD3 File Offset: 0x00038CD3
		public FlightPedalsTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x04005739 RID: 22329
		public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");

		// Token: 0x0400573A RID: 22330
		public const int elementId_leftPedal = 0;

		// Token: 0x0400573B RID: 22331
		public const int elementId_rightPedal = 1;

		// Token: 0x0400573C RID: 22332
		public const int elementId_slide = 2;
	}
}
