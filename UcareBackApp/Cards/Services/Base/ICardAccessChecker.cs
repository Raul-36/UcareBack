using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Cards.Services.Base
{
    public interface ICardAccessChecker
    {
        Task<bool> HasAccessToCard(Guid userId, Guid cardId);
    }
}