using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LojaVirtual.Models;
using LojaVirtual.Models.ProdutoAgregador;

namespace LojaVirtual.Libraries.Gerenciador.Frete
{
    public class CalcularPacote
    {
        public List<Pacote> CalcularPacotesDeProdutos(List<ProdutoItem> produtos)
        {
            List<Pacote> pacotes = new List<Pacote>();

            Pacote pacote = new Pacote();
            foreach (var item in produtos)
            {
                for(int i = 0; i < item.QuantidadeProdutoCarrinho; i++)
                {
                    var peso = pacote.Peso + item.Peso;
                    var comprimento = (pacote.Comprimento > item.Comprimento) ? pacote.Comprimento : item.Comprimento;
                    var largura = (pacote.Largura > item.Largura) ? pacote.Largura : item.Largura;
                    var altura = pacote.Altura + item.Altura;

                    //TODO - Criar novo pacote caso: 30kg, Dimensao > 200cm;
                }
            }


            //pacotes.Add();
            return pacotes;
        }
    }
}
