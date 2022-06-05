using System;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
	// Token: 0x06004211 RID: 16913 RVA: 0x000EB6D8 File Offset: 0x000E98D8
	public ConditionalHideAttribute(string conditionalSourceField)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = false;
		this.Inverse = false;
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x000EB730 File Offset: 0x000E9930
	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.Inverse = false;
	}

	// Token: 0x06004213 RID: 16915 RVA: 0x000EB788 File Offset: 0x000E9988
	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x06004214 RID: 16916 RVA: 0x000EB7E0 File Offset: 0x000E99E0
	public ConditionalHideAttribute(bool hideInInspector = false)
	{
		this.ConditionalSourceField = "";
		this.HideInInspector = hideInInspector;
		this.Inverse = false;
	}

	// Token: 0x06004215 RID: 16917 RVA: 0x000EB83C File Offset: 0x000E9A3C
	public ConditionalHideAttribute(string[] conditionalSourceFields, bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceFields = conditionalSourceFields;
		this.ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x000EB89C File Offset: 0x000E9A9C
	public ConditionalHideAttribute(string[] conditionalSourceFields, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceFields = conditionalSourceFields;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x04003948 RID: 14664
	public string ConditionalSourceField = "";

	// Token: 0x04003949 RID: 14665
	public string ConditionalSourceField2 = "";

	// Token: 0x0400394A RID: 14666
	public string[] ConditionalSourceFields = new string[0];

	// Token: 0x0400394B RID: 14667
	public bool[] ConditionalSourceFieldInverseBools = new bool[0];

	// Token: 0x0400394C RID: 14668
	public bool HideInInspector;

	// Token: 0x0400394D RID: 14669
	public bool Inverse;

	// Token: 0x0400394E RID: 14670
	public bool UseOrLogic;

	// Token: 0x0400394F RID: 14671
	public bool InverseCondition1;

	// Token: 0x04003950 RID: 14672
	public bool InverseCondition2;
}
