using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParkingLotDisplay
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ParkingRow> parkingRows; // A1, A2, A3, A4, A5, A6;
        List<int> spaceCount = new List<int> { 3, 6, 10, 5, 13, 10 };
        DispatcherTimer timer;
        static DeviceClient deviceClient;
        List<Device> devices = new List<Device>();
        Random random = new Random();
        int parkStatus = 1;
        public MainPage()
        {
            this.InitializeComponent();

            //Generate rows
            parkingRows = new ObservableCollection<ParkingRow>();
            try
            {
                parkingRows.Add(GenerateParkingRow(3, "A1"));
                parkingRows.Add(GenerateParkingRow(6, "A2"));
                parkingRows.Add(GenerateParkingRow(10, "A3"));
                parkingRows.Add(GenerateParkingRow(5, "A4"));
                parkingRows.Add(GenerateParkingRow(13, "A5"));
                parkingRows.Add(GenerateParkingRow(10, "A6"));

                lstParkingRows.ItemsSource = parkingRows;
            }
            catch (Exception ex)
            {
                //TODO: Handle error
            }

            //Initiate timer
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();

            LoadDevices();

            //Send message from random device
            int randomDevice = random.Next(0, devices.Count - 1);
            SendMessageToCloudAsync(devices[10], 1);
            UpdateDisplay(devices[randomDevice], parkStatus);

        }

        private void UpdateDisplay(Device device, int parkStatus)
        {
            foreach (var row in parkingRows)
            {
                if (row.Id == device.DeviceId.Substring(0, 2))
                {
                    foreach (var status in row.Status)
                    {
                        if (status.ParkingLotName == device.DeviceId)
                        {
                            status.ParkStatus = parkStatus;
                        }
                    }
                }

            }
        }


        private void LoadDevices()
        {
            int devicetype=0;
            //Add devices here

            if (devicetype == 0)
            {
                devices.Add(new Device { DeviceId = "A11", DeviceKey = "ybUO0SYRhAp/syiw+RRKm/1BFw8efJ/4xjEvbpioqMs=", Status = 0 });
                devices.Add(new Device { DeviceId = "A23", DeviceKey = "prB7xZ/3uwtieizJyaKLgZzq0wge67Q2jFyQ8LJJDIE=", Status = 0 });
                devices.Add(new Device { DeviceId = "A24", DeviceKey = "YtQQlCmvhWmX3k6gEmJt7bgKWi/IbG3FPuhOWQNXpS8=", Status = 0 });
                devices.Add(new Device { DeviceId = "A31", DeviceKey = "oANZxvsPR2VJ5Cfv5MDq62JzHLqF3/tUzF2gfJ/dtBs=", Status = 0 });
                devices.Add(new Device { DeviceId = "A34", DeviceKey = "a91RHMSq0FfnryAe4jAl4NILwtgN2oZOXFj4StjFyMQ=", Status = 0 });
                devices.Add(new Device { DeviceId = "A35", DeviceKey = "v2CaNMkyGrlkVonk0WXgsiPAUgLkM9czAvdWjDVE+gM=", Status = 0 });
                devices.Add(new Device { DeviceId = "A51", DeviceKey = "j9/lVkyMnri4Xa4ZAtBeIldB4SQv7gyz84kXFHg7o6A=", Status = 0 });
                devices.Add(new Device { DeviceId = "A54", DeviceKey = "XtBhYYrzZuGMlAkG4C+nC8VTSxSE6YwSGREtYmuqtQo=", Status = 0 });
                devices.Add(new Device { DeviceId = "A57", DeviceKey = "gI8isttKTfk0y/RJife3KY25Ppwrxi6/Cv0ttu4nTFc=", Status = 0 });
                devices.Add(new Device { DeviceId = "A61", DeviceKey = "RTJcucTKRC/aivN5PP8dqMOTUzBPBotk5vU4k8GLtPo=", Status = 0 });
                devices.Add(new Device { DeviceId = "A66", DeviceKey = "lZYWMTZiWC5WhHUd4mik+zxk8jSFNz1Se1Ts2c1VbHQ=", Status = 0 });
            }
            else if (devicetype == 1)
            {
                //Add iot devices to pass through edge gateway here
                //devices.Add(new Device { DeviceId = "ParkingSimulator",   = "x509=true", Status = 0 });
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            int randomDevice = random.Next(0, devices.Count - 1);
            if (parkStatus == 1)
            {
                parkStatus = 0;
            }
            else
            {
                parkStatus = 1;
            }

            SendMessageToCloudAsync(devices[randomDevice], parkStatus);
            //System.Diagnostics.Debug.WriteLine(devices);
            UpdateDisplay(devices[randomDevice], parkStatus);

        }

        private ParkingRow GenerateParkingRow(int count, string id)
        {
            ParkingRow row = new ParkingRow();
            row.Id = id;
            row.Status = new List<ParkingStatus>();

            for (int i = 1; i <= count; i++)
            {
                row.Status.Add(new ParkingStatus { ParkStatus = 0, ParkingLotName = id + i });
            }

            return row;
        }

       async void Logout(Device device)
        {
            var deviceMetaData = new
            {
                ObjectType = "DeviceInfo",
                IsSimulatedDevice = 0,
                Version = "1.0",
                DeviceProperties = new
                {
                    MessengerUser = "",
                }
            };

            deviceClient = DeviceClient.Create("LocalRM4fd43.azure-devices.net", new DeviceAuthenticationWithRegistrySymmetricKey(device.DeviceId, device.DeviceKey));
            var messageString = JsonConvert.SerializeObject(deviceMetaData);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            //await deviceClient.SendEventAsync(message);


        }

        private static async void ReceiveC2dAsync()
        {
            //Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine("Received message: {0}",
                Encoding.ASCII.GetString(receivedMessage.GetBytes());
                //Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
        private async void SendMessageToCloudAsync(Device randomDevice, int parkStatus)
        {

            foreach (var d in devices)
            {
                var parkingStatus = new
                {
                    deviceId = d.DeviceId,
                    ParkStatus = d.Status
                };


                if (d == randomDevice)
                {
                    parkingStatus = new
                    {
                        deviceId = d.DeviceId,
                        ParkStatus = parkStatus
                    };

                    //Clear messenger user
                    if(parkStatus == 0)
                    {
                        Logout(randomDevice);
                    }

                    //update device
                    d.Status = parkStatus;

                }

                deviceClient = DeviceClient.Create("LocalRM4fd43.azure-devices.net", new DeviceAuthenticationWithRegistrySymmetricKey(d.DeviceId, d.DeviceKey));
                var messageString = JsonConvert.SerializeObject(parkingStatus);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                //await deviceClient.SendEventAsync(message);
            }
        }
    }

    public class Device
    {
        public string DeviceId { get; set; }
        public string DeviceKey { get; set; }
        public int Status { get; set; }
    }
    public class ParkingRow
    {

        public string Id { get; set; }
        public List<ParkingStatus> Status { get; set; }


    }
    public class ParkingStatus : INotifyPropertyChanged
    {
        private int _p;
        private string _imageUrl;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string ParkingLotName { get; set; }
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }
        public int ParkStatus
        {

            get
            {
                return _p;
            }
            set
            {

                if (value == 0)
                {
                    ImageUrl = "Assets/blank.png";
                }
                else
                {
                    ImageUrl = "Assets/car.png";
                }

                _p = value;
            }
        }
    }
}
