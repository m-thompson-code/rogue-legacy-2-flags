using System;
using UnityEngine;

// Token: 0x02000C68 RID: 3176
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
	// Token: 0x06005B9A RID: 23450 RVA: 0x0015A810 File Offset: 0x00158A10
	public ConditionalHideAttribute(string conditionalSourceField)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = false;
		this.Inverse = false;
	}

	// Token: 0x06005B9B RID: 23451 RVA: 0x0015A868 File Offset: 0x00158A68
	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.Inverse = false;
	}

	// Token: 0x06005B9C RID: 23452 RVA: 0x0015A8C0 File Offset: 0x00158AC0
	public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceField = conditionalSourceField;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x06005B9D RID: 23453 RVA: 0x0015A918 File Offset: 0x00158B18
	public ConditionalHideAttribute(bool hideInInspector = false)
	{
		this.ConditionalSourceField = "";
		this.HideInInspector = hideInInspector;
		this.Inverse = false;
	}

	// Token: 0x06005B9E RID: 23454 RVA: 0x0015A974 File Offset: 0x00158B74
	public ConditionalHideAttribute(string[] conditionalSourceFields, bool[] conditionalSourceFieldInverseBools, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceFields = conditionalSourceFields;
		this.ConditionalSourceFieldInverseBools = conditionalSourceFieldInverseBools;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x06005B9F RID: 23455 RVA: 0x0015A9D4 File Offset: 0x00158BD4
	public ConditionalHideAttribute(string[] conditionalSourceFields, bool hideInInspector, bool inverse)
	{
		this.ConditionalSourceFields = conditionalSourceFields;
		this.HideInInspector = hideInInspector;
		this.Inverse = inverse;
	}

	// Token: 0x04004C0D RID: 19469
	public string ConditionalSourceField = "";

	// Token: 0x04004C0E RID: 19470
	public string ConditionalSourceField2 = "";

	// Token: 0x04004C0F RID: 19471
	public string[] ConditionalSourceFields = new string[0];

	// Token: 0x04004C10 RID: 19472
	public bool[] ConditionalSourceFieldInverseBools = new bool[0];

	// Token: 0x04004C11 RID: 19473
	public bool HideInInspector;

	// Token: 0x04004C12 RID: 19474
	public bool Inverse;

	// Token: 0x04004C13 RID: 19475
	public bool UseOrLogic;

	// Token: 0x04004C14 RID: 19476
	public bool InverseCondition1;

	// Token: 0x04004C15 RID: 19477
	public bool InverseCondition2;
}
