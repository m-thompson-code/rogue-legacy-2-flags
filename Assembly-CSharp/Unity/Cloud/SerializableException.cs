using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Unity.Cloud
{
	// Token: 0x02000832 RID: 2098
	public class SerializableException
	{
		// Token: 0x06004556 RID: 17750 RVA: 0x000F7976 File Offset: 0x000F5B76
		public SerializableException()
		{
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x000F7980 File Offset: 0x000F5B80
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

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x000F7AD1 File Offset: 0x000F5CD1
		// (set) Token: 0x06004559 RID: 17753 RVA: 0x000F7AD9 File Offset: 0x000F5CD9
		public string DetailedProblemIdentifier { get; set; }

		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x000F7AE2 File Offset: 0x000F5CE2
		// (set) Token: 0x0600455B RID: 17755 RVA: 0x000F7AEA File Offset: 0x000F5CEA
		public string FullText { get; set; }

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x0600455C RID: 17756 RVA: 0x000F7AF3 File Offset: 0x000F5CF3
		// (set) Token: 0x0600455D RID: 17757 RVA: 0x000F7AFB File Offset: 0x000F5CFB
		public SerializableException InnerException { get; set; }

		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x000F7B04 File Offset: 0x000F5D04
		// (set) Token: 0x0600455F RID: 17759 RVA: 0x000F7B0C File Offset: 0x000F5D0C
		public string Message { get; set; }

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06004560 RID: 17760 RVA: 0x000F7B15 File Offset: 0x000F5D15
		// (set) Token: 0x06004561 RID: 17761 RVA: 0x000F7B1D File Offset: 0x000F5D1D
		public string ProblemIdentifier { get; set; }

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x000F7B26 File Offset: 0x000F5D26
		// (set) Token: 0x06004563 RID: 17763 RVA: 0x000F7B2E File Offset: 0x000F5D2E
		public List<SerializableStackFrame> StackTrace { get; set; }

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x000F7B37 File Offset: 0x000F5D37
		// (set) Token: 0x06004565 RID: 17765 RVA: 0x000F7B3F File Offset: 0x000F5D3F
		public string Type { get; set; }
	}
}
