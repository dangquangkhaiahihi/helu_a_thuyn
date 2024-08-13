using System;
using System.Collections.Generic;
using System.Text;

namespace CSMSBE.Core;

public class ResponseItem<T>
{
    public T Result { get; set; }
    public int? Err { get; set; }
    public ResponseErrorData ResponseError { get; set; }
}

public class ResponseFileItem<T>
{
    public T Result { get; set; }
    public Dictionary<long?, string> FileId{ get; set; }
    public ResponseErrorData ResponseError { get; set; }
}
