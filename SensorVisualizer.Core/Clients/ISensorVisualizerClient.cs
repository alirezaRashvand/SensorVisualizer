using SensorVisualizer.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SensorVisualizer.Core.Clients
{
    public interface ISensorVisualizerClient
    {
        Task<Sensor> GetSensorAsync(int id);
        Task<Sensor> GetSensorAsync(int id, CancellationToken cancellationToken);
        Task<PagedList<Sensor>> GetSensorsAsync(int skipCount, int takeCount, string filter = null);
        Task<PagedList<Sensor>> GetSensorsAsync(int skipCount, int takeCount, string filter = null, CancellationToken cancellationToken = default);
    }
}
