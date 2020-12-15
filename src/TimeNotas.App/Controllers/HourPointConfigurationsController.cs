using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TimeNotas.App.Models;
using TimeNotes.Domain;
using TimeNotes.Domain.Data.Interfaces;

namespace TimeNotas.App.Controllers
{
    [Authorize]
    public class HourPointConfigurationsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHourPointConfigurationsRepository _hourPointConfigurationsRepository;
        private readonly IMapper _mapper;

        public HourPointConfigurationsController(UserManager<IdentityUser> userManager,
            IHourPointConfigurationsRepository hourPointConfigurationsRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _hourPointConfigurationsRepository = hourPointConfigurationsRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

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
                IdentityUser identityUser = await _userManager.GetUserAsync(User);

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

            _hourPointConfigurationsRepository.UpdateHourPointConfiguration(hourPointConfigurations);
            await _hourPointConfigurationsRepository.Commit();

            return RedirectToAction(nameof(Index));
        }
    }
}
