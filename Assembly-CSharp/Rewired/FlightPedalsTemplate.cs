using System;

namespace Rewired
{
	// Token: 0x0200092E RID: 2350
	public sealed class FlightPedalsTemplate : ControllerTemplate, IFlightPedalsTemplate, IControllerTemplate
	{
		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x00113750 File Offset: 0x00111950
		IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x00113759 File Offset: 0x00111959
		IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x00113762 File Offset: 0x00111962
		IControllerTemplateAxis IFlightPedalsTemplate.slide
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x0011376B File Offset: 0x0011196B
		public FlightPedalsTemplate(object payload) : base(payload)
		{
		}

		// Token: 0x040041CD RID: 16845
		public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");

		// Token: 0x040041CE RID: 16846
		public const int elementId_leftPedal = 0;

		// Token: 0x040041CF RID: 16847
		public const int elementId_rightPedal = 1;

		// Token: 0x040041D0 RID: 16848
		public const int elementId_slide = 2;
	}
}
