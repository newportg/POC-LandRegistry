Feature: TableStore
	In order to serve my clients
	As a application service
	I want to be able to store data in the TableStore

@TableStore1
Scenario: Insert into a TableStore Container
	Given A Connection to a TableStore Container <StorageAccount> <Container>
	When I Insert a TableStore object <Object>
	Then I should receive a HttpStatus <HTTPStatus>
Examples:
| StorageAccount   | Container  | Key                                                                                      | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container1 | Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw== | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 201        |

@TableStore2
Scenario: Delete into a TableStore Container
	Given A Connection to a TableStore Container <StorageAccount> <Container>
	And I Insert a TableStore object <Object>
	When I Delete a TableStore object <Object>
	Then I should receive a HttpStatus <HTTPStatus>
Examples:
| StorageAccount   | Container  | Key                                                                                      | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container2 | Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw== | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 204        |

@TableStore3
Scenario: Get object from a TableStore Container
	Given A Connection to a TableStore Container <StorageAccount> <Container>
	And I Insert a TableStore object <Object>
	When I Get a TableStore object <Object>
	Then The returned TableStore object should be the same as the original Object <Object>
Examples:
| StorageAccount   | Container  | Key                                                                                      | Object                                                                                | HTTPStatus |
| devstoreaccount1 | container3 | Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw== | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 202        |

Scenario: List containers that are availbale in the TableStore
	Given A Connection to a TableStore Container <StorageAccount> <Container>
	When I Get a List of the available TableStore containers
	Then The returned list will not contain <Container>
Examples:
| StorageAccount   | Container        | Key                                                                                      | Object                                                                                | HTTPStatus |
| devstoreaccount1 | missingcontainer | Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw== | { "Request":{"AddressId":"AddressId", "PropertyNumber": "99", "PostCode":"TQ56 4HY"}} | 200        |


