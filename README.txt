FOR STARTUP AND CONFIGURATION:

Stored Procedure:
	Step 1 - Run the Northwind Init.sql script first to create the demo db
	Step 2 - Run the Procedure Script.sql second to create the actual procedure. Ensure you are under the freshsly created Northwind db context and NOT master
	Step 3 - Run the Procedure Example Execution.sql script to test with the provided data and any new params you want. Again, make sure you are using the Northwind Db context

Api:
	Ensure visual studio is installed for easy testing via swagger
	Open up the Roulette.sln file and make sure to build the project (To make sure installed nuget packages like SQLite are also installed and updated) then run the project
	There should be 5 methods availible, one is just the default weatherforecast that can be ignored. Feel free to play around with all the methods under Roulette