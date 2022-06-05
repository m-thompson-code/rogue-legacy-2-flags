using System;
using System.CodeDom.Compiler;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000D38 RID: 3384
	[GeneratedCode("simple-json", "1.0.0")]
	public interface IJsonSerializerStrategy
	{
		// Token: 0x060060B7 RID: 24759
		bool TrySerializeNonPrimitiveObject(object input, out object output);

		// Token: 0x060060B8 RID: 24760
		object DeserializeObject(object value, Type type);
	}
}
