using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManagementApi.Enum
{
    public class ForgettingDates 
    {
        public DateTime currentDate()
        {
            return DateTime.Now;
        }

        public DateTime startDatefortheStatement()
        {
            DateTime startDate = new DateTime(currentDate().Year, currentDate().Month-1, currentDate().Day-1);

            return startDate;
        }

        public DateTime endDatefortheStatement()
        {
            DateTime endDate = currentDate().AddDays(-1);

            return endDate;
        }

        
    }
}
