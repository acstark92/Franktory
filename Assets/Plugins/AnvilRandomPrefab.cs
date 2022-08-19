using UnityEngine;
using WurstMod.Any;
using WurstMod.Shared;
using Random = System.Random;

public class AnvilRandomPrefab : MonoBehaviour
{
	public ResourceDefs.AnvilAsset[] anvilAssetsToLoad;

	private void Awake()
	{
		var ldr = gameObject.AddComponent<WurstMod.MappingComponents.Generic.AnvilPrefab>();
		ldr.prefab = anvilAssetsToLoad[new Random().Next(0, anvilAssetsToLoad.Length)];
		ldr.Spawn();
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.0f, 0.0f, 0.6f, 0.5f);
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawSphere(Vector3.zero, 0.1f);
	}
}