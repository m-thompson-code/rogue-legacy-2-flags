using System;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[CreateAssetMenu(menuName = "Custom/Libraries/Class Library")]
public class ClassLibrary : ScriptableObject
{
	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x0600200B RID: 8203 RVA: 0x00010F7B File Offset: 0x0000F17B
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

	// Token: 0x0600200C RID: 8204 RVA: 0x000A4354 File Offset: 0x000A2554
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

	// Token: 0x04001CB5 RID: 7349
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/ClassLibrary";

	// Token: 0x04001CB6 RID: 7350
	[SerializeField]
	private ClassTypeClassDataDictionary m_classLibrary;

	// Token: 0x04001CB7 RID: 7351
	private static ClassLibrary m_instance;
}
