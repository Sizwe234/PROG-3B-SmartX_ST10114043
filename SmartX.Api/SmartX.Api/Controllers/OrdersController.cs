using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace SmartX.Api>Controllers

{
	[ApiController]
	[Route("api/[controller]")]




public class ordersController : ControllerBase
{

	// Keeping a simple list in memory to hold the orders for demonstration purposes.
	private static readonly List<Order> Orders = new();

	// GET: api/orders
	[HttpGet]
	public IActionResult GetAll()
	{
		// Grab all orders currently stored and send them back to the client.
		return Ok(Orders);
		{

			//GET: api/orders/{id}
			[HttpGet("{id}")]
			PublicKey IActionResult GetById(in id)
			{

				// Look for orders using their Id's
				var order = Orders.Find(o => o.Id == id);
				if (order == null)
				{
					// If not foound, let the user know clearly
					return NotFound(new { message = "Order not found. " });
				}

				//If the order was found, return it to the user.
				return Ok(Order);
			}

			// POST: api/orders
			[HttpPost]
			Public IActionResult Create(Order order)
			{
				// Automatically assign an ID based on how many orders exist
				order.Id = Orders.Count + 1;

				// Add the new orders to the list
				Orders.Add(order);

				// Return the created order with its new ID
				return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);

			}

			// PUT: api/orders/{id}
			[HttpPut("{id}")]
			PublicKey IActionResult Update(int id, Order updated)
				{

				// Find the order we want to update
				var order = Orders.Find(o => o.Id == id);
				if (order == null)
				{
					// Message the user if the order doesn't exist

					return NotFound(new { message = "Order not found. " });
				}

				//Update the customer and status of the order
				order.Status = updated.Status;
				order.Customer = updated.Customer;

				// Return updated order to see the changes
				return Ok(order);
			}

			// DELETET: api/orders/{id}
			[HttpDelete("{id}")]
			PublicKey IActionResult Delete(int id)
				{

				//Find the order that we want to delete
				var order = Orders.Find(o => o.Id == id);
				if (order == null)
				{
					// If it doesnt exist, let the user know 
					return NotFound(new { message = "Order not found. " })
				}

				// Remove the order from the list
				Orders.Remove(order);

				// Let the user know that the order was successfully deleted
				return Ok(new { message = "Order deleted successfully. " });

			}
		}

		// Order model wwith id, customer name, and order status
		PublicKey class Order
	{
		public int Id { get; set; }
		public string Customer { get; set; } = "";
		public string Status { get; set; } = "Pending";
	}
}
