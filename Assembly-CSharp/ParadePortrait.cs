using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020002A7 RID: 679
public class ParadePortrait : MonoBehaviour
{
	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06001A31 RID: 6705 RVA: 0x00052A5F File Offset: 0x00050C5F
	// (set) Token: 0x06001A32 RID: 6706 RVA: 0x00052A67 File Offset: 0x00050C67
	public bool LoopAnim
	{
		get
		{
			return this.m_loopAnim;
		}
		set
		{
			this.m_loopAnim = value;
		}
	}

	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06001A33 RID: 6707 RVA: 0x00052A70 File Offset: 0x00050C70
	// (set) Token: 0x06001A34 RID: 6708 RVA: 0x00052A78 File Offset: 0x00050C78
	public bool RunSimpleAnim
	{
		get
		{
			return this.m_runSimpleAnim;
		}
		set
		{
			this.m_runSimpleAnim = value;
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x06001A35 RID: 6709 RVA: 0x00052A81 File Offset: 0x00050C81
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x06001A36 RID: 6710 RVA: 0x00052A89 File Offset: 0x00050C89
	// (set) Token: 0x06001A37 RID: 6711 RVA: 0x00052A91 File Offset: 0x00050C91
	public EnemyType EnemyType
	{
		get
		{
			return this.m_enemyType;
		}
		set
		{
			this.m_enemyType = value;
		}
	}

	// Token: 0x17000C04 RID: 3076
	// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00052A9A File Offset: 0x00050C9A
	// (set) Token: 0x06001A39 RID: 6713 RVA: 0x00052AA2 File Offset: 0x00050CA2
	public EnemyRank EnemyRank
	{
		get
		{
			return this.m_enemyRank;
		}
		set
		{
			this.m_enemyRank = value;
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (get) Token: 0x06001A3A RID: 6714 RVA: 0x00052AAB File Offset: 0x00050CAB
	// (set) Token: 0x06001A3B RID: 6715 RVA: 0x00052AB3 File Offset: 0x00050CB3
	public GameObject Model
	{
		get
		{
			return this.m_model;
		}
		set
		{
			this.m_model = value;
		}
	}

	// Token: 0x17000C06 RID: 3078
	// (get) Token: 0x06001A3C RID: 6716 RVA: 0x00052ABC File Offset: 0x00050CBC
	public TMP_Text NameText
	{
		get
		{
			return this.m_nameText;
		}
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x00052AC4 File Offset: 0x00050CC4
	private void Awake()
	{
		if (this.m_animator)
		{
			EffectManager.AddAnimatorToDisableList(this.m_animator);
		}
		EnemyData enemyData = EnemyClassLibrary.GetEnemyData(this.m_enemyType, this.m_enemyRank);
		if (enemyData)
		{
			int enemiesDefeated = SaveManager.ModeSaveData.GetEnemiesDefeated(GameModeType.Regular, this.m_enemyType, this.m_enemyRank);
			int timesDefeatedByEnemy = SaveManager.ModeSaveData.GetTimesDefeatedByEnemy(GameModeType.Regular, this.m_enemyType, this.m_enemyRank);
			if (enemiesDefeated <= 0 && timesDefeatedByEnemy <= 0)
			{
				this.m_nameText.text = "??????";
				this.m_model.SetActive(false);
			}
			else
			{
				this.m_hiddenGO.SetActive(false);
				this.m_nameText.text = LocalizationManager.GetString(enemyData.Title, false, false);
				if (this.m_enemyType == EnemyType.FlyingHunter)
				{
					this.m_animator.SetBool("Awake", true);
				}
			}
			this.m_slainByPlayerText.text = enemiesDefeated.ToString();
			this.m_slainPlayerText.text = timesDefeatedByEnemy.ToString();
		}
		if (this.m_runSimpleAnim)
		{
			this.m_paradeAnimatorArray = new ParadePortrait.ParadeAnimatorEntry[6];
			this.m_paradeAnimatorArray[1] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = this.m_startingAnimName + "_Tell_Intro"
			};
			this.m_paradeAnimatorArray[2] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = this.m_startingAnimName + "_Tell_Hold"
			};
			this.m_paradeAnimatorArray[3] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = this.m_startingAnimName + "_Attack_Intro"
			};
			this.m_paradeAnimatorArray[4] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = this.m_startingAnimName + "_Attack_Hold"
			};
			this.m_paradeAnimatorArray[5] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = this.m_startingAnimName + "_Exit"
			};
			this.m_paradeAnimatorArray[0] = new ParadePortrait.ParadeAnimatorEntry
			{
				StateName = "Neutral"
			};
		}
		if (this.m_paradeAnimatorArray.Length != 0 && this.m_animator)
		{
			base.StartCoroutine(this.AnimateCoroutine());
		}
		if (this.m_enemyType == EnemyType.Slug)
		{
			this.m_animator.SetFloat("FacingSpeed", 1f);
		}
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x00052CEA File Offset: 0x00050EEA
	private IEnumerator AnimateCoroutine()
	{
		EffectManager.AddAnimatorToDisableList(this.m_animator);
		int stateArrayLength = this.m_paradeAnimatorArray.Length;
		while (this.m_animatorArrayIndex < stateArrayLength)
		{
			ParadePortrait.ParadeAnimatorEntry entry = this.m_paradeAnimatorArray[this.m_animatorArrayIndex];
			if (global::AnimatorUtility.HasState(this.m_animator, entry.StateName))
			{
				this.m_animator.Play(entry.StateName, 0, 0f);
				this.m_animator.SetFloat("Anim_Speed", entry.AnimSpeed);
			}
			yield return null;
			while (this.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
			{
				yield return null;
			}
			float delay = Time.time + entry.ExitHoldDuration;
			while (Time.time < delay)
			{
				yield return null;
			}
			this.m_animatorArrayIndex++;
			if (this.m_animatorArrayIndex >= stateArrayLength && this.m_loopAnim)
			{
				delay = Time.time + this.m_loopDelay;
				while (Time.time < delay)
				{
					yield return null;
				}
				this.m_animatorArrayIndex = 0;
			}
			entry = null;
		}
		yield break;
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x00052CFC File Offset: 0x00050EFC
	public void UpdateEnemyType()
	{
		EnemyController enemyPrefab = EnemyLibrary.GetEnemyPrefab(this.EnemyType, this.EnemyRank);
		if (!enemyPrefab)
		{
			return;
		}
		Transform transform = enemyPrefab.transform.FindDeep("Visuals");
		if (transform && transform.childCount > 0)
		{
			base.name = enemyPrefab.EnemyType.ToString() + " " + enemyPrefab.EnemyRank.ToString() + " Portrait";
			this.NameText.text = enemyPrefab.EnemyType.ToString() + " " + enemyPrefab.EnemyRank.ToString();
			UnityEngine.Object.DestroyImmediate(this.Model);
			int index = 0;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeSelf)
				{
					index = i;
					break;
				}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.GetChild(index).gameObject, base.transform);
			float scale = EnemyClassLibrary.GetEnemyData(enemyPrefab.EnemyType, enemyPrefab.EnemyRank).Scale;
			float x = gameObject.transform.localScale.x;
			gameObject.transform.localScale = new Vector3(scale, scale, scale) * x;
			Animator component = enemyPrefab.GetComponent<Animator>();
			this.Animator.runtimeAnimatorController = component.runtimeAnimatorController;
			this.Animator.avatar = component.avatar;
			this.Model = gameObject;
			return;
		}
		Debug.Log(string.Concat(new string[]
		{
			"<color=red>ERROR: Could not update portrait for enemy of type: ",
			enemyPrefab.EnemyType.ToString(),
			".",
			enemyPrefab.EnemyRank.ToString(),
			" because Visuals child GO not found.</color>"
		}));
	}

	// Token: 0x04001898 RID: 6296
	[SerializeField]
	private EnemyType m_enemyType;

	// Token: 0x04001899 RID: 6297
	[SerializeField]
	private EnemyRank m_enemyRank;

	// Token: 0x0400189A RID: 6298
	[SerializeField]
	private TMP_Text m_nameText;

	// Token: 0x0400189B RID: 6299
	[SerializeField]
	private TMP_Text m_slainByPlayerText;

	// Token: 0x0400189C RID: 6300
	[SerializeField]
	private TMP_Text m_slainPlayerText;

	// Token: 0x0400189D RID: 6301
	[SerializeField]
	private Animator m_animator;

	// Token: 0x0400189E RID: 6302
	[SerializeField]
	private GameObject m_model;

	// Token: 0x0400189F RID: 6303
	[SerializeField]
	private GameObject m_hiddenGO;

	// Token: 0x040018A0 RID: 6304
	[SerializeField]
	private ParadePortrait.ParadeAnimatorEntry[] m_paradeAnimatorArray;

	// Token: 0x040018A1 RID: 6305
	[SerializeField]
	private bool m_runSimpleAnim;

	// Token: 0x040018A2 RID: 6306
	[SerializeField]
	private string m_startingAnimName;

	// Token: 0x040018A3 RID: 6307
	[SerializeField]
	private float m_loopDelay = 1f;

	// Token: 0x040018A4 RID: 6308
	[SerializeField]
	private bool m_loopAnim;

	// Token: 0x040018A5 RID: 6309
	private int m_animatorArrayIndex;

	// Token: 0x02000B4D RID: 2893
	[Serializable]
	private class ParadeAnimatorEntry
	{
		// Token: 0x04004C05 RID: 19461
		public string StateName;

		// Token: 0x04004C06 RID: 19462
		public float AnimSpeed = 1f;

		// Token: 0x04004C07 RID: 19463
		public float ExitHoldDuration;
	}
}
