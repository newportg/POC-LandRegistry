Feature: BlobStore
	In order to serve my clients
	As a application service
	I want to be able to store data in the BlobStore

@BlobStore1
Scenario: Insert into a BlobStore Container
	Given A Connection to a BlobStore Container <StorageAccount> <Container>
	When I Insert a object <Object>
	Then I should receive a HttpStatus <HTTPStatus>
Examples:
| StorageAccount   | Container | Key | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container1 |     | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 201        |

@BlobStore2
Scenario: Delete into a BlobStore Container
	Given A Connection to a BlobStore Container <StorageAccount> <Container>
	And I Insert a object <Object>
	When I Delete a object <Object>
	Then I should receive a HttpStatus <HTTPStatus>
Examples:
| StorageAccount   | Container | Key | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container2 |     | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 202        |

@BlobStore3
Scenario: Get object from a BlobStore Container
	Given A Connection to a BlobStore Container <StorageAccount> <Container>
	And I Insert a object <Object>
	When I Get a object <Object>
	Then The returned blob should be the same as the original Object <Object>
Examples:
| StorageAccount   | Container  | Key | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container3 |     | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 202        |

Scenario: List containers that are availbale in the BlobStore
	Given A Connection to a BlobStore Container <StorageAccount> <Container>
	When I Get a List of the available containers
	Then The returned list will not contain <Container>
Examples:
| StorageAccount   | Container        | Key | Object                                                                                | HTTPStatus |
| devstoreaccount1 | missingcontainer |     | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 200        |


