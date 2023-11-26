using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Persistence
{
    /// <summary>
    /// (3 видео 0:10:00) Используется при старте приложения и проверяет создана ли база или нет. Если нет то будет создана на основе нашего контекста
    /// </summary>
    public class DbInitializer
    {
        public static void Initialize(NotesDbContext context) 
        {
            context.Database.EnsureCreated();
        }
    }
}
