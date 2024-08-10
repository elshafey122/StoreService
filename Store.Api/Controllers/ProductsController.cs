using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Core.Services.Contract;
using Store.Core.Specifications.Product_Specs;
using Store.API.Dtos;
using Talabat.APIs.Helpers;
using Store.API.Errors;

namespace Store.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;
        public ProductsController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpeceficationsParams speceficationsParams)
        {
            var products=await _productService.GetProductsAsync(speceficationsParams);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            var count= await _productService.GetCountAsync(speceficationsParams);   

            return Ok(new Pagination<ProductToReturnDto>(speceficationsParams.PageIndex,  speceficationsParams.PageSize,count, data));
        }

        // /api/Products/1
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>>GetProduct(int id)
        {

            var product =await _productService.GetProductAsync(id);

            if (product == null)
                return NotFound(new {Message="Not Found" , StatusCode=404});

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
