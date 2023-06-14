using DLC_3.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLC_3.Core
{     
    public class ViewModel() : ObservableObject
    {
        public static Server _server = new Server();
        public List<int> codeCommandList = new List<int>();        
    }
}
