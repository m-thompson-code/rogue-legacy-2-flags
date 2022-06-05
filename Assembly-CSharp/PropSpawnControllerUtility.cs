using System;
using UnityEngine;

// Token: 0x0200080A RID: 2058
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Prop Spawn Controller Utility")]
public class PropSpawnControllerUtility : ScriptableObject
{
	// Token: 0x170016F0 RID: 5872
	// (get) Token: 0x0600440F RID: 17423 RVA: 0x000F0D44 File Offset: 0x000EEF44
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

	// Token: 0x06004410 RID: 17424 RVA: 0x000F0DA4 File Offset: 0x000EEFA4
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

	// Token: 0x06004411 RID: 17425 RVA: 0x000F0EBD File Offset: 0x000EF0BD
	public static Vector2Int GetGameSubLayerRange(CameraLayer cameraLayer)
	{
		return CameraLayerUtility.GetSubLayerRange(cameraLayer);
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x000F0EC8 File Offset: 0x000EF0C8
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

	// Token: 0x06004413 RID: 17427 RVA: 0x000F0F51 File Offset: 0x000EF151
	public static int GetSortOrder(CameraLayer cameraLayer, int subLayer)
	{
		return subLayer;
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x000F0F54 File Offset: 0x000EF154
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

	// Token: 0x04003A28 RID: 14888
	[SerializeField]
	private PropSpawnControllerUtility.PropSpawnControllerColors m_colors;

	// Token: 0x04003A29 RID: 14889
	[SerializeField]
	private PropSpawnControllerUtility.PropSpawnControllerSortOrder m_sortOrders;

	// Token: 0x04003A2A RID: 14890
	private const string GAME_SORTING_LAYER_NAME = "GameEditorProps";

	// Token: 0x04003A2B RID: 14891
	private const string FOREGROUND_SORTING_LAYER_NAME = "ForegroundEditorProps";

	// Token: 0x04003A2C RID: 14892
	private const string BACKGROUND_SORTING_LAYER_NAME = "BackgroundEditorProps";

	// Token: 0x04003A2D RID: 14893
	private const string FOREGROUND_PERSP_SORTING_LAYER_NAME = "ForegroundPerspEditorProps";

	// Token: 0x04003A2E RID: 14894
	private const string BACKGROUND_PERSP_NEAR_SORTING_LAYER_NAME = "BackgroundPerspNearEditorProps";

	// Token: 0x04003A2F RID: 14895
	private const string BACKGROUND_PERSP_FAR_SORTING_LAYER_NAME = "BackgroundPerspFarEditorProps";

	// Token: 0x04003A30 RID: 14896
	private const string RESOURCES_PATH = "Scriptable Objects/PropSpawnControllerUtility";

	// Token: 0x04003A31 RID: 14897
	public const string EDITOR_PATH = "Assets/Content/Scriptable Objects/PropSpawnControllerUtility.asset";

	// Token: 0x04003A32 RID: 14898
	private static PropSpawnControllerUtility m_instance;

	// Token: 0x02000E3A RID: 3642
	[Serializable]
	private class PropSpawnControllerColors
	{
		// Token: 0x04005742 RID: 22338
		public Color BackgroundPerspFarMaxColor = new Color(0.6f, 0.9f, 0.8f);

		// Token: 0x04005743 RID: 22339
		public Color BackgroundPerspFarMinColor = new Color(0.4f, 0.6f, 0.8f);

		// Token: 0x04005744 RID: 22340
		public Color BackgroundPerspNearMaxColor = new Color(0.6f, 0.9f, 0.8f);

		// Token: 0x04005745 RID: 22341
		public Color BackgroundPerspNearMinColor = new Color(0.4f, 0.6f, 0.8f);

		// Token: 0x04005746 RID: 22342
		public Color BackgroundOrthoColor = new Color(0.8f, 0.6f, 0.6f);

		// Token: 0x04005747 RID: 22343
		public Color GameColor = new Color(0.53f, 0.05f, 0.63f);

		// Token: 0x04005748 RID: 22344
		public Color ForegroundOrthoColor = new Color(0.55f, 0.55f, 0.6f);

		// Token: 0x04005749 RID: 22345
		public Color ForegroundPerspMaxColor = new Color(0.4f, 0.4f, 0.45f);

		// Token: 0x0400574A RID: 22346
		public Color ForegroundPerspMinColor = new Color(0f, 0f, 0.1f);
	}

	// Token: 0x02000E3B RID: 3643
	[Serializable]
	private class PropSpawnControllerSortOrder
	{
		// Token: 0x0400574B RID: 22347
		public float DefaultZPosition = -0.5f;

		// Token: 0x0400574C RID: 22348
		public float BackgroundZPosition = 1f;

		// Token: 0x0400574D RID: 22349
		public float ForegroundZPosition = -1f;
	}
}
