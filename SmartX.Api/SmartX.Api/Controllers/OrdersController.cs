using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SmartX.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		// Keeping a simple list in memory to hold the orders for demonstration purposes.
		private static readonly List<Order> Orders = new();

		// GET: api/orders
		[HttpGet]
		public IActionResult GetAll()
		{
			// Grab all orders currently stored and send them back to the client.
			return Ok(Orders);
		}

		// GET: api/orders/{id}
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			// Look for orders using their Id
			var order = Orders.Find(o => o.Id == id);
			if (order == null)
			{
				// If not found, let the user know clearly
				return NotFound(new { message = "Order not found." });
			}

			// If the order was found, return it to the user.
			return Ok(order);
		}

		// POST: api/orders
		[HttpPost]
		public IActionResult Create(Order order)
		{
			// Automatically assign an ID based on how many orders exist
			order.Id = Orders.Count + 1;

			// Add the new order to the list
			Orders.Add(order);

			// Return the created order with its new ID
			return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
		}

		// PUT: api/orders/{id}
		[HttpPut("{id}")]
		public IActionResult Update(int id, Order updated)
		{
			// Find the order we want to update
			var order = Orders.Find(o => o.Id == id);
			if (order == null)
			{
				// Message the user if the order doesn't exist
				return NotFound(new { message = "Order not found." });
			}

			// Update the customer and status of the order
			order.Status = updated.Status;
			order.Customer = updated.Customer;

			// Return updated order to see the changes
			return Ok(order);
		}

		// DELETE: api/orders/{id}
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			// Find the order that we want to delete
			var order = Orders.Find(o => o.Id == id);
			if (order == null)
			{
				// If it doesn’t exist, let the user know
				return NotFound(new { message = "Order not found." });
			}

			// Remove the order from the list
			Orders.Remove(order);

			// Let the user know that the order was successfully deleted
			return Ok(new { message = "Order deleted successfully." });
		}
	}

	// Order model with id, customer name, and order status
	public class Order
	{
		public int Id { get; set; }
		public string Customer { get; set; } = "";
		public string Status { get; set; } = "Pending";
	}
}
