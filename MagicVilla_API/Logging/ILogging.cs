﻿using System;
namespace MagicVilla_API.Logging
{
    public interface ILogging
    {
        public void Log(string message, LogType type);
    }
}


