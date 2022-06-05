using System;
using System.Collections.Generic;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch
{
	// Token: 0x02000931 RID: 2353
	[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x06004EA4 RID: 20132 RVA: 0x00113A09 File Offset: 0x00111C09
		object IExternalInputManager.Initialize(Platform platform, object configVars)
		{
			return null;
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x00113A0C File Offset: 0x00111C0C
		void IExternalInputManager.Deinitialize()
		{
		}

		// Token: 0x04004216 RID: 16918
		[SerializeField]
		private NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

		// Token: 0x02000EF0 RID: 3824
		[Serializable]
		private class UserData : IKeyedData<int>
		{
			// Token: 0x17002428 RID: 9256
			// (get) Token: 0x06006F48 RID: 28488 RVA: 0x0019DA06 File Offset: 0x0019BC06
			// (set) Token: 0x06006F49 RID: 28489 RVA: 0x0019DA0E File Offset: 0x0019BC0E
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

			// Token: 0x17002429 RID: 9257
			// (get) Token: 0x06006F4A RID: 28490 RVA: 0x0019DA17 File Offset: 0x0019BC17
			// (set) Token: 0x06006F4B RID: 28491 RVA: 0x0019DA1F File Offset: 0x0019BC1F
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

			// Token: 0x1700242A RID: 9258
			// (get) Token: 0x06006F4C RID: 28492 RVA: 0x0019DA28 File Offset: 0x0019BC28
			// (set) Token: 0x06006F4D RID: 28493 RVA: 0x0019DA30 File Offset: 0x0019BC30
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

			// Token: 0x1700242B RID: 9259
			// (get) Token: 0x06006F4E RID: 28494 RVA: 0x0019DA39 File Offset: 0x0019BC39
			// (set) Token: 0x06006F4F RID: 28495 RVA: 0x0019DA41 File Offset: 0x0019BC41
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

			// Token: 0x1700242C RID: 9260
			// (get) Token: 0x06006F50 RID: 28496 RVA: 0x0019DA4A File Offset: 0x0019BC4A
			// (set) Token: 0x06006F51 RID: 28497 RVA: 0x0019DA52 File Offset: 0x0019BC52
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

			// Token: 0x1700242D RID: 9261
			// (get) Token: 0x06006F52 RID: 28498 RVA: 0x0019DA5B File Offset: 0x0019BC5B
			// (set) Token: 0x06006F53 RID: 28499 RVA: 0x0019DA63 File Offset: 0x0019BC63
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

			// Token: 0x1700242E RID: 9262
			// (get) Token: 0x06006F54 RID: 28500 RVA: 0x0019DA6C File Offset: 0x0019BC6C
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x1700242F RID: 9263
			// (get) Token: 0x06006F55 RID: 28501 RVA: 0x0019DA74 File Offset: 0x0019BC74
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x17002430 RID: 9264
			// (get) Token: 0x06006F56 RID: 28502 RVA: 0x0019DA7C File Offset: 0x0019BC7C
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x17002431 RID: 9265
			// (get) Token: 0x06006F57 RID: 28503 RVA: 0x0019DA84 File Offset: 0x0019BC84
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x17002432 RID: 9266
			// (get) Token: 0x06006F58 RID: 28504 RVA: 0x0019DA8C File Offset: 0x0019BC8C
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x17002433 RID: 9267
			// (get) Token: 0x06006F59 RID: 28505 RVA: 0x0019DA94 File Offset: 0x0019BC94
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x17002434 RID: 9268
			// (get) Token: 0x06006F5A RID: 28506 RVA: 0x0019DA9C File Offset: 0x0019BC9C
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x17002435 RID: 9269
			// (get) Token: 0x06006F5B RID: 28507 RVA: 0x0019DAA4 File Offset: 0x0019BCA4
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x17002436 RID: 9270
			// (get) Token: 0x06006F5C RID: 28508 RVA: 0x0019DAAC File Offset: 0x0019BCAC
			private NintendoSwitchInputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x17002437 RID: 9271
			// (get) Token: 0x06006F5D RID: 28509 RVA: 0x0019DAB4 File Offset: 0x0019BCB4
			public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x17002438 RID: 9272
			// (get) Token: 0x06006F5E RID: 28510 RVA: 0x0019DABC File Offset: 0x0019BCBC
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

			// Token: 0x06006F5F RID: 28511 RVA: 0x0019DD0C File Offset: 0x0019BF0C
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

			// Token: 0x06006F60 RID: 28512 RVA: 0x0019DD54 File Offset: 0x0019BF54
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

			// Token: 0x040059E0 RID: 23008
			[SerializeField]
			private int _allowedNpadStyles = -1;

			// Token: 0x040059E1 RID: 23009
			[SerializeField]
			private int _joyConGripStyle = 1;

			// Token: 0x040059E2 RID: 23010
			[SerializeField]
			private bool _adjustIMUsForGripStyle = true;

			// Token: 0x040059E3 RID: 23011
			[SerializeField]
			private int _handheldActivationMode;

			// Token: 0x040059E4 RID: 23012
			[SerializeField]
			private bool _assignJoysticksByNpadId = true;

			// Token: 0x040059E5 RID: 23013
			[SerializeField]
			private bool _useVibrationThread = true;

			// Token: 0x040059E6 RID: 23014
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040059E7 RID: 23015
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);

			// Token: 0x040059E8 RID: 23016
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);

			// Token: 0x040059E9 RID: 23017
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);

			// Token: 0x040059EA RID: 23018
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);

			// Token: 0x040059EB RID: 23019
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);

			// Token: 0x040059EC RID: 23020
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);

			// Token: 0x040059ED RID: 23021
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);

			// Token: 0x040059EE RID: 23022
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040059EF RID: 23023
			[SerializeField]
			private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);

			// Token: 0x040059F0 RID: 23024
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000EF1 RID: 3825
		[Serializable]
		private sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x17002439 RID: 9273
			// (get) Token: 0x06006F78 RID: 28536 RVA: 0x0019DEF0 File Offset: 0x0019C0F0
			// (set) Token: 0x06006F79 RID: 28537 RVA: 0x0019DEF8 File Offset: 0x0019C0F8
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

			// Token: 0x1700243A RID: 9274
			// (get) Token: 0x06006F7A RID: 28538 RVA: 0x0019DF01 File Offset: 0x0019C101
			// (set) Token: 0x06006F7B RID: 28539 RVA: 0x0019DF09 File Offset: 0x0019C109
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

			// Token: 0x1700243B RID: 9275
			// (get) Token: 0x06006F7C RID: 28540 RVA: 0x0019DF12 File Offset: 0x0019C112
			// (set) Token: 0x06006F7D RID: 28541 RVA: 0x0019DF1A File Offset: 0x0019C11A
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

			// Token: 0x06006F7E RID: 28542 RVA: 0x0019DF23 File Offset: 0x0019C123
			internal NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x1700243C RID: 9276
			// (get) Token: 0x06006F7F RID: 28543 RVA: 0x0019DF40 File Offset: 0x0019C140
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

			// Token: 0x06006F80 RID: 28544 RVA: 0x0019DFF0 File Offset: 0x0019C1F0
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

			// Token: 0x06006F81 RID: 28545 RVA: 0x0019E038 File Offset: 0x0019C238
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

			// Token: 0x040059F1 RID: 23025
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			private bool _isAllowed = true;

			// Token: 0x040059F2 RID: 23026
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x040059F3 RID: 23027
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			private int _joyConAssignmentMode = -1;

			// Token: 0x040059F4 RID: 23028
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000EF2 RID: 3826
		[Serializable]
		private sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x1700243D RID: 9277
			// (get) Token: 0x06006F88 RID: 28552 RVA: 0x0019E0A0 File Offset: 0x0019C2A0
			// (set) Token: 0x06006F89 RID: 28553 RVA: 0x0019E0A8 File Offset: 0x0019C2A8
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

			// Token: 0x1700243E RID: 9278
			// (get) Token: 0x06006F8A RID: 28554 RVA: 0x0019E0B1 File Offset: 0x0019C2B1
			// (set) Token: 0x06006F8B RID: 28555 RVA: 0x0019E0B9 File Offset: 0x0019C2B9
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

			// Token: 0x06006F8C RID: 28556 RVA: 0x0019E0C2 File Offset: 0x0019C2C2
			internal DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x1700243F RID: 9279
			// (get) Token: 0x06006F8D RID: 28557 RVA: 0x0019E0D4 File Offset: 0x0019C2D4
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

			// Token: 0x06006F8E RID: 28558 RVA: 0x0019E158 File Offset: 0x0019C358
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

			// Token: 0x06006F8F RID: 28559 RVA: 0x0019E1A0 File Offset: 0x0019C3A0
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

			// Token: 0x040059F5 RID: 23029
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			private bool _enabled;

			// Token: 0x040059F6 RID: 23030
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x040059F7 RID: 23031
			private Dictionary<int, object[]> __delegates;
		}
	}
}
