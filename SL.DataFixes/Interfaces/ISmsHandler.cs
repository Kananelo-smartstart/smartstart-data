using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.DataFixes.Interfaces
{
    /// <summary>
    /// Contract that defines the structure of an <see cref="ISms<Ds,E>"/>
    /// </summary>
    /// <typeparam name="Ds">Data Source</typeparam>
    /// <typeparam name="E">Table Object</typeparam>
    public interface ISmsHandler<Ds, E> where Ds : class where E : class
    {
        /// <summary>
        /// Creates an insteance of <see cref="E"/> for the given <see cref="Ds"/>
        /// </summary>
        /// <param name="orgService">Instance of <see cref="Ds"/></param>
        /// <param name="sms">Instance of <see cref="E"/></param>
        /// <returns><see cref="Guid"/> for the newly created SMS</returns>
        Guid CreateSms(Ds orgService, E sms);
    }
}
