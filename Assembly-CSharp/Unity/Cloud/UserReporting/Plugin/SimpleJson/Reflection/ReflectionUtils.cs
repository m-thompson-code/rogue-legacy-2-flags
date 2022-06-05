using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection
{
	// Token: 0x0200084C RID: 2124
	[GeneratedCode("reflection-utils", "1.0.0")]
	internal class ReflectionUtils
	{
		// Token: 0x06004665 RID: 18021 RVA: 0x000FBD63 File Offset: 0x000F9F63
		public static Type GetTypeInfo(Type type)
		{
			return type;
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x000FBD66 File Offset: 0x000F9F66
		public static Attribute GetAttribute(MemberInfo info, Type type)
		{
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(info, type);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x000FBD8C File Offset: 0x000F9F8C
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

		// Token: 0x06004668 RID: 18024 RVA: 0x000FBE08 File Offset: 0x000FA008
		public static Attribute GetAttribute(Type objectType, Type attributeType)
		{
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(objectType, attributeType);
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x000FBE2E File Offset: 0x000FA02E
		public static Type[] GetGenericTypeArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		// Token: 0x0600466A RID: 18026 RVA: 0x000FBE36 File Offset: 0x000FA036
		public static bool IsTypeGeneric(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType;
		}

		// Token: 0x0600466B RID: 18027 RVA: 0x000FBE44 File Offset: 0x000FA044
		public static bool IsTypeGenericeCollectionInterface(Type type)
		{
			if (!ReflectionUtils.IsTypeGeneric(type))
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IEnumerable<>);
		}

		// Token: 0x0600466C RID: 18028 RVA: 0x000FBE98 File Offset: 0x000FA098
		public static bool IsAssignableFrom(Type type1, Type type2)
		{
			return ReflectionUtils.GetTypeInfo(type1).IsAssignableFrom(ReflectionUtils.GetTypeInfo(type2));
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x000FBEAB File Offset: 0x000FA0AB
		public static bool IsTypeDictionary(Type type)
		{
			return typeof(IDictionary).IsAssignableFrom(type) || (ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<, >));
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x000FBEE5 File Offset: 0x000FA0E5
		public static bool IsNullableType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x000FBF0B File Offset: 0x000FA10B
		public static object ToNullableType(object obj, Type nullableType)
		{
			if (obj != null)
			{
				return Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture);
			}
			return null;
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x000FBF23 File Offset: 0x000FA123
		public static bool IsValueType(Type type)
		{
			return ReflectionUtils.GetTypeInfo(type).IsValueType;
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x000FBF30 File Offset: 0x000FA130
		public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
		{
			return type.GetConstructors();
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x000FBF38 File Offset: 0x000FA138
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

		// Token: 0x06004673 RID: 18035 RVA: 0x000FBFD4 File Offset: 0x000FA1D4
		public static IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x000FBFDE File Offset: 0x000FA1DE
		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x000FBFE8 File Offset: 0x000FA1E8
		public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetGetMethod(true);
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x000FBFF1 File Offset: 0x000FA1F1
		public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetSetMethod(true);
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x000FBFFA File Offset: 0x000FA1FA
		public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
		{
			return ReflectionUtils.GetConstructorByReflection(constructorInfo);
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x000FC002 File Offset: 0x000FA202
		public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
		{
			return ReflectionUtils.GetConstructorByReflection(type, argsType);
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x000FC00B File Offset: 0x000FA20B
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
		{
			return (object[] args) => constructorInfo.Invoke(args);
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x000FC024 File Offset: 0x000FA224
		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
		{
			ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
			if (!(constructorInfo == null))
			{
				return ReflectionUtils.GetConstructorByReflection(constructorInfo);
			}
			return null;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x000FC04A File Offset: 0x000FA24A
		public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x000FC052 File Offset: 0x000FA252
		public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x000FC05A File Offset: 0x000FA25A
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
			return (object source) => methodInfo.Invoke(source, ReflectionUtils.EmptyObjects);
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x000FC078 File Offset: 0x000FA278
		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
		{
			return (object source) => fieldInfo.GetValue(source);
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x000FC091 File Offset: 0x000FA291
		public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x000FC099 File Offset: 0x000FA299
		public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x000FC0A1 File Offset: 0x000FA2A1
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

		// Token: 0x06004682 RID: 18050 RVA: 0x000FC0BF File Offset: 0x000FA2BF
		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
		{
			return delegate(object source, object value)
			{
				fieldInfo.SetValue(source, value);
			};
		}

		// Token: 0x04003B91 RID: 15249
		private static readonly object[] EmptyObjects = new object[0];

		// Token: 0x02000E5F RID: 3679
		// (Invoke) Token: 0x06006CA1 RID: 27809
		public delegate object GetDelegate(object source);

		// Token: 0x02000E60 RID: 3680
		// (Invoke) Token: 0x06006CA5 RID: 27813
		public delegate void SetDelegate(object source, object value);

		// Token: 0x02000E61 RID: 3681
		// (Invoke) Token: 0x06006CA9 RID: 27817
		public delegate object ConstructorDelegate(params object[] args);

		// Token: 0x02000E62 RID: 3682
		// (Invoke) Token: 0x06006CAD RID: 27821
		public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

		// Token: 0x02000E63 RID: 3683
		public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
		{
			// Token: 0x06006CB0 RID: 27824 RVA: 0x001941A6 File Offset: 0x001923A6
			public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
			{
				this._valueFactory = valueFactory;
			}

			// Token: 0x1700236A RID: 9066
			// (get) Token: 0x06006CB1 RID: 27825 RVA: 0x001941C0 File Offset: 0x001923C0
			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			// Token: 0x1700236B RID: 9067
			// (get) Token: 0x06006CB2 RID: 27826 RVA: 0x001941CD File Offset: 0x001923CD
			public bool IsReadOnly
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x1700236C RID: 9068
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

			// Token: 0x1700236D RID: 9069
			// (get) Token: 0x06006CB5 RID: 27829 RVA: 0x001941E4 File Offset: 0x001923E4
			public ICollection<TKey> Keys
			{
				get
				{
					return this._dictionary.Keys;
				}
			}

			// Token: 0x1700236E RID: 9070
			// (get) Token: 0x06006CB6 RID: 27830 RVA: 0x001941F1 File Offset: 0x001923F1
			public ICollection<TValue> Values
			{
				get
				{
					return this._dictionary.Values;
				}
			}

			// Token: 0x06006CB7 RID: 27831 RVA: 0x001941FE File Offset: 0x001923FE
			public void Add(TKey key, TValue value)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CB8 RID: 27832 RVA: 0x00194205 File Offset: 0x00192405
			public void Add(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CB9 RID: 27833 RVA: 0x0019420C File Offset: 0x0019240C
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

			// Token: 0x06006CBA RID: 27834 RVA: 0x001942AC File Offset: 0x001924AC
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CBB RID: 27835 RVA: 0x001942B3 File Offset: 0x001924B3
			public bool Contains(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CBC RID: 27836 RVA: 0x001942BA File Offset: 0x001924BA
			public bool ContainsKey(TKey key)
			{
				return this._dictionary.ContainsKey(key);
			}

			// Token: 0x06006CBD RID: 27837 RVA: 0x001942C8 File Offset: 0x001924C8
			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CBE RID: 27838 RVA: 0x001942D0 File Offset: 0x001924D0
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

			// Token: 0x06006CBF RID: 27839 RVA: 0x00194306 File Offset: 0x00192506
			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x06006CC0 RID: 27840 RVA: 0x00194318 File Offset: 0x00192518
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			// Token: 0x06006CC1 RID: 27841 RVA: 0x0019432A File Offset: 0x0019252A
			public bool Remove(TKey key)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CC2 RID: 27842 RVA: 0x00194331 File Offset: 0x00192531
			public bool Remove(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06006CC3 RID: 27843 RVA: 0x00194338 File Offset: 0x00192538
			public bool TryGetValue(TKey key, out TValue value)
			{
				value = this[key];
				return true;
			}

			// Token: 0x040057D6 RID: 22486
			private Dictionary<TKey, TValue> _dictionary;

			// Token: 0x040057D7 RID: 22487
			private readonly object _lock = new object();

			// Token: 0x040057D8 RID: 22488
			private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;
		}
	}
}
