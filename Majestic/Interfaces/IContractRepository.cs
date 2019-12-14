using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Majestic.Models
{
    public interface IContractRepository
    {
        void SaveContract(string title);
        void Delete(int id);
        void Update(int id);
    }
}
