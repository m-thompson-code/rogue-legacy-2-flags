using System;
using UnityEngine;

// Token: 0x02000222 RID: 546
[CreateAssetMenu(menuName = "Custom/Libraries/Class Library")]
public class ClassLibrary : ScriptableObject
{
	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x0600166E RID: 5742 RVA: 0x00045F5A File Offset: 0x0004415A
	private static ClassLibrary Instance
	{
		get
		{
			if (ClassLibrary.m_instance == null)
			{
				ClassLibrary.m_instance = CDGResources.Load<ClassLibrary>("Scriptable Objects/Libraries/ClassLibrary", "", true);
			}
			return ClassLibrary.m_instance;
		}
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x00045F84 File Offset: 0x00044184
	public static ClassData GetClassData(ClassType classType)
	{
		ClassData classData = null;
		ClassTypeClassDataDictionary classLibrary = ClassLibrary.Instance.m_classLibrary;
		if (classLibrary != null)
		{
			classLibrary.TryGetValue(classType, out classData);
			if (classData == null)
			{
				Debug.LogWarning("<color=red>Class: " + classType.ToString() + " not found in Class Library. Please ensure the class exists in the Class Library scriptable object.</color>");
			}
			return classData;
		}
		throw new Exception("Class Library is null.");
	}

	// Token: 0x040015A9 RID: 5545
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ClassLibrary";

	// Token: 0x040015AA RID: 5546
	[SerializeField]
	private ClassTypeClassDataDictionary m_classLibrary;

	// Token: 0x040015AB RID: 5547
	private static ClassLibrary m_instance;
}
