using LojaVirtual.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSCorreios;

namespace LojaVirtual.Libraries.Gerenciador.Frete
{
    public class WSCorreiosCalcularFrete
    {
        private IConfiguration _configuration;
        private CalcPrecoPrazoWSSoap _servico;

        public WSCorreiosCalcularFrete(IConfiguration configuration, CalcPrecoPrazoWSSoap servico)
        {
            _configuration = configuration;
            _servico = servico;
        }

        public async Task CalcularValorPrazoFrete(String cepDestino, String tipoFrete, Pacote pacote)
        {
            var cepOrigem = _configuration.GetValue<String>("Frete:CepOrigem");
            var maoPropria = _configuration.GetValue<String>("Frete:MaoPropria");
            var avisoRecebimento = _configuration.GetValue<String>("Frete:AvisoRecebimento");
            var diametro = Math.Max(  Math.Max(pacote.Comprimento, pacote.Largura) , pacote.Altura);


            cResultado resultado = await _servico.CalcPrecoPrazoAsync("", "", tipoFrete, cepOrigem, cepDestino, pacote.Peso.ToString(), 1, pacote.Comprimento, pacote.Altura, pacote.Largura, diametro, maoPropria, 0, avisoRecebimento );

            if(resultado.Servicos[0].Erro == "0")
            {
                //TODO - Implementar um resultado

            }
            else
            {
                throw new Exception("Erro: " + resultado.Servicos[0].MsgErro);
            }
        }
    }
}
