Feature: ApplicationEnquiry

A short summary of the feature

@API-ApplicationEnquiry
Scenario: API-ApplicationEnquiry
	Given UniqueMsgId <UniqueMsgId>
	And TitleNumber <TitleNumber>
	And ClosedAndContinued Flag <ClosedAndContinued>
	And ClosedAndContinued Flag <ContinueIfFeeExceeds>
	And ApplicationReference <ApplicationReference>
	When I call the land registy API
	Then I should recieve a LandregistryDTO response <Response>
Examples: 
| Test                        | UniqueMsgId                       | TitleNumber | ApplicationReference | ClosedAndContinued | ContinueIfFeeExceeds | Response |
| 1-MultipleReturned          | success-many-results              | DN100       |                      | True               | False                |          |
| 2-OneReturned               | success-one-result                | DN101       |                      | True               | False                |          |
| 3-AppRefOneReturned         | application-reference-one-result  |             | U565LDE              | False              | False                |          |
| 4-AppRefClosed30Days        | application-reference-thirty-days |             | G665YYG              | False              | False                |          |
| 5-NoneReturned              | success-no-results                | DN102       |                      | False              | True                 |          |
| 6-TitleIsClosed             | closed-title-number               | DN999       |                      | False              | True                 |          |
| 7-TitleIsClosedAndContinued | closed-and-continue               | BK554444    |                      | True               | False                |          |
| 8-TitleIsNotFound           | title-number-not-found            | DY100       |                      | True               | False                |          |
| 9-AccessDenied              | access-denied                     | DN100       |                      | True               | False                |          |
| 10-OutOfHours               | out-of-hours                      | DN100       |                      | True               | False                |          |

