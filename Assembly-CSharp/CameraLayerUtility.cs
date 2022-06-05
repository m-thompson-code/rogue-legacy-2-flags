using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000342 RID: 834
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Camera Layer Utility")]
public class CameraLayerUtility : ScriptableObject
{
	// Token: 0x17000CD2 RID: 3282
	// (get) Token: 0x06001AEE RID: 6894 RVA: 0x0000DF76 File Offset: 0x0000C176
	public static int DefaultEnemySubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultEnemySubLayer;
		}
	}

	// Token: 0x17000CD3 RID: 3283
	// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0000DF82 File Offset: 0x0000C182
	public static int DefaultPlayerSubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultPlayerSubLayer;
		}
	}

	// Token: 0x17000CD4 RID: 3284
	// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0000DF8E File Offset: 0x0000C18E
	public static int DefaultProjectileSubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultProjectileSubLayer;
		}
	}

	// Token: 0x17000CD5 RID: 3285
	// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x000935A0 File Offset: 0x000917A0
	private static CameraLayerUtility Instance
	{
		get
		{
			if (CameraLayerUtility.m_instance == null)
			{
				if (Application.isPlaying)
				{
					CameraLayerUtility.m_instance = CDGResources.Load<CameraLayerUtility>("Scriptable Objects/CameraLayerUtility", "", true);
				}
				if (CameraLayerUtility.m_instance == null)
				{
					Debug.LogFormat("<color=red>| CameraLayerUtility | Failed to find CameraLayerUtility SO at Path ({0})</color>", new object[]
					{
						"Scriptable Objects/CameraLayerUtility"
					});
				}
			}
			return CameraLayerUtility.m_instance;
		}
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x00093600 File Offset: 0x00091800
	private static float GetForegroundOrthoOffset(int order)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_foregroundSubLayerRange.x, CameraLayerUtility.Instance.m_foregroundSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_backgroundSubLayerRange.y - CameraLayerUtility.Instance.m_backgroundSubLayerRange.x;
		if (CameraLayerUtility.Instance.m_distanceBetweenForegroundOrthoSubLayers == -1f)
		{
			CameraLayerUtility.Instance.m_distanceBetweenForegroundOrthoSubLayers = (CameraLayerUtility.Instance.m_foregroundOrthoZPositionRange.y - CameraLayerUtility.Instance.m_foregroundOrthoZPositionRange.x) / (float)num;
		}
		return (float)(num - 1 - order) * CameraLayerUtility.Instance.m_distanceBetweenForegroundOrthoSubLayers;
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000936A4 File Offset: 0x000918A4
	private static float GetForegroundPerspOffset(int order)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_foregroundSubLayerRange.x, CameraLayerUtility.Instance.m_foregroundSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_backgroundSubLayerRange.y - CameraLayerUtility.Instance.m_backgroundSubLayerRange.x;
		if (CameraLayerUtility.Instance.m_distanceBetweenForegroundPerspSubLayers == -1f)
		{
			CameraLayerUtility.Instance.m_distanceBetweenForegroundPerspSubLayers = (CameraLayerUtility.Instance.m_foregroundPerspZPositionRange.y - CameraLayerUtility.Instance.m_foregroundPerspZPositionRange.x) / (float)num;
		}
		return (float)(num - 1 - order) * CameraLayerUtility.Instance.m_distanceBetweenForegroundPerspSubLayers;
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x00093748 File Offset: 0x00091948
	private static float GetGameOffset(int order, bool isProp)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_gameSubLayerRange.x, CameraLayerUtility.Instance.m_gameSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_gameSubLayerRange.y - CameraLayerUtility.Instance.m_gameSubLayerRange.x;
		float num2;
		if (isProp)
		{
			if (CameraLayerUtility.Instance.m_distanceBetweenPropGameSubLayers == -1f)
			{
				CameraLayerUtility.Instance.m_distanceBetweenPropGameSubLayers = (CameraLayerUtility.Instance.m_propGameZPositionRange.y - CameraLayerUtility.Instance.m_propGameZPositionRange.x) / (float)num;
			}
			num2 = CameraLayerUtility.Instance.m_distanceBetweenPropGameSubLayers;
		}
		else
		{
			if (CameraLayerUtility.Instance.m_distanceBetweenGameSubLayers == -1f)
			{
				CameraLayerUtility.Instance.m_distanceBetweenGameSubLayers = (CameraLayerUtility.Instance.m_gameZPositionRange.y - CameraLayerUtility.Instance.m_gameZPositionRange.x) / (float)num;
			}
			num2 = CameraLayerUtility.Instance.m_distanceBetweenGameSubLayers;
		}
		return (float)(-1 * order) * num2;
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x0009383C File Offset: 0x00091A3C
	private static float GetBackgroundFarOffset(int order)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_backgroundSubLayerRange.x, CameraLayerUtility.Instance.m_backgroundSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_backgroundSubLayerRange.y - CameraLayerUtility.Instance.m_backgroundSubLayerRange.x;
		if (CameraLayerUtility.Instance.m_distanceBetweenBackgroundFarSubLayers == -1f)
		{
			CameraLayerUtility.Instance.m_distanceBetweenBackgroundFarSubLayers = (CameraLayerUtility.Instance.m_backgroundPerspFarZPositionRange.y - CameraLayerUtility.Instance.m_backgroundPerspFarZPositionRange.x) / (float)num;
		}
		return (float)(num - 1 - order) * CameraLayerUtility.Instance.m_distanceBetweenBackgroundFarSubLayers;
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000938E0 File Offset: 0x00091AE0
	private static float GetBackgroundNearOffset(int order)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_backgroundSubLayerRange.x, CameraLayerUtility.Instance.m_backgroundSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_backgroundSubLayerRange.y - CameraLayerUtility.Instance.m_backgroundSubLayerRange.x;
		if (CameraLayerUtility.Instance.m_distanceBetweenBackgroundNearSubLayers == -1f)
		{
			CameraLayerUtility.Instance.m_distanceBetweenBackgroundNearSubLayers = (CameraLayerUtility.Instance.m_backgroundPerspNearZPositionRange.y - CameraLayerUtility.Instance.m_backgroundPerspNearZPositionRange.x) / (float)num;
		}
		return (float)(num - 1 - order) * CameraLayerUtility.Instance.m_distanceBetweenBackgroundNearSubLayers;
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x00093984 File Offset: 0x00091B84
	private static float GetBackgroundOrthoOffset(int order)
	{
		order = Mathf.Clamp(order, CameraLayerUtility.Instance.m_backgroundSubLayerRange.x, CameraLayerUtility.Instance.m_backgroundSubLayerRange.y);
		int num = CameraLayerUtility.Instance.m_backgroundSubLayerRange.y - CameraLayerUtility.Instance.m_backgroundSubLayerRange.x;
		if (CameraLayerUtility.Instance.m_distanceBetweenBackgroundOrthoSubLayers == -1f)
		{
			CameraLayerUtility.Instance.m_distanceBetweenBackgroundOrthoSubLayers = (CameraLayerUtility.Instance.m_backgroundOrthoZPositionRange.y - CameraLayerUtility.Instance.m_backgroundOrthoZPositionRange.x) / (float)num;
		}
		return (float)(num - 1 - order) * CameraLayerUtility.Instance.m_distanceBetweenBackgroundOrthoSubLayers;
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x00093A28 File Offset: 0x00091C28
	public static Vector2Int GetSubLayerRange(CameraLayer cameraLayer)
	{
		if (cameraLayer <= CameraLayer.Foreground_ORTHO)
		{
			switch (cameraLayer)
			{
			case CameraLayer.Game:
				return CameraLayerUtility.Instance.m_gameSubLayerRange;
			case CameraLayer.Foreground_PERSP:
				break;
			case (CameraLayer)3:
				goto IL_51;
			case CameraLayer.Background_Far_PERSP:
				goto IL_46;
			default:
				if (cameraLayer != CameraLayer.Foreground_ORTHO)
				{
					goto IL_51;
				}
				break;
			}
			return CameraLayerUtility.Instance.m_foregroundSubLayerRange;
		}
		if (cameraLayer != CameraLayer.Background_ORTHO && cameraLayer != CameraLayer.Background_Near_PERSP)
		{
			goto IL_51;
		}
		IL_46:
		return CameraLayerUtility.Instance.m_foregroundSubLayerRange;
		IL_51:
		return Vector2Int.zero;
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x00093A8C File Offset: 0x00091C8C
	public static LayerMask GetLayer(CameraLayer cameraLayer)
	{
		if (cameraLayer <= CameraLayer.Foreground_ORTHO)
		{
			switch (cameraLayer)
			{
			case CameraLayer.Game:
				return LayerMask.NameToLayer("Default");
			case CameraLayer.Foreground_PERSP:
				return LayerMask.NameToLayer("Foreground_Persp");
			case (CameraLayer)3:
				break;
			case CameraLayer.Background_Far_PERSP:
				return LayerMask.NameToLayer("Background_Persp_Far");
			default:
				if (cameraLayer == CameraLayer.ForegroundLights)
				{
					return LayerMask.NameToLayer("Foreground_Lights");
				}
				if (cameraLayer == CameraLayer.Foreground_ORTHO)
				{
					return LayerMask.NameToLayer("Foreground_Ortho");
				}
				break;
			}
		}
		else
		{
			if (cameraLayer == CameraLayer.Background_ORTHO)
			{
				return LayerMask.NameToLayer("Background_Ortho");
			}
			if (cameraLayer == CameraLayer.Background_Near_PERSP || cameraLayer == CameraLayer.Background_Wall)
			{
				return LayerMask.NameToLayer("Background_Persp_Near");
			}
		}
		throw new ArgumentOutOfRangeException("cameraLayer", string.Format("No case for Camera Layer ({0})", cameraLayer));
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x00093B64 File Offset: 0x00091D64
	public static float GetTuckYOffset(CameraLayer cameraLayer, int subLayer)
	{
		float zdepth = CameraLayerUtility.GetZDepth(cameraLayer, subLayer, false, false);
		float zdepth2 = CameraLayerUtility.GetZDepth(CameraLayer.Background_Near_PERSP, 0, false, false);
		float t = zdepth / zdepth2;
		return Mathf.Lerp(CameraLayerUtility.Instance.m_defaultCameraSizeTuckRange.x, CameraLayerUtility.Instance.m_defaultCameraSizeTuckRange.y, t);
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x00093BAC File Offset: 0x00091DAC
	public static int GetSpriteOrderInLayer(CameraLayer cameraLayer, int order)
	{
		if (CameraLayerUtility.m_zPositionTable == null)
		{
			CameraLayerUtility.m_zPositionTable = new Dictionary<CameraLayerUtility.LayerID, int>();
		}
		CameraLayerUtility.LayerID layerID = new CameraLayerUtility.LayerID(cameraLayer, order);
		if (CameraLayerUtility.m_zPositionTable.ContainsKey(layerID))
		{
			Dictionary<CameraLayerUtility.LayerID, int> zPositionTable = CameraLayerUtility.m_zPositionTable;
			CameraLayerUtility.LayerID key = layerID;
			int num = zPositionTable[key];
			zPositionTable[key] = num + 1;
		}
		else
		{
			CameraLayerUtility.m_zPositionTable.Add(layerID, 0);
		}
		return CameraLayerUtility.m_zPositionTable[layerID];
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x00093C14 File Offset: 0x00091E14
	public static float GetZDepth(CameraLayer cameraLayer, int order, bool isProp, bool isDeco = false)
	{
		float num = 0f;
		float num2 = 1f;
		float num3;
		if (cameraLayer <= CameraLayer.Foreground_ORTHO)
		{
			switch (cameraLayer)
			{
			case CameraLayer.Game:
				if (CameraLayerUtility.Instance.m_gameInitialZPosition == -1f)
				{
					CameraLayerUtility.Instance.m_gameInitialZPosition = CameraLayerUtility.Instance.m_gameZPositionRange.y - (CameraLayerUtility.Instance.m_gameZPositionRange.y - CameraLayerUtility.Instance.m_gameZPositionRange.x) / 2f;
				}
				num = CameraLayerUtility.Instance.m_gameInitialZPosition;
				num3 = CameraLayerUtility.GetGameOffset(order, isProp);
				goto IL_192;
			case CameraLayer.Foreground_PERSP:
				num = CameraLayerUtility.Instance.m_foregroundPerspZPositionRange.x;
				num3 = CameraLayerUtility.GetForegroundPerspOffset(order);
				goto IL_192;
			case (CameraLayer)3:
				break;
			case CameraLayer.Background_Far_PERSP:
				num = CameraLayerUtility.Instance.m_backgroundPerspFarZPositionRange.x;
				num3 = CameraLayerUtility.GetBackgroundFarOffset(order);
				goto IL_192;
			default:
				if (cameraLayer == CameraLayer.ForegroundLights)
				{
					num3 = (float)(-1 * order) * CameraLayerUtility.Instance.m_distanceBetweenForegroundPerspSubLayers;
					goto IL_192;
				}
				if (cameraLayer == CameraLayer.Foreground_ORTHO)
				{
					num = CameraLayerUtility.Instance.m_foregroundOrthoZPositionRange.x;
					num3 = CameraLayerUtility.GetForegroundOrthoOffset(order);
					goto IL_192;
				}
				break;
			}
		}
		else
		{
			if (cameraLayer == CameraLayer.Background_ORTHO)
			{
				num = CameraLayerUtility.Instance.m_backgroundOrthoZPositionRange.x;
				num3 = CameraLayerUtility.GetBackgroundOrthoOffset(order);
				goto IL_192;
			}
			if (cameraLayer == CameraLayer.Background_Near_PERSP)
			{
				num = CameraLayerUtility.Instance.m_backgroundPerspNearZPositionRange.x;
				num3 = CameraLayerUtility.GetBackgroundNearOffset(order);
				goto IL_192;
			}
			if (cameraLayer == CameraLayer.Background_Wall)
			{
				num = CameraLayerUtility.Instance.m_backgroundPerspNearZPositionRange.y;
				num3 = 0f;
				goto IL_192;
			}
		}
		throw new ArgumentOutOfRangeException("cameraLayer: " + cameraLayer.ToString());
		IL_192:
		float num4 = 0f;
		if (isDeco)
		{
			num4 = -1E-05f;
		}
		return num + num2 * num3 + num4;
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x00093DCC File Offset: 0x00091FCC
	private void Reset()
	{
		if (Application.isPlaying)
		{
			this.m_gameInitialZPosition = -1f;
			this.m_distanceBetweenGameSubLayers = -1f;
			this.m_distanceBetweenBackgroundFarSubLayers = -1f;
			this.m_distanceBetweenBackgroundNearSubLayers = -1f;
			this.m_distanceBetweenForegroundPerspSubLayers = -1f;
			this.m_distanceBetweenForegroundOrthoSubLayers = -1f;
			this.m_distanceBetweenBackgroundOrthoSubLayers = -1f;
		}
	}

	// Token: 0x04001907 RID: 6407
	[Header("Game Layer Settings")]
	[SerializeField]
	private Vector2Int m_gameSubLayerRange = new Vector2Int(-500, 500);

	// Token: 0x04001908 RID: 6408
	[SerializeField]
	private Vector2 m_gameZPositionRange = new Vector2(-10f, 10f);

	// Token: 0x04001909 RID: 6409
	[SerializeField]
	private Vector2 m_propGameZPositionRange = new Vector2(-1f, 1f);

	// Token: 0x0400190A RID: 6410
	[Header("Background Settings")]
	[SerializeField]
	private Vector2Int m_backgroundSubLayerRange = new Vector2Int(0, 500);

	// Token: 0x0400190B RID: 6411
	[SerializeField]
	private Vector2 m_backgroundPerspNearZPositionRange = new Vector2(0.5f, 10f);

	// Token: 0x0400190C RID: 6412
	[SerializeField]
	private Vector2 m_backgroundPerspFarZPositionRange = new Vector2(20f, 50f);

	// Token: 0x0400190D RID: 6413
	[SerializeField]
	private Vector2 m_backgroundOrthoZPositionRange = new Vector2(0.5f, 5f);

	// Token: 0x0400190E RID: 6414
	[Header("Foreground Settings")]
	[SerializeField]
	private Vector2Int m_foregroundSubLayerRange = new Vector2Int(0, 500);

	// Token: 0x0400190F RID: 6415
	[SerializeField]
	private Vector2 m_foregroundPerspZPositionRange = new Vector2(-5f, -0.5f);

	// Token: 0x04001910 RID: 6416
	[SerializeField]
	private Vector2 m_foregroundOrthoZPositionRange = new Vector2(-5f, -0.5f);

	// Token: 0x04001911 RID: 6417
	[Header("Vertical Tuck Settings")]
	[SerializeField]
	private Vector2 m_defaultCameraSizeTuckRange = new Vector2(-0.25f, -0.5f);

	// Token: 0x04001912 RID: 6418
	[Header("Default Sub Layers")]
	[SerializeField]
	private int m_defaultPlayerSubLayer = 5000;

	// Token: 0x04001913 RID: 6419
	[SerializeField]
	private int m_defaultEnemySubLayer = 2000;

	// Token: 0x04001914 RID: 6420
	[SerializeField]
	private int m_defaultProjectileSubLayer = 9000;

	// Token: 0x04001915 RID: 6421
	private float m_distanceBetweenForegroundPerspSubLayers = -1f;

	// Token: 0x04001916 RID: 6422
	private float m_distanceBetweenBackgroundNearSubLayers = -1f;

	// Token: 0x04001917 RID: 6423
	private float m_distanceBetweenForegroundOrthoSubLayers = -1f;

	// Token: 0x04001918 RID: 6424
	private float m_distanceBetweenBackgroundOrthoSubLayers = -1f;

	// Token: 0x04001919 RID: 6425
	private float m_distanceBetweenBackgroundFarSubLayers = -1f;

	// Token: 0x0400191A RID: 6426
	private float m_gameInitialZPosition = -1f;

	// Token: 0x0400191B RID: 6427
	private float m_distanceBetweenGameSubLayers = -1f;

	// Token: 0x0400191C RID: 6428
	private float m_distanceBetweenPropGameSubLayers = -1f;

	// Token: 0x0400191D RID: 6429
	private static CameraLayerUtility m_instance;

	// Token: 0x0400191E RID: 6430
	private const float DECO_OFFSET = -1E-05f;

	// Token: 0x0400191F RID: 6431
	private const string RESOURCES_PATH = "Scriptable Objects/CameraLayerUtility";

	// Token: 0x04001920 RID: 6432
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/CameraLayerUtility.asset";

	// Token: 0x04001921 RID: 6433
	private static Dictionary<CameraLayerUtility.LayerID, int> m_zPositionTable;

	// Token: 0x02000343 RID: 835
	[Serializable]
	private class BiomeSubLayerMultiplierEntry
	{
		// Token: 0x04001922 RID: 6434
		public BiomeType Biome;

		// Token: 0x04001923 RID: 6435
		public float ForegroundMultiplier = 1f;

		// Token: 0x04001924 RID: 6436
		public float BackgroundMultiplier = 1f;
	}

	// Token: 0x02000344 RID: 836
	private struct LayerID
	{
		// Token: 0x06001B01 RID: 6913 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		public LayerID(CameraLayer cameraLayer, int subLayer)
		{
			this.CameraLayer = cameraLayer;
			this.SubLayer = subLayer;
		}

		// Token: 0x04001925 RID: 6437
		public CameraLayer CameraLayer;

		// Token: 0x04001926 RID: 6438
		public int SubLayer;
	}
}
