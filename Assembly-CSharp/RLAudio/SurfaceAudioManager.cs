using System;
using System.Collections.Generic;

namespace RLAudio
{
	// Token: 0x02000E99 RID: 3737
	public static class SurfaceAudioManager
	{
		// Token: 0x06006960 RID: 26976 RVA: 0x0018224C File Offset: 0x0018044C
		public static float GetSurfaceParameter(SurfaceType surfaceType)
		{
			if (!SurfaceAudioManager.SURFACE_TYPE_PARAMETER_TABLE.ContainsKey(surfaceType))
			{
				surfaceType = SurfaceType.Default;
			}
			if (PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom() != null)
			{
				BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
				if (currentPlayerRoom != null)
				{
					BiomeType appearanceBiomeType = currentPlayerRoom.AppearanceBiomeType;
					if (surfaceType == SurfaceType.Default && SurfaceAudioManager.BIOME_DEFAULT_SURFACE_TYPE_TABLE.ContainsKey(appearanceBiomeType))
					{
						surfaceType = SurfaceAudioManager.BIOME_DEFAULT_SURFACE_TYPE_TABLE[appearanceBiomeType];
					}
				}
			}
			return SurfaceAudioManager.SURFACE_TYPE_PARAMETER_TABLE[surfaceType];
		}

		// Token: 0x040055C5 RID: 21957
		private static Dictionary<SurfaceType, float> SURFACE_TYPE_PARAMETER_TABLE = new Dictionary<SurfaceType, float>
		{
			{
				SurfaceType.Default,
				0f
			},
			{
				SurfaceType.Stone,
				0f
			},
			{
				SurfaceType.Boardwalk,
				1f
			},
			{
				SurfaceType.Boat,
				2f
			},
			{
				SurfaceType.Lantern,
				3f
			},
			{
				SurfaceType.Barrel,
				4f
			},
			{
				SurfaceType.Box,
				5f
			},
			{
				SurfaceType.Bookshelf,
				6f
			},
			{
				SurfaceType.WeakFloor,
				7f
			},
			{
				SurfaceType.Chair,
				8f
			},
			{
				SurfaceType.Table,
				9f
			},
			{
				SurfaceType.SpikeTrap,
				10f
			},
			{
				SurfaceType.ShootTrap,
				11f
			},
			{
				SurfaceType.RocketBox,
				12f
			},
			{
				SurfaceType.Thorn,
				13f
			},
			{
				SurfaceType.Grass,
				14f
			},
			{
				SurfaceType.Dirt,
				15f
			},
			{
				SurfaceType.Snow,
				16f
			},
			{
				SurfaceType.Magic,
				17f
			}
		};

		// Token: 0x040055C6 RID: 21958
		private static Dictionary<BiomeType, SurfaceType> BIOME_DEFAULT_SURFACE_TYPE_TABLE = new Dictionary<BiomeType, SurfaceType>
		{
			{
				BiomeType.Forest,
				SurfaceType.Snow
			}
		};
	}
}
