using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000D35 RID: 3381
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonArray : List<object>
	{
		// Token: 0x06006081 RID: 24705 RVA: 0x00035367 File Offset: 0x00033567
		public JsonArray()
		{
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x0003536F File Offset: 0x0003356F
		public JsonArray(int capacity) : base(capacity)
		{
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x00035378 File Offset: 0x00033578
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this) ?? string.Empty;
		}
	}
}
