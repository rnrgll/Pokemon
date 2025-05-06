using System;
using System.Collections.Generic;
using UnityEngine;

namespace LDH.LDH_Scripts
{
	public class InventoryTest : MonoBehaviour
	{
		public List<InventorySlot> tempInventoryData;

		private void Start()
		{
			Manager.Data.PlayerData.Inventory.MakeTempInventory(tempInventoryData);
		}
	}
}