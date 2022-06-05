using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unity.Cloud
{
	// Token: 0x02000D17 RID: 3351
	public class SerializableException
	{
		// Token: 0x06005F8C RID: 24460 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public SerializableException()
		{
		}

		// Token: 0x06005F8D RID: 24461 RVA: 0x00165520 File Offset: 0x00163720
		public SerializableException(Exception exception)
		{
			this.Message = exception.Message;
			this.FullText = exception.ToString();
			Type type = exception.GetType();
			this.Type = type.FullName;
			this.StackTrace = new List<SerializableStackFrame>();
			foreach (StackFrame stackFrame in new StackTrace(exception, true).GetFrames())
			{
				this.StackTrace.Add(new SerializableStackFrame(stackFrame));
			}
			if (this.StackTrace.Count > 0)
			{
				SerializableStackFrame serializableStackFrame = this.StackTrace[0];
				this.ProblemIdentifier = string.Format("{0} at {1}.{2}", this.Type, serializableStackFrame.DeclaringType, serializableStackFrame.MethodName);
			}
			else
			{
				this.ProblemIdentifier = this.Type;
			}
			if (this.StackTrace.Count > 1)
			{
				SerializableStackFrame serializableStackFrame2 = this.StackTrace[0];
				SerializableStackFrame serializableStackFrame3 = this.StackTrace[1];
				this.DetailedProblemIdentifier = string.Format("{0} at {1}.{2} from {3}.{4}", new object[]
				{
					this.Type,
					serializableStackFrame2.DeclaringType,
					serializableStackFrame2.MethodName,
					serializableStackFrame3.DeclaringType,
					serializableStackFrame3.MethodName
				});
			}
			if (exception.InnerException != null)
			{
				this.InnerException = new SerializableException(exception.InnerException);
			}
		}

		// Token: 0x17001F4B RID: 8011
		// (get) Token: 0x06005F8E RID: 24462 RVA: 0x00034ACB File Offset: 0x00032CCB
		// (set) Token: 0x06005F8F RID: 24463 RVA: 0x00034AD3 File Offset: 0x00032CD3
		public string DetailedProblemIdentifier { get; set; }

		// Token: 0x17001F4C RID: 8012
		// (get) Token: 0x06005F90 RID: 24464 RVA: 0x00034ADC File Offset: 0x00032CDC
		// (set) Token: 0x06005F91 RID: 24465 RVA: 0x00034AE4 File Offset: 0x00032CE4
		public string FullText { get; set; }

		// Token: 0x17001F4D RID: 8013
		// (get) Token: 0x06005F92 RID: 24466 RVA: 0x00034AED File Offset: 0x00032CED
		// (set) Token: 0x06005F93 RID: 24467 RVA: 0x00034AF5 File Offset: 0x00032CF5
		public SerializableException InnerException { get; set; }

		// Token: 0x17001F4E RID: 8014
		// (get) Token: 0x06005F94 RID: 24468 RVA: 0x00034AFE File Offset: 0x00032CFE
		// (set) Token: 0x06005F95 RID: 24469 RVA: 0x00034B06 File Offset: 0x00032D06
		public string Message { get; set; }

		// Token: 0x17001F4F RID: 8015
		// (get) Token: 0x06005F96 RID: 24470 RVA: 0x00034B0F File Offset: 0x00032D0F
		// (set) Token: 0x06005F97 RID: 24471 RVA: 0x00034B17 File Offset: 0x00032D17
		public string ProblemIdentifier { get; set; }

		// Token: 0x17001F50 RID: 8016
		// (get) Token: 0x06005F98 RID: 24472 RVA: 0x00034B20 File Offset: 0x00032D20
		// (set) Token: 0x06005F99 RID: 24473 RVA: 0x00034B28 File Offset: 0x00032D28
		public List<SerializableStackFrame> StackTrace { get; set; }

		// Token: 0x17001F51 RID: 8017
		// (get) Token: 0x06005F9A RID: 24474 RVA: 0x00034B31 File Offset: 0x00032D31
		// (set) Token: 0x06005F9B RID: 24475 RVA: 0x00034B39 File Offset: 0x00032D39
		public string Type { get; set; }
	}
}
