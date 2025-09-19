namespace NvsBank.Domain.Entities.DTO;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>(); // Os registros da página atual
    public int Page { get; set; } // Número da página atual
    public int PageSize { get; set; } // Quantidade de itens por página
    public int TotalCount { get; set; } // Total de registros na base
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize); // Total de páginas
}