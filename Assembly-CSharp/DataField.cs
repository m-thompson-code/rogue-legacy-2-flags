using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class DataField<T>
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000142 RID: 322 RVA: 0x0000C16B File Offset: 0x0000A36B
	// (set) Token: 0x06000143 RID: 323 RVA: 0x0000C173 File Offset: 0x0000A373
	public T Value
	{
		get
		{
			return this.m_currentValue;
		}
		set
		{
			this.m_currentValue = value;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000144 RID: 324 RVA: 0x0000C17C File Offset: 0x0000A37C
	public T DefaultValue
	{
		get
		{
			return this.m_defaultValue;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000145 RID: 325 RVA: 0x0000C184 File Offset: 0x0000A384
	public bool IsDefault
	{
		get
		{
			return EqualityComparer<T>.Default.Equals(this.Value, this.m_defaultValue);
		}
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000C19C File Offset: 0x0000A39C
	public void SetDefaultValue(T defaultValue)
	{
		this.m_currentValue = defaultValue;
		this.m_defaultValue = defaultValue;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000C1AC File Offset: 0x0000A3AC
	public void Reset()
	{
		this.Value = this.m_defaultValue;
	}

	// Token: 0x0400025D RID: 605
	[SerializeField]
	private T m_currentValue;

	// Token: 0x0400025E RID: 606
	[SerializeField]
	private T m_defaultValue;
}
