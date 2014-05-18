using UnityEngine;
using UnityEditor;
 
public class GlobalResourcesMenuItem
{
	[MenuItem("Assets/Create/GlobalResources")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<GlobalResources> ();
	}
}