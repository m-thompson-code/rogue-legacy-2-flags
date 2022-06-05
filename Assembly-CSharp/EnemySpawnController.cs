using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000607 RID: 1543
public class EnemySpawnController : MonoBehaviour, ISpawnController, IHasProjectileNameArray
{
	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x060037FF RID: 14335 RVA: 0x000BF6DC File Offset: 0x000BD8DC
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

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x06003800 RID: 14336 RVA: 0x000BF737 File Offset: 0x000BD937
	// (set) Token: 0x06003801 RID: 14337 RVA: 0x000BF73F File Offset: 0x000BD93F
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

	// Token: 0x170013D6 RID: 5078
	// (get) Token: 0x06003802 RID: 14338 RVA: 0x000BF748 File Offset: 0x000BD948
	// (set) Token: 0x06003803 RID: 14339 RVA: 0x000BF750 File Offset: 0x000BD950
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

	// Token: 0x170013D7 RID: 5079
	// (get) Token: 0x06003804 RID: 14340 RVA: 0x000BF759 File Offset: 0x000BD959
	// (set) Token: 0x06003805 RID: 14341 RVA: 0x000BF761 File Offset: 0x000BD961
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

	// Token: 0x170013D8 RID: 5080
	// (get) Token: 0x06003806 RID: 14342 RVA: 0x000BF76A File Offset: 0x000BD96A
	// (set) Token: 0x06003807 RID: 14343 RVA: 0x000BF772 File Offset: 0x000BD972
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

	// Token: 0x170013D9 RID: 5081
	// (get) Token: 0x06003808 RID: 14344 RVA: 0x000BF77B File Offset: 0x000BD97B
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x170013DA RID: 5082
	// (get) Token: 0x06003809 RID: 14345 RVA: 0x000BF783 File Offset: 0x000BD983
	// (set) Token: 0x0600380A RID: 14346 RVA: 0x000BF78B File Offset: 0x000BD98B
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

	// Token: 0x170013DB RID: 5083
	// (get) Token: 0x0600380B RID: 14347 RVA: 0x000BF794 File Offset: 0x000BD994
	// (set) Token: 0x0600380C RID: 14348 RVA: 0x000BF7A6 File Offset: 0x000BD9A6
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

	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600380D RID: 14349 RVA: 0x000BF7AF File Offset: 0x000BD9AF
	public bool ShouldSpawn
	{
		get
		{
			return !(this.SpawnLogicController != null) || this.SpawnLogicController.ShouldSpawn;
		}
	}

	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600380E RID: 14350 RVA: 0x000BF7CC File Offset: 0x000BD9CC
	// (set) Token: 0x0600380F RID: 14351 RVA: 0x000BF7D4 File Offset: 0x000BD9D4
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

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x06003810 RID: 14352 RVA: 0x000BF7DD File Offset: 0x000BD9DD
	// (set) Token: 0x06003811 RID: 14353 RVA: 0x000BF7E5 File Offset: 0x000BD9E5
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

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x06003812 RID: 14354 RVA: 0x000BF7EE File Offset: 0x000BD9EE
	// (set) Token: 0x06003813 RID: 14355 RVA: 0x000BF7F6 File Offset: 0x000BD9F6
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

	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x06003814 RID: 14356 RVA: 0x000BF7FF File Offset: 0x000BD9FF
	// (set) Token: 0x06003815 RID: 14357 RVA: 0x000BF823 File Offset: 0x000BDA23
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

	// Token: 0x170013E1 RID: 5089
	// (get) Token: 0x06003816 RID: 14358 RVA: 0x000BF82C File Offset: 0x000BDA2C
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x170013E2 RID: 5090
	// (get) Token: 0x06003817 RID: 14359 RVA: 0x000BF834 File Offset: 0x000BDA34
	// (set) Token: 0x06003818 RID: 14360 RVA: 0x000BF857 File Offset: 0x000BDA57
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

	// Token: 0x170013E3 RID: 5091
	// (get) Token: 0x06003819 RID: 14361 RVA: 0x000BF860 File Offset: 0x000BDA60
	// (set) Token: 0x0600381A RID: 14362 RVA: 0x000BF868 File Offset: 0x000BDA68
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

	// Token: 0x0600381B RID: 14363 RVA: 0x000BF871 File Offset: 0x000BDA71
	private void Awake()
	{
		this.m_onEnemyDeath = new Action<object, EnemyDeathEventArgs>(this.OnEnemyDeath);
		this.m_onEnemyTimedOut = new Action<object, EnemyActivationStateChangedEventArgs>(this.OnEnemyTimedOut);
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x000BF897 File Offset: 0x000BDA97
	private void OnDrawGizmos()
	{
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x000BF899 File Offset: 0x000BDA99
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawSphere(this.m_spawnPoint, 0.5f);
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x000BF8AC File Offset: 0x000BDAAC
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

	// Token: 0x0600381F RID: 14367 RVA: 0x000BF9BC File Offset: 0x000BDBBC
	private void ResetEnemyPosition(EnemyController enemy)
	{
		if (this.m_spawnPoint == new Vector3(-1000f, -1000f, -1000f))
		{
			EnemyData enemyData = EnemyClassLibrary.GetEnemyData(this.Type, this.Rank);
			this.m_spawnPoint = this.GetSpawnPoint(enemyData.IsFlying);
		}
		enemy.ResetPositionForSpawnController(this.m_spawnPoint, this.m_spawnCollider);
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x000BFA20 File Offset: 0x000BDC20
	public void SetColor(Color color)
	{
		if (!Application.isPlaying)
		{
			base.GetComponent<SpriteRenderer>().color = color;
			this.ID = EnemySpawnController.ColorTable[color];
		}
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x000BFA46 File Offset: 0x000BDC46
	public void SetForceCommander(bool forceCommander)
	{
		this.ForceCommander = forceCommander;
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x000BFA4F File Offset: 0x000BDC4F
	public void SetForceFlying(bool forceFlying)
	{
		this.ForceFlying = forceFlying;
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x000BFA58 File Offset: 0x000BDC58
	public void SetRoom(BaseRoom value)
	{
		this.m_room = value;
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x000BFA61 File Offset: 0x000BDC61
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

	// Token: 0x06003825 RID: 14373 RVA: 0x000BFA99 File Offset: 0x000BDC99
	public void SetEnemyIndex(int index)
	{
		this.EnemyIndex = index;
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x000BFAA2 File Offset: 0x000BDCA2
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

	// Token: 0x06003827 RID: 14375 RVA: 0x000BFADA File Offset: 0x000BDCDA
	public void ResetIsDead()
	{
		this.IsDead = false;
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x000BFAE3 File Offset: 0x000BDCE3
	private void OnEnemyDeath(object sender, EnemyDeathEventArgs eventArgs)
	{
		this.IsDead = true;
		this.EnemyInstance.OnEnemyDeathRelay.RemoveListener(this.m_onEnemyDeath);
		this.EnemyInstance.OnReactivationTimedOutRelay.RemoveListener(this.m_onEnemyTimedOut);
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x000BFB1A File Offset: 0x000BDD1A
	private void OnEnemyTimedOut(object sender, EnemyActivationStateChangedEventArgs eventArgs)
	{
		if (eventArgs.Enemy != null)
		{
			this.ResetEnemyPosition(eventArgs.Enemy);
		}
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x000BFB36 File Offset: 0x000BDD36
	public void RemoveListeners()
	{
		if (this.EnemyInstance)
		{
			this.EnemyInstance.OnEnemyDeathRelay.RemoveListener(this.m_onEnemyDeath);
			this.EnemyInstance.OnReactivationTimedOutRelay.RemoveListener(this.m_onEnemyTimedOut);
		}
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x000BFB74 File Offset: 0x000BDD74
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

	// Token: 0x0600382C RID: 14380 RVA: 0x000BFCCC File Offset: 0x000BDECC
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

	// Token: 0x0600382F RID: 14383 RVA: 0x000BFF1D File Offset: 0x000BE11D
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002AD1 RID: 10961
	private const int GROUNDING_RAYCAST_DISTANCE = 20;

	// Token: 0x04002AD2 RID: 10962
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

	// Token: 0x04002AD3 RID: 10963
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

	// Token: 0x04002AD4 RID: 10964
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

	// Token: 0x04002AD5 RID: 10965
	private EnemyController m_enemy;

	// Token: 0x04002AD6 RID: 10966
	private Vector3 m_spawnPoint = new Vector3(-1000f, -1000f, -1000f);

	// Token: 0x04002AD7 RID: 10967
	private BaseRoom m_room;

	// Token: 0x04002AD8 RID: 10968
	private SpawnLogicController m_spawnLogic;

	// Token: 0x04002AD9 RID: 10969
	private int m_enemyIndex = -1;

	// Token: 0x04002ADA RID: 10970
	private string m_gizmoText = "";

	// Token: 0x04002ADB RID: 10971
	private static RaycastHit2D[] m_raycastAllocArray = new RaycastHit2D[5];

	// Token: 0x04002ADC RID: 10972
	private Collider2D m_spawnCollider;

	// Token: 0x04002ADD RID: 10973
	public static string[] EmptyProjectileNameArray_STATIC = new string[0];

	// Token: 0x04002ADE RID: 10974
	private bool m_isDead;

	// Token: 0x04002ADF RID: 10975
	private Action<object, EnemyDeathEventArgs> m_onEnemyDeath;

	// Token: 0x04002AE0 RID: 10976
	private Action<object, EnemyActivationStateChangedEventArgs> m_onEnemyTimedOut;

	// Token: 0x04002AE1 RID: 10977
	[SerializeField]
	private bool m_forceFlying;

	// Token: 0x04002AE2 RID: 10978
	[SerializeField]
	private bool m_forceCommander;

	// Token: 0x04002AE3 RID: 10979
	[SerializeField]
	private bool m_override;

	// Token: 0x04002AE4 RID: 10980
	[SerializeField]
	private bool m_overrideLevel;

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	private EnemyType m_type;

	// Token: 0x04002AE6 RID: 10982
	[SerializeField]
	private EnemyRank m_rank = EnemyRank.None;

	// Token: 0x04002AE7 RID: 10983
	[SerializeField]
	private int m_level = -1;

	// Token: 0x04002AE8 RID: 10984
	[SerializeField]
	private int m_id = -1;

	// Token: 0x04002AE9 RID: 10985
	[NonSerialized]
	private string[] m_projectileNameArray;

	// Token: 0x04002AEA RID: 10986
	private bool m_hasCheckedForSpawnLogicController;
}
