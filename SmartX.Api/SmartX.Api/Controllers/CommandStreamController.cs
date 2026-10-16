using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace smartX.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]

{

public class CommandStreamController : ControllerBase
	{

		// Queue for normal telemetry packets (FIFO)
		private static readonly Queue<string> TelemetryQueue = new();

		// Critical alerts get pushed into this priority queue to be considered first 
		private static readonly PriorityQueue<string> CriticalAlerts = new PriorityQueue<string>();

		// Manually override commands get stacked here so we can undo the previous one
		private static readonly Stack<string> CommandHistory = new();

		// Dictionary keeps track of registered devices for fast  lookups using ID's
		private static readonly Dictionary<string, string> Devices = new();

		// Hashset stores unique error states so we dont process things twice 
		private static readonly HashSet<string> ErrorStates = new();

		// POST: api/commandsteam/telemetry
		[HttpPost("telemetry")]
		public IActionResult AddTelemetry([FromBody] string packet)
		{
			// Drop telemetry packet into the FIFO queue
			TelemetryQueue.Enqueue(packet);
			return Ok(new { message = "Telemetry packet queued. " })
		}

		// POST: api/ commandstream/command
		[HttpPost("command")]
		public IActionResult SendCommand([FromBody] string command)

		{
			// Put command on the stack so we can undo later if needed
			CommandHistory.Push(command);
			return Ok(new { message = $"Command '{command}' send." });
		}

		// POST: api/commandstream/undo
		[HttpPost("undo")]
		public IActionResult UndoCommand()
		{
			if (CommandHistory.Count == 0)
			{
				// If stack is empty, nothing to undo
				return BadRequest(new { message = "No commands to undo." });

			}
			var last = CommandHistory.Pop();

			// Pop the last command off the stack and confirm that it was stacked
			retun Ok(new { message = $"Undid command '{last}' ." });
		}

		// POST: api/commandstream/register
		[HttpPost("register")]
		public IActionResult RegisterDevice([FromBody] string deviceId)
		{
			// Add device into dictionary with status set to online
			Devices[devicesId] = "Online";
			return Ok(new { message = $"Device '{deviceId}' registered. " });

		}

		// POST: api/commandstream/error
		[HttpPost("error")]
		public IActionResult AddError([FromBody] string error)
		{
			// Add error into hashset so duplicates dont get tracked twice
			ErrorStates.Add(error);
			return Ok(new { message = $"Error '{error}' tracked. " });

		}

		// GET: api/commandstream/status
		[HttpGet("status")]
		public IActionResult Status()
		{
			// Return snapshot counts of everything we're tracking
			return Ok(new
			{
				telemetryCount = TelemetryQueue.Count,
				criticalCount = CriticalAlerts.Count,
				commandHistoryCount = CommandHistory.Count,
				deviceCount = Devices.Count,
				errorCount = ErrorStates.Count
			});
		}






	}
}
