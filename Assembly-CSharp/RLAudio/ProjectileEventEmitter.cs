using System;
using FMOD.Studio;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E89 RID: 3721
	public class ProjectileEventEmitter : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x1700216F RID: 8559
		// (get) Token: 0x060068EC RID: 26860 RVA: 0x0003A1DF File Offset: 0x000383DF
		// (set) Token: 0x060068ED RID: 26861 RVA: 0x0003A1E7 File Offset: 0x000383E7
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

		// Token: 0x17002170 RID: 8560
		// (get) Token: 0x060068EE RID: 26862 RVA: 0x0003A1F0 File Offset: 0x000383F0
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

		// Token: 0x17002171 RID: 8561
		// (get) Token: 0x060068EF RID: 26863 RVA: 0x0003A216 File Offset: 0x00038416
		private bool HasLifetimeEvent
		{
			get
			{
				return !string.IsNullOrEmpty(this.LifetimeEvent);
			}
		}

		// Token: 0x17002172 RID: 8562
		// (get) Token: 0x060068F0 RID: 26864 RVA: 0x0003A226 File Offset: 0x00038426
		// (set) Token: 0x060068F1 RID: 26865 RVA: 0x0003A22E File Offset: 0x0003842E
		public string HitBreakableEvent { get; private set; }

		// Token: 0x17002173 RID: 8563
		// (get) Token: 0x060068F2 RID: 26866 RVA: 0x0003A237 File Offset: 0x00038437
		// (set) Token: 0x060068F3 RID: 26867 RVA: 0x0003A23F File Offset: 0x0003843F
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

		// Token: 0x17002174 RID: 8564
		// (get) Token: 0x060068F4 RID: 26868 RVA: 0x0003A248 File Offset: 0x00038448
		// (set) Token: 0x060068F5 RID: 26869 RVA: 0x0003A250 File Offset: 0x00038450
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

		// Token: 0x17002175 RID: 8565
		// (get) Token: 0x060068F6 RID: 26870 RVA: 0x0003A259 File Offset: 0x00038459
		// (set) Token: 0x060068F7 RID: 26871 RVA: 0x0003A261 File Offset: 0x00038461
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

		// Token: 0x060068F8 RID: 26872 RVA: 0x00180FC8 File Offset: 0x0017F1C8
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

		// Token: 0x060068F9 RID: 26873 RVA: 0x001810D4 File Offset: 0x0017F2D4
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

		// Token: 0x060068FA RID: 26874 RVA: 0x0003A26A File Offset: 0x0003846A
		protected virtual void OnDisable()
		{
			if (this.GetIsEventPlaying(this.m_lifeTimeEventInstance))
			{
				AudioManager.Stop(this.m_lifeTimeEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
		}

		// Token: 0x060068FB RID: 26875 RVA: 0x0018110C File Offset: 0x0017F30C
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

		// Token: 0x060068FC RID: 26876 RVA: 0x00181180 File Offset: 0x0017F380
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

		// Token: 0x060068FD RID: 26877 RVA: 0x001811AC File Offset: 0x0017F3AC
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

		// Token: 0x060068FE RID: 26878 RVA: 0x00181214 File Offset: 0x0017F414
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

		// Token: 0x060068FF RID: 26879 RVA: 0x001812C8 File Offset: 0x0017F4C8
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

		// Token: 0x06006900 RID: 26880 RVA: 0x0003A286 File Offset: 0x00038486
		public void SetPlaySpawnAudio(bool playSpawnAudio)
		{
			this.m_playSpawnAudio = playSpawnAudio;
		}

		// Token: 0x06006901 RID: 26881 RVA: 0x0003A28F File Offset: 0x0003848F
		public void SetPlayLifetimeAudio(bool playLifetimeAudio)
		{
			this.m_playLifetimeAudio = playLifetimeAudio;
		}

		// Token: 0x06006902 RID: 26882 RVA: 0x0003A298 File Offset: 0x00038498
		public void SetPlayDeathAudio(bool playDeathAudio)
		{
			this.m_playDeathAudio = playDeathAudio;
		}

		// Token: 0x04005555 RID: 21845
		[SerializeField]
		protected bool m_onlyPlayCollisionWhenHitTerrain;

		// Token: 0x04005556 RID: 21846
		[SerializeField]
		protected bool m_playSpawnAudioAutomatically = true;

		// Token: 0x04005557 RID: 21847
		protected Projectile_RL m_projectile;

		// Token: 0x04005558 RID: 21848
		protected string m_deathEvent;

		// Token: 0x04005559 RID: 21849
		protected string m_lifetimeEvent;

		// Token: 0x0400555A RID: 21850
		protected string m_hitSurfaceEvent;

		// Token: 0x0400555B RID: 21851
		protected EventInstance m_lifeTimeEventInstance;

		// Token: 0x0400555C RID: 21852
		protected string m_description;

		// Token: 0x0400555D RID: 21853
		protected string m_trimmedName = string.Empty;

		// Token: 0x0400555E RID: 21854
		protected string m_spawnEvent;

		// Token: 0x0400555F RID: 21855
		protected GameObject m_lastGameObjectHit;

		// Token: 0x04005560 RID: 21856
		private Action<Projectile_RL, GameObject> m_onCollision;

		// Token: 0x04005561 RID: 21857
		private Action<Projectile_RL, GameObject> m_onDeath;

		// Token: 0x04005562 RID: 21858
		private bool m_playSpawnAudio = true;

		// Token: 0x04005563 RID: 21859
		private bool m_playLifetimeAudio = true;

		// Token: 0x04005564 RID: 21860
		private bool m_playDeathAudio = true;

		// Token: 0x04005565 RID: 21861
		protected const string WALL_SPELL_PROJECTILE_NAME_STARTS_WITH = "DamageWall";

		// Token: 0x04005566 RID: 21862
		protected const string PROJECTILE_DEATH_DUE_TO_WALL_SPELL_AUDIO_PATH = "event:/SFX/Spells/sfx_spell_blueWall_hit";

		// Token: 0x04005567 RID: 21863
		protected const string STATIC_WALL_PROJECTILE_NAME_STARTS_WITH = "StaticWall";

		// Token: 0x04005568 RID: 21864
		protected const string PROJECTILE_DEATH_DUE_TO_STATIC_WALL_AUDIO_PATH = "event:/SFX/Spells/sfx_spell_shieldWall_damageAbsorb";
	}
}
