using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos.HookInputDto
{
    public interface IRecordTsInput : IRecordMp4Input { }

    public class RecordTsInput : RecordMp4Input, IRecordTsInput { }
}
