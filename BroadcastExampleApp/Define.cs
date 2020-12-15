using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BroadcastGlobal;
using BroadcastGlobal.Higher;

namespace BroadcastExampleApp
{
    public class Define
    {
        public const string Key1 = "key1";
        public const string Key2 = "key2";
        public const string Key3 = "key3";
        public const string Key4 = "key4";
    }
    public class MyDefineContent : BaseBroadContent
    {
        public string StrContent;
        public int IntContent;

        public override string ToString()
        {
            return $"StrContent:{StrContent}\nIntContent{IntContent}";
        }
    }

    public class MyDefineEvnet: SubEvent<string>
    {
        // my define
    }
}
