﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League.DL.Exceptions
{
    public class TransferRepoADOException : Exception
    {
        public TransferRepoADOException(string message) : base(message)
        {
        }

        public TransferRepoADOException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
