using VendaLanches.Models;

namespace VendaLanches.Repositories.Interfaces;

public interface IPedidoRepository
{
    void CriarPedido(Pedido pedido);
}
