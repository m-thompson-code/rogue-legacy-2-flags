using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class DataField<T>
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000156 RID: 342 RVA: 0x00003781 File Offset: 0x00001981
	// (set) Token: 0x06000157 RID: 343 RVA: 0x00003789 File Offset: 0x00001989
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

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000158 RID: 344 RVA: 0x00003792 File Offset: 0x00001992
	public T DefaultValue
	{
		get
		{
			return this.m_defaultValue;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000159 RID: 345 RVA: 0x0000379A File Offset: 0x0000199A
	public bool IsDefault
	{
		get
		{
			return EqualityComparer<T>.Default.Equals(this.Value, this.m_defaultValue);
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000037B2 File Offset: 0x000019B2
	public void SetDefaultValue(T defaultValue)
	{
		this.m_currentValue = defaultValue;
		this.m_defaultValue = defaultValue;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000037C2 File Offset: 0x000019C2
	public void Reset()
	{
		this.Value = this.m_defaultValue;
	}

	// Token: 0x0400027E RID: 638
	[SerializeField]
	private T m_currentValue;

	// Token: 0x0400027F RID: 639
	[SerializeField]
	private T m_defaultValue;
}
