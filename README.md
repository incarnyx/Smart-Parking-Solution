# Smart Parking
Smart parking is a parking strategy that combines technology and  human innovation in an effort to use as few resources as possible—such as fuel, time and space—to achieve faster, easier and denser parking of vehicles for the majority of time they remain idle.

## Description
This is a smart parking solution based on a pre-configured Azure remote monitoring solution. Click [HERE](https://docs.microsoft.com/en-us/azure/iot-accelerators/iot-accelerators-remote-monitoring-sample-walkthrough) for the documentation and walkthrough. A UWP app has been developed to mimic the parking lot situation and a chatbot has been deployed to allow ease of access for the users to check-in to a parking slot. The backend architecture combines both cloud and edge computing to eventually build a serverless solution.

## Pre-requisites
[Azure](https://azure.microsoft.com/en-in/) - An active Azure subscription in which you are the admin
[Smooch Api](https://app.smooch.io/) - The free trial lasts for 7 days
[Remote Monitoring Solution](https://github.com/Azure/azure-iot-remote-monitoring) - The solution uses V1 of the remote monitoring solution. The documentation can be found [HERE](https://docs.microsoft.com/en-us/previous-versions/azure/iot-suite/).

## Getting Started
1. Clone the repository
2. Open Visual Studio Developer Command Line and run the following command to setup Remote monitoring solution for local development.
```python
build.cmd local
```
Follow the wizard to provision Azure services.
3. Before testing the solution follow [this](https://github.com/Azure/azure-iot-remote-monitoring/blob/master/Docs/configure-preconfigured-demo.md) to change the service plans to minimize the cost.
4. Open RemoteMonitoring.sln and build the solution to restore Nuget packages. For the Remote Monitoring solution to work, It’s important that the Web and EventProcessor.WebJob projects to run at the same time.
5. Add a few device by clicking on the add device button or add them through your IoT Hub.
6. The UWP app has two main functions:
* Send telemetry data from sensors (Free/occupied) every minute. Data is simulated for demo purposes.
* Display parking status (Free/occupied) on a monitor so that drivers can find the free spot quickly.
Deploy on your edge device that will function as a gateway. This App can be deployed to a Raspberry Pi by changing the Device Configuration to ARM and choosing a Remote device for deployment.
7. To integrate the Smooch Api and develop the bot, run the file in the ChatBot folder via the following command:
```python
node index.js
```
8. Once done, deploy the ChatBot to a WebApp. You can follow [this](https://blogs.msdn.microsoft.com/cdndevs/2015/11/06/visual-studio-code-for-mac-developers-how-to-deploy-your-site-to-microsoft-azure/) tutorial.
9. Create a free account on Front.
10. Open Smooch dashboard and connect Facebook Messenger and Front so that both these platforms can send and receive messages.
11. Deploy the final solution to Azure using the following command:
```python
build.cmd cloud release smartparkerwebapp
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change. Please read [CONTRIBUTING.md]() for details on the code of conduct, and the process for submitting pull requests.

Please make sure to update tests as appropriate.

## Disclaimer
This is not a production level solution but a proof of concept. All licenses used were strictly for development.

## License
This project is licensed under the MIT License - see the [LICENSE.md]() file for details.

