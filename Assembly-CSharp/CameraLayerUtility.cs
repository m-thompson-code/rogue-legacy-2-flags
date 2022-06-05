using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CA RID: 458
[CreateAssetMenu(menuName = "Custom/Rogue Legacy 2/Camera Layer Utility")]
public class CameraLayerUtility : ScriptableObject
{
	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x06001272 RID: 4722 RVA: 0x00035D3E File Offset: 0x00033F3E
	public static int DefaultEnemySubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultEnemySubLayer;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06001273 RID: 4723 RVA: 0x00035D4A File Offset: 0x00033F4A
	public static int DefaultPlayerSubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultPlayerSubLayer;
		}
	}

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06001274 RID: 4724 RVA: 0x00035D56 File Offset: 0x00033F56
	public static int DefaultProjectileSubLayer
	{
		get
		{
			return CameraLayerUtility.Instance.m_defaultProjectileSubLayer;
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06001275 RID: 4725 RVA: 0x00035D64 File Offset: 0x00033F64
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

	// Token: 0x06001276 RID: 4726 RVA: 0x00035DC4 File Offset: 0x00033FC4
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

	// Token: 0x06001277 RID: 4727 RVA: 0x00035E68 File Offset: 0x00034068
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

	// Token: 0x06001278 RID: 4728 RVA: 0x00035F0C File Offset: 0x0003410C
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

	// Token: 0x06001279 RID: 4729 RVA: 0x00036000 File Offset: 0x00034200
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

	// Token: 0x0600127A RID: 4730 RVA: 0x000360A4 File Offset: 0x000342A4
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

	// Token: 0x0600127B RID: 4731 RVA: 0x00036148 File Offset: 0x00034348
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

	// Token: 0x0600127C RID: 4732 RVA: 0x000361EC File Offset: 0x000343EC
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

	// Token: 0x0600127D RID: 4733 RVA: 0x00036250 File Offset: 0x00034450
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

	// Token: 0x0600127E RID: 4734 RVA: 0x00036328 File Offset: 0x00034528
	public static float GetTuckYOffset(CameraLayer cameraLayer, int subLayer)
	{
		float zdepth = CameraLayerUtility.GetZDepth(cameraLayer, subLayer, false, false);
		float zdepth2 = CameraLayerUtility.GetZDepth(CameraLayer.Background_Near_PERSP, 0, false, false);
		float t = zdepth / zdepth2;
		return Mathf.Lerp(CameraLayerUtility.Instance.m_defaultCameraSizeTuckRange.x, CameraLayerUtility.Instance.m_defaultCameraSizeTuckRange.y, t);
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00036370 File Offset: 0x00034570
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

	// Token: 0x06001280 RID: 4736 RVA: 0x000363D8 File Offset: 0x000345D8
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

	// Token: 0x06001281 RID: 4737 RVA: 0x00036590 File Offset: 0x00034790
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

	// Token: 0x040012D9 RID: 4825
	[Header("Game Layer Settings")]
	[SerializeField]
	private Vector2Int m_gameSubLayerRange = new Vector2Int(-500, 500);

	// Token: 0x040012DA RID: 4826
	[SerializeField]
	private Vector2 m_gameZPositionRange = new Vector2(-10f, 10f);

	// Token: 0x040012DB RID: 4827
	[SerializeField]
	private Vector2 m_propGameZPositionRange = new Vector2(-1f, 1f);

	// Token: 0x040012DC RID: 4828
	[Header("Background Settings")]
	[SerializeField]
	private Vector2Int m_backgroundSubLayerRange = new Vector2Int(0, 500);

	// Token: 0x040012DD RID: 4829
	[SerializeField]
	private Vector2 m_backgroundPerspNearZPositionRange = new Vector2(0.5f, 10f);

	// Token: 0x040012DE RID: 4830
	[SerializeField]
	private Vector2 m_backgroundPerspFarZPositionRange = new Vector2(20f, 50f);

	// Token: 0x040012DF RID: 4831
	[SerializeField]
	private Vector2 m_backgroundOrthoZPositionRange = new Vector2(0.5f, 5f);

	// Token: 0x040012E0 RID: 4832
	[Header("Foreground Settings")]
	[SerializeField]
	private Vector2Int m_foregroundSubLayerRange = new Vector2Int(0, 500);

	// Token: 0x040012E1 RID: 4833
	[SerializeField]
	private Vector2 m_foregroundPerspZPositionRange = new Vector2(-5f, -0.5f);

	// Token: 0x040012E2 RID: 4834
	[SerializeField]
	private Vector2 m_foregroundOrthoZPositionRange = new Vector2(-5f, -0.5f);

	// Token: 0x040012E3 RID: 4835
	[Header("Vertical Tuck Settings")]
	[SerializeField]
	private Vector2 m_defaultCameraSizeTuckRange = new Vector2(-0.25f, -0.5f);

	// Token: 0x040012E4 RID: 4836
	[Header("Default Sub Layers")]
	[SerializeField]
	private int m_defaultPlayerSubLayer = 5000;

	// Token: 0x040012E5 RID: 4837
	[SerializeField]
	private int m_defaultEnemySubLayer = 2000;

	// Token: 0x040012E6 RID: 4838
	[SerializeField]
	private int m_defaultProjectileSubLayer = 9000;

	// Token: 0x040012E7 RID: 4839
	private float m_distanceBetweenForegroundPerspSubLayers = -1f;

	// Token: 0x040012E8 RID: 4840
	private float m_distanceBetweenBackgroundNearSubLayers = -1f;

	// Token: 0x040012E9 RID: 4841
	private float m_distanceBetweenForegroundOrthoSubLayers = -1f;

	// Token: 0x040012EA RID: 4842
	private float m_distanceBetweenBackgroundOrthoSubLayers = -1f;

	// Token: 0x040012EB RID: 4843
	private float m_distanceBetweenBackgroundFarSubLayers = -1f;

	// Token: 0x040012EC RID: 4844
	private float m_gameInitialZPosition = -1f;

	// Token: 0x040012ED RID: 4845
	private float m_distanceBetweenGameSubLayers = -1f;

	// Token: 0x040012EE RID: 4846
	private float m_distanceBetweenPropGameSubLayers = -1f;

	// Token: 0x040012EF RID: 4847
	private static CameraLayerUtility m_instance;

	// Token: 0x040012F0 RID: 4848
	private const float DECO_OFFSET = -1E-05f;

	// Token: 0x040012F1 RID: 4849
	private const string RESOURCES_PATH = "Scriptable Objects/CameraLayerUtility";

	// Token: 0x040012F2 RID: 4850
	public const string ASSET_PATH = "Assets/Content/Scriptable Objects/CameraLayerUtility.asset";

	// Token: 0x040012F3 RID: 4851
	private static Dictionary<CameraLayerUtility.LayerID, int> m_zPositionTable;

	// Token: 0x02000AF1 RID: 2801
	[Serializable]
	private class BiomeSubLayerMultiplierEntry
	{
		// Token: 0x04004AC3 RID: 19139
		public BiomeType Biome;

		// Token: 0x04004AC4 RID: 19140
		public float ForegroundMultiplier = 1f;

		// Token: 0x04004AC5 RID: 19141
		public float BackgroundMultiplier = 1f;
	}

	// Token: 0x02000AF2 RID: 2802
	private struct LayerID
	{
		// Token: 0x06005AEA RID: 23274 RVA: 0x00157CAD File Offset: 0x00155EAD
		public LayerID(CameraLayer cameraLayer, int subLayer)
		{
			this.CameraLayer = cameraLayer;
			this.SubLayer = subLayer;
		}

		// Token: 0x04004AC6 RID: 19142
		public CameraLayer CameraLayer;

		// Token: 0x04004AC7 RID: 19143
		public int SubLayer;
	}
}
