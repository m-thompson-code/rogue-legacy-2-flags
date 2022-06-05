using System;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch
{
	// Token: 0x02000EB0 RID: 3760
	[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x06006BAB RID: 27563 RVA: 0x0000F49B File Offset: 0x0000D69B
		object IExternalInputManager.Initialize(Platform platform, object configVars)
		{
			return null;
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x00002FCA File Offset: 0x000011CA
		void IExternalInputManager.Deinitialize()
		{
		}

		// Token: 0x04005782 RID: 22402
		[SerializeField]
		private NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

		// Token: 0x02000EB1 RID: 3761
		[Serializable]
		private class UserData : IKeyedData<int>
		{
			// Token: 0x1700238F RID: 9103
			// (get) Token: 0x06006BAE RID: 27566 RVA: 0x0003B0A5 File Offset: 0x000392A5
			// (set) Token: 0x06006BAF RID: 27567 RVA: 0x0003B0AD File Offset: 0x000392AD
			public int allowedNpadStyles
			{
				get
				{
					return this._allowedNpadStyles;
				}
				set
				{
					this._allowedNpadStyles = value;
				}
			}

			// Token: 0x17002390 RID: 9104
			// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x0003B0B6 File Offset: 0x000392B6
			// (set) Token: 0x06006BB1 RID: 27569 RVA: 0x0003B0BE File Offset: 0x000392BE
			public int joyConGripStyle
			{
				get
				{
					return this._joyConGripStyle;
				}
				set
				{
					this._joyConGripStyle = value;
				}
			}

			// Token: 0x17002391 RID: 9105
			// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x0003B0C7 File Offset: 0x000392C7
			// (set) Token: 0x06006BB3 RID: 27571 RVA: 0x0003B0CF File Offset: 0x000392CF
			public bool adjustIMUsForGripStyle
			{
				get
				{
					return this._adjustIMUsForGripStyle;
				}
				set
				{
					this._adjustIMUsForGripStyle = value;
				}
			}

			// Token: 0x17002392 RID: 9106
			// (get) Token: 0x06006BB4 RID: 27572 RVA: 0x0003B0D8 File Offset: 0x000392D8
			// (set) Token: 0x06006BB5 RID: 27573 RVA: 0x0003B0E0 File Offset: 0x000392E0
			public int handheldActivationMode
			{
				get
				{
					return this._handheldActivationMode;
				}
				set
				{
					this._handheldActivationMode = value;
				}
			}

			// Token: 0x17002393 RID: 9107
			// (get) Token: 0x06006BB6 RID: 27574 RVA: 0x0003B0E9 File Offset: 0x000392E9
			// (set) Token: 0x06006BB7 RID: 27575 RVA: 0x0003B0F1 File Offset: 0x000392F1
			public bool assignJoysticksByNpadId
			{
				get
				{
					return this._assignJoysticksByNpadId;
				}
				set
				{
					this._assignJoysticksByNpadId = value;
				}
			}

			// Token: 0x17002394 RID: 9108
			// (get) Token: 0x06006BB8 RID: 27576 RVA: 0x0003B0FA File Offset: 0x000392FA
			// (set) Token: 0x06006BB9 RID: 27577 RVA: 0x0003B102 File Offset: 0x00039302
			public bool useVibrationThread
			{
				get
				{
					return this._useVibrationThread;
				}
				set
				{
					this._useVibrationThread = value;
				}
			}

			// Token: 0x17002395 RID: 9109
			// (get) Token: 0x06006BBA RID: 27578 RVA: 0x0003B10B File Offset: 0x0003930B
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x17002396 RID: 9110
			// (get) Token: 0x06006BBB RID: 27579 RVA: 0x0003B113 File Offset: 0x00039313
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x17002397 RID: 9111
			// (get) Token: 0x06006BBC RID: 27580 RVA: 0x0003B11B File Offset: 0x0003931B
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x17002398 RID: 9112
			// (get) Token: 0x06006BBD RID: 27581 RVA: 0x0003B123 File Offset: 0x00039323
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x17002399 RID: 9113
			// (get) Token: 0x06006BBE RID: 27582 RVA: 0x0003B12B File Offset: 0x0003932B
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x1700239A RID: 9114
			// (get) Token: 0x06006BBF RID: 27583 RVA: 0x0003B133 File Offset: 0x00039333
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x1700239B RID: 9115
			// (get) Token: 0x06006BC0 RID: 27584 RVA: 0x0003B13B File Offset: 0x0003933B
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x1700239C RID: 9116
			// (get) Token: 0x06006BC1 RID: 27585 RVA: 0x0003B143 File Offset: 0x00039343
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x1700239D RID: 9117
			// (get) Token: 0x06006BC2 RID: 27586 RVA: 0x0003B14B File Offset: 0x0003934B
			private NintendoSwitchInputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x1700239E RID: 9118
			// (get) Token: 0x06006BC3 RID: 27587 RVA: 0x0003B153 File Offset: 0x00039353
			public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x1700239F RID: 9119
			// (get) Token: 0x06006BC4 RID: 27588 RVA: 0x001829C0 File Offset: 0x00180BC0
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					dictionary.Add(0, new object[]
					{
						new Func<int>(() => this.allowedNpadStyles),
						new Action<int>(delegate(int x)
						{
							this.allowedNpadStyles = x;
						})
					});
					dictionary.Add(1, new object[]
					{
						new Func<int>(() => this.joyConGripStyle),
						new Action<int>(delegate(int x)
						{
							this.joyConGripStyle = x;
						})
					});
					dictionary.Add(2, new object[]
					{
						new Func<bool>(() => this.adjustIMUsForGripStyle),
						new Action<bool>(delegate(bool x)
						{
							this.adjustIMUsForGripStyle = x;
						})
					});
					dictionary.Add(3, new object[]
					{
						new Func<int>(() => this.handheldActivationMode),
						new Action<int>(delegate(int x)
						{
							this.handheldActivationMode = x;
						})
					});
					dictionary.Add(4, new object[]
					{
						new Func<bool>(() => this.assignJoysticksByNpadId),
						new Action<bool>(delegate(bool x)
						{
							this.assignJoysticksByNpadId = x;
						})
					});
					Dictionary<int, object[]> dictionary2 = dictionary;
					int key = 5;
					object[] array = new object[2];
					array[0] = new Func<object>(() => this.npadNo1);
					dictionary2.Add(key, array);
					Dictionary<int, object[]> dictionary3 = dictionary;
					int key2 = 6;
					object[] array2 = new object[2];
					array2[0] = new Func<object>(() => this.npadNo2);
					dictionary3.Add(key2, array2);
					Dictionary<int, object[]> dictionary4 = dictionary;
					int key3 = 7;
					object[] array3 = new object[2];
					array3[0] = new Func<object>(() => this.npadNo3);
					dictionary4.Add(key3, array3);
					Dictionary<int, object[]> dictionary5 = dictionary;
					int key4 = 8;
					object[] array4 = new object[2];
					array4[0] = new Func<object>(() => this.npadNo4);
					dictionary5.Add(key4, array4);
					Dictionary<int, object[]> dictionary6 = dictionary;
					int key5 = 9;
					object[] array5 = new object[2];
					array5[0] = new Func<object>(() => this.npadNo5);
					dictionary6.Add(key5, array5);
					Dictionary<int, object[]> dictionary7 = dictionary;
					int key6 = 10;
					object[] array6 = new object[2];
					array6[0] = new Func<object>(() => this.npadNo6);
					dictionary7.Add(key6, array6);
					Dictionary<int, object[]> dictionary8 = dictionary;
					int key7 = 11;
					object[] array7 = new object[2];
					array7[0] = new Func<object>(() => this.npadNo7);
					dictionary8.Add(key7, array7);
					Dictionary<int, object[]> dictionary9 = dictionary;
					int key8 = 12;
					object[] array8 = new object[2];
					array8[0] = new Func<object>(() => this.npadNo8);
					dictionary9.Add(key8, array8);
					Dictionary<int, object[]> dictionary10 = dictionary;
					int key9 = 13;
					object[] array9 = new object[2];
					array9[0] = new Func<object>(() => this.npadHandheld);
					dictionary10.Add(key9, array9);
					Dictionary<int, object[]> dictionary11 = dictionary;
					int key10 = 14;
					object[] array10 = new object[2];
					array10[0] = new Func<object>(() => this.debugPad);
					dictionary11.Add(key10, array10);
					dictionary.Add(15, new object[]
					{
						new Func<bool>(() => this.useVibrationThread),
						new Action<bool>(delegate(bool x)
						{
							this.useVibrationThread = x;
						})
					});
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x06006BC5 RID: 27589 RVA: 0x00182C10 File Offset: 0x00180E10
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06006BC6 RID: 27590 RVA: 0x00182C58 File Offset: 0x00180E58
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x04005783 RID: 22403
			[SerializeField]
			private int _allowedNpadStyles = -1;

			// Token: 0x04005784 RID: 22404
			[SerializeField]
			private int _joyConGripStyle = 1;

			// Token: 0x04005785 RID: 22405
			[SerializeField]
			private bool _adjustIMUsForGripStyle = true;

			// Token: 0x04005786 RID: 22406
			[SerializeField]
			private int _handheldActivationMode;

			// Token: 0x04005787 RID: 22407
			[SerializeField]
			private bool _assignJoysticksByNpadId = true;

			// Token: 0x04005788 RID: 22408
			[SerializeField]
			private bool _useVibrationThread = true;

			// Token: 0x04005789 RID: 22409
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x0400578A RID: 22410
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);

			// Token: 0x0400578B RID: 22411
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);

			// Token: 0x0400578C RID: 22412
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);

			// Token: 0x0400578D RID: 22413
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);

			// Token: 0x0400578E RID: 22414
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);

			// Token: 0x0400578F RID: 22415
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);

			// Token: 0x04005790 RID: 22416
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);

			// Token: 0x04005791 RID: 22417
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x04005792 RID: 22418
			[SerializeField]
			private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);

			// Token: 0x04005793 RID: 22419
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000EB2 RID: 3762
		[Serializable]
		private sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x170023A0 RID: 9120
			// (get) Token: 0x06006BDE RID: 27614 RVA: 0x0003B211 File Offset: 0x00039411
			// (set) Token: 0x06006BDF RID: 27615 RVA: 0x0003B219 File Offset: 0x00039419
			private bool isAllowed
			{
				get
				{
					return this._isAllowed;
				}
				set
				{
					this._isAllowed = value;
				}
			}

			// Token: 0x170023A1 RID: 9121
			// (get) Token: 0x06006BE0 RID: 27616 RVA: 0x0003B222 File Offset: 0x00039422
			// (set) Token: 0x06006BE1 RID: 27617 RVA: 0x0003B22A File Offset: 0x0003942A
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x170023A2 RID: 9122
			// (get) Token: 0x06006BE2 RID: 27618 RVA: 0x0003B233 File Offset: 0x00039433
			// (set) Token: 0x06006BE3 RID: 27619 RVA: 0x0003B23B File Offset: 0x0003943B
			private int joyConAssignmentMode
			{
				get
				{
					return this._joyConAssignmentMode;
				}
				set
				{
					this._joyConAssignmentMode = value;
				}
			}

			// Token: 0x06006BE4 RID: 27620 RVA: 0x0003B244 File Offset: 0x00039444
			internal NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x170023A3 RID: 9123
			// (get) Token: 0x06006BE5 RID: 27621 RVA: 0x00182D40 File Offset: 0x00180F40
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(() => this.isAllowed),
								new Action<bool>(delegate(bool x)
								{
									this.isAllowed = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(() => this.rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						},
						{
							2,
							new object[]
							{
								new Func<int>(() => this.joyConAssignmentMode),
								new Action<int>(delegate(int x)
								{
									this.joyConAssignmentMode = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x06006BE6 RID: 27622 RVA: 0x00182DF0 File Offset: 0x00180FF0
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06006BE7 RID: 27623 RVA: 0x00182E38 File Offset: 0x00181038
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x04005794 RID: 22420
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			private bool _isAllowed = true;

			// Token: 0x04005795 RID: 22421
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x04005796 RID: 22422
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			private int _joyConAssignmentMode = -1;

			// Token: 0x04005797 RID: 22423
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000EB3 RID: 3763
		[Serializable]
		private sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x170023A4 RID: 9124
			// (get) Token: 0x06006BEE RID: 27630 RVA: 0x0003B294 File Offset: 0x00039494
			// (set) Token: 0x06006BEF RID: 27631 RVA: 0x0003B29C File Offset: 0x0003949C
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x170023A5 RID: 9125
			// (get) Token: 0x06006BF0 RID: 27632 RVA: 0x0003B2A5 File Offset: 0x000394A5
			// (set) Token: 0x06006BF1 RID: 27633 RVA: 0x0003B2AD File Offset: 0x000394AD
			private bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					this._enabled = value;
				}
			}

			// Token: 0x06006BF2 RID: 27634 RVA: 0x0003B2B6 File Offset: 0x000394B6
			internal DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x170023A6 RID: 9126
			// (get) Token: 0x06006BF3 RID: 27635 RVA: 0x00182E70 File Offset: 0x00181070
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(() => this.enabled),
								new Action<bool>(delegate(bool x)
								{
									this.enabled = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(() => this.rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x06006BF4 RID: 27636 RVA: 0x00182EF4 File Offset: 0x001810F4
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06006BF5 RID: 27637 RVA: 0x00182F3C File Offset: 0x0018113C
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x04005798 RID: 22424
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			private bool _enabled;

			// Token: 0x04005799 RID: 22425
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x0400579A RID: 22426
			private Dictionary<int, object[]> __delegates;
		}
	}
}
