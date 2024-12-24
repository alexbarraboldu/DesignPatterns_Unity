using System.Collections.Generic;

using UnityEngine;

namespace Patterns.Adapter
{
	public class InventorySystemAdapter : InventorySystem, IInventorySystem
	{
		private List<InventoryItem> _cloudInventory;

		public void SyncInventories()
		{
			var _cloudInventory = GetInventory();

			Debug.Log("Synchronizing local drive and cloud inventories");
		}

		public void AddItem(InventoryItem item, SaveLocation location)
		{
			switch (location)
			{
				case SaveLocation.Disk:
					Debug.Log("Adding item to local drive");
					break;
				case SaveLocation.Cloud:
					AddItem(item);
					break;
				case SaveLocation.Both:
					Debug.Log("Adding item to local drive and the cloud");
					break;
			}
		}

		public void RemoveItem(InventoryItem item, SaveLocation location)
		{
			Debug.Log("Remove item from local/cloud/both");
		}

		public List<InventoryItem> GetInventory(SaveLocation location)
		{
			Debug.Log("Get inventory from local/cloud/history");

			return new List<InventoryItem>();
		}
	}
}
