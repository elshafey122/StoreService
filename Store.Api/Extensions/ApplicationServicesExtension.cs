using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using Store.Service;
using Store.API.Errors;
using Store.Repository;

namespace Store.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService,OrderService>();  
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));

			services.AddAutoMapper(typeof(MappingProfiles));



            services.Configure<ApiBehaviorOptions>(options =>   // used to handle failuer on model validation and get errtors using modelstate.isvalid() in controller
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });

            return services;
        }
    }
}
