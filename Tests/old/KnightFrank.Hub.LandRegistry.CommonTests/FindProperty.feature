Feature: FindProperty
	In order to validate the owners of a property
	As a responcioble Estate Agent 
	I want to be able to Call the HMLR and retrieve a Property Title

@mytag

Scenario: Find a Property
	Given For a given property detail <Payload>
	When I call the land registy to <Request>
	Then I should receive <Response>
Examples:
| Request | Payload                                                                                                                                   | Response                                                                                                                                   |
| Find    | {"PropertyName": "Holmecroft","PropertyNumber": "68","Line1": "Westfield Road","City": "Woking","County": "Surrey","PostCode": "GU22 9NG"} | {"Status": "OK","PropertyName": "Holmecroft","PropertyNumber": "68","Line1": "Westfield Road","City": "Woking","County": "Surrey","PostCode": "GU22 9NG"} |
