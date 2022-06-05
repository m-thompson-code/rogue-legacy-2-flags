using System;
using System.Diagnostics;
using System.Reflection;

namespace Unity.Cloud
{
	// Token: 0x02000D18 RID: 3352
	public class SerializableStackFrame
	{
		// Token: 0x06005F9C RID: 24476 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public SerializableStackFrame()
		{
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x00165674 File Offset: 0x00163874
		public SerializableStackFrame(StackFrame stackFrame)
		{
			MethodBase method = stackFrame.GetMethod();
			Type declaringType = method.DeclaringType;
			this.DeclaringType = ((declaringType != null) ? declaringType.FullName : null);
			this.Method = method.ToString();
			this.MethodName = method.Name;
			this.FileName = stackFrame.GetFileName();
			this.FileLine = stackFrame.GetFileLineNumber();
			this.FileColumn = stackFrame.GetFileColumnNumber();
		}

		// Token: 0x17001F52 RID: 8018
		// (get) Token: 0x06005F9E RID: 24478 RVA: 0x00034B42 File Offset: 0x00032D42
		// (set) Token: 0x06005F9F RID: 24479 RVA: 0x00034B4A File Offset: 0x00032D4A
		public string DeclaringType { get; set; }

		// Token: 0x17001F53 RID: 8019
		// (get) Token: 0x06005FA0 RID: 24480 RVA: 0x00034B53 File Offset: 0x00032D53
		// (set) Token: 0x06005FA1 RID: 24481 RVA: 0x00034B5B File Offset: 0x00032D5B
		public int FileColumn { get; set; }

		// Token: 0x17001F54 RID: 8020
		// (get) Token: 0x06005FA2 RID: 24482 RVA: 0x00034B64 File Offset: 0x00032D64
		// (set) Token: 0x06005FA3 RID: 24483 RVA: 0x00034B6C File Offset: 0x00032D6C
		public int FileLine { get; set; }

		// Token: 0x17001F55 RID: 8021
		// (get) Token: 0x06005FA4 RID: 24484 RVA: 0x00034B75 File Offset: 0x00032D75
		// (set) Token: 0x06005FA5 RID: 24485 RVA: 0x00034B7D File Offset: 0x00032D7D
		public string FileName { get; set; }

		// Token: 0x17001F56 RID: 8022
		// (get) Token: 0x06005FA6 RID: 24486 RVA: 0x00034B86 File Offset: 0x00032D86
		// (set) Token: 0x06005FA7 RID: 24487 RVA: 0x00034B8E File Offset: 0x00032D8E
		public string Method { get; set; }

		// Token: 0x17001F57 RID: 8023
		// (get) Token: 0x06005FA8 RID: 24488 RVA: 0x00034B97 File Offset: 0x00032D97
		// (set) Token: 0x06005FA9 RID: 24489 RVA: 0x00034B9F File Offset: 0x00032D9F
		public string MethodName { get; set; }
	}
}
