using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

namespace RLAudio
{
	// Token: 0x020008EE RID: 2286
	public class DualChoiceSpecialRoomAudioEventEmitterController : MonoBehaviour, IAudioEventEmitter
	{
		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x0010E38E File Offset: 0x0010C58E
		public string Description
		{
			get
			{
				if (this.m_description == string.Empty)
				{
					this.m_description = this.ToString();
				}
				return this.m_description;
			}
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x0010E3B4 File Offset: 0x0010C5B4
		private void Awake()
		{
			this.m_room = base.GetComponent<Room>();
			this.m_room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
			this.m_specialRoomController = base.GetComponent<BaseSpecialRoomController>();
			this.m_specialRoomController.RoomCompletedRelay.AddListener(new Action(this.OnRoomComplete), false);
			this.m_onLeftChoiceHover = new Action<GameObject>(this.OnLeftChoiceHover);
			this.m_onRightChoiceHover = new Action<GameObject>(this.OnRightChoiceHover);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x0010E43C File Offset: 0x0010C63C
		protected virtual void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
		{
			if (!this.m_dualChoiceController)
			{
				PropSpawnController[] propSpawnControllers = this.m_room.SpawnControllerManager.PropSpawnControllers;
				for (int i = 0; i < propSpawnControllers.Length; i++)
				{
					if (propSpawnControllers[i].ShouldSpawn && !(propSpawnControllers[i].PropInstance == null))
					{
						this.m_dualChoiceController = propSpawnControllers[i].PropInstance.gameObject.GetComponent<DualChoicePropController>();
						if (this.m_dualChoiceController)
						{
							Interactable[] componentsInChildren = propSpawnControllers[i].PropInstance.gameObject.GetComponentsInChildren<Interactable>();
							for (int j = 0; j < componentsInChildren.Length; j++)
							{
								if (!componentsInChildren[j].TriggerOnTouch)
								{
									componentsInChildren[j].m_triggerOnEnterEvent.AddListener(new UnityAction<GameObject>(this.OnChoiceMade));
								}
							}
							break;
						}
					}
				}
			}
			if (this.m_dualChoiceController)
			{
				this.m_dualChoiceController.LeftHoverRelay.AddListener(this.m_onLeftChoiceHover, false);
				this.m_dualChoiceController.RightHoverRelay.AddListener(this.m_onRightChoiceHover, false);
			}
			if (!this.m_isRoomComplete)
			{
				AudioManager.PlayOneShot(this, this.m_roomDiscoveredAudioEventPath, default(Vector3));
			}
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x0010E55E File Offset: 0x0010C75E
		private void OnDisable()
		{
			if (this.m_dualChoiceController)
			{
				this.m_dualChoiceController.LeftHoverRelay.RemoveListener(this.m_onLeftChoiceHover);
				this.m_dualChoiceController.RightHoverRelay.RemoveListener(this.m_onRightChoiceHover);
			}
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x0010E59C File Offset: 0x0010C79C
		private void OnChoiceMade(GameObject gameObj)
		{
			if (this.m_choiceMadeUnityEvent != null)
			{
				this.m_choiceMadeUnityEvent.Invoke();
			}
			string path = this.m_leftChosenAudioEventPath;
			if (this.m_currentChoice == RoomSide.Right)
			{
				path = this.m_rightChosenAudioEventPath;
			}
			this.PlayChoiceAudio(path, gameObj.transform.position);
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x0010E5E5 File Offset: 0x0010C7E5
		private void OnLeftChoiceHover(GameObject gameObj)
		{
			this.m_currentChoice = RoomSide.Left;
			this.m_currentChoiceGameObject = gameObj;
			this.PlayChoiceAudio(this.m_rightHoverAudioEventPath, gameObj.transform.position);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x0010E60C File Offset: 0x0010C80C
		private void OnRightChoiceHover(GameObject gameObj)
		{
			this.m_currentChoice = RoomSide.Right;
			this.m_currentChoiceGameObject = gameObj;
			this.PlayChoiceAudio(this.m_rightHoverAudioEventPath, gameObj.transform.position);
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x0010E633 File Offset: 0x0010C833
		private void PlayChoiceAudio(string path, Vector3 position)
		{
			AudioManager.PlayOneShot(this, path, position);
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x0010E640 File Offset: 0x0010C840
		protected virtual void OnRoomComplete()
		{
			AudioManager.PlayOneShot(this, this.m_roomCompleteAudioEventPath, default(Vector3));
			this.m_isRoomComplete = true;
		}

		// Token: 0x04003F22 RID: 16162
		[SerializeField]
		[EventRef]
		private string m_leftHoverAudioEventPath;

		// Token: 0x04003F23 RID: 16163
		[SerializeField]
		[EventRef]
		private string m_rightHoverAudioEventPath;

		// Token: 0x04003F24 RID: 16164
		[SerializeField]
		[EventRef]
		private string m_leftChosenAudioEventPath;

		// Token: 0x04003F25 RID: 16165
		[SerializeField]
		[EventRef]
		private string m_rightChosenAudioEventPath;

		// Token: 0x04003F26 RID: 16166
		[SerializeField]
		[EventRef]
		private string m_roomDiscoveredAudioEventPath;

		// Token: 0x04003F27 RID: 16167
		[SerializeField]
		[EventRef]
		private string m_roomCompleteAudioEventPath;

		// Token: 0x04003F28 RID: 16168
		[SerializeField]
		private UnityEvent m_choiceMadeUnityEvent;

		// Token: 0x04003F29 RID: 16169
		private string m_description = string.Empty;

		// Token: 0x04003F2A RID: 16170
		private RoomSide m_currentChoice = RoomSide.None;

		// Token: 0x04003F2B RID: 16171
		private GameObject m_currentChoiceGameObject;

		// Token: 0x04003F2C RID: 16172
		protected bool m_isRoomComplete;

		// Token: 0x04003F2D RID: 16173
		private DualChoicePropController m_dualChoiceController;

		// Token: 0x04003F2E RID: 16174
		private BaseSpecialRoomController m_specialRoomController;

		// Token: 0x04003F2F RID: 16175
		private Room m_room;

		// Token: 0x04003F30 RID: 16176
		private Action<GameObject> m_onLeftChoiceHover;

		// Token: 0x04003F31 RID: 16177
		private Action<GameObject> m_onRightChoiceHover;
	}
}
