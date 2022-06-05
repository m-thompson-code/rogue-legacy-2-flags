using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

// Token: 0x02000801 RID: 2049
public class MonoBehaviour_RL : MonoBehaviour
{
	// Token: 0x060043F0 RID: 17392 RVA: 0x000F0450 File Offset: 0x000EE650
	public virtual void OnDestroy()
	{
		foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			Type fieldType = fieldInfo.FieldType;
			if (typeof(IList).IsAssignableFrom(fieldType))
			{
				IList list = fieldInfo.GetValue(this) as IList;
				if (list != null)
				{
					list.Clear();
					return;
				}
			}
			if (typeof(IDictionary).IsAssignableFrom(fieldType))
			{
				IDictionary dictionary = fieldInfo.GetValue(this) as IDictionary;
				if (dictionary != null)
				{
					dictionary.Clear();
				}
			}
			if (typeof(Array).IsAssignableFrom(fieldType))
			{
				Array array = fieldInfo.GetValue(this) as Array;
				if (array != null)
				{
					Array.Clear(array, 0, array.Length);
				}
			}
			if (!fieldType.IsPrimitive)
			{
				fieldInfo.SetValue(this, null);
			}
		}
		foreach (PropertyInfo propertyInfo in base.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			Type propertyType = propertyInfo.PropertyType;
			if (typeof(IList).IsAssignableFrom(propertyType))
			{
				IList list2 = propertyInfo.GetValue(this, null) as IList;
				if (list2 != null)
				{
					list2.Clear();
					return;
				}
			}
			if (typeof(IDictionary).IsAssignableFrom(propertyType))
			{
				IDictionary dictionary2 = propertyInfo.GetValue(this, null) as IDictionary;
				if (dictionary2 != null)
				{
					dictionary2.Clear();
				}
			}
			if (typeof(Array).IsAssignableFrom(propertyType))
			{
				Array array2 = propertyInfo.GetValue(this, null) as Array;
				if (array2 != null)
				{
					Array.Clear(array2, 0, array2.Length);
				}
			}
			if (!propertyType.IsPrimitive)
			{
				propertyInfo.SetValue(this, null, null);
			}
		}
	}
}
