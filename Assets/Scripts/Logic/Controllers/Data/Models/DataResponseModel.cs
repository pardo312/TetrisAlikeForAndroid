using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiufenPackages.SceneFlow.Logic
{

    public class DataResponseModel
    {
        public bool Success;
        public string Message;
        public int Code;

        public DataResponseModel(bool success, string message, int code)
        {
            Success = success;
            Message = message;
            Code = code;
        }
    }
}
