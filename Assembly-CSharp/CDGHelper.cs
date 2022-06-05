using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000C5F RID: 3167
public class CDGHelper
{
	// Token: 0x06005B32 RID: 23346 RVA: 0x00158D2C File Offset: 0x00156F2C
	public static T FindStaticInstance<T>(bool createNewIfNull) where T : MonoBehaviour
	{
		T[] array = UnityEngine.Object.FindObjectsOfType<T>();
		if (array.Length == 0 && createNewIfNull)
		{
			return new GameObject(typeof(T).ToString()).AddComponent<T>();
		}
		if (array.Length == 1)
		{
			return array[0].GetComponent<T>();
		}
		if (array.Length > 1)
		{
			throw new Exception(string.Format("{0}: Found multiple instances of given Type", Time.frameCount));
		}
		return default(T);
	}

	// Token: 0x06005B33 RID: 23347 RVA: 0x00158DA4 File Offset: 0x00156FA4
	public static T FindStaticInstance<T>(string instanceName, bool createNewIfNull) where T : Component
	{
		GameObject gameObject = GameObject.Find(instanceName);
		T t = default(T);
		if (gameObject != null)
		{
			t = gameObject.GetComponent<T>();
			if (t == null)
			{
				throw new MissingComponentException(instanceName);
			}
		}
		if (gameObject == null && createNewIfNull)
		{
			t = new GameObject
			{
				name = instanceName
			}.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x06005B34 RID: 23348 RVA: 0x00158E08 File Offset: 0x00157008
	public static T FindStaticInstance<T>(string instanceName, string prefabPath, bool createNewIfNull) where T : Component
	{
		GameObject gameObject = GameObject.Find(instanceName);
		T t = default(T);
		if (gameObject != null)
		{
			t = gameObject.GetComponent<T>();
			if (t == null)
			{
				throw new MissingComponentException(instanceName);
			}
		}
		if (gameObject == null && createNewIfNull)
		{
			t = UnityEngine.Object.Instantiate<T>(CDGResources.Load<T>(prefabPath, "", true));
			t.gameObject.name = instanceName;
			if (t == null)
			{
				throw new Exception("Cannot create new static instance.");
			}
		}
		return t;
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x0003205D File Offset: 0x0003025D
	public static int RandomPlusMinus()
	{
		if (UnityEngine.Random.Range(0, 2) < 1)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x00158E94 File Offset: 0x00157094
	public static Vector2 AngleToVector(float degAngle)
	{
		float num = degAngle;
		degAngle *= 0.017453292f;
		Vector2 result = new Vector2(Mathf.Cos(degAngle), Mathf.Sin(degAngle));
		if (num == 90f || num == -90f || num == 270f || num == -270f)
		{
			result.x = 0f;
		}
		if (num == 0f || num == 360f || num == 180f || num == -180f)
		{
			result.y = 0f;
		}
		return result;
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x0003206C File Offset: 0x0003026C
	public static float VectorToAngle(Vector2 pt)
	{
		return CDGHelper.AngleBetweenPts(Vector2.zero, pt);
	}

	// Token: 0x06005B38 RID: 23352 RVA: 0x00158F1C File Offset: 0x0015711C
	public static float AngleBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		float x = pt2.x - pt1.x;
		return CDGHelper.WrapAngleDegrees(Mathf.Atan2(pt2.y - pt1.y, x) * 57.29578f, false);
	}

	// Token: 0x06005B39 RID: 23353 RVA: 0x00032079 File Offset: 0x00030279
	public static Vector2 VectorBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		return new Vector2(pt2.x - pt1.x, pt2.y - pt1.y);
	}

	// Token: 0x06005B3A RID: 23354 RVA: 0x00158F58 File Offset: 0x00157158
	public static float DistanceBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		float num = pt2.x - pt1.x;
		float num2 = pt2.y - pt1.y;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x06005B3B RID: 23355 RVA: 0x0003209A File Offset: 0x0003029A
	public static float ConvertToGameUnits(float pixelFloat)
	{
		return pixelFloat / 60f;
	}

	// Token: 0x06005B3C RID: 23356 RVA: 0x000320A3 File Offset: 0x000302A3
	public static Vector2 ConvertToGameUnits(Vector2 pixelVector)
	{
		return new Vector2(pixelVector.x / 60f, pixelVector.y / 60f);
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x000320C2 File Offset: 0x000302C2
	public static float ConvertToPixelUnits(float gameFloat)
	{
		return gameFloat * 60f;
	}

	// Token: 0x06005B3E RID: 23358 RVA: 0x000320CB File Offset: 0x000302CB
	public static Vector2 ConvertToPixelUnits(Vector2 gameVector)
	{
		return new Vector2(gameVector.x * 60f, gameVector.y * 60f);
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x000320EA File Offset: 0x000302EA
	public static Vector3 ToVector3(Vector2 vector)
	{
		return new Vector3(vector.x, vector.y, 0f);
	}

	// Token: 0x06005B40 RID: 23360 RVA: 0x00032102 File Offset: 0x00030302
	public static Vector3 ToVector3(float value)
	{
		return new Vector3(value, value, value);
	}

	// Token: 0x06005B41 RID: 23361 RVA: 0x00158F8C File Offset: 0x0015718C
	public static Mesh BuildMeshFromPolygonCollider2D(PolygonCollider2D poly2D)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		int pathCount = poly2D.pathCount;
		for (int i = 0; i < pathCount; i++)
		{
			Vector2[] path = poly2D.GetPath(i);
			for (int j = 0; j < path.Length; j++)
			{
				list.Add(new Vector3(path[j].x, path[j].y, 0f));
			}
			int[] array = new Triangulator(path).Triangulate();
			for (int k = 0; k < array.Length; k++)
			{
				array[k] += i * pathCount;
			}
			list2.AddRange(array);
		}
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x06005B42 RID: 23362 RVA: 0x00159070 File Offset: 0x00157270
	public static Mesh BuildMeshFromPointList(List<Vector2> pointsList)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		Vector2[] array = pointsList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new Vector3(array[i].x, array[i].y, 0f));
		}
		int[] collection = new Triangulator(array).Triangulate();
		list2.AddRange(collection);
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x06005B43 RID: 23363 RVA: 0x0015910C File Offset: 0x0015730C
	public static Vector2 RotatedPoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
	{
		theRotation *= 0.017453292f;
		double num = (double)Mathf.Cos(theRotation);
		double num2 = (double)Mathf.Sin(theRotation);
		return new Vector2
		{
			x = (float)((double)theOrigin.x + (double)(thePoint.x - theOrigin.x) * num - (double)(thePoint.y - theOrigin.y) * num2),
			y = (float)((double)theOrigin.y + (double)(thePoint.y - theOrigin.y) * num + (double)(thePoint.x - theOrigin.x) * num2)
		};
	}

	// Token: 0x06005B44 RID: 23364 RVA: 0x0003210C File Offset: 0x0003030C
	public static Vector2 ChangeVector2Length(Vector2 thePoint, Vector2 theOrigin, float theLength)
	{
		thePoint -= theOrigin;
		thePoint = thePoint.normalized * theLength;
		return thePoint + theOrigin;
	}

	// Token: 0x06005B45 RID: 23365 RVA: 0x001591A0 File Offset: 0x001573A0
	public static float GetLockedAngle(float currentAngle, int numberOfDirections)
	{
		currentAngle = CDGHelper.WrapAngleDegrees(currentAngle, false);
		float num = 360f / (float)numberOfDirections;
		float num2 = num / 2f;
		float num3 = 0f;
		int i = 0;
		while (i <= numberOfDirections)
		{
			float num4 = num3 + num2;
			float num5 = num3 - num2;
			if (currentAngle <= num4 && currentAngle >= num5)
			{
				if (num3 >= 360f)
				{
					return 0f;
				}
				return num3;
			}
			else
			{
				num3 += num;
				i++;
			}
		}
		return currentAngle;
	}

	// Token: 0x06005B46 RID: 23366 RVA: 0x0003212D File Offset: 0x0003032D
	public static float WrapAngleDegrees(float degrees, bool convertToHalfRadius)
	{
		return CDGHelper.ToDegrees(CDGHelper.WrapAngleRadians(CDGHelper.ToRadians(degrees), convertToHalfRadius));
	}

	// Token: 0x06005B47 RID: 23367 RVA: 0x00159204 File Offset: 0x00157404
	public static float WrapAngleRadians(float radians, bool convertToHalfRadius)
	{
		float num = 6.2831855f;
		int num2 = (int)(radians / num);
		radians -= num * (float)num2;
		if (convertToHalfRadius)
		{
			if (radians > 3.1415927f)
			{
				return radians - num;
			}
			if (radians < -3.1415927f)
			{
				return radians + num;
			}
			return radians;
		}
		else
		{
			if (radians < 0f)
			{
				return radians + num;
			}
			return radians;
		}
	}

	// Token: 0x06005B48 RID: 23368 RVA: 0x00159250 File Offset: 0x00157450
	public static float TurnToFaceRadians(Vector2 sourcePosition, Vector2 targetPosition, float sourceTurnSpeedInRads, float sourceOrientationInRads, float elapsedTime, bool convertToHalfRadius)
	{
		float x = targetPosition.x - sourcePosition.x;
		float num = CDGHelper.WrapAngleRadians(Mathf.Atan2(targetPosition.y - sourcePosition.y, x), false);
		float num2 = CDGHelper.WrapAngleRadians(sourceOrientationInRads, false);
		float num3 = CDGHelper.WrapAngleRadians(sourceOrientationInRads, true);
		float num4 = num - num2;
		bool flag = num4 > 3.1415927f || num4 < -3.1415927f;
		float num5 = sourceTurnSpeedInRads * elapsedTime;
		if (flag)
		{
			float num6 = CDGHelper.WrapAngleRadians(num4, true);
			num6 = Mathf.Clamp(num6, -num5, num5);
			return CDGHelper.WrapAngleRadians(num3 + num6, convertToHalfRadius);
		}
		num4 = Mathf.Clamp(num4, -num5, num5);
		return CDGHelper.WrapAngleRadians(num2 + num4, convertToHalfRadius);
	}

	// Token: 0x06005B49 RID: 23369 RVA: 0x00032140 File Offset: 0x00030340
	public static float TurnToFaceDegrees(Vector2 sourcePosition, Vector2 targetPosition, float sourceTurnSpeedInDegrees, float sourceOrientationInDegrees, float elapsedTime, bool convertToHalfRadius)
	{
		return CDGHelper.ToDegrees(CDGHelper.TurnToFaceRadians(sourcePosition, targetPosition, CDGHelper.ToRadians(sourceTurnSpeedInDegrees), CDGHelper.ToRadians(sourceOrientationInDegrees), elapsedTime, convertToHalfRadius));
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x001592EC File Offset: 0x001574EC
	public static int GetRandomOdds(List<float> oddsArray)
	{
		int count = oddsArray.Count;
		float num = 0f;
		for (int i = 0; i < count; i++)
		{
			num += oddsArray[i];
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		int num4 = 0;
		foreach (float num5 in oddsArray)
		{
			if (num5 == 0f)
			{
				num4++;
			}
			else
			{
				num3 += num5;
				if (num2 <= num3)
				{
					return num4;
				}
				num4++;
			}
		}
		return -1;
	}

	// Token: 0x06005B4B RID: 23371 RVA: 0x0015939C File Offset: 0x0015759C
	public static int GetRandomOdds(float[] oddsArray)
	{
		int num = oddsArray.Length;
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 += oddsArray[i];
		}
		float num3 = UnityEngine.Random.Range(0f, num2);
		float num4 = 0f;
		int num5 = 0;
		foreach (float num6 in oddsArray)
		{
			if (num6 == 0f)
			{
				num5++;
			}
			else
			{
				num4 += num6;
				if (num3 <= num4)
				{
					return num5;
				}
				num5++;
			}
		}
		return -1;
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x00159420 File Offset: 0x00157620
	public static int GetRandomOdds(List<int> oddsArray)
	{
		int count = oddsArray.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num += oddsArray[i];
		}
		int num2 = UnityEngine.Random.Range(0, num + 1);
		int num3 = 0;
		int num4 = 0;
		foreach (int num5 in oddsArray)
		{
			if (num5 == 0)
			{
				num4++;
			}
			else
			{
				num3 += num5;
				if (num2 <= num3)
				{
					return num4;
				}
				num4++;
			}
		}
		return -1;
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x001594C4 File Offset: 0x001576C4
	public static int GetRandomOdds(int[] oddsArray)
	{
		int num = oddsArray.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			num2 += oddsArray[i];
		}
		int num3 = UnityEngine.Random.Range(0, num2 + 1);
		int num4 = 0;
		int num5 = 0;
		foreach (int num6 in oddsArray)
		{
			if (num6 == 0)
			{
				num5++;
			}
			else
			{
				num4 += num6;
				if (num3 <= num4)
				{
					return num5;
				}
				num5++;
			}
		}
		return -1;
	}

	// Token: 0x06005B4E RID: 23374 RVA: 0x0015953C File Offset: 0x0015773C
	public static void Shuffle<T>(List<T> array)
	{
		for (int i = array.Count; i > 1; i--)
		{
			int index = UnityEngine.Random.Range(0, i);
			T value = array[index];
			array[index] = array[i - 1];
			array[i - 1] = value;
		}
	}

	// Token: 0x06005B4F RID: 23375 RVA: 0x00159584 File Offset: 0x00157784
	public static void Shuffle<T>(T[] array)
	{
		for (int i = array.Length; i > 1; i--)
		{
			int num = UnityEngine.Random.Range(0, i);
			T t = array[num];
			array[num] = array[i - 1];
			array[i - 1] = t;
		}
	}

	// Token: 0x06005B50 RID: 23376 RVA: 0x001595CC File Offset: 0x001577CC
	public static T CopyComponent<T>(T original, GameObject destination, bool forceAdd = false) where T : Component
	{
		Collider2D collider2D = original as Collider2D;
		bool flag = false;
		if (collider2D != null && collider2D.isTrigger)
		{
			flag = true;
		}
		Type type = original.GetType();
		Component component = (!forceAdd) ? destination.GetComponent(type) : null;
		if (component == null)
		{
			component = (destination.AddComponent(type) as T);
		}
		foreach (FieldInfo fieldInfo in type.GetFields())
		{
			if (!fieldInfo.IsStatic)
			{
				fieldInfo.SetValue(component, fieldInfo.GetValue(original));
			}
		}
		foreach (PropertyInfo propertyInfo in type.GetProperties())
		{
			if (propertyInfo.CanWrite && propertyInfo.CanRead && (!flag || !(propertyInfo.Name == "density")) && (!(type == typeof(CircleCollider2D)) || !(propertyInfo.Name == "usedByComposite")) && (!(type == typeof(CapsuleCollider2D)) || !(propertyInfo.Name == "usedByComposite")) && propertyInfo.CanWrite && !(propertyInfo.Name == "name"))
			{
				propertyInfo.SetValue(component, propertyInfo.GetValue(original, null), null);
			}
		}
		return component as T;
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x00159754 File Offset: 0x00157954
	public static bool ComponentsEqual(Component component1, Component component2)
	{
		Type type = component1.GetType();
		foreach (FieldInfo fieldInfo in type.GetFields())
		{
			if (!(fieldInfo.Name == "name") && !(fieldInfo.Name == "transform") && !fieldInfo.IsStatic)
			{
				object value = fieldInfo.GetValue(component1);
				if (!(value.GetType() == typeof(UnityEngine.Object)))
				{
					object value2 = fieldInfo.GetValue(component2);
					if (value == fieldInfo.GetValue(component2))
					{
						Debug.Log(string.Concat(new string[]
						{
							"Diff on ",
							fieldInfo.Name,
							". Val 1: ",
							value.ToString(),
							". Val 2: ",
							value2.ToString()
						}));
						return false;
					}
				}
			}
		}
		foreach (PropertyInfo propertyInfo in type.GetProperties())
		{
			if (!(propertyInfo.Name == "name") && !(propertyInfo.Name == "transform"))
			{
				object value3 = propertyInfo.GetValue(component1, null);
				if (!(value3.GetType() == typeof(UnityEngine.Object)))
				{
					object value4 = propertyInfo.GetValue(component2, null);
					if (!value3.Equals(value4))
					{
						Debug.Log(string.Concat(new string[]
						{
							"Diff on ",
							propertyInfo.Name,
							". Val 1: ",
							value3.ToString(),
							". Val 2: ",
							value4.ToString()
						}));
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06005B52 RID: 23378 RVA: 0x0003215E File Offset: 0x0003035E
	public static float ToDegrees(float rads)
	{
		return rads * 57.29578f;
	}

	// Token: 0x06005B53 RID: 23379 RVA: 0x00032167 File Offset: 0x00030367
	public static float ToRadians(float degrees)
	{
		return degrees * 0.017453292f;
	}

	// Token: 0x06005B54 RID: 23380 RVA: 0x00032170 File Offset: 0x00030370
	public static Color NormalizeRGB(float r, float g, float b, float a)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x00032193 File Offset: 0x00030393
	public static Color NormalizeRGB(Vector4 color)
	{
		return CDGHelper.NormalizeRGB(color.x, color.y, color.z, color.w);
	}

	// Token: 0x06005B56 RID: 23382 RVA: 0x000321B2 File Offset: 0x000303B2
	public static Color NormalizeRGB(Color color)
	{
		return CDGHelper.NormalizeRGB(color.r, color.g, color.b, color.a);
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x00159908 File Offset: 0x00157B08
	public static BoxCollider2D ConvertPolyToBoxCollider(PolygonCollider2D polyCollider, bool destroyPolyCollider)
	{
		GameObject gameObject = polyCollider.gameObject;
		Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
		gameObject.transform.localEulerAngles = Vector3.zero;
		if (!CDGHelper.CanPolyColliderBeBoxCollider(polyCollider))
		{
			gameObject.transform.localEulerAngles = localEulerAngles;
			return null;
		}
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.size = polyCollider.bounds.size;
		boxCollider2D.offset = polyCollider.bounds.center - polyCollider.gameObject.transform.position;
		boxCollider2D.isTrigger = polyCollider.isTrigger;
		boxCollider2D.usedByComposite = polyCollider.usedByComposite;
		boxCollider2D.usedByEffector = polyCollider.usedByEffector;
		if ((!Application.isEditor || Application.isPlaying) && destroyPolyCollider)
		{
			UnityEngine.Object.DestroyImmediate(polyCollider, true);
		}
		gameObject.transform.localEulerAngles = localEulerAngles;
		return boxCollider2D;
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x001599E4 File Offset: 0x00157BE4
	public static bool CanPolyColliderBeBoxCollider(PolygonCollider2D polyCollider)
	{
		if (polyCollider.GetTotalPointCount() != 4)
		{
			return false;
		}
		Vector2 vector = new Vector2(float.MinValue, float.MinValue);
		int num = 0;
		Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
		int num2 = 0;
		foreach (Vector2 vector3 in polyCollider.points)
		{
			if (vector.x != vector3.x && vector.y != vector3.x)
			{
				if (num == 0)
				{
					vector.x = vector3.x;
				}
				else if (num == 1)
				{
					vector.y = vector3.x;
				}
				num++;
			}
			if (vector2.x != vector3.y && vector2.y != vector3.y)
			{
				if (num2 == 0)
				{
					vector2.x = vector3.y;
				}
				else if (num2 == 1)
				{
					vector2.y = vector3.y;
				}
				num2++;
			}
		}
		return num == 2 && num2 == 2;
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x00159AE8 File Offset: 0x00157CE8
	public static string ToRoman(int number)
	{
		if (number < 0 || number > 3999)
		{
			Debug.Log("Cannot return roman numberals for: " + number.ToString() + ". Value must be between 1 and 3999");
			return string.Empty;
		}
		if (number < 1)
		{
			return string.Empty;
		}
		if (number >= 1000)
		{
			return "M" + CDGHelper.ToRoman(number - 1000);
		}
		if (number >= 900)
		{
			return "CM" + CDGHelper.ToRoman(number - 900);
		}
		if (number >= 500)
		{
			return "D" + CDGHelper.ToRoman(number - 500);
		}
		if (number >= 400)
		{
			return "CD" + CDGHelper.ToRoman(number - 400);
		}
		if (number >= 100)
		{
			return "C" + CDGHelper.ToRoman(number - 100);
		}
		if (number >= 90)
		{
			return "XC" + CDGHelper.ToRoman(number - 90);
		}
		if (number >= 50)
		{
			return "L" + CDGHelper.ToRoman(number - 50);
		}
		if (number >= 40)
		{
			return "XL" + CDGHelper.ToRoman(number - 40);
		}
		if (number >= 10)
		{
			return "X" + CDGHelper.ToRoman(number - 10);
		}
		if (number >= 9)
		{
			return "IX" + CDGHelper.ToRoman(number - 9);
		}
		if (number >= 5)
		{
			return "V" + CDGHelper.ToRoman(number - 5);
		}
		if (number >= 4)
		{
			return "IV" + CDGHelper.ToRoman(number - 4);
		}
		if (number >= 1)
		{
			return "I" + CDGHelper.ToRoman(number - 1);
		}
		return string.Empty;
	}

	// Token: 0x06005B5A RID: 23386 RVA: 0x000321D1 File Offset: 0x000303D1
	public static bool IsPercent(float value)
	{
		return Mathf.Floor(value) != value;
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x000321DF File Offset: 0x000303DF
	public static int ToPercent(float value)
	{
		return Mathf.RoundToInt(value * 100f);
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x00159C88 File Offset: 0x00157E88
	public static bool DoCollisionTypesCollide_V2(CollisionType collidesWithType, GameObject other)
	{
		TagType tagType = TagType.Untagged;
		bool flag = false;
		foreach (KeyValuePair<string, TagType> keyValuePair in CDGHelper.m_tagToEnumTable)
		{
			if (other.CompareTag(keyValuePair.Key))
			{
				flag = true;
				tagType = keyValuePair.Value;
				break;
			}
		}
		if (!flag)
		{
			string tag = other.tag;
			tagType = TagType_RL.ToEnum(tag);
			CDGHelper.m_tagToEnumTable.Add(tag, tagType);
		}
		return (CollisionType_RL.GetEquivalentCollisionType(tagType) & collidesWithType) > CollisionType.None;
	}

	// Token: 0x04004BF2 RID: 19442
	private static Dictionary<string, TagType> m_tagToEnumTable = new Dictionary<string, TagType>();
}
