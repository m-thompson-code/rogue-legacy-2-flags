using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023B RID: 571
[CreateAssetMenu(menuName = "Custom/Libraries/Look Library")]
public class LookLibrary : ScriptableObject
{
	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x060016F3 RID: 5875 RVA: 0x00047B76 File Offset: 0x00045D76
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

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x060016F4 RID: 5876 RVA: 0x00047B9F File Offset: 0x00045D9F
	public static Material VampireFangsMaterial
	{
		get
		{
			return LookLibrary.Instance.m_vampireFangs_Material;
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x060016F5 RID: 5877 RVA: 0x00047BAB File Offset: 0x00045DAB
	public static Material ClownEyesMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownEyes_Material;
		}
	}

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x060016F6 RID: 5878 RVA: 0x00047BB7 File Offset: 0x00045DB7
	public static Material ClownMouthMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownMouth_Material;
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x060016F7 RID: 5879 RVA: 0x00047BC3 File Offset: 0x00045DC3
	public static Material ClownHeadMaterial
	{
		get
		{
			return LookLibrary.Instance.m_clownHead_Material;
		}
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x00047BD0 File Offset: 0x00045DD0
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

	// Token: 0x060016F9 RID: 5881 RVA: 0x00047CC0 File Offset: 0x00045EC0
	public static List<MaterialWeightObject> GetEyeLookData()
	{
		return LookLibrary.Instance.m_eyeData.WeightList;
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x00047CD1 File Offset: 0x00045ED1
	public static List<MaterialWeightObject> GetEyeLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Eyes);
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x00047CDA File Offset: 0x00045EDA
	public static List<MaterialWeightObject> GetMouthLookData()
	{
		return LookLibrary.Instance.m_mouthData.WeightList;
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x00047CEB File Offset: 0x00045EEB
	public static List<MaterialWeightObject> GetMouthLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Mouth);
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x00047CF4 File Offset: 0x00045EF4
	public static List<MaterialWeightObject> GetFacialHairLookData()
	{
		return LookLibrary.Instance.m_facialHairData.WeightList;
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x00047D05 File Offset: 0x00045F05
	public static List<MaterialWeightObject> GetFacialHairLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.FacialHair);
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x00047D0E File Offset: 0x00045F0E
	public static List<ColorWeightObject> GetSkinColorLookData()
	{
		return LookLibrary.Instance.m_skinColorData.WeightList;
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x00047D1F File Offset: 0x00045F1F
	public static List<ColorWeightObject> GetSkinColorLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<ColorWeightObject>(tag, LookType.SkinColor);
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x00047D28 File Offset: 0x00045F28
	public static List<MaterialWeightObject> GetHairLookData()
	{
		return LookLibrary.Instance.m_hairData.WeightList;
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x00047D39 File Offset: 0x00045F39
	public static List<MaterialWeightObject> GetHairLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<MaterialWeightObject>(tag, LookType.Hair);
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x00047D42 File Offset: 0x00045F42
	public static List<BodyWeightObject> GetBodyLookData()
	{
		return LookLibrary.Instance.m_bodyData.WeightList;
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x00047D53 File Offset: 0x00045F53
	public static List<BodyWeightObject> GetBodyLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<BodyWeightObject>(tag, LookType.Body);
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x00047D5C File Offset: 0x00045F5C
	public static List<ColorWeightObject> GetHairColorLookData()
	{
		return LookLibrary.Instance.m_hairColorData.WeightList;
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x00047D6D File Offset: 0x00045F6D
	public static List<ColorWeightObject> GetHairColorLookData(string tag)
	{
		return LookLibrary.GetLookDataByTag<ColorWeightObject>(tag, LookType.HairColor);
	}

	// Token: 0x04001677 RID: 5751
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/LookLibrary";

	// Token: 0x04001678 RID: 5752
	[SerializeField]
	private MaterialWeight_LookData m_eyeData;

	// Token: 0x04001679 RID: 5753
	[SerializeField]
	private MaterialWeight_LookData m_mouthData;

	// Token: 0x0400167A RID: 5754
	[SerializeField]
	private MaterialWeight_LookData m_facialHairData;

	// Token: 0x0400167B RID: 5755
	[SerializeField]
	private ColorWeight_LookData m_skinColorData;

	// Token: 0x0400167C RID: 5756
	[SerializeField]
	private MaterialWeight_LookData m_hairData;

	// Token: 0x0400167D RID: 5757
	[SerializeField]
	private ColorWeight_LookData m_hairColorData;

	// Token: 0x0400167E RID: 5758
	[SerializeField]
	private BodyWeight_LookData m_bodyData;

	// Token: 0x0400167F RID: 5759
	[Header("Special Materials")]
	[SerializeField]
	private Material m_vampireFangs_Material;

	// Token: 0x04001680 RID: 5760
	[Space(5f)]
	[SerializeField]
	private Material m_clownEyes_Material;

	// Token: 0x04001681 RID: 5761
	[SerializeField]
	private Material m_clownMouth_Material;

	// Token: 0x04001682 RID: 5762
	[SerializeField]
	private Material m_clownHead_Material;

	// Token: 0x04001683 RID: 5763
	private static LookLibrary m_instance;
}
