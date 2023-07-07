using System;
using Confluent.Kafka;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace AivenTest
{
    interface IKafkaSettings
    {
        public string CERTIFICATEFILE { get; set; }
        public string PRIVATEKEY { get; }
        public string CACERT { get; set; }
        public string ENDPOINT { get; set; }
        public bool HasCertFiles();

    }
    public class KafkaServiceSettings : IKafkaSettings
    {
        public string CERTIFICATEFILE { get; set; }
        public string PRIVATEKEY { get; }
        public string CACERT { get; set; }
        public string ENDPOINT { get; set; }

        public KafkaServiceSettings()
        {
            CERTIFICATEFILE = "c:\\projects\\kafka\\service.cert";
            PRIVATEKEY = "c:\\projects\\kafka\\service.key";
            CACERT = "c:\\projects\\kafka\\ca.pem";
            ENDPOINT = "kafka-3c59a9a9-isaacjg-d86e.aivencloud.com:15046";
        }
        public bool HasCertFiles()
        {
            if (!File.Exists(CERTIFICATEFILE))
            {
                Console.WriteLine($"{CERTIFICATEFILE} not found");
                return false;
            }
            if (!File.Exists(PRIVATEKEY))
            {
                Console.WriteLine($"{PRIVATEKEY} not found");
                return false;

            }
            if (!File.Exists(CACERT))
            {
                Console.WriteLine($"{CACERT} not found");
                return false;
            }
            return true;
        }
    }
    public class WeatherSensor
    {
        public Guid id;
        public string Name;
        public int TempF = 0;
        public int Humidity = 0;
        public DateTime LastUpdate = DateTime.UtcNow;
        private readonly Random Seed = new();

        public WeatherSensor(string Name)
        {
            this.Name = Name;
            id = Guid.NewGuid();
            TempF = Seed.Next(25, 90);
            Humidity = Seed.Next(50, 85);
        }

        public string ToJson(bool pretty = false)
        {
            Formatting format = (pretty) ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(this, format);
        }
        public void ReadSensor()
        {
            TempF += Seed.Next(-3, 3);
            Humidity += Seed.Next(-3, 3);
            LastUpdate = DateTime.UtcNow;
            Console.WriteLine($"{Name} Updating Settings" + Environment.NewLine + this.ToJson(pretty:true));
        }

    }
    public class KafkaProducer
    {
        private ProducerConfig Config;
        private IProducer<Null, string> Producer;

        public KafkaProducer(KafkaServiceSettings settings)
        {
            if (!settings.HasCertFiles())
            {
                throw (new ArgumentException());
            };

            Config = new ProducerConfig
            {
                BootstrapServers = settings.ENDPOINT,
                EnableSslCertificateVerification = true,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCertificateLocation = settings.CERTIFICATEFILE,
                SslKeyLocation = settings.PRIVATEKEY,
                SslCaLocation = settings.CACERT,
                // Debug = "all"
            };

            Producer = new ProducerBuilder<Null, string>(Config).Build();
        }

        public void SendMessage(string Topic, string Value)
        {
            try
            {
                Producer.ProduceAsync(Topic, new Message<Null, string>
                {
                    Value = Value
                })
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var producer = new KafkaProducer(new KafkaServiceSettings());

            var t1 = Task.Factory.StartNew(() =>
            {
                var sensor = new WeatherSensor("Weather-Station-A");

                int i = 0;
                var random = new Random();
                while (true)
                {
                    sensor.ReadSensor();
                    producer.SendMessage("sensor", sensor.ToJson());
                    if (i++ >= 5) break;
                    Thread.Sleep(random.Next(1000, 5000));                    
                }
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                var sensor = new WeatherSensor("Weather-Station-B");
                int i = 0;
                var random = new Random();
                while (true)
                {
                    sensor.ReadSensor();
                    producer.SendMessage("sensor", sensor.ToJson());
                    if (i++ >= 5) break;
                    Thread.Sleep(random.Next(1000, 5000));
                }
            });

            t1.Wait();
            t2.Wait();
        }
    }
}

