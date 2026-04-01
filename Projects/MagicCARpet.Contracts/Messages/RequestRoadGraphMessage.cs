using System;
using MagicCARpet.Contracts.Models;

namespace MagicCARpet.Contracts.Messages
{
    public record RequestRoadGraphMessage(Action<Graph> ResultCallback);
}
