using System;
using UnityEngine;

// Token: 0x02000CD2 RID: 3282
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Prop Spawn Controller Utility")]
public class PropSpawnControllerUtility : ScriptableObject
{
	// Token: 0x17001EEE RID: 7918
	// (get) Token: 0x06005D98 RID: 23960 RVA: 0x0015E89C File Offset: 0x0015CA9C
	private static PropSpawnControllerUtility Instance
	{
		get
		{
			if (PropSpawnControllerUtility.m_instance == null)
			{
				if (Application.isPlaying)
				{
					PropSpawnControllerUtility.m_instance = CDGResources.Load<PropSpawnControllerUtility>("Scriptable Objects/PropSpawnControllerUtility", "", true);
				}
				if (PropSpawnControllerUtility.m_instance == null)
				{
					Debug.LogFormat("<color=red>| PropSpawnControllerUtility | Failed to find PropSpawnControllerUtility SO at Path ({0})</color>", new object[]
					{
						"Scriptable Objects/PropSpawnControllerUtility"
					});
				}
			}
			return PropSpawnControllerUtility.m_instance;
		}
	}

	// Token: 0x06005D99 RID: 23961 RVA: 0x0015E8FC File Offset: 0x0015CAFC
	public static Color GetColor(CameraLayer cameraLayer, int subLayer)
	{
		Color result = Color.white;
		if (cameraLayer != CameraLayer.Background_Far_PERSP && cameraLayer != CameraLayer.Background_Near_PERSP && cameraLayer != CameraLayer.Foreground_PERSP)
		{
			if (cameraLayer != CameraLayer.Game)
			{
				if (cameraLayer != CameraLayer.Foreground_ORTHO)
				{
					if (cameraLayer == CameraLayer.Background_ORTHO)
					{
						result = PropSpawnControllerUtility.Instance.m_colors.BackgroundOrthoColor;
					}
				}
				else
				{
					result = PropSpawnControllerUtility.Instance.m_colors.ForegroundOrthoColor;
				}
			}
			else
			{
				result = PropSpawnControllerUtility.Instance.m_colors.GameColor;
			}
		}
		else
		{
			Vector2Int vector2Int = Vector2Int.zero;
			Color a;
			Color b;
			if (cameraLayer == CameraLayer.Background_Far_PERSP)
			{
				vector2Int = CameraLayerUtility.GetSubLayerRange(CameraLayer.Background_Far_PERSP);
				a = PropSpawnControllerUtility.Instance.m_colors.BackgroundPerspFarMaxColor;
				b = PropSpawnControllerUtility.Instance.m_colors.BackgroundPerspFarMinColor;
			}
			else if (cameraLayer == CameraLayer.Background_Near_PERSP)
			{
				vector2Int = CameraLayerUtility.GetSubLayerRange(CameraLayer.Background_Near_PERSP);
				a = PropSpawnControllerUtility.Instance.m_colors.BackgroundPerspNearMaxColor;
				b = PropSpawnControllerUtility.Instance.m_colors.BackgroundPerspNearMinColor;
			}
			else
			{
				vector2Int = CameraLayerUtility.GetSubLayerRange(CameraLayer.Foreground_PERSP);
				a = PropSpawnControllerUtility.Instance.m_colors.ForegroundPerspMaxColor;
				b = PropSpawnControllerUtility.Instance.m_colors.ForegroundPerspMinColor;
			}
			result = Color.Lerp(a, b, (float)subLayer / (float)(vector2Int.y - vector2Int.x - 1));
		}
		return result;
	}

	// Token: 0x06005D9A RID: 23962 RVA: 0x00033827 File Offset: 0x00031A27
	public static Vector2Int GetGameSubLayerRange(CameraLayer cameraLayer)
	{
		return CameraLayerUtility.GetSubLayerRange(cameraLayer);
	}

	// Token: 0x06005D9B RID: 23963 RVA: 0x0015EA18 File Offset: 0x0015CC18
	public static int GetSortingLayerID(CameraLayer cameraLayer)
	{
		if (cameraLayer <= CameraLayer.Foreground_ORTHO)
		{
			switch (cameraLayer)
			{
			case CameraLayer.Game:
				return SortingLayer.NameToID("GameEditorProps");
			case CameraLayer.Foreground_PERSP:
				return SortingLayer.NameToID("ForegroundPerspEditorProps");
			case (CameraLayer)3:
				break;
			case CameraLayer.Background_Far_PERSP:
				return SortingLayer.NameToID("BackgroundPerspFarEditorProps");
			default:
				if (cameraLayer == CameraLayer.Foreground_ORTHO)
				{
					return SortingLayer.NameToID("ForegroundEditorProps");
				}
				break;
			}
		}
		else
		{
			if (cameraLayer == CameraLayer.Background_ORTHO)
			{
				return SortingLayer.NameToID("BackgroundEditorProps");
			}
			if (cameraLayer == CameraLayer.Background_Near_PERSP)
			{
				return SortingLayer.NameToID("BackgroundPerspNearEditorProps");
			}
		}
		return SortingLayer.NameToID("GameEditorProps");
	}

	// Token: 0x06005D9C RID: 23964 RVA: 0x0002B621 File Offset: 0x00029821
	public static int GetSortOrder(CameraLayer cameraLayer, int subLayer)
	{
		return subLayer;
	}

	// Token: 0x06005D9D RID: 23965 RVA: 0x0015EAA4 File Offset: 0x0015CCA4
	public static float GetZPosition(CameraLayer cameraLayer, int subLayer)
	{
		if (cameraLayer == CameraLayer.Foreground_ORTHO || cameraLayer == CameraLayer.Foreground_PERSP || (cameraLayer == CameraLayer.Game && subLayer > 0))
		{
			return PropSpawnControllerUtility.Instance.m_sortOrders.ForegroundZPosition;
		}
		if (cameraLayer == CameraLayer.Background_ORTHO || cameraLayer == CameraLayer.Background_Far_PERSP || cameraLayer == CameraLayer.Background_Near_PERSP || (cameraLayer == CameraLayer.Game && subLayer < 0))
		{
			return PropSpawnControllerUtility.Instance.m_sortOrders.BackgroundZPosition;
		}
		return PropSpawnControllerUtility.Instance.m_sortOrders.DefaultZPosition;
	}

	// Token: 0x04004CF4 RID: 19700
	[SerializeField]
	private PropSpawnControllerUtility.PropSpawnControllerColors m_colors;

	// Token: 0x04004CF5 RID: 19701
	[SerializeField]
	private PropSpawnControllerUtility.PropSpawnControllerSortOrder m_sortOrders;

	// Token: 0x04004CF6 RID: 19702
	private const string GAME_SORTING_LAYER_NAME = "GameEditorProps";

	// Token: 0x04004CF7 RID: 19703
	private const string FOREGROUND_SORTING_LAYER_NAME = "ForegroundEditorProps";

	// Token: 0x04004CF8 RID: 19704
	private const string BACKGROUND_SORTING_LAYER_NAME = "BackgroundEditorProps";

	// Token: 0x04004CF9 RID: 19705
	private const string FOREGROUND_PERSP_SORTING_LAYER_NAME = "ForegroundPerspEditorProps";

	// Token: 0x04004CFA RID: 19706
	private const string BACKGROUND_PERSP_NEAR_SORTING_LAYER_NAME = "BackgroundPerspNearEditorProps";

	// Token: 0x04004CFB RID: 19707
	private const string BACKGROUND_PERSP_FAR_SORTING_LAYER_NAME = "BackgroundPerspFarEditorProps";

	// Token: 0x04004CFC RID: 19708
	private const string RESOURCES_PATH = "Scriptable Objects/PropSpawnControllerUtility";

	// Token: 0x04004CFD RID: 19709
	public const string EDITOR_PATH = "Assets/Content/Scriptable Objects/PropSpawnControllerUtility.asset";

	// Token: 0x04004CFE RID: 19710
	private static PropSpawnControllerUtility m_instance;

	// Token: 0x02000CD3 RID: 3283
	[Serializable]
	private class PropSpawnControllerColors
	{
		// Token: 0x04004CFF RID: 19711
		public Color BackgroundPerspFarMaxColor = new Color(0.6f, 0.9f, 0.8f);

		// Token: 0x04004D00 RID: 19712
		public Color BackgroundPerspFarMinColor = new Color(0.4f, 0.6f, 0.8f);

		// Token: 0x04004D01 RID: 19713
		public Color BackgroundPerspNearMaxColor = new Color(0.6f, 0.9f, 0.8f);

		// Token: 0x04004D02 RID: 19714
		public Color BackgroundPerspNearMinColor = new Color(0.4f, 0.6f, 0.8f);

		// Token: 0x04004D03 RID: 19715
		public Color BackgroundOrthoColor = new Color(0.8f, 0.6f, 0.6f);

		// Token: 0x04004D04 RID: 19716
		public Color GameColor = new Color(0.53f, 0.05f, 0.63f);

		// Token: 0x04004D05 RID: 19717
		public Color ForegroundOrthoColor = new Color(0.55f, 0.55f, 0.6f);

		// Token: 0x04004D06 RID: 19718
		public Color ForegroundPerspMaxColor = new Color(0.4f, 0.4f, 0.45f);

		// Token: 0x04004D07 RID: 19719
		public Color ForegroundPerspMinColor = new Color(0f, 0f, 0.1f);
	}

	// Token: 0x02000CD4 RID: 3284
	[Serializable]
	private class PropSpawnControllerSortOrder
	{
		// Token: 0x04004D08 RID: 19720
		public float DefaultZPosition = -0.5f;

		// Token: 0x04004D09 RID: 19721
		public float BackgroundZPosition = 1f;

		// Token: 0x04004D0A RID: 19722
		public float ForegroundZPosition = -1f;
	}
}
