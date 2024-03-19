using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MagicCARpet.Contracts.Messages
{
    public record CheckRoadNodeTriggeredMessage(Vector3 Position, Action<bool, string> ResultFunction);
}
