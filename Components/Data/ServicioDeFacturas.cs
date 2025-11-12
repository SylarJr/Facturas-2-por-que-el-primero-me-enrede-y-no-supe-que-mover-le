using Microsoft.Data.Sqlite;

namespace Facturas.Components.Data
{
    public class ServicioDeFacturas
    {

        private List<Factura> facturas = new List<Factura>();

        public async Task<List<Factura>> obtenerFactura()
        {
            facturas.Clear();
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();



            return facturas;
        }
    }
}
