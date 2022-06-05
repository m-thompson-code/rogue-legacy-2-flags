using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x0200090C RID: 2316
	public class ProjectileEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x00110E3F File Offset: 0x0010F03F
		// (set) Token: 0x06004BF2 RID: 19442 RVA: 0x00110E47 File Offset: 0x0010F047
		public string DeathEvent
		{
			get
			{
				return this.m_deathEvent;
			}
			protected set
			{
				this.m_deathEvent = value;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00110E50 File Offset: 0x0010F050
		public string Description
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_description))
				{
					this.m_description = string.Format("{0}", this);
				}
				return this.m_description;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x00110E76 File Offset: 0x0010F076
		private bool HasLifetimeEvent
		{
			get
			{
				return !string.IsNullOrEmpty(this.LifetimeEvent);
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00110E86 File Offset: 0x0010F086
		// (set) Token: 0x06004BF6 RID: 19446 RVA: 0x00110E8E File Offset: 0x0010F08E
		public string HitBreakableEvent { get; private set; }

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x06004BF7 RID: 19447 RVA: 0x00110E97 File Offset: 0x0010F097
		// (set) Token: 0x06004BF8 RID: 19448 RVA: 0x00110E9F File Offset: 0x0010F09F
		public string HitSurfaceEvent
		{
			get
			{
				return this.m_hitSurfaceEvent;
			}
			protected set
			{
				this.m_hitSurfaceEvent = value;
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x00110EA8 File Offset: 0x0010F0A8
		// (set) Token: 0x06004BFA RID: 19450 RVA: 0x00110EB0 File Offset: 0x0010F0B0
		public string LifetimeEvent
		{
			get
			{
				return this.m_lifetimeEvent;
			}
			protected set
			{
				this.m_lifetimeEvent = value;
			}
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x00110EB9 File Offset: 0x0010F0B9
		// (set) Token: 0x06004BFC RID: 19452 RVA: 0x00110EC1 File Offset: 0x0010F0C1
		public string SpawnEvent
		{
			get
			{
				return this.m_spawnEvent;
			}
			protected set
			{
				this.m_spawnEvent = value;
			}
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00110ECC File Offset: 0x0010F0CC
		protected virtual void Awake()
		{
			this.m_projectile = base.GetComponent<Projectile_RL>();
			this.m_projectile.OnSpawnRelay.AddListener(new Action<Projectile_RL, GameObject>(this.OnSpawn), false);
			this.m_onCollision = new Action<Projectile_RL, GameObject>(this.OnCollision);
			this.m_onDeath = new Action<Projectile_RL, GameObject>(this.OnDeath);
			this.m_trimmedName = this.GetName();
			ProjectileAudioLibraryEntry entry = ProjectileAudioLibrary.GetEntry(this.m_trimmedName);
			if (entry != null)
			{
				this.SpawnEvent = entry.SpawnSingleEventPath;
				this.LifetimeEvent = entry.LifetimeEventPath;
				this.HitBreakableEvent = entry.HitItemEventPath;
				this.HitSurfaceEvent = entry.HitSurfaceEventPath;
				this.DeathEvent = entry.DeathEventPath;
			}
			if (!string.IsNullOrEmpty(this.LifetimeEvent))
			{
				try
				{
					this.m_lifeTimeEventInstance = AudioUtility.GetEventInstance(this.LifetimeEvent, base.transform);
				}
				catch (Exception)
				{
					Debug.LogFormat("<color=red>| {0} | The FMOD Event path you specified for the Lifetime Event <b>({1})</b> is invalid.</color>", new object[]
					{
						this,
						this.LifetimeEvent
					});
				}
			}
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00110FD8 File Offset: 0x0010F1D8
		protected string GetName()
		{
			int num = base.name.IndexOf("(Clone)");
			string result = base.name;
			if (num != -1)
			{
				result = base.name.Remove(num);
			}
			return result;
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x0011100F File Offset: 0x0010F20F
		protected virtual void OnDisable()
		{
			if (this.GetIsEventPlaying(this.m_lifeTimeEventInstance))
			{
				AudioManager.Stop(this.m_lifeTimeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x0011102C File Offset: 0x0010F22C
		protected virtual void OnDestroy()
		{
			this.m_projectile.OnSpawnRelay.RemoveListener(new Action<Projectile_RL, GameObject>(this.OnSpawn));
			this.m_projectile.OnCollisionRelay.RemoveListener(this.m_onCollision);
			this.m_projectile.OnDeathRelay.RemoveListener(this.m_onDeath);
			if (this.m_lifeTimeEventInstance.isValid())
			{
				this.m_lifeTimeEventInstance.release();
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x001110A0 File Offset: 0x0010F2A0
		protected bool GetIsEventPlaying(EventInstance eventInstance)
		{
			if (eventInstance.isValid())
			{
				PLAYBACK_STATE playback_STATE;
				eventInstance.getPlaybackState(out playback_STATE);
				return playback_STATE != PLAYBACK_STATE.STOPPED;
			}
			return false;
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x001110CC File Offset: 0x0010F2CC
		protected virtual void OnCollision(Projectile_RL projectile, GameObject colliderObj)
		{
			this.m_lastGameObjectHit = colliderObj;
			if (!this.m_onlyPlayCollisionWhenHitTerrain && colliderObj.CompareTag("Breakable"))
			{
				AudioManager.PlayOneShot(this, this.HitBreakableEvent, base.transform.position);
				return;
			}
			if (colliderObj.CompareTag("Platform"))
			{
				AudioManager.PlayOneShot(this, this.HitSurfaceEvent, base.transform.position);
			}
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00111134 File Offset: 0x0010F334
		protected void OnDeath(Projectile_RL projectile, GameObject colliderObj)
		{
			if (this.GetIsEventPlaying(this.m_lifeTimeEventInstance))
			{
				AudioManager.Stop(this.m_lifeTimeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			if (this.m_playDeathAudio)
			{
				bool flag = true;
				if (this.m_lastGameObjectHit)
				{
					string name = this.m_lastGameObjectHit.name;
					if (name.StartsWith("DamageWall"))
					{
						AudioManager.PlayOneShotAttached(this, "event:/SFX/Spells/sfx_spell_blueWall_hit", base.gameObject);
						this.m_lastGameObjectHit = null;
						flag = false;
					}
					else if (name.StartsWith("StaticWall"))
					{
						AudioManager.PlayOneShotAttached(this, "event:/SFX/Spells/sfx_spell_shieldWall_damageAbsorb", base.gameObject);
						this.m_lastGameObjectHit = null;
						flag = false;
					}
				}
				if (flag)
				{
					AudioManager.PlayOneShotAttached(this, this.DeathEvent, base.gameObject);
				}
			}
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x001111E8 File Offset: 0x0010F3E8
		protected virtual void OnSpawn(Projectile_RL projectile, GameObject colliderObj)
		{
			if (this.m_playSpawnAudioAutomatically && this.m_playSpawnAudio)
			{
				AudioManager.PlayOneShotAttached(this, this.SpawnEvent, base.gameObject);
			}
			this.m_projectile.OnCollisionRelay.AddListener(this.m_onCollision, false);
			this.m_projectile.OnDeathRelay.AddListener(this.m_onDeath, false);
			if (this.m_playLifetimeAudio && this.HasLifetimeEvent)
			{
				AudioManager.PlayAttached(this, this.m_lifeTimeEventInstance, base.gameObject);
			}
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x00111269 File Offset: 0x0010F469
		public void SetPlaySpawnAudio(bool playSpawnAudio)
		{
			this.m_playSpawnAudio = playSpawnAudio;
		}

		// Token: 0x06004C06 RID: 19462 RVA: 0x00111272 File Offset: 0x0010F472
		public void SetPlayLifetimeAudio(bool playLifetimeAudio)
		{
			this.m_playLifetimeAudio = playLifetimeAudio;
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x0011127B File Offset: 0x0010F47B
		public void SetPlayDeathAudio(bool playDeathAudio)
		{
			this.m_playDeathAudio = playDeathAudio;
		}

		// Token: 0x04003FF5 RID: 16373
		[SerializeField]
		protected bool m_onlyPlayCollisionWhenHitTerrain;

		// Token: 0x04003FF6 RID: 16374
		[SerializeField]
		protected bool m_playSpawnAudioAutomatically = true;

		// Token: 0x04003FF7 RID: 16375
		protected Projectile_RL m_projectile;

		// Token: 0x04003FF8 RID: 16376
		protected string m_deathEvent;

		// Token: 0x04003FF9 RID: 16377
		protected string m_lifetimeEvent;

		// Token: 0x04003FFA RID: 16378
		protected string m_hitSurfaceEvent;

		// Token: 0x04003FFB RID: 16379
		protected EventInstance m_lifeTimeEventInstance;

		// Token: 0x04003FFC RID: 16380
		protected string m_description;

		// Token: 0x04003FFD RID: 16381
		protected string m_trimmedName = string.Empty;

		// Token: 0x04003FFE RID: 16382
		protected string m_spawnEvent;

		// Token: 0x04003FFF RID: 16383
		protected GameObject m_lastGameObjectHit;

		// Token: 0x04004000 RID: 16384
		private Action<Projectile_RL, GameObject> m_onCollision;

		// Token: 0x04004001 RID: 16385
		private Action<Projectile_RL, GameObject> m_onDeath;

		// Token: 0x04004002 RID: 16386
		private bool m_playSpawnAudio = true;

		// Token: 0x04004003 RID: 16387
		private bool m_playLifetimeAudio = true;

		// Token: 0x04004004 RID: 16388
		private bool m_playDeathAudio = true;

		// Token: 0x04004005 RID: 16389
		protected const string WALL_SPELL_PROJECTILE_NAME_STARTS_WITH = "DamageWall";

		// Token: 0x04004006 RID: 16390
		protected const string PROJECTILE_DEATH_DUE_TO_WALL_SPELL_AUDIO_PATH = "event:/SFX/Spells/sfx_spell_blueWall_hit";

		// Token: 0x04004007 RID: 16391
		protected const string STATIC_WALL_PROJECTILE_NAME_STARTS_WITH = "StaticWall";

		// Token: 0x04004008 RID: 16392
		protected const string PROJECTILE_DEATH_DUE_TO_STATIC_WALL_AUDIO_PATH = "event:/SFX/Spells/sfx_spell_shieldWall_damageAbsorb";
	}
}
