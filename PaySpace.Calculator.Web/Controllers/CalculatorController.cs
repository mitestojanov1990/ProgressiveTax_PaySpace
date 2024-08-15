using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using PaySpace.Calculator.Web.Models;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Controllers;

public sealed class CalculatorController : Controller
{
    private readonly ICalculatorService _calculatorService;
    private readonly IPostalCodeService _postalCodeService;
    private readonly IHistoryService _historyService;
    private readonly ISessionManager _sessionManager;

    public CalculatorController(
        ICalculatorService calculatorService,
        IPostalCodeService postalCodeService,
        IHistoryService historyService,
        ISessionManager sessionManager)
    {
        _calculatorService = calculatorService;
        _postalCodeService = postalCodeService;
        _historyService = historyService;
        _sessionManager = sessionManager;
    }

    public async Task<IActionResult> Index()
    {
        if (!IsUserAuthenticated())
        {
            return RedirectToAction("Login", "Auth");
        }
        var vm = await this.GetCalculatorViewModelAsync();
        return this.View(vm);
    }

    public async Task<IActionResult> History()
    {
        if (!IsUserAuthenticated())
        {
            return RedirectToAction("Login", "Auth");
        }
        return this.View(new CalculatorHistoryViewModel
        {
            CalculatorHistory = await _historyService.GetHistoryAsync()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken()]
    public async Task<IActionResult> Index(CalculateRequestViewModel request)
    {
        if (!IsUserAuthenticated())
        {
            return RedirectToAction("Login", "Auth");
        }
        if (this.ModelState.IsValid)
        {
            try
            {
                await _calculatorService.CalculateTaxAsync(new CalculateRequest
                {
                    PostalCode = request.PostalCode,
                    Income = request.Income
                });

                return this.RedirectToAction(nameof(this.History));
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
            }
        }

        var vm = await this.GetCalculatorViewModelAsync(request);

        return this.View(vm);
    }

    private async Task<CalculatorViewModel> GetCalculatorViewModelAsync(CalculateRequestViewModel? request = null)
    {
        var postalCodes = await _postalCodeService.GetPostalCodesAsync();
        var selectList = new SelectList(postalCodes, "Code", "Code");
        var viewModel = new CalculatorViewModel
        {
            PostalCodes = selectList,
            Income = 0,
            PostalCode = string.Empty
        };

        if (request == null)
        {
            return viewModel;
        }

        viewModel.Income = request.Income;
        viewModel.PostalCode = request.PostalCode ?? string.Empty;
        return viewModel;
    }
    private bool IsUserAuthenticated()
    {
        return _sessionManager.GetToken() != null;
    }
}