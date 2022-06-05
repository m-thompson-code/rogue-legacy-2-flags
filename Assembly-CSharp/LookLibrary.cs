using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
[CreateAssetMenu(menuName = "Custom/Libraries/Look Library")]
public class LookLibrary : ScriptableObject
{
	// Token: 0x17000E68 RID: 3688
	// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000114A1 File Offset: 0x0000F6A1
	private static LookLibrary Instance
	{
		get
		{
			if (LookLibrary.m_instance == null)
			{
				LookLibrary.m_instance = CDGResources.Load<LookLibrary>("Scriptable Objects/Libraries/LookLibrary", "", true);
			}
			return LookLibrary.m_instance;
		}
	}

	// Token: 0x17000E69 RID: 3689
	// (get) Token: 0x060020A7 RID: 8359 RVA: 0x000114CA File Offset: 0x0000F6CA
	public static Material VampireFangsMaterial
	{
		get
		{
			return LookLibrary.Instance.m_vampireFangs_Material;
		}
	}

	// Token: 0x17000E6A RID: 3690
	// (get) Token: 0x060020A8 RID: 8360 RVA: 0x000114D6 File Offset: 0x0000F6D6
	public static Material ClownEyesMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownEyes_Material;
		}
	}

	// Token: 0x17000E6B RID: 3691
	// (get) Token: 0x060020A9 RID: 8361 RVA: 0x000114E2 File Offset: 0x0000F6E2
	public static Material ClownMouthMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownMouth_Material;
		}
	}

	// Token: 0x17000E6C RID: 3692
	// (get) Token: 0x060020AA RID: 8362 RVA: 0x000114EE File Offset: 0x0000F6EE
	public static Material ClownHeadMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownHead_Material;
		}
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000A5AB0 File Offset: 0x000A3CB0
	public static List<T> GetLookDataByTag<T>(string tag, LookType lookType)
	{
		List<T> list = new List<T>();
		IList list2 = null;
		switch (lookType)
		{
		case LookType.Eyes:
			list2 = LookLibrary.GetEyeLookData();
			break;
		case LookType.Mouth:
			list2 = LookLibrary.GetMouthLookData();
			break;
		case LookType.FacialHair:
			list2 = LookLibrary.GetFacialHairLookData();
			break;
		case LookType.SkinColor:
			list2 = LookLibrary.GetSkinColorLookData();
			break;
		case LookType.Hair:
			list2 = LookLibrary.GetHairLookData();
			break;
		case LookType.Body:
			list2 = LookLibrary.GetBodyLookData();
			break;
		case LookType.HairColor:
			list2 = LookLibrary.GetHairColorLookData();
			break;
		}
		int count = list2.Count;
		foreach (object obj in list2)
		{
			ILookWeight lookWeight = (ILookWeight)obj;
			string[] tags = lookWeight.Tags;
			for (int i = 0; i < tags.Length; i++)
			{
				if (tags[i] == tag)
				{
					list.Add((T)((object)lookWeight));
				}
			}
		}
		return list;
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x000114FA File Offset: 0x0000F6FA
	public static List<MaterialWeightObject> GetEyeLookData()
	{
		return LookLibrary.Instance.m_eyeData.WeightList;
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x0001150B File Offset: 0x0000F70B
	public static List<MaterialWeightObject> GetEyeLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Eyes);
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x00011514 File Offset: 0x0000F714
	public static List<MaterialWeightObject> GetMouthLookData()
	{
		return LookLibrary.Instance.m_mouthData.WeightList;
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x00011525 File Offset: 0x0000F725
	public static List<MaterialWeightObject> GetMouthLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Mouth);
	}

	// Token: 0x060020B0 RID: 8368 RVA: 0x0001152E File Offset: 0x0000F72E
	public static List<MaterialWeightObject> GetFacialHairLookData()
	{
		return LookLibrary.Instance.m_facialHairData.WeightList;
	}

	// Token: 0x060020B1 RID: 8369 RVA: 0x0001153F File Offset: 0x0000F73F
	public static List<MaterialWeightObject> GetFacialHairLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.FacialHair);
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x00011548 File Offset: 0x0000F748
	public static List<ColorWeightObject> GetSkinColorLookData()
	{
		return LookLibrary.Instance.m_skinColorData.WeightList;
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x00011559 File Offset: 0x0000F759
	public static List<ColorWeightObject> GetSkinColorLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<ColorWeightObject>(tag, LookType.SkinColor);
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x00011562 File Offset: 0x0000F762
	public static List<MaterialWeightObject> GetHairLookData()
	{
		return LookLibrary.Instance.m_hairData.WeightList;
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x00011573 File Offset: 0x0000F773
	public static List<MaterialWeightObject> GetHairLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Hair);
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x0001157C File Offset: 0x0000F77C
	public static List<BodyWeightObject> GetBodyLookData()
	{
		return LookLibrary.Instance.m_bodyData.WeightList;
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x0001158D File Offset: 0x0000F78D
	public static List<BodyWeightObject> GetBodyLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<BodyWeightObject>(tag, LookType.Body);
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x00011596 File Offset: 0x0000F796
	public static List<ColorWeightObject> GetHairColorLookData()
	{
		return LookLibrary.Instance.m_hairColorData.WeightList;
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000115A7 File Offset: 0x0000F7A7
	public static List<ColorWeightObject> GetHairColorLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<ColorWeightObject>(tag, LookType.HairColor);
	}

	// Token: 0x04001D8F RID: 7567
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/LookLibrary";

	// Token: 0x04001D90 RID: 7568
	[SerializeField]
	private MaterialWeight_LookData m_eyeData;

	// Token: 0x04001D91 RID: 7569
	[SerializeField]
	private MaterialWeight_LookData m_mouthData;

	// Token: 0x04001D92 RID: 7570
	[SerializeField]
	private MaterialWeight_LookData m_facialHairData;

	// Token: 0x04001D93 RID: 7571
	[SerializeField]
	private ColorWeight_LookData m_skinColorData;

	// Token: 0x04001D94 RID: 7572
	[SerializeField]
	private MaterialWeight_LookData m_hairData;

	// Token: 0x04001D95 RID: 7573
	[SerializeField]
	private ColorWeight_LookData m_hairColorData;

	// Token: 0x04001D96 RID: 7574
	[SerializeField]
	private BodyWeight_LookData m_bodyData;

	// Token: 0x04001D97 RID: 7575
	[Header("Special Materials")]
	[SerializeField]
	private Material m_vampireFangs_Material;

	// Token: 0x04001D98 RID: 7576
	[Space(5f)]
	[SerializeField]
	private Material m_clownEyes_Material;

	// Token: 0x04001D99 RID: 7577
	[SerializeField]
	private Material m_clownMouth_Material;

	// Token: 0x04001D9A RID: 7578
	[SerializeField]
	private Material m_clownHead_Material;

	// Token: 0x04001D9B RID: 7579
	private static LookLibrary m_instance;
}
