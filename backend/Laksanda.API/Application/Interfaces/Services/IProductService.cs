using Laksanda.API.Application.DTOs.Products;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<ProductDto?> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid productId, CancellationToken cancellationToken = default);
}
