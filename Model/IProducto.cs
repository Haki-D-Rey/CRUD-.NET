namespace SCELL.Model
{
    public interface IProducto
    {
        int? idProducto { get; set; }
        string descripcion { get; set; }
        int cantidad { get; set; }
        string? precio { get; set; }
        DateTime fechaCreacion { get; set; }
        DateTime? fechaModificacion { get; set; }
        bool estadoActivo { get; set; }
    }
}
