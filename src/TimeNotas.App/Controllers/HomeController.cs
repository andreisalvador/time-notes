using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Core.Extensions;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;

namespace TimeNotas.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const byte CHART_MONTHS_RANGE = 6;
        private readonly IHourPointsRepository _hourPointsRepository;
        private readonly IHourPointConfigurationsRepository _hourPointConfigurationsRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHourPointsRepository hourPointsRepository, UserManager<IdentityUser> userManager, IHourPointConfigurationsRepository hourPointConfigurationsRepository)
        {
            _logger = logger;
            _hourPointsRepository = hourPointsRepository;
            _userManager = userManager;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            if (identityUser is null) throw new ArgumentException($"Usuário não encontrado na base de dados.");

            DateTime currentDate = DateTime.Now.GetDateTimeInFirstDate();
            DateTime lastDaySearchedMonth = currentDate.GetDateTimeInLastDate();

            IEnumerable<HourPoints> userHourPoints = await _hourPointsRepository
                .GetHourPointsWhere(x => (x.Date.Date >= currentDate.Date && x.Date.Date <= lastDaySearchedMonth.Date) && x.UserId.Equals(Guid.Parse(identityUser.Id)));

            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser.Id));

            HomeDashboardViewModel homeDashboardViewModel = new HomeDashboardViewModel(new MonthExtract(currentDate, userHourPoints, hourPointConfigurations));

            return View(homeDashboardViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetChartSixMonthsData()
        {
            DateTime currentDate = DateTime.Now.GetDateTimeInFirstDate();
            DateTime endDate = currentDate.GetDateTimeInLastDate();
            DateTime startDate = endDate.AddMonths(-5).GetDateTimeInFirstDate();

            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            if (identityUser is null) throw new ArgumentException($"Usuário não encontrado na base de dados.");

            IEnumerable<HourPoints> userHourPoints = await _hourPointsRepository
                .GetHourPointsWhere(x => (x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date) && x.UserId.Equals(Guid.Parse(identityUser.Id)));

            var userHourPointsByMonth = userHourPoints.ToLookup(x => x.Date.Date.Month);

            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser.Id));

            IList<HomeDashboardViewModel> userHourPointsFromSixMonths = new List<HomeDashboardViewModel>();            

            while (userHourPointsFromSixMonths.Count < CHART_MONTHS_RANGE)
            {
                IEnumerable<HourPoints> hoursPointsOfMonth = userHourPointsByMonth[startDate.Month];
                
                userHourPointsFromSixMonths.Add(new HomeDashboardViewModel(new MonthExtract(startDate, hoursPointsOfMonth, hourPointConfigurations)));

                startDate = startDate.AddMonths(1);
            }

            return Json(userHourPointsFromSixMonths);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
