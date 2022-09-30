using System;
using System.Data.Common;
using System.Linq;

namespace ISRORCert.Database
{
    internal interface IDbEntity
    {
        void Read(DbDataReader reader);
    }
}
