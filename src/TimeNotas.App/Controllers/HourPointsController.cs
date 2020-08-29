using AutoMapper;
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
    public class HourPointsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly HourPointsServices _hourPointsServices;
        private readonly IHourPointsRepository _hourPointsRepository;
        private static List<TimeEntryModel> apontamentos = new List<TimeEntryModel>();

        public HourPointsController(HourPointsServices hourPointsServices,
            IHourPointsRepository hourPointsRepository,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _hourPointsServices = hourPointsServices;
            _hourPointsRepository = hourPointsRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser identityUser = await _userManager.GetUserAsync(User);

            IEnumerable<HourPoints> userHourPoints = await _hourPointsRepository.GetAllHourPointsWithTimeEntries(Guid.Parse(identityUser.Id));
            // List<HourPointsModel> userHourPointsModel = _mapper.Map<List<HourPointsModel>>(userHourPoints);

            List<HourPointsModel> userHourPointsModel = new List<HourPointsModel>();
            foreach (var item in userHourPoints)
            {
                userHourPointsModel.Add(new HourPointsModel(item.TimeEntries.Select(s => new TimeEntryModel() { DateHourPointed = s.DateHourPointed, Id = s.Id }).ToList())
                {
                    Date = item.Date,
                    ExtraTime = item.ExtraTime,
                    MissingTime = item.MissingTime,
                    Id = item.Id
                });
            }

            return View(userHourPointsModel);
        }

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
                TimeEntry timeEntry = await _hourPointsRepository.GetTimeEntryById(timeEntryModel.Id);

                timeEntry.ChangeDateHourPointed(timeEntry.DateHourPointed);

                _hourPointsRepository.UpdateTimeEntry(timeEntry);

                await _hourPointsRepository.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ApontamentosController/Delete/5
        public async Task<IActionResult> Delete(Guid timeEntryId)
        {
            TimeEntry timeEntry = await _hourPointsRepository.GetTimeEntryById(timeEntryId);

            return View(_mapper.Map<TimeEntryModel>(timeEntry));
        }

        // POST: ApontamentosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TimeEntryModel timeEntryModel)
        {
            try
            {
                _hourPointsRepository.RemoveTimeEntry(_mapper.Map<TimeEntry>(timeEntryModel));
                await _hourPointsRepository.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
