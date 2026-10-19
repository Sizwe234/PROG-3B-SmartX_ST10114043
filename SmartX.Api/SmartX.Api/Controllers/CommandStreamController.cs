using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SmartX.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommandStreamController : ControllerBase
	{

		private static readonly Queue<TelemetryRequest> TelemetryQueue = new();


		private static readonly PriorityQueue<CriticalRequest, int> CriticalAlerts = new();


		private static readonly Stack<CommandRequest> CommandHistory = new();


		private static readonly Dictionary<string, string> Devices = new();


		private static readonly HashSet<ErrorRequest> ErrorStates = new();



		public class TelemetryRequest
		{
			public string DeviceId { get; set; } = "";
			public double Value { get; set; }
		}


		public class CriticalRequest
		{
			public string DeviceId { get; set; } = "";
			public string Alert { get; set; } = "";
		}


		public class CommandRequest
		{
			public string DeviceId { get; set; } = "";
			public string Command { get; set; } = "";
		}


		public class RegisterRequest
		{
			public string DeviceId { get; set; } = "";
		}


		public class ErrorRequest
		{
			public string DeviceId { get; set; } = "";
			public string Error { get; set; } = "";
		}


		public class DeviceUpdateRequest
		{
			public string DeviceId { get; set; } = "";
			public string Status { get; set; } = "";
		}



		[HttpPost("telemetry")]
		public IActionResult AddTelemetry([FromBody] TelemetryRequest request)
		{
			TelemetryQueue.Enqueue(request);
			return Ok(new { message = $"Telemetry from {request.DeviceId} queued with value {request.Value}." });
		}


		[HttpPost("critical")]
		public IActionResult AddCriticalAlert([FromBody] CriticalRequest request)
		{
			CriticalAlerts.Enqueue(request, 0);
			return Ok(new { message = $"Critical alert '{request.Alert}' queued for {request.DeviceId}." });
		}


		[HttpPost("command")]
		public IActionResult SendCommand([FromBody] CommandRequest request)
		{
			CommandHistory.Push(request);
			return Ok(new { message = $"Command '{request.Command}' sent to {request.DeviceId}." });
		}


		[HttpPost("undo")]
		public IActionResult UndoCommand()
		{
			if (CommandHistory.Count == 0)
				return BadRequest(new { message = "No commands to undo." });

			var last = CommandHistory.Pop();
			return Ok(new { message = $"Undid command '{last.Command}' for {last.DeviceId}." });
		}


		[HttpPost("register")]
		public IActionResult RegisterDevice([FromBody] RegisterRequest request)
		{
			Devices[request.DeviceId] = "Online";
			return Ok(new { message = $"Device '{request.DeviceId}' registered." });
		}


		[HttpPost("removeDevice")]
		public IActionResult RemoveDevice([FromBody] RegisterRequest request)
		{
			if (Devices.ContainsKey(request.DeviceId))
			{
				Devices.Remove(request.DeviceId);
				return Ok(new { message = $"Device '{request.DeviceId}' removed." });
			}
			else
			{
				return NotFound(new { message = $"Device '{request.DeviceId}' not found." });
			}
		}


		[HttpPut("updateDevice")]
		public IActionResult UpdateDevice([FromBody] DeviceUpdateRequest request)
		{
			if (Devices.ContainsKey(request.DeviceId))
			{
				Devices[request.DeviceId] = request.Status;
				return Ok(new { message = $"Device '{request.DeviceId}' updated to status '{request.Status}'." });
			}
			else
			{
				return NotFound(new { message = $"Device '{request.DeviceId}' not found." });
			}
		}


		[HttpPost("error")]
		public IActionResult AddError([FromBody] ErrorRequest request)
		{
			ErrorStates.Add(request);
			return Ok(new { message = $"Error '{request.Error}' tracked for {request.DeviceId}." });
		}


		[HttpGet("status")]
		public IActionResult Status()
		{
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
