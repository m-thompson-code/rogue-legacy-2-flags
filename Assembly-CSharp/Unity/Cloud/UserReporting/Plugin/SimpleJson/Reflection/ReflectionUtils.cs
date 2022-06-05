using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection
{
	// Token: 0x02000D3A RID: 3386
	[GeneratedCode("reflection-utils", "1.0.0")]
	internal class ReflectionUtils
	{
		// Token: 0x060060C4 RID: 24772 RVA: 0x000355A6 File Offset: 0x000337A6
		public static Type GetTypeInfo(Type type)
		{
			return type;
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x000355A9 File Offset: 0x000337A9
		public static Attribute GetAttribute(MemberInfo info, Type type)
		{
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(info, type);
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x00168960 File Offset: 0x00166B60
		public static Type GetGenericListElementType(Type type)
		{
			foreach (Type type2 in ((IEnumerable<Type>)type.GetInterfaces()))
			{
				if (ReflectionUtils.IsTypeGeneric(type2) && type2.GetGenericTypeDefinition() == typeof(IList<>))
				{
					return ReflectionUtils.GetGenericTypeArguments(type2)[0];
				}
			}
			return ReflectionUtils.GetGenericTypeArguments(type)[0];
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x000355CF File Offset: 0x000337CF
		public static Attribute GetAttribute(Type objectType, Type attributeType)
		{
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(objectType, attributeType);
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x000355F5 File Offset: 0x000337F5
		public static Type[] GetGenericTypeArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x000355FD File Offset: 0x000337FD
		public static bool IsTypeGeneric(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType;
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x001689DC File Offset: 0x00166BDC
		public static bool IsTypeGenericeCollectionInterface(Type type)
		{
			if (!ReflectionUtils.IsTypeGeneric(type))
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IEnumerable<>);
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0003560A File Offset: 0x0003380A
		public static bool IsAssignableFrom(Type type1, Type type2)
		{
			return ReflectionUtils.GetTypeInfo(type1).IsAssignableFrom(ReflectionUtils.GetTypeInfo(type2));
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x0003561D File Offset: 0x0003381D
		public static bool IsTypeDictionary(Type type)
		{
			return typeof(IDictionary).IsAssignableFrom(type) || (ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<, >));
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x00035657 File Offset: 0x00033857
		public static bool IsNullableType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0003567D File Offset: 0x0003387D
		public static object ToNullableType(object obj, Type nullableType)
		{
			if (obj != null)
			{
				return Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture);
			}
			return null;
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x00035695 File Offset: 0x00033895
		public static bool IsValueType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsValueType;
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x000356A2 File Offset: 0x000338A2
		public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
		{
			return type.GetConstructors();
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x00168A30 File Offset: 0x00166C30
		public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
		{
			foreach (ConstructorInfo constructorInfo in ReflectionUtils.GetConstructors(type))
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (argsType.Length == parameters.Length)
				{
					int num = 0;
					bool flag = true;
					ParameterInfo[] parameters2 = constructorInfo.GetParameters();
					for (int i = 0; i < parameters2.Length; i++)
					{
						if (parameters2[i].ParameterType != argsType[num])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return constructorInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x000356AA File Offset: 0x000338AA
		public static IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x000356B4 File Offset: 0x000338B4
		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x000356BE File Offset: 0x000338BE
		public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x000356C7 File Offset: 0x000338C7
		public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x000356D0 File Offset: 0x000338D0
		public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
		{
			return ReflectionUtils.GetConstructorByReflection(constructorInfo);
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x000356D8 File Offset: 0x000338D8
		public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
		{
			return ReflectionUtils.GetConstructorByReflection(type, argsType);
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x000356E1 File Offset: 0x000338E1
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
		{
			return (object[] args) => constructorInfo.Invoke(args);
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x00168ACC File Offset: 0x00166CCC
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
		{
			ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
			if (!(constructorInfo == null))
			{
				return ReflectionUtils.GetConstructorByReflection(constructorInfo);
			}
			return null;
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x000356FA File Offset: 0x000338FA
		public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x00035702 File Offset: 0x00033902
		public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x0003570A File Offset: 0x0003390A
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
			return (object source) => methodInfo.Invoke(source, ReflectionUtils.EmptyObjects);
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x00035728 File Offset: 0x00033928
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
		{
			return (object source) => fieldInfo.GetValue(source);
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x00035741 File Offset: 0x00033941
		public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x00035749 File Offset: 0x00033949
		public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x00035751 File Offset: 0x00033951
		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
			return delegate(object source, object value)
			{
				methodInfo.Invoke(source, new object[]
				{
					value
				});
			};
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x0003576F File Offset: 0x0003396F
		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
		{
			return delegate(object source, object value)
			{
				fieldInfo.SetValue(source, value);
			};
		}

		// Token: 0x04004EF3 RID: 20211
		private static readonly object[] EmptyObjects = new object[0];

		// Token: 0x02000D3B RID: 3387
		// (Invoke) Token: 0x060060E5 RID: 24805
		public delegate object GetDelegate(object source);

		// Token: 0x02000D3C RID: 3388
		// (Invoke) Token: 0x060060E9 RID: 24809
		public delegate void SetDelegate(object source, object value);

		// Token: 0x02000D3D RID: 3389
		// (Invoke) Token: 0x060060ED RID: 24813
		public delegate object ConstructorDelegate(params object[] args);

		// Token: 0x02000D3E RID: 3390
		// (Invoke) Token: 0x060060F1 RID: 24817
		public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

		// Token: 0x02000D3F RID: 3391
		public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
		{
			// Token: 0x060060F4 RID: 24820 RVA: 0x00035795 File Offset: 0x00033995
			public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
			{
				this._valueFactory = valueFactory;
			}

			// Token: 0x17001FAB RID: 8107
			// (get) Token: 0x060060F5 RID: 24821 RVA: 0x000357AF File Offset: 0x000339AF
			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			// Token: 0x17001FAC RID: 8108
			// (get) Token: 0x060060F6 RID: 24822 RVA: 0x00027E04 File Offset: 0x00026004
			public bool IsReadOnly
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17001FAD RID: 8109
			public TValue this[TKey key]
			{
				get
				{
					return this.Get(key);
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17001FAE RID: 8110
			// (get) Token: 0x060060F9 RID: 24825 RVA: 0x000357C5 File Offset: 0x000339C5
			public ICollection<TKey> Keys
			{
				get
				{
					return this._dictionary.Keys;
				}
			}

			// Token: 0x17001FAF RID: 8111
			// (get) Token: 0x060060FA RID: 24826 RVA: 0x000357D2 File Offset: 0x000339D2
			public ICollection<TValue> Values
			{
				get
				{
					return this._dictionary.Values;
				}
			}

			// Token: 0x060060FB RID: 24827 RVA: 0x00027E04 File Offset: 0x00026004
			public void Add(TKey key, TValue value)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060060FC RID: 24828 RVA: 0x00027E04 File Offset: 0x00026004
			public void Add(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060060FD RID: 24829 RVA: 0x00168AF4 File Offset: 0x00166CF4
			private TValue AddValue(TKey key)
			{
				TValue tvalue = this._valueFactory(key);
				object @lock = this._lock;
				lock (@lock)
				{
					if (this._dictionary == null)
					{
						this._dictionary = new Dictionary<TKey, TValue>();
						this._dictionary[key] = tvalue;
					}
					else
					{
						TValue result;
						if (this._dictionary.TryGetValue(key, out result))
						{
							return result;
						}
						Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._dictionary);
						dictionary[key] = tvalue;
						this._dictionary = dictionary;
					}
				}
				return tvalue;
			}

			// Token: 0x060060FE RID: 24830 RVA: 0x00027E04 File Offset: 0x00026004
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x060060FF RID: 24831 RVA: 0x00027E04 File Offset: 0x00026004
			public bool Contains(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006100 RID: 24832 RVA: 0x000357DF File Offset: 0x000339DF
			public bool ContainsKey(TKey key)
			{
				return this._dictionary.ContainsKey(key);
			}

			// Token: 0x06006101 RID: 24833 RVA: 0x00027E04 File Offset: 0x00026004
			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006102 RID: 24834 RVA: 0x00168B94 File Offset: 0x00166D94
			private TValue Get(TKey key)
			{
				if (this._dictionary == null)
				{
					return this.AddValue(key);
				}
				TValue result;
				if (!this._dictionary.TryGetValue(key, out result))
				{
					return this.AddValue(key);
				}
				return result;
			}

			// Token: 0x06006103 RID: 24835 RVA: 0x000357ED File Offset: 0x000339ED
			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x06006104 RID: 24836 RVA: 0x000357ED File Offset: 0x000339ED
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x06006105 RID: 24837 RVA: 0x00027E04 File Offset: 0x00026004
			public bool Remove(TKey key)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006106 RID: 24838 RVA: 0x00027E04 File Offset: 0x00026004
			public bool Remove(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006107 RID: 24839 RVA: 0x000357FF File Offset: 0x000339FF
			public bool TryGetValue(TKey key, out TValue value)
			{
				value = this[key];
				return true;
			}

			// Token: 0x04004EF4 RID: 20212
			private Dictionary<TKey, TValue> _dictionary;

			// Token: 0x04004EF5 RID: 20213
			private readonly object _lock = new object();

			// Token: 0x04004EF6 RID: 20214
			private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;
		}
	}
}
