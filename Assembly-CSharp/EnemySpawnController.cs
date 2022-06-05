using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A28 RID: 2600
public class EnemySpawnController : MonoBehaviour, ISpawnController, IHasProjectileNameArray
{
	// Token: 0x17001B2B RID: 6955
	// (get) Token: 0x06004E9C RID: 20124 RVA: 0x0012DE90 File Offset: 0x0012C090
	public string[] ProjectileNameArray
	{
		get
		{
			if (this.m_projectileNameArray == null)
			{
				BaseAIScript aiscript = EnemyClassLibrary.GetEnemyClassData(this.Type).GetAIScript(this.Rank);
				if (aiscript && aiscript.ProjectileNameArray != null)
				{
					this.m_projectileNameArray = aiscript.ProjectileNameArray;
				}
				else
				{
					this.m_projectileNameArray = EnemySpawnController.EmptyProjectileNameArray_STATIC;
				}
			}
			return this.m_projectileNameArray;
		}
	}

	// Token: 0x17001B2C RID: 6956
	// (get) Token: 0x06004E9D RID: 20125 RVA: 0x0002ACEA File Offset: 0x00028EEA
	// (set) Token: 0x06004E9E RID: 20126 RVA: 0x0002ACF2 File Offset: 0x00028EF2
	public EnemyController EnemyInstance
	{
		get
		{
			return this.m_enemy;
		}
		private set
		{
			this.m_enemy = value;
		}
	}

	// Token: 0x17001B2D RID: 6957
	// (get) Token: 0x06004E9F RID: 20127 RVA: 0x0002ACFB File Offset: 0x00028EFB
	// (set) Token: 0x06004EA0 RID: 20128 RVA: 0x0002AD03 File Offset: 0x00028F03
	public int EnemyIndex
	{
		get
		{
			return this.m_enemyIndex;
		}
		private set
		{
			this.m_enemyIndex = value;
		}
	}

	// Token: 0x17001B2E RID: 6958
	// (get) Token: 0x06004EA1 RID: 20129 RVA: 0x0002AD0C File Offset: 0x00028F0C
	// (set) Token: 0x06004EA2 RID: 20130 RVA: 0x0002AD14 File Offset: 0x00028F14
	public bool ForceCommander
	{
		get
		{
			return this.m_forceCommander;
		}
		private set
		{
			this.m_forceCommander = value;
		}
	}

	// Token: 0x17001B2F RID: 6959
	// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x0002AD1D File Offset: 0x00028F1D
	// (set) Token: 0x06004EA4 RID: 20132 RVA: 0x0002AD25 File Offset: 0x00028F25
	public bool ForceFlying
	{
		get
		{
			return this.m_forceFlying;
		}
		private set
		{
			this.m_forceFlying = value;
		}
	}

	// Token: 0x17001B30 RID: 6960
	// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x00003713 File Offset: 0x00001913
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x17001B31 RID: 6961
	// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x0002AD2E File Offset: 0x00028F2E
	// (set) Token: 0x06004EA7 RID: 20135 RVA: 0x0002AD36 File Offset: 0x00028F36
	public int ID
	{
		get
		{
			return this.m_id;
		}
		private set
		{
			this.m_id = value;
		}
	}

	// Token: 0x17001B32 RID: 6962
	// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x0002AD3F File Offset: 0x00028F3F
	// (set) Token: 0x06004EA9 RID: 20137 RVA: 0x0002AD51 File Offset: 0x00028F51
	public bool IsDead
	{
		get
		{
			return !this.ShouldSpawn || this.m_isDead;
		}
		private set
		{
			this.m_isDead = value;
		}
	}

	// Token: 0x17001B33 RID: 6963
	// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0002AD5A File Offset: 0x00028F5A
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x17001B34 RID: 6964
	// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0002AD77 File Offset: 0x00028F77
	// (set) Token: 0x06004EAC RID: 20140 RVA: 0x0002AD7F File Offset: 0x00028F7F
	public int Level
	{
		get
		{
			return this.m_level;
		}
		set
		{
			this.m_level = value;
		}
	}

	// Token: 0x17001B35 RID: 6965
	// (get) Token: 0x06004EAD RID: 20141 RVA: 0x0002AD88 File Offset: 0x00028F88
	// (set) Token: 0x06004EAE RID: 20142 RVA: 0x0002AD90 File Offset: 0x00028F90
	public bool Override
	{
		get
		{
			return this.m_override;
		}
		set
		{
			this.m_override = value;
		}
	}

	// Token: 0x17001B36 RID: 6966
	// (get) Token: 0x06004EAF RID: 20143 RVA: 0x0002AD99 File Offset: 0x00028F99
	// (set) Token: 0x06004EB0 RID: 20144 RVA: 0x0002ADA1 File Offset: 0x00028FA1
	public bool OverrideLevel
	{
		get
		{
			return this.m_overrideLevel;
		}
		set
		{
			this.m_overrideLevel = value;
		}
	}

	// Token: 0x17001B37 RID: 6967
	// (get) Token: 0x06004EB1 RID: 20145 RVA: 0x0002ADAA File Offset: 0x00028FAA
	// (set) Token: 0x06004EB2 RID: 20146 RVA: 0x0002ADCE File Offset: 0x00028FCE
	public EnemyRank Rank
	{
		get
		{
			if (this.Room && this.Room.SpawnedAsEasyRoom)
			{
				return EnemyRank.Basic;
			}
			return this.m_rank;
		}
		set
		{
			this.m_rank = value;
		}
	}

	// Token: 0x17001B38 RID: 6968
	// (get) Token: 0x06004EB3 RID: 20147 RVA: 0x0002ADD7 File Offset: 0x00028FD7
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x17001B39 RID: 6969
	// (get) Token: 0x06004EB4 RID: 20148 RVA: 0x0002ADDF File Offset: 0x00028FDF
	// (set) Token: 0x06004EB5 RID: 20149 RVA: 0x0002AE02 File Offset: 0x00029002
	public SpawnLogicController SpawnLogicController
	{
		get
		{
			if (!this.m_hasCheckedForSpawnLogicController)
			{
				this.m_hasCheckedForSpawnLogicController = true;
				this.m_spawnLogic = base.GetComponent<SpawnLogicController>();
			}
			return this.m_spawnLogic;
		}
		private set
		{
			this.m_spawnLogic = value;
		}
	}

	// Token: 0x17001B3A RID: 6970
	// (get) Token: 0x06004EB6 RID: 20150 RVA: 0x0002AE0B File Offset: 0x0002900B
	// (set) Token: 0x06004EB7 RID: 20151 RVA: 0x0002AE13 File Offset: 0x00029013
	public EnemyType Type
	{
		get
		{
			return this.m_type;
		}
		set
		{
			this.m_type = value;
		}
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x0002AE1C File Offset: 0x0002901C
	private void Awake()
	{
		this.m_onEnemyDeath = new Action<object, EnemyDeathEventArgs>(this.OnEnemyDeath);
		this.m_onEnemyTimedOut = new Action<object, EnemyActivationStateChangedEventArgs>(this.OnEnemyTimedOut);
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x00002FCA File Offset: 0x000011CA
	private void OnDrawGizmos()
	{
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x0002AE42 File Offset: 0x00029042
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(this.m_spawnPoint, 0.5f);
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x0012DEEC File Offset: 0x0012C0EC
	private Vector3 GetSpawnPoint(bool isFlying)
	{
		if (isFlying)
		{
			return base.transform.position;
		}
		Array.Clear(EnemySpawnController.m_raycastAllocArray, 0, EnemySpawnController.m_raycastAllocArray.Length);
		LayerMask mask = LayerMask.GetMask(new string[]
		{
			"Platform_CollidesWithAll",
			"Platform_CollidesWithEnemy",
			"Platform_OneWay"
		});
		Physics2D.RaycastNonAlloc(base.transform.position + new Vector3(0f, 0.1f, 0f), Vector2.down, EnemySpawnController.m_raycastAllocArray, 20f, mask);
		foreach (RaycastHit2D hit in EnemySpawnController.m_raycastAllocArray)
		{
			if (hit)
			{
				GameObject root = hit.collider.GetRoot(false);
				if (root.CompareTag("Platform") || root.CompareTag("OneWay"))
				{
					this.m_spawnCollider = hit.collider;
					return hit.point;
				}
			}
		}
		return base.transform.position;
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x0012DFFC File Offset: 0x0012C1FC
	private void ResetEnemyPosition(EnemyController enemy)
	{
		if (this.m_spawnPoint == new Vector3(-1000f, -1000f, -1000f))
		{
			EnemyData enemyData = EnemyClassLibrary.GetEnemyData(this.Type, this.Rank);
			this.m_spawnPoint = this.GetSpawnPoint(enemyData.IsFlying);
		}
		enemy.ResetPositionForSpawnController(this.m_spawnPoint, this.m_spawnCollider);
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x0002AE54 File Offset: 0x00029054
	public void SetColor(Color color)
	{
		if (!Application.isPlaying)
		{
			base.GetComponent<SpriteRenderer>().color = color;
			this.ID = EnemySpawnController.ColorTable[color];
		}
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x0002AE7A File Offset: 0x0002907A
	public void SetForceCommander(bool forceCommander)
	{
		this.ForceCommander = forceCommander;
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x0002AE83 File Offset: 0x00029083
	public void SetForceFlying(bool forceFlying)
	{
		this.ForceFlying = forceFlying;
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x0002AE8C File Offset: 0x0002908C
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x0002AE95 File Offset: 0x00029095
	public void SetEnemy(EnemyType enemyType, EnemyRank enemyRank, int enemyLevel)
	{
		if (!this.Override)
		{
			this.Type = enemyType;
			this.Rank = enemyRank;
		}
		if (!this.OverrideLevel)
		{
			this.Level = enemyLevel;
			return;
		}
		if (this.Level <= 0)
		{
			this.Level = 1;
		}
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x0002AECD File Offset: 0x000290CD
	public void SetEnemyIndex(int index)
	{
		this.EnemyIndex = index;
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x0002AED6 File Offset: 0x000290D6
	public void ForceEnemyDead(bool disableEnemy = true)
	{
		if (this.EnemyInstance)
		{
			if (this.EnemyInstance.IsBoss)
			{
				return;
			}
			if (disableEnemy)
			{
				this.EnemyInstance.gameObject.SetActive(false);
			}
		}
		this.IsDead = true;
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x0002AF0E File Offset: 0x0002910E
	public void ResetIsDead()
	{
		this.IsDead = false;
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x0002AF17 File Offset: 0x00029117
	private void OnEnemyDeath(object sender, EnemyDeathEventArgs eventArgs)
	{
		this.IsDead = true;
		this.EnemyInstance.OnEnemyDeathRelay.RemoveListener(this.m_onEnemyDeath);
		this.EnemyInstance.OnReactivationTimedOutRelay.RemoveListener(this.m_onEnemyTimedOut);
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x0002AF4E File Offset: 0x0002914E
	private void OnEnemyTimedOut(object sender, EnemyActivationStateChangedEventArgs eventArgs)
	{
		if (eventArgs.Enemy != null)
		{
			this.ResetEnemyPosition(eventArgs.Enemy);
		}
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x0002AF6A File Offset: 0x0002916A
	public void RemoveListeners()
	{
		if (this.EnemyInstance)
		{
			this.EnemyInstance.OnEnemyDeathRelay.RemoveListener(this.m_onEnemyDeath);
			this.EnemyInstance.OnReactivationTimedOutRelay.RemoveListener(this.m_onEnemyTimedOut);
		}
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x0012E060 File Offset: 0x0012C260
	public void InitializeEnemyInstance()
	{
		if (!this.ShouldSpawn)
		{
			return;
		}
		this.EnemyInstance = EnemyManager.GetEnemyFromPool(this.Type, this.Rank);
		if (!this.EnemyInstance.IsNativeNull())
		{
			this.EnemyInstance.SetLevel(this.Level);
			this.EnemyInstance.SetEnemyIndex(this.EnemyIndex);
			this.EnemyInstance.Summoner = null;
			this.EnemyInstance.SetRoom(this.Room);
			this.EnemyInstance.gameObject.SetActive(true);
			this.EnemyInstance.EnemySpawnController = this;
			if (this.EnemyInstance.IsInitialized)
			{
				this.EnemyInstance.ResetCharacter();
			}
			if (this.Room.SpawnedAsEasyRoom)
			{
				this.EnemyInstance.IsCommander = false;
			}
			else
			{
				this.EnemyInstance.IsCommander = this.ForceCommander;
			}
			this.EnemyInstance.InitializeCommanderStatusEffects();
			this.ResetEnemyPosition(this.EnemyInstance);
			this.EnemyInstance.OnEnemyDeathRelay.AddListener(this.m_onEnemyDeath, false);
			this.EnemyInstance.OnReactivationTimedOutRelay.AddListener(this.m_onEnemyTimedOut, false);
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Failed to get a <b>{1} {2}</b> from Enemy Pool</color>", new object[]
		{
			this,
			this.Rank,
			this.Type
		});
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x0012E1B8 File Offset: 0x0012C3B8
	private void OnDisable()
	{
		if (this.EnemyInstance)
		{
			this.EnemyInstance.OnEnemyDeathRelay.RemoveListener(this.m_onEnemyDeath);
			this.EnemyInstance.OnReactivationTimedOutRelay.RemoveListener(this.m_onEnemyTimedOut);
			if (!GameManager.IsApplicationClosing && this.EnemyInstance.gameObject.activeSelf)
			{
				this.EnemyInstance.gameObject.SetActive(false);
			}
			this.EnemyInstance = null;
		}
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003B34 RID: 15156
	private const int GROUNDING_RAYCAST_DISTANCE = 20;

	// Token: 0x04003B35 RID: 15157
	public static Dictionary<Color, int> ColorTable = new Dictionary<Color, int>
	{
		{
			Color.white,
			0
		},
		{
			Color.red,
			1
		},
		{
			Color.green,
			2
		},
		{
			Color.blue,
			3
		},
		{
			Color.cyan,
			4
		},
		{
			Color.magenta,
			5
		},
		{
			Color.yellow,
			6
		},
		{
			Color.grey,
			7
		}
	};

	// Token: 0x04003B36 RID: 15158
	public static Dictionary<int, Color> IDToColorTable = new Dictionary<int, Color>
	{
		{
			0,
			Color.white
		},
		{
			1,
			Color.red
		},
		{
			2,
			Color.green
		},
		{
			3,
			Color.blue
		},
		{
			4,
			Color.cyan
		},
		{
			5,
			Color.magenta
		},
		{
			6,
			Color.yellow
		},
		{
			7,
			Color.grey
		}
	};

	// Token: 0x04003B37 RID: 15159
	public static Dictionary<Color, string> ColorNameTable = new Dictionary<Color, string>
	{
		{
			Color.white,
			"White"
		},
		{
			Color.red,
			"Red"
		},
		{
			Color.green,
			"Green"
		},
		{
			Color.blue,
			"Blue"
		},
		{
			Color.cyan,
			"Cyan"
		},
		{
			Color.magenta,
			"Magenta"
		},
		{
			Color.yellow,
			"Yellow"
		},
		{
			Color.grey,
			"Grey"
		}
	};

	// Token: 0x04003B38 RID: 15160
	private EnemyController m_enemy;

	// Token: 0x04003B39 RID: 15161
	private Vector3 m_spawnPoint = new Vector3(-1000f, -1000f, -1000f);

	// Token: 0x04003B3A RID: 15162
	private BaseRoom m_room;

	// Token: 0x04003B3B RID: 15163
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04003B3C RID: 15164
	private int m_enemyIndex = -1;

	// Token: 0x04003B3D RID: 15165
	private string m_gizmoText = "";

	// Token: 0x04003B3E RID: 15166
	private static RaycastHit2D[] m_raycastAllocArray = new RaycastHit2D[5];

	// Token: 0x04003B3F RID: 15167
	private Collider2D m_spawnCollider;

	// Token: 0x04003B40 RID: 15168
	public static string[] EmptyProjectileNameArray_STATIC = new string[0];

	// Token: 0x04003B41 RID: 15169
	private bool m_isDead;

	// Token: 0x04003B42 RID: 15170
	private Action<object, EnemyDeathEventArgs> m_onEnemyDeath;

	// Token: 0x04003B43 RID: 15171
	private Action<object, EnemyActivationStateChangedEventArgs> m_onEnemyTimedOut;

	// Token: 0x04003B44 RID: 15172
	[SerializeField]
	private bool m_forceFlying;

	// Token: 0x04003B45 RID: 15173
	[SerializeField]
	private bool m_forceCommander;

	// Token: 0x04003B46 RID: 15174
	[SerializeField]
	private bool m_override;

	// Token: 0x04003B47 RID: 15175
	[SerializeField]
	private bool m_overrideLevel;

	// Token: 0x04003B48 RID: 15176
	[SerializeField]
	private EnemyType m_type;

	// Token: 0x04003B49 RID: 15177
	[SerializeField]
	private EnemyRank m_rank = EnemyRank.None;

	// Token: 0x04003B4A RID: 15178
	[SerializeField]
	private int m_level = -1;

	// Token: 0x04003B4B RID: 15179
	[SerializeField]
	private int m_id = -1;

	// Token: 0x04003B4C RID: 15180
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x04003B4D RID: 15181
	private bool m_hasCheckedForSpawnLogicController;
}
