using Domain.DTOs;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductService
    {
        Task<PagedResponse<ProductDTO>> GetPagedProductsAsync(int pageNumber, int pageSize);
        Task<ApiResponse<ProductDTO>> AddProductAsync(ProductDTO productDTO);
    }
}
