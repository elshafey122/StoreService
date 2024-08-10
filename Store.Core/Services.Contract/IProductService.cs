using Store.Core.Entities;
using Store.Core.Specifications.Product_Specs;

namespace Store.Core.Services.Contract;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpeceficationsParams speceficationsParams);

    Task<Product?> GetProductAsync(int productId);

    Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
    Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

    Task<int> GetCountAsync(ProductSpeceficationsParams speceficationsParams);

}
