using System;
using System.Collections;
using FMOD.Studio;
using RLAudio;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000510 RID: 1296
public class StudyBossRoomController : BossRoomController
{
	// Token: 0x170011CE RID: 4558
	// (get) Token: 0x06003020 RID: 12320 RVA: 0x000A4A5D File Offset: 0x000A2C5D
	public EnemyController MimicChestBoss
	{
		get
		{
			if (this.m_mimicChestSpawner)
			{
				return this.m_mimicChestSpawner.EnemyInstance;
			}
			return null;
		}
	}

	// Token: 0x170011CF RID: 4559
	// (get) Token: 0x06003021 RID: 12321 RVA: 0x000A4A79 File Offset: 0x000A2C79
	public override float BossHealthAsPercentage
	{
		get
		{
			if (this.MimicChestBoss && this.MimicChestBoss.isActiveAndEnabled)
			{
				return this.MimicChestBoss.CurrentHealth / (float)this.MimicChestBoss.ActualMaxHealth;
			}
			return base.BossHealthAsPercentage;
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000A4AB4 File Offset: 0x000A2CB4
	protected override void Awake()
	{
		base.Awake();
		this.m_hazardsToDisableOnModeShift = base.GetComponentsInChildren<IHazardSpawnController>(true);
		this.m_platformsToDisableOnModeShift = base.GetComponentsInChildren<SpecialPlatformSpawnController>(true);
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x000A4AD8 File Offset: 0x000A2CD8
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		if (this.MimicChestBoss)
		{
			this.MimicChestBoss.GetComponent<Interactable>().TriggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.ActivateMimicBoss));
			this.MimicChestBoss.gameObject.SetActive(false);
		}
		this.m_mimicBossDefeated = false;
		base.OnPlayerEnterRoom(sender, eventArgs);
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x000A4B33 File Offset: 0x000A2D33
	protected override void RemoveListeners()
	{
		if (this.m_listenersAssigned && this.MimicChestBoss)
		{
			this.MimicChestBoss.GetComponent<Interactable>().TriggerOnEnterEvent.RemoveListener(new UnityAction<GameObject>(this.ActivateMimicBoss));
		}
		base.RemoveListeners();
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000A4B74 File Offset: 0x000A2D74
	protected override void OnBossDeath(object sender, EventArgs args)
	{
		EnemyDeathEventArgs enemyDeathEventArgs = args as EnemyDeathEventArgs;
		if (enemyDeathEventArgs.Victim == this.MimicChestBoss)
		{
			this.m_mimicBossDefeated = true;
		}
		else if (enemyDeathEventArgs.Victim == base.Boss)
		{
			foreach (IHazardSpawnController hazardSpawnController in this.m_hazardsToDisableOnModeShift)
			{
				if (hazardSpawnController.Type != HazardType.SnowMound && hazardSpawnController.Hazard != null)
				{
					hazardSpawnController.Hazard.gameObject.SetActive(false);
				}
			}
			foreach (SpecialPlatformSpawnController specialPlatformSpawnController in this.m_platformsToDisableOnModeShift)
			{
				if (specialPlatformSpawnController.SpecialPlatformInstance)
				{
					specialPlatformSpawnController.SpecialPlatformInstance.gameObject.SetActive(false);
				}
			}
			if (this.MimicChestBoss)
			{
				base.StartCoroutine(this.SpawnMimicBossIntro());
			}
		}
		base.OnBossDeath(sender, args);
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000A4C5B File Offset: 0x000A2E5B
	protected IEnumerator SpawnMimicBossIntro()
	{
		this.OutroStartRelay.Dispatch();
		this.m_waitYield.CreateNew(3f, false);
		yield return this.m_waitYield;
		EnemyController mimicBoss = this.MimicChestBoss;
		mimicBoss.gameObject.SetActive(true);
		mimicBoss.Animator.Play("BossIntro_Closed", 0, 1f);
		mimicBoss.transform.localPosition = new Vector3(-8000f, -8000f);
		while (!mimicBoss.IsInitialized)
		{
			yield return null;
		}
		if (mimicBoss.IsFacingRight)
		{
			bool lockFlip = mimicBoss.LockFlip;
			mimicBoss.LockFlip = false;
			mimicBoss.CharacterCorgi.Flip(false, true);
			mimicBoss.LockFlip = lockFlip;
		}
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Weapon, false);
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Body, false);
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Terrain, true);
		mimicBoss.SetVelocity(0f, 0f, false);
		mimicBoss.ControllerCorgi.GravityActive(false);
		mimicBoss.ControllerCorgi.enabled = false;
		float chestOpacity = 0f;
		Renderer mimicRenderer = mimicBoss.GetComponentInChildren<Renderer>();
		mimicRenderer.material.SetFloat("_Opacity", chestOpacity);
		Vector3 position = base.Chest.transform.position;
		position.y += 1f;
		mimicBoss.transform.localPosition = position;
		mimicBoss.UpdateBounds();
		mimicBoss.ResetCollisionState();
		Interactable mimicInteract = mimicBoss.GetComponent<Interactable>();
		mimicInteract.SetIsInteractableActive(false);
		position.y -= 1f;
		TweenManager.TweenTo(mimicBoss.transform, 1f, new EaseDelegate(Ease.Quad.EaseOut), new object[]
		{
			"position.x",
			position.x,
			"position.y",
			position.y
		});
		this.m_chestAudioEventInstance = AudioUtility.GetEventInstance("event:/SFX/Interactables/sfx_env_prop_bossChest_spawn_loop", base.Chest.transform);
		AudioManager.PlayAttached(this, this.m_chestAudioEventInstance, base.Chest.gameObject);
		while (chestOpacity < 1f)
		{
			mimicRenderer.material.SetFloat("_Opacity", chestOpacity);
			chestOpacity += Time.deltaTime;
			yield return null;
		}
		AudioManager.Stop(this.m_chestAudioEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		mimicInteract.SetIsInteractableActive(true);
		yield break;
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000A4C6A File Offset: 0x000A2E6A
	private void ActivateMimicBoss(GameObject sender)
	{
		base.StartCoroutine(this.ActivateMimicBossCoroutine());
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000A4C79 File Offset: 0x000A2E79
	private IEnumerator ActivateMimicBossCoroutine()
	{
		Debug.Log("Activating Mimic boss");
		EnemyController mimicBoss = this.MimicChestBoss;
		yield return mimicBoss.LogicController.LogicScript.SpawnAnim();
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Weapon, true);
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Body, true);
		mimicBoss.HitboxController.SetHitboxActiveState(HitboxType.Terrain, false);
		mimicBoss.ControllerCorgi.enabled = true;
		mimicBoss.UpdateBounds();
		mimicBoss.ResetCollisionState();
		mimicBoss.ControllerCorgi.GravityActive(true);
		mimicBoss.LogicController.TriggerAggro(null, null);
		mimicBoss.Animator.SetBool("IsActive", true);
		yield break;
	}

	// Token: 0x0400264C RID: 9804
	[SerializeField]
	private EnemySpawnController m_mimicChestSpawner;

	// Token: 0x0400264D RID: 9805
	private IHazardSpawnController[] m_hazardsToDisableOnModeShift;

	// Token: 0x0400264E RID: 9806
	private SpecialPlatformSpawnController[] m_platformsToDisableOnModeShift;

	// Token: 0x0400264F RID: 9807
	private bool m_mimicBossDefeated;
}
