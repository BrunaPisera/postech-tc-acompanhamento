Feature: Acompanhamento

Acompanhamento feature 

@acompanhamento
Scenario: Update order status to Recebido
	Given the order to be saved
	And the order does not exists on the database
	Then should save the order on the database with the status Recebido