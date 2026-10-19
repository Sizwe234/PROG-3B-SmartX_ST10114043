SmartX System – Part 2
This project is my Part 2 submission for PROG 3B. It builds on Part 1 by adding a Real-Time Command Stream with advanced data structures.
The API is responsible for handling telemetry, commands, critical alerts, device registration, and predictive suggestions.
The Blazor Dashboard connects to the API and provides a simple interface to test these features.
Together, the API and Dashboard demonstrate how the SmartX system evolves into a dynamic, real-time processing engine.

The features implemented include a telemetry queue that stores incoming sensor readings in order,
a priority queue that ensures urgent alerts such as power spikes bypass normal telemetry, and a stack that keeps a history of manual commands and allows undo functionality.
A dictionary is used for fast lookups of registered devices and their statuses, while a hash set tracks unique error states without duplicates. 
Finally, a predictive suggestion engine has been added to provide proactive actions, such as suggesting activation of Pump A when moisture readings drop below a threshold.

To run the application, open two terminals and start the API and Dashboard separately. For the API, navigate to the SmartX.Api folder and run the command
dotnet run --launch-profile https --project SmartX.Api.csproj. For the Dashboard, navigate to the SmartX.Dashboard folder and run
dotnet run --project SmartX.Dashboard.csproj. This will start both services and allow you to interact with the system.

Testing the API can be done using curl or Postman. For example, sending telemetry can be tested with a POST request to /api/commandstream/telemetry including a JSON body with a device ID and value.
Critical alerts can be tested with a POST request to /api/commandstream/critical, while undoing commands can be tested with a POST request to /api/commandstream/undo. 
The predictive suggestion engine can be tested with a POST request to /api/commandstream/suggestion including a device ID and value. 
For instance, sending a moisture reading below 30 will return a suggestion to activate Pump A.

In my demonstration video, I will show how to run the API and Dashboard using the command prompt, how to register devices and send telemetry, 
how to add critical alerts and undo commands, and how to use the predictive suggestion engine. 

All code is committed to GitHub with clear messages. This README explains how to run and test the application.
