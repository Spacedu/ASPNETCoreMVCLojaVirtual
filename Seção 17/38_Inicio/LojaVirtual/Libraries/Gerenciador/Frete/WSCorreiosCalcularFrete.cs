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

        public async Task<List<ValorPrazoFrete>> CalcularFrete(String cepDestino, String tipoFrete, List<Pacote> pacotes)
        {
            List<ValorPrazoFrete> ValorDosPacotesPorFrete = new List<ValorPrazoFrete>();
            foreach (var pacote in pacotes)
            {
                ValorDosPacotesPorFrete.Add(await CalcularValorPrazoFrete(cepDestino, tipoFrete, pacote));
            }

            List<ValorPrazoFrete> ValorDosFretes = ValorDosPacotesPorFrete
                .GroupBy(a => a.TipoFrete)
                .Select(list => new ValorPrazoFrete
                {
                    TipoFrete = list.First().TipoFrete,
                    Prazo = list.Max(c => c.Prazo),
                    Valor = list.Sum(c => c.Valor)
                }).ToList();

            return ValorDosFretes;
        }

        private async Task<ValorPrazoFrete> CalcularValorPrazoFrete(String cepDestino, String tipoFrete, Pacote pacote)
        {
            var cepOrigem = _configuration.GetValue<String>("Frete:CepOrigem");
            var maoPropria = _configuration.GetValue<String>("Frete:MaoPropria");
            var avisoRecebimento = _configuration.GetValue<String>("Frete:AvisoRecebimento");
            var diametro = Math.Max(  Math.Max(pacote.Comprimento, pacote.Largura) , pacote.Altura);


            cResultado resultado = await _servico.CalcPrecoPrazoAsync("", "", tipoFrete, cepOrigem, cepDestino, pacote.Peso.ToString(), 1, pacote.Comprimento, pacote.Altura, pacote.Largura, diametro, maoPropria, 0, avisoRecebimento );

            if(resultado.Servicos[0].Erro == "0")
            {
                return new ValorPrazoFrete()
                {
                    TipoFrete = tipoFrete,
                    Prazo = int.Parse(resultado.Servicos[0].PrazoEntrega),
                    Valor = double.Parse(resultado.Servicos[0].Valor.Replace(".", "").Replace(",", "."))
                };
            }
            else
            {
                throw new Exception("Erro: " + resultado.Servicos[0].MsgErro);
            }
        }
    }
}
