using UnityEngine;

namespace Patterns.Adapter.Example
{
	public class ClientAdapter : MonoBehaviour
	{
		public InventoryItem item;
		private InventorySystem _inventorySystem;
		private IInventorySystem _inventorySystemAdapter;

		private void Start()
		{
			_inventorySystem = new InventorySystem();
			_inventorySystemAdapter = new InventorySystemAdapter();
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Add item (no adapter)"))
				_inventorySystem.AddItem(item);

			if (GUILayout.Button("Add item (with adapter)"))
				_inventorySystemAdapter.AddItem(item, SaveLocation.Both);
		}
	}
}
