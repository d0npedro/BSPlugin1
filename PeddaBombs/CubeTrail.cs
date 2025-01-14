using UnityEngine;

public class CubeTrail : MonoBehaviour
{
	private int _TransformShaderProperty = Shader.PropertyToID("_Transform");

	public Renderer renderer;

	public Material material;

	private float speed = 10f;

	private Vector3 lastPosition;

	private Quaternion lastRotation;

	private Material instancedMaterial;

	private void Awake()
	{
		instancedMaterial = Object.Instantiate(material);
		renderer.sharedMaterial = instancedMaterial;
	}

	private void OnEnable()
	{
		lastPosition = base.transform.position;
		lastRotation = base.transform.rotation;
		instancedMaterial.SetMatrix(_TransformShaderProperty, Matrix4x4.TRS(lastPosition, lastRotation, Vector3.one));
	}

	public void LateUpdate()
	{
		lastPosition = Vector3.Lerp(lastPosition, base.transform.position, Time.deltaTime * speed);
		lastRotation = Quaternion.Lerp(lastRotation, base.transform.rotation, Time.deltaTime * speed);
		instancedMaterial.SetMatrix(_TransformShaderProperty, Matrix4x4.TRS(lastPosition, lastRotation, Vector3.one));
	}

	private void OnDestroy()
	{
		Object.Destroy(instancedMaterial);
	}
}
