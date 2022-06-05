using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class ParadePortrait : MonoBehaviour
{
	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06002448 RID: 9288 RVA: 0x000141F0 File Offset: 0x000123F0
	// (set) Token: 0x06002449 RID: 9289 RVA: 0x000141F8 File Offset: 0x000123F8
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

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x0600244A RID: 9290 RVA: 0x00014201 File Offset: 0x00012401
	// (set) Token: 0x0600244B RID: 9291 RVA: 0x00014209 File Offset: 0x00012409
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

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x0600244C RID: 9292 RVA: 0x00014212 File Offset: 0x00012412
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
	}

	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x0600244D RID: 9293 RVA: 0x0001421A File Offset: 0x0001241A
	// (set) Token: 0x0600244E RID: 9294 RVA: 0x00014222 File Offset: 0x00012422
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

	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x0600244F RID: 9295 RVA: 0x0001422B File Offset: 0x0001242B
	// (set) Token: 0x06002450 RID: 9296 RVA: 0x00014233 File Offset: 0x00012433
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

	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x06002451 RID: 9297 RVA: 0x0001423C File Offset: 0x0001243C
	// (set) Token: 0x06002452 RID: 9298 RVA: 0x00014244 File Offset: 0x00012444
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

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x06002453 RID: 9299 RVA: 0x0001424D File Offset: 0x0001244D
	public TMP_Text NameText
	{
		get
		{
			return this.m_nameText;
		}
	}

	// Token: 0x06002454 RID: 9300 RVA: 0x000AF1B4 File Offset: 0x000AD3B4
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

	// Token: 0x06002455 RID: 9301 RVA: 0x00014255 File Offset: 0x00012455
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

	// Token: 0x06002456 RID: 9302 RVA: 0x000AF3DC File Offset: 0x000AD5DC
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

	// Token: 0x04002008 RID: 8200
	[SerializeField]
	private EnemyType m_enemyType;

	// Token: 0x04002009 RID: 8201
	[SerializeField]
	private EnemyRank m_enemyRank;

	// Token: 0x0400200A RID: 8202
	[SerializeField]
	private TMP_Text m_nameText;

	// Token: 0x0400200B RID: 8203
	[SerializeField]
	private TMP_Text m_slainByPlayerText;

	// Token: 0x0400200C RID: 8204
	[SerializeField]
	private TMP_Text m_slainPlayerText;

	// Token: 0x0400200D RID: 8205
	[SerializeField]
	private Animator m_animator;

	// Token: 0x0400200E RID: 8206
	[SerializeField]
	private GameObject m_model;

	// Token: 0x0400200F RID: 8207
	[SerializeField]
	private GameObject m_hiddenGO;

	// Token: 0x04002010 RID: 8208
	[SerializeField]
	private ParadePortrait.ParadeAnimatorEntry[] m_paradeAnimatorArray;

	// Token: 0x04002011 RID: 8209
	[SerializeField]
	private bool m_runSimpleAnim;

	// Token: 0x04002012 RID: 8210
	[SerializeField]
	private string m_startingAnimName;

	// Token: 0x04002013 RID: 8211
	[SerializeField]
	private float m_loopDelay = 1f;

	// Token: 0x04002014 RID: 8212
	[SerializeField]
	private bool m_loopAnim;

	// Token: 0x04002015 RID: 8213
	private int m_animatorArrayIndex;

	// Token: 0x0200047C RID: 1148
	[Serializable]
	private class ParadeAnimatorEntry
	{
		// Token: 0x04002016 RID: 8214
		public string StateName;

		// Token: 0x04002017 RID: 8215
		public float AnimSpeed = 1f;

		// Token: 0x04002018 RID: 8216
		public float ExitHoldDuration;
	}
}
