using System;

namespace Project.Models
{
    /// <summary>
    /// Настройки
    /// </summary>
    [Serializable]
    public class AppSettings
    {
        public string DatabaseUrl { get; set; }
    }
}
