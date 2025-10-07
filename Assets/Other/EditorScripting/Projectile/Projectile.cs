using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
	[HideInInspector] new public Rigidbody rigidbody;
	public float damageRadius = 1f;


	public void Reset()
	{
		rigidbody = GetComponent<Rigidbody>();
	}
}
