﻿using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;

namespace AdminDashboard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var products =await unitOfWork.Repository<Product>().GetAllAsync();
            var mappedProducts = mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductViewModel>>(products);
            return View(mappedProducts);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(ProductViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Image != null)
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                else
                    model.PictureUrl = "products/hat-react2.png";

                var mappedProduct = mapper.Map<ProductViewModel,Product>(model);
                await unitOfWork.Repository<Product>().AddAsync(mappedProduct);
                await unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mappedProduct = mapper.Map<Product,ProductViewModel>(product);
            return View(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();
            if(ModelState.IsValid)
            {
                if(model.Image != null)
                {
                    if(model.PictureUrl != null) // old image found
                    {
                        PictureSettings.DeleteFile(model.PictureUrl); //   foldername/filename
						model.PictureUrl = PictureSettings.UploadFile(model.Image, "products"); 
                    }
                    else
                    {
                        model.PictureUrl = PictureSettings.UploadFile(model.Image, "products");
                    }
                }

                var mappedProduct = mapper.Map<ProductViewModel, Product>(model);
                unitOfWork.Repository<Product>().Update(mappedProduct);
                var result = await unitOfWork.CompleteAsync();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult>Delete(int id)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mappedProduct =mapper.Map<Product,ProductViewModel>(product);
            return View(mappedProduct);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id,ProductViewModel model)
        {
            if(id !=model.Id)
                return NotFound();
            try
            {
                if (model.PictureUrl != null)
                    PictureSettings.DeleteFile(model.PictureUrl);

				var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
				unitOfWork.Repository<Product>().Delete(product);
                await unitOfWork.CompleteAsync();
                return RedirectToAction("Index");

            }
            catch (System.Exception)
            {

                return View(model);
            }
        }
    }
}
