﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreciseQ.AllMeds.Installer.Updater
{
    public class UpdateProgressEventArgs : EventArgs
    {
        public UpdateProgressEventArgs()
        {
            Status = UpdateProgressStatus.None;
        }

        public UpdateProgressEventArgs(string message) : this()
        {
            Message = message;
        }

        public UpdateProgressEventArgs(UpdateProgressStatus status, string message) : this(message)
        {
            Status = status;
        }

        public UpdateProgressStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum UpdateProgressStatus
    {
        None,
        Error,
        Completed
    }
}
