using Emmellsoft.IoT.Rpi.SenseHat;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace unlockingiot
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ISenseHat hat;
        private string connectionString = "{your device connectionstring}";
        private DeviceClient deviceClient;

        public MainPage()
        {
            this.InitializeComponent();
            InitDevices();
        }

        public async Task InitDevices()
        {
            hat = await SenseHatFactory.GetSenseHat().ConfigureAwait(false);

            hat.Display.Fill(Colors.Aqua);
            hat.Display.Update();

            ConnectIoTHub();
        }

        public async Task ConnectIoTHub()
        {
            deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
            await deviceClient.OpenAsync();
            await deviceClient.SetMethodHandlerAsync("start", StartLED, null);
            await deviceClient.SetMethodHandlerAsync("stop", StopLED, null);
        }

        private Task<MethodResponse> StopLED(MethodRequest methodRequest, object userContext)
        {
            hat.Display.Fill(Colors.DarkRed);
            hat.Display.Update();
            return Task.FromResult(new MethodResponse(0));
        }

        private Task<MethodResponse> StartLED(MethodRequest methodRequest, object userContext)
        {
            hat.Display.Fill(Colors.DarkGreen);
            hat.Display.Update();
            return Task.FromResult(new MethodResponse(0));
        }
    }
}
