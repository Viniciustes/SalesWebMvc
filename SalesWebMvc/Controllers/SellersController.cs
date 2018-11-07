using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Exception;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var sellers = await _sellerService.FindAllAsync();
            return View(sellers);
        }

        public IActionResult Details(int? id)
        {
            return FindSellerByIdReturnView(id);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
                return IsNotValid(seller);

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            return FindSellerByIdReturnView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _sellerService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Edit(int? id)
        {
            return FindSellerByIdReturnView(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Name,Email,BaseSalary,BirthDate,DepartmentId")] Seller seller)
        {
            if (!ModelState.IsValid)
                return IsNotValid(seller);

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(errorViewModel);
        }

        private IActionResult IsNotValid(Seller seller)
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments, Seller = seller };

            return View(viewModel);
        }

        private IActionResult FindSellerByIdReturnView(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            var seller = _sellerService.FindById(id.Value);

            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not found" });

            var departments = _departmentService.FindAll();
            var sellerFormViewModel = new SellerFormViewModel { Departments = departments, Seller = seller };

            return View(sellerFormViewModel);
        }
    }
}