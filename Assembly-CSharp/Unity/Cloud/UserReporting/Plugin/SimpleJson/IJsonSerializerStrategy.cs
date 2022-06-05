using System;
using System.CodeDom.Compiler;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x0200084A RID: 2122
	[GeneratedCode("simple-json", "1.0.0")]
	public interface IJsonSerializerStrategy
	{
		// Token: 0x06004658 RID: 18008
		bool TrySerializeNonPrimitiveObject(object input, out object output);

		// Token: 0x06004659 RID: 18009
		object DeserializeObject(object value, Type type);
	}
}
