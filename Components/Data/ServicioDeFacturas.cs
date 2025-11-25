using Microsoft.Data.Sqlite;
using Facturas.Components.Data;

namespace Facturas.Components.Data
{
    public class ServicioDeFacturas
    {
        private string ruta = "mibase.db";


        public void InicializarBD()
        {
            using var conexion = new SqliteConnection($"Data Source={ruta}");
            conexion.Open();
            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                CREATE TABLE IF NOT EXISTS facturas (
                    identificador INTEGER PRIMARY KEY AUTOINCREMENT, 
                    nombre TEXT, 
                    articulos TEXT, 
                    precio INTEGER, 
                    total INTEGER, 
                    fecha TEXT
                )";
            comando.ExecuteNonQuery();
        }

      
        public async Task<List<Factura>> obtenerFactura()
        {
            var listaResultados = new List<Factura>();
            using var conexion = new SqliteConnection($"Data Source={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT identificador, nombre, articulos, precio, total, fecha FROM facturas";

            using var reader = await comando.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                listaResultados.Add(new Factura
                {
                    Identificador = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    articulo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    precio = reader.GetInt32(3),
                    total = reader.GetInt32(4),
                    Fecha = DateTime.Parse(reader.GetString(5)) 
                });
            }
            return listaResultados;
        }

        public async Task GuardarFactura(Factura nuevaFactura)
        {
            using var conexion = new SqliteConnection($"Data Source={ruta}");
            await conexion.OpenAsync();
            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                INSERT INTO facturas (nombre, articulos, precio, total, fecha) 
                VALUES (@nombre, @articulos, @precio, @total, @fecha)";

            comando.Parameters.AddWithValue("@nombre", nuevaFactura.Nombre);
            comando.Parameters.AddWithValue("@articulos", nuevaFactura.articulo ?? "");
            comando.Parameters.AddWithValue("@precio", 0);
            comando.Parameters.AddWithValue("@total", nuevaFactura.total);
            comando.Parameters.AddWithValue("@fecha", nuevaFactura.Fecha.ToString("yyyy-MM-dd")); 

            await comando.ExecuteNonQueryAsync();
        }


        public async Task EliminarFactura(int id)
        {
            using var conexion = new SqliteConnection($"Data Source={ruta}");
            await conexion.OpenAsync();
            var comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM facturas WHERE identificador = @id";
            comando.Parameters.AddWithValue("@id", id);
            await comando.ExecuteNonQueryAsync();
        }

 
        public async Task ModificarFactura(Factura factura)
        {
            using var conexion = new SqliteConnection($"Data Source={ruta}");
            await conexion.OpenAsync();
            var comando = conexion.CreateCommand();
            comando.CommandText = @"
                UPDATE facturas 
                SET nombre = @nombre, 
                    fecha = @fecha, 
                    total = @total,
                    articulos = @articulos
                WHERE identificador = @id";

            comando.Parameters.AddWithValue("@nombre", factura.Nombre);
            comando.Parameters.AddWithValue("@fecha", factura.Fecha.ToString("yyyy-MM-dd"));
            comando.Parameters.AddWithValue("@total", factura.total);
            comando.Parameters.AddWithValue("@articulos", factura.articulo ?? "");
            comando.Parameters.AddWithValue("@id", factura.Identificador);

            await comando.ExecuteNonQueryAsync();
        }
    }
}