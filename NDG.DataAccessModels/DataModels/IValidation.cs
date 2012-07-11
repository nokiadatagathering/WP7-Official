using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDG.DataAccessModels.DataModels
{
    interface IValidation
    {
        bool Validate();
        string InvalidMessage { get; }
    }
}
