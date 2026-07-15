using Laksanda.API.Application.DTOs.Products;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.Select(MapToDto).ToArray();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        return product is null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var errors = ProductValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedCode = request.ProductCode.Trim();
        var existingByCode = await _productRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null)
        {
            throw new ArgumentException("ProductCode already exists.");
        }

        var normalizedBarcode = request.Barcode?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedBarcode))
        {
            var existingByBarcode = await _productRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);
            if (existingByBarcode is not null)
            {
                throw new ArgumentException("Barcode already exists.");
            }
        }

        var product = new Product
        {
            ProductCode = normalizedCode,
            ProductName = request.ProductName.Trim(),
            Category = request.Category?.Trim(),
            Unit = request.Unit?.Trim(),
            SellingPrice = request.SellingPrice,
            Barcode = normalizedBarcode,
            Status = request.Status
        };

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(Guid productId, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var errors = ProductValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        var normalizedCode = request.ProductCode.Trim();
        var existingByCode = await _productRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null && existingByCode.ProductId != productId)
        {
            throw new ArgumentException("ProductCode already exists.");
        }

        var normalizedBarcode = request.Barcode?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedBarcode))
        {
            var existingByBarcode = await _productRepository.GetByBarcodeAsync(normalizedBarcode, cancellationToken);
            if (existingByBarcode is not null && existingByBarcode.ProductId != productId)
            {
                throw new ArgumentException("Barcode already exists.");
            }
        }

        product.ProductCode = normalizedCode;
        product.ProductName = request.ProductName.Trim();
        product.Category = request.Category?.Trim();
        product.Unit = request.Unit?.Trim();
        product.SellingPrice = request.SellingPrice;
        product.Barcode = normalizedBarcode;
        product.Status = request.Status;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(product);
    }

    public async Task<bool> DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return false;
        }

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductCode = product.ProductCode,
            ProductName = product.ProductName,
            Category = product.Category,
            Unit = product.Unit,
            SellingPrice = product.SellingPrice,
            Barcode = product.Barcode,
            Status = product.Status
        };
    }
}
