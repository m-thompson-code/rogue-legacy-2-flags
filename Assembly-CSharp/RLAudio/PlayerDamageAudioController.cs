using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000905 RID: 2309
	public class PlayerDamageAudioController : DamageAudioController
	{
		// Token: 0x06004BC1 RID: 19393 RVA: 0x00110548 File Offset: 0x0010E748
		protected override void Awake()
		{
			this.m_hitEventInstance = AudioUtility.GetEventInstance(this.m_playerHitAudioPath, base.transform);
			this.m_bardHitEventInstance = AudioUtility.GetEventInstance(this.m_bardHitAudioPath, base.transform);
			this.m_deathEventInstance = AudioUtility.GetEventInstance(this.m_playerDeathAudioPath, base.transform);
			this.m_bardDeathEventInstance = AudioUtility.GetEventInstance(this.m_bardDeathAudioPath, base.transform);
			this.m_killingBlowEventInstance = AudioUtility.GetEventInstance(this.m_killingBlowAudioPath, base.transform);
			this.m_armourHitEventInstance = AudioUtility.GetEventInstance(this.m_armourHitAudioPath, base.transform);
			this.m_armourBreakEventInstance = AudioUtility.GetEventInstance(this.m_armourBreakAudioPath, base.transform);
			this.m_playerHealth = base.GetComponent<IHealth>();
			base.Awake();
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00110608 File Offset: 0x0010E808
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (this.m_hitEventInstance.isValid())
			{
				this.m_hitEventInstance.release();
			}
			if (this.m_bardHitEventInstance.isValid())
			{
				this.m_bardHitEventInstance.release();
			}
			if (this.m_deathEventInstance.isValid())
			{
				this.m_deathEventInstance.release();
			}
			if (this.m_bardDeathEventInstance.isValid())
			{
				this.m_bardDeathEventInstance.release();
			}
			if (this.m_killingBlowEventInstance.isValid())
			{
				this.m_killingBlowEventInstance.release();
			}
			if (this.m_armourHitEventInstance.isValid())
			{
				this.m_armourHitEventInstance.release();
			}
			if (this.m_armourBreakEventInstance.isValid())
			{
				this.m_armourBreakEventInstance.release();
			}
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x001106CC File Offset: 0x0010E8CC
		private void SetAudioEventsGenderAndSizeParameters()
		{
			float playerGenderAudioParameterValue = AudioUtility.GetPlayerGenderAudioParameterValue();
			float playerSizeAudioParameterValue = AudioUtility.GetPlayerSizeAudioParameterValue();
			EventInstance eventInstance;
			EventInstance eventInstance2;
			this.GetCurrentClassEventInstances(out eventInstance, out eventInstance2);
			if (eventInstance.isValid())
			{
				eventInstance.setParameterByName("gender", playerGenderAudioParameterValue, false);
				eventInstance.setParameterByName("Player_Size", playerSizeAudioParameterValue, false);
			}
			if (eventInstance2.isValid())
			{
				eventInstance2.setParameterByName("gender", playerGenderAudioParameterValue, false);
				eventInstance2.setParameterByName("Player_Size", playerSizeAudioParameterValue, false);
			}
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00110740 File Offset: 0x0010E940
		private void GetCurrentClassEventInstances(out EventInstance hitEvent, out EventInstance deathEvent)
		{
			if (PlayerManager.GetPlayerController().CharacterClass.ClassType == ClassType.LuteClass)
			{
				hitEvent = this.m_bardHitEventInstance;
				deathEvent = this.m_bardDeathEventInstance;
				return;
			}
			hitEvent = this.m_hitEventInstance;
			deathEvent = this.m_deathEventInstance;
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00110794 File Offset: 0x0010E994
		protected override void OnTakeDamage(GameObject attacker, float damageTaken, bool isCrit)
		{
			this.SetAudioEventsGenderAndSizeParameters();
			base.OnTakeDamage(attacker, damageTaken, isCrit);
			EventInstance damageAudio = this.GetDamageAudio();
			if (damageAudio.isValid())
			{
				AudioManager.PlayAttached(this, damageAudio, base.gameObject);
			}
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x001107D0 File Offset: 0x0010E9D0
		private EventInstance GetDamageAudio()
		{
			bool flag = this.m_playerHealth.CurrentHealth <= 0f;
			int currentArmor = PlayerManager.GetPlayerController().CurrentArmor;
			bool flag2 = currentArmor > 0;
			EventInstance result;
			EventInstance eventInstance;
			this.GetCurrentClassEventInstances(out result, out eventInstance);
			if (flag)
			{
				result = this.m_killingBlowEventInstance;
			}
			else if (flag2)
			{
				result = this.m_armourHitEventInstance;
			}
			else if (this.m_previousArmourValue > 0)
			{
				result = this.m_armourBreakEventInstance;
			}
			this.m_previousArmourValue = currentArmor;
			return result;
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x0011083C File Offset: 0x0010EA3C
		protected override void OnDeath(GameObject otherObj)
		{
			base.OnDeath(otherObj);
			EventInstance eventInstance;
			EventInstance eventInstance2;
			this.GetCurrentClassEventInstances(out eventInstance, out eventInstance2);
			if (eventInstance2.isValid())
			{
				AudioManager.PlayAttached(this, eventInstance2, base.gameObject);
			}
		}

		// Token: 0x04003FCA RID: 16330
		[SerializeField]
		[EventRef]
		private string m_playerHitAudioPath;

		// Token: 0x04003FCB RID: 16331
		[SerializeField]
		[EventRef]
		private string m_bardHitAudioPath;

		// Token: 0x04003FCC RID: 16332
		[SerializeField]
		[EventRef]
		private string m_playerDeathAudioPath;

		// Token: 0x04003FCD RID: 16333
		[SerializeField]
		[EventRef]
		private string m_bardDeathAudioPath;

		// Token: 0x04003FCE RID: 16334
		[SerializeField]
		[EventRef]
		private string m_killingBlowAudioPath;

		// Token: 0x04003FCF RID: 16335
		[SerializeField]
		[EventRef]
		private string m_armourHitAudioPath;

		// Token: 0x04003FD0 RID: 16336
		[SerializeField]
		[EventRef]
		private string m_armourBreakAudioPath;

		// Token: 0x04003FD1 RID: 16337
		private IHealth m_playerHealth;

		// Token: 0x04003FD2 RID: 16338
		private EventInstance m_hitEventInstance;

		// Token: 0x04003FD3 RID: 16339
		private EventInstance m_deathEventInstance;

		// Token: 0x04003FD4 RID: 16340
		private EventInstance m_killingBlowEventInstance;

		// Token: 0x04003FD5 RID: 16341
		private EventInstance m_armourHitEventInstance;

		// Token: 0x04003FD6 RID: 16342
		private EventInstance m_armourBreakEventInstance;

		// Token: 0x04003FD7 RID: 16343
		private EventInstance m_bardHitEventInstance;

		// Token: 0x04003FD8 RID: 16344
		private EventInstance m_bardDeathEventInstance;

		// Token: 0x04003FD9 RID: 16345
		private int m_previousArmourValue = -1;
	}
}
