using VendaLanches.Context;
using VendaLanches.Models;

namespace VendaLanches.Areas.Admin.Servicos;

public class GraficoVendasService
{
    private readonly AppDbContext context;

    public GraficoVendasService(AppDbContext context)
    {
        this.context = context;
    }

    public List<LancheGrafico> GetVendasLanches(int dias = 360)
    {
        var data = DateTime.Now.AddDays(-dias);

        var lanches = (from pd in context.PedidoDetalhes 
                        join l in context.Lanches on pd.LancheId equals l.LancheId
                        where pd.Pedido.PedidoEnviado >= data
                        group pd by new { pd.LancheId, l.Nome }
                        into g
                        select new
                        {
                            LancheNome = g.Key.Nome,
                            LanchesQuantidade = g.Sum(q => q.Quantidade),
                            LanchesValorTotal = g.Sum(a => a.Preco * a.Quantidade)
                        });

        var lista = new List<LancheGrafico>();

        foreach (var item in lanches)
        {
            var lanche = new LancheGrafico
            {
                LancheNome = item.LancheNome,
                LanchesQuantidade = item.LanchesQuantidade,
                LanchesValorTotal = item.LanchesValorTotal
            };
            lista.Add(lanche);
        }
        
        return lista;
    }
}
