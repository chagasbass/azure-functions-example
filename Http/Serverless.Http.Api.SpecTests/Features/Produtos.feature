Feature: Integração de Produtos

	Scenario: ErroValidacaoContratoProduto
	Given um contrato de produtos contendo dados para recebimento
	And e solicitado a validacao do contrato
	When o campo quantidade for invalido
	Then deve retornar notificacoes

	Scenario: SucessoValidacaoContratoProduto
	Given um contrato de produtos contendo dados para recebimento
	And e solicitado a validacao do contrato
	When os campos do contrato forem invalidos
	Then nao deve retornar notificacoes