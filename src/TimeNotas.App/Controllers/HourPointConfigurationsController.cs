using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;
using TimeNotes.Infrastructure.Cache;

namespace TimeNotas.App.Controllers
{
    [Authorize]
    public class HourPointConfigurationsController : Controller
    {
        private readonly UserManager<TimeNotesUser> _userManager;
        private readonly RedisCache _cache;
        private readonly IHourPointConfigurationsRepository _hourPointConfigurationsRepository;
        private readonly IMapper _mapper;

        public HourPointConfigurationsController(UserManager<TimeNotesUser> userManager,
            IHourPointConfigurationsRepository hourPointConfigurationsRepository,
            IMapper mapper, RedisCache cache)
        {
            _userManager = userManager;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            TimeNotesUser identityUser = await _userManager.GetUserAsync(User);

            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsByUserId(Guid.Parse(identityUser?.Id));

            return View(_mapper.Map<HourPointConfigurationsModel>(hourPointConfigurations));
        }


        public IActionResult Create()
        {
            return View(new HourPointConfigurationsModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HourPointConfigurationsModel hourPointConfigurationsModel)
        {
            try
            {
                TimeNotesUser identityUser = await _userManager.GetUserAsync(User);

                HourPointConfigurations hourPointConfigurations = new HourPointConfigurations(hourPointConfigurationsModel.WorkDays,
                    hourPointConfigurationsModel.BankOfHours,
                    hourPointConfigurationsModel.OfficeHour,
                    hourPointConfigurationsModel.LunchTime,
                    hourPointConfigurationsModel.StartWorkTime,
                    hourPointConfigurationsModel.ToleranceTime,
                    Guid.Parse(identityUser.Id),
                    hourPointConfigurationsModel.HourValue);

                _hourPointConfigurationsRepository.AddHourPointConfiguration(hourPointConfigurations);
                await _hourPointConfigurationsRepository.Commit();

                await EnsurePasscode(hourPointConfigurationsModel, identityUser);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsById(id);

            return View(_mapper.Map<HourPointConfigurationsModel>(hourPointConfigurations));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HourPointConfigurationsModel hourPointConfigurationsModel)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values);

            HourPointConfigurations hourPointConfigurations = await _hourPointConfigurationsRepository.GetHourPointConfigurationsById(hourPointConfigurationsModel.Id);

            hourPointConfigurations.ChangeWorkDays(hourPointConfigurationsModel.WorkDays);
            hourPointConfigurations.ChangeLunchTime(hourPointConfigurationsModel.LunchTime);
            hourPointConfigurations.ChangeOfficeHour(hourPointConfigurationsModel.OfficeHour);
            hourPointConfigurations.ChangeStartWorkTime(hourPointConfigurationsModel.StartWorkTime);
            hourPointConfigurations.ChangeToleranceTime(hourPointConfigurationsModel.ToleranceTime);
            hourPointConfigurations.ChangeBankOfHours(hourPointConfigurationsModel.BankOfHours);
            hourPointConfigurations.ChangeHourValue(hourPointConfigurationsModel.HourValue);

            if (hourPointConfigurationsModel.UseAlexaSupport && !hourPointConfigurations.UseAlexaSupport)
            {
                TimeNotesUser identityUser = await _userManager.GetUserAsync(User);
                hourPointConfigurations.ActiveAlexaSupport();
                await EnsurePasscode(hourPointConfigurationsModel, identityUser);                
            }
            else
                hourPointConfigurations.DisableAlexaSupport();

            _hourPointConfigurationsRepository.UpdateHourPointConfiguration(hourPointConfigurations);
            await _hourPointConfigurationsRepository.Commit();

            return RedirectToAction(nameof(Index));
        }

        private async Task EnsurePasscode(HourPointConfigurationsModel hourPointConfigurationsModel, TimeNotesUser identityUser)
        {
            if (hourPointConfigurationsModel.UseAlexaSupport && string.IsNullOrWhiteSpace(identityUser.AlexaUserId))
            {
                string passcode = await GetPasscode();
                await _cache.SetAsync(passcode, identityUser.Id);
                TempData["passcode"] = passcode;
            }
        }

        private async Task<string> GetPasscode()
        {
            string passcode = GeneratePasscode();

            while (await _cache.ContainsKeyAsync(passcode))
                passcode = GeneratePasscode();

            return passcode;

            static string GeneratePasscode()
            {
                return new Random().Next(100000, 999999).ToString();
            }
        }
    }
}
