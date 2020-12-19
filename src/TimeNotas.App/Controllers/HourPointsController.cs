using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Core.Extensions;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Domain.Services;
using TimeNotes.Infrastructure.Components.Exports.Excel;

namespace TimeNotas.App.Controllers
{
    [Authorize]
    public class HourPointsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly HourPointsServices _hourPointsServices;
        private readonly IHourPointsRepository _hourPointsRepository;
        private readonly IHourPointConfigurationsRepository _hourPointConfigurationsRepository;
        private readonly ExcelExport<HourPointsModel> _excelExport;

        public HourPointsController(HourPointsServices hourPointsServices,
            IHourPointsRepository hourPointsRepository,
            IHourPointConfigurationsRepository hourPointConfigurationsRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper, ExcelExport<HourPointsModel> excelExport)
        {
            _hourPointsServices = hourPointsServices;
            _hourPointsRepository = hourPointsRepository;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
            _userManager = userManager;
            _mapper = mapper;
            _excelExport = excelExport;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<string> OpenEndPoint()
        {
            return await Task.FromResult("DEU CERTO MANO");
        }

        public async Task<IActionResult> Index(DateTime? searchDate = null)
        {
            if (!searchDate.HasValue)
                searchDate = DateTime.Now.GetDateTimeInFirstDate();

            ViewData["CurrentSearchDate"] = searchDate.Value.ToString("yyyy-MM");

            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            var config = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser.Id));

            if (config is null)
                return RedirectToAction("Create", "HourPointConfigurations");

            IEnumerable<HourPointsModel> userHourPointsModel = await GetHourPointsFromUserInSearchedDate(searchDate, identityUser);

            return View(nameof(Index), userHourPointsModel.OrderBy(h => h.Date));
        }

        public async Task<IActionResult> AutoGenerateTimeEntriesToday()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            var config = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser.Id));

            if (config.StartWorkTime <= TimeSpan.Zero || config.ToleranceTime <= TimeSpan.Zero)
                return RedirectToAction("Edit", "HourPointConfigurations", config);

            if (identityUser is null) throw new ArgumentException($"Usuário não encontrado na base de dados.");

            await _hourPointsServices.AutoGenerateHourPointForTodayWithTimeEntries(Guid.Parse(identityUser.Id));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PointNow()
            => await Create(new TimeEntryModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeEntryModel timeEntryModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = await _userManager.GetUserAsync(User);

                if (identityUser is null) throw new ArgumentException($"Usuário não encontrado na base de dados.");

                await _hourPointsServices.AddTimeEntryToHourPoints(Guid.Parse(identityUser.Id), _mapper.Map<TimeEntry>(timeEntryModel));
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
            => View(new TimeEntryModel());

        public async Task<IActionResult> Edit(Guid timeEntryId)
        {
            TimeEntry timeEntry = await _hourPointsRepository.GetTimeEntryById(timeEntryId);

            return View(_mapper.Map<TimeEntryModel>(timeEntry));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TimeEntryModel timeEntryModel)
        {
            try
            {
                IdentityUser identityUser = await _userManager.GetUserAsync(User);

                await _hourPointsServices.UpdateTimeEntryDateHourPointed(timeEntryModel.HourPointsId, timeEntryModel.Id, Guid.Parse(identityUser.Id), timeEntryModel.DateHourPointed);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> DeleteHourPoints(Guid hourPointsId)
        {
            HourPoints hourPoints = await _hourPointsRepository.GetHourPointsById(hourPointsId);

            _hourPointsRepository.RemoveHourPoints(hourPoints);
            await _hourPointsRepository.Commit();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid timeEntryId)
        {
            TimeEntry timeEntry = await _hourPointsRepository.GetTimeEntryById(timeEntryId);

            return View(_mapper.Map<TimeEntryModel>(timeEntry));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TimeEntryModel timeEntryModel)
        {
            try
            {
                IdentityUser identityUser = await _userManager.GetUserAsync(User);

                await _hourPointsServices.RemoveTimeEntryFromHourPoints(Guid.Parse(identityUser.Id), timeEntryModel.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(timeEntryModel);
            }
        }

        public async Task<IActionResult> RecalculateTimes(Guid hourPointsId)
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            await _hourPointsServices.RecalculeExtraTimeAndMissingTime(hourPointsId, Guid.Parse(identityUser.Id));

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ExportUserHourPointsToExcel(DateTime searchDate)
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            IEnumerable<HourPointsModel> userHourPointsModel = await GetHourPointsFromUserInSearchedDate(searchDate, identityUser);

            byte[] fileBytes = _excelExport.ExportExcel(userHourPointsModel);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{searchDate.ToString("MMMM")}_{searchDate.Year}.xlsx");
        }

        private async Task<IEnumerable<HourPointsModel>> GetHourPointsFromUserInSearchedDate(DateTime? searchDate, IdentityUser identityUser)
        {
            DateTime lastDaySearchedMonth = new DateTime(searchDate.Value.Year, searchDate.Value.Month, DateTime.DaysInMonth(searchDate.Value.Year, searchDate.Value.Month));

            IEnumerable<HourPoints> userHourPoints = await _hourPointsRepository.GetHourPointsWhere(x => (x.Date.Date >= searchDate.Value.Date && x.Date.Date <= lastDaySearchedMonth.Date) && x.UserId.Equals(Guid.Parse(identityUser.Id)));

            return _mapper.Map<IEnumerable<HourPointsModel>>(userHourPoints);
        }
    }
}
