using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BouysController : MonoBehaviour
{
	private Rigidbody _rigidbody;

	private Transform[] _bouys;

	public void Awake()
	{
		_rigidbody = GetComponentInParent<Rigidbody>();

		_bouys = new Transform[transform.childCount];
		for (int i = 0; i < _bouys.Length; i++)
		{
			_bouys[i] = transform.GetChild(i);
		}
	}


	[SerializeField] private int _bouysIndex = 0;

	[ContextMenu("Add Force At Position")]
	private void AddForceAtBouy()
	{
		_rigidbody.AddForceAtPosition(_bouys[_bouysIndex].up * 100, _bouys[_bouysIndex].position);
	}

	private void AddForceAtAllBous(float speed)
	{
		for (int i = 0; i < _bouys.Length; i++)
		{
			_rigidbody.AddForceAtPosition(Vector3.up * (speed / _bouys.Length), _bouys[i].position);
		}
	}


	private void FixedUpdate()
	{
		float force = Physics.gravity.y * -1;

		if (_rigidbody.transform.position.y >= 0.1)
		{
			_rigidbody.drag = 0f;
		}
		else if (_rigidbody.transform.position.y < 0f)
		{
			AddForceAtAllBous(force * 2);
			_rigidbody.drag = 2f;
		}
		else
		{
			AddForceAtAllBous(force);
			_rigidbody.drag = 5f;
		}
	}
}
