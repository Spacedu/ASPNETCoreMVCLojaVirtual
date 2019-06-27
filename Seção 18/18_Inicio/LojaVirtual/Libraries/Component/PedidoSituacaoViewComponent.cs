using LojaVirtual.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Component
{
    public class PedidoSituacaoViewComponent : ViewComponent
    {
        public PedidoSituacaoViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(Pedido pedido)
        {
            return View();
        }
    }
}
