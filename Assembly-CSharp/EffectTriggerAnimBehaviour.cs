using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B5 RID: 437
[SharedBetweenAnimators]
public class EffectTriggerAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x0600112F RID: 4399 RVA: 0x00031AE0 File Offset: 0x0002FCE0
	public static void ClearNormalizedStartCounters()
	{
		EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Clear();
	}

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06001130 RID: 4400 RVA: 0x00031AEC File Offset: 0x0002FCEC
	// (set) Token: 0x06001131 RID: 4401 RVA: 0x00031AF3 File Offset: 0x0002FCF3
	public static bool DISABLE_GLOBALLY { get; set; } = false;

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x06001132 RID: 4402 RVA: 0x00031AFB File Offset: 0x0002FCFB
	// (set) Token: 0x06001133 RID: 4403 RVA: 0x00031B03 File Offset: 0x0002FD03
	public EffectTriggerEntry[] TriggerArray
	{
		get
		{
			return this.m_effectTriggerEntryList;
		}
		set
		{
			this.m_effectTriggerEntryList = value;
		}
	}

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06001134 RID: 4404 RVA: 0x00031B0C File Offset: 0x0002FD0C
	// (set) Token: 0x06001135 RID: 4405 RVA: 0x00031B14 File Offset: 0x0002FD14
	public AnimBehaviourCondition[] ConditionArray
	{
		get
		{
			return this.m_animBehaviourConditionArray;
		}
		set
		{
			this.m_animBehaviourConditionArray = value;
		}
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x00031B20 File Offset: 0x0002FD20
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EffectTriggerAnimBehaviour.DISABLE_GLOBALLY)
		{
			return;
		}
		if (EffectManager.AnimatorEffectsDisabled(animator))
		{
			return;
		}
		if (this.m_animBehaviourConditionArray == null)
		{
			Debug.Log("AnimBehaviourConditionArray null on animator: " + animator.name + " StateInfoHash: " + stateInfo.shortNameHash.ToString());
			string text = "";
			foreach (EffectTriggerEntry effectTriggerEntry in this.m_effectTriggerEntryList)
			{
				text = text + effectTriggerEntry.EffectTypeName + ",  ";
			}
			Debug.Log("Effects: " + text);
			return;
		}
		for (int j = 0; j < this.m_animBehaviourConditionArray.Length; j++)
		{
			if (!this.m_animBehaviourConditionArray[j].IsTrue(animator))
			{
				return;
			}
		}
		for (int k = 0; k < this.m_effectTriggerEntryList.Length; k++)
		{
			EffectTriggerEntry effectTriggerEntry2 = this.m_effectTriggerEntryList[k];
			if (effectTriggerEntry2.EffectStartType == EffectStartType.FirstLoopOnly && effectTriggerEntry2.NormalizedStartTime <= 0f)
			{
				effectTriggerEntry2.EffectStartType = EffectStartType.None;
			}
			if (effectTriggerEntry2.EffectStartType == EffectStartType.None)
			{
				this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
			}
			else
			{
				int instanceID = animator.GetInstanceID();
				if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID))
				{
					EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID, 0);
				}
				else
				{
					EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID] = 0;
				}
			}
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x00031C74 File Offset: 0x0002FE74
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EffectTriggerAnimBehaviour.DISABLE_GLOBALLY)
		{
			return;
		}
		if (EffectManager.AnimatorEffectsDisabled(animator))
		{
			return;
		}
		if (this.m_animBehaviourConditionArray == null)
		{
			Debug.Log("AnimBehaviourConditionArray null on animator: " + animator.name + " StateInfoHash: " + stateInfo.shortNameHash.ToString());
			string text = "";
			foreach (EffectTriggerEntry effectTriggerEntry in this.m_effectTriggerEntryList)
			{
				text = text + effectTriggerEntry.EffectTypeName + ",  ";
			}
			Debug.Log("Effects: " + text);
			return;
		}
		for (int j = 0; j < this.m_animBehaviourConditionArray.Length; j++)
		{
			if (!this.m_animBehaviourConditionArray[j].IsTrue(animator))
			{
				return;
			}
		}
		for (int k = 0; k < this.m_effectTriggerEntryList.Length; k++)
		{
			EffectTriggerEntry effectTriggerEntry2 = this.m_effectTriggerEntryList[k];
			EffectStartType effectStartType = effectTriggerEntry2.EffectStartType;
			if (effectStartType != EffectStartType.None)
			{
				if (effectStartType != EffectStartType.FirstLoopOnly)
				{
					if (effectStartType == EffectStartType.OnceEveryLoop)
					{
						int instanceID = animator.GetInstanceID();
						if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID))
						{
							EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID, 0);
						}
						if (stateInfo.normalizedTime >= (float)EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID] + effectTriggerEntry2.NormalizedStartTime)
						{
							Dictionary<int, int> normalizedStartCounterDict = EffectTriggerAnimBehaviour.m_normalizedStartCounterDict;
							int num = instanceID;
							int i = normalizedStartCounterDict[num];
							normalizedStartCounterDict[num] = i + 1;
							this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
						}
					}
				}
				else
				{
					int instanceID2 = animator.GetInstanceID();
					if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID2))
					{
						EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID2, 0);
					}
					if (stateInfo.normalizedTime >= effectTriggerEntry2.NormalizedStartTime && EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID2] <= 0)
					{
						Dictionary<int, int> normalizedStartCounterDict2 = EffectTriggerAnimBehaviour.m_normalizedStartCounterDict;
						int i = instanceID2;
						int num = normalizedStartCounterDict2[i];
						normalizedStartCounterDict2[i] = num + 1;
						this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
					}
				}
			}
		}
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x00031E60 File Offset: 0x00030060
	private void InvokeTrigger(GameObject parentObj, Animator animator, EffectTriggerEntry entry, int layerIndex)
	{
		Vector3 vector = parentObj.transform.position;
		IMidpointObj component = parentObj.GetComponent<IMidpointObj>();
		if (component != null)
		{
			vector = component.Midpoint;
		}
		EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(parentObj, entry.TriggerDirection);
		EffectTrigger.InvokeTrigger(entry, parentObj, parentObj, vector, vector, effectDirectionFromObject, animator);
	}

	// Token: 0x04001220 RID: 4640
	private static Dictionary<int, int> m_normalizedStartCounterDict = new Dictionary<int, int>();

	// Token: 0x04001221 RID: 4641
	[SerializeField]
	private EffectTriggerEntry[] m_effectTriggerEntryList = new EffectTriggerEntry[0];

	// Token: 0x04001222 RID: 4642
	[SerializeField]
	private AnimBehaviourCondition[] m_animBehaviourConditionArray = new AnimBehaviourCondition[0];
}
