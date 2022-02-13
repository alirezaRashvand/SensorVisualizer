using SensorVisualizer.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SensorVisualizer.Core.Clients
{
    public class TestSensorVisualizerClient : ISensorVisualizerClient
    {
        public static readonly IReadOnlyList<Sensor> testSensors = new List<Sensor>
        {
            new Sensor{ Id = 1, Name = "Home", Latitude = 36.280832170590415m, Longitude = 49.99455819095462m, Temperature = 4.9m },
            new Sensor{ Id = 2, Name = "Company 1", Latitude = 24.695289871814555m, Longitude = 46.7057443346503m, Temperature = 5.2m },
            new Sensor{ Id = 3, Name = "Main Room", Latitude = 34.83528998828431m, Longitude = 48.463776953350326m, Temperature = 28.1m },
            new Sensor{ Id = 4, Name = "Kitchen", Latitude = 36.20914791729145m, Longitude = 50.14741843845722m, Temperature = 6m },
            new Sensor{ Id = 5, Name = "Company 2", Latitude = 36.17998311484446m, Longitude = 50.284518310831345m, Temperature = 6m },
            new Sensor{ Id = 6, Name = "New york Company", Latitude = 40.77987208521298m, Longitude = -74.13901587396019m, Temperature = 4m }
        };

        public async Task<Sensor> GetSensorAsync(int id)
        {
            Task<Sensor> sensorTask = Task.Factory.StartNew(() => testSensors.FirstOrDefault(s => s.Id == id));
            await Task.Delay(2500);
            Sensor sensor = await sensorTask;
            if (sensor == null)
                throw new System.ArgumentException("There is no sensor by given id");
            return sensor;
        }

        public async Task<Sensor> GetSensorAsync(int id, CancellationToken cancellationToken)
        {
            Task<Sensor> sensorTask = Task.Factory.StartNew(() => testSensors.FirstOrDefault(s => s.Id == id), cancellationToken);
            await Task.Delay(2500, cancellationToken);
            Sensor sensor = await sensorTask;
            if (sensor == null)
                throw new System.ArgumentException("There is no sensor by given id");
            return sensor;
        }

        public async Task<PagedList<Sensor>> GetSensorsAsync(int skipCount, int takeCount, string filter = null)
        {
            IQueryable<Sensor> query = testSensors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(s => s.Name.Contains(filter, System.StringComparison.OrdinalIgnoreCase));
            Task<int> countTask = Task.Factory.StartNew(() => query.Count());
            Task<List<Sensor>> sensorsTask = Task.Factory.StartNew(() => query.Skip(skipCount).Take(takeCount).ToList());
            await Task.Delay(2500);
            return new PagedList<Sensor>(await sensorsTask, await countTask);
        }

        public async Task<PagedList<Sensor>> GetSensorsAsync(int skipCount, int takeCount, string filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Sensor> query = testSensors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(s => s.Name.Contains(filter, System.StringComparison.OrdinalIgnoreCase));
            Task<int> countTask = Task.Factory.StartNew(() => query.Count(), cancellationToken);
            Task<List<Sensor>> sensorsTask = Task.Factory.StartNew(() => query.Skip(skipCount).Take(takeCount).ToList(), cancellationToken);
            await Task.Delay(2500, cancellationToken);
            return new PagedList<Sensor>(await sensorsTask, await countTask);
        }
    }
}
