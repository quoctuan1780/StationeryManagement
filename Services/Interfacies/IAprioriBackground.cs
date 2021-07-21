using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IAprioriBackground
    {
        Task RecommandationBackground(DateTime? fromDate = null, DateTime? toDate = null, double minsup = 0.15, double minconf = 0.81);
    }
}
