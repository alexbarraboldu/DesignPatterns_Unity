using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Patterns.BehaviourTree
{
	[Serializable]
	public class InterfaceReference<TInterface, TObject> where TInterface : class where TObject : Object
	{
		[SerializeField] TObject underlyingValue;

		public TInterface Value
		{
			get //=> underlyingValue switch
			{
				if (underlyingValue is Component)
				{
					return (underlyingValue as Component).GetComponent<TInterface>();
				}

				return null;
				//null => null,
				//TInterface @interface => @interface,
				//_ => throw new InvalidOperationException($"{underlyingValue} needs to implement interface {typeof(TInterface).Name}.")
			}
			set => underlyingValue = value switch
			{
				null => null,
				TObject newValue => newValue,
				_ => throw new ArgumentException($"{value} needs to be of tyype {typeof(TObject)}.", string.Empty)
			};
		}

		public TObject UnderlyingValue
		{
			get => underlyingValue;
			set => underlyingValue = value;
		}

		public InterfaceReference() { }

		public InterfaceReference(TObject target) => underlyingValue = target;

		public InterfaceReference(TInterface @interface) => underlyingValue = @interface as TObject;
	}

	[Serializable]
	public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Component> where TInterface : class { }
}
