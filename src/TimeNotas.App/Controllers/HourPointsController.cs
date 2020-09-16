using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Domain.Services;

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

        public HourPointsController(HourPointsServices hourPointsServices,
            IHourPointsRepository hourPointsRepository,
            IHourPointConfigurationsRepository hourPointConfigurationsRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _hourPointsServices = hourPointsServices;
            _hourPointsRepository = hourPointsRepository;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            var config = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser.Id));

            if (config is null)
                return RedirectToAction("Create", "HourPointConfigurations");

            IEnumerable<HourPoints> userHourPoints = await _hourPointsRepository.GetAllHourPointsWithTimeEntries(Guid.Parse(identityUser.Id));

            IEnumerable<HourPointsModel> userHourPointsModel = _mapper.Map<IEnumerable<HourPointsModel>>(userHourPoints);

            return View(userHourPointsModel.OrderBy(h => h.Date));
        }

        public async Task<IActionResult> AutoGenerateTimeEntriesToday()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

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
    }
}
