using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000847 RID: 2119
	[GeneratedCode("simple-json", "1.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonArray : List<object>
	{
		// Token: 0x06004622 RID: 17954 RVA: 0x000FA238 File Offset: 0x000F8438
		public JsonArray()
		{
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x000FA240 File Offset: 0x000F8440
		public JsonArray(int capacity) : base(capacity)
		{
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x000FA249 File Offset: 0x000F8449
		public override string ToString()
		{
			return SimpleJson.SerializeObject(this) ?? string.Empty;
		}
	}
}
