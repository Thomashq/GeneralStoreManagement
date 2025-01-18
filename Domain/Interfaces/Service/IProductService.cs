using Domain.DTOs;
using Domain.Shared;

namespace Domain.Interfaces.Service
{
    public interface IProductService
    {
        Task<PagedResponse<ProductDTO>> GetPagedProductsAsync(int pageNumber, int pageSize);
        Task<ApiResponse<ProductDTO>> AddProductAsync(ProductDTO productDTO);
    }
}
