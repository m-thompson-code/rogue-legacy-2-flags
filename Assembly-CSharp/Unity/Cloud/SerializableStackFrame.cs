using System;
using System.Diagnostics;
using System.Reflection;

namespace Unity.Cloud
{
	// Token: 0x02000833 RID: 2099
	public class SerializableStackFrame
	{
		// Token: 0x06004566 RID: 17766 RVA: 0x000F7B48 File Offset: 0x000F5D48
		public SerializableStackFrame()
		{
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x000F7B50 File Offset: 0x000F5D50
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

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x06004568 RID: 17768 RVA: 0x000F7BC5 File Offset: 0x000F5DC5
		// (set) Token: 0x06004569 RID: 17769 RVA: 0x000F7BCD File Offset: 0x000F5DCD
		public string DeclaringType { get; set; }

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x0600456A RID: 17770 RVA: 0x000F7BD6 File Offset: 0x000F5DD6
		// (set) Token: 0x0600456B RID: 17771 RVA: 0x000F7BDE File Offset: 0x000F5DDE
		public int FileColumn { get; set; }

		// Token: 0x17001726 RID: 5926
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x000F7BE7 File Offset: 0x000F5DE7
		// (set) Token: 0x0600456D RID: 17773 RVA: 0x000F7BEF File Offset: 0x000F5DEF
		public int FileLine { get; set; }

		// Token: 0x17001727 RID: 5927
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x000F7BF8 File Offset: 0x000F5DF8
		// (set) Token: 0x0600456F RID: 17775 RVA: 0x000F7C00 File Offset: 0x000F5E00
		public string FileName { get; set; }

		// Token: 0x17001728 RID: 5928
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x000F7C09 File Offset: 0x000F5E09
		// (set) Token: 0x06004571 RID: 17777 RVA: 0x000F7C11 File Offset: 0x000F5E11
		public string Method { get; set; }

		// Token: 0x17001729 RID: 5929
		// (get) Token: 0x06004572 RID: 17778 RVA: 0x000F7C1A File Offset: 0x000F5E1A
		// (set) Token: 0x06004573 RID: 17779 RVA: 0x000F7C22 File Offset: 0x000F5E22
		public string MethodName { get; set; }
	}
}
