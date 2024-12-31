using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Domain.Shared;

namespace GeneralStoreManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<ProductDTO>> GetPagedProductsAsync(int pageNumber, int pageSize)
        {
            var (products, totalItems) = await _repository.GetPagedAsync(pageNumber, pageSize);

            var productDTOs = products.Select(product => new ProductDTO
            {
                Name = product.Name,
                Status = product.Status,
                Amount = product.Amount,
                Section = product.Section,
                Price = product.Price
            }).ToList();

            return new PagedResponse<ProductDTO>(productDTOs, pageNumber, pageSize, totalItems, "Listados com sucesso");
        }

        public async Task<ApiResponse<ProductDTO>> AddProductAsync(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Status = productDTO.Status,
                Amount = productDTO.Amount,
                Section = productDTO.Section,
                Price = productDTO.Price,
                CreationDate = DateTime.UtcNow
            };

            await _repository.AddAsync(product);

            var productDTOResponse = new ProductDTO
            {
                Name = product.Name,
                Status = product.Status,
                Amount = product.Amount,
                Section = product.Section,
                Price = product.Price
            };

            return new ApiResponse<ProductDTO>(productDTOResponse, true, "Criado com sucesso");
        }
    }
}
