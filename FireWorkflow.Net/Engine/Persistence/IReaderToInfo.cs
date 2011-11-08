using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FireWorkflow.Net.Engine.Persistence
{
    //基类实现接口
    public interface IReaderToInfo
    {
        void ReaderToInfo(IDataReader dr);
    }
}
