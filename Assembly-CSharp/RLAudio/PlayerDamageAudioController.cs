using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace RLAudio
{
	// Token: 0x02000E81 RID: 3713
	public class PlayerDamageAudioController : DamageAudioController
	{
		// Token: 0x060068B6 RID: 26806 RVA: 0x001808A4 File Offset: 0x0017EAA4
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

		// Token: 0x060068B7 RID: 26807 RVA: 0x00180964 File Offset: 0x0017EB64
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

		// Token: 0x060068B8 RID: 26808 RVA: 0x00180A28 File Offset: 0x0017EC28
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

		// Token: 0x060068B9 RID: 26809 RVA: 0x00180A9C File Offset: 0x0017EC9C
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

		// Token: 0x060068BA RID: 26810 RVA: 0x00180AF0 File Offset: 0x0017ECF0
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

		// Token: 0x060068BB RID: 26811 RVA: 0x00180B2C File Offset: 0x0017ED2C
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

		// Token: 0x060068BC RID: 26812 RVA: 0x00180B98 File Offset: 0x0017ED98
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

		// Token: 0x04005527 RID: 21799
		[SerializeField]
		[EventRef]
		private string m_playerHitAudioPath;

		// Token: 0x04005528 RID: 21800
		[SerializeField]
		[EventRef]
		private string m_bardHitAudioPath;

		// Token: 0x04005529 RID: 21801
		[SerializeField]
		[EventRef]
		private string m_playerDeathAudioPath;

		// Token: 0x0400552A RID: 21802
		[SerializeField]
		[EventRef]
		private string m_bardDeathAudioPath;

		// Token: 0x0400552B RID: 21803
		[SerializeField]
		[EventRef]
		private string m_killingBlowAudioPath;

		// Token: 0x0400552C RID: 21804
		[SerializeField]
		[EventRef]
		private string m_armourHitAudioPath;

		// Token: 0x0400552D RID: 21805
		[SerializeField]
		[EventRef]
		private string m_armourBreakAudioPath;

		// Token: 0x0400552E RID: 21806
		private IHealth m_playerHealth;

		// Token: 0x0400552F RID: 21807
		private EventInstance m_hitEventInstance;

		// Token: 0x04005530 RID: 21808
		private EventInstance m_deathEventInstance;

		// Token: 0x04005531 RID: 21809
		private EventInstance m_killingBlowEventInstance;

		// Token: 0x04005532 RID: 21810
		private EventInstance m_armourHitEventInstance;

		// Token: 0x04005533 RID: 21811
		private EventInstance m_armourBreakEventInstance;

		// Token: 0x04005534 RID: 21812
		private EventInstance m_bardHitEventInstance;

		// Token: 0x04005535 RID: 21813
		private EventInstance m_bardDeathEventInstance;

		// Token: 0x04005536 RID: 21814
		private int m_previousArmourValue = -1;
	}
}
