using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection;

namespace Unity.Cloud.UserReporting.Plugin.SimpleJson
{
	// Token: 0x02000D39 RID: 3385
	[GeneratedCode("simple-json", "1.0.0")]
	public class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		// Token: 0x060060B9 RID: 24761 RVA: 0x00167E78 File Offset: 0x00166078
		public PocoJsonSerializerStrategy()
		{
			this.ConstructorCache = new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, ReflectionUtils.ConstructorDelegate>(this.ContructorDelegateFactory));
			this.GetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<string, ReflectionUtils.GetDelegate>>(this.GetterValueFactory));
			this.SetCache = new ReflectionUtils.ThreadSafeDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(new ReflectionUtils.ThreadSafeDictionaryValueFactory<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>>(this.SetterValueFactory));
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x0002B621 File Offset: 0x00029821
		protected virtual string MapClrMemberNameToJsonFieldName(string clrPropertyName)
		{
			return clrPropertyName;
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x00035562 File Offset: 0x00033762
		internal virtual ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
		{
			return ReflectionUtils.GetContructor(key, key.IsArray ? PocoJsonSerializerStrategy.ArrayConstructorParameterTypes : PocoJsonSerializerStrategy.EmptyTypes);
		}

		// Token: 0x060060BC RID: 24764 RVA: 0x00167ED4 File Offset: 0x001660D4
		internal virtual IDictionary<string, ReflectionUtils.GetDelegate> GetterValueFactory(Type type)
		{
			IDictionary<string, ReflectionUtils.GetDelegate> dictionary = new Dictionary<string, ReflectionUtils.GetDelegate>();
			foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
			{
				if (propertyInfo.CanRead)
				{
					MethodInfo getterMethodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
					if (!getterMethodInfo.IsStatic && getterMethodInfo.IsPublic)
					{
						dictionary[this.MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = ReflectionUtils.GetGetMethod(propertyInfo);
					}
				}
			}
			foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
			{
				if (!fieldInfo.IsStatic && fieldInfo.IsPublic)
				{
					dictionary[this.MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = ReflectionUtils.GetGetMethod(fieldInfo);
				}
			}
			return dictionary;
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x00167FC0 File Offset: 0x001661C0
		internal virtual IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> SetterValueFactory(Type type)
		{
			IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> dictionary = new Dictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>();
			foreach (PropertyInfo propertyInfo in ReflectionUtils.GetProperties(type))
			{
				if (propertyInfo.CanWrite)
				{
					MethodInfo setterMethodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
					if (!setterMethodInfo.IsStatic && setterMethodInfo.IsPublic)
					{
						dictionary[this.MapClrMemberNameToJsonFieldName(propertyInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(propertyInfo.PropertyType, ReflectionUtils.GetSetMethod(propertyInfo));
					}
				}
			}
			foreach (FieldInfo fieldInfo in ReflectionUtils.GetFields(type))
			{
				if (!fieldInfo.IsInitOnly && !fieldInfo.IsStatic && fieldInfo.IsPublic)
				{
					dictionary[this.MapClrMemberNameToJsonFieldName(fieldInfo.Name)] = new KeyValuePair<Type, ReflectionUtils.SetDelegate>(fieldInfo.FieldType, ReflectionUtils.GetSetMethod(fieldInfo));
				}
			}
			return dictionary;
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x0003557E File Offset: 0x0003377E
		public virtual bool TrySerializeNonPrimitiveObject(object input, out object output)
		{
			return this.TrySerializeKnownTypes(input, out output) || this.TrySerializeUnknownTypes(input, out output);
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x001680CC File Offset: 0x001662CC
		public virtual object DeserializeObject(object value, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string text = value as string;
			if (type == typeof(Guid) && string.IsNullOrEmpty(text))
			{
				return default(Guid);
			}
			if (value == null)
			{
				return null;
			}
			object obj = null;
			if (text != null)
			{
				if (text.Length != 0)
				{
					if (type == typeof(DateTime) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTime)))
					{
						return DateTime.ParseExact(text, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(DateTimeOffset) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset)))
					{
						return DateTimeOffset.ParseExact(text, PocoJsonSerializerStrategy.Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
					}
					if (type == typeof(Guid) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid)))
					{
						return new Guid(text);
					}
					if (type == typeof(Uri))
					{
						Uri result;
						if (Uri.IsWellFormedUriString(text, UriKind.RelativeOrAbsolute) && Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out result))
						{
							return result;
						}
						return null;
					}
					else
					{
						if (type == typeof(string))
						{
							return text;
						}
						if (type == typeof(TimeSpan) || (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(TimeSpan)))
						{
							return TimeSpan.Parse(text);
						}
						return Convert.ChangeType(text, type, CultureInfo.InvariantCulture);
					}
				}
				else
				{
					if (type == typeof(Guid))
					{
						obj = default(Guid);
					}
					else if (ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
					{
						obj = null;
					}
					else
					{
						obj = text;
					}
					if (!ReflectionUtils.IsNullableType(type) && Nullable.GetUnderlyingType(type) == typeof(Guid))
					{
						return text;
					}
				}
			}
			if (type.IsEnum)
			{
				return Enum.Parse(type, value.ToString());
			}
			if (value is bool)
			{
				return value;
			}
			bool flag = value is long;
			bool flag2 = value is double;
			if ((flag && type == typeof(long)) || (flag2 && type == typeof(double)))
			{
				return value;
			}
			if ((!flag2 || !(type != typeof(double))) && (!flag || !(type != typeof(long))))
			{
				IDictionary<string, object> dictionary = value as IDictionary<string, object>;
				if (dictionary != null)
				{
					IDictionary<string, object> dictionary2 = dictionary;
					if (ReflectionUtils.IsTypeDictionary(type))
					{
						Type[] genericTypeArguments = ReflectionUtils.GetGenericTypeArguments(type);
						Type type2 = genericTypeArguments[0];
						Type type3 = genericTypeArguments[1];
						Type key = typeof(Dictionary<, >).MakeGenericType(new Type[]
						{
							type2,
							type3
						});
						IDictionary dictionary3 = (IDictionary)this.ConstructorCache[key](Array.Empty<object>());
						foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
						{
							dictionary3.Add(keyValuePair.Key, this.DeserializeObject(keyValuePair.Value, type3));
						}
						return dictionary3;
					}
					if (type == typeof(object))
					{
						return value;
					}
					obj = Activator.CreateInstance(type);
					using (IEnumerator<KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> enumerator2 = this.SetCache[type].GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>> keyValuePair2 = enumerator2.Current;
							object value2;
							if (dictionary2.TryGetValue(keyValuePair2.Key, out value2))
							{
								value2 = this.DeserializeObject(value2, keyValuePair2.Value.Key);
								keyValuePair2.Value.Value(obj, value2);
							}
							else
							{
								string text2 = keyValuePair2.Key;
								text2 = char.ToLower(text2[0]).ToString() + text2.Substring(1);
								if (dictionary2.TryGetValue(text2, out value2))
								{
									value2 = this.DeserializeObject(value2, keyValuePair2.Value.Key);
									keyValuePair2.Value.Value(obj, value2);
								}
							}
						}
						return obj;
					}
				}
				IList<object> list = value as IList<object>;
				if (list != null)
				{
					IList<object> list2 = list;
					IList list3 = null;
					if (type.IsArray)
					{
						list3 = (IList)this.ConstructorCache[type](new object[]
						{
							list2.Count
						});
						int num = 0;
						using (IEnumerator<object> enumerator3 = list2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								object value3 = enumerator3.Current;
								list3[num++] = this.DeserializeObject(value3, type.GetElementType());
							}
							goto IL_663;
						}
					}
					if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || ReflectionUtils.IsAssignableFrom(typeof(IList), type))
					{
						Type genericListElementType = ReflectionUtils.GetGenericListElementType(type);
						ReflectionUtils.ConstructorDelegate constructorDelegate;
						if ((constructorDelegate = this.ConstructorCache[type]) == null)
						{
							constructorDelegate = this.ConstructorCache[typeof(List<>).MakeGenericType(new Type[]
							{
								genericListElementType
							})];
						}
						list3 = (IList)constructorDelegate(Array.Empty<object>());
						foreach (object value4 in list2)
						{
							list3.Add(this.DeserializeObject(value4, genericListElementType));
						}
					}
					IL_663:
					obj = list3;
				}
				return obj;
			}
			obj = ((type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(float) || type == typeof(bool) || type == typeof(decimal) || type == typeof(byte) || type == typeof(short)) ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : value);
			if (ReflectionUtils.IsNullableType(type))
			{
				return ReflectionUtils.ToNullableType(obj, type);
			}
			return obj;
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x00035594 File Offset: 0x00033794
		protected virtual object SerializeEnum(Enum p)
		{
			return Convert.ToDouble(p, CultureInfo.InvariantCulture);
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x00168788 File Offset: 0x00166988
		protected virtual bool TrySerializeKnownTypes(object input, out object output)
		{
			bool result = true;
			if (input is DateTime)
			{
				output = ((DateTime)input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is DateTimeOffset)
			{
				output = ((DateTimeOffset)input).ToUniversalTime().ToString(PocoJsonSerializerStrategy.Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is TimeSpan)
			{
				output = ((TimeSpan)input).ToString();
			}
			else if (input is Guid)
			{
				output = ((Guid)input).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else
			{
				Enum @enum = input as Enum;
				if (@enum != null)
				{
					output = this.SerializeEnum(@enum);
				}
				else
				{
					result = false;
					output = null;
				}
			}
			return result;
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x00168868 File Offset: 0x00166A68
		protected virtual bool TrySerializeUnknownTypes(object input, out object output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			output = null;
			Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			IDictionary<string, object> dictionary = new JsonObject();
			foreach (KeyValuePair<string, ReflectionUtils.GetDelegate> keyValuePair in this.GetCache[type])
			{
				if (keyValuePair.Value != null)
				{
					dictionary.Add(this.MapClrMemberNameToJsonFieldName(keyValuePair.Key), keyValuePair.Value(input));
				}
			}
			output = dictionary;
			return true;
		}

		// Token: 0x04004EED RID: 20205
		internal IDictionary<Type, ReflectionUtils.ConstructorDelegate> ConstructorCache;

		// Token: 0x04004EEE RID: 20206
		internal IDictionary<Type, IDictionary<string, ReflectionUtils.GetDelegate>> GetCache;

		// Token: 0x04004EEF RID: 20207
		internal IDictionary<Type, IDictionary<string, KeyValuePair<Type, ReflectionUtils.SetDelegate>>> SetCache;

		// Token: 0x04004EF0 RID: 20208
		internal static readonly Type[] EmptyTypes = new Type[0];

		// Token: 0x04004EF1 RID: 20209
		internal static readonly Type[] ArrayConstructorParameterTypes = new Type[]
		{
			typeof(int)
		};

		// Token: 0x04004EF2 RID: 20210
		private static readonly string[] Iso8601Format = new string[]
		{
			"yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z",
			"yyyy-MM-dd\\THH:mm:ss\\Z",
			"yyyy-MM-dd\\THH:mm:ssK"
		};
	}
}
