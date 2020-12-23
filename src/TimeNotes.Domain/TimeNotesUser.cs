using Microsoft.AspNetCore.Identity;

namespace TimeNotes.Domain
{
    public class TimeNotesUser : IdentityUser
    {
        public string AlexaUserId { get; private set; }

        public void SetAlexaUserId(string alexaUserId)
        {
            AlexaUserId = alexaUserId;
        }

        public void UnassingAlexaFromUser()
         => AlexaUserId = string.Empty;
    }
}
