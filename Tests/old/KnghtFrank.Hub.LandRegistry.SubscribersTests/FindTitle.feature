Feature: Find Title
	In order to validate the owners of a property
	As a responcioble Estate Agent 
	I want to be able to Call the HMLR and retrieve a Property Title

Scenario: Find Address
Given I have the following request body:
         """
  		{
			"PropertyName": "Holmecroft",
			"PropertyNumber": "68",
			"Line1": "Westfield Road",
			"City": "Woking",
			"County": "Surrey",
			"PostCode": "GU22 9NG"
		}       
         """
When I post this request to the "Find" Operation
Then The result should be a 200 ("OK") response