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

        public void CalcularValorPrazoFrete(String cepDestino, String tipoFrete, Pacote pacote)
        {
            var cepOrigem = _configuration.GetValue<String>("Frete:CepOrigem");
            var maoPropria = _configuration.GetValue<String>("Frete:MaoPropria"); ;
            var avisoRecebimento = _configuration.GetValue<String>("Frete:AvisoRecebimento"); ;

            _servico.CalcPrecoPrazoAsync("", "", tipoFrete);
        }
    }
}
