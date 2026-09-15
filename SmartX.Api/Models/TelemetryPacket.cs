using System;

namespace SmartX.Api.Models
{
    
    public class TelemetryPacket<T>
    {
        public string DeviceId { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        
        
        public T SensorData { get; set; }

        public TelemetryPacket(string deviceId, string locationName, T sensorData)
        {
            DeviceId = deviceId;
            LocationName = locationName;
            SensorData = sensorData;
            GeneratedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"[{GeneratedAt:HH:mm:ss}] Device: {DeviceId} ({LocationName}) -> Data: {SensorData}";
        }
    }
}
