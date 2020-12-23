using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeNotes.Domain;
using TimeNotes.Domain.Services;
using TimeNotes.Infrastructure.Cache;

namespace TimeNotas.App.Controllers
{
    [AllowAnonymous]
    public class AlexaController : Controller
    {
        private readonly RedisCache _cache;
        private readonly HourPointsServices _hourPointsServices;
        private readonly UserManager<TimeNotesUser> _userManager;
        private readonly ILogger<AlexaController> _logger;

        public AlexaController(RedisCache cache, HourPointsServices hourPointsServices, UserManager<TimeNotesUser> userManager, ILogger<AlexaController> logger)
        {
            _cache = cache;
            _hourPointsServices = hourPointsServices;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ExistsUserRegisteredWithAlexaId(string alexaUserId)
        {
            if (string.IsNullOrWhiteSpace(alexaUserId))
                return BadRequest("As informações enviadas não podem ser vazia, entre em contato com a administração do time notes.");

            TimeNotesUser user = await FindUserByAlexaUserId(alexaUserId);

            return Ok(user != null);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAlexa(string alexaUserId, string passcode)
        {
            if (string.IsNullOrWhiteSpace(alexaUserId) || string.IsNullOrWhiteSpace(passcode))
                return BadRequest("As informações enviadas não podem ser vazia, verifique o código e tente novamente.");

            string userId = await _cache.GetAsync<string>(passcode);

            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Código informado inválido ou expirado, tente novamente ou solicite um novo código.");

            TimeNotesUser user = await _userManager.FindByIdAsync(userId);

            _logger.LogInformation("Usuário para cadastrar alexa encontrado.");

            user.AssignedAlexaToUser(alexaUserId);

            IdentityResult updateResult = await _userManager.UpdateAsync(user);

            await _cache.RemoveAsync(passcode);

            if (updateResult.Succeeded) return Ok("Alexa registrada com sucesso.");

            return BadRequest("Ocorreram problemas ao tentar vincular sua alexa ao time notes, fale com os administradores do site.");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPointNow(string alexaUserId)
        {
            TimeNotesUser user = await FindUserByAlexaUserId(alexaUserId);

            if (user is null)
                return BadRequest("Parece que você não registrou sua alexa no time notes. Para cadastrá-la, basta ir no site na aba de configurações e habilitar a opção Use Alexa Support e depois falar comigo novamente me passando o código que você recebeu.");

            DateTime point = DateTime.Now;

            await _hourPointsServices.AddTimeEntryToHourPoints(Guid.Parse(user.Id), new TimeEntry(point));

            return Ok($"Apontamento realizado às {point.Hour}:{point.Minute}");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPointAt(string alexaUserId, string time)
        {
            DateTime point = DateTime.Today + TimeSpan.Parse(time);

            TimeNotesUser user = await FindUserByAlexaUserId(alexaUserId);

            if (user is null)
                return BadRequest("Parece que você não registrou sua alexa no time notes. Para cadastrá-la, basta ir no site na aba de configurações e habilitar a opção Use Alexa Support e depois falar comigo novamente me passando o código que você recebeu.");

            await _hourPointsServices.AddTimeEntryToHourPoints(Guid.Parse(user.Id), new TimeEntry(point));

            return Ok($"Apontamento realizado às {point.Hour}:{point.Minute}");
        }

        private async Task<TimeNotesUser> FindUserByAlexaUserId(string alexaUserId)
        {
            if (string.IsNullOrWhiteSpace(alexaUserId)) 
                return null;

            return await _userManager.Users.Where(user => user.AlexaUserId == alexaUserId).SingleOrDefaultAsync();
        }
    }
}
