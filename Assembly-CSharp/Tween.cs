using System;
using System.Collections;
using System.Reflection;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000CF2 RID: 3314
public class Tween : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x17001F0B RID: 7947
	// (get) Token: 0x06005E65 RID: 24165 RVA: 0x00034073 File Offset: 0x00032273
	// (set) Token: 0x06005E66 RID: 24166 RVA: 0x0003407B File Offset: 0x0003227B
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001F0C RID: 7948
	// (get) Token: 0x06005E67 RID: 24167 RVA: 0x00034084 File Offset: 0x00032284
	// (set) Token: 0x06005E68 RID: 24168 RVA: 0x0003408C File Offset: 0x0003228C
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x17001F0D RID: 7949
	// (get) Token: 0x06005E69 RID: 24169 RVA: 0x00034095 File Offset: 0x00032295
	// (set) Token: 0x06005E6A RID: 24170 RVA: 0x0003409D File Offset: 0x0003229D
	public string TweenedObjectName { get; private set; }

	// Token: 0x17001F0E RID: 7950
	// (get) Token: 0x06005E6B RID: 24171 RVA: 0x000340A6 File Offset: 0x000322A6
	public bool UseUnscaledTime
	{
		get
		{
			return this.m_useUnscaledTime;
		}
	}

	// Token: 0x17001F0F RID: 7951
	// (get) Token: 0x06005E6C RID: 24172 RVA: 0x000340AE File Offset: 0x000322AE
	// (set) Token: 0x06005E6D RID: 24173 RVA: 0x000340B6 File Offset: 0x000322B6
	public float CoroutineStartTime { get; private set; }

	// Token: 0x17001F10 RID: 7952
	// (get) Token: 0x06005E6E RID: 24174 RVA: 0x000340BF File Offset: 0x000322BF
	public float TweenStartTime
	{
		get
		{
			return this.m_tweenStartTime;
		}
	}

	// Token: 0x17001F11 RID: 7953
	// (get) Token: 0x06005E6F RID: 24175 RVA: 0x000340C7 File Offset: 0x000322C7
	public float TweenEndTime
	{
		get
		{
			return this.m_tweenEndTime;
		}
	}

	// Token: 0x17001F12 RID: 7954
	// (get) Token: 0x06005E70 RID: 24176 RVA: 0x000340CF File Offset: 0x000322CF
	public float CoroutineElapsedTime
	{
		get
		{
			if (this.UseUnscaledTime)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}
	}

	// Token: 0x17001F13 RID: 7955
	// (get) Token: 0x06005E71 RID: 24177 RVA: 0x000340E4 File Offset: 0x000322E4
	public object[] PropertyArray
	{
		get
		{
			return this.m_propertiesList;
		}
	}

	// Token: 0x17001F14 RID: 7956
	// (get) Token: 0x06005E72 RID: 24178 RVA: 0x000340EC File Offset: 0x000322EC
	public IRelayLink OnTweenCompleteRelay
	{
		get
		{
			return this.m_onTweenCompleteRelay.link;
		}
	}

	// Token: 0x17001F15 RID: 7957
	// (get) Token: 0x06005E73 RID: 24179 RVA: 0x000340F9 File Offset: 0x000322F9
	// (set) Token: 0x06005E74 RID: 24180 RVA: 0x00034101 File Offset: 0x00032301
	public Coroutine TweenCoroutine { get; private set; }

	// Token: 0x17001F16 RID: 7958
	// (get) Token: 0x06005E75 RID: 24181 RVA: 0x0003410A File Offset: 0x0003230A
	public bool Paused
	{
		get
		{
			return this.m_paused;
		}
	}

	// Token: 0x06005E76 RID: 24182 RVA: 0x00034112 File Offset: 0x00032312
	public void SetPaused(bool pause)
	{
		this.m_paused = pause;
	}

	// Token: 0x17001F17 RID: 7959
	// (get) Token: 0x06005E77 RID: 24183 RVA: 0x0003411B File Offset: 0x0003231B
	public object TweenedObj
	{
		get
		{
			return this.m_tweenedObject;
		}
	}

	// Token: 0x17001F18 RID: 7960
	// (get) Token: 0x06005E78 RID: 24184 RVA: 0x00034123 File Offset: 0x00032323
	// (set) Token: 0x06005E79 RID: 24185 RVA: 0x0003412B File Offset: 0x0003232B
	public bool IsType { get; private set; }

	// Token: 0x17001F19 RID: 7961
	// (get) Token: 0x06005E7A RID: 24186 RVA: 0x00034134 File Offset: 0x00032334
	// (set) Token: 0x06005E7B RID: 24187 RVA: 0x0003413C File Offset: 0x0003233C
	public string ID { get; set; }

	// Token: 0x06005E7C RID: 24188 RVA: 0x0016158C File Offset: 0x0015F78C
	public void SetValues(object tweenObject, float duration, bool useUnscaledTime, EaseDelegate ease, bool tweenTo, object[] properties)
	{
		if (tweenObject is Type)
		{
			this.IsType = true;
		}
		else
		{
			this.IsType = false;
		}
		this.m_tweenedObject = tweenObject;
		UnityEngine.Object @object = tweenObject as UnityEngine.Object;
		if (@object != null)
		{
			this.TweenedObjectName = @object.name;
		}
		if (CameraController.IsInstantiated && @object == CameraController.ForegroundPostProcessing)
		{
			this.m_updatesForegroundPostProcessing = true;
		}
		this.m_ease = ease;
		this.m_tweenTo = tweenTo;
		this.m_propertiesList = properties;
		this.m_useUnscaledTime = useUnscaledTime;
		if (this.m_useUnscaledTime)
		{
			this.m_tweenStartTime = Time.unscaledTime;
		}
		else
		{
			this.m_tweenStartTime = Time.time;
		}
		this.m_tweenEndTime = this.m_tweenStartTime + duration;
		this.m_initialValues = new float[this.m_propertiesList.Length / 2];
		this.m_tweenStarted = false;
		this.ID = null;
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.m_propertiesList.Length; i += 2)
		{
			if (this.m_propertiesList[i + 1] is int)
			{
				this.m_propertiesList[i + 1] = (float)((int)this.m_propertiesList[i + 1]);
			}
			if ((string)this.m_propertiesList[i] == "delay")
			{
				num = (float)this.m_propertiesList[i + 1];
			}
			if ((string)this.m_propertiesList[i] == "subtract")
			{
				num2 = (float)this.m_propertiesList[i + 1];
			}
		}
		this.m_tweenStartTime += num;
		this.m_tweenEndTime += num;
		this.m_tweenStartTime -= num2;
		this.m_tweenEndTime -= num2;
	}

	// Token: 0x06005E7D RID: 24189 RVA: 0x00161740 File Offset: 0x0015F940
	public void AddCustomEndHandler(object methodObject, string functionName, params object[] args)
	{
		if (methodObject == null)
		{
			throw new Exception("methodObject cannot be null");
		}
		Type[] array = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			Type type = args[i] as Type;
			if (type != null)
			{
				array[i] = type;
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		this.m_methodInfo = methodObject.GetType().GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null);
		if (this.m_methodInfo == null)
		{
			throw new Exception("Function " + functionName + " not found in class " + methodObject.GetType().ToString());
		}
		this.m_methodObject = methodObject;
		this.m_methodArgs = args;
	}

	// Token: 0x06005E7E RID: 24190 RVA: 0x001617EC File Offset: 0x0015F9EC
	public void AddCustomEndHandler_Type(Type objectType, string functionName, params object[] args)
	{
		Type[] array = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			Type type = args[i] as Type;
			if (type != null)
			{
				array[i] = type;
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		this.m_methodInfo = objectType.GetMethod(functionName, array);
		if (this.m_methodInfo == null)
		{
			throw new Exception("Function " + functionName + " not found in class " + objectType.ToString());
		}
		this.m_methodObject = null;
		this.m_methodArgs = args;
	}

	// Token: 0x06005E7F RID: 24191 RVA: 0x00034145 File Offset: 0x00032345
	public void StartTween()
	{
		this.CoroutineStartTime = Time.time;
		if (this.UseUnscaledTime)
		{
			this.CoroutineStartTime = Time.unscaledTime;
		}
		this.TweenCoroutine = base.StartCoroutine(this.UpdateCoroutine());
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x00034177 File Offset: 0x00032377
	public void StopTween(bool runEndHandlers)
	{
		this.m_endTween = true;
		this.m_endTweenImmediately = true;
		this.m_endTweenWithoutEndHandlers = !runEndHandlers;
	}

	// Token: 0x06005E81 RID: 24193 RVA: 0x00034191 File Offset: 0x00032391
	public void StopTweenWithConditionChecks(bool runEndHandlers, object tweenedObj, string id = null)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (tweenedObj != this.TweenedObj)
		{
			return;
		}
		if (!string.IsNullOrEmpty(id) && id != this.ID)
		{
			return;
		}
		this.StopTween(runEndHandlers);
	}

	// Token: 0x06005E82 RID: 24194 RVA: 0x000341C4 File Offset: 0x000323C4
	public void EndTween(bool runEndHandlers)
	{
		this.m_endTween = true;
		this.m_endTweenImmediately = false;
		this.m_endTweenWithoutEndHandlers = !runEndHandlers;
	}

	// Token: 0x06005E83 RID: 24195 RVA: 0x0016187C File Offset: 0x0015FA7C
	private void InvokeEndHandlers()
	{
		this.m_onTweenCompleteRelay.Dispatch();
		if (this.OnTweenComplete_UnityEvent != null)
		{
			this.OnTweenComplete_UnityEvent.Invoke();
		}
		if (this.m_methodInfo != null)
		{
			this.m_methodInfo.Invoke(this.m_methodObject, this.m_methodArgs);
		}
	}

	// Token: 0x06005E84 RID: 24196 RVA: 0x001618D0 File Offset: 0x0015FAD0
	private void InitializeTween()
	{
		int num = 0;
		for (int i = 0; i < this.m_propertiesList.Length; i += 2)
		{
			string a = (string)this.m_propertiesList[i];
			if (a != "delay" && a != "subtract")
			{
				string text = this.m_propertiesList[i] as string;
				string name = null;
				int num2 = text.IndexOf('.');
				Type type = this.m_tweenedObject.GetType();
				this.m_structObjs[num] = null;
				this.m_structPropertyInfo[num] = null;
				this.m_structFieldInfo[num] = null;
				this.m_structName[num] = null;
				bool flag = num2 > 0;
				if (flag)
				{
					name = text.Substring(num2 + 1);
					text = text.Substring(0, num2);
				}
				if (this.IsType)
				{
					this.m_propertyInfo[num] = (this.m_tweenedObject as Type).GetProperty(text);
					if (this.m_propertyInfo[num] == null)
					{
						this.m_fieldInfo[num] = (this.m_tweenedObject as Type).GetField(text);
					}
				}
				else
				{
					this.m_propertyInfo[num] = type.GetProperty(text);
					if (this.m_propertyInfo[num] == null)
					{
						this.m_fieldInfo[num] = type.GetField(text);
					}
				}
				if (flag)
				{
					this.m_structName[num] = text;
					if (this.m_propertyInfo[num] != null)
					{
						this.m_structObjs[num] = this.m_propertyInfo[num].GetValue(this.m_tweenedObject, null);
					}
					else
					{
						this.m_structObjs[num] = type.GetField(this.m_structName[num]).GetValue(this.m_tweenedObject);
					}
					Type type2 = this.m_structObjs[num].GetType();
					this.m_structPropertyInfo[num] = type2.GetProperty(name);
					if (this.m_structPropertyInfo[num] == null)
					{
						this.m_structFieldInfo[num] = type2.GetField(name);
					}
				}
				if (this.m_propertyInfo[num] == null && this.m_fieldInfo[num] == null)
				{
					throw new Exception("Property/Field " + text + " not found on object " + this.m_tweenedObject.GetType().ToString());
				}
				if (this.m_propertyInfo[num] != null)
				{
					if (this.m_structObjs[num] == null)
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_propertyInfo[num].GetValue(this.m_tweenedObject, null));
					}
					else if (this.m_structPropertyInfo[num] != null)
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_structPropertyInfo[num].GetValue(this.m_structObjs[num], null));
					}
					else
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_structFieldInfo[num].GetValue(this.m_structObjs[num]));
					}
				}
				else if (this.m_structObjs[num] == null)
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_fieldInfo[num].GetValue(this.m_tweenedObject));
				}
				else if (this.m_structPropertyInfo[num] != null)
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_structPropertyInfo[num].GetValue(this.m_structObjs[num], null));
				}
				else
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_structFieldInfo[num].GetValue(this.m_structObjs[num]));
				}
				num++;
			}
		}
	}

	// Token: 0x06005E85 RID: 24197 RVA: 0x000341DE File Offset: 0x000323DE
	public IEnumerator UpdateCoroutine()
	{
		float coroutineElapsedTime = Time.time;
		if (this.m_useUnscaledTime)
		{
			coroutineElapsedTime = Time.unscaledTime;
		}
		while (coroutineElapsedTime < this.m_tweenEndTime && !this.m_endTween)
		{
			while (this.m_paused)
			{
				if (this.m_endTween)
				{
					break;
				}
				yield return null;
				if (!this.m_useUnscaledTime)
				{
					this.m_tweenEndTime += Time.deltaTime;
					coroutineElapsedTime = Time.time;
				}
				else
				{
					this.m_tweenEndTime += Time.unscaledDeltaTime;
					coroutineElapsedTime = Time.unscaledTime;
				}
			}
			while (coroutineElapsedTime < this.m_tweenStartTime && !this.m_endTween)
			{
				yield return null;
				coroutineElapsedTime = Time.time;
				if (this.m_useUnscaledTime)
				{
					coroutineElapsedTime = Time.unscaledTime;
				}
			}
			if (!this.m_tweenStarted)
			{
				this.m_tweenStarted = true;
				this.InitializeTween();
			}
			if (this.m_tweenedObject == null || this.m_tweenedObject.ToString().Equals("null"))
			{
				base.gameObject.SetActive(false);
				yield break;
			}
			int num = 0;
			for (int i = 0; i < this.m_propertiesList.Length - 1; i += 2)
			{
				string a = (string)this.m_propertiesList[i];
				if (a != "delay" && a != "subtract")
				{
					float num2;
					if (this.m_tweenTo)
					{
						num2 = this.m_ease(coroutineElapsedTime - this.m_tweenStartTime, this.m_initialValues[num], (float)this.m_propertiesList[i + 1] - this.m_initialValues[num], this.m_tweenEndTime - this.m_tweenStartTime);
					}
					else
					{
						num2 = this.m_ease(coroutineElapsedTime - this.m_tweenStartTime, this.m_initialValues[num], (float)this.m_propertiesList[i + 1], this.m_tweenEndTime - this.m_tweenStartTime);
					}
					bool flag = this.m_propertyInfo[num] == null;
					bool flag2 = this.m_structObjs[num] != null;
					bool flag3 = this.m_structPropertyInfo[num] == null;
					if (!flag2)
					{
						if (!flag)
						{
							if (this.m_propertyInfo[num].PropertyType == typeof(int))
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, (int)num2, null);
							}
							else
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, num2, null);
							}
						}
						else if (this.m_fieldInfo[num].FieldType == typeof(int))
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, (int)num2);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, num2);
						}
					}
					else
					{
						this.UpdateStructValues(num);
						if (!flag3)
						{
							if (this.m_structPropertyInfo[num].PropertyType == typeof(int))
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], (int)num2, null);
							}
							else
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], num2, null);
							}
						}
						else if (this.m_structFieldInfo[num].FieldType == typeof(int))
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], (int)num2);
						}
						else
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], num2);
						}
						if (!flag)
						{
							this.m_propertyInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num], null);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num]);
						}
					}
					num++;
				}
			}
			coroutineElapsedTime = Time.time;
			if (this.m_useUnscaledTime)
			{
				coroutineElapsedTime = Time.unscaledTime;
			}
			if (this.m_updatesForegroundPostProcessing)
			{
				CameraController.ForegroundPostProcessing.ApplyShader();
			}
			yield return null;
		}
		if (!this.m_tweenStarted)
		{
			this.m_tweenStarted = true;
			this.InitializeTween();
		}
		this.Complete();
		yield break;
	}

	// Token: 0x06005E86 RID: 24198 RVA: 0x00161C1C File Offset: 0x0015FE1C
	private void UpdateStructValues(int index)
	{
		if (this.m_propertyInfo[index] != null)
		{
			this.m_structObjs[index] = this.m_propertyInfo[index].GetValue(this.m_tweenedObject, null);
			return;
		}
		this.m_structObjs[index] = this.m_fieldInfo[index].GetValue(this.m_tweenedObject);
	}

	// Token: 0x06005E87 RID: 24199 RVA: 0x00161C74 File Offset: 0x0015FE74
	public void Complete()
	{
		if (!this.m_endTweenImmediately && this.m_tweenedObject != null && !this.m_tweenedObject.Equals(null))
		{
			int num = 0;
			for (int i = 0; i < this.m_propertiesList.Length - 1; i += 2)
			{
				string a = (string)this.m_propertiesList[i];
				if (a != "delay" && a != "subtract")
				{
					float num2;
					if (this.m_tweenTo)
					{
						num2 = (float)this.m_propertiesList[i + 1];
					}
					else
					{
						num2 = this.m_initialValues[num] + (float)this.m_propertiesList[i + 1];
					}
					this.m_tweenStartTime = this.m_tweenEndTime;
					bool flag = this.m_propertyInfo[num] == null;
					bool flag2 = this.m_structObjs[num] != null;
					bool flag3 = this.m_structPropertyInfo[num] == null;
					if (!flag2)
					{
						if (!flag)
						{
							if (this.m_propertyInfo[num].PropertyType == typeof(int))
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, (int)num2, null);
							}
							else
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, num2, null);
							}
						}
						else if (this.m_fieldInfo[num].FieldType == typeof(int))
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, (int)num2);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, num2);
						}
					}
					else
					{
						this.UpdateStructValues(num);
						if (!flag3)
						{
							if (this.m_structPropertyInfo[num].PropertyType == typeof(int))
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], (int)num2, null);
							}
							else
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], num2, null);
							}
						}
						else if (this.m_structFieldInfo[num].FieldType == typeof(int))
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], (int)num2);
						}
						else
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], num2);
						}
						if (!flag)
						{
							this.m_propertyInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num], null);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num]);
						}
					}
					num++;
				}
			}
		}
		if (!this.m_endTweenWithoutEndHandlers)
		{
			this.InvokeEndHandlers();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005E88 RID: 24200 RVA: 0x0001BE85 File Offset: 0x0001A085
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x06005E89 RID: 24201 RVA: 0x00161F38 File Offset: 0x00160138
	public void ResetValues()
	{
		this.m_tweenedObject = null;
		Array.Clear(this.m_propertyInfo, 0, this.m_propertyInfo.Length);
		Array.Clear(this.m_fieldInfo, 0, this.m_fieldInfo.Length);
		if (this.m_propertiesList != null)
		{
			Array.Clear(this.m_propertiesList, 0, this.m_propertiesList.Length);
		}
		this.m_propertiesList = null;
		if (this.m_initialValues != null)
		{
			Array.Clear(this.m_initialValues, 0, this.m_initialValues.Length);
		}
		this.m_initialValues = null;
		this.m_tweenStartTime = 0f;
		this.m_tweenEndTime = 0f;
		this.m_tweenTo = false;
		this.m_ease = new EaseDelegate(Ease.None);
		this.m_paused = false;
		this.ID = null;
		this.m_tweenStarted = false;
		this.TweenCoroutine = null;
		this.CoroutineStartTime = 0f;
		this.m_endTween = false;
		this.m_endTweenImmediately = false;
		this.m_endTweenWithoutEndHandlers = false;
		this.m_onTweenCompleteRelay.RemoveAll(true, true);
		if (this.OnTweenComplete_UnityEvent != null)
		{
			this.OnTweenComplete_UnityEvent.RemoveAllListeners();
		}
		this.m_methodObject = null;
		this.m_methodInfo = null;
		if (this.m_methodArgs != null)
		{
			Array.Clear(this.m_methodArgs, 0, this.m_methodArgs.Length);
		}
	}

	// Token: 0x06005E8B RID: 24203 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04004D8E RID: 19854
	private const int MAX_PROPERTY_COUNT = 10;

	// Token: 0x04004D8F RID: 19855
	private const string DELAY_STRING = "delay";

	// Token: 0x04004D90 RID: 19856
	private const string SUBTRACT_STRING = "subtract";

	// Token: 0x04004D91 RID: 19857
	private PropertyInfo[] m_propertyInfo = new PropertyInfo[10];

	// Token: 0x04004D92 RID: 19858
	private FieldInfo[] m_fieldInfo = new FieldInfo[10];

	// Token: 0x04004D93 RID: 19859
	private object[] m_structObjs = new object[10];

	// Token: 0x04004D94 RID: 19860
	private PropertyInfo[] m_structPropertyInfo = new PropertyInfo[10];

	// Token: 0x04004D95 RID: 19861
	private FieldInfo[] m_structFieldInfo = new FieldInfo[10];

	// Token: 0x04004D96 RID: 19862
	private string[] m_structName = new string[10];

	// Token: 0x04004D97 RID: 19863
	private object m_tweenedObject;

	// Token: 0x04004D98 RID: 19864
	private float m_tweenStartTime;

	// Token: 0x04004D99 RID: 19865
	private float m_tweenEndTime;

	// Token: 0x04004D9A RID: 19866
	private float[] m_initialValues;

	// Token: 0x04004D9B RID: 19867
	private bool m_tweenTo;

	// Token: 0x04004D9C RID: 19868
	private EaseDelegate m_ease;

	// Token: 0x04004D9D RID: 19869
	private bool m_useUnscaledTime;

	// Token: 0x04004D9E RID: 19870
	private bool m_paused;

	// Token: 0x04004D9F RID: 19871
	private bool m_updatesForegroundPostProcessing;

	// Token: 0x04004DA0 RID: 19872
	private object[] m_propertiesList;

	// Token: 0x04004DA1 RID: 19873
	private bool m_tweenStarted;

	// Token: 0x04004DA2 RID: 19874
	private bool m_endTween;

	// Token: 0x04004DA3 RID: 19875
	private bool m_endTweenImmediately;

	// Token: 0x04004DA4 RID: 19876
	private bool m_endTweenWithoutEndHandlers;

	// Token: 0x04004DA5 RID: 19877
	private object m_methodObject;

	// Token: 0x04004DA6 RID: 19878
	private object[] m_methodArgs;

	// Token: 0x04004DA7 RID: 19879
	private MethodInfo m_methodInfo;

	// Token: 0x04004DA8 RID: 19880
	private const string m_nullString = "null";

	// Token: 0x04004DAD RID: 19885
	private Relay m_onTweenCompleteRelay = new Relay();

	// Token: 0x04004DAE RID: 19886
	public UnityEvent OnTweenComplete_UnityEvent;
}
